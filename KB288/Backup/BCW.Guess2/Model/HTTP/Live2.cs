using System;
using System.Collections.Generic;
using System.Text;

namespace TPR2.Model.Http
{
    /// <summary>
    /// ʵ����Live ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
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
        /// �����곡�ȷ�/һ������
        /// </summary>
        public string txtLive2zlist
        {
            set { _txtlive2zlist = value; }
            get { return _txtlive2zlist; }
        }
        /// <summary>
        /// �����곡�ȷ�/һ���������б�
        /// </summary>
        public string txtLive2zlists
        {
            set { _txtlive2zlists = value; }
            get { return _txtlive2zlists; }
        }
        /// <summary>
        /// ��ϸ��¼
        /// </summary>
        public string txtLive2View
        {
            set { _txtlive2view = value; }
            get { return _txtlive2view; }
        }
        #endregion Model

    }
}
