using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;
using System.Data;
using System.IO;
using System.Data.Common;
using System.Xml;

namespace DBConfigControl
{
    /// <summary>
    /// 初始化SQLite数据库信息
    /// </summary>
    public class InitializationSQLiteDB
    {
        private static InitializationSQLiteDB control = new InitializationSQLiteDB();

        /// <summary>
        /// 初始化数据库信息 实例 单例
        /// </summary>
        public static InitializationSQLiteDB Instance
        {
            get
            {
                return control;
            }
        }

        /// <summary>
        /// 创建数据库文件
        /// <param name="_DBFileSavePath">数据库文件保存路径</param>
        /// </summary>
        public bool CreateDataBase(string _DBFileSavePath)
        {
            bool IsSucced = false;
            try
            {
                //1.创建Db文件
                SQLiteConnection.CreateFile(_DBFileSavePath);

                List<string> ScriptFiles = GetScriptsList();//获得命令脚本文件
                if (ScriptFiles != null && ScriptFiles.Count > 0)
                {
                    bool _bolSucced = true;
                    foreach (string _srcFilepaath in ScriptFiles)
                    {
                        _bolSucced = ExecuteNoQueryCommand(_DBFileSavePath, _srcFilepaath);
                        if (!_bolSucced)
                        {
                            break;
                        }

                    }
                    IsSucced = _bolSucced;
                }
            }
            catch
            {
            }
            return IsSucced;
        }

        /// <summary>
        /// 2.用户选择数据存储目录，为用户创建相应的数据库文件和说明书存储目录
        /// </summary>
        /// <param name="_DBSaveDirectory">数据文件存储目录</param>
        /// <param name="Isoverride">是否覆盖(如果存在同名的数据库文件)</param>
        /// <returns></returns>
        public bool SaveDbconfig(string _DBSaveDirectory,bool Isoverride)
        {
            bool _Succed = false;

            string DbFileSaveDirectory = Path.Combine(_DBSaveDirectory, "Database");
            DirectoryInfo dir = new DirectoryInfo(DbFileSaveDirectory);
            if (!dir.Exists)
            {
                dir.Create();
            }

            string DbSavePath = Path.Combine(dir.FullName, "IPSdb.db3");
            string ImgSaveDirpath = Path.Combine(_DBSaveDirectory,"ImagePath");
            FileInfo file = new FileInfo(DbSavePath);
            if (file.Exists)
            {
                if (Isoverride)
                {
                    file.Delete();//删除原文件
                    _Succed = CreateDataBase(DbSavePath);//重新创建数据库文件
                }
            }
            else
            {
                _Succed = CreateDataBase(DbSavePath);//重新创建数据库文件
            }

            
            if (_Succed)
            {
                try
                {
                    if (!Directory.Exists(ImgSaveDirpath))
                    {
                        Directory.CreateDirectory(ImgSaveDirpath);
                    }

                    SQLiteDBConfig _LtDbConfig = new SQLiteDBConfig();
                    _LtDbConfig.ServerAddress =DbSavePath;
                    _LtDbConfig.DBReamrke = "个人数据库";
                    _LtDbConfig.DbName = "IPSdb";
                    _LtDbConfig.IsUse = true;

                    List<DBConfig> _configs = DBConfigFactory.Instance.LoadAllDbConfig();
                    if (_configs == null)
                    {
                        _configs = new List<DBConfig>();
                    }
                    else
                    { 
                        foreach(DBConfig _config in _configs)
                        {
                            _config.IsUse = false;
                        }
                    }
                    _configs.Add(_LtDbConfig);

                    DBConfigFactory.Instance.SaveDbConfigs(_configs);
                }
                catch
                {
                    _Succed = false;
                }
            }

            return _Succed;
        }

