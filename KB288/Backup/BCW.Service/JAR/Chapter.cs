using System;
using System.Collections.Generic;
using System.Text;

namespace BCW.Service
{
    public class Chapter
    {
        private string _chaptertitle;
        private string _content;
        public string ChapterTitle
        {
            set { _chaptertitle = value; }
            get { return _chaptertitle; }
        }
        public string Content
        {
            set { _content = value; }
            get { return _content; }
        }
        public Chapter(string title, string content)
        {
            this.ChapterTitle = title;
            this.Content = content;
        }
        public Chapter()
        { }
    }
}
