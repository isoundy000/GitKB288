using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
	/// <summary>
	/// ҵ���߼���SysTemp ��ժҪ˵����
	/// </summary>
	public class SysTemp
	{
		private readonly BCW.DAL.SysTemp dal=new BCW.DAL.SysTemp();
		public SysTemp()
		{}
		#region  ��Ա����

		/// <summary>
		/// �õ����ID
		/// </summary>
		public int GetMaxId()
		{
			return dal.GetMaxId();
		}

		/// <summary>
		/// �Ƿ���ڸü�¼
		/// </summary>
		public bool Exists(int ID)
		{
			return dal.Exists(ID);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public int  Add(BCW.Model.SysTemp model)
		{
			return dal.Add(model);
		}

        /// <summary>
        /// ����GuessOddsTime
        /// </summary>
        public void UpdateGuessOddsTime(int ID, DateTime GuessOddsTime)
        {
            dal.UpdateGuessOddsTime(ID, GuessOddsTime);
		}

		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public void Delete(int ID)
		{
			
			dal.Delete(ID);
		}

		/// <summary>
        /// �õ�GuessOddsTime
		/// </summary>
        public DateTime GetGuessOddsTime(int ID)
        {
            return dal.GetGuessOddsTime(ID);
        }

		/// <summary>
		/// �����ֶ�ȡ�����б�
		/// </summary>
		public DataSet GetList(string strField, string strWhere)
		{
			return dal.GetList(strField, strWhere);
		}

		/// <summary>
		/// ȡ��ÿҳ��¼
		/// </summary>
		/// <param name="p_pageIndex">��ǰҳ</param>
		/// <param name="p_pageSize">��ҳ��С</param>
		/// <param name="p_recordCount">�����ܼ�¼��</param>
		/// <param name="strWhere">��ѯ����</param>
		/// <returns>IList SysTemp</returns>
		public IList<BCW.Model.SysTemp> GetSysTemps(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetSysTemps(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

		#endregion  ��Ա����
	}
}

