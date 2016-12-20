using System;
using System.Collections.Generic;
using System.Text;

namespace BCW.Service
{
    public class EBook
    {
        private string _name;
        private string _creator;
        public string Name
        {
            set { _name = value; }
            get { return _name; }
        }
        public string Creator
        {
            set { _creator = value; }
            get { return _creator; }
        }
        private List<Chapter> contents = new List<Chapter>();

        public void Add(Chapter chapter)
        {
            contents.Add(chapter);
        }
        public List<Chapter> MyContent
        {
            get { return contents; }
        }
    }
}
