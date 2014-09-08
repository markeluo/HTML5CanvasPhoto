/**
 * Created with JetBrains WebStorm.
 * User: markeluo
 * Date: 13-3-6
 * Time: 下午5:32
 * To change this template use File | Settings | File Templates.
 */
//Album相册信息:{AlbumNO,AlbumName,AlbumRemark,AlbumRootPath,AlbumTitleImg,CreateDate,PhotosNumber,SubPhotos,UpdateDate}
//照片信息:{PhotoNO,PhotoName,PhotoRemark,PhotoData,PhotoMiniData,IsTitleImg,PhotoCreateDate,PhotoUpdateDate}
Namespace.register("PhotoApp.UIManager");

//region 0.页面加载处理
$(document).ready(function(){
    //1.初始化面板
    PhotoApp.UIManager.InitCanvasPanel();

    //2.加载相册列表
    PhotoApp.UIManager.LoadingAlbumList();
});
//endregion

//region 1.页面相关参数
PhotoApp.UIManager.UIPars={
    Canvas:null,//Canvas画布对象
    AblumList:null,//相册列表
    AblumPageSize:30,//相册每页显示照片数
    AblumOffLeft:20,//相册与左侧元素的距离
    AblumOffTop:10,//相册与上方元素的距离
    AblumPanelWidth:110,//相册宽度
    AblumPanelHeight:110,//相册高度
    PhotoOffLeft:10,//相册与左侧元素的距离
    PhotoOffTop:10,//相册与上方元素的距离
    PhotoPanelWidth:120,//相册宽度
    PhotoPanelHeight:120,//相册高度
    OpenAblumNo:null,//被打开的相册
    CheckedPhotoNo:null,//选中的照片
    PhotoDataPars:{
        borderSize : 10,
        activeImage : null,
        inprogress : false,
        resizeSpeed : 350,
        widthCurrent: 250,
        heightCurrent: 250,
        xScale : 1,
        yScale : 1,
        imgPreloader:null
    },
    PhotoList:[],//图片对象列表
    AblumeList:[]//相册列表
}
//endregion

//region 2.初始化画布
PhotoApp.UIManager.InitCanvasPanel=function(){
    var canvas = document.getElementById("myCanvas");
    //0.控制画布大小
    canvas.width=$(document.body).width();
    canvas.height=$(document).height()-60;

    $("#FootMenu").hide();
    $("#FootMenu").css("width",canvas.width+"px");
    $("#FootMenu").css("top",(canvas.height+3)+"px");

    //1.绘制背景
    var ctx= canvas.getContext("2d");
    ctx.fillStyle = "rgba(255,255,255,1)";
    ctx.fillRect(0,0,canvas.width,canvas.height);
    PhotoApp.UIManager.UIPars.Canvas=canvas;

    //2.画布单击事件
    PhotoApp.UIManager.UIPars.Canvas.onclick = function(){};
}
//endregion

//region 3.加载相册列表
PhotoApp.UIManager.LoadingAlbumList=function(){
    PhotoApp.UIManager.CanvasContextClear();
    PhotoApp.UIManager.UIPars.OpenAblumNo=null;//被打开的相册清空
    PhotoApp.UIManager.UIPars.CheckedPhotoNo=null;//选中的照片清空
    $("#Ablumlist").unbind().removeClass("Menuasty");
    $(".MenuAblumsty").remove();

    $("#FootMenu").hide();
    if(PhotoApp.UIManager.UIPars.AblumeList!=null && PhotoApp.UIManager.UIPars.AblumeList.length>0){
        var canvasctx=PhotoApp.UIManager.UIPars.Canvas.getContext("2d");
        PhotoApp.UIManager.DrawAlbum(canvasctx,PhotoApp.UIManager.UIPars.AblumeList,0);//相册列表
        PhotoApp.UIManager.DrawAlbumImg(canvasctx,PhotoApp.UIManager.UIPars.AblumeList,0);//加载相册封面
    }else{
        PhotoApp.BLL.GetAblumeList(function(_Result){
            if(_Result.result){
                if(_Result.Data!=null && _Result.Data.length>0){
                    PhotoApp.UIManager.UIPars.AblumeList=_Result.Data;
                    for(var i=0;i<PhotoApp.UIManager.UIPars.AblumeList.length;i++){
                        PhotoApp.UIManager.UIPars.AblumeList[i].Postion={Left:0,Top:0,Right:0,Bottom:0};
                    }
                    var canvasctx=PhotoApp.UIManager.UIPars.Canvas.getContext("2d");
                    PhotoApp.UIManager.DrawAlbum(canvasctx,PhotoApp.UIManager.UIPars.AblumeList,0);//相册列表
                    PhotoApp.UIManager.DrawAlbumImg(canvasctx,PhotoApp.UIManager.UIPars.AblumeList,0);//加载相册封面
                }
            }
        });
    }
    PhotoApp.UIManager.UIPars.Canvas.onclick=PhotoApp.UIManager.AlbumOnlclick;
}
//3.1.绘制相册信息(不现实封面照片)
PhotoApp.UIManager.DrawAlbum=function(ctx,_AblumeList,_Index){
        ctx.fillStyle = '#c9c9c9';
        ctx.strokeRect(PhotoApp.UIManager.UIPars.AblumOffLeft+5,PhotoApp.UIManager.UIPars.AblumOffTop+5,
            PhotoApp.UIManager.UIPars.AblumPanelWidth,PhotoApp.UIManager.UIPars.AblumPanelHeight);//绘制边框
        ctx.strokeRect(PhotoApp.UIManager.UIPars.AblumOffLeft,PhotoApp.UIManager.UIPars.AblumOffTop,
            PhotoApp.UIManager.UIPars.AblumPanelWidth,PhotoApp.UIManager.UIPars.AblumPanelHeight);//绘制边框
        ctx.clearRect(PhotoApp.UIManager.UIPars.AblumOffLeft+1,PhotoApp.UIManager.UIPars.AblumOffTop+1,
            PhotoApp.UIManager.UIPars.AblumPanelWidth-1,PhotoApp.UIManager.UIPars.AblumPanelHeight-1);//清空区域

        _AblumeList[_Index].Postion.Left=PhotoApp.UIManager.UIPars.AblumOffLeft+10;
        _AblumeList[_Index].Postion.Top=PhotoApp.UIManager.UIPars.AblumOffTop+10;
        _AblumeList[_Index].Postion.Right=PhotoApp.UIManager.UIPars.AblumOffLeft+10+PhotoApp.UIManager.UIPars.AblumPanelWidth;
        _AblumeList[_Index].Postion.Bottom=PhotoApp.UIManager.UIPars.AblumOffTop+10+PhotoApp.UIManager.UIPars.AblumPanelHeight;
//        ctx.drawImage(this,PhotoApp.UIManager.UIPars.AblumOffLeft+10,PhotoApp.UIManager.UIPars.AblumOffTop+10,
//            (PhotoApp.UIManager.UIPars.AblumPanelWidth-20),(PhotoApp.UIManager.UIPars.AblumPanelWidth-20));

        //文字显示部分
        ctx.clearRect(PhotoApp.UIManager.UIPars.AblumOffLeft+1,PhotoApp.UIManager.UIPars.AblumOffTop+PhotoApp.UIManager.UIPars.AblumPanelWidth+10,PhotoApp.UIManager.UIPars.AblumPanelWidth-1,35);//清空区域
        ctx.fillStyle = '#000000';
        ctx.font = 'bold 10px 微软雅黑';
        ctx.fillText(_AblumeList[_Index].AlbumName,PhotoApp.UIManager.UIPars.AblumOffLeft+5,
            PhotoApp.UIManager.UIPars.AblumOffTop+PhotoApp.UIManager.UIPars.AblumPanelHeight+25);
        ctx.font = '8px 微软雅黑';
        ctx.fillStyle = '#7e7d7d';
        ctx.fillText(_AblumeList[_Index].PhotosNumber+" 张",PhotoApp.UIManager.UIPars.AblumOffLeft+5,
            PhotoApp.UIManager.UIPars.AblumOffTop+PhotoApp.UIManager.UIPars.AblumPanelHeight+40);
        PhotoApp.UIManager.UIPars.AblumOffLeft+=130;
        if((PhotoApp.UIManager.UIPars.AblumOffLeft+130)>PhotoApp.UIManager.UIPars.Canvas.width){
            PhotoApp.UIManager.UIPars.AblumOffLeft=10;
            PhotoApp.UIManager.UIPars.AblumOffTop+=180;
        }
        //绘制完成一张后再绘制下一张
        if(_Index+1<_AblumeList.length){
            PhotoApp.UIManager.DrawAlbum(ctx,_AblumeList,_Index+1);
        }
}
//3.2.绘制相册封面
PhotoApp.UIManager.DrawAlbumImg=function(ctx,_AblumeList,_Index){
    PhotoApp.BLL.GetAblumeDetail(_AblumeList[_Index].AlbumNO,function(_result){
        var ThisImg=new Image()
        ThisImg.src=_result.Data.AlbumTitleImg;
        ThisImg.onload = function () {
            ctx.drawImage(this, _AblumeList[_Index].Postion.Left, _AblumeList[_Index].Postion.Top,
                (PhotoApp.UIManager.UIPars.AblumPanelWidth-20),(PhotoApp.UIManager.UIPars.AblumPanelWidth-20));
            //绘制完成一张后再绘制下一张
            if(_Index+1<_AblumeList.length){
                PhotoApp.UIManager.DrawAlbumImg(ctx,_AblumeList,_Index+1);
            }
        }
        ThisImg=null;
    });
}
//3.3.相册封面双击事件
PhotoApp.UIManager.AlbumOnlclick=function(ev){
    var SelectedAblum=PhotoApp.UIManager.GetAblumInfo({X:ev.x,Y:ev.y},PhotoApp.UIManager.UIPars.AblumeList);
    if(SelectedAblum!=null){
        PhotoApp.UIManager.LoadingPhotos(SelectedAblum.AlbumNO,1,PhotoApp.UIManager.UIPars.AblumPageSize);//加载选中相册中的照片列表
    }
}
//3.4.根据点击坐标和相册列表获取所点击相册
PhotoApp.UIManager.GetAblumInfo=function(_offsetInfo,_AblumList){
    var ThisAblum=null;
    if(_AblumList!=null && _AblumList.length>0){
        for(var i=0;i<_AblumList.length;i++){
            if(_offsetInfo.X>=_AblumList[i].Postion.Left &&_offsetInfo.X<=_AblumList[i].Postion.Right
                && _offsetInfo.Y>=_AblumList[i].Postion.Top && _offsetInfo.Y<=_AblumList[i].Postion.Bottom){
                ThisAblum=_AblumList[i];
                break;
            }
        }
    }
    return ThisAblum;
}
//endregion

//region 4.画布清空
PhotoApp.UIManager.CanvasContextClear=function() {
    PhotoApp.UIManager.UIPars.AblumOffLeft=10;//相册与左侧元素的距离
    PhotoApp.UIManager.UIPars.AblumOffTop=10;//相册与上方元素的距离
    PhotoApp.UIManager.UIPars.PhotoOffLeft=10;//照片与左侧元素的距离
    PhotoApp.UIManager.UIPars.PhotoOffTop=10;//照片与上方元素的距离
    var ctx=PhotoApp.UIManager.UIPars.Canvas.getContext("2d");
    ctx.clearRect(0, 0,PhotoApp.UIManager.UIPars.Canvas.width,PhotoApp.UIManager.UIPars.Canvas.height);
}
//endregion

//region 5.加载相册照片列表
PhotoApp.UIManager.LoadingPhotos=function(_AblumnNo,_Page,_PageSize){
    var datapars={
        AlbumNo:_AblumnNo,
        Size:_PageSize,
        Page:_Page
    }
    var ThisAblum=PhotoApp.UIManager.GetAblumByNo(_AblumnNo);//获取相册信息
    $("#HeadMenu").find(".MenuAblumsty").remove();
    $("#HeadMenu").append("<a class='MenuAblumsty'>"+ThisAblum.AlbumName+"></a>");
    $("#Ablumlist").unbind().bind("click",PhotoApp.UIManager.LoadingAlbumList).addClass("Menuasty");

//    if(ThisAblum.SubPhotos!=null && ThisAblum.SubPhotos.length>0){
//
//        PhotoApp.UIManager.CanvasContextClear();//清空画布
//        var canvasctx=PhotoApp.UIManager.UIPars.Canvas.getContext("2d");
//        PhotoApp.UIManager.DrawPhotos(canvasctx,ThisAblum.SubPhotos,0);//相册列表
//        PhotoApp.UIManager.DrawPhotoImg(canvasctx,ThisAblum.SubPhotos,0);//加载相册封面
//
//    }else{
        PhotoApp.BLL.GetPhotos(datapars,function(_Result){
            if(_Result.result){
                if(_Result.Data!=null && _Result.Data.length>0){
                    ThisAblum.SubPhotos=_Result.Data;
                    for(var i=0;i<ThisAblum.SubPhotos.length;i++){
                        ThisAblum.SubPhotos[i].Postion={Left:0,Top:0,Right:0,Bottom:0};
                    }
                    PhotoApp.UIManager.CanvasContextClear();//清空画布
                    var canvasctx=PhotoApp.UIManager.UIPars.Canvas.getContext("2d");
                    PhotoApp.UIManager.DrawPhotos(canvasctx,ThisAblum.SubPhotos,0);//相册列表
                    PhotoApp.UIManager.DrawPhotoImg(canvasctx,ThisAblum.SubPhotos,0);//加载相册封面
                }
            }else{
                alert(_Result.message);
                return false;
            }
        });
//    }

    PhotoApp.UIManager.UIPars.OpenAblumNo=_AblumnNo;
    PhotoApp.UIManager.UIPars.Canvas.onclick=PhotoApp.UIManager.PhotoOnlclick;

    $("#FootMenu").show();//显示底部分页菜单
    PhotoApp.UIManager.InitPageMenu(_Page,ThisAblum.PhotosNumber,PhotoApp.UIManager.UIPars.AblumPageSize)
}
//2.根据相册编号获取相册信息
PhotoApp.UIManager.GetAblumByNo=function(_AblumNo){
    var ThisAblum=null;
    if(PhotoApp.UIManager.UIPars.AblumeList!=null && PhotoApp.UIManager.UIPars.AblumeList.length>0){
        for(var i=0;i<PhotoApp.UIManager.UIPars.AblumeList.length;i++){
            if(PhotoApp.UIManager.UIPars.AblumeList[i].AlbumNO===_AblumNo){
                ThisAblum=PhotoApp.UIManager.UIPars.AblumeList[i];
                break;
            }
        }
    }
    return ThisAblum;
}
//3.绘制照片信息至画布(未填充缩略图)
PhotoApp.UIManager.DrawPhotos=function(ctx,_PhotoList,_Index){
    ctx.fillStyle = '#c9c9c9';
//    ctx.strokeRect(PhotoApp.UIManager.UIPars.PhotoOffLeft+5,PhotoApp.UIManager.UIPars.PhotoOffTop+5,
//        PhotoApp.UIManager.UIPars.PhotoPanelWidth,PhotoApp.UIManager.UIPars.PhotoPanelHeight);//绘制边框
    ctx.strokeRect(PhotoApp.UIManager.UIPars.PhotoOffLeft,PhotoApp.UIManager.UIPars.PhotoOffTop,
        PhotoApp.UIManager.UIPars.PhotoPanelWidth,PhotoApp.UIManager.UIPars.PhotoPanelHeight);//绘制边框
//    ctx.clearRect(PhotoApp.UIManager.UIPars.PhotoOffLeft+1,PhotoApp.UIManager.UIPars.PhotoOffTop+1,
//        PhotoApp.UIManager.UIPars.PhotoPanelWidth-1,PhotoApp.UIManager.UIPars.PhotoPanelHeight-1);//清空区域

    _PhotoList[_Index].Postion.Left=PhotoApp.UIManager.UIPars.PhotoOffLeft+10;
    _PhotoList[_Index].Postion.Top=PhotoApp.UIManager.UIPars.PhotoOffTop+10;
    _PhotoList[_Index].Postion.Right=PhotoApp.UIManager.UIPars.PhotoOffLeft+10+PhotoApp.UIManager.UIPars.PhotoPanelWidth;
    _PhotoList[_Index].Postion.Bottom=PhotoApp.UIManager.UIPars.PhotoOffTop+10+PhotoApp.UIManager.UIPars.PhotoPanelHeight;
//        ctx.drawImage(this,PhotoApp.UIManager.UIPars.PhotoOffLeft+10,PhotoApp.UIManager.UIPars.PhotoOffTop+10,
//            (PhotoApp.UIManager.UIPars.PhotoPanelWidth-20),(PhotoApp.UIManager.UIPars.PhotoPanelWidth-20));

    //文字显示部分
    ctx.clearRect(PhotoApp.UIManager.UIPars.PhotoOffLeft+1,PhotoApp.UIManager.UIPars.PhotoOffTop+PhotoApp.UIManager.UIPars.PhotoPanelWidth+10,PhotoApp.UIManager.UIPars.PhotoPanelWidth-1,35);//清空区域
    ctx.fillStyle = '#000000';
    ctx.font = 'bold 10px 微软雅黑';
    _PhotoList[_Index].PhotoName=PhotoApp.UIManager.TitleStrManager(_PhotoList[_Index].PhotoName,10);
    ctx.fillText(_PhotoList[_Index].PhotoName,PhotoApp.UIManager.UIPars.PhotoOffLeft+5,
        PhotoApp.UIManager.UIPars.PhotoOffTop+PhotoApp.UIManager.UIPars.PhotoPanelHeight+25);
    ctx.font = '8px 微软雅黑';
    ctx.fillStyle = '#7e7d7d';
    ctx.fillText(_PhotoList[_Index].PhotoCreateDate,PhotoApp.UIManager.UIPars.PhotoOffLeft+5,
        PhotoApp.UIManager.UIPars.PhotoOffTop+PhotoApp.UIManager.UIPars.PhotoPanelHeight+40);
    PhotoApp.UIManager.UIPars.PhotoOffLeft+=130;
    if((PhotoApp.UIManager.UIPars.PhotoOffLeft+130)>PhotoApp.UIManager.UIPars.Canvas.width){
        PhotoApp.UIManager.UIPars.PhotoOffLeft=10;
        PhotoApp.UIManager.UIPars.PhotoOffTop+=180;
    }
    //绘制完成一张后再绘制下一张
    if(_Index+1<_PhotoList.length){
        PhotoApp.UIManager.DrawPhotos(ctx,_PhotoList,_Index+1);
    }
}
//4.填充缩略图
PhotoApp.UIManager.DrawPhotoImg=function(ctx,_PhotoList,_Index){
    PhotoApp.BLL.GetMiniPhotoData(_PhotoList[_Index].PhotoNO,function(_result){
        var ThisImg=new Image()
        ThisImg.src=_result.Data;
        ThisImg.onload = function () {
            ctx.drawImage(this, _PhotoList[_Index].Postion.Left, _PhotoList[_Index].Postion.Top,
                (PhotoApp.UIManager.UIPars.PhotoPanelWidth-20),(PhotoApp.UIManager.UIPars.PhotoPanelWidth-20));
            //绘制完成一张后再绘制下一张
            if(_Index+1<_PhotoList.length){
                PhotoApp.UIManager.DrawPhotoImg(ctx,_PhotoList,_Index+1);
            }
        }
        ThisImg=null;
    });
}
//5.单击照片查看原图
PhotoApp.UIManager.PhotoOnlclick=function(ev){
    PhotoApp.UIManager.ShowPhotoData(ev);
}
//6.根据点击坐标和相册中照片列表获取所点击照片
PhotoApp.UIManager.GetPhotoInfoByAblum=function(_offsetInfo,_Ablum){
    var ThisPhoto=null;
    if(_Ablum!=null && _Ablum.SubPhotos!=null && _Ablum.SubPhotos.length>0){
        for(var i=0;i<_Ablum.SubPhotos.length;i++){
            if(_offsetInfo.X>=_Ablum.SubPhotos[i].Postion.Left &&_offsetInfo.X<=_Ablum.SubPhotos[i].Postion.Right
                && _offsetInfo.Y>=_Ablum.SubPhotos[i].Postion.Top && _offsetInfo.Y<=_Ablum.SubPhotos[i].Postion.Bottom){
                ThisPhoto=_Ablum.SubPhotos[i];
                break;
            }
        }
    }
    return ThisPhoto;
}

