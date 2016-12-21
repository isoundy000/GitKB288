using System;
using System.Web;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using BCW.Common;

namespace BCW.Service
{
    public class CreateCHM
    {
        private ChmBook book = new ChmBook();
        public CreateCHM()
        {
        }
        public CreateCHM(ChmBook b)
        {
            this.book = b;
        }
        string startPath = "";

        string hhcFile = @"C:\Program Files\HTML Help Workshop\hhc.exe";//hhc.exe文件位置，windows自带的，一般是这个路径
        public string _defaultTopic = "chmfile/1.html";
        StreamWriter streamWriter;

        private bool Compile()
        {
            string _chmFile = startPath + @"\" + this.book.FileName + ".chm";//chm文件存储路径
            Process helpCompileProcess = new Process();  //创建新的进程，用Process启动HHC.EXE来Compile一个CHM文件
            try
            {
                //判断文件是否存在并不被占用
                try
                {
                    string path = _chmFile;  //chm生成路径
                    if (File.Exists(path))
                    {
                        File.Delete(path);
                    }
                }
                catch
                {
                    throw new Exception("文件被打开！");
                }


                ProcessStartInfo processStartInfo = new ProcessStartInfo();
                processStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                processStartInfo.FileName = hhcFile;  //调入HHC.EXE文件 
                processStartInfo.Arguments = "\"" + GetPathToProjectFile() + "\"";//获取空的HHP文件
                processStartInfo.UseShellExecute = false;
                helpCompileProcess.StartInfo = processStartInfo;
                helpCompileProcess.Start();
                helpCompileProcess.WaitForExit(); //组件无限期地等待关联进程退出

                if (helpCompileProcess.ExitCode == 0)
                {
                    return false;
                }

            }
            finally
            {
                helpCompileProcess.Close();
            }
            return true;
        }

        public void OpenHhp(string title)
        {
            FileStream fs = new FileStream(GetPathToProjectFile(), FileMode.Create); //创建hhp文件
            streamWriter = new System.IO.StreamWriter(fs, System.Text.Encoding.GetEncoding("GB18030"));
            streamWriter.WriteLine("[OPTIONS]");
            streamWriter.WriteLine("Title=" + title);
            streamWriter.WriteLine("Compatibility=1.1 or later");
            streamWriter.WriteLine("Compiled file=" + GetCompiledHtmlFilename());  //chm文件名
            streamWriter.WriteLine("Contents file=" + GetContentsHtmlFilename());  //hhc文件名
            streamWriter.WriteLine("Index file=" + this.book.FileName + ".hhk");
            streamWriter.WriteLine("Default topic=" + _defaultTopic);  //默认页
            streamWriter.WriteLine("Display compile progress=NO"); //是否显示编译过程
            streamWriter.WriteLine("Language=0x804 中文(中国)");  //chm文件语言
            streamWriter.WriteLine("Default Window=Main");
            streamWriter.WriteLine();
            streamWriter.WriteLine("[WINDOWS]");
            streamWriter.WriteLine("Main=" + this.book.FileName + "\",\"" + this.book.FileName + ".hhc\",\"" + this.book.FileName + ".hhk\",,,,,,,0x20,180,0x104E, [80,60,720,540],0x0,0x0,,,,,0");//这里最重要了，一般默认即可
            streamWriter.WriteLine();
            streamWriter.WriteLine("[FILES]");
            for (int i = 0; i < this.book.MyContent.Count; i++)
            {
                streamWriter.WriteLine("" + this.book.MyContent[i].Content + ".html");
            }
            streamWriter.WriteLine();
            streamWriter.Close();
        }

        /// <summary>
        /// 目录文件
        /// </summary>
        private void OpenHhc()
        {
            FileStream fs = new FileStream(GetContentsHtmlFilename(), FileMode.Create); //创建hhp文件
            streamWriter = new System.IO.StreamWriter(fs, System.Text.Encoding.GetEncoding("GB18030"));
            streamWriter.WriteLine("<!DOCTYPE HTML PUBLIC \"-//IETF//DTD HTML//EN\">");
            streamWriter.WriteLine("<HTML>");
            streamWriter.WriteLine("<HEAD>");
            streamWriter.WriteLine("<meta name=\"GENERATOR\" content=\"Microsoft&reg; HTML Help Workshop 4.1\">");
            streamWriter.WriteLine("<!-- Sitemap 1.0 -->");
            streamWriter.WriteLine("</HEAD>");
            streamWriter.WriteLine("<BODY>");
            streamWriter.WriteLine("<OBJECT type=\"text/site properties\">");
            streamWriter.WriteLine("<param name=\"Window Styles\" value=\"0x237\">");
            streamWriter.WriteLine("</OBJECT>");
            //父目录
            streamWriter.WriteLine("<UL>");
            streamWriter.WriteLine("<LI> <OBJECT type=\"text/sitemap\">");
            streamWriter.WriteLine("<param name=\"Name\" value=\"" + this.book.Name + "\">");
            streamWriter.WriteLine("</OBJECT>");
            streamWriter.WriteLine("<UL>");
            for (int i = 0; i < this.book.MyContent.Count; i++)
            {
                //子目录
                streamWriter.WriteLine("<LI><OBJECT type=\"text/sitemap\">");
                streamWriter.WriteLine("<param name=\"Name\" value=\"" + this.book.MyContent[i].ChapterTitle + "\">");
                streamWriter.WriteLine("<param name=\"Local\" value=\"" + this.book.MyContent[i].Content + ".html\">");
                streamWriter.WriteLine("</OBJECT>");
            }
            streamWriter.WriteLine("</UL>");
            streamWriter.WriteLine("</UL>");
            streamWriter.WriteLine("</BODY>");
            streamWriter.WriteLine("</HTML>");
            streamWriter.WriteLine();
            streamWriter.Close();
        }

