using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace WeiChat.Dao
{
    public class MySqlDbHelper
    {
        private string _connSrt;
        public MySqlDbHelper(string connSrt)
        {
            _connSrt = connSrt;
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        /// <summary>
        /// 判读是否存在
        /// </summary>
        /// <param name="strSql">sql语句</param>
        /// <param name="cmdParams">cmd参数</param>
        /// <returns>true：存在；false：不存在</returns>
        public bool Exists(string strSql, params MySqlParameter[] cmdParams)
        {
            object obj = GetSingle(strSql, cmdParams);
            int cmdResult;
            if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
            {
                cmdResult = 0;
            }
            else
            {
                cmdResult = int.Parse(obj.ToString());
            }
            if (cmdResult == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 执行简单数据操作
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns>受影响的行数</returns>
        public int ExecuteSql(string sql, params MySqlParameter[] cmdParms)
        {
            using (MySqlConnection connection = new MySqlConnection(_connSrt))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    try
                    {
                        PrepareCommand(cmd, connection, sql, cmdParms, null);
                        int rows = cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                        return rows;
                    }
                    catch (MySqlException e)
                    {
                        return -1;
                    }
                }
            }
        }

        /// <summary>
        /// 执行简单sql语句（响应时间）
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="times">响应时间</param>
        /// <returns>受影响的行数</returns>
        public int ExecuteSqlByTime(string sql, int times)
        {
            using (MySqlConnection connection = new MySqlConnection(sql))
            {
                using (MySqlCommand cmd = new MySqlCommand(sql, connection))
                {
                    try
                    {
                        connection.Open();
                        cmd.CommandTimeout = times;
                        int rows = cmd.ExecuteNonQuery();
                        return rows;
                    }
                    catch (MySqlException e)
                    {
                        connection.Close();
                        throw e;
                    }
                }
            }
        }

        /// <summary>
        /// 执行多条SQL语句，事项数据库事务
        /// </summary>
        /// <param name="sqlList">SQL数据集合</param>
        /// <returns>受影响的行数</returns>
        public int ExecuteSqlTran(List<String> sqlList)
        {
            using (MySqlConnection conn = new MySqlConnection(_connSrt))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conn;
                MySqlTransaction tx = conn.BeginTransaction();
                cmd.Transaction = tx;
                try
                {
                    int count = 0;
                    for (int n = 0; n < sqlList.Count; n++)
                    {
                        string strsql = sqlList[n];
                        if (strsql.Trim().Length > 1)
                        {
                            cmd.CommandText = strsql;
                            count += cmd.ExecuteNonQuery();
                        }
                    }
                    tx.Commit();
                    return count;
                }
                catch
                {
                    tx.Rollback();
                    return 0;
                }
            }
        }

        /// <summary>
        /// 执行查询语句，返回MySqlDataReader ( 注意：调用该方法后，一定要对MySqlDataReader进行Close )
        /// </summary>
        /// <param name="strSQL">查询语句</param>
        /// <returns>MySqlDataReader</returns>
        public MySqlDataReader ExecuteReader(string sql, params MySqlParameter[] cmdParms)
        {
            MySqlConnection connection = new MySqlConnection(_connSrt);
            MySqlCommand cmd = new MySqlCommand();
            try
            {
                PrepareCommand(cmd, connection, sql, cmdParms, null);
                MySqlDataReader myReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();
                return myReader;
            }
            catch (MySqlException e)
            {
                throw e;
            }
        }

        /// <summary>
        /// 获取第一行第一列的值
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="cmdParams">sql参数</param>
        /// <returns>第一行第一列值</returns>
        public object GetSingle(string sql, params MySqlParameter[] cmdParams)
        {
            using (MySqlConnection connection = new MySqlConnection(_connSrt))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    PrepareCommand(cmd, connection, sql, cmdParams, null);
                    object obj = cmd.ExecuteScalar();
                    cmd.Parameters.Clear();
                    if ((object.Equals(obj, null)) || (object.Equals(obj, DBNull.Value)))
                    {
                        return null;
                    }
                    return obj;
                }
            }
        }

        private void PrepareCommand(MySqlCommand cmd, MySqlConnection conn, string cmdText, MySqlParameter[] cmdParams, MySqlTransaction trans)
        {
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            if (trans != null)
            {
                cmd.Transaction = trans;
            }

            if (cmdParams != null)
            {
                foreach (MySqlParameter parameter in cmdParams)
                {
                    if ((parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input) &&
                        (parameter.Value == null))
                    {
                        parameter.Value = DBNull.Value;
                    }
                    cmd.Parameters.Add(parameter);
                }
            }
        }
    }
}
