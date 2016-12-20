using System;
namespace BCW.Model
{
	/// <summary>
	/// ʵ����Order ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
	/// </summary>
	[Serializable]
	public class Order
	{
		public Order()
		{}
		#region Model
		private int _id;
		private int _topicid;
		private int _usid;
		private string _usname;
		private string _title;
		private int _selltypes;
		private DateTime _addtime;
		private DateTime _extime;
		/// <summary>
		/// ����ID
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// ҳ��ID
		/// </summary>
		public int TopicId
		{
			set{ _topicid=value;}
			get{return _topicid;}
		}
		/// <summary>
		/// �û�ID
		/// </summary>
		public int UsId
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
		/// ҳ�����
		/// </summary>
		public string Title
		{
			set{ _title=value;}
			get{return _title;}
		}
		/// <summary>
		/// �������ͣ�0���μƷ�,�۷�һ�Σ��������/1���ܼƷ�/2���¼Ʒѣ�
		/// </summary>
		public int SellTypes
		{
			set{ _selltypes=value;}
			get{return _selltypes;}
		}
		/// <summary>
		/// ��ʼʱ��
		/// </summary>
		public DateTime AddTime
		{
			set{ _addtime=value;}
			get{return _addtime;}
		}
		/// <summary>
		/// ����ʱ��
		/// </summary>
		public DateTime ExTime
		{
			set{ _extime=value;}
			get{return _extime;}
		}
		#endregion Model

	}
}