        /// <summary>
        /// 1.用户选择数据存储目录，为用户创建相应的数据库文件和说明书存储目录(内部会判断之前是否已经进行过配置，即同名数据库文件判断)
        /// </summary>
        /// <param name="_DBSaveDirectory">保存数据的目录</param>
        /// <returns>0:保存失败;1:保存成功;2:之前配置过，存在同名的数据库文件</returns>
        public int SaveDbconfig(string _DBSaveDirectory)
        {
            int _state = 0;

            string DbSavePath = Path.Combine(_DBSaveDirectory, "IPSdb.db3");
            string ImgSaveDirpath = Path.Combine(_DBSaveDirectory,"ImagePath");
            if (!Directory.Exists(ImgSaveDirpath))
            {
                Directory.CreateDirectory(ImgSaveDirpath);
            }
            FileInfo file = new FileInfo(DbSavePath);
            if (file.Exists)
            {
                _state = 2;
            }
            else
            {
                try
                {
                    bool _dbcreateSucced = CreateDataBase(DbSavePath);//创建状态
                    if (_dbcreateSucced)
                    {
                        if (!Directory.Exists(ImgSaveDirpath))
                        {
                            Directory.CreateDirectory(ImgSaveDirpath);
                        }

                        SQLiteDBConfig _LtDbConfig = new SQLiteDBConfig();
                        _LtDbConfig.ServerAddress = DbSavePath;
                        _LtDbConfig.DBReamrke = "相册信息1";
                        _LtDbConfig.DbName = "Ablum01";
                        _LtDbConfig.IsUse = true;

                        List<DBConfig> _configs = new List<DBConfig>();
                        _configs.Add(_LtDbConfig);

                        DBConfigFactory.Instance.SaveDbConfigs(_configs);

                        _state = 1;
                    }
                    else
                    {
                        _state = 0;
                    }
                }
                catch
                {
                    _state = 0;
                }
            }
            return _state;
        }

        /// <summary>
        /// 更改数据库存储配置
        /// </summary>
        /// <param name="_DbFilePath"></param>
        /// <returns></returns>
        public bool ChangeDbConfig(string _DbFilePath)
        {
            bool bolSucced = false;
            try
            {
                SQLiteDBConfig _LtDbConfig = new SQLiteDBConfig();
                _LtDbConfig.ServerAddress = _DbFilePath;
                _LtDbConfig.DBReamrke = "个人数据库";
                _LtDbConfig.DbName = "IPSdb";
                _LtDbConfig.IsUse = true;

                List<DBConfig> _configs = DBConfigFactory.Instance.LoadAllDbConfig();
                if (_configs == null)
                {
                    _configs = new List<DBConfig>();
                }
                else
                {
                    foreach (DBConfig _config in _configs)
                    {
                        _config.IsUse = false;
                    }
                }
                _configs.Add(_LtDbConfig);

                DBConfigFactory.Instance.SaveDbConfigs(_configs);
            }
            catch (Exception ex)
            { 
            }
            return bolSucced;
        }

        #region 获得创建数据库所需的命令脚本信息

        /// <summary>
        /// 命令脚本文件列表
        /// </summary>
        /// <returns></returns>
        private List<string> GetScriptsList()
        {
            List<string> scripts = null;
            string CreateDbConfigPath = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "DBcreate/LtdbConfig.xml");
            FileInfo _file=new FileInfo (CreateDbConfigPath);
            if (_file.Exists)//配置文件存在
            {
                string strDirectory = _file.Directory.FullName;
                XmlDocument doc = new XmlDocument();
                doc.Load(CreateDbConfigPath);
                XmlNode root = doc.DocumentElement;
                if (root.ChildNodes != null && root.ChildNodes.Count > 0)
                {
                    scripts = new List<string>();
                    foreach (XmlNode _node in root.ChildNodes)
                    {
                        scripts.Add(Path.Combine(strDirectory,_node.InnerText));
                    }
                }
            }
            return scripts;
        }
        
        #endregion

        #region 创建表、视图、索引，添加初始数据

        /// <summary>
        /// 执行非查询命令
        /// </summary>
        /// <param name="_connectionString">数据库连接字符串（存储位置）</param>
        /// <param name="_ScriptPath">脚本文件存储地址</param>
        /// <returns>true:执行成功;false:执行失败</returns>
        private bool ExecuteNoQueryCommand(string _connectionString, string _ScriptPath)
        {
            bool _bolSucced=false;
            try
            {
                //读取命令字符串
                string Scriptvalue = GetScriptDetail(_ScriptPath);

                SQLiteConnection connection = new SQLiteConnection(string.Format("Data Source={0}",_connectionString));
                connection.Open();

                DbTransaction transaction = connection.BeginTransaction();//启动事务

                SQLiteCommand command = new SQLiteCommand(connection);
                command.CommandText = Scriptvalue;

                int affectedRows = command.ExecuteNonQuery();

                transaction.Commit();//提交事务
                connection.Close();

                _bolSucced = true;
            }
            catch(Exception ex)
            {
            }

            return _bolSucced;
        }

        /// <summary>
        /// 读取文件脚本中的内容
        /// </summary>
        /// <param name="_filePath"></param>
        /// <returns></returns>
        private string GetScriptDetail(string _filePath)
        {
            try
            {
                System.IO.StreamReader reader = new System.IO.StreamReader(_filePath);
                string content = reader.ReadToEnd();
                reader.Close();
                return content;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
