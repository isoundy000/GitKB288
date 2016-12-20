using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model.Game;
namespace BCW.BLL.Game
{
	/// <summary>
	/// ҵ���߼���HcPay ��ժҪ˵����
	/// </summary>
	public class HcPay
	{
		private readonly BCW.DAL.Game.HcPay dal=new BCW.DAL.Game.HcPay();
		public HcPay()
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
		public bool Exists(int id)
		{
			return dal.Exists(id);
		}
             
        /// <summary>
        /// �Ƿ���ڶҽ���¼
        /// </summary>
        public bool ExistsState(int id, int UsID)
        {
            return dal.ExistsState(id, UsID);
        }
                
        /// <summary>
        /// ÿIDÿ����ע����
        /// </summary>
        public long GetPayCent(int UsID, int CID)
        {
            return dal.GetPayCent(UsID, CID);
        }

		/// <summary>
		/// ����һ������
		/// </summary>
		public int  Add(BCW.Model.Game.HcPay model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Update(BCW.Model.Game.HcPay model)
		{
			dal.Update(model);
		}
        
        /// <summary>
        /// �����û��ҽ���ʶ
        /// </summary>
        public void UpdateState(int id)
        {
            dal.UpdateState(id);
        }

		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public void Delete(int id)
		{
			
			dal.Delete(id);
		}
                
        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public void Delete(string strWhere)
        {
            dal.Delete(strWhere);
        }

		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public BCW.Model.Game.HcPay GetHcPay(int id)
		{
			
			return dal.GetHcPay(id);
		}
                
        /// <summary>
        /// �õ�һ��WinCent
        /// </summary>
        public long GetWinCent(int ID)
        {
            return dal.GetWinCent(ID);
        }
        /// <summary>
        /// �õ�һ��WinCent1
        /// </summary>
        public long GetWinCent1(string time1, string time2)
        {
            return dal.GetWinCent1(time1,time2);
        }
        /// <summary>
        /// �õ�һ��GetPayCent1
        /// </summary>
        public long GetPayCent1(string time1, string time2)
        {
            return dal.GetPayCent1(time1, time2);
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
		/// <returns>IList HcPay</returns>
		public IList<BCW.Model.Game.HcPay> GetHcPays(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetHcPays(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}
           
        
        /// <summary>
        /// ȡ�����м�¼
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">��ҳ��С</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <returns>IList HcPay</returns>
        public IList<BCW.Model.Game.HcPay> GetHcPaysTop(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetHcPaysTop(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }

		#endregion  ��Ա����
	}
}

