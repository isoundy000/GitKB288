using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.bydr.Model;
namespace BCW.bydr.BLL
{
	/// <summary>
	/// ҵ���߼���CmgTicket ��ժҪ˵����
	/// </summary>
	public class CmgTicket
	{
		private readonly BCW.bydr.DAL.CmgTicket dal=new BCW.bydr.DAL.CmgTicket();
		public CmgTicket()
		{}
		#region  ��Ա����



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
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool ExistsBID(int Bid)
        {
            return dal.ExistsBID(Bid);
        }

		/// <summary>
		/// ����һ������
		/// </summary>
		public int  Add(BCW.bydr.Model.CmgTicket model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Update(BCW.bydr.Model.CmgTicket model)
		{
			dal.Update(model);
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
		public BCW.bydr.Model.CmgTicket GetCmgTicket(int ID)
		{
			
			return dal.GetCmgTicket(ID);
		}
         /// <summary>
        /// �õ�ȫ���ռ���
        /// </summary>
        public long GettoplistColletGoldsum()
        {
            return dal.GettoplistColletGoldsum();
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
		/// <returns>IList CmgTicket</returns>
		public IList<BCW.bydr.Model.CmgTicket> GetCmgTickets(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetCmgTickets(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

		#endregion  ��Ա����
	}
}

