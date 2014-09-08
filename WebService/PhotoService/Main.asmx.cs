using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.IO;
using System.Text;
using System.Drawing;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using PhotoApp.Model;
using System.Runtime.Serialization.Json;

namespace PhotoService
{
    /// <summary>
    /// Main 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。
    // [System.Web.Script.Services.ScriptService]
    public class Main : System.Web.Services.WebService
    {
        JavaScriptSerializer serializer = new JavaScriptSerializer();
 
        /// <summary>
        /// 获取相册列表
        /// </summary>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void GetAblumeList()
        {
            WebServiceHelper.AblumManagerHelper manager = new WebServiceHelper.AblumManagerHelper();
            List<Album> _albums=manager.GetAllAlbumlist();
           
            string strJsonData =ObjectToJSON.Instance.GetJsonString(_albums);

            ResponseControl.Instance.OutPutjson("{ \"result\" : \"true\",\"message\":\"执行成功\",\"Data\":" + strJsonData + " }");
        }

        /// <summary>
        /// 获取相册详情
        /// </summary>
        /// <param name="jsonData">请求相册参数</param>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void GetAblumeDetail(string jsonData)
        {
            string AlbumNo = "";
             Dictionary<string, object> json = (Dictionary<string, object>)serializer.DeserializeObject(jsonData);
             if (!string.IsNullOrEmpty(json["AlbumNo"].ToString()))
             {
                 AlbumNo = json["AlbumNo"].ToString();
                 WebServiceHelper.AblumManagerHelper manager = new WebServiceHelper.AblumManagerHelper();
                 Album _album = manager.GetAlbumInfoByNo(AlbumNo);
                 string strJsonData = ObjectToJSON.Instance.GetJsonString(_album);

                 ResponseControl.Instance.OutPutjson("{ \"result\" : \"true\",\"message\":\"执行成功\",\"Data\":" + strJsonData + " }");
             }
             else
             {
                 ResponseControl.Instance.OutPutjson("{ \"result\" : \"true\",\"message\":\"执行失败，参数格式错误！\",\"Data\":}");
             }
        }

        /// <summary>
        /// 获取照片列表
        /// </summary>
        /// <param name="jsonData">请求照片列表参数</param>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void GetPhotoList(string jsonData)
        {
            string AlbumNo = "";
            Dictionary<string, object> json = (Dictionary<string, object>)serializer.DeserializeObject(jsonData);
            if (!string.IsNullOrEmpty(json["AlbumNo"].ToString()))
            {
                AlbumNo = json["AlbumNo"].ToString();
                int _pagesize = 10;
                int _page = 1;
                if (json["Size"] != DBNull.Value && !string.IsNullOrEmpty(json["Size"].ToString()))
                {
                    _pagesize = Convert.ToInt32(json["Size"]);
                }
                if (json["Page"] != DBNull.Value && !string.IsNullOrEmpty(json["Page"].ToString()))
                {
                    _page = Convert.ToInt32(json["Page"]);
                }
                WebServiceHelper.PhotoManagerHelper manager = new WebServiceHelper.PhotoManagerHelper();
                List<PhotoInfo> _photos = manager.GetPhotosByPage(AlbumNo,_page,_pagesize);
                string strJsonData = ObjectToJSON.Instance.GetJsonString(_photos);

                ResponseControl.Instance.OutPutjson("{ \"result\" : \"true\",\"message\":\"执行成功\",\"Data\":" + strJsonData + " }");
            }
            else
            {
                ResponseControl.Instance.OutPutjson("{ \"result\" : \"true\",\"message\":\"执行失败，参数格式错误！\",\"Data\":}");
            }
        }

        /// <summary>
        /// 获取照片缩略图
        /// </summary>
        /// <param name="jsonData">请求照片缩略图参数</param>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void GetMiniPhotoData(string jsonData)
        {
            string strPhotoNo = "";
            Dictionary<string, object> json = (Dictionary<string, object>)serializer.DeserializeObject(jsonData);
            if (!string.IsNullOrEmpty(json["PhotoNo"].ToString()))
            {
                strPhotoNo = json["PhotoNo"].ToString();
  
                WebServiceHelper.PhotoManagerHelper manager = new WebServiceHelper.PhotoManagerHelper();
                string MiniImgData = ObjectToJSON.Instance.GetJsonString(manager.GetPhotoMiniImgData(strPhotoNo));

                ResponseControl.Instance.OutPutjson("{ \"result\" : \"true\",\"message\":\"执行成功\",\"Data\":" + MiniImgData + " }");
            }
            else
            {
                ResponseControl.Instance.OutPutjson("{ \"result\" : \"true\",\"message\":\"执行失败，参数格式错误！\",\"Data\":}");
            }
        }

        /// <summary>
        /// 获取照片原图
        /// </summary>
        /// <param name="jsonData">请求照片原图参数</param>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void GetPhotoData(string jsonData)
        {
            string strPhotoNo = "";
            Dictionary<string, object> json = (Dictionary<string, object>)serializer.DeserializeObject(jsonData);
            if (!string.IsNullOrEmpty(json["PhotoNo"].ToString()))
            {
                strPhotoNo = json["PhotoNo"].ToString();

                WebServiceHelper.PhotoManagerHelper manager = new WebServiceHelper.PhotoManagerHelper();
                string MiniImgData = ObjectToJSON.Instance.GetJsonString(manager.GetPhotoImgData(strPhotoNo));

                ResponseControl.Instance.OutPutjson("{ \"result\" : \"true\",\"message\":\"执行成功\",\"Data\":" + MiniImgData + " }");
            }
            else
            {
                ResponseControl.Instance.OutPutjson("{ \"result\" : \"true\",\"message\":\"执行失败，参数格式错误！\",\"Data\":}");
            }
        }
    }
}
