using System;
using System.Collections.Generic;
using System.Text;

namespace BCW.Service
{
    public class ChmBook
    {
        private string _name;
        private string _filename;
        private string _startpath;
        public string Name
        {
            set { _name = value; }
            get { return _name; }
        }
        public string FileName
        {
            set { _filename = value; }
            get { return _filename; }
        }
        public string StartPath
        {
            set { _startpath = value; }
            get { return _startpath; }
        }
        private List<ChmChapter> contents = new List<ChmChapter>();

        public void Add(ChmChapter ChmChapter)
        {
            contents.Add(ChmChapter);
        }
        public List<ChmChapter> MyContent
        {
            get { return contents; }
        }
    }
}
