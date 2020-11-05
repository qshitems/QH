
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using NLog.Fluent;
using QH.Core.Auth;
using QH.Core.Extensions;
using QH.Core.Helpers;
using QH.Core.Options;
using QH.IRepository;

namespace QH.Repository
{
    public abstract class BaseRepository<T, TKey> : IBaseRepository<T, TKey> where T : class
    {

        public readonly IUser _user;
        public BaseRepository()
        {

        }

        public BaseRepository(IUser user)
        {
            _user = user;
        }

        #region Field
        /// <summary>
        /// 事务数据库连接对象
        /// </summary>
        private IDbConnection tranConnection;
        #endregion

        #region Property
        /// <summary>
        /// 超时时长，默认240s
        /// </summary>
        public int CommandTimeout { get; set; } = 240;

        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// 数据库连接对象
        /// </summary>
        public IDbConnection Connection
        {
            get
            {

                if (ConnectionString.IsNull())
                {
                    //  ConnectionString = Appsettings.GetSection("DbOption").Get<DbOption>().ConnectionString;
                    try
                    {
                        ConnectionString = Appsettings.App("DbOption:ConnectionString");
                        if (ConnectionString.IsNull())
                            ConnectionString = "Data Source=.;Initial Catalog=admindb;User ID=sa;Password=000000;Persist Security Info=True;Max Pool Size=50;Min Pool Size=0;Connection Lifetime=300;";
                    }
                    catch (Exception)
                    {

                        ConnectionString = "Data Source=.;Initial Catalog=admindb;User ID=sa;Password=000000;Persist Security Info=True;Max Pool Size=50;Min Pool Size=0;Connection Lifetime=300;";
                    }

                    //  var uuuuii = Appsettings.App<DbOption>("DbOption");
                    //var uuu = Appsettings.GetAppSettings<DbOption>("DbOption");
                    //ConnectionString = uuu.ConnectionString;
                }
                var connection = new SqlConnection(ConnectionString);
                if (connection.State != ConnectionState.Open)
                    connection.Open();
                return connection;
            }
        }

        /// <summary>
        /// 事务对象
        /// </summary>
        public IDbTransaction Transaction { get; set; }

        #endregion

        #region Transaction
        /// <summary>
        /// 开启事务
        /// </summary>
        /// <returns>IRepository</returns>
        public IBaseRepository<T, TKey> BeginTrans()
        {
            tranConnection = Connection;
            Transaction = tranConnection.BeginTransaction();
            return this;
        }

        /// <summary>
        /// 提交事务
        /// </summary>
        public void Commit()
        {
            Transaction?.Commit();
            Transaction?.Dispose();
            Close();
        }

        /// <summary>
        /// 回滚事务
        /// </summary>
        public void Rollback()
        {
            Transaction?.Rollback();
            Transaction?.Dispose();
            Close();
        }

        /// <summary>
        /// 关闭连接
        /// </summary>
        public void Close()
        {
            tranConnection?.Close();
            tranConnection?.Dispose();
            Transaction = null;
        }
        #endregion

        #region 同步
        //public T Get(TKey id) => _dbConnection.Get<T>(id);
        //public T Get(string conditions, object parameters = null) => _dbConnection.QueryFirstOrDefault<T>(conditions, parameters);
        //public IEnumerable<T> GetList() => _dbConnection.GetList<T>();

        //public IEnumerable<T> GetList(object whereConditions) => _dbConnection.GetList<T>(whereConditions);

        //public IEnumerable<T> GetList(string conditions, object parameters = null) => _dbConnection.GetList<T>(conditions, parameters);

        //public IEnumerable<T> GetListPaged(int pageNumber, int rowsPerPage, string conditions, string orderby, object parameters = null) => _dbConnection.GetListPaged<T>(pageNumber, rowsPerPage, conditions, orderby, parameters);

