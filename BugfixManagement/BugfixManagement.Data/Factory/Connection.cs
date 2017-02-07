using BugfixManagement.Data.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;

namespace BugfixManagement.Data.Factory
{
    internal class Connection : IConnection
    {
        private IDbConnection dbConn = null;
        private IDbTransaction dbTrans = null;
        private string _Connstr;

        public Connection(string connectionString)
        {
            _Connstr = connectionString;
        }

        public string ConnectionString
        {
            get
            {
                return _Connstr;
            }
            set
            {
                _Connstr = value;
                dbConn.ConnectionString = _Connstr;
            }
        }

        public IDbConnection CreateConnection()
        {
            return new OleDbConnection();
            // İstenirse SqlConnection yapılabilir ya da enum ile birden fazla seçenek sunulabilir.
        }

        public IDbConnection CreateConnection(string connectionString)
        {
            return new OleDbConnection(connectionString);
            // İstenirse SqlConnection yapılabilir ya da enum ile birden fazla seçenek sunulabilir.
        }

        public void OpenConnection()
        {
            try
            {
                if (dbConn.State != ConnectionState.Open)
                    dbConn.Open();
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        public void CloseConnection()
        {
            try
            {
                if (dbConn.State != ConnectionState.Closed)
                    dbConn.Close();
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        public void CreateTransaction()
        {
            try
            {
                if (dbConn.State != ConnectionState.Open)
                {
                    dbConn.Open();
                }

                dbTrans = dbConn.BeginTransaction();
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        public ConnectionState GetConnectionState()
        {
            return dbConn.State;
        }

        public void CommitTransaction()
        {
            try
            {
                dbTrans.Commit();
            }
            catch (Exception exc)
            {
                throw exc;
            }
            finally
            {
                dbTrans.Dispose();
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                dbTrans.Rollback();
            }
            catch (Exception exc)
            {
                throw exc;
            }
            finally
            {
                dbTrans.Dispose();
            }
        }

        public IDbCommand CreateCommand()
        {
            try
            {
                return dbConn.CreateCommand();
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        public IDbDataParameter CreateParameter(IDbCommand dbCmd)
        {
            try
            {
                return dbCmd.CreateParameter();
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        public IDbDataAdapter CreateAdapter(IDbCommand dbCmd)
        {
            try
            {
                return new OleDbDataAdapter((OleDbCommand)dbCmd);
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        public void Dispose()
        {
            if (dbConn != null)
            {
                if (dbConn.State == ConnectionState.Open)
                    dbConn.Close();

                dbConn.Dispose();
            }
        }

        public DataSet GetResultSet(CommandType cmdType, string query)
        {
            try
            {
                dbConn = this.CreateConnection(_Connstr);
                CreateTransaction();

                DataSet dataSet = new DataSet();

                using (IDbCommand dbCmd = CreateCommand())
                {
                    dbCmd.Transaction = dbTrans;
                    dbCmd.CommandType = cmdType;
                    dbCmd.CommandText = query;

                    IDbDataAdapter dbAdapter = CreateAdapter(dbCmd);
                    dbAdapter.Fill(dataSet);
                    dbCmd.Parameters.Clear();
                }

                CommitTransaction();

                return dataSet;
            }
            catch (Exception exc)
            {
                RollbackTransaction();
                throw exc;
            }
            finally
            {
                CloseConnection();
            }
        }

        public DataSet GetResultSet(CommandType cmdType, string sql, Dictionary<string, object> parameters = null)
        {
            try
            {
                dbConn = this.CreateConnection(_Connstr);
                CreateTransaction();

                DataSet dataSet = new DataSet();

                using (IDbCommand dbCmd = CreateCommand())
                {
                    dbCmd.Transaction = dbTrans;
                    dbCmd.CommandType = cmdType;
                    dbCmd.CommandText = sql;

                    if (parameters != null)
                    {
                        IDbDataParameter parameter;

                        foreach (var item in parameters.Keys)
                        {
                            if (!string.IsNullOrWhiteSpace(item))
                            {
                                parameter = null;
                                parameter = CreateParameter(dbCmd);
                                parameter.ParameterName = item;
                                parameter.Value = parameters[item];
                                dbCmd.Parameters.Add(parameter);
                            }
                        }
                    }

                    IDbDataAdapter dbAdapter = CreateAdapter(dbCmd);
                    dbAdapter.Fill(dataSet);
                    dbCmd.Parameters.Clear();
                }

                CommitTransaction();

                return dataSet;
            }
            catch (Exception exc)
            {
                RollbackTransaction();
                throw exc;
            }
            finally
            {
                CloseConnection();
            }
        }

        public int Execute(CommandType cmdType, string sql, Dictionary<string, object> parameters = null)
        {
            int result = -1;

            try
            {
                dbConn = this.CreateConnection(_Connstr);
                this.CreateTransaction();

                using (IDbCommand dbCmd = CreateCommand())
                {
                    dbCmd.Transaction = dbTrans;
                    dbCmd.CommandType = cmdType;
                    dbCmd.CommandText = sql;

                    if (parameters != null)
                    {
                        IDbDataParameter parameter;

                        foreach (var item in parameters.Keys)
                        {
                            if (!string.IsNullOrWhiteSpace(item))
                            {
                                parameter = null;
                                parameter = CreateParameter(dbCmd);
                                parameter.ParameterName = item;
                                parameter.Value = parameters[item];
                                dbCmd.Parameters.Add(parameter);
                            }
                        }
                    }

                    result = dbCmd.ExecuteNonQuery();
                    CommitTransaction();

                    dbCmd.Parameters.Clear();
                }
            }
            catch (Exception exc)
            {
                RollbackTransaction();
                throw exc;
            }
            finally
            {
                CloseConnection();
            }

            return result;
        }

        public object ExecuteScalar(CommandType cmdType, string sql, Dictionary<string, object> parameters = null)
        {
            object result = null;

            try
            {
                dbConn = this.CreateConnection(_Connstr);
                this.CreateTransaction();

                using (IDbCommand dbCmd = CreateCommand())
                {
                    dbCmd.Transaction = dbTrans;
                    dbCmd.CommandType = cmdType;
                    dbCmd.CommandText = sql;

                    if (parameters != null)
                    {
                        IDbDataParameter parameter;

                        foreach (var item in parameters.Keys)
                        {
                            if (!string.IsNullOrWhiteSpace(item))
                            {
                                parameter = null;
                                parameter = CreateParameter(dbCmd);
                                parameter.ParameterName = item;
                                parameter.Value = parameters[item];
                                dbCmd.Parameters.Add(parameter);
                            }
                        }
                    }

                    result = dbCmd.ExecuteScalar();
                    CommitTransaction();

                    dbCmd.Parameters.Clear();
                }
            }
            catch (Exception exc)
            {
                RollbackTransaction();
                throw exc;
            }
            finally
            {
                CloseConnection();
            }

            return result;
        }
    }
}
