using System;
using System.Collections.Generic;
using System.Text;

namespace BCW.HB.Model
{
    /// <summary>
    /// Shared:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class Shared
    {
        public Shared()
        { }
        #region Model
        private int _userid;
        private string _sharedidlist;
        private string _shareurl;
        private string _sharecontent;
        /// <summary>
        /// 
        /// </summary>
        public int UserID
        {
            set { _userid = value; }
            get { return _userid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string SharedIDList
        {
            set { _sharedidlist = value; }
            get { return _sharedidlist; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ShareUrl
        {
            set { _shareurl = value; }
            get { return _shareurl; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ShareContent
        {
            set { _sharecontent = value; }
            get { return _sharecontent; }
        }
        #endregion Model

    }
}

