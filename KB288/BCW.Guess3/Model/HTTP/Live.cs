using System;
using System.Collections.Generic;
using System.Text;

namespace TPR3.Model.Http
{
    /// <summary>
    /// 实体类Live 。(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public class Live
    {
        public Live()
        { }
        #region Model
        private string _txtlivezlist;
        private string _txtlivestate;
        private string _txtliveview;

        /// <summary>
        /// 足球即时比分
        /// </summary>
        public string txtLivezlist
        {
            set { _txtlivezlist = value; }
            get { return _txtlivezlist; }
        }
        /// <summary>
        /// 未开/进行/完场赛事记录
        /// </summary>
        public string txtLiveState
        {
            set { _txtlivestate = value; }
            get { return _txtlivestate; }
        }
        /// <summary>
        /// 详细记录
        /// </summary>
        public string txtLiveView
        {
            set { _txtliveview = value; }
            get { return _txtliveview; }
        }
        #endregion Model

    }
}
