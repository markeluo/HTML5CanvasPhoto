using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PhotoApp.Model;
using PhotoApp.IBLL;
using PhotoApp.DAL;
using Db4objects.Db4o;

namespace PhotoApp.BLL
{
    /// <summary>
    /// 相册管理方法
    /// </summary>
    public class AlbumManager:IAlbumManager
    {
        /// <summary>
        /// 创建相册
        /// </summary>
        /// <param name="_albuminfo"></param>
        /// <returns></returns>
        public ManagerStateMsg SaveAlbum(Album _albuminfo)
        {
            bool albumIsExtbol = AlbumBolIsExt(_albuminfo.AlbumName);//相册是否已存在
            ManagerStateMsg msg = new ManagerStateMsg(false, "");
            if (!albumIsExtbol)
            {
                IObjectContainer db = DataHelper.Instance.GetDbObj(typeof(RootAlbum));
                try
                {
                    IObjectSet _result = db.Query(typeof(RootAlbum));
                    RootAlbum root = (RootAlbum)_result.Next();
                    if (root.SubAlbums != null && root.SubAlbums.Count > 0)
                    {
                        root.SubAlbums.Add(_albuminfo);
                    }
                    else
                    {
                        root.SubAlbums = new List<Album>();
                        root.SubAlbums.Add(_albuminfo);
                    }
                    db.Store(root);
                    db.Commit();
                    msg.ReturnValue = _albuminfo;
                    msg.Statevalue = true;
                    msg.MsgInfo = "保存成功";
                }
                catch (Exception ex)
                {
                }
                finally
                {
                    db.Close();
                }
            }
            else
            {
                msg.MsgInfo = "同名相册已存在！";
            }

            return msg;
        }

