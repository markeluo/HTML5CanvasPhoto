using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using PhotoApp.Model;
using PhotoApp.IBLL;
using PhotoApp.SQLitBLL;

namespace SyncPhoto
{
    /// <summary>
    /// 同步处理
    /// </summary>
    public class DirectorySyncManager
    {
        public delegate void DelCreateAlbumArg(string _Name,bool IsSucced);
        public delegate void DelAddPhotoArg(string _AlbumName, string _ImgName,bool IsSucced);
        public event DelCreateAlbumArg CreateAlbumEvent;
        public event DelAddPhotoArg AddPhotoEvent;
        public event EventHandler SyncEndEvent;

        private string rootPath = "";
        public DirectorySyncManager(string _RootPath)
        {
            rootPath = _RootPath;
        }
        private Thread ThisScanThread = null;
        public void Start()
        {
            ThisScanThread = new System.Threading.Thread(new System.Threading.ThreadStart(scanthread));
            ThisScanThread.IsBackground = true;
            ThisScanThread.Start();
        }

        private void scanthread()
        {
            if (Directory.Exists(rootPath))
            {
               string[] SubDirs= Directory.GetDirectories(rootPath);
               List<Album> ablums= CreateAlbum(SubDirs);

               //缩略图缓存目录是否存在，不存在则创建
               string miniImgtempRoot = Path.Combine(rootPath, "miniImgtemp");
               if (!Directory.Exists(miniImgtempRoot))
               {
                   Directory.CreateDirectory(miniImgtempRoot);
               }

               AddPhotoToAlbum(ablums,SubDirs);

               if (SyncEndEvent != null)
               {
                   SyncEndEvent(null, null);
               }
            }
            else
            {
                if (SyncEndEvent != null)
                {
                    SyncEndEvent(null, null);
                }
            }
        }

        /// <summary>
        /// 创建相册
        /// </summary>
        /// <param name="SubDirs"></param>
        private List<Album> CreateAlbum(string[] SubDirs)
        {
            List<Album> list = new List<Album>();
            if (SubDirs != null && SubDirs.Length > 0)
            {
                IAlbumManager manager = new AlbumManager();
                Album newalbume = null;
                DirectoryInfo _dir=null;
                foreach (string strpath in SubDirs)
                {
                    _dir=new DirectoryInfo(strpath);
                    newalbume = new Album(_dir.Name, "");
                    ManagerStateMsg state = manager.SaveAlbum(newalbume);
                    if (CreateAlbumEvent != null) 
                    {
                        if (state.Statevalue)
                        {
                            list.Add((Album)state.ReturnValue);
                        }
                        CreateAlbumEvent(_dir.Name, state.Statevalue);
                    }
                }
            }
            else
            {
                if (SyncEndEvent != null)
                {
                    SyncEndEvent(null, null);
                }
            }

            return list;
        }

        /// <summary>
        /// 添加照片
        /// </summary>
        /// <param name="abums">已创建相册列表</param>
        /// <param name="SubDirs">相册文件夹数组</param>
        private void AddPhotoToAlbum(List<Album> abums,string[] SubDirs)
        {
            if (SubDirs != null && SubDirs.Length > 0)
            {
                IAlbumManager manager = new AlbumManager();
                List<string> ImgList = null;

                DirectoryInfo _dir = null;
                foreach (string AblumPath in SubDirs)
                {
                    _dir = new DirectoryInfo(AblumPath);
                    if (Directory.Exists(AblumPath))
                    {
                        ImgList = Directory.GetFiles(AblumPath, "*.jpg").ToList();
                        ImgList.AddRange(Directory.GetFiles(AblumPath, "*.bmp").ToList());
                        ImgList.AddRange(Directory.GetFiles(AblumPath, "*.png").ToList());

                    }
                    AddPhotosToAlume(ImgList,GetAlbumByName(abums,_dir.Name));//添加照片至相册
                }
            }
            else
            {
                if (SyncEndEvent != null)
                {
                    SyncEndEvent(null, null);
                }
            }
        }

        private Album GetAlbumByName(List<Album> _albums, string _DirName)
        {
            Album _newalum=null;
            if (_albums != null && _albums.Count > 0)
            {
                foreach (Album _album in _albums)
                {
                    if (_album.AlbumName == _DirName)
                    {
                        _newalum = _album;
                        break;
                    }
                }
            }
            return _newalum;
        }

        /// <summary>
        ///添加照片至相册
        /// </summary>
        /// <param name="Photos"></param>
        /// <param name="_alumobj"></param>
        private void AddPhotosToAlume(List<string> Photos, Album _alumobj)
        {
            if (Photos != null && Photos.Count > 0)
            {
                string strImgPath = "";
                string strImgName = "";
                FileInfo tempfile = null;
                System.Drawing.Image newimg = null;
                string miniImgtempRoot = Path.Combine(rootPath, "miniImgtemp//" + _alumobj.AlbumName);
                if (!Directory.Exists(miniImgtempRoot))
                {
                    Directory.CreateDirectory(miniImgtempRoot);
                }
                string miniPath="";
                PhotoInfo newphoto = null;

                PhotoManager manager=new PhotoManager ();

                #region 逐一存储照片

                foreach (string _strImgName in Photos)
                {
                    miniPath="";
                    strImgPath = _strImgName;
                    if (File.Exists(strImgPath))
                    {
                        tempfile = new FileInfo(strImgPath);
                        miniPath=Path.Combine(miniImgtempRoot,tempfile.Name);//缩略图名称

                        strImgName = tempfile.Name.Substring(0, tempfile.Name.LastIndexOf("."));
                        tempfile = null;

                        newphoto = new PhotoInfo(strImgName,"");
                        newphoto.PhotoData = strImgPath;

                        bool bolstate = ImgManager.Instance.MakeThumbnail(strImgPath, miniPath, 100, 100);
                        if (bolstate)
                        {
                            #region 缩略图处理成功后存储照片信息
                            newphoto.PhotoMiniData = miniPath;
                            ManagerStateMsg msg = manager.AddPhoto(_alumobj.AlbumNO, newphoto);//添加照片至相册 
                           if (msg.Statevalue)
                           {
                               if (AddPhotoEvent != null)
                               {
                                   AddPhotoEvent(_alumobj.AlbumName, strImgName, true);
                               }
                               else
                               {
                                   if (AddPhotoEvent != null)
                                   {
                                       AddPhotoEvent(_alumobj.AlbumName, strImgName, false);
                                   }
                               }
                           }
                            #endregion
                        }
                        else
                        {
                            newphoto = null;
                            if (AddPhotoEvent != null)
                            {
                                AddPhotoEvent(_alumobj.AlbumName, strImgName, false);
                            }
                        }
                        System.Threading.Thread.Sleep(500);
                    }
                }
                #endregion
            }
        }
    }
}