//region 7.照片显示
//7.1.显示照片详情
PhotoApp.UIManager.ShowPhotoData=function(ev){
    var oOpenedAblum=null;
    if(PhotoApp.UIManager.UIPars.AblumeList!=null && PhotoApp.UIManager.UIPars.AblumeList.length>0){
        for(var i=0;i<PhotoApp.UIManager.UIPars.AblumeList.length;i++){
            if(PhotoApp.UIManager.UIPars.AblumeList[i].AlbumNO===PhotoApp.UIManager.UIPars.OpenAblumNo){
                oOpenedAblum=PhotoApp.UIManager.UIPars.AblumeList[i];
                break;
            }
        }
    }
    var SelectedPhoto=PhotoApp.UIManager.GetPhotoInfoByAblum({X:ev.x,Y:ev.y},oOpenedAblum);
    if(SelectedPhoto!=null){
        PhotoApp.UIManager.ShowPhotoLoading();//加载图标显示
        if(SelectedPhoto.PhotoData!=null && SelectedPhoto.PhotoData.length>0){
            PhotoApp.UIManager.PhotoImgLoad(SelectedPhoto);
        }else{
            PhotoApp.BLL.GetPhotoData(SelectedPhoto.PhotoNO,function(result){
                SelectedPhoto.PhotoData=result.Data;
                PhotoApp.UIManager.PhotoImgLoad(SelectedPhoto);
            })
        }
    }
}
PhotoApp.UIManager.PhotoImgLoad=function(SelectedPhoto){
    PhotoApp.UIManager.UIPars.PhotoDataPars.imgPreloader =null;
    PhotoApp.UIManager.UIPars.PhotoDataPars.imgPreloader=new Image();
    // once image is preloaded, resize image container
    PhotoApp.UIManager.UIPars.PhotoDataPars.imgPreloader.onload=function(){
        var newWidth = PhotoApp.UIManager.UIPars.PhotoDataPars.imgPreloader.width;
        var newHeight = PhotoApp.UIManager.UIPars.PhotoDataPars.imgPreloader.height;
        if(newHeight>450){
            var sizesc=newHeight/450;
            newHeight=450;
            newWidth=newWidth/sizesc;
            sizesc=null;
        }
        $('#loading').hide();
        $('#lightboxImage').attr('src',SelectedPhoto.PhotoData).width(newWidth).height(newHeight).show();
        PhotoApp.UIManager.resizeImageContainer(newWidth, newHeight);
    };

    PhotoApp.UIManager.UIPars.PhotoDataPars.imgPreloader.src =SelectedPhoto.PhotoData;
}
//7.2.显示加载
PhotoApp.UIManager.ShowPhotoLoading=function(){
    if($("#overlay").length>0){
        $('#loading').show();
    }else{
        //region 添加照片显示容器
        var PhotoDetailPanel="<div id='overlay'></div>" +
            "<div id='lightbox'>" +
                "<div id='outerImageContainer'>" +
                    "<div id='imageContainer'>" +
                        "<iframe id='lightboxIframe'/><img id='lightboxImage'>" +
                        "<div id='loading' ><a href='javascript://' id='loadingLink'><img src='images/loading.gif'></a></div>" +
                    "</div>" +
                "</div>" +
                "<div id='imageDataContainer' class='clearfix'>" +
                    "<div id='imageData'>" +
                        "<div id='imageDetails'>" +
                            "<span id='caption'></span>" +
                            "<span id='numberDisplay'></span>" +
                        "</div>" +
                        "<div id='bottomNav'>" +
                            "<a href='javascript://' id='bottomNavClose' title='关闭'><img src='images/closelabel.gif'></a>" +
                        "</div>" +
                    "</div>" +
                "</div>" +
            "</div>";
        $("body").append(PhotoDetailPanel);
        $("#imageDataContainer").hide();
        $("#imageDataContainer").addClass('ontop');
        $("#overlay").unbind().bind("click",function(){
            $('#lightbox').hide();
            $('#overlay').fadeOut();
            $("#lightboxImage").hide();
        });
        $("#lightbox").unbind().bind("click",function(){
            $('#lightbox').hide();
            $('#overlay').fadeOut();
            $("#lightboxImage").hide();
        });
        $("#loadingLink").unbind().bind("click",function(){
            $('#lightbox').hide();
            $('#overlay').fadeOut();
            $("#lightboxImage").hide();
            return false;
        });
        $("#bottomNavClose").unbind().bind("click",function(){
            $('#lightbox').hide();
            $('#overlay').fadeOut();
            $("#lightboxImage").hide();
            return false;
        });
        //endregion
    }
    var arrayPageSize = PhotoApp.UIManager.getPageSize();
    $('#outerImageContainer').width(250).height(250);
    $('#imageDataContainer').width(250);
    $("#overlay").hide().css({width: '100%', height:'100%', opacity :0.8}).fadeIn();
    var arrayPageScroll = PhotoApp.UIManager.getPageScroll();
    var lightboxTop = arrayPageScroll[1] + (arrayPageSize[3] / 10);
    var lightboxLeft = arrayPageScroll[0];
    $('#lightbox').css({top: lightboxTop+'px', left: lightboxLeft+'px'}).show();
}
//获得页面尺寸信息
PhotoApp.UIManager.getPageSize=function() {
    var jqueryPageSize = new Array($(document).width(),$(document).height(), $(window).width(), $(window).height());
    return jqueryPageSize;
};
PhotoApp.UIManager.getPageScroll=function() {
    var xScroll, yScroll;

    if (self.pageYOffset) {
        yScroll = self.pageYOffset;
        xScroll = self.pageXOffset;
    } else if (document.documentElement && document.documentElement.scrollTop){  // Explorer 6 Strict
        yScroll = document.documentElement.scrollTop;
        xScroll = document.documentElement.scrollLeft;
    } else if (document.body) {// all other Explorers
        yScroll = document.body.scrollTop;
        xScroll = document.body.scrollLeft;
    }

    var arrayPageScroll = new Array(xScroll,yScroll);
    return arrayPageScroll;
};
PhotoApp.UIManager.resizeImageContainer=function(imgWidth,imgHeight){
    // get current width and height
    PhotoApp.UIManager.UIPars.PhotoDataPars.widthCurrent = $("#outerImageContainer").outerWidth();
    PhotoApp.UIManager.UIPars.PhotoDataPars.heightCurrent = $("#outerImageContainer").outerHeight();

    // get new width and height
    var widthNew = Math.max(350, imgWidth  + (PhotoApp.UIManager.UIPars.PhotoDataPars.borderSize * 2));
    var heightNew = (imgHeight  + (PhotoApp.UIManager.UIPars.PhotoDataPars.borderSize * 2));

    // scalars based on change from old to new
    PhotoApp.UIManager.UIPars.PhotoDataPars.xScale = ( widthNew / PhotoApp.UIManager.UIPars.PhotoDataPars.widthCurrent) * 100;
    PhotoApp.UIManager.UIPars.PhotoDataPars.yScale = ( heightNew / PhotoApp.UIManager.UIPars.PhotoDataPars.heightCurrent) * 100;

    // calculate size difference between new and old image, and resize if necessary
    wDiff = PhotoApp.UIManager.UIPars.PhotoDataPars.widthCurrent - widthNew;
    hDiff = PhotoApp.UIManager.UIPars.PhotoDataPars.heightCurrent - heightNew;

    $('#imageDataContainer').animate({width: widthNew},PhotoApp.UIManager.UIPars.PhotoDataPars.resizeSpeed,'linear');
    $('#outerImageContainer').animate({width: widthNew},PhotoApp.UIManager.UIPars.PhotoDataPars.resizeSpeed,'linear',function(){
        $('#outerImageContainer').animate({height: heightNew},PhotoApp.UIManager.UIPars.PhotoDataPars.resizeSpeed,'linear',function(){
            PhotoApp.UIManager.ShowImage();
        });
    });

    // if new and old image are same size and no scaling transition is necessary,
    // do a quick pause to prevent image flicker.
    if((hDiff == 0) && (wDiff == 0)){
        if (jQuery.browser.msie){ PhotoApp.UIManager.pause(250); } else { PhotoApp.UIManager.pause(100);}
    }

}
PhotoApp.UIManager.ShowImage=function(){
    $('#loading').hide();
    $("#imageDataContainer").show();
    $('#lightboxImage').fadeIn("fast");
    PhotoApp.UIManager.UIPars.PhotoDataPars.inprogress = false;
}
PhotoApp.UIManager.pause=function(ms) {
    var date = new Date();
    var curDate = null;
    do{curDate = new Date();}
    while(curDate - date < ms);
};
//endregion

