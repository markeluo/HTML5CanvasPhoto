using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhotoApp.Model
{
    /// <summary>
    /// 根目录
    /// </summary>
    public class RootAlbum
    {
        private List<Album> subAlbums;
        /// <summary>
        /// 子相册列表
        /// </summary>
        public List<Album> SubAlbums
        {
            get { return subAlbums; }
            set { subAlbums = value; }
        }
        private DateTime lastUpDateTime;
        /// <summary>
        /// 上一次更新时间
        /// </summary>
        public DateTime LastUpDateTime
        {
            get { return lastUpDateTime; }
            set { lastUpDateTime = value; }
        }
    }
}
