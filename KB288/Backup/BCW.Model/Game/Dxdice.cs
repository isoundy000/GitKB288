using System;
namespace BCW.Model.Game
{
	/// <summary>
	/// ʵ����Dxdice ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
	/// </summary>
	[Serializable]
	public class Dxdice
	{
		public Dxdice()
		{}
		#region Model
		private int _id;
		private int _types;
		private string _dxdicea;
		private string _dxdiceb;
		private DateTime _stoptime;
		private int _usid;
		private string _usname;
		private int _reid;
		private string _rename;
		private long _price;
		private int _bztype;
		private DateTime _addtime;
		private DateTime _retime;
		private int _iswin;
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
		/// ���ͣ�0��ͨ/1˽�˶�ս��
		/// </summary>
		public int Types
		{
			set{ _types=value;}
			get{return _types;}
		}
		/// <summary>
		/// ������һ��
		/// </summary>
		public string DxdiceA
		{
			set{ _dxdicea=value;}
			get{return _dxdicea;}
		}
		/// <summary>
		/// �����ڶ���
		/// </summary>
		public string DxdiceB
		{
			set{ _dxdiceb=value;}
			get{return _dxdiceb;}
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
		/// �û�ID
		/// </summary>
		public int UsID
		{
			set{ _usid=value;}
			get{return _usid;}
		}
		/// <summary>
		/// �û��ǳ�
		/// </summary>
		public string UsName
		{
			set{ _usname=value;}
			get{return _usname;}
		}
		/// <summary>
		/// Ӧս�û�ID
		/// </summary>
		public int ReID
		{
			set{ _reid=value;}
			get{return _reid;}
		}
		/// <summary>
		/// Ӧս�û��ǳ�
		/// </summary>
		public string ReName
		{
			set{ _rename=value;}
			get{return _rename;}
		}
		/// <summary>
		/// ��ս��
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
		/// Ӧսʱ��
		/// </summary>
		public DateTime ReTime
		{
			set{ _retime=value;}
			get{return _retime;}
		}
		/// <summary>
		/// ׯ���Ƿ�ʤ(1ʤ)
		/// </summary>
		public int IsWin
		{
			set{ _iswin=value;}
			get{return _iswin;}
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

