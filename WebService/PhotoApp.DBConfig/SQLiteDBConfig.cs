using System;
using System.Collections.Generic;
using System.Text;

namespace DBConfigControl
{
    /// <summary>
    /// SQLite数据库信息
    /// </summary>
    public class SQLiteDBConfig:DBConfig
    {
        private string serverAddress;
        /// <summary>
        /// 地址
        /// </summary>
        public string ServerAddress
        {
            get { return serverAddress; }
            set 
            {
                if (value.ToLower().Contains("data source="))
                {
                    serverAddress =value;
                }
                else
                {
                    serverAddress = "Data Source=" + value;
                }  
            }
        }
        private int dbId;
        /// <summary>
        /// 编号
        /// </summary>
        public int DbId
        {
            get { return dbId; }
            set { dbId = value; }
        }
        private string dbName;
        /// <summary>
        /// 数据库名称
        /// </summary>
        public string DbName
        {
            get { return dbName; }
            set { dbName = value; }
        }

        /// <summary>
        /// 数据类型名称
        /// </summary>
        public override string DbTypeName
        {
            get
            {
                return "SQLite";
            }
        }

        private bool configistrue = true;
        /// <summary>
        /// 配置是否正确
        /// </summary>
        public override bool IsTrue
        {
            get
            {
                try
                {
                    bool bolconnect = ConnectAddressIstrue();
                    if (bolconnect)
                    {
                            configistrue = true;
                    }
                    else
                    {
                        configistrue = false;
                    }
                }
                catch
                {
                    configistrue = false;
                }

                return configistrue;
            }
        }


        /// <summary>
        /// 连接地址是否正确
        /// </summary>
        /// <returns>true:正确;false:错误</returns>
        private bool ConnectAddressIstrue()
        {
            bool result = false;
            using (System.Data.SQLite.SQLiteConnection connection = new System.Data.SQLite.SQLiteConnection(serverAddress))
            {
                try
                {
                    connection.Open();
                    connection.Close();
                    result = true;
                }
                catch
                {
                }
                finally 
                {
                    connection.Close();
                }
                
            }
            return result;
        }


        public override string GetConnectionstring()
        {
            return this.serverAddress;
        }
    }
}
