using System;
using System.Collections.Generic;
using System.Text;

namespace TPR2.Model.Http
{
 	/// <summary>
	/// ʵ����Live ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
	/// </summary>
	[Serializable]
	public class Live3
	{
		public Live3()
		{}
		#region Model
        private string _txtlivellist;
        private string _txtlivetlist;
        private string _txtliveview;

        /// <summary>
        /// ����ʱ�ȷ�
        /// </summary>
        public string txtLivellist
        {
            set { _txtlivellist = value; }
            get { return _txtlivellist; }
        }
        /// <summary>
        /// ��������ȫ������
        /// </summary>
        public string txtLivetlist
        {
            set { _txtlivetlist = value; }
            get { return _txtlivetlist; }
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
