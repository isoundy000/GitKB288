using System;
namespace BCW.Baccarat.Model
{
	/// <summary>
	/// ʵ����BaccaratUser ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
	/// </summary>
	[Serializable]
	public class BaccaratUser
	{
		public BaccaratUser()
		{}
		#region Model
		private int _id;
		private int _usid;
		private string _usname;
		private int _setid;
        private int _roomtimes;
        /// <summary>
        /// 
        /// </summary>
        public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int UsID
		{
			set{ _usid=value;}
			get{return _usid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string UsName
		{
			set{ _usname=value;}
			get{return _usname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int SetID
		{
			set{ _setid=value;}
			get{return _setid;}
		}
        /// <summary>
		/// 
		/// </summary>
		public int RoomTimes
        {
            set { _roomtimes = value; }
            get { return _roomtimes; }
        }
        #endregion Model

    }
}

