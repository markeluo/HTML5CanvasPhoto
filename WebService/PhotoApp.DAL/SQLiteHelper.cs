using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;
using System.Data.Common;
using System.Data;

namespace PhotoApp.DAL
{
    /// <summary>
    /// SQLite数据库访问帮助类
    /// </summary>
    public class SQLiteHelper
    {
        private static SQLiteHelper help = new SQLiteHelper();

        private static string connectionString = string.Empty;
        public static SQLiteHelper Instance
        {
            get
            {
                if (string.IsNullOrEmpty(connectionString))
                {
                    LoadConnectionstring();
                }
                return help;
            }
        }
        /// <summary>
        /// 连接更改
        /// </summary>
        public void ConnectionChanged()
        {
            connectionString = "";
        }

        private static void LoadConnectionstring()
        {
            DBConfigControl.DBConfig dbconfig = DBConfigControl.DBConfigFactory.Instance.LoadDbconfig();
            connectionString = dbconfig.GetConnectionstring();
        }

        /// <summary>
        /// 执行非查询
        /// </summary>
        /// <param name="sql">命令语句</param>
        /// <param name="parameters">参数</param>
        /// <returns>返回影响记录的行数</returns>
        public int ExecuteNonQuery(string sql, SQLiteParameter[] parameters)
        {
            int affectedRows = 0;
            SQLiteConnection connection = new SQLiteConnection(connectionString);
            connection.Open();

            DbTransaction transaction = connection.BeginTransaction();//启动事务

            SQLiteCommand command = new SQLiteCommand(connection);
            command.CommandText = sql;
            if (parameters != null)
            {
                command.Parameters.AddRange(parameters);
            }
            affectedRows = command.ExecuteNonQuery();

            transaction.Commit();//提交事务

            return affectedRows;
        }

        /// <summary>
        /// 插入一行记录，获得该记录的自增长ID
        /// </summary>
        /// <param name="sql">命令语句</param>
        /// <param name="parameters">参数</param>
        /// <returns>新插入数据的自增长ID</returns>
        public int InsertRecordGetIdentityID(string sql, SQLiteParameter[] parameters)
        {
            int ID = 0;
            SQLiteConnection connection = new SQLiteConnection(connectionString);
            connection.Open();

            DbTransaction transaction = connection.BeginTransaction();//启动事务

            SQLiteCommand command = new SQLiteCommand(connection);
            command.CommandText = sql;
            if (parameters != null)
            {
                command.Parameters.AddRange(parameters);
            }

            object _obj = command.ExecuteScalar();
            transaction.Commit();//提交事务
            try
            {
                if (_obj != null)
                {
                    ID = Convert.ToInt32(_obj);
                }
            }
            catch 
            {     
            }
            return ID;
        }

        #region 事务控制处理
        /// <summary>
        /// 执行更改命令，返回处理是否成功
        /// </summary>
        /// <param name="cmd">命令</param>
        /// <param name="_transaction">事务</param>
        /// <param name="submitaction">是否提交事务</param>
        /// <returns></returns>
        public bool ExeCuteCmd(SQLiteCommand cmd, DbTransaction _transaction, bool submitaction)
        {
            bool bolSucced = true;
            try
            {
                int _RecordRows = cmd.ExecuteNonQuery();
            }
            catch
            {
                bolSucced = false;
            }
            if (submitaction)
            {
                _transaction.Commit();
            }
            return bolSucced;
        }
        #endregion

        /// <summary>
        /// 执行一个查询语句，返回一个包含查询结果的DataTable
        /// </summary>
        /// <param name="sql">要执行的查询语句</param>
        /// <param name="parameters">执行SQL查询语句所需要的参数，参数必须以它们在SQL语句中的顺序为准</param>
        /// <returns></returns>
        public DataTable ExecuteDataTable(string sql, SQLiteParameter[] parameters)
        {
            DataTable data = null;
            try
            {
                SQLiteConnection connection = new SQLiteConnection(connectionString);
                SQLiteCommand command = new SQLiteCommand(sql, connection);
                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters);
                }
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);
                data = new DataTable();
                adapter.Fill(data);
            }
            catch (Exception ex)
            { 
            
            }
            return data;
        }

        /// <summary>
        /// 插入table中的数据至数据库相应名称的表中
        /// </summary>
        /// <param name="dt">需要插入的数据表(需要有表头)</param>
        /// <param name="tablename">插入到的表的名称</param>
        /// <returns>true:插入成功;false:插入失败</returns>
        public bool insertDbByDataTable(DataTable dt, string tablename)
        {
            bool result = true;
            if (dt == null || dt.Columns.Count <= 0 || tablename == null || tablename.Trim() == string.Empty) return false;
            SQLiteConnection conn = new SQLiteConnection(connectionString);
            conn.Open();
            SQLiteTransaction dbTrans = conn.BeginTransaction();

            SQLiteCommand cmd = conn.CreateCommand();
            try
            {
                cmd.CommandType = CommandType.Text;
                cmd.Connection = conn;
                string pars = "";
                string columns = "";
                int index = 0;
                SQLiteParameter[] parameters = new SQLiteParameter[dt.Columns[0].AutoIncrement ? dt.Columns.Count - 1 : dt.Columns.Count];
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    if (!dt.Columns[i].AutoIncrement)
                    {
                        columns += dt.Columns[i].ColumnName + ",";
                        parameters[index] = cmd.CreateParameter();
                        pars += "?,";
                        index++;
                    }
                }
                columns = string.Format("{0}({1})", tablename, columns.TrimEnd(','));
                pars = pars.TrimEnd(',');
                cmd.Parameters.AddRange(parameters);
                cmd.CommandText = string.Format("insert into {0} values({1})", columns, pars);
                foreach (DataRow r in dt.Rows)
                {
                    object[] data = r.ItemArray;
                    index = 0;
                    foreach (SQLiteParameter p in parameters)
                    {
                        p.Value = data[index];
                        index++;
                    }
                    cmd.ExecuteNonQuery();
                }
            }
            catch
            {
                result = false;
            }
            dbTrans.Commit();
            return result;
        }

        /// <summary>
        /// 查询数据库中所有的表
        /// </summary>
        /// <returns></returns>
        public DataTable GetSchema()
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                DataTable data = connection.GetSchema("TABLES");
                connection.Close();
                return data;
            }
        }

        /// <summary>
        /// 判断相应名称的表是否存在
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns>true:存在;false:不存在</returns>
        public bool TableIsExist(String tableName)
        {
            bool result = false;
            if (string.IsNullOrEmpty(tableName))
            {
                return false;
            }
            try
            {
                String sql = "select count(*) as c from sqlite_master where type ='table' and name ='" + tableName.Trim() + "' ";
                SQLiteConnection connection = new SQLiteConnection(connectionString);
                connection.Open();
                SQLiteCommand command = new SQLiteCommand(sql, connection);
                int on = Convert.ToInt32(command.ExecuteScalar());
                if (on > 0)
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            catch
            {
            }
            return result;
        }

    }
}
