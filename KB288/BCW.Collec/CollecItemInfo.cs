using System;
namespace BCW.Model.Collec
{
	/// <summary>
	/// 实体类CollecItem 。(属性说明自动提取数据库字段的描述信息)
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
		/// 自增ID
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 项目名称
		/// </summary>
		public string ItemName
		{
			set{ _itemname=value;}
			get{return _itemname;}
		}
		/// <summary>
		/// 内容类型
		/// </summary>
		public int Types
		{
			set{ _types=value;}
			get{return _types;}
		}
		/// <summary>
		/// 类型子类别
		/// </summary>
		public int NodeId
		{
			set{ _nodeid=value;}
			get{return _nodeid;}
		}
		/// <summary>
		/// 页面编码2=gb2312,1=utf-8,0=unicode
		/// </summary>
		public int WebEncode
		{
			set{ _webencode=value;}
			get{return _webencode;}
		}
		/// <summary>
		/// 网站名称
		/// </summary>
		public string WebName
		{
			set{ _webname=value;}
			get{return _webname;}
		}
		/// <summary>
		/// 网站地址
		/// </summary>
		public string WebUrl
		{
			set{ _weburl=value;}
			get{return _weburl;}
		}
		/// <summary>
		/// 项目备注
		/// </summary>
		public string ItemRemark
		{
			set{ _itemremark=value;}
			get{return _itemremark;}
		}
		/// <summary>
		/// 列表索引页面
		/// </summary>
		public string ListUrl
		{
			set{ _listurl=value;}
			get{return _listurl;}
		}
		/// <summary>
		/// 列表开始标记
		/// </summary>
		public string ListStart
		{
			set{ _liststart=value;}
			get{return _liststart;}
		}
		/// <summary>
		/// 列表结束标记
		/// </summary>
		public string ListEnd
		{
			set{ _listend=value;}
			get{return _listend;}
		}
		/// <summary>
		/// 链接开始标记
		/// </summary>
		public string LinkStart
		{
			set{ _linkstart=value;}
			get{return _linkstart;}
		}
		/// <summary>
		/// 链接结束标记
		/// </summary>
		public string LinkEnd
		{
			set{ _linkend=value;}
			get{return _linkend;}
		}
		/// <summary>
		/// 标题开始标记
		/// </summary>
		public string TitleStart
		{
			set{ _titlestart=value;}
			get{return _titlestart;}
		}
		/// <summary>
		/// 标题结束标记
		/// </summary>
		public string TitleEnd
		{
			set{ _titleend=value;}
			get{return _titleend;}
		}
		/// <summary>
		/// 关键字开始
		/// </summary>
		public string KeyWordStart
		{
			set{ _keywordstart=value;}
			get{return _keywordstart;}
		}
		/// <summary>
		/// 关键字结束
		/// </summary>
		public string KeyWordEnd
		{
			set{ _keywordend=value;}
			get{return _keywordend;}
		}
		/// <summary>
		/// 获取时间正则
		/// </summary>
		public string DateRegex
		{
			set{ _dateregex=value;}
			get{return _dateregex;}
		}
		/// <summary>
		/// 列表的下一页正则
		/// </summary>
		public string NextListRegex
		{
			set{ _nextlistregex=value;}
			get{return _nextlistregex;}
		}
		/// <summary>
		/// 正文开始标记
		/// </summary>
		public string ContentStart
		{
			set{ _contentstart=value;}
			get{return _contentstart;}
		}
		/// <summary>
		/// 正文结束标记
		/// </summary>
		public string ContentEnd
		{
			set{ _contentend=value;}
			get{return _contentend;}
		}
		/// <summary>
		/// 过滤正文开始
		/// </summary>
		public string RemoveBodyStart
		{
			set{ _removebodystart=value;}
			get{return _removebodystart;}
		}
		/// <summary>
		/// 过滤正文结束
		/// </summary>
		public string RemoveBodyEnd
		{
			set{ _removebodyend=value;}
			get{return _removebodyend;}
		}
		/// <summary>
		/// 详细页的下一页正则
		/// </summary>
		public string NextPageRegex
		{
			set{ _nextpageregex=value;}
			get{return _nextpageregex;}
		}
		/// <summary>
		/// 过滤标签
		/// </summary>
		public string Script_Html
		{
			set{ _script_html=value;}
			get{return _script_html;}
		}
		/// <summary>
		/// 指定采集的新闻数量
		/// </summary>
		public int CollecNum
		{
			set{ _collecnum=value;}
			get{return _collecnum;}
		}
		/// <summary>
		/// 是否保存图片
		/// </summary>
		public int IsSaveImg
		{
			set{ _issaveimg=value;}
			get{return _issaveimg;}
		}
		/// <summary>
		/// 是否倒序采集
		/// </summary>
		public int IsDesc
		{
			set{ _isdesc=value;}
			get{return _isdesc;}
		}
		/// <summary>
		/// 项目状态（1可用、0不可用）
		/// </summary>
		public int State
		{
			set{ _state=value;}
			get{return _state;}
		}
		/// <summary>
        ///正文替换(多个用$分开)
		/// </summary>
		public string RemoveTitle
		{
			set{ _removetitle=value;}
			get{return _removetitle;}
		}
		/// <summary>
        /// 正文替换成(多个用$分开)
		/// </summary>
		public string RemoveContent
		{
			set{ _removecontent=value;}
			get{return _removecontent;}
		}
		#endregion Model

	}
}

