using System;
using System.Web;
using BCW.Common;
using BCW.Data;
/// <summary>
///Global 的摘要说明
/// </summary>

namespace Global
{
    public class Global : System.Web.HttpApplication
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        public Global()
        {
            InitializeComponent();
        }

        protected void Application_Start(Object sender, EventArgs e)
        {

            //启动线程更新
            //System.Threading.Thread dataImport = new System.Threading.Thread(new System.Threading.ThreadStart(DataImport));
            //dataImport.Start();
            //dataImport.IsBackground = true;
        }

        protected void Session_Start(Object sender, EventArgs e)
        {
            Application.Lock();
            //访问统计
            if (ub.Get("SiteVCountType") == "0")
            {
                BCW.Model.Statinfo model = new BCW.Model.Statinfo();
                model.IP = Utils.GetUsIP();
                model.PUrl = Utils.getPageUrl();
                model.Browser = Utils.GetBrowser();
                model.System = Utils.GetSystem();
                model.AddTime = DateTime.Now;
                new BCW.BLL.Statinfo().Add(model);
            }

            Application.UnLock();
        }

        protected void Application_BeginRequest(Object sender, EventArgs e)
        {
            //HttpContext.Current.Response.Cache.SetNoStore(); 
        }

        protected void Application_EndRequest(Object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(Object sender, EventArgs e)
        {

        }

        protected void Application_Error(Object sender, EventArgs e)
        {

        }

        protected void Session_End(Object sender, EventArgs e)
        {

        }

        protected void Application_End(Object sender, EventArgs e)
        {

        }

        #region Web Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
        }
        #endregion

        //****************以下为用户自定义*******************
        private static void DataImport()
        {

            while (true)
            {
                test();
                System.Threading.Thread.Sleep(600000);//1000为1秒
            }
        }
        private static void test()
        {
            //String sLogFilePath = System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "log.txt";
            //LogHelper.Write(sLogFilePath, "测试状态");
            
        }
    }
}