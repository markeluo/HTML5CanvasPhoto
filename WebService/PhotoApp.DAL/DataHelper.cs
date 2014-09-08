using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Db4objects.Db4o;
using PhotoApp.Model;
using Db4objects.Db4o.Config;

namespace PhotoApp.DAL
{
    /// <summary>
    /// 数据处理帮助类
    /// </summary>
    public class DataHelper
    {
        private static DataHelper helper = new DataHelper();

        private static string DbSavePath = string.Empty;
        /// <summary>
        /// 数据处理实例
        /// </summary>
        public static DataHelper Instance
        {
            get
            {
                if (string.IsNullOrEmpty(DbSavePath))
                {
                    LoadingDbInfo();
                }
                return helper;
            }
        }

        /// <summary>
        /// 加载数据库信息
        /// </summary>
        private static void LoadingDbInfo()
        {
            DbSavePath=Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"luowanliPhoto.yap");
            bool IsFirsInit = false;
            if (!File.Exists(DbSavePath))
            {
                IsFirsInit = true;
            }
            IObjectContainer db = Db4oFactory.OpenFile(DbSavePath);
            try
            {
                if (IsFirsInit)
                {
                    RootAlbum rootalbum = new RootAlbum();
                    rootalbum.LastUpDateTime = DateTime.Now;
                    db.Store(rootalbum);//初始化数据
                }
            }
            finally
            { 
                db.Close(); //关闭
            }
            
        }

        IEmbeddedConfiguration configuration = null;

        /// <summary>
        /// 获取数据库对象
        /// </summary>
        /// <param name="_claobj">更新的对象</param>
        /// <returns></returns>
        public IObjectContainer GetDbObj(object _claobj)
        {
            IObjectContainer db = null;
            try
            {
                configuration = Db4oEmbedded.NewConfiguration();
                // Update all referenced objects for the Driver class
                configuration.Common.ObjectClass(_claobj).CascadeOnUpdate(true);
                db = Db4oEmbedded.OpenFile(configuration, DbSavePath);
            }
            catch(Exception ex)
            { 
            }

            return db;
        }

    }
}
