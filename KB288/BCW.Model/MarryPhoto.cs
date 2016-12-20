using System;
namespace BCW.Model
{
	/// <summary>
	/// 实体类MarryPhoto 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class MarryPhoto
	{
		public MarryPhoto()
		{}
		#region Model
		private int _id;
		private int _marryid;
		private int _usid;
		private string _usname;
		private string _prevfile;
		private string _actfile;
		private string _notes;
		private DateTime _addtime;
		/// <summary>
		/// 自增ID
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 结婚ID
		/// </summary>
		public int MarryId
		{
			set{ _marryid=value;}
			get{return _marryid;}
		}
		/// <summary>
		/// 上传用户ID
		/// </summary>
		public int UsID
		{
			set{ _usid=value;}
			get{return _usid;}
		}
		/// <summary>
		/// 上传用户昵称
		/// </summary>
		public string UsName
		{
			set{ _usname=value;}
			get{return _usname;}
		}
		/// <summary>
		/// 小图地址
		/// </summary>
		public string PrevFile
		{
			set{ _prevfile=value;}
			get{return _prevfile;}
		}
		/// <summary>
		/// 大图地址
		/// </summary>
		public string ActFile
		{
			set{ _actfile=value;}
			get{return _actfile;}
		}
		/// <summary>
		/// 图片描述
		/// </summary>
		public string Notes
		{
			set{ _notes=value;}
			get{return _notes;}
		}
		/// <summary>
		/// 上传时间
		/// </summary>
		public DateTime AddTime
		{
			set{ _addtime=value;}
			get{return _addtime;}
		}
		#endregion Model

	}
}