        /// <summary>
        /// 索引文件
        /// </summary>
        private void OpenHhk()
        {
            FileStream fs = new FileStream(startPath + @"\" + this.book.FileName + ".hhk", FileMode.Create); //创建hhp文件
            streamWriter = new System.IO.StreamWriter(fs, System.Text.Encoding.GetEncoding("GB18030"));
            streamWriter.WriteLine("<!DOCTYPE HTML PUBLIC \"-//IETF//DTD HTML//EN\">");
            streamWriter.WriteLine("<HTML>");
            streamWriter.WriteLine("<HEAD>");
            streamWriter.WriteLine("<meta name=\"GENERATOR\" content=\"Microsoft&reg; HTML Help Workshop 4.1\">");
            streamWriter.WriteLine("<!-- Sitemap 1.0 -->");
            streamWriter.WriteLine("</HEAD>");
            streamWriter.WriteLine("<BODY>");
            streamWriter.WriteLine("<UL>");
            streamWriter.WriteLine("<LI> <OBJECT type=\"text/sitemap\">");
            for (int i = 0; i < this.book.MyContent.Count; i++)
            {
                streamWriter.WriteLine("<param name=\"Name\" value=\"" + this.book.MyContent[i].ChapterTitle + "\">");
                streamWriter.WriteLine("<param name=\"Local\" value=\"" + this.book.MyContent[i].Content + ".html\">");
            }
            streamWriter.WriteLine("</OBJECT>");
            streamWriter.WriteLine("</UL>");

            streamWriter.WriteLine("</BODY>");
            streamWriter.WriteLine("</HTML>");
            streamWriter.WriteLine();
            streamWriter.Close();
        }

        /// <summary>
        /// 创建HTML文件
        /// </summary>
        public void CreateHtml(string fileName, string Author, string Title, string Content)
        {
            FileStream fs = new FileStream(fileName, FileMode.Create);//文件名
            StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.GetEncoding("UTF-8"));
            sw.WriteLine("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">");
            sw.WriteLine("<html xmlns=\"http://www.w3.org/1999/xhtml\">");
            sw.WriteLine("<head>");
            sw.WriteLine("<style>");
            sw.WriteLine("<!--");
            sw.WriteLine("body {  line-height: 20px}");
            sw.WriteLine("td {  line-height: 20px}");
            sw.WriteLine(".Paragraph{ font-size: 9pt }");
            sw.WriteLine("A:link   {text-decoration: none; color:#0033CC}");
            sw.WriteLine("A:visited  {text-decoration: none; color: #0033CC}");
            sw.WriteLine("A:active {text-decoration: none; color: #0000ff }");
            sw.WriteLine("A:hover {text-decoration: underline; color: #FF0000 }");
            sw.WriteLine("-->");
            sw.WriteLine("</style>");
            sw.WriteLine("<title>" + Title + "</title>");//文章名
            sw.WriteLine("<meta content=\"text/html; charset=utf-8\" http-equiv=\"content-type\"/>");
            sw.WriteLine("</head>");
            sw.WriteLine("<body bgcolor=\"#FFFFFF\"><div align=\"center\"><center><table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"80%\"><tr><td width=\"80%\" bgcolor=\"#000000\"><table border=\"0\" cellspacing=\"1\" width=\"100%\" cellpadding=\"6\" height=\"98\"><tr bgcolor=\"#FFCC00\"><td width=\"100%\" height=\"21\"><p align=\"center\"><strong>" + Title + "</strong></td></tr><tr bgcolor=\"#FFF5D0\"><td width=\"100%\" height=\"51\" align=center><table><tr><td><p class=\"Paragraph\"><br>" + Content + "<br><br><br></p></td></tr></table></td></tr><tr><td width=\"100%\" bgcolor=\"#FFFFFD\" height=\"20\"><table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\"><tr><td width=\"100%\"><p align=\"right\" class=\"Paragraph\">" + Author + " </td></tr></table></td></tr></table></td></tr></table></center></div><p align=center><font size=1>Made by <a href=\"http://" + Utils.GetDomain() + "\" target=_blank>" + Utils.GetDomain() + "</a> create date:" + DateTime.Now + " </font></p>");
            sw.WriteLine("</body>");
            sw.WriteLine("</html>");
            sw.Close();

        }


        /// <summary>
        /// 获取hhp文件路径
        /// </summary>
        /// <returns></returns>
        private string GetPathToProjectFile()
        {
            return startPath + @"\" + this.book.FileName + ".hhp";
        }

        /// <summary>
        /// 获取含有列表的hhc文件
        /// </summary>
        /// <returns></returns>
        private string GetContentsHtmlFilename()
        {
            return startPath + @"\" + this.book.FileName + ".hhc";
        }

        /// <summary>
        /// 设置编译后的文件名
        /// </summary>
        /// <returns></returns>
        private string GetCompiledHtmlFilename()
        {
            return startPath + @"\" + this.book.FileName + ".chm";
        }

        public void MakeChm()
        {
            startPath = this.book.StartPath;//起始路径

            OpenHhp(this.book.FileName + ".chm");//打开hhp文件
            OpenHhc();//打开hhc文件
            OpenHhk();//打开hhk文件
            if (Compile())//编译成chm文件
            {
                //this is ok!
            }
            File.Delete(startPath + @"\" + this.book.FileName + ".hhc");
            File.Delete(startPath + @"\" + this.book.FileName + ".hhp");
            File.Delete(startPath + @"\" + this.book.FileName + ".hhk");
            Directory.Delete((startPath + @"\chmfile\"), true);
        }
    }
}
