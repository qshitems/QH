
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using QH.Core.Extensions;
using System.Data.SqlClient;
using QH.Core.Models;
using QH.Core.Options;
using QH.Core.Helpers;
using Microsoft.Extensions.Configuration;

namespace QH.Core.DbHelper
{
    /// <summary>
    /// 数据库连接工厂类
    /// </summary>
    public class ConnectionFactory
    {
        /// <summary>
        /// 获取数据库连接
        /// </summary>
        /// <param name="dbtype">数据库类型</param>
        /// <param name="conStr">数据库连接字符串</param>
        /// <returns>数据库连接</returns>
        public static IDbConnection CreateConnection( string strConn, DatabaseType dbType= DatabaseType.SqlServer)
        {
            return CreateConnection(dbType, strConn);
        }

        /// <summary>
        /// 获取数据库连接
        /// </summary>
        /// <param name="dbtype">数据库类型</param>
        /// <param name="conStr">数据库连接字符串</param>
        /// <returns>数据库连接</returns>
        public static IDbConnection CreateConnection(string dbtype, string strConn)
        {
            if (dbtype.IsNullOrWhiteSpace())
                throw new ArgumentNullException("获取数据库连接居然不传数据库类型，你想上天吗？");
            if (strConn.IsNullOrWhiteSpace())
                throw new ArgumentNullException("获取数据库连接居然不传数据库类型，你想上天吗？");
            var dbType = GetDataBaseType(dbtype);
            return CreateConnection(dbType,strConn);
        }
        /// <summary>
        /// 获取数据库连接
        /// </summary>
        /// <param name="dbtype">数据库类型</param>
        /// <param name="ConnectionString">数据库连接字符串</param>
        /// <returns>数据库连接</returns>
        //public static IDbConnection CreateConnection()
        //{ 
        //    return CreateConnection(Appsettings.GetSection("DbOption").Get<DbOption>());
        //}

        /// <summary>
        /// 获取数据库连接
        /// </summary>
        /// <param name="dbtype">数据库类型</param>
        /// <param name="ConnectionString">数据库连接字符串</param>
        /// <returns>数据库连接</returns>
        public static IDbConnection CreateConnection(DbOption dbOption)
        {
            if (dbOption.DbType.IsNullOrWhiteSpace())
                throw new ArgumentNullException("获取数据库连接居然不传数据库类型，你想上天吗？");
            if (dbOption.ConnectionString.IsNullOrWhiteSpace())
                throw new ArgumentNullException("获取数据库连接居然不传数据库类型，你想上天吗？");
            Appsettings.GetSection("DbOption").Get<DbOption>();
            return CreateConnection(dbOption.DbType, dbOption.ConnectionString);
        }

        /// <summary>
        /// 获取数据库连接
        /// </summary>
        /// <param name="dbType">数据库类型</param>
        /// <param name="conStr">数据库连接字符串</param>
        /// <returns>数据库连接</returns>
        public static IDbConnection CreateConnection(DatabaseType dbType, string strConn)
        {
            IDbConnection connection = null;           
            if (strConn.IsNullOrWhiteSpace())
                throw new ArgumentNullException("获取数据库连接居然不传数据库类型，你想上天吗？");
            
            switch (dbType)
            {
                case DatabaseType.SqlServer:
                    connection = new SqlConnection(strConn);
                    break;
                case DatabaseType.MySQL:
                  //  connection = new MySqlConnection(strConn);
                    break;
                case DatabaseType.PostgreSQL:
                    //connection = new NpgsqlConnection(strConn);
                    break;
                default:
                    throw new ArgumentNullException($"这是我的错，还不支持的{dbType.ToString()}数据库类型");

            }
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }
            return connection;
        }

        /// <summary>
        /// 转换数据库类型
        /// </summary>
        /// <param name="dbtype">数据库类型字符串</param>
        /// <returns>数据库类型</returns>
        public static DatabaseType GetDataBaseType(string dbtype)
        {
            if (dbtype.IsNullOrWhiteSpace())
                throw new ArgumentNullException("获取数据库连接居然不传数据库类型，你想上天吗？");
            DatabaseType returnValue = DatabaseType.SqlServer;
            foreach (DatabaseType dbType in Enum.GetValues(typeof(DatabaseType)))
            {
                if (dbType.ToString().Equals(dbtype, StringComparison.OrdinalIgnoreCase))
                {
                    returnValue = dbType;
                    break;
                }
            }
            return returnValue;
        }
        /// <summary>
        /// 返回分页的字符串
        /// </summary>
        /// <param name="table">要分页的表，可以为复合查询(必填)</param>
        /// <param name="columnName">要获取的字段(必填)</param>
        /// <param name="sortName">分页时需要排序的字段(必填)</param>
        /// <param name="bln">true 降序（desc）</param>
        /// <param name="where">查询条件</param>
        /// <param name="page">当前页码(必填)</param>
        /// <param name="pageSize">页面显示条数(必填)</param>
        /// <returns>分页的字符串</returns>
        public static string GetTableByPager(string table, string columnName, string sortName, bool bln, string where, int page, int pageSize)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("select ");
            sb.Append(columnName);
            sb.Append(" from (select *,ROW_NUMBER() OVER (order by ");
            sb.Append(sortName);
            sb.Append(bln ? " desc" : " asc");
            sb.Append(") as pagerID from ");
            if (table.Contains("select"))
            {
                sb.Append("(");
                sb.Append(table);
                if (!string.IsNullOrEmpty(where))
                {
                    sb.Append(" where 1=1 ");
                    sb.Append(where);
                }
                sb.Append(") temptemp");
            }
            else
            {
                sb.Append(table);
                if (!string.IsNullOrEmpty(where))
                {
                    sb.Append(" where 1=1 ");
                    sb.Append(where);
                }
            }
            int start = (page - 1) * pageSize + 1;
            int end = start + pageSize;
            sb.Append(" ) temp where pagerID>=");
            sb.Append(start);
            sb.Append(" and pagerID<");
            sb.Append(end);
            return sb.ToString();
        }

    }
}
