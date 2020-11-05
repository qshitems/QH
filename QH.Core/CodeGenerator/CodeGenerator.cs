
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Data;
using Dapper;
using System.Threading.Tasks;
using System.Data.SqlTypes;
using QH.Core.Options;
using QH.Core.Models;
using QH.Core.Extensions;
using QH.Core.DbHelper;

namespace QH.Core.CodeGenerator
{
    /// <summary>
    /// 代码生成器
    /// <remarks>
    /// 根据数据库表以及表对应的列生成对应的数据库实体
    /// </remarks>
    /// </summary>
    public class CodeGenerator
    {
        private readonly string Delimiter = "\\";//分隔符，默认为windows下的\\分隔符

        private static CodeGenerateOption _options;
        public CodeGenerator(IOptions<CodeGenerateOption> options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));
            _options = options.Value;
            if (_options.ConnectionString.IsNullOrWhiteSpace())
                throw new ArgumentNullException("不指定数据库连接串就生成代码，你想上天吗？");
            if (_options.DbType.IsNullOrWhiteSpace())
                throw new ArgumentNullException("不指定数据库类型就生成代码，你想逆天吗？");
            var path = AppDomain.CurrentDomain.BaseDirectory;
            if (_options.OutputPath.IsNullOrWhiteSpace())
                _options.OutputPath = path;
            var flag = path.IndexOf("/bin");
            if (flag > 0)
                Delimiter = "/";//如果可以取到值，修改分割符
        }

        /// <summary>
        /// 根据数据库连接字符串生成数据库表对应的模板代码
        /// </summary>
        /// <param name="isCoveredExsited">是否覆盖已存在的同名文件</param>
        public void GenerateTemplateCodesFromDatabase(bool isCoveredExsited = true)
        {
            DatabaseType dbType = ConnectionFactory.GetDataBaseType(_options.DbType);
            List<DbTable> tables = new List<DbTable>();
            using (var dbConnection = ConnectionFactory.CreateConnection(dbType, _options.ConnectionString))
            {
                tables = dbConnection.GetCurrentDatabaseTableList(dbType);
            }

            if (tables != null && tables.Any())
            {
                foreach (var table in tables)
                {
                    GenerateEntity(table, isCoveredExsited);
                    if (table.Columns.Any(c => c.IsPrimaryKey))
                    {
                        var pkTypeName = table.Columns.First(m => m.IsPrimaryKey).CSharpType;
                        pkTypeName = pkTypeName.ToLower() == "int32" ? "int" : pkTypeName;
                        pkTypeName = pkTypeName.ToLower() == "int64" ? "long" : pkTypeName;
                        pkTypeName = pkTypeName.ToLower() == "string" ? "string" : pkTypeName;
                        GenerateIRepository(table, pkTypeName, isCoveredExsited);
                        GenerateRepository(table, pkTypeName, isCoveredExsited);
                    }
                    GenerateIServices(table, isCoveredExsited);
                    GenerateServices(table, isCoveredExsited);

                }
            }
        }

        /// <summary>
        /// 生成实体代码
        /// </summary>
        /// <param name="table">表名</param>
        /// <param name="isCoveredExsited">是否覆盖</param>
        private void GenerateEntity(DbTable table, bool isCoveredExsited = true)
        {

            var pkTypeName = table.Columns.First(m => m.IsPrimaryKey).CSharpType;
            var sb = new StringBuilder();
            string _TableName = GetTableName(table);
            string _ModelName = GetModelName(table);
            foreach (var column in table.Columns)
            {
                var tmp = GenerateEntityProperty(_TableName, column);
                sb.AppendLine(tmp);
            }
            GenerateModelpath(table, out string path, out string pathP);
            var content = ReadTemplate("ModelTemplate.txt");
            content = content.Replace("{GeneratorTime}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
                .Replace("{ModelsNamespace}", _options.ModelsNamespace)
                .Replace("{Author}", _options.Author)
                .Replace("{Comment}", table.TableComment)
                .Replace("{ModelName}", _ModelName)
                .Replace("{ModelProperties}", sb.ToString())
                .Replace("{TableName}", $"[Table(\"{table.TableName}\")]");

            WriteAndSave(path, content);
            #region 新建一个部分类来添加一些扩展属性
            var contentP = ReadTemplate("ModelTemplate.txt");
            contentP = contentP.Replace("{GeneratorTime}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
                .Replace("{ModelsNamespace}", _options.ModelsNamespace)
                .Replace("{Author}", _options.Author)
                .Replace("{Comment}", table.TableComment)
                .Replace("{ModelName}", _ModelName)
                .Replace("{ModelProperties}", "")
                .Replace("{TableName}", "");
            WriteAndSave(pathP, contentP);
            #endregion
        }

        /// <summary>
        /// 生成IService层代码文件
        /// </summary>
        /// <param name="modelTypeName"></param>
        /// <param name="keyTypeName"></param>
        /// <param name="ifExsitedCovered"></param>
        private void GenerateIServices(DbTable table, bool ifExsitedCovered = true)
        {
            var iServicesPath = _options.OutputPath + Delimiter + "IServices";
            string _TableName = GetTableName(table);
            string _ModelName = GetModelName(table);
            if (!Directory.Exists(iServicesPath))
            {
                Directory.CreateDirectory(iServicesPath);
            }
            var fullPath = iServicesPath + Delimiter + "I" + _TableName + "Service.cs";
            if (File.Exists(fullPath) && !ifExsitedCovered)
                return;
            var content = ReadTemplate("IServicesTemplate.txt");
            content = content.Replace("{Comment}", table.TableComment)
                .Replace("{Author}", _options.Author)
                .Replace("{GeneratorTime}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
                .Replace("{IServicesNamespace}", _options.IServicesNamespace)
                .Replace("{ModelName}", _ModelName)
                .Replace("{TableName}", _TableName);
            WriteAndSave(fullPath, content);
        }

        private string GetTableName(DbTable table)
        {
            var _tnames = table.TableName.Split('_');
            var _TableName = "";
            for (int i = 0; i < _tnames.Length; i++)
            {
                if (i == 0 && _tnames[0].Length <= 3)
                    continue;
                _TableName += _tnames[i].FirstCharToUpper();

            }

            //   var _TableName = $"{table.TableName.Replace("ad_","").Replace("_", "")}";

            _TableName = _TableName.Substring(0, 1).ToUpper() + _TableName.Substring(1);
            return _TableName;
        }

        private string GetModelName(DbTable table)
        {
            return $"{GetTableName(table)}Entity";
        }

        /// <summary>
        /// 生成Services层代码文件
        /// </summary>
        /// <param name="modelTypeName"></param>
        /// <param name="keyTypeName"></param>
        /// <param name="ifExsitedCovered"></param>
        private void GenerateServices(DbTable table, bool ifExsitedCovered = true)
        {
            var repositoryPath = _options.OutputPath + Delimiter + "Services";
            string _TableName = GetTableName(table);
            string _ModelName = GetModelName(table);
            if (!Directory.Exists(repositoryPath))
            {
                Directory.CreateDirectory(repositoryPath);
            }
            var fullPath = repositoryPath + Delimiter + _TableName + "Service.cs";
            if (File.Exists(fullPath) && !ifExsitedCovered)
                return;
            var content = ReadTemplate("ServiceTemplate.txt");
            content = content.Replace("{Comment}", table.TableComment)
                .Replace("{Author}", _options.Author)
                .Replace("{GeneratorTime}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
                .Replace("{ServicesNamespace}", _options.ServicesNamespace)
                .Replace("{ModelName}", _ModelName)
                .Replace("{TableName}", _TableName);
            WriteAndSave(fullPath, content);
        }


        /// <summary>
        /// 生成IRepository层代码文件
        /// </summary>
        /// <param name="modelTypeName"></param>
        /// <param name="keyTypeName"></param>
        /// <param name="ifExsitedCovered"></param>
        private void GenerateIRepository(DbTable table, string keyTypeName, bool ifExsitedCovered = true)
        {
            var iRepositoryPath = _options.OutputPath + Delimiter + "IRepository";
            if (!Directory.Exists(iRepositoryPath))
            {
                Directory.CreateDirectory(iRepositoryPath);
            }
            string _TableName = GetTableName(table);
            string _ModelName = GetModelName(table);
            var fullPath = iRepositoryPath + Delimiter + "I" + _TableName + "Repository.cs";
            if (File.Exists(fullPath) && !ifExsitedCovered)
                return;
            var content = ReadTemplate("IRepositoryTemplate.txt");
            content = content.Replace("{Comment}", table.TableComment)
                .Replace("{Author}", _options.Author)
                .Replace("{GeneratorTime}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
                .Replace("{IRepositoryNamespace}", _options.IRepositoryNamespace)
                .Replace("{ModelName}", _ModelName)
                .Replace("{KeyTypeName}", keyTypeName)
                .Replace("{TableName}", _TableName);
            WriteAndSave(fullPath, content);
        }
        /// <summary>
        /// 生成Repository层代码文件
        /// </summary>
        /// <param name="modelTypeName"></param>
        /// <param name="keyTypeName"></param>
        /// <param name="ifExsitedCovered"></param>
        private void GenerateRepository(DbTable table, string keyTypeName, bool ifExsitedCovered = true)
        {
            var repositoryPath = _options.OutputPath + Delimiter + "Repository";
            if (!Directory.Exists(repositoryPath))
            {
                Directory.CreateDirectory(repositoryPath);
            }
            string _TableName = GetTableName(table);
            string _ModelName = GetModelName(table);
            var fullPath = repositoryPath + Delimiter + _TableName + "Repository.cs";

            var sb = new StringBuilder();
            foreach (var column in table.Columns)
            {
                var tmp = GenerateEntityProperty(_TableName, column);
                sb.AppendLine(tmp);
            }

            if (File.Exists(fullPath) && !ifExsitedCovered)
                return;
            var content = ReadTemplate("RepositoryTemplate.txt");
            content = content.Replace("{Comment}", table.TableComment)
                .Replace("{Author}", _options.Author)
                .Replace("{GeneratorTime}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
                .Replace("{RepositoryNamespace}", _options.RepositoryNamespace)
                .Replace("{ModelName}", _ModelName)
                .Replace("{KeyTypeName}", keyTypeName)
                .Replace("{TableName}", _TableName)
                .Replace("{UpdateByTableName}", GenerateUpdate(table));
            WriteAndSave(fullPath, content);
        }


        /// <summary>
        /// 生成属性
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="column">列</param>
        /// <returns></returns>
        private static string GenerateEntityProperty(string tableName, DbTableColumn column)
        {
            var sb = new StringBuilder();
            if (!string.IsNullOrEmpty(column.Comment))
            {
                sb.AppendLine("\t\t/// <summary>");
                sb.AppendLine("\t\t/// " + column.Comment);
                sb.AppendLine("\t\t/// </summary>");
            }
            if (column.IsPrimaryKey)
            {
                sb.AppendLine("\t\t[Key]");
                //if (column.IsIdentity)
                //{
                //    sb.AppendLine("\t\t[DatabaseGenerated(DatabaseGeneratedOption.Identity)]");
                //}
                var colType = column.CSharpType;
                colType = colType.ToLower() == "int32" ? "int" : colType;
                colType = colType.ToLower() == "int64" ? "long" : colType;
                colType = colType.ToLower() == "string" ? "string" : colType;
                if (colType.ToLower() == "string")
                {
                    sb.AppendLine($"\t\t[DisplayFormat(ConvertEmptyStringToNull = false)]");
                }
                sb.AppendLine($"\t\tpublic {colType} Id " + "{get;set;}");
            }
            else
            {
                if (!column.IsNullable)
                {
                    sb.AppendLine("\t\t[Required]");
                }

                if (column.ColumnLength.HasValue && column.ColumnLength.Value > 0)
                {
                    sb.AppendLine($"\t\t[MaxLength({column.ColumnLength.Value})]");
                }
                //if (column.IsIdentity)
                //{
                //    sb.AppendLine("\t\t[DatabaseGenerated(DatabaseGeneratedOption.Identity)]");
                //}

                var colType = column.CSharpType;
                colType = colType.ToLower() == "int32" ? "int" : colType;
                colType = colType.ToLower() == "int64" ? "long" : colType;
                colType = colType.ToLower() == "string" ? "string" : colType;
                colType = colType.ToLower() == "boolean" ? "bool" : colType;
                if (colType.ToLower() == "string")
                {
                    sb.AppendLine($"\t\t[DisplayFormat(ConvertEmptyStringToNull = false)]");
                }
                // if (colType.ToLower() != "string" && colType.ToLower() != "byte[]" && colType.ToLower() != "object" &&
                // column.IsNullable)
                if (colType.ToLower() != "string" && colType.ToLower() != "byte[]" && colType.ToLower() != "object")
                {
                    colType = colType + "?";
                }
                sb.AppendLine($"\t\tpublic {colType} {column.ColName} " + "{get;set;}");
            }

            return sb.ToString();
        }

        /// <summary>
        /// 生成update
        /// </summary>
        /// <param name="table">表</param>
        /// <returns></returns>
        private static string GenerateUpdate(DbTable table)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"   var sql = \"update {table.TableName} set \";");
            var _id = string.Empty;
            foreach (var column in table.Columns)
            {
                if (column.IsPrimaryKey)
                {
                    _id = column.ColName;
                    continue;
                }
                sb.AppendLine($"\t\t   if (model.{ column.ColName} != null) sql += \" { column.ColName} = @{ column.ColName},\";");
            }
            sb.AppendLine($"\t\t   sql = sql.Trim().TrimEnd(',');");
            sb.AppendLine($"\t\t   sql += \" where {_id} = @{_id}\";");
            return sb.ToString();
        }


        /// <summary>
        /// 生成insert
        /// </summary>
        /// <param name="table">表</param>
        /// <returns></returns>
        private static string GenerateInsert(DbTable table)
        {
            string str = "", fields="", _field="";
            foreach (var column in table.Columns)
            {
                if (column.IsPrimaryKey)
                {
                    continue;
                }
                fields += $"{column.ColName},";
                _field = $"@{column.ColName},";
            }
            fields = fields.TrimEnd(',');
            _field = fields.TrimEnd(',');
            str =$" var sql = \" insert into  {table.TableName} ({fields})  values ({_field}) \";";
            return  str;
        }


        /// <summary>
        /// 根据表格信息生成实体路径
        /// </summary>
        /// <param name="table">表信息</param>
        /// <param name="path">实体路径</param>
        /// <param name="pathP">部分类路径</param>
        private void GenerateModelpath(DbTable table, out string path, out string pathP)
        {
            var modelPath = _options.OutputPath + Delimiter + "Models"; ;
            if (!Directory.Exists(modelPath))
            {
                Directory.CreateDirectory(modelPath);
            }

            StringBuilder fullPath = new StringBuilder();
            fullPath.Append(modelPath);
            fullPath.Append(Delimiter);
            fullPath.Append("Partial");
            if (!Directory.Exists(fullPath.ToString()))
            {
                Directory.CreateDirectory(fullPath.ToString());
            }
            fullPath.Append(Delimiter);
            fullPath.Append($"{GetModelName(table)}");
            fullPath.Append(".cs");
            pathP = fullPath.ToString();
            path = fullPath.Replace("Partial" + Delimiter, "").ToString();

        }

        /// <summary>
        /// 从代码模板中读取内容
        /// </summary>
        /// <param name="templateName">模板名称，应包括文件扩展名称。比如：template.txt</param>
        /// <returns></returns>
        private string ReadTemplate(string templateName)
        {
            var currentAssembly = Assembly.GetExecutingAssembly();
            var content = string.Empty;



            using (var stream = currentAssembly.GetManifestResourceStream($"{currentAssembly.GetName().Name}.CodeTemplate.{templateName}"))
            {
                if (stream != null)
                {
                    using (var reader = new StreamReader(stream))
                    {
                        content = reader.ReadToEnd();
                    }
                }
            }
            return content;
        }

        /// <summary>
        /// 写文件
        /// </summary>
        /// <param name="fileName">文件完整路径</param>
        /// <param name="content">内容</param>
        private static void WriteAndSave(string fileName, string content)
        {
            //实例化一个文件流--->与写入文件相关联
            using (var fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            {
                //实例化一个StreamWriter-->与fs相关联
                using (var sw = new StreamWriter(fs))
                {
                    //开始写入
                    sw.Write(content);
                    //清空缓冲区
                    sw.Flush();
                    //关闭流
                    sw.Close();
                    fs.Close();
                }
            }
        }

    }
}
