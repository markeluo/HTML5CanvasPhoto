using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using Db4objects.Db4o;
using PhotoApp.DAL;
using PhotoApp.Model;

namespace PhotoApp.BLL
{
    public class PhotoManager:IBLL.IPhotoManager
    {
        /// <summary>
        /// 添加照片
        /// </summary>
        /// <param name="_AlbumNo">照片编号</param>
        /// <param name="_photoinfo">照片信息</param>
        /// <returns></returns>
        public Model.ManagerStateMsg AddPhoto(string _AlbumNo, Model.PhotoInfo _photoinfo)
        {
            Model.ManagerStateMsg msg = new Model.ManagerStateMsg(false, "方法 AddPhoto 执行出现错误！");
            IObjectContainer db = DataHelper.Instance.GetDbObj(typeof(RootAlbum));
            try
            {
                IObjectSet _result = db.Query(typeof(RootAlbum));
                RootAlbum root = (RootAlbum)_result.Next();
                if (root.SubAlbums != null && root.SubAlbums.Count > 0)
                {
                    foreach (Album _abm in root.SubAlbums)
                    {
                        if (_abm.AlbumNO == _AlbumNo)
                        {
                            if (_abm.SubPhotos == null)
                            {
                                _abm.SubPhotos = new List<PhotoInfo>();
                            }
                            if (_abm.SubPhotos.Count == 0)
                            {
                                _abm.AlbumTitleImg = _photoinfo.PhotoMiniData;
                                _photoinfo.IsTitleImg = true;
                            }
                            _abm.SubPhotos.Add(_photoinfo);
                            break;
                        }
                    }
                    db.Store(root);
                    db.Commit();

                    msg.ReturnValue = _photoinfo;
                    msg.Statevalue = true;
                    msg.MsgInfo = "保存成功";
                }
                else
                {
                    msg.Statevalue = false;
                    msg.MsgInfo = "不存在任何相册！";  
                }
            }
            catch (Exception ex)
            {
                msg.Statevalue = false;
                msg.MsgInfo = ex.Message;  
            }
            finally
            {
                db.Close();
            }
            return msg;
        }

        /// <summary>
        /// 编辑照片信息
        /// </summary>
        /// <param name="_AlbumNo"></param>
        /// <param name="_photoinfo"></param>
        /// <returns></returns>
        public Model.ManagerStateMsg EditPhoto(string _AlbumNo, Model.PhotoInfo _photoinfo)
        {
            Model.ManagerStateMsg msg = new Model.ManagerStateMsg(false, "方法 EditPhoto 执行出现错误！");
            IObjectContainer db = DataHelper.Instance.GetDbObj(typeof(RootAlbum));
            try
            {
                IObjectSet _result = db.Query(typeof(RootAlbum));
                RootAlbum root = (RootAlbum)_result.Next();
                if (root.SubAlbums != null && root.SubAlbums.Count > 0)
                {
                    foreach (Album _abm in root.SubAlbums)
                    {
                        if (_abm.AlbumNO == _AlbumNo)
                        {
                            int Imgindex = GetPhotoIndex(_abm.SubPhotos, _photoinfo.PhotoName);//返回照片索引
                            if (Imgindex >-1)
                            {
                                _abm.SubPhotos[Imgindex].PhotoName = _photoinfo.PhotoName;
                                _abm.SubPhotos[Imgindex].PhotoRemark = _photoinfo.PhotoRemark;
                                _abm.UpdateDate = DateTime.Now;

                                msg.Statevalue = true;
                                msg.MsgInfo = "编辑信息保存 成功！";
                            }
                            else
                            {
                                msg.Statevalue = false;
                                msg.MsgInfo = "未找到符合条件的照片！";
                            }

                            break;
                        }
                    }
                    if (msg.Statevalue)
                    {
                        db.Store(root);//保存
                        db.Commit();
                    }
                }
                else
                {
                    msg.Statevalue = false;
                    msg.MsgInfo = "不存在任何相册！";
                }
            }
            catch (Exception ex)
            {
                msg.Statevalue = false;
                msg.MsgInfo = ex.Message;
            }
            finally
            {
                db.Close();
            }
            return msg;
        }

        /// <summary>
        /// 根据名称返回图片索引值
        /// </summary>
        /// <param name="_photos"></param>
        /// <param name="_photoName"></param>
        /// <returns></returns>
        private int GetPhotoIndex(List<PhotoInfo> _photos, string _photoName)
        {
            int photoInex = -1;
            if (_photos != null && _photos.Count > 0)
            {
                for (int i = 0; i < _photos.Count; i++)
                {
                    if (_photos[i].PhotoName == _photoName)
                    {
                        photoInex = i;
                        break;
                    }
                }
            }
            return photoInex;
        }

        /// <summary>
        /// 删除照片
        /// </summary>
        /// <param name="_AlbumNo"></param>
        /// <param name="_photoNo"></param>
        /// <returns></returns>
        public Model.ManagerStateMsg DeletePhoto(string _AlbumNo, string _photoNo)
        {
            Model.ManagerStateMsg msg = new Model.ManagerStateMsg(false, "方法 EditPhoto 执行出现错误！");
            IObjectContainer db = DataHelper.Instance.GetDbObj(typeof(RootAlbum));
            try
            {
                IObjectSet _result = db.Query(typeof(RootAlbum));
                RootAlbum root = (RootAlbum)_result.Next();
                if (root.SubAlbums != null && root.SubAlbums.Count > 0)
                {
                    
                    foreach (Album _abm in root.SubAlbums)
                    {
                        if (_abm.AlbumNO == _AlbumNo)
                        {
                            int Imgindex = GetPhotoIndex(_abm.SubPhotos, _photoNo);//返回照片索引
                            if (Imgindex > -1)
                            {
                                bool bolistitleimg = false;
                                if (_abm.SubPhotos[Imgindex].IsTitleImg)
                                {
                                    bolistitleimg = true;
                                }
                                _abm.SubPhotos.RemoveAt(Imgindex);//删除照片
                                if (_abm.SubPhotos.Count > 0)
                                {
                                    if (bolistitleimg)
                                    {
                                        _abm.AlbumTitleImg = _abm.SubPhotos[0].PhotoMiniData;
                                    }
                                }
                                else
                                {
                                    _abm.AlbumTitleImg = "";
                                }

                                msg.Statevalue = true;
                                msg.MsgInfo = "删除成功！";
                            }
                            else
                            {
                                msg.Statevalue = false;
                                msg.MsgInfo = "未找到符合条件的照片！";
                            }

                            break;
                        }
                    }
                    if (msg.Statevalue)
                    {
                        db.Store(root);//保存
                        db.Commit();
                    }
                }
                else
                {
                    msg.Statevalue = false;
                    msg.MsgInfo = "不存在任何相册！";
                }
            }
            catch (Exception ex)
            {
                msg.Statevalue = false;
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