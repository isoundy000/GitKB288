using System;
using System.Collections.Generic;
using System.Text;

namespace TPR2.Model.Http
{
    /// <summary>
    /// 实体类NBA 。(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public class Nba
    {
        public Nba()
        { }
        #region Model
        private string _txtlivenbalist;
        private string _txtlivenbapage;
        private string _txtlivenbaview;
        /// <summary>
        /// NBA文字直播列表
        /// </summary>
        public string txtLivenbalist
        {
            set { _txtlivenbalist = value; }
            get { return _txtlivenbalist; }
        }

        /// <summary>
        /// NBA文字直播列表
        /// </summary>
        public string txtLivenbapage
        {
            set { _txtlivenbapage = value; }
            get { return _txtlivenbapage; }
        }

        /// <summary>
        /// NBA文字直播列表
        /// </summary>
        public string txtLivenbaview
        {
            set { _txtlivenbaview = value; }
            get { return _txtlivenbaview; }
        }
        #endregion Model

    }
}
