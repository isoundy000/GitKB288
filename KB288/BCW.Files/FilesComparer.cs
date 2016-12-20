namespace BCW.Files
{
    using BCW.Files.Model;
    using System;
    using System.Collections.Generic;

    public class FilesComparer : IComparer<FileFolderInfo>
    {
        private string _sortColumn;

        public FilesComparer(string sortExpression)
        {
            this._sortColumn = sortExpression;
        }

        public int Compare(FileFolderInfo a, FileFolderInfo b)
        {
            switch (this._sortColumn.ToLower())
            {
                case "name":
                    return string.Compare(a.Name, b.Name);

                case "ext":
                    return string.Compare(a.Ext, b.Ext);

                case "size":
                    return string.Compare(a.Size, b.Size);

                case "modifydate":
                    return DateTime.Compare(a.ModifyDate, b.ModifyDate);
            }
            return string.Compare(a.Name, b.Name);
        }
    }
}
