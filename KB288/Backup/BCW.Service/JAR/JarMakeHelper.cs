using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using BCW.Common;

namespace BCW.Service
{
 
    public sealed class JarMakeHelper
    {
        private EBook book = new EBook();
        private string MANIFEST = "Manifest-Version: 1.0\r\nMicroEdition-Configuration: CLDC-1.0\r\nMIDlet-Name: {name}\r\nCreated-By: 1.4.2_09 (Sun Microsystems Inc.)\r\nMIDlet-Vendor: {author}\r\nMIDlet-1: {name}, /0.png, MBook\r\nMIDlet-Version: 1.0\r\nMicroEdition-Profile: MIDP-1.0\r\n";

        public JarMakeHelper(EBook b)
        {
            this.book = b;
        }
        public void Make(string JarName)
        {
            using (ZipOutputStream bookstream = new ZipOutputStream(File.Create(JarName)))
            {
                bookstream.SetLevel(5);
                //生成头部信息说明文档
                CreatMANIFEST(bookstream);
                //将需要的class文件复制过来
                CreatClasses(bookstream);
                //生产数据文件
                CreatDataContent(bookstream);
                //创建小说，索引 最重要的难点
                CreatIndex(bookstream);
                bookstream.Finish();
                bookstream.Close();
            }
        }

        /// <summary>
        /// 生成小说对应章节的数据文件
        /// </summary>
        /// <param name="bookstream"></param>
        private void CreatDataContent(ZipOutputStream bookstream)
        {
            int i = 1;
            foreach (Chapter c in book.MyContent)
            {
                ZipEntry entry = new ZipEntry(i++.ToString());
                byte[] buffer = Encoding.Unicode.GetBytes(c.Content);
                entry.Size = buffer.Length;
                bookstream.PutNextEntry(entry);
                bookstream.Write(buffer, 0, buffer.Length);
                bookstream.Flush();
                bookstream.CloseEntry();
            }
        }

        /// <summary>
        /// 将需要的class文件和图片加载进来
        /// </summary>
        /// <param name="bookstream"></param>
        private void CreatClasses(ZipOutputStream bookstream)
        {
            FileStream file = null;
            ZipEntry entry = null;
            foreach (string s in Directory.GetFiles(System.Web.HttpContext.Current.Server.MapPath("~/Files/sys/JarMake/")))
            {
                file = File.OpenRead(s);
                byte[] buffer = new byte[file.Length];
                file.Read(buffer, 0, buffer.Length);
                entry = new ZipEntry(Path.GetFileName(s));
                entry.Size = file.Length;
                file.Close();
                bookstream.PutNextEntry(entry);
                bookstream.Write(buffer, 0, buffer.Length);
                bookstream.Flush();
                bookstream.CloseEntry();
            }
        }
        /// <summary>
        /// 生成小说的索引文件
        /// </summary>
        /// <param name="bookstream"></param>
        private void CreatIndex(ZipOutputStream bookstream)
        {
            List<byte> list = new List<byte>();
            list.Add(0x00);
            list.Add(0x01);
            list.Add(0x30);
            string str = "";
            int count = 0;
            int num2 = 0;
            using (MemoryStream stream = new MemoryStream())
            {
                //书名头部信息的写入
                stream.Write(list.ToArray(), 0, 3);
                byte[] bytes = Encoding.UTF8.GetBytes(book.Name);
                count = bytes.Length;
                num2 = ((count << 8) & 0xff00) + ((count >> 0x10) & 0xff);
                stream.WriteByte((byte)(num2 % 0x100));
                stream.WriteByte((byte)(num2 / 0x100));
                stream.Write(bytes, 0, bytes.Length);
                /*^^^^^^^^^头部书名信息结束^^^^^^^^^^^*/
                if ((this.book.MyContent == null) || (this.book.MyContent.Count < 1))
                {
                    return;
                }
                //书本的内容数量
                str = this.book.MyContent.Count.ToString();
                count = str.Length;
                num2 = ((count << 8) & 0xff00) + ((count >> 0x10) & 0xff);
                stream.WriteByte((byte)(num2 % 0x100));
                stream.WriteByte((byte)(num2 / 0x100));
                stream.Write(Encoding.Default.GetBytes(str), 0, count);
                /*书本中章节数量 写入完毕*/
                //章节标题信息写入
                for (int i = 0; i < this.book.MyContent.Count; i++)
                {
                    str = string.Format("{0},{1},{2}", i + 1, (this.book.MyContent[i].Content.Length * 2) + 2, this.book.MyContent[i].ChapterTitle);
                    count = Encoding.UTF8.GetBytes(str).Length;
                    num2 = ((count << 8) & 0xff00) + ((count >> 0x10) & 0xff);
                    stream.WriteByte((byte)(num2 % 0x100));
                    stream.WriteByte((byte)(num2 / 0x100));
                    stream.Write(Encoding.UTF8.GetBytes(str), 0, count);
                }
                str = string.Format("{0}\r\n作者:{1}\r\n制作:{2}", this.book.Name, this.book.Creator, Utils.GetDomain());
                bytes = Encoding.UTF8.GetBytes(str);
                count = bytes.Length;
                stream.WriteByte((byte)((count >> 0x18) & 0xff));
                stream.WriteByte((byte)((count >> 0x10) & 0xff));
                stream.WriteByte((byte)((count >> 8) & 0xff));
                stream.WriteByte((byte)(count & 0xff));
                stream.Write(bytes, 0, count);
                ZipEntry entry = new ZipEntry("0");
                entry.Size = stream.Length;
                bookstream.PutNextEntry(entry);
                bookstream.Write(stream.ToArray(), 0, (int)stream.Length);
                bookstream.Flush();
                bookstream.CloseEntry();
            }

        }
        /// <summary>
        /// 生成头部信息文档
        /// </summary>
        /// <param name="bookstream"></param>
        private void CreatMANIFEST(ZipOutputStream bookstream)
        {
            string s = this.MANIFEST.Replace("{name}", book.Name);
            s = s.Replace("{author}", book.Creator);
            byte[] buffer = Encoding.UTF8.GetBytes(s.ToCharArray());
            ZipEntry entry = new ZipEntry("META-INF/MANIFEST.MF");
            entry.Size = buffer.Length;
            bookstream.PutNextEntry(entry);
            bookstream.Write(buffer, 0, buffer.Length);
            bookstream.Flush();
            bookstream.CloseEntry();
        }
    }
}
