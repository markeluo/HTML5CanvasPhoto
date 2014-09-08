using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhotoApp.Model
{
    public class GUIDInfo
    {
        private static GUIDInfo guid = new GUIDInfo();
        public static GUIDInfo Instance
        {
            get
            {
                return guid;
            }
        }

        /// <summary>
        /// string 27位ID生成规则（前面17位为时间，后面10位为随机生成数）：
        /// </summary>
        /// <returns></returns>
        public string GetGUID()
        {
            Guid id = Guid.NewGuid();
            byte[] byid = id.ToByteArray();

            uint uid = BitConverter.ToUInt32(byid, 12);
            string strid = DateTime.Now.ToString("yyyyMMddHHmmssfff") + uid.ToString("D10");

            return decimal.Parse(strid).ToString();
        }
    }
}
