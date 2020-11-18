using System;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
namespace MVCPrac.Models
{
    public class DataAccess : IDisposable
    {
        string strCon = "";
        public List<string> ErrorMsg;
        SqlConnection SqlCon;
        private void OpenConnection()
        {
            if (SqlCon.State == ConnectionState.Open)
            {
                SqlCon.Close();
            }
            SqlCon = new SqlConnection(strCon);
        }
        private void CloseConnection()
        {
            if (SqlCon.State != ConnectionState.Closed)
            {
                SqlCon.Close();
            }
        }
        public DataAccess(string connstring)
        {
            strCon = connstring;
            ErrorMsg = new List<string>();
        }

        public string ExecuteProc(List<SQLProc> lstSQLProc)
        {
            OpenConnection();
            SqlTransaction tran = SqlCon.BeginTransaction();
            try
            {
                SqlCommand command;
                foreach (SQLProc sqlproc in lstSQLProc)
                {
                    command = new SqlCommand();
                    command.Connection = SqlCon;
                    command.Transaction = tran;
                    command.CommandText = sqlproc.ProcedureName;
                    command.CommandType = CommandType.StoredProcedure;
                    if (sqlproc.SqlParameters.Count > 0)
                        command.Parameters.AddRange(sqlproc.SqlParameters.ToArray());
                    command.ExecuteNonQuery();
                }
                tran.Commit();
                return "";
            }
            catch (Exception ex)
            {
                tran.Rollback();
                return ex.Message;
            }
            finally
            {
                CloseConnection();
            }
        }

        public DataTable GetDataTableFromQuery(string SQLScript)
        {
            OpenConnection();
            try
            {
                DataTable objdata = new DataTable();
                SqlCommand command = new SqlCommand(SQLScript, SqlCon);
                SqlDataAdapter adpt = new SqlDataAdapter(command);
                adpt.Fill(objdata);
                return objdata;
            }
            catch (Exception ex)
            {
                ErrorMsg.Add(ex.Message);
                return null;
            }
            finally
            {
                CloseConnection();
            }
        }

        public void Dispose()
        {
            if (SqlCon != null)
            {
                if (SqlCon.State == ConnectionState.Open)
                    SqlCon.Close();
                SqlCon.Dispose();
            }
        }
    }

    public class SQLProc
    {
        public string ProcedureName { get; set; }
        public List<SqlParameter> SqlParameters { get; set; }
    }
}