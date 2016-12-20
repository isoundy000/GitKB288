using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
	/// <summary>
	/// ҵ���߼���dawnlifeDays ��ժҪ˵����
	/// </summary>
	public class dawnlifeDays
	{
		private readonly BCW.DAL.dawnlifeDays dal=new BCW.DAL.dawnlifeDays();
		public dawnlifeDays()
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
		public int  Add(BCW.Model.dawnlifeDays model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Update(BCW.Model.dawnlifeDays model)
		{
			dal.Update(model);
		}

        public void Updategoods(int ID,string goods)
        {
            dal.Updategoods(ID,goods);
        }
        public void Updateprice(int ID, string price)
        {
            dal.Updateprice(ID, price);
        }

		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public void Delete(int ID)
		{
			
			dal.Delete(ID);
		}

		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public BCW.Model.dawnlifeDays GetdawnlifeDays(int ID)
		{
			
			return dal.GetdawnlifeDays(ID);
		}


        /// ���ݲ�ѯӰ�������
        /// </summary>
        public int GetRowByUsID(int UsID, int day,long coin)
        {
            return dal.GetRowByUsID(UsID, day,coin );
        }

		/// <summary>
		/// �����ֶ�ȡ�����б�
		/// </summary>
		public DataSet GetList(string strField, string strWhere)
		{
			return dal.GetList(strField, strWhere);
		}

        /// ���ݲ�ѯӰ�������
        /// </summary>
        public int GetDayByUsID(int UsID)
        {
            return dal.GetDayByUsID(UsID);
        }

		/// <summary>
		/// ȡ��ÿҳ��¼
		/// </summary>
		/// <param name="p_pageIndex">��ǰҳ</param>
		/// <param name="p_pageSize">��ҳ��С</param>
		/// <param name="p_recordCount">�����ܼ�¼��</param>
		/// <param name="strWhere">��ѯ����</param>
		/// <returns>IList dawnlifeDays</returns>
		public IList<BCW.Model.dawnlifeDays> GetdawnlifeDayss(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetdawnlifeDayss(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

		#endregion  ��Ա����
	}
}

