using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

namespace PhotoService
{
    /// <summary>
    /// 消息输出返回控制
    /// </summary>
    public class ResponseControl
    {
        private static ResponseControl response = new ResponseControl();
        /// <summary>
        /// 消息输出实例
        /// </summary>
        public static ResponseControl Instance
        {
            get
            {
                return response;
            }
        }

        /// <summary>
        /// 将消息输出到客户端
        /// </summary>
        /// <param name="StrContent">输出内容</param>
        public void Output(string StrContent)
        {
            System.Web.HttpContext.Current.Response.AddHeader("Access-Control-Allow-Origin", "*");   //跨域策略
            HttpRequest Request = HttpContext.Current.Request;
            string callback = Request["callback"];
            HttpResponse Response = HttpContext.Current.Response;
            Encoding utf8 = Encoding.GetEncoding("utf-8");
            Response.ContentEncoding = utf8;
            Response.Write(callback + "(" + StrContent + ")");  //此方法是在jquery.min.js版本中使用
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }
        /// <summary>
        /// 输出JSON格式返回值
        /// </summary>
        /// <param name="strcontent"></param>
        public  void OutPutjson(string strcontent)
        {
            System.Web.HttpContext.Current.Response.AddHeader("Access-Control-Allow-Origin", "*");   //跨域策略
            HttpRequest Request = HttpContext.Current.Request;
            string callback = Request["callback"];
            HttpResponse Response = HttpContext.Current.Response;
            Encoding utf8 = Encoding.GetEncoding("utf-8");
            Response.ContentEncoding = utf8;
            Response.Write(strcontent);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        /// <summary>
        /// 直接输出执行成功消息
        /// </summary>
        public void ResultOK()
        {
            string result = "{ \"result\" : \"true\",\"message\":\"执行成功\" }";
            Output(result);
        }
        /// <summary>
        /// 将错误信息直接输出
        /// </summary>
        /// <param name="ErrorMsg"></param>
        public  void ResultError(string ErrorMsg)
        {
            string result = "{ \"result\" : \"false\", \"message\" : \"" + ErrorMsg + "\" }";
            Output(result);
        }
    }
}