/**
 * Created with JetBrains WebStorm.
 * User: markeluo
 * Date: 13-2-20
 * Time: 上午10:02
 * To change this template use File | Settings | File Templates.
 */
Namespace.register("PhotoApp.DataManager");
var WebServiceAddress="http://localhost:6062/Main.asmx";//请求地址
//var WebServiceAddress="http://localhost:6062/Main.asmx";//请求地址
PhotoApp.DataManager.DataManage = function () {
    this.url = null;
    this.ReadData = function (objcet) {
        this.url = WebServiceAddress + "/" + objcet.MethodName;
            this.NetDispose(objcet);
    }

    //.net处理方式
    this.NetDispose = function (objcet) {
        if (objcet.MethodName != "PhotoUpload") {
//            $.ajax({
//                type: "POST",
//                url: this.url,
//                dataType: "jsonp",
//                data: { "jsonData": objcet.Paras },
//                success: function (_result) {
//                    objcet.CallBackFunction(_result);
//                },
//                error: function (_result) {
//                    objcet.CallBackFunction(_result);
//                }
//            });

            /* jquery1.7.2 post提交方式 */
            $.post(
                this.url,
                { "jsonData": objcet.Paras },
                function (_result) {
                    objcet.CallBackFunction(_result);
                },"json");
        }
        else {
            /* jquery1.7.2 post提交方式 */
            $.post(
                this.url,
                { "jsonData": objcet.Paras },
                function (_result) {
                    objcet.CallBackFunction(_result);
                },
                "json");
        }
    }
    this.getPostData = function (MethodName, Paras) {
        //根据WSDL分析sayHelloWorld是方法名，parameters是传入参数名
        var postdata = "<?xml version=\"1.0\" encoding=\"utf-8\"?>";
        postdata += "<soap:Envelope xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\">";
        postdata += "<soap:Body><" + MethodName + " xmlns=\"http://tempuri.org/\">";
        if (Paras != null) {
            postdata += "<parameters>" + Paras + "</parameters>";
        }
        postdata += "</" + MethodName + "></soap:Body>";
        postdata += "</soap:Envelope>";
        return postdata;
    }
}
PhotoApp.DataManagerDAL =new PhotoApp.DataManager.DataManage();