using BugfixManagement.Data.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;

namespace BugfixManagement.Data.Factory
{
    internal class DataManager : IDataManager
    {
        private string _ConnStr;

        public DataManager()
            : this(string.Empty)
        { }

        public DataManager(string connectionString)
        {
            _ConnStr = connectionString;
        }

        public string ConnectionString
        {
            get { return _ConnStr; }
            set { _ConnStr = value; }
        }

        public int ExecuteQuery(string query, Dictionary<string, object> parameters = null)
        {
            try
            {
                int result = -1;

                using (IConnection conn = new Connection(_ConnStr))
                {
                    result = conn.Execute(CommandType.Text, query, parameters);
                }

                return result;
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        public int ExecuteProcedure(string query, Dictionary<string, object> parameters = null)
        {
            try
            {
                int result = -1;

                using (IConnection conn = new Connection(_ConnStr))
                {
                    result = conn.Execute(CommandType.StoredProcedure, query, parameters);
                }

                return result;
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        public DataSet GetResultSetOfQuery(string query)
        {
            try
            {
                DataSet ds = null;
                using (IConnection conn = new Connection(_ConnStr))
                {
                    ds = conn.GetResultSet(CommandType.Text, query);
                }
                return ds;
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        public DataSet GetResultSetOfQuery(string query, Dictionary<string, object> parameters = null)
        {
            try
            {
                DataSet ds = null;
                using (IConnection conn = new Connection(_ConnStr))
                {
                    ds = conn.GetResultSet(CommandType.Text, query, parameters);
                }
                return ds;
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        public DataSet GetResultSetOfProcedure(string procedure)
        {
            try
            {
                DataSet ds = null;
                using (IConnection conn = new Connection(_ConnStr))
                {
                    ds = conn.GetResultSet(CommandType.StoredProcedure, procedure);
                }
                return ds;
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        public int Insert(string tableName, Dictionary<string, object> parameters = null)
        {
            int result = -1;

            try
            {
                if (string.IsNullOrWhiteSpace(tableName))
                {
                    throw new Exception("Table Name can not be null or empty.");
                }

                if (parameters == null)
                {
                    throw new Exception("Parameter List can not be null.");
                }

                if (parameters.Count == 0)
                {
                    throw new Exception("Parameter List can not be empty.");
                }

                string sql = "INSERT INTO #TABLE_NAME#(#COLUMNS#) VALUES(#VALS#);";
                sql = sql.Replace("#TABLE_NAME#", tableName);
                string cols = "";
                string vals = "";

                foreach (var item in parameters.Keys)
                {
                    if (item != null)
                    {
                        if (!(string.IsNullOrWhiteSpace(item) | "Id".Equals(item)))
                        {
                            cols = string.Format("{0}{1}, ", cols, item);
                            vals += "?, ";
                        }
                    }
                }

                cols = cols.TrimEnd().TrimEnd(',');
                vals = vals.TrimEnd().TrimEnd(',');

                sql = sql.Replace("#COLUMNS#", cols);
                sql = sql.Replace("#VALS#", vals);

                result = ExecuteQuery(sql, parameters);
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        public int Update(string tableName, Dictionary<string, object> parameters = null)
        {
            int result = -1;

            try
            {
                if (string.IsNullOrWhiteSpace(tableName))
                {
                    throw new Exception("Table Name can not be null or empty.");
                }

                if (parameters == null)
                {
                    throw new Exception("Parameter List can not be null.");
                }

                if (parameters.Count == 0)
                {
                    throw new Exception("Parameter List can not be empty.");
                }

                string sql = "UPDATE #TABLE_NAME# SET #COLUMNS# WHERE Id=?;";
                sql = sql.Replace("#TABLE_NAME#", tableName);
                string cols = "";
                Dictionary<string, object> keyValColl = new Dictionary<string, object>();
                object objid = 0;
                foreach (var item in parameters.Keys)
                {
                    if (item != null)
                    {
                        if ("Id".Equals(item))
                        {
                            objid = parameters[item];
                        }
                        if (!(string.IsNullOrWhiteSpace(item) | "Id".Equals(item)))
                        {
                            cols = string.Format("{0}{1}=?, ", cols, item);
                            keyValColl[item] = parameters[item];
                        }
                    }
                }

                cols = cols.TrimEnd().TrimEnd(',');

                sql = sql.Replace("#COLUMNS#", cols);

                keyValColl["Id"] = objid;

                result = ExecuteQuery(sql, parameters);
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        public int Delete(string tableName, int pid)
        {
            int result = -1;
            try
            {
                if (string.IsNullOrWhiteSpace(tableName))
                {
                    throw new Exception("Table Name can not be null or empty.");
                }

                if (pid > 0)
                {
                    Dictionary<string, object> parameters = new Dictionary<string, object>();
                    parameters["Id"] = pid;

                    string sql = "DELETE FROM #TABLE_NAME# WHERE Id=?";
                    sql = sql.Replace("#TABLE_NAME#", tableName);
                    result = ExecuteQuery(sql, parameters);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }
    }
}
