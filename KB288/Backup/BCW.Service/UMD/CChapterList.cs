namespace BCW.Service
{
    using System;
    using System.Collections;
    using System.Reflection;

    public class CChapterList : CollectionBase
    {
        public int Add(CChapter chapter)
        {
            return base.List.Add(chapter);
        }

        public int IndexOf(string title)
        {
            for (int i = 0; i < base.List.Count; i++)
            {
                if (this[i].Title == title)
                {
                    return i;
                }
            }
            return -1;
        }

        public void Insert(int index, CChapter chapter)
        {
            base.List.Insert(index, chapter);
        }

        public void Remove(string title)
        {
            int index = this.IndexOf(title);
            if (index != -1)
            {
                base.RemoveAt(index);
            }
        }

        public CChapter this[int index]
        {
            get
            {
                return (CChapter) base.List[index];
            }
            set
            {
                base.List[index] = value;
            }
        }
    }
}
