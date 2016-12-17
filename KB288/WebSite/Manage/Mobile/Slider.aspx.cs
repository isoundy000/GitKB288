using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using BCW.Common;
using BCW.Files;

public partial class Manage_MobileSlider : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder( "" );
    private int pType;
    private string pAct;
    protected string xmlPath = "/Controls/upfile.xml";

    protected void Page_Load( object sender, EventArgs e )
    {
        builder.Append( Out.Tab( "<div class=\"title\">", "" ) );
        builder.Append( "<a href=\"" + Utils.getUrl( "Slider.aspx" ) + "\">轮播管理</a><br />" );
        builder.Append( Out.Tab( "</div>", "" ) );

        pType = Utils.ParseInt(Utils.GetRequest( "ptype", "all", 1, "", "" ));
        pAct =Utils.GetRequest( "act", "all", 1, "", "" ) ;

        
        builder.Append( Out.Tab( "<div class=\"text\">", "" ) );
        builder.Append( pType == 0 ? "主页" : "<a href=\"" + Utils.getUrl( "Slider.aspx?ptype=0" ) + "\">主页</a>" );
        builder.Append( "|" );
        builder.Append( pType == 1 ? "论坛页" : "<a href=\"" + Utils.getUrl( "Slider.aspx?ptype=1" ) + "\">论坛页</a>" );
        builder.Append( Out.Tab( "</div>", "" ) );



        switch(pAct)
        {
            case "addFile":      //上传文件 
                this.AddFile();
                break;
            case "UploadFile":      //上传文件 
                this.UploadFile();
                break;
            case "EditFile":      //编辑 
                this.EditFile();
                break;
            case "SaveFile":        //保存修改
                this.SaveFile();
                break;
            case "delFile":      //删除文件 
                this.DelFile();
                break;
            default:            //文件列表
                this.ListFile();
                break;             
        }         
                                                                       

        
        builder.Append( Out.Tab( "</div><div class=\"title\"><a href=\"" + Utils.getUrl( "../default.aspx" ) + "\">返回App管理中心</a>", "<a href=\"" + Utils.getUrl( "default.aspx" ) + "\">返回管理中心</a>" ) );
        builder.Append( Out.Tab( "</div>", "<br />" ) );
    }

    private void AddFile()
    {
         string strText = string.Empty;
         string strName = string.Empty;
         string strType = string.Empty;
         string strValu = string.Empty;
         string strEmpt = string.Empty;
         string strIdea = string.Empty;
         string strOthe = string.Empty;

         Master.Title = "上传文件";

         builder.Append( Out.Tab( "<div>", "" ) );
         builder.Append( "上传允许格式:.jpg,.jpeg,.png,.bmp<br />" );
         builder.Append( Out.Tab( "</div>", "" ) );

         string sUpType = string.Empty;
         string sText = string.Empty;
         string sName = string.Empty;
         string sType = string.Empty;
         string sValu = string.Empty;
         string sEmpt = string.Empty;

         sText = "内容类型:/,参数(|分隔多个)/,选择附件:/,,";
         sName = "contentType,params,fileName,backurl";
         sType = "text,text,file,hidden";
         sValu = "" + "'''";
         sEmpt = "false,false,false,";
         strOthe = "上传,"+Utils.getUrl("Slider.aspx?ptype="+pType+"&act=UploadFile")+",post,2,blue";

         builder.Append( Out.wapform( sText, sName, sType, sValu, sEmpt, strIdea, strOthe ) );

         builder.Append( Out.Tab( "<div>", "" ) );
         builder.Append( "返回上一页" );
         builder.Append( Out.Tab( "</div>", "" ) );
    }

    private void EditFile()
    {
        int _id = Utils.ParseInt(Utils.GetRequest("id","all",1,@"^\d*$","0"));

        
        Master.Title = "编辑内容";

        BCW.MobileSlider.Model.MobileSlider _data = new BCW.MobileSlider.BLL.MobileSlider().GetModel( _id );
        if( _data == null )
        {
            Utils.Error( "参数错误", "" );
        }   

        string strOthe = string.Empty;
        string strIdea = string.Empty;
        string sUpType = string.Empty;
        string sText = string.Empty;
        string sName = string.Empty;
        string sType = string.Empty;
        string sValu = string.Empty;
        string sEmpt = string.Empty;

        sText = "内容类型:/,参数(|分隔多个)/,,";
        sName = "id,contentType,params,backurl";
        sType = "hidden,text,text,hidden";
        sValu = ""+_data.id+"'" + _data .contentType+ "'"+_data.param+"'";
        sEmpt = "false,false,false,";
        strOthe = "确定修改," + Utils.getUrl( "Slider.aspx?ptype=" + pType + "&act=SaveFile" ) + ",post,2,blue";

        builder.Append( Out.wapform( sText, sName, sType, sValu, sEmpt, strIdea, strOthe ) );

        builder.Append( Out.Tab( "<div>", "" ) );
        builder.Append( "<a href=\""+Utils.getUrl( "Slider.aspx?ptype=" + pType)+"\">取消修改</a>" );
        builder.Append( Out.Tab( "</div>", "" ) );
    }

    private void SaveFile()
    {
        int _id = Utils.ParseInt( Utils.GetRequest( "id", "post", 1, @"^\d*$", "0" ) );
        string _contentType = Utils.GetRequest( "contentType", "post", 1, "", "" );
        string _params = Utils.GetRequest( "params", "post", 1, "", "" );

        BCW.MobileSlider.Model.MobileSlider _data = new BCW.MobileSlider.BLL.MobileSlider().GetModel( _id );
        if( _data == null )
        {
            Utils.Error( "参数错误", "" );
        }

        _data.contentType = _contentType;
        _data.param = _params;
        new BCW.MobileSlider.BLL.MobileSlider().Update( _data );

        Utils.Success( "编辑内容", "修改成功", Utils.getUrl( "Slider.aspx?ptype=" + pType ), "2" );

    }

    private void DelFile()
    {
                    
        int _id = Utils.ParseInt( Utils.GetRequest( "id", "get", 1, @"^\d*$", "0" ) );
        BCW.MobileSlider.Model.MobileSlider _data = new BCW.MobileSlider.BLL.MobileSlider().GetModel( _id );
        if( _data == null )
        {
            Utils.Error( "参数错误", "" );
        }

        builder.Append( Out.Tab( "<div>", "" ) );
        builder.Append( System.Web.HttpContext.Current.Request.MapPath( _data.url ) );
        builder.Append( Out.Tab( "</div>", "" ) );

        if( FileTool.DeleteFile(  _data.url  ) == true )
        {
            new BCW.MobileSlider.BLL.MobileSlider().Delete( _id );
            Utils.Success( "删除文件", "成功删除文件", Utils.getUrl( "Slider.aspx?ptype=" + pType ), "2" );
        }

    }

    private void ListFile()
    {
        builder.Append( Out.Tab( "<div class=\"text\">", "" ) );
        builder.Append( "<a href=\"" + Utils.getUrl( "Slider.aspx?act=addFile&ptype="+pType ) + "\">添加文件...</a><br />" );
        builder.Append( Out.Tab( "</div>", "" ) );

        builder.Append( Out.Tab( "<div>", "" ) );
        DataSet _ds = new BCW.MobileSlider.BLL.MobileSlider().GetList( "ptype=" + ( int ) pType );

        for( int i = 0; i < _ds.Tables[ 0 ].Rows.Count; i++ )
        {    

            builder.Append("url:"+_ds.Tables[ 0 ].Rows[ i ][ "url" ].ToString()+" <br />");
            builder.Append( "contentType:" + _ds.Tables[ 0 ].Rows[ i ][ "contentType" ].ToString() + " <br />" );
            builder.Append( "param:" + _ds.Tables[ 0 ].Rows[ i ][ "param" ].ToString() + " <br />" );
            builder.Append( "<a href=\"" + Utils.getUrl( _ds.Tables[ 0 ].Rows[ i ][ "url" ].ToString()) + "\">查看图片</a>" );
            builder.Append( "|" );
            builder.Append( "<a href=\"" + Utils.getUrl( "Slider.aspx?act=EditFile&ptype=" + pType +"&id="+ _ds.Tables[ 0 ].Rows[ i ][ "id" ].ToString() ) + "\">编辑参数</a>" );
            builder.Append( "|" );
            builder.Append( "<a href=\"" + Utils.getUrl( "Slider.aspx?act=delFile&ptype=" + pType + "&id=" + _ds.Tables[ 0 ].Rows[ i ][ "id" ].ToString() ) + "\">删除</a><br /><br />" );
        }

        builder.Append( Out.Tab( "</div>", "" ) );
    }

    private void UploadFile()
    {
        string ac = Utils.ToSChinese( Utils.GetRequest( "ac", "post", 1, "", "" ) );
        string _contentType = Utils.GetRequest( "contentType", "post", 1, "", "" );
        string _params = Utils.GetRequest( "params", "post", 1, "", "" );

        int meid = new BCW.User.Users().GetUsId();

        if( string.IsNullOrEmpty( _contentType ) )
            Utils.Error( "内容类型不能为空", "" );

        if( string.IsNullOrEmpty( _params ) )
            Utils.Error( "参数不能为空", "" );                                    

        if( ac == "上传" )
        {
            //int ManageId = new BCW.User.Manage().IsManageLogin();
            //if( ManageId != 1 && ManageId != 9 )
            //{
            //    BCW.User.Users.ShowVerifyRole( "f", meid );//非验证会员提示
            //    new BCW.User.Limits().CheckUserLimit( BCW.User.Limits.enumRole.Role_Upfile, meid );//会员上传权限 
            //}
                
            this.SaveFiles(_contentType,_params);  
            Utils.Success( "上传成功", "上传成功,正在返回...", Utils.getUrl( "Slider.aspx?ptype=" + pType ), "2" );
            //Utils.Success( "文件回帖", "回复文件成功！" "2222", ReplaceWap( Utils.getUrl( "reply.aspx?forumid=" + forumid + "&amp;bid=" + bid + "" ) ), "2" );
        }
    }

    /// <summary>
    /// 上传文件
    /// </summary>
    private void SaveFiles( string _contentType ,string _params)
    {

        int AddNum = 0;

        //遍历File表单元素
        System.Web.HttpFileCollection files = System.Web.HttpContext.Current.Request.Files;
        //int j = 1;
        int j = files.Count;
        try
        {
            string GetFiles = string.Empty;
            for( int iFile = files.Count - 1; iFile > -1; iFile-- )
            {
                //检查文件扩展名字
                System.Web.HttpPostedFile postedFile = files[ iFile ];
                string fileName, fileExtension;
                fileName = System.IO.Path.GetFileName( postedFile.FileName );//上传的文件名字
                string UpExt = ".jpg,.jpeg,.png,.bmp";//文件格式a
                int UpLength = Convert.ToInt32( ub.GetSub( "UpaMaxFileSize", xmlPath ) );//文件大小限制
                if( fileName != "" )
                {
                    fileExtension = System.IO.Path.GetExtension( fileName ).ToLower();
                    //检查是否允许上传格式
                    if( UpExt.IndexOf( fileExtension ) == -1 )
                    {
                        continue;
                    }
                    //非法上传
                    if( fileExtension == ".asp" || fileExtension == ".aspx" || fileExtension == ".jsp" || fileExtension == ".php" || fileExtension == ".asa" || fileExtension == ".cer" || fileExtension == ".cdx" || fileExtension == ".htr" || fileExtension == ".exe" )
                    {
                        continue;
                    }
                    if( postedFile.ContentLength > Convert.ToInt32( UpLength * 1024 ) )   //超过文件大小限制
                    {
                        continue;
                    }
                    string DirPath = string.Empty;
                    string prevDirPath = string.Empty;
                    string Path = "/Files/Mobile/Slider";  
                    int IsVerify = 0;
                    if( FileTool.CreateDirectory( Path, out DirPath ) )
                    {                        
                        //生成随机文件名
                        fileName = DT.getDateTimeNum() + iFile + fileExtension;//现在系统时间+数组下标+文件后缀名
                        string SavePath = System.Web.HttpContext.Current.Request.MapPath( DirPath ) + fileName;
                        postedFile.SaveAs( SavePath );

                        //=============================图片木马检测,包括TXT===========================
                        string vSavePath = SavePath;
                        if(fileExtension == ".jpg" || fileExtension == ".jpeg" || fileExtension == ".png" || fileExtension == ".bmp" )  //加点
                        {
                            bool IsPass = true;
                            System.IO.StreamReader sr = new System.IO.StreamReader( vSavePath, System.Text.Encoding.Default );
                            string strContent = sr.ReadToEnd().ToLower();
                            sr.Close();
                            string str = "system.|request|javascript|script |script>|.getfolder|.createfolder|.deletefolder|.createdirectory|.deletedirectory|.saveas|wscript.shell|script.encode|server.|.createobject|execute|activexobject|language=";
                            foreach( string s in str.Split( '|' ) )
                            {
                                if( strContent.IndexOf( s ) != -1 )
                                {
                                    System.IO.File.Delete( vSavePath );
                                    IsPass = false;
                                    break;
                                }
                            }
                            if( IsPass == false )
                                continue;
                        }
                        //=============================图片木马检测完毕,包括TXT===========================   

                        BCW.MobileSlider.Model.MobileSlider model = new BCW.MobileSlider.Model.MobileSlider();
                        model.url = DirPath + fileName;
                        model.contentType =_contentType;
                        model.param = _params;
                        model.ptype = pType;
                        new BCW.MobileSlider.BLL.MobileSlider().Add( model );

                        DataSet _ds = new BCW.MobileSlider.BLL.MobileSlider().GetList( 1, "", "id desc" );
                        if( _ds.Tables[ 0 ].Rows.Count > 0 )
                        {
                            int mid = int.Parse( _ds.Tables[ 0 ].Rows[ 0 ][ "id" ].ToString() );
                            BCW.MobileSlider.Model.MobileSlider _model2 = new BCW.MobileSlider.BLL.MobileSlider().GetModel( mid );
                            _model2.sortid = _model2.id;
                            new BCW.MobileSlider.BLL.MobileSlider().Update( _model2 ); 
                        }                             
                    }
                    //j++;
                    j--;
                }
            }

        }
        catch
        {
        }
    }
}
