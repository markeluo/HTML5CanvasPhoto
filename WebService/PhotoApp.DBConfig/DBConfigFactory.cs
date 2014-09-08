using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;

namespace DBConfigControl
{
    /// <summary>
    /// 数据库处理
    /// </summary>
    public class DBConfigFactory
    {
        private static DBConfigFactory control = new DBConfigFactory();
        /// <summary>
        ///信息控制类
        /// </summary>
        public static DBConfigFactory Instance
        {
            get
            {
                return control;
            }
        }

        /// <summary>
        /// 获得当前配置的正在使用数据库信息
        /// </summary>
        /// <returns></returns>
        public DBConfig LoadDbconfig()
        {
            DBConfig config = null;
            List<DBConfig> allconfigs = LoadAllDbConfig();//所有的数据库配置
            if (allconfigs != null && allconfigs.Count > 0)
            {
                foreach (DBConfig _config in allconfigs)
                {
                    if (_config.IsUse)
                    {
                        config = _config;
                        break;
                    }
                }
            }
            return config;
        }

        /// <summary>
        /// 多数据库时，获得所有的数据库信息
        /// </summary>
        /// <returns></returns>
        public List<DBConfig> LoadAllDbConfig()
        {
            int type = GetDBType();//数据库的类型

            List<DBConfig> configs = null;
            if (type == 1)
            {
                configs = GetSQliteDbConfigs();
            }
            else
            {
                configs = GetSQLDbConfigs();
            }


            return configs;
        }

        /// <summary>
        /// 保存数据库配置 (暂时只实现了SQlite数据库配置的保存)
        /// </summary>
        /// <param name="_configs">配置列表</param>
        /// <returns>true:保存成功;false:保存失败</returns>
        public bool SaveDbConfigs(List<DBConfig> _configs)
        {
            bool bolSucced = false;
            if (_configs != null && _configs.Count > 0)
            {
                try
                {
                    string DbSettingPath = GetDbSettingPath();//数据库配置文件存储地址

                    XmlDocument doc = new XmlDocument();

                    doc.LoadXml("<DataBaseConfig></DataBaseConfig>");

                    XmlNode root = doc.DocumentElement;

                    XmlNode MainNode = doc.CreateElement("DataBases");


                    foreach (DBConfig _config in _configs)
                    {
                        #region 添加数据库

                        XmlNode DbNode = doc.CreateElement("DbInfo");

                        if (_config is SQLiteDBConfig)
                        {
                            SQLiteDBConfig _sqliteconfig = (SQLiteDBConfig)_config;
                            XmlAttribute attri = doc.CreateAttribute("IsUs");
                            if (_sqliteconfig.IsUse)
                            {
                                attri.Value = "1";
                            }
                            else
                            {
                                attri.Value = "0";
                            }
                            DbNode.Attributes.Append(attri);

                            XmlNode DbFileNode = doc.CreateElement("File");
                            DbFileNode.InnerText = _sqliteconfig.ServerAddress;
                            DbNode.AppendChild(DbFileNode);

                            XmlNode DbRemarkeNode = doc.CreateElement("ReMarke");
                            DbRemarkeNode.InnerText = _sqliteconfig.DBReamrke;
                            DbNode.AppendChild(DbRemarkeNode);

                        }

                        MainNode.AppendChild(DbNode);
                        #endregion
                    }
                    root.AppendChild(MainNode);

                    doc.Save(DbSettingPath);

                    bolSucced = true;
                }
                catch
                { 
                }
            }
            return bolSucced;
        }

        #region 获取数据库信息(SQL 数据库/SQLite数据库)
        /// <summary>
        /// 获取数据库类型
        /// </summary>
        /// <returns>0:SQL Sevever数据库;1:SQLite数据库</returns>
        private int GetDBType()
        {
            int type = 0;
            string strDbTypeConfigPath = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "ConfigFIle/DbTypeConfig.xml");
            FileInfo file = new FileInfo(strDbTypeConfigPath);
            if (file.Exists)
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(strDbTypeConfigPath);
                XmlNode root = doc.DocumentElement;
                if (root.ChildNodes != null && root.ChildNodes.Count > 0)
                {
                    foreach (XmlNode _node in root.ChildNodes)
                    {
                        if (_node.Attributes["Type"].InnerText == "sqlite")
                        {
                            type = 1;
                        }
                        else
                        {
                            type = 0;
                        }
                        break;
                    }
                }
            }
            return type;
        }

        /// <summary>
        /// SQLite数据库配置信息列表
        /// </summary>
        /// <returns></returns>
        private List<DBConfig> GetSQliteDbConfigs()
        {
            List<DBConfig> _configs = null;
            
            
            #region 获得数据库配置存储地址
            string DbSettingPath = GetDbSettingPath();
            #endregion

            #region 读取数据库配置
            if (File.Exists(DbSettingPath))
            {
                try
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(DbSettingPath);
                    XmlNode root = doc.DocumentElement;
                    if (root.ChildNodes != null && root.ChildNodes.Count == 1)
                    {
                        XmlNode _node = root.ChildNodes[0];
                        if (_node.ChildNodes != null && _node.ChildNodes.Count > 0)
                        {
                            _configs = new List<DBConfig>();
                            SQLiteDBConfig _config = null;
                            foreach (XmlNode _dbnode in _node.ChildNodes)
                            {
                                if (_dbnode.Name == "DbInfo" && _dbnode.ChildNodes != null && _dbnode.ChildNodes.Count > 0)
                                {
                                    _config = new SQLiteDBConfig();

                                    if (_dbnode.Attributes["IsUs"].InnerText == "1")
                                    {
                                        _config.IsUse = true;
                                    }
                                    foreach (XmlNode _dbdetail in _dbnode.ChildNodes)
                                    {
                                        if (_dbdetail.Name == "File")
                                        {
                                            _config.ServerAddress = _dbdetail.InnerText;
                                        }
                                        if (_dbdetail.Name == "ReMarke")
                                        {
                                            _config.DBReamrke = _dbdetail.InnerText;
                                        }
                                    }

                                    _configs.Add(_config);
                                }
                            }
                        }
                    }
                }
                catch
                { 
                }
            }
            #endregion

            return _configs;
        }

        /// <summary>
        /// 获得数据库配置文件存储地址
        /// </summary>
        /// <returns></returns>
        private string GetDbSettingPath()
        {
            string DbSettingPath = string.Empty;
            try
            {
                string ConfigFileName = string.Empty;
                string strDbTypeConfigPath = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "ConfigFIle/DbTypeConfig.xml");
                FileInfo file = new FileInfo(strDbTypeConfigPath);
                if (file.Exists)
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(strDbTypeConfigPath);
                    XmlNode root = doc.DocumentElement;
                    if (root.ChildNodes != null && root.ChildNodes.Count > 0)
                    {
                        foreach (XmlNode _node in root.ChildNodes)
                        {
                            ConfigFileName = _node.InnerText;
                            break;
                        }
                    }
                }

                DbSettingPath = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, string.Format("ConfigFIle/{0}", ConfigFileName));
            }
            catch
            {
            }
            return DbSettingPath;
        }

        /// <summary>
        /// SQL Sever数据库配置信息列表
        /// </summary>
        /// <returns></returns>
        private List<DBConfig> GetSQLDbConfigs()
        {
            List<DBConfig> _configs = null;
            return _configs;
        }

        #endregion
    }
}
