using System;
using System.Collections.Generic;
using System.Text;

namespace TPR.Model.Http
{
    /// <summary>
    /// ʵ����Live ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
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
        /// �����곡�ȷ�/һ������
        /// </summary>
        public string txtLive2llist
        {
            set { _txtlive2llist = value; }
            get { return _txtlive2llist; }
        }
        /// <summary>
        /// �����곡�ȷ�/һ���������б�
        /// </summary>
        public string txtLive2llists
        {
            set { _txtlive2llists = value; }
            get { return _txtlive2llists; }
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
