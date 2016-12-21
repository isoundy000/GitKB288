using System;
using System.Collections.Generic;
using System.Text;

namespace TPR.Model.Http
{
    /// <summary>
    /// 实体类Live 。(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public class Live4
    {
        public Live4()
        { }
        #region Model
        private string _txtlive2llist;
        private string _txtlive2llists;
        private string _txtlive2view;

        /// <summary>
        /// 篮球完场比分/一周赛事
        /// </summary>
        public string txtLive2llist
        {
            set { _txtlive2llist = value; }
            get { return _txtlive2llist; }
        }
        /// <summary>
        /// 篮球完场比分/一周赛事子列表
        /// </summary>
        public string txtLive2llists
        {
            set { _txtlive2llists = value; }
            get { return _txtlive2llists; }
        }
        /// <summary>
        /// 详细记录
        /// </summary>
        public string txtLive2View
        {
            set { _txtlive2view = value; }
            get { return _txtlive2view; }
        }
        #endregion Model

    }
}
