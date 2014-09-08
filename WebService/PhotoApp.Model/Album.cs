using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhotoApp.Model
{
    /// <summary>
    /// 相册
    /// </summary>
    public class Album
    {
        public Album()
        { 
        }
        /// <summary>
        /// 相册构造函数
        /// </summary>
        /// <param name="_Name">名称</param>
        /// <param name="_Remark">备注</param>
        public Album(string _Name, string _Remark)
        {
            albumName = _Name;
            albumRemark = _Remark;
            albumNO = GUIDInfo.Instance.GetGUID();//生成相册编号
            createDate =updateDate= DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
        }
        /// <summary>
        /// 相册编号
        /// </summary>
        private string albumNO;
        /// <summary>
        /// 相册编号
        /// </summary>
        public string AlbumNO
        {
            get { return albumNO; }
            set { albumNO = value; }
        }
        /// <summary>
        /// 相册目录
        /// </summary>
        private string albumRootPath;
        /// <summary>
        /// 相册目录
        /// </summary>
        public string AlbumRootPath
        {
            get { return albumRootPath; }
            set { albumRootPath = value; }
        }

        /// <summary>
        /// 相册名称
        /// </summary>
        private string albumName;
        /// <summary>
        /// 相册名称
        /// </summary>
        public string AlbumName
        {
            get { return albumName; }
            set { albumName = value; }
        }
        /// <summary>
        /// 相册备注/注释
        /// </summary>
        private string albumRemark;
        /// <summary>
        /// 相册备注/注释
        /// </summary>
        public string AlbumRemark
        {
            get { return albumRemark; }
            set { albumRemark = value; }
        }
        /// <summary>
        /// 相册封面
        /// </summary>
        private string albumTitleImg;
        /// <summary>
        /// 相册封面图片
        /// </summary>
        public string AlbumTitleImg
        {
            get { return albumTitleImg; }
            set { albumTitleImg = value; }
        }

        /// <summary>
        /// 相片总数
        /// </summary>
        private int photosNumber;
        /// <summary>
        /// 相片总数
        /// </summary>
        public int PhotosNumber
        {
            get { return photosNumber; }
            set { photosNumber = value; }
        }
        /// <summary>
        /// 创建日期
        /// </summary>
        private string  createDate;
        /// <summary>
        /// 创建日期
        /// </summary>
        public string CreateDate
        {
            get { return createDate; }
            set { createDate = value; }
        }
        /// <summary>
        /// 更新日期
        /// </summary>
        private string updateDate;
        /// <summary>
        /// 更新日期
        /// </summary>
        public string UpdateDate
        {
            get { return updateDate; }
            set { updateDate = value; }
        }

        /// <summary>
        /// 照片列表
        /// </summary>
        private List<PhotoInfo> subPhotos;
        /// <summary>
        /// 包含照片列表
        /// </summary>
        public List<PhotoInfo> SubPhotos
        {
            get { return subPhotos; }
            set { subPhotos = value; }
        }
    }
}
