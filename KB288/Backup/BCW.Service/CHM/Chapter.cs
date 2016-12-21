using System;
using System.Collections.Generic;
using System.Text;

namespace BCW.Service
{
    public class ChmChapter
    {
        private string _Chaptertitle;
        private string _content;
        public string ChapterTitle
        {
            set { _Chaptertitle = value; }
            get { return _Chaptertitle; }
        }
        public string Content
        {
            set { _content = value; }
            get { return _content; }
        }
        public ChmChapter(string title, string content)
        {
            this.ChapterTitle = title;
            this.Content = content;
        }
        public ChmChapter()
        { }
    }
}