//endregion

//region 6.显示分页
PhotoApp.UIManager.InitPageMenu=function(_Page,_SumPhotos,_PageSize){
    var THisSumPages=parseInt(_SumPhotos/_PageSize);
    if(_SumPhotos%_PageSize!=0){
        THisSumPages=THisSumPages+1;
    }
    $("#FootMenu_First").unbind().bind("click",function(){
        PhotoApp.UIManager.LoadingPhotos(PhotoApp.UIManager.UIPars.OpenAblumNo,(_Page-1),PhotoApp.UIManager.UIPars.AblumPageSize);
    }).addClass("Menuasty");
    $("#FootMenu_Next").unbind().bind("click",function(){
        PhotoApp.UIManager.LoadingPhotos(PhotoApp.UIManager.UIPars.OpenAblumNo,(_Page+1),PhotoApp.UIManager.UIPars.AblumPageSize);
    }).addClass("Menuasty");
    if(_Page<=1){
        _Page=1;
        $("#FootMenu_First").unbind().removeClass("Menuasty");
    }
    if(_Page>=THisSumPages){
        _Page=THisSumPages;
        $("#FootMenu_Next").unbind().removeClass("Menuasty");
    }
    var i=_Page;
    var FootPageMenustr="";
    if(i<4){
        for(var j=1;j<=i;j++){
            if(j===i){
                FootPageMenustr=FootPageMenustr+j;
            }else{
                FootPageMenustr=FootPageMenustr+"<a data-filed="+j+">"+j+"</a>";
            }
        }
        if(i+3<THisSumPages){
            for(var j=(i+1);j<=(i+3);j++){
                FootPageMenustr=FootPageMenustr+"<a data-filed="+j+">"+j+"</a>";
            }
            FootPageMenustr=FootPageMenustr+"..."+"<a data-filed="+THisSumPages+">"+THisSumPages+"</a>";
        }else{
            if(i<THisSumPages){
                for(var j=(i+1);j<=THisSumPages;j++){
                    FootPageMenustr=FootPageMenustr+"<a data-filed="+j+">"+j+"</a>";
                }
            }
        }
    }else{
        FootPageMenustr=FootPageMenustr+"<a data-filed='1'>1</a>"+"...";
        for(var j=(i-2);j<=i;j++){
            if(j===i){
                FootPageMenustr=FootPageMenustr+j;
            }else{
                FootPageMenustr=FootPageMenustr+"<a data-filed="+j+">"+j+"</a>";
            }
        }
        if(i+3<THisSumPages){
            for(var j=(i+1);j<=(i+3);j++){
                FootPageMenustr=FootPageMenustr+"<a data-filed="+j+">"+j+"</a>";
            }
            FootPageMenustr=FootPageMenustr+"..."+"<a data-filed="+THisSumPages+">"+THisSumPages+"</a>";
        }else{
            if(i<THisSumPages){
                for(var j=(i+1);j<=THisSumPages;j++){
                    FootPageMenustr=FootPageMenustr+"<a data-filed="+j+">"+j+"</a>";
                }
            }
        }
    }
    $("#FootMenuItems").html("");
    $("#FootMenuItems").append(FootPageMenustr);

    $("#FootMenuItems").find("a").unbind().bind("click",function(ev){
        var ThisPageIndex=$(this).data("filed");
        PhotoApp.UIManager.LoadingPhotos(PhotoApp.UIManager.UIPars.OpenAblumNo,ThisPageIndex,PhotoApp.UIManager.UIPars.AblumPageSize);
    }).addClass("Menuasty");
}
//endregion

//region 7.相关处理
//字符串截取处理
PhotoApp.UIManager.TitleStrManager=function(_OldString,_MaxLength){
    if(_OldString.length>=_MaxLength){
        return _OldString.substring(0,10)+"...";
    }
    return _OldString;
}
//endregion
