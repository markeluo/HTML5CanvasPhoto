using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Text;

namespace PhotoService
{
    public class ObjectToJSON
    {
        private static ObjectToJSON manager = new ObjectToJSON();
        public static ObjectToJSON Instance
        {
            get
            {
                return manager;
            }
        }

        /// <summary>
        /// 获取对象JSON数据
        /// </summary>
        /// <param name="_obj"></param>
        /// <returns></returns>
        public string GetJsonString(object _obj)
        {
            string strJsonData = "";
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(_obj.GetType());
            using (MemoryStream ms = new MemoryStream())
            {
                serializer.WriteObject(ms, _obj);
                StringBuilder sb = new StringBuilder();

                sb.Append(Encoding.UTF8.GetString(ms.ToArray()));

                strJsonData = sb.ToString();
                ms.Close();
                ms.Dispose();
            }

            return strJsonData;
        }
    }
}