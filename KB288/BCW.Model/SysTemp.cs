using System;
namespace BCW.Model
{
	/// <summary>
	/// ʵ����SysTemp ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
	/// </summary>
	[Serializable]
	public class SysTemp
	{
		public SysTemp()
		{}
		#region Model
		private int _id;
		private DateTime _guessoddstime;
		/// <summary>
		/// ����ID
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 8��ˮλ����ʱ��
		/// </summary>
		public DateTime GuessOddsTime
		{
			set{ _guessoddstime=value;}
			get{return _guessoddstime;}
		}
		#endregion Model

	}
}

