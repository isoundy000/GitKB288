using System;
namespace BCW.Model.Game
{
	/// <summary>
	/// ʵ����Mora ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
	/// </summary>
	[Serializable]
	public class Mora
	{
		public Mora()
		{}
		#region Model
		private int _id;
		private int _types;
		private string _title;
		private int _truetype;
		private int _choosetype;
		private DateTime _stoptime;
		private int _usid;
		private string _usname;
		private int _reid;
		private string _rename;
		private long _price;
		private int _bztype;
		private DateTime _addtime;
		private DateTime _retime;
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
		/// ����(0������ս/1˽�˶�ս)
		/// </summary>
		public int Types
		{
			set{ _types=value;}
			get{return _types;}
		}
		/// <summary>
		/// ��ţ����
		/// </summary>
		public string Title
		{
			set{ _title=value;}
			get{return _title;}
		}
		/// <summary>
		/// ��ȷ��(1/2/3)
		/// </summary>
		public int TrueType
		{
			set{ _truetype=value;}
			get{return _truetype;}
		}
		/// <summary>
		/// �ύ��(1/2/3)
		/// </summary>
		public int ChooseType
		{
			set{ _choosetype=value;}
			get{return _choosetype;}
		}
		/// <summary>
		/// ����ʱ��
		/// </summary>
		public DateTime StopTime
		{
			set{ _stoptime=value;}
			get{return _stoptime;}
		}
		/// <summary>
		/// ����ID
		/// </summary>
		public int UsID
		{
			set{ _usid=value;}
			get{return _usid;}
		}
		/// <summary>
		/// �����û��ǳ�
		/// </summary>
		public string UsName
		{
			set{ _usname=value;}
			get{return _usname;}
		}
		/// <summary>
		/// ��սID
		/// </summary>
		public int ReID
		{
			set{ _reid=value;}
			get{return _reid;}
		}
		/// <summary>
		/// ��ս�û��ǳ�
		/// </summary>
		public string ReName
		{
			set{ _rename=value;}
			get{return _rename;}
		}
		/// <summary>
		/// ��ֵ
		/// </summary>
		public long Price
		{
			set{ _price=value;}
			get{return _price;}
		}
		/// <summary>
		/// ����
		/// </summary>
		public int BzType
		{
			set{ _bztype=value;}
			get{return _bztype;}
		}
		/// <summary>
		/// ����ʱ��
		/// </summary>
		public DateTime AddTime
		{
			set{ _addtime=value;}
			get{return _addtime;}
		}
		/// <summary>
		/// ��սʱ��
		/// </summary>
		public DateTime ReTime
		{
			set{ _retime=value;}
			get{return _retime;}
		}
		/// <summary>
		/// ״̬
		/// </summary>
		public int State
		{
			set{ _state=value;}
			get{return _state;}
		}
		#endregion Model

	}
}

