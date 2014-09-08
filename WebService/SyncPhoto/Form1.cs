using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PhotoApp.IBLL;
using PhotoApp.Model;
using PhotoApp.SQLitBLL;
using System.IO;

namespace SyncPhoto
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private string strSelPath = "";
        private void button1_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                strSelPath=textBox1.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        /// <summary>
        /// 开始同步
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(strSelPath.Trim()))
            { 
               DirectorySyncManager manager = new DirectorySyncManager(strSelPath);
               manager.AddPhotoEvent += new DirectorySyncManager.DelAddPhotoArg(manager_AddPhotoEvent);
               manager.CreateAlbumEvent += new DirectorySyncManager.DelCreateAlbumArg(manager_CreateAlbumEvent);
               manager.SyncEndEvent += new EventHandler(manager_SyncEndEvent);
                manager.Start();
            }
            
        }

        void manager_SyncEndEvent(object sender, EventArgs e)
        {
            this.UIInvoke(() =>
            {
                listBox1.Items.Add("同步结束!");
            });
        }

        void manager_CreateAlbumEvent(string _Name, bool IsSucced)
        {
            this.UIInvoke(() =>
            {
                string strTipinfo = "";
                if (IsSucced)
                {
                    strTipinfo = "相册["+_Name+"]创建成功!";
                }
                else
                {
                    strTipinfo = "相册[" + _Name + "]创建失败!";
                }
                listBox1.Items.Add(strTipinfo);
            });
        }

        void manager_AddPhotoEvent(string _AlbumName, string _ImgName, bool IsSucced)
        {
            this.UIInvoke(() =>
           {
               string strTipinfo = "";
               if (IsSucced)
               {
                   strTipinfo = "相册[" + _AlbumName + "],成功上传:" + _ImgName + "!";
               }
               else
               {
                   strTipinfo = "相册[" + _AlbumName + "],上传:" + _ImgName + "失败!";
               }
               listBox1.Items.Add(strTipinfo);
           });
        }

        private void button3_Click(object sender, EventArgs e)
        {
            IAlbumManager manager = new AlbumManager();
            List<Album> albums= manager.GetAlbumList();
            foreach (Album _album in albums)
            {
                listBox1.Items.Add(string.Format("{0}共有{1}张相片!", _album.AlbumName,_album.SubPhotos.Count));
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           DBConfigControl.DBConfig _config= DBConfigControl.DBConfigFactory.Instance.LoadDbconfig();
           if (_config != null && _config.IsUse)
           {
               listBox1.Items.Add("数据库初始化成功");
           }
           else
           {
               string strDataRoot = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
               if (!Directory.Exists(strDataRoot))
               {
                   Directory.CreateDirectory(strDataRoot);
               }
              int _Initstate=DBConfigControl.InitializationSQLiteDB.Instance.SaveDbconfig(strDataRoot);
              if (_Initstate >1)
              {
                  listBox1.Items.Add("数据库初始化成功");
              }
           }
        }

    }
}
