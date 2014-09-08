using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PhotoApp.Model;
using PhotoApp.IBLL;
using PhotoApp.DAL;
using System.Data.SQLite;
using System.Data;

namespace PhotoApp.SQLitBLL
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
                SQLiteParameter[] pars = null;
                try
                {
                    pars = new SQLiteParameter[7];
                    pars[0] = new SQLiteParameter("@AlbumNo", DbType.String);
                    pars[0].Value = _albuminfo.AlbumNO;
                    pars[1] = new SQLiteParameter("@AlbumName", DbType.String);
                    pars[1].Value = _albuminfo.AlbumName;
                    pars[2] = new SQLiteParameter("@AlbumRootPath", DbType.String);
                    pars[2].Value = _albuminfo.AlbumRootPath;
                    pars[3] = new SQLiteParameter("@AlbumRemark", DbType.String);
                    pars[3].Value = _albuminfo.AlbumRemark;
                    pars[4] = new SQLiteParameter("@AlbumTitleImg", DbType.String);
                    pars[4].Value = _albuminfo.AlbumTitleImg;
                    pars[5] = new SQLiteParameter("@CreateDate", DbType.String);
                    pars[5].Value = _albuminfo.CreateDate;
                    pars[6] = new SQLiteParameter("@UpdateDate", DbType.String);
                    pars[6].Value = _albuminfo.UpdateDate;

                    string strSQL = "insert into Album (AlbumNo,AlbumName,AlbumRootPath,AlbumRemark,AlbumTitleImg,CreateDate,UpdateDate) values (@AlbumNo,@AlbumName,@AlbumRootPath,@AlbumRemark,@AlbumTitleImg,@CreateDate,@UpdateDate)";
                    int _uprows=SQLiteHelper.Instance.ExecuteNonQuery(strSQL, pars);
                    if (_uprows > 0)
                    {
                        msg.ReturnValue = _albuminfo;
                        msg.Statevalue = true;
                        msg.MsgInfo = "保存成功";
                    }
                    else
                    {
                        msg.MsgInfo = "写入数据库失败！";
                    }
                }
                catch (Exception ex)
                {
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
            SQLiteParameter[] pars = null;
            try
            {
                pars = new SQLiteParameter[1];
                pars[0] = new SQLiteParameter("@AlbumName", DbType.String);
                pars[0].Value = _AlbumName;
                string strSQL = "select AlbumNo from Album where AlbumName=@AlbumName";
                DataTable _dt = SQLiteHelper.Instance.ExecuteDataTable(strSQL, pars);
                if (_dt != null && _dt.Rows.Count > 0)
                {
                    bolIsExt = true;
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
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
            
            try
            {
                string strQuerySQL = @"select Album.[AlbumNo] as 'AlbumNo',AlbumName,AlbumRootPath,AlbumRemark,AlbumTitleImg,Album.CreateDate as 'CreateDate',Album.UpdateDate as 'UpdateDate',count(Photo.[AlbumNo]) as 'PhotoTols' 
                 from Album,Photo where Album.[AlbumNo]=Photo.[AlbumNo] group by Album.[AlbumNo]";
                DataTable _dt = SQLiteHelper.Instance.ExecuteDataTable(strQuerySQL, null);
                if (_dt != null && _dt.Rows.Count > 0)
                {
                    if (_dt != null && _dt.Rows.Count > 0)
                    {
                        Album _albumInfo = null;
                        foreach (DataRow _row in _dt.Rows)
                        {
                            _albumInfo = FormatAlbumByRow(_row);
                            if (_albumInfo != null)
                            {
                                Albums.Add(_albumInfo);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return Albums;
        }

        /// <summary>
        /// 格式化相册信息
        /// </summary>
        /// <param name="_row"></param>
        /// <returns></returns>
        private Album FormatAlbumByRow(DataRow _row)
        {
            Album _album = null;
            try
            {
                //AlbumNo,AlbumName,AlbumRootPath,AlbumRemark,AlbumTitleImg,CreateDate,UpdateDate,PhotoTols
                if (_row["AlbumNo"] != DBNull.Value && !string.IsNullOrEmpty(_row["AlbumNo"].ToString()))
                {
                    _album = new Album();
                    _album.AlbumNO = _row["AlbumNo"].ToString();
                    if (_row["AlbumName"] != DBNull.Value)
                    {
                        _album.AlbumName = _row["AlbumName"].ToString();
                    }
                    if (_row["AlbumRootPath"] != DBNull.Value)
                    {
                        _album.AlbumRootPath = _row["AlbumRootPath"].ToString();
                    }
                    if (_row["AlbumRemark"] != DBNull.Value)
                    {
                        _album.AlbumRemark = _row["AlbumRemark"].ToString();
                    }
                    if (_row["AlbumTitleImg"] != DBNull.Value)
                    {
                        _album.AlbumTitleImg = _row["AlbumTitleImg"].ToString();
                    }
                    if (_row["CreateDate"] != DBNull.Value)
                    {
                        _album.CreateDate = _row["CreateDate"].ToString();
                    }
                    if (_row["UpdateDate"] != DBNull.Value)
                    {
                        _album.UpdateDate = _row["UpdateDate"].ToString();
                    }
                    if (_row["PhotoTols"] != DBNull.Value)
                    {
                        _album.PhotosNumber = Convert.ToInt32(_row["PhotoTols"]);
                    }
                }
            }
            catch
            { 
            }
            return _album;
        }

        /// <summary>
        /// 编辑相册信息
        /// </summary>
        /// <param name="_albuminfo">相册信息</param>
        /// <returns></returns>
        public ManagerStateMsg EditAlbum(Album _albuminfo)
        {
            ManagerStateMsg msg = new ManagerStateMsg(false, "方法 EditAlbum 执行出现错误！");
            try
            {
                SQLiteParameter[] pars = null;
                pars = new SQLiteParameter[4];
                pars[0] = new SQLiteParameter("@AlbumNo", DbType.String);
                pars[0].Value = _albuminfo.AlbumNO;
                pars[1] = new SQLiteParameter("@AlbumName", DbType.String);
                pars[1].Value = _albuminfo.AlbumName;
                pars[2] = new SQLiteParameter("@AlbumRemark", DbType.String);
                pars[2].Value = _albuminfo.AlbumRemark;
                pars[3] = new SQLiteParameter("@UpdateDate", DbType.String);
                pars[3].Value = _albuminfo.UpdateDate;

                string strQuerySQL ="update Album set AlbumName=@AlbumName,AlbumRemark=@AlbumRemark,UpdateDate=@UpdateDate where AlbumNo=@AlbumNo";
                int _upRowscount= SQLiteHelper.Instance.ExecuteNonQuery(strQuerySQL, pars);
                if (_upRowscount>0)
                {
                    msg.MsgInfo = "相册信息更新成功！";
                    msg.Statevalue = true;
                }
                else
                {
                    msg.MsgInfo = "更新相册信息失败！";
                }
            }
            catch (Exception ex)
            {
                msg.MsgInfo = ex.Message;
            }
            return msg;
        }

        /// <summary>
        /// 根据相册编号返回相册信息
        /// </summary>
        /// <param name="_AlbumNo"></param>
        /// <returns></returns>
        public Album GetAlbumInfoByNo(string _AlbumNo)
        {
            Album _album = null;
            try
            {
                SQLiteParameter[] pars = new SQLiteParameter[1];
                pars[0] = new SQLiteParameter("@AlbumNo", DbType.String);
                pars[0].Value = _AlbumNo;
                string strQuerySQL = @"select Photo.[AlbumNo] as 'AlbumNo',AlbumName,AlbumRootPath,AlbumRemark,AlbumTitleImg,Album.CreateDate as 'CreateDate',Album.UpdateDate as 'UpdateDate',count(Photo.[AlbumNo]) as 'PhotoTols' 
                 from Album,Photo where Album.[AlbumNo]=Photo.[AlbumNo] and Album.[AlbumNo]=@AlbumNo group by Photo.[AlbumNo]";
                DataTable _dt = SQLiteHelper.Instance.ExecuteDataTable(strQuerySQL, pars);
                if (_dt != null && _dt.Rows.Count > 0)
                {
                    if (_dt != null && _dt.Rows.Count > 0)
                    {
                        _album= FormatAlbumByRow(_dt.Rows[0]);
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return _album;
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
            try
            {
                PhotoManager manager = new PhotoManager();
                PhotoInfo _photo = manager.GetPhotoInfoByNo(_photoNo);//获取照片信息
                if (_photo!=null)
                {
                    string strUpALLPhotoSQL = "update Photo set IsTitleImg=0 where AlbumNo=@AlbumNo";
                    SQLiteParameter[] photopars = new SQLiteParameter[1];
                    photopars[0] = new SQLiteParameter("@AlbumNo", DbType.String);
                    photopars[0].Value = _albumNo;

                    int uprowscount = SQLiteHelper.Instance.ExecuteNonQuery(strUpALLPhotoSQL, photopars);
                    photopars = null;
                    if (uprowscount > 0)
                    {
                        string strQuerystr = "update Album set AlbumTitleImg=@AlbumTitleImg where AlbumNo=@AlbumNo;update Photo set IsTitleImg=1 where PhotoNO=@photoNO";
                        SQLiteParameter[] pars = new SQLiteParameter[3];
                        pars[0] = new SQLiteParameter("@AlbumTitleImg", DbType.String);
                        pars[0].Value = _photo.PhotoMiniData;
                        pars[1] = new SQLiteParameter("@AlbumNo", DbType.String);
                        pars[1].Value = _albumNo;
                        pars[2] = new SQLiteParameter("@PhotoNO", DbType.String);
                        pars[2].Value = _photoNo;
                        uprowscount = SQLiteHelper.Instance.ExecuteNonQuery(strQuerystr, pars);
                        pars = null;
                        if (uprowscount > 0)
                        {
                            msg.Statevalue = true;
                            msg.MsgInfo = "更新成功！";
                        }
                        else
                        {
                            msg.MsgInfo = "更新失败！";
                        }
                    }
                    else
                    {
                        msg.MsgInfo = "恢复所有照片的首页状态失败！";
                    }

                }
                else
                {
                    msg.MsgInfo = "未找到符合条件的照片信息！";
                }
               
                
                msg.Statevalue = true;
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

            return msg;
        }
 
        /// <summary>
        /// 删除相册
        /// </summary>
        /// <param name="_albumNo"></param>
        /// <returns></returns>
        public ManagerStateMsg DeleteAlbum(string _albumNo)
        {

            ManagerStateMsg msg = new ManagerStateMsg(false, "方法 DeleteAlbum 执行出现错误！");
            try
            {
                string strQuerystr = "delete from Album where AlbumNo=@AlbumNo";
                SQLiteParameter[] pars = new SQLiteParameter[1];
                pars[0] = new SQLiteParameter("@AlbumNo", DbType.String);
                pars[0].Value =_albumNo;
                int uprowscount=SQLiteHelper.Instance.ExecuteNonQuery(strQuerystr,pars);
                if(uprowscount>0)
                {
                    msg.MsgInfo = "相册删除成功！";
                    msg.Statevalue = true;
                }
                else
                {
                    msg.Statevalue = true;
                    msg.MsgInfo = "未找到符合条件的相册！";
                }
            }
            catch (Exception ex)
            {
                msg.MsgInfo = ex.Message;
            }

            return msg;
        }
    }
}
