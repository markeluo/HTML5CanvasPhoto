using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhotoApp.Model
{
    /// <summary>
    /// 照片信息
    /// </summary>
    public class PhotoInfo
    {
        public PhotoInfo()
        { 
        }
        /// <summary>
        /// 照片信息构造函数
        /// </summary>
        /// <param name="_Name"></param>
        /// <param name="_Remark"></param>
        public PhotoInfo(string _Name, string _Remark)
        {
            this.photoName = _Name;
            this.photoRemark = _Remark;
            this.photoNO = GUIDInfo.Instance.GetGUID();//生成相册编号
            this.photoCreateDate = this.photoUpdateDate = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
        }
        /// <summary>
        /// 照片编号
        /// </summary>
        private string photoNO;
        /// <summary>
        /// 照片编号
        /// </summary>
        public string PhotoNO
        {
            get { return photoNO; }
            set { photoNO = value; }
        }
        /// <summary>
        /// 照片名称
        /// </summary>
        private string photoName;
        /// <summary>
        /// 照片名称
        /// </summary>
        public string PhotoName
        {
            get { return photoName; }
            set { photoName = value; }
        }
        /// <summary>
        /// 照片备注/注释
        /// </summary>
        private string photoRemark;
        /// <summary>
        /// 照片备注/注释
        /// </summary>
        public string PhotoRemark
        {
            get { return photoRemark; }
            set { photoRemark = value; }
        }
        private bool isTitleImg;
        /// <summary>
        /// 是否为封面
        /// </summary>
        public bool IsTitleImg
        {
            get { return isTitleImg; }
            set { isTitleImg = value; }
        }
        /// <summary>
        /// 照片数据/路径
        /// </summary>
        private string photoData;
        /// <summary>
        /// 照片数据/路径
        /// </summary>
        public string PhotoData
        {
            get { return photoData; }
            set { photoData = value; }
        }
        /// <summary>
        /// 缩略图数据/路径
        /// </summary>
        private string photoMiniData;
        /// <summary>
        /// 缩略图数据/路径
        /// </summary>
        public string PhotoMiniData
        {
            get { return photoMiniData; }
            set { photoMiniData = value; }
        }
        /// <summary>
        /// 照片创建日期
        /// </summary>
        private string  photoCreateDate;
        /// <summary>
        /// 照片创建日期
        /// </summary>
        public string  PhotoCreateDate
        {
            get { return photoCreateDate; }
            set { photoCreateDate = value; }
        }
        /// <summary>
        /// 照片更新日期
        /// </summary>
        private string  photoUpdateDate;
        /// <summary>
        /// 照片更新日期
        /// </summary>
        public string  PhotoUpdateDate
        {
            get { return photoUpdateDate; }
            set { photoUpdateDate = value; }
        }
    }
}
