using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using PhotoApp.DAL;
using PhotoApp.Model;
using System.Data.SQLite;
using System.Data;

namespace PhotoApp.SQLitBLL
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
            try
            {
                SQLiteParameter[] Ablumpars = new SQLiteParameter[1];
                Ablumpars[0] = new SQLiteParameter("@AlbumNo", DbType.String);
                Ablumpars[0].Value = _photoinfo.PhotoNO;
                string strAblumTols="select count(PhotoNO) as 'PhotoTols' from Photo where AlbumNo=@AlbumNo group by AlbumNo";
                DataTable dt = SQLiteHelper.Instance.ExecuteDataTable(strAblumTols, Ablumpars);
                int AblumPhotosCount = 0;
                if (dt != null && dt.Rows.Count > 0)
                {
                    AblumPhotosCount = Convert.ToInt32(dt.Rows[0]["PhotoTols"]);
                }

                SQLiteParameter[] pars = new SQLiteParameter[9];
                pars[0] = new SQLiteParameter("@PhotoNO", DbType.String);
                pars[0].Value = _photoinfo.PhotoNO;
                pars[1] = new SQLiteParameter("@AlbumNo", DbType.String);
                pars[1].Value = _AlbumNo;
                pars[2] = new SQLiteParameter("@PhotoName", DbType.String);
                pars[2].Value = _photoinfo.PhotoName;
                pars[3] = new SQLiteParameter("@PhotoRemark", DbType.String);
                pars[3].Value = _photoinfo.PhotoRemark;
                pars[4] = new SQLiteParameter("@IsTitleImg", DbType.Int32);
                if (AblumPhotosCount != 0)
                {
                    pars[4].Value = 0;
                }
                else {
                    pars[4].Value = 1;
                }
                
                pars[5] = new SQLiteParameter("@PhotoData", DbType.String);
                pars[5].Value = _photoinfo.PhotoData;
                pars[6] = new SQLiteParameter("@PhotoMiniData", DbType.String);
                pars[6].Value = _photoinfo.PhotoMiniData;
                pars[7] = new SQLiteParameter("@PhotoCreateDate", DbType.String);
                pars[7].Value = _photoinfo.PhotoCreateDate;
                pars[8] = new SQLiteParameter("@PhotoUpdateDate", DbType.String);
                pars[8].Value = _photoinfo.PhotoUpdateDate;

                string strSQL = @"insert into Photo (PhotoNO,AlbumNo,PhotoName,PhotoRemark,IsTitleImg,PhotoData,PhotoMiniData,PhotoCreateDate,PhotoUpdateDate) 
                 values (@PhotoNO,@AlbumNo,@PhotoName,@PhotoRemark,@IsTitleImg,@PhotoData,@PhotoMiniData,@PhotoCreateDate,@PhotoUpdateDate)";
                int _uprows = SQLiteHelper.Instance.ExecuteNonQuery(strSQL, pars);
                if (_uprows > 0)
                {
                    if (AblumPhotosCount == 0)
                    {
                        AlbumManager manager = new AlbumManager();
                        manager.ChangeTitleImg(_AlbumNo, _photoinfo.PhotoNO);
                    }

                    msg.ReturnValue = _photoinfo;
                    msg.Statevalue = true;
                    msg.MsgInfo = "保存成功";
                }
                else
                {
                    msg.MsgInfo = "写入数据库失败！";
                }

                msg.ReturnValue = _photoinfo;
                msg.Statevalue = true;
                msg.MsgInfo = "保存成功";
            }
            catch (Exception ex)
            {
                msg.Statevalue = false;
                msg.MsgInfo = ex.Message;  
            }
            return msg;
        }

        /// <summary>
        /// 根据编号获取照片信息
        /// </summary>
        /// <param name="_strNo"></param>
        /// <returns></returns>
        public PhotoInfo GetPhotoInfoByNo(string _strNo)
        {
            PhotoInfo _photoInfo = null;
            try
            {
                SQLiteParameter[] pars = new SQLiteParameter[1];
                pars[0] = new SQLiteParameter("@PhotoNO", DbType.String);
                pars[0].Value = _strNo;
                string strQuerySQL = @"select * from Photo where PhotoNO=@PhotoNO";
                DataTable _dt = SQLiteHelper.Instance.ExecuteDataTable(strQuerySQL, pars);
                if (_dt != null && _dt.Rows.Count > 0)
                {
                    if (_dt != null && _dt.Rows.Count > 0)
                    {
                        _photoInfo = FormatPhotoByRow(_dt.Rows[0]);
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return _photoInfo;
        }

        private PhotoInfo FormatPhotoByRow(DataRow _row)
        {
            PhotoInfo _photo = null;
            try
            {
                //AlbumNo,AlbumName,AlbumRootPath,AlbumRemark,AlbumTitleImg,CreateDate,UpdateDate,PhotoTols
                if (_row["PhotoNO"] != DBNull.Value && !string.IsNullOrEmpty(_row["PhotoNO"].ToString()))
                {
                    _photo = new PhotoInfo();
                    _photo.PhotoNO = _row["PhotoNO"].ToString();
                    if (_row["PhotoName"] != DBNull.Value)
                    {
                        _photo.PhotoName = _row["PhotoName"].ToString();
                    }
                    if (_row["PhotoRemark"] != DBNull.Value)
                    {
                        _photo.PhotoRemark = _row["PhotoRemark"].ToString();
                    }
                    if (_row["PhotoData"] != DBNull.Value)
                    {
                        _photo.PhotoData = _row["PhotoData"].ToString();
                    }
                    if (_row["PhotoMiniData"] != DBNull.Value)
                    {
                        _photo.PhotoMiniData = _row["PhotoMiniData"].ToString();
                    }
                    if (_row["IsTitleImg"] != DBNull.Value)
                    {
                        if (Convert.ToInt32(_row["IsTitleImg"]) == 1)
                        {
                            _photo.IsTitleImg = true;
                        }
                        else
                        {
                            _photo.IsTitleImg = false;
                        }
                    }
                    if (_row["PhotoCreateDate"] != DBNull.Value)
                    {
                        _photo.PhotoCreateDate = _row["PhotoCreateDate"].ToString();
                    }
                    if (_row["PhotoUpdateDate"] != DBNull.Value)
                    {
                        _photo.PhotoUpdateDate =_row["PhotoUpdateDate"].ToString();
                    }
                }
            }
            catch
            {
            }
            return _photo;
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
            
            try
            {
                SQLiteParameter[] pars = null;
                pars = new SQLiteParameter[4];
                pars[0] = new SQLiteParameter("@PhotoName", DbType.String);
                pars[0].Value = _photoinfo.PhotoName;
                pars[1] = new SQLiteParameter("@PhotoRemark", DbType.String);
                pars[1].Value = _photoinfo.PhotoRemark;
                pars[2] = new SQLiteParameter("@PhotoUpdateDate", DbType.String);
                pars[2].Value = _photoinfo.PhotoUpdateDate;
                pars[3] = new SQLiteParameter("@PhotoNO", DbType.String);
                pars[3].Value = _photoinfo.PhotoNO;

                string strQuerySQL = "update Photo set PhotoName=@PhotoName,PhotoRemark=@PhotoRemark,PhotoUpdateDate=@PhotoUpdateDate where PhotoNO=@PhotoNO";
                int _upRowscount = SQLiteHelper.Instance.ExecuteNonQuery(strQuerySQL, pars);
                if (_upRowscount > 0)
                {
                    msg.MsgInfo = "编辑信息保存成功！";
                    msg.Statevalue = true;
                }
                else
                {
                    msg.MsgInfo = "更新照片信息失败！";
                }
            }
            catch (Exception ex)
            {
                msg.Statevalue = false;
                msg.MsgInfo = ex.Message;
            }
            return msg;
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
            
            try
            {
                SQLiteParameter[] pars = null;
                pars = new SQLiteParameter[1];
                pars[0] = new SQLiteParameter("@PhotoNO", DbType.String);
                pars[0].Value = _photoNo;

                string strQuerySQL = "delete from Photo where PhotoNO=@PhotoNO";
                int _upRowscount = SQLiteHelper.Instance.ExecuteNonQuery(strQuerySQL, pars);
                if (_upRowscount > 0) 
                {
                    strQuerySQL = "select count(PhotoNO) as 'PhotoTols' from Photo where IsTitleImg=1 and AlbumNo=@AlbumNo";
                    SQLiteParameter[] Titleimgpars = new SQLiteParameter[1];
                    Titleimgpars[0] = new SQLiteParameter("@AlbumNo", DbType.String);
                    Titleimgpars[0].Value = _AlbumNo;
                    DataTable _dt = SQLiteHelper.Instance.ExecuteDataTable(strQuerySQL, Titleimgpars);
                    if (_dt != null && _dt.Rows.Count > 0)
                    {
                    }
                    else
                    {
                        _dt = SQLiteHelper.Instance.ExecuteDataTable("select AlbumNo,PhotoNO from Photo order by PhotoCreateDate limit 0,1", null);
                        if (_dt != null && _dt.Rows.Count > 0)
                        {
                            string strPhotoNO = _dt.Rows[0]["PhotoNO"].ToString();
                            string strAlbumNo=_dt.Rows[0]["AlbumNo"].ToString();
                            AlbumManager manager = new AlbumManager();
                            manager.ChangeTitleImg(strAlbumNo, strPhotoNO);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                msg.Statevalue = false;
                msg.MsgInfo = ex.Message;
            }

            return msg;
        }

        /// <summary>
        /// 获取相册照片列表
        /// </summary>
        /// <param name="_AlbumNo"></param>
        /// <returns></returns>
        public List<PhotoInfo> GetPhotosByAlbum(string _AlbumNo)
        {
            List<PhotoInfo> list =null;
            try
            {
                SQLiteParameter[] pars = new SQLiteParameter[1];
                pars[0] = new SQLiteParameter("@AlbumNo", DbType.String);
                pars[0].Value = _AlbumNo;
                string strQuerySQL = @"select * from Photo where AlbumNo=@AlbumNo";
                DataTable _dt = SQLiteHelper.Instance.ExecuteDataTable(strQuerySQL, pars);
                if (_dt != null && _dt.Rows.Count > 0)
                {
                    if (_dt != null && _dt.Rows.Count > 0)
                    {
                        list = new List<PhotoInfo>();
                        PhotoInfo _photoInfo = null;
                        foreach (DataRow _row in _dt.Rows)
                        {
                            _photoInfo = FormatPhotoByRow(_row);
                            if (_photoInfo != null)
                            {
                                list.Add(_photoInfo);
                            }
                        }
                        
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return list;
        }

        /// <summary>
        /// 获取相册照片列表
        /// </summary>
        /// <param name="_page">相册页数</param>
        /// <param name="_size">每页显示照片数</param>
        /// <param name="_AlbumNo">相册编号</param>
        /// <returns></returns>
        public List<PhotoInfo> GetPagePhotosByAlbum(int _page, int _size, string _AlbumNo) 
        {
            List<PhotoInfo> listphotos = null;
            try
            {
                SQLiteParameter[] pars = new SQLiteParameter[1];
                pars[0] = new SQLiteParameter("@AlbumNo", DbType.String);
                pars[0].Value = _AlbumNo;
                int StartIndex = 0;
                int PageSize = _size;
                if (_page != 1)
                {
                    StartIndex = (_page - 1) * _size;
                }
                string strQuerySQL = string.Format("select * from Photo where AlbumNo=@AlbumNo order by PhotoCreateDate limit {0},{1}", StartIndex, PageSize);
                DataTable _dt = SQLiteHelper.Instance.ExecuteDataTable(strQuerySQL, pars);
                if (_dt != null && _dt.Rows.Count > 0)
                {
                    if (_dt != null && _dt.Rows.Count > 0)
                    {
                        listphotos = new List<PhotoInfo>();
                        PhotoInfo _photoInfo = null;
                        foreach (DataRow _row in _dt.Rows)
                        {
                            _photoInfo = FormatPhotoByRow(_row);
                            if (_photoInfo != null)
                            {
                                listphotos.Add(_photoInfo);
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
            }
            return listphotos;
        }
    }
}