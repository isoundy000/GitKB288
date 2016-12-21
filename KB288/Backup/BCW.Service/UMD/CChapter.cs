namespace BCW.Service
{
    using System;
    using System.Collections;
    using System.Drawing;

    public class CChapter
    {
        private string content;
        private ArrayList imagelist;
        private string title;

        public CChapter()
        {
            this.content = string.Empty;
            this.title = string.Empty;
            this.imagelist = new ArrayList();
        }

        public CChapter(string title, string content)
        {
            this.content = string.Empty;
            this.title = string.Empty;
            this.imagelist = new ArrayList();
            this.title = title;
            this.content = content;
        }

        public void AppendImage(Image picture)
        {
            this.imagelist.Add(picture);
        }

        public void RemoveImage(int index)
        {
            if ((index >= 0) && (index < this.imagelist.Count))
            {
                this.imagelist.RemoveAt(index);
            }
        }

        public override string ToString()
        {
            return this.title;
        }

        public string Content
        {
            get
            {
                return this.content;
            }
            set
            {
                this.content = value;
            }
        }

        public ArrayList ImageList
        {
            get
            {
                return this.imagelist;
            }
            set
            {
                this.imagelist = value;
            }
        }

        public string Title
        {
            get
            {
                return this.title;
            }
            set
            {
                this.title = value;
            }
        }
    }
}
