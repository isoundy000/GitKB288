using System;
using System.Collections.Generic;
using System.Text;

namespace TPR2.Model.Http
{
    /// <summary>
    /// ʵ����NBA ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
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
        /// NBA����ֱ���б�
        /// </summary>
        public string txtLivenbalist
        {
            set { _txtlivenbalist = value; }
            get { return _txtlivenbalist; }
        }

        /// <summary>
        /// NBA����ֱ���б�
        /// </summary>
        public string txtLivenbapage
        {
            set { _txtlivenbapage = value; }
            get { return _txtlivenbapage; }
        }

        /// <summary>
        /// NBA����ֱ���б�
        /// </summary>
        public string txtLivenbaview
        {
            set { _txtlivenbaview = value; }
            get { return _txtlivenbaview; }
        }
        #endregion Model

    }
}
