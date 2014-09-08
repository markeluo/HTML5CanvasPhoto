/**
 * Created with JetBrains WebStorm.
 * User: markeluo
 * Date: 13-2-20
 * Time: 上午10:16
 * To change this template use File | Settings | File Templates.
 */
Namespace.register("PhotoApp.BLL");

//region 1.获取相册列表
PhotoApp.BLL.GetAblumeList=function(_callbackFun){
    PhotoApp.DataManagerDAL.ReadData({
        "MethodName":"GetAblumeList", "Paras":"", "CallBackFunction":function (_result) {
            _callbackFun(_result);
        }
    });
}
//endregion

//region 2.获取相册详情
PhotoApp.BLL.GetAblumeDetail=function(_AblumNo,_callBackFun){
    var parasJSON={AlbumNo:_AblumNo};
    parasJSON=JSON.stringify(parasJSON);
    PhotoApp.DataManagerDAL.ReadData({
        "MethodName":"GetAblumeDetail", "Paras":parasJSON, "CallBackFunction":function (_result) {
            _callBackFun(_result);
        }
    });
}
//endregion

//region 3.获取相册中照片列表
PhotoApp.BLL.GetPhotos=function(_parsObj,_callBackFun){
    var parasJSON=
    {
        AlbumNo:_parsObj.AlbumNo,
        Size:_parsObj.Size,
        Page:_parsObj.Page
    };
    parasJSON=JSON.stringify(parasJSON);
    PhotoApp.DataManagerDAL.ReadData({
        "MethodName":"GetPhotoList", "Paras":parasJSON, "CallBackFunction":function (_result) {
            _callBackFun(_result);
        }
    });
}
//endregion

//region 4.获取照片缩略图
PhotoApp.BLL.GetMiniPhotoData=function(_PhotoNo,_callBackFun){
    var parasJSON={PhotoNo:_PhotoNo};
    parasJSON=JSON.stringify(parasJSON);
    PhotoApp.DataManagerDAL.ReadData({
        "MethodName":"GetMiniPhotoData", "Paras":parasJSON, "CallBackFunction":function (_result) {
            _callBackFun(_result);
        }
    });
}
PhotoApp.BLL.GetPhotoData=function(_PhotoNo,_callBackFun){
    var parasJSON={PhotoNo:_PhotoNo};
    parasJSON=JSON.stringify(parasJSON);
    PhotoApp.DataManagerDAL.ReadData({
        "MethodName":"GetPhotoData", "Paras":parasJSON, "CallBackFunction":function (_result) {
            _callBackFun(_result);
        }
    });
}
//endregion


