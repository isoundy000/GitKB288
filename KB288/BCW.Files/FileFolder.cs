namespace BCW.Files.Model
{
    using System;

    public class FileFolderInfo
    {
        private string _Ext;
        private string _FormatName;
        private string _FullName;
        private DateTime _ModifyDate;
        private string _Name;
        private string _Size;
        private string _Type;

        public string Ext
        {
            get
            {
                return this._Ext;
            }
            set
            {
                this._Ext = value;
            }
        }

        public string FormatName
        {
            get
            {
                return this._FormatName;
            }
            set
            {
                this._FormatName = value;
            }
        }

        public string FullName
        {
            get
            {
                return this._FullName;
            }
            set
            {
                this._FullName = value;
            }
        }

        public DateTime ModifyDate
        {
            get
            {
                return Convert.ToDateTime(this._ModifyDate.ToUniversalTime().AddHours(8.0).ToString("yyyy-MM-dd hh:mm:ss"));
            }
            set
            {
                this._ModifyDate = value;
            }
        }

        public string Name
        {
            get
            {
                return this._Name;
            }
            set
            {
                this._Name = value;
            }
        }

        public string Size
        {
            get
            {
                if (string.IsNullOrEmpty(this._Size))
                {
                    return string.Empty;
                }
                if (this._Size.Length < 8)
                {
                    return ((Convert.ToUInt32(this._Size) / 0x400) + " KB");
                }
                return (((Convert.ToUInt32(this._Size) / 0x400) / 0x400) + " MB");
            }
            set
            {
                this._Size = value;
            }
        }

        public string Type
        {
            get
            {
                return this._Type;
            }
            set
            {
                this._Type = value;
            }
        }
    }
}
