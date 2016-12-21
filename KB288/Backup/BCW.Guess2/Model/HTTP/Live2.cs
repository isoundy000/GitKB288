using System;
using System.Collections.Generic;
using System.Text;

namespace TPR2.Model.Http
{
    /// <summary>
    /// 实体类Live 。(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public class Live2
    {
        public Live2()
        { }
        #region Model
        private string _txtlive2zlist;
        private string _txtlive2zlists;
        private string _txtlive2view;

        /// <summary>
        /// 足球完场比分/一周赛事
        /// </summary>
        public string txtLive2zlist
        {
            set { _txtlive2zlist = value; }
            get { return _txtlive2zlist; }
        }
        /// <summary>
        /// 足球完场比分/一周赛事子列表
        /// </summary>
        public string txtLive2zlists
        {
            set { _txtlive2zlists = value; }
            get { return _txtlive2zlists; }
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
