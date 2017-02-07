using System.Collections.Generic;
using System.Data;


namespace BugfixManagement.Data.Core.Interfaces
{
    public interface IDataManager
    {
        string ConnectionString { get; set; }

        DataSet GetResultSetOfQuery(string query);

        DataSet GetResultSetOfQuery(string query,  Dictionary<string, object> parameters = null);

        DataSet GetResultSetOfProcedure(string procedure);

        int ExecuteQuery(string query,  Dictionary<string, object> parameters = null);

        int ExecuteProcedure(string query,  Dictionary<string, object> parameters = null);
    }
}