        //public int? Insert(T entity) => _dbConnection.Insert(entity);
        //public int Update(T entity) => _dbConnection.Update(entity);
        //public int Delete(TKey id) => _dbConnection.Delete<T>(id);

        //public int Delete(T entity) => _dbConnection.Delete(entity);
        //public int DeleteList(object whereConditions, IDbTransaction transaction = null, int? commandTimeout = null) =>
        //     _dbConnection.DeleteList<T>(whereConditions, transaction, commandTimeout);

        //public int DeleteList(string conditions, object parameters = null, IDbTransaction transaction = null, int? commandTimeout = null)
        //{
        //    return _dbConnection.DeleteList<T>(conditions, parameters, transaction, commandTimeout);
        //}
        //public int RecordCount(string conditions = "", object parameters = null)
        //{
        //    return _dbConnection.RecordCount<T>(conditions, parameters);
        //}



        #endregion



        #region 异步


        public async Task<T> GetAsync(TKey id)
        {

            if (Transaction?.Connection != null)
            {
                return await Transaction.Connection.GetAsync<T>(id, Transaction, commandTimeout: CommandTimeout).ConfigureAwait(false);
            }
            else
            {
                using var _dbConnection = Connection;
                return await _dbConnection.GetAsync<T>(id).ConfigureAwait(false);
            }
        }

        public async Task<T> GetAsync(object whereConditions)
        {

            if (Transaction?.Connection != null)
            {
                return await Transaction.Connection.FindAsync<T>(whereConditions, Transaction, commandTimeout: CommandTimeout).ConfigureAwait(false);
            }
            else
            {
                using var _dbConnection = Connection;
                return await _dbConnection.FindAsync<T>(whereConditions).ConfigureAwait(false);
            }
        }
        public async Task<T> GetAsync(string conditions, object parameters = null)
        {
            if (Transaction?.Connection != null)
            {
                return await Transaction.Connection.GetAsync<T>(conditions, parameters, Transaction, commandTimeout: CommandTimeout).ConfigureAwait(false);
            }
            else
            {
                using var _dbConnection = Connection;
                return await _dbConnection.GetAsync<T>(conditions, parameters).ConfigureAwait(false);
            }
        }

        public async Task<IEnumerable<T>> GetListAsync()
        {
            if (Transaction?.Connection != null)
            {
                return await Transaction.Connection.GetListAsync<T>(Transaction, commandTimeout: CommandTimeout).ConfigureAwait(false);
            }
            else
            {
                using var _dbConnection = Connection;
                return await _dbConnection.GetListAsync<T>().ConfigureAwait(false);
            }
        }

        public async Task<IEnumerable<T>> GetListAsync(object whereConditions)
        {
            if (Transaction?.Connection != null)
            {
                return await Transaction.Connection.GetListAsync<T>(whereConditions, Transaction, commandTimeout: CommandTimeout).ConfigureAwait(false);
            }
            else
            {
                using var _dbConnection = Connection;
                return await _dbConnection.GetListAsync<T>(whereConditions, null, commandTimeout: CommandTimeout).ConfigureAwait(false);
            }
        }

        public async Task<IEnumerable<T>> GetListAsync(string conditions, object parameters = null)
        {
            if (Transaction?.Connection != null)
            {
                return await Transaction.Connection.GetListAsync<T>(conditions, parameters, Transaction, commandTimeout: CommandTimeout).ConfigureAwait(false);
            }
            else
            {
                using var _dbConnection = Connection;
                return await _dbConnection.GetListAsync<T>(conditions, parameters, null, commandTimeout: CommandTimeout).ConfigureAwait(false);
            }
        }
        public async Task<IEnumerable<T>> GetListPagedAsync(int pageNumber, int rowsPerPage, string conditions, string orderby = "id desc", object parameters = null)
        {
            if (Transaction?.Connection != null)
            {
                return await Transaction.Connection.GetListPagedAsync<T>(pageNumber, rowsPerPage, conditions, orderby, parameters, Transaction, commandTimeout: CommandTimeout).ConfigureAwait(false);
            }
            else
            {
                using var _dbConnection = Connection;
                return await _dbConnection.GetListPagedAsync<T>(pageNumber, rowsPerPage, conditions, orderby, parameters, null, commandTimeout: CommandTimeout).ConfigureAwait(false);
            }
        }
        public async Task<int?> InsertAsync(T entity)
        {
            if (Transaction?.Connection != null)
            {
                return await Transaction.Connection.InsertAsync(entity, Transaction, commandTimeout: CommandTimeout).ConfigureAwait(false);
            }
            else
            {
                using var _dbConnection = Connection;
                return await _dbConnection.InsertAsync(entity, Transaction, commandTimeout: CommandTimeout).ConfigureAwait(false);
            }
        }

