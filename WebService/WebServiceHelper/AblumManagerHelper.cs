using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PhotoApp.Model;
using PhotoApp.IBLL;
using PhotoApp.SQLitBLL;

namespace WebServiceHelper
{
    /// <summary>
    /// 相册处理帮助类
    /// </summary>
    public class AblumManagerHelper
    {
        /// <summary>
        /// 获取所有相册列表
        /// </summary>
        /// <returns></returns>
        public List<Album> GetAllAlbumlist()
        {
            List<Album> listdata = null;
            IAlbumManager manager = new PhotoApp.SQLitBLL.AlbumManager();
            listdata=manager.GetAlbumList();
            if (listdata != null && listdata.Count > 0)
            {
                foreach (Album _album in listdata)
                {
                    _album.AlbumRootPath = "";
                    _album.AlbumTitleImg = "";
                }
            }
            return listdata;
        }

        /// <summary>
        /// 根据相册编号返回相册详情.包括封面图片数据
        /// </summary>
        /// <param name="_AlbumNo"></param>
        /// <returns></returns>
        public Album GetAlbumInfoByNo(string _AlbumNo)
        {
            Album _album = null;
            try
            {
                IAlbumManager manager = new PhotoApp.SQLitBLL.AlbumManager();
                _album = manager.GetAlbumInfoByNo(_AlbumNo);
                _album.AlbumTitleImg = PhotoApp.SQLitBLL.ImgManager.Instance.GetImgData(_album.AlbumTitleImg, false);//获取缩略图数据
            }
            catch (Exception ex)
            {
            }
            return _album;
        }
    }
}
