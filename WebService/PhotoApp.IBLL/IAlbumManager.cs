using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PhotoApp.Model;

namespace PhotoApp.IBLL
{
    /// <summary>
    /// 相册管理_接口
    /// </summary>
    public interface IAlbumManager
    {
        /// <summary>
        /// 创建相册
        /// </summary>
        /// <param name="_albuminfo"></param>
        /// <returns></returns>
        ManagerStateMsg SaveAlbum(Album _albuminfo);

        /// <summary>
        /// 根据相册编号返回相册详情
        /// </summary>
        /// <param name="_albumNo">相册编号</param>
        /// <returns></returns>
        Album GetAlbumInfoByNo(string _albumNo);

        /// <summary>
        /// 编辑相册信息
        /// </summary>
        /// <param name="_albuminfo"></param>
        /// <returns></returns>
        ManagerStateMsg EditAlbum(Album _albuminfo);

        /// <summary>
        /// 更改相册封面
        /// </summary>
        /// <param name="_albumNo">相册编号</param>
        /// <param name="_photoNo">封面照片编号</param>
        /// <returns></returns>
        ManagerStateMsg ChangeTitleImg(string _albumNo, string _photoNo);

         /// <summary>
        /// 同名相册是否已存在
        /// </summary>
        /// <param name="_AlbumName">相册名称</param>
        /// <returns></returns>
        bool AlbumBolIsExt(string _AlbumName);

        /// <summary>
        /// 获取相册列表
        /// </summary>
        /// <returns></returns>
        List<Album> GetAlbumList();

        /// <summary>
        /// 删除相册
        /// </summary>
        /// <param name="_albumNo">相册编号</param>
        /// <returns></returns>
        ManagerStateMsg DeleteAlbum(string _albumNo);
    }
}