        public async Task<int?> InsertAsync(List<T> entity)
        {
            if (Transaction?.Connection != null)
            {
                return await Transaction.Connection.InsertAsync(entity, Transaction, commandTimeout: CommandTimeout).ConfigureAwait(false);
            }
            else
            {
                using var _dbConnection = Connection;
                return await _dbConnection.InsertAsync(entity).ConfigureAwait(false);
            }
        }
        public async Task<int> UpdateAsync(T entity)
        {
            if (Transaction?.Connection != null)
            {
                return await Transaction.Connection.UpdateAsync(entity, Transaction, commandTimeout: CommandTimeout).ConfigureAwait(false);
            }
            else
            {
                using var _dbConnection = Connection;
                return await _dbConnection.UpdateAsync(entity).ConfigureAwait(false);
            }
        }

        public async Task<int> UpdateAsync(List<T> entity)
        {
            if (Transaction?.Connection != null)
            {
                return await Transaction.Connection.UpdateAsync(entity, Transaction, commandTimeout: CommandTimeout).ConfigureAwait(false);
            }
            else
            {
                using var _dbConnection = Connection;
                return await _dbConnection.UpdateAsync(entity).ConfigureAwait(false);
            }
        }
        public async Task<int> DeleteAsync(TKey id)
        {
            if (Transaction?.Connection != null)
            {
                return await Transaction.Connection.DeleteAsync(id, Transaction, commandTimeout: CommandTimeout).ConfigureAwait(false);
            }
            else
            {
                using var _dbConnection = Connection;
                return await _dbConnection.DeleteAsync<T>(id).ConfigureAwait(false);
            }
        }

        public async Task<int> DeleteAsync(T entity)
        {
            if (Transaction?.Connection != null)
            {
                return await Transaction.Connection.UpdateAsync(entity, Transaction, commandTimeout: CommandTimeout).ConfigureAwait(false);
            }
            else
            {
                using var _dbConnection = Connection;
                return await _dbConnection.DeleteAsync<T>(entity).ConfigureAwait(false);
            }
        }


        public async Task<int> DeleteListAsync(object whereConditions)
        {
            if (Transaction?.Connection != null)
            {
                return await Transaction.Connection.DeleteListAsync<T>(whereConditions, Transaction, commandTimeout: CommandTimeout).ConfigureAwait(false);
            }
            else
            {
                using var _dbConnection = Connection;
                return await _dbConnection.DeleteListAsync<T>(whereConditions).ConfigureAwait(false);
            }
        }


        public async Task<int> DeleteListAsync(string conditions, object parameters = null)
        {
            if (Transaction?.Connection != null)
            {
                return await Transaction.Connection.DeleteListAsync<T>(conditions, parameters, Transaction, commandTimeout: CommandTimeout).ConfigureAwait(false);
            }
            else
            {
                using var _dbConnection = Connection;
                return await _dbConnection.DeleteListAsync<T>(conditions, parameters).ConfigureAwait(false);
            }
        }

