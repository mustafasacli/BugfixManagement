using System;
using System.Collections.Generic;
using System.Data;


namespace BugfixManagement.Data.Core.Interfaces
{
    public interface IConnection : IDisposable
    {
        IDbConnection CreateConnection();

        IDbConnection CreateConnection(string connectionString);

        void OpenConnection();

        void CloseConnection();

        ConnectionState GetConnectionState();

        void CommitTransaction();

        void RollbackTransaction();

        void CreateTransaction();

        IDbCommand CreateCommand();

        IDbDataParameter CreateParameter(IDbCommand dbCmd);

        IDbDataAdapter CreateAdapter(IDbCommand dbCmd);

        string ConnectionString { get; set; }

        DataSet GetResultSet(CommandType cmdType, string query, Dictionary<string, object> parameters = null);

        int Execute(CommandType cmdType, string sql, Dictionary<string, object> parameters = null);

        object ExecuteScalar(CommandType cmdType, string sql, Dictionary<string, object> parameters = null);
    }
}
