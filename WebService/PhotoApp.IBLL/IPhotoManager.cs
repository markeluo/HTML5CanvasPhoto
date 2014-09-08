using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PhotoApp.Model;

namespace PhotoApp.IBLL
{
    /// <summary>
    /// 照片管理接口
    /// </summary>
    public interface IPhotoManager
    {
        /// <summary>
        /// 添加照片
        /// </summary>
        /// <param name="_AlbumNo">相册编号</param>
        /// <param name="_photoinfo">照片信息</param>
        /// <returns></returns>
        ManagerStateMsg AddPhoto(string _AlbumNo,PhotoApp.Model.PhotoInfo _photoinfo);
        /// <summary>
        /// 编辑照片信息
        /// </summary>
        /// <param name="_AlbumNo">相册编号</param>
        /// <param name="_photoinfo"></param>
        /// <returns></returns>
        ManagerStateMsg EditPhoto(string _AlbumNo, PhotoApp.Model.PhotoInfo _photoinfo);

        /// <summary>
        /// 根据照片编号返回照片信息
        /// </summary>
        /// <param name="_PhotoNo"></param>
        /// <returns></returns>
        PhotoInfo GetPhotoInfoByNo(string _PhotoNo);
        
        /// <summary>
        /// 删除照片
        /// </summary>
        /// <param name="_AlbumNo">相册编号</param>
        /// <param name="_photoNo">照片编号</param>
        /// <returns></returns>
        ManagerStateMsg DeletePhoto(string _AlbumNo, string _photoNo);

        /// <summary>
        /// 获取相册照片列表
        /// </summary>
        /// <param name="_AlbumNo"></param>
        /// <returns></returns>
        List<PhotoInfo> GetPhotosByAlbum(string _AlbumNo);

        /// <summary>
        /// 获取相册照片列表
        /// </summary>
        /// <param name="_page">相册页数</param>
        /// <param name="_size">每页显示照片数</param>
        /// <param name="_AlbumNo">相册编号</param>
        /// <returns></returns>
        List<PhotoInfo> GetPagePhotosByAlbum(int _page, int _size, string _AlbumNo);
    }
}