        public async Task<int> RecordCountAsync(string conditions = "", object parameters = null)
        {
            if (Transaction?.Connection != null)
            {
                return await Transaction.Connection.RecordCountAsync<T>(conditions, parameters, Transaction, commandTimeout: CommandTimeout).ConfigureAwait(false);
            }
            else
            {
                using var _dbConnection = Connection;
                return await _dbConnection.RecordCountAsync<T>(conditions, parameters).ConfigureAwait(false);
            }
        }
        public async Task<bool> SoftDeleteAsync(TKey id, IUser _user)
        {
            var currenttype = typeof(T);
            var tableName = currenttype.Name;
            var tableattr = currenttype.GetCustomAttributes(true).SingleOrDefault(attr => attr.GetType().Name == typeof(TableAttribute).Name) as dynamic;
            if (tableattr != null)
            {
                tableName = tableattr.Name;
                if (!string.IsNullOrEmpty(tableattr.Schema))
                {
                    string schemaName = tableattr.Schema;
                    tableName = string.Format("{0}.{1}", schemaName, tableName);
                }
            }
            IEnumerable<PropertyInfo> props = typeof(T).GetProperties();
            string sql = $"update {tableName} set IsDeleted=@IsDeleted,ModifiedUserId=@ModifiedUserId,ModifiedUserName=@ModifiedUserName ,ModifiedTime=@ModifiedTime where Id = @Id";
            var parameters = new
            {
                IsDeleted = 1,
                ModifiedUserId = _user.Id,
                ModifiedUserName = _user.Name,
                id,
                ModifiedTime = DateTime.UtcNow
            };
            if (Transaction?.Connection != null)
            {
                return await Transaction.Connection.ExecuteAsync(sql, parameters, Transaction, commandTimeout: CommandTimeout).ConfigureAwait(false) > 0;
            }
            else
            {
                using var _dbConnection = Connection;
                return await _dbConnection.ExecuteAsync(sql, parameters).ConfigureAwait(false) > 0;
            }
        }

        public async Task<bool> SoftDeleteAsync(int[] ids, IUser _user)
        {
            var currenttype = typeof(T);
            var tableName = currenttype.Name;
            var tableattr = currenttype.GetCustomAttributes(true).SingleOrDefault(attr => attr.GetType().Name == typeof(TableAttribute).Name) as dynamic;
            if (tableattr != null)
            {
                tableName = tableattr.Name;
                if (!string.IsNullOrEmpty(tableattr.Schema))
                {
                    string schemaName = tableattr.Schema;
                    tableName = string.Format("{0}.{1}", schemaName, tableName);
                }
            }
            IEnumerable<PropertyInfo> props = typeof(T).GetProperties();
            string sql = $"update {tableName} set IsDeleted=@IsDeleted,ModifiedUserId=@ModifiedUserId,ModifiedUserName=@ModifiedUserName ,ModifiedTime=@ModifiedTime where Id in @Id";
            var parameters = new
            {
                IsDeleted = 1,
                ModifiedUserId = _user.Id,
                ModifiedUserName = _user.Name,
                ids,
                ModifiedTime = DateTime.UtcNow
            };
            if (Transaction?.Connection != null)
            {
                return await Transaction.Connection.ExecuteAsync(sql, parameters, Transaction, commandTimeout: CommandTimeout).ConfigureAwait(false) > 0;
            }
            else
            {
                using var _dbConnection = Connection;
                return await _dbConnection.ExecuteAsync(sql, parameters).ConfigureAwait(false) > 0;
            }
        }



        #endregion


        #region IDisposable Support


        public void Dispose()
        {
            // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。

            Close();
            // TODO: 如果在以上内容中替代了终结器，则取消注释以下行。
            // GC.SuppressFinalize(this);
        }

        #endregion
    }


    public abstract class BaseRepository<TEntity> : BaseRepository<TEntity, int> where TEntity : class, new()
    {
        protected BaseRepository()
        {
        }

        protected BaseRepository(IUser user) : base(user)
        {

        }
    }
}
