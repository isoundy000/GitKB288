using System;
using System.Collections.Generic;
using System.Text;

namespace TPR3.Model.Http
{
    /// <summary>
    /// ʵ����Live ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
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
        /// ����ʱ�ȷ�
        /// </summary>
        public string txtLivezlist
        {
            set { _txtlivezlist = value; }
            get { return _txtlivezlist; }
        }
        /// <summary>
        /// δ��/����/�곡���¼�¼
        /// </summary>
        public string txtLiveState
        {
            set { _txtlivestate = value; }
            get { return _txtlivestate; }
        }
        /// <summary>
        /// ��ϸ��¼
        /// </summary>
        public string txtLiveView
        {
            set { _txtliveview = value; }
            get { return _txtliveview; }
        }
        #endregion Model

    }
}
