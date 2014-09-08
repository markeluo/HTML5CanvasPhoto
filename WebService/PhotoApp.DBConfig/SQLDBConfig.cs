using System;
using System.Collections.Generic;
using System.Text;

namespace DBConfigControl
{
    /// <summary>
    /// SQL Server数据库配置信息
    /// </summary>
    public class SQLDBConfig:DBConfig
    {
        private string userName;
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }
        private string userPwd;
        /// <summary>
        /// 密码
        /// </summary>
        public string UserPwd
        {
            get { return userPwd; }
            set { userPwd = value; }
        }
        private string serverAddress;
        /// <summary>
        /// SQL Server服务器地址
        /// </summary>
        public string ServerAddress
        {
            get { return serverAddress; }
            set { serverAddress = value; }
        }
        private int dbId;
        /// <summary>
        /// 数据库编号
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
        private string dbRemarke;
        /// <summary>
        /// 备注
        /// </summary>
        public string DbRemarke
        {
            get { return dbRemarke; }
            set { dbRemarke = value; }
        }

        /// <summary>
        /// 数据类型名称
        /// </summary>
        public override string DbTypeName
        {
            get
            {
                return "SQLServer";
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
               return configistrue;
            }
        }

        /// <summary>
        /// 获得连接字符串 重写
        /// </summary>
        /// <returns>连接字符串</returns>
        public override string GetConnectionstring()
        {
            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(UserPwd))
            {

                return String.Format("server={0};database={1};user id = {2};password={3};Connect Timeout=3000", serverAddress, dbName, userName, userPwd);
            }
            else
            {
                return String.Format("packet size=4096;integrated security=SSPI;data source={0};persist security info=False;initial catalog={1};Connect Timeout=3000", serverAddress, dbName);
            }
        }
    }
}