        /// <summary>
        /// 同名相册是否已存在
        /// </summary>
        /// <param name="_AlbumName">相册名称</param>
        /// <returns></returns>
        public bool AlbumBolIsExt(string _AlbumName) 
        {
            bool bolIsExt = false;
            IObjectContainer db = DataHelper.Instance.GetDbObj(typeof(RootAlbum));
            try
            {
                IObjectSet _result = db.Query(typeof(RootAlbum));
                RootAlbum root = (RootAlbum)_result.Next();
                if (root.SubAlbums != null && root.SubAlbums.Count > 0)
                {
                    foreach (Album _albuminfo in root.SubAlbums)
                    {
                        if (_albuminfo.AlbumName.Trim().ToLower() == _AlbumName.Trim().ToLower())
                        {
                            bolIsExt = true;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                db.Close();
            }
            return bolIsExt;
        }

        /// <summary>
        /// 获取相册列表
        /// </summary>
        /// <returns></returns>
        public List<Album> GetAlbumList()
        {
            List<Album> Albums = new List<Album>();
            IObjectContainer db = DataHelper.Instance.GetDbObj(typeof(RootAlbum));
            try
            {
                IObjectSet _result = db.Query(typeof(RootAlbum));
                RootAlbum root = (RootAlbum)_result.Next();
                if (root.SubAlbums != null && root.SubAlbums.Count > 0)
                {
                    Albums = root.SubAlbums;
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                db.Close();
            }
            return Albums;
        }

        /// <summary>
        /// 编辑相册信息
        /// </summary>
        /// <param name="_albuminfo">相册信息</param>
        /// <returns></returns>
        public ManagerStateMsg EditAlbum(Album _albuminfo)
        {
            ManagerStateMsg msg = new ManagerStateMsg(false, "方法 EditAlbum 执行出现错误！");
            IObjectContainer db = DataHelper.Instance.GetDbObj(typeof(RootAlbum));
            try
            {
                IObjectSet _result = db.Query(typeof(RootAlbum));
                RootAlbum root = (RootAlbum)_result.Next();
                if (root.SubAlbums != null && root.SubAlbums.Count > 0)
                {
                    foreach (Album _album in root.SubAlbums)
                    {
                        if (_album.AlbumNO == _albuminfo.AlbumNO)
                        {
                            _album.AlbumName = _albuminfo.AlbumName;
                            _album.AlbumRemark = _albuminfo.AlbumRemark;
                            _album.UpdateDate = DateTime.Now;

                            db.Store(root);
                            db.Commit();

                            msg.MsgInfo = "相册信息更新成功！";
                            msg.Statevalue = true;
                            break;
                        }
                    }
                }
                if (msg.Statevalue)
                {
                }
                else
                {
                    msg.MsgInfo = "未找到符合条件的相册！";
                }
            }
            catch (Exception ex)
            {
                msg.MsgInfo = ex.Message;
            }
            finally
            {
                db.Close();
            }

            return msg;
        }

        /// <summary>
        /// 更改相册封面
        /// </summary>
        /// <param name="_albumNo">相册编号</param>
        /// <param name="_photoNo">封面照片编号</param>
        /// <returns></returns>
        public ManagerStateMsg ChangeTitleImg(string _albumNo, string _photoNo)
        {
            ManagerStateMsg msg = new ManagerStateMsg(false, "方法 ChangeTitleImg 执行出现错误！");
            IObjectContainer db = DataHelper.Instance.GetDbObj(typeof(RootAlbum));
            try
            {
                IObjectSet _result = db.Query(typeof(RootAlbum));
                RootAlbum root = (RootAlbum)_result.Next();
                if (root.SubAlbums != null && root.SubAlbums.Count > 0)
                {
                    foreach (Album _album in root.SubAlbums)
                    {
                        if (_album.AlbumNO == _albumNo)
                        {
                            ChangeAlbumTitleImg(_album, _photoNo);//更新相册封面
                            
                            db.Store(root);
                            db.Commit();

                            msg.MsgInfo = "相册首页更新成功！";
                            msg.Statevalue = true;
                            break;
                        }
                    }
                }
                if (msg.Statevalue)
                {
                }
                else
                {
                    msg.MsgInfo = "未找到符合条件的相册！";
                }
            }
            catch (Exception ex)
            {
                msg.MsgInfo = ex.Message;
            }
            finally
            {
                db.Close();
            }

            return msg;
        }

        /// <summary>
        /// 更改相册封面图片
        /// </summary>
        /// <param name="_album">相册</param>
        /// <param name="_titleimgNo">图片编号</param>
        private void ChangeAlbumTitleImg(Album _album, string _titleimgNo)
        {
            if (_album != null && _album.SubPhotos != null && _album.SubPhotos.Count > 0)
            {
                if (string.IsNullOrEmpty(_titleimgNo))
                {
                    _album.AlbumTitleImg = _album.SubPhotos[0].PhotoMiniData;
                }
                else
                {
                    foreach (PhotoInfo _photo in _album.SubPhotos)
                    {
                        if (_photo.PhotoNO == _titleimgNo)
                        {
                            _album.AlbumTitleImg = _photo.PhotoMiniData;
                            _photo.IsTitleImg = true;
                        }
                        else
                        {
                            _photo.IsTitleImg = false;
                        }
                    }
                }
            }
            else
            {
                _album.AlbumTitleImg = "";
            }
        }

        /// <summary>
        /// 删除相册
        /// </summary>
        /// <param name="_albumNo"></param>
        /// <returns></returns>
        public ManagerStateMsg DeleteAlbum(string _albumNo)
        {

            ManagerStateMsg msg = new ManagerStateMsg(false, "方法 DeleteAlbum 执行出现错误！");
            IObjectContainer db = DataHelper.Instance.GetDbObj(typeof(RootAlbum));
            try
            {
                IObjectSet _result = db.Query(typeof(RootAlbum));
                RootAlbum root = (RootAlbum)_result.Next();
                if (root.SubAlbums != null && root.SubAlbums.Count > 0)
                {
                    foreach (Album _album in root.SubAlbums)
                    {
                        if (_album.AlbumNO == _albumNo)
                        {
                            root.SubAlbums.Remove(_album);
                            
                            msg.MsgInfo = "相册删除成功！";
                            msg.Statevalue = true;
                            break;
                        }
                    }
                    db.Store(root);
                    db.Commit();
                }
                if (msg.Statevalue)
                {
                }
                else
                {
                    msg.MsgInfo = "未找到符合条件的相册！";
                }
            }
            catch (Exception ex)
            {
                msg.MsgInfo = ex.Message;
            }
            finally
            {
                db.Close();
            }

            return msg;
        }
    }
}
