using System;
namespace BCW.Model.Collec
{
	/// <summary>
	/// ʵ����CollecItem ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
	/// </summary>
	[Serializable]
	public class CollecItem
	{
		public CollecItem()
		{}
		#region Model
		private int _id;
		private string _itemname;
		private int _types;
		private int _nodeid;
		private int _webencode;
		private string _webname;
		private string _weburl;
		private string _itemremark;
		private string _listurl;
		private string _liststart;
		private string _listend;
		private string _linkstart;
		private string _linkend;
		private string _titlestart;
		private string _titleend;
		private string _keywordstart;
		private string _keywordend;
		private string _dateregex;
		private string _nextlistregex;
		private string _contentstart;
		private string _contentend;
		private string _removebodystart;
		private string _removebodyend;
		private string _removetitle;
		private string _removecontent;
		private string _nextpageregex;
		private string _script_html;
		private int _collecnum;
		private int _issaveimg;
		private int _isdesc;
		private int _state;
		/// <summary>
		/// ����ID
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// ��Ŀ����
		/// </summary>
		public string ItemName
		{
			set{ _itemname=value;}
			get{return _itemname;}
		}
		/// <summary>
		/// ��������
		/// </summary>
		public int Types
		{
			set{ _types=value;}
			get{return _types;}
		}
		/// <summary>
		/// ���������
		/// </summary>
		public int NodeId
		{
			set{ _nodeid=value;}
			get{return _nodeid;}
		}
		/// <summary>
		/// ҳ�����2=gb2312,1=utf-8,0=unicode
		/// </summary>
		public int WebEncode
		{
			set{ _webencode=value;}
			get{return _webencode;}
		}
		/// <summary>
		/// ��վ����
		/// </summary>
		public string WebName
		{
			set{ _webname=value;}
			get{return _webname;}
		}
		/// <summary>
		/// ��վ��ַ
		/// </summary>
		public string WebUrl
		{
			set{ _weburl=value;}
			get{return _weburl;}
		}
		/// <summary>
		/// ��Ŀ��ע
		/// </summary>
		public string ItemRemark
		{
			set{ _itemremark=value;}
			get{return _itemremark;}
		}
		/// <summary>
		/// �б�����ҳ��
		/// </summary>
		public string ListUrl
		{
			set{ _listurl=value;}
			get{return _listurl;}
		}
		/// <summary>
		/// �б�ʼ���
		/// </summary>
		public string ListStart
		{
			set{ _liststart=value;}
			get{return _liststart;}
		}
		/// <summary>
		/// �б�������
		/// </summary>
		public string ListEnd
		{
			set{ _listend=value;}
			get{return _listend;}
		}
		/// <summary>
		/// ���ӿ�ʼ���
		/// </summary>
		public string LinkStart
		{
			set{ _linkstart=value;}
			get{return _linkstart;}
		}
		/// <summary>
		/// ���ӽ������
		/// </summary>
		public string LinkEnd
		{
			set{ _linkend=value;}
			get{return _linkend;}
		}
		/// <summary>
		/// ���⿪ʼ���
		/// </summary>
		public string TitleStart
		{
			set{ _titlestart=value;}
			get{return _titlestart;}
		}
		/// <summary>
		/// ����������
		/// </summary>
		public string TitleEnd
		{
			set{ _titleend=value;}
			get{return _titleend;}
		}
		/// <summary>
		/// �ؼ��ֿ�ʼ
		/// </summary>
		public string KeyWordStart
		{
			set{ _keywordstart=value;}
			get{return _keywordstart;}
		}
		/// <summary>
		/// �ؼ��ֽ���
		/// </summary>
		public string KeyWordEnd
		{
			set{ _keywordend=value;}
			get{return _keywordend;}
		}
		/// <summary>
		/// ��ȡʱ������
		/// </summary>
		public string DateRegex
		{
			set{ _dateregex=value;}
			get{return _dateregex;}
		}
		/// <summary>
		/// �б����һҳ����
		/// </summary>
		public string NextListRegex
		{
			set{ _nextlistregex=value;}
			get{return _nextlistregex;}
		}
		/// <summary>
		/// ���Ŀ�ʼ���
		/// </summary>
		public string ContentStart
		{
			set{ _contentstart=value;}
			get{return _contentstart;}
		}
		/// <summary>
		/// ���Ľ������
		/// </summary>
		public string ContentEnd
		{
			set{ _contentend=value;}
			get{return _contentend;}
		}
		/// <summary>
		/// �������Ŀ�ʼ
		/// </summary>
		public string RemoveBodyStart
		{
			set{ _removebodystart=value;}
			get{return _removebodystart;}
		}
		/// <summary>
		/// �������Ľ���
		/// </summary>
		public string RemoveBodyEnd
		{
			set{ _removebodyend=value;}
			get{return _removebodyend;}
		}
		/// <summary>
		/// ��ϸҳ����һҳ����
		/// </summary>
		public string NextPageRegex
		{
			set{ _nextpageregex=value;}
			get{return _nextpageregex;}
		}
		/// <summary>
		/// ���˱�ǩ
		/// </summary>
		public string Script_Html
		{
			set{ _script_html=value;}
			get{return _script_html;}
		}
		/// <summary>
		/// ָ���ɼ�����������
		/// </summary>
		public int CollecNum
		{
			set{ _collecnum=value;}
			get{return _collecnum;}
		}
		/// <summary>
		/// �Ƿ񱣴�ͼƬ
		/// </summary>
		public int IsSaveImg
		{
			set{ _issaveimg=value;}
			get{return _issaveimg;}
		}
		/// <summary>
		/// �Ƿ���ɼ�
		/// </summary>
		public int IsDesc
		{
			set{ _isdesc=value;}
			get{return _isdesc;}
		}
		/// <summary>
		/// ��Ŀ״̬��1���á�0�����ã�
		/// </summary>
		public int State
		{
			set{ _state=value;}
			get{return _state;}
		}
		/// <summary>
        ///�����滻(�����$�ֿ�)
		/// </summary>
		public string RemoveTitle
		{
			set{ _removetitle=value;}
			get{return _removetitle;}
		}
		/// <summary>
        /// �����滻��(�����$�ֿ�)
		/// </summary>
		public string RemoveContent
		{
			set{ _removecontent=value;}
			get{return _removecontent;}
		}
		#endregion Model

	}
}

