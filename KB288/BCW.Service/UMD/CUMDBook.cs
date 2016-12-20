namespace BCW.Service
{
    using System;
    using System.Drawing;
    using System.IO;
    using System.Text;

    public class CUMDBook
    {
        private string _Book_Author = string.Empty;
        private CChapterList _Book_Chapters = null;
        private Image _Book_Cover = null;
        private Encoding _Book_Encoding = Encoding.Unicode;
        private string _Book_Kind = string.Empty;
        private string _Book_Path = string.Empty;
        private DateTime _Book_PublishDate = DateTime.Now;
        private string _Book_Publisher = string.Empty;
        private string _Book_Title = string.Empty;
        private byte[] _Book_Type = new byte[] { 1, 1 };
        private string _Book_Vendor = string.Empty;

        public void AppendChapter(CChapter chapter)
        {
            this._Book_Chapters.Add(chapter);
        }


        public CUMDBook()
        {
            if (_Book_Chapters == null)
                _Book_Chapters = new CChapterList();
        }

        /*
         *  外部调用实例       
         * CUMDBook umd = new CUMDBook();
        umd.Author = "作者";
        umd.BookKind = "自创天书";//书本类型
        umd.BookPath = Server.MapPath("/Files/1.umd");//保存路径
        umd.BookTitle = "我的电子书";//书名
        umd.Publisher = "nowtx";//出版社
        umd.Vendor = "nowtx.net";//供应商
        System.Drawing.Image cover = System.Drawing.Image.FromFile(Server.MapPath("/Files/cover.jpg"));
        umd.Cover = cover;//封面
        umd.PublishDate = DateTime.Now;//出版时间
        //umd.BookType = new byte[] { 1, 1 };//纯文本 /{ 1, 2 }图画/{ 1, 3 }文字加图画
        //for (int i = 0; i < 10; i++)
        //{
        //    CChapter c = new CChapter();
        //    c.Title = "123" + i + "";
        //    c.Content = "456" + i + "";
        //    umd.AppendChapter(c);
        //}
        //builder.Append(umd.CanSave());//验证输入
        UMDFactory.WriteUMDBook(umd);
         * */

        public string CanSave()
        {
            if ((this.BookTitle == null) || (this.BookTitle.Length < 1))
            {
                return "标题不能为空！";
            }
            if ((this.Author == null) || (this.Author.Length < 1))
            {
                return "作者不能为空！";
            }
            if ((this.Chapters == null) || (this.Chapters.Count < 1))
            {
                return "内容数量不能小于0！";
            }
            if ((this.BookPath == null) || (this.BookPath.Length < 1))
            {
                return "保存文件的路径不正确！";
            }
            if (File.Exists(this.BookPath))
            {
                try
                {
                    File.Delete(this.BookPath);
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception.Message);
                    return "无法覆盖，文件正在被使用！";
                }
            }
            return null;
        }

        public string Author
        {
            get
            {
                return this._Book_Author;
            }
            set
            {
                this._Book_Author = value;
            }
        }

        public Encoding BookEncoding
        {
            get
            {
                return this._Book_Encoding;
            }
        }

        public string BookKind
        {
            get
            {
                return this._Book_Kind;
            }
            set
            {
                this._Book_Kind = value;
            }
        }

        public string BookPath
        {
            get
            {
                return this._Book_Path;
            }
            set
            {
                this._Book_Path = value;
            }
        }

        public string BookTitle
        {
            get
            {
                return this._Book_Title;
            }
            set
            {
                this._Book_Title = value;
            }
        }

        public byte[] BookType
        {
            get
            {
                return this._Book_Type;
            }
            set
            {
                this._Book_Type = value;
            }
        }

        public CChapterList Chapters
        {
            get
            {
                return this._Book_Chapters;
            }
        }

        public Image Cover
        {
            get
            {
                return this._Book_Cover;
            }
            set
            {
                this._Book_Cover = value;
            }
        }

        public DateTime PublishDate
        {
            get
            {
                return this._Book_PublishDate;
            }
            set
            {
                this._Book_PublishDate = value;
            }
        }

        public string Publisher
        {
            get
            {
                return this._Book_Publisher;
            }
            set
            {
                this._Book_Publisher = value;
            }
        }

        public string Vendor
        {
            get
            {
                return this._Book_Vendor;
            }
            set
            {
                this._Book_Vendor = value;
            }
        }
    }
}
