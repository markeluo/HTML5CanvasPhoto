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
    /// 照片处理帮助类
    /// </summary>
    public class PhotoManagerHelper
    {
        /// <summary>
        /// 获取照片列表
        /// </summary>
        /// <param name="_AblumNo"></param>
        /// <param name="_page"></param>
        /// <param name="_size"></param>
        /// <returns></returns>
        public List<PhotoInfo> GetPhotosByPage(string _AblumNo, int _page, int _size)
        {
            List<PhotoInfo> photos = null;
            IPhotoManager manager = new PhotoManager();
            photos = manager.GetPagePhotosByAlbum(_page, _size, _AblumNo);
            foreach (PhotoInfo _ph in photos)
            {
                _ph.PhotoData = "";
                _ph.PhotoMiniData = "";
            }
            return photos;
        }

        /// <summary>
        /// 获取照片缩略图数据
        /// </summary>
        /// <param name="_PhotoNo"></param>
        /// <returns></returns>
        public string GetPhotoMiniImgData(string _PhotoNo)
        {
            string strMiniData = "";
            try
            {
                IPhotoManager manager = new PhotoApp.SQLitBLL.PhotoManager();
                PhotoInfo _photo = manager.GetPhotoInfoByNo(_PhotoNo);
                strMiniData = PhotoApp.SQLitBLL.ImgManager.Instance.GetImgData(_photo.PhotoMiniData, false);//获取缩略图数据
            }
            catch (Exception ex)
            { 
            }
            return strMiniData;
        }

        /// <summary>
        /// 获取照片原图数据
        /// </summary>
        /// <param name="_PhotoNo"></param>
        /// <returns></returns>
        public string GetPhotoImgData(string _PhotoNo)
        {
            string strMiniData = "";
            try
            {
                IPhotoManager manager = new PhotoApp.SQLitBLL.PhotoManager();
                PhotoInfo _photo = manager.GetPhotoInfoByNo(_PhotoNo);
                strMiniData = PhotoApp.SQLitBLL.ImgManager.Instance.GetImgData(_photo.PhotoData, false);//获取原图数据
            }
            catch (Exception ex)
            {
            }
            return strMiniData;
        }
    }
}
