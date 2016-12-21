using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
	/// <summary>
	/// ҵ���߼���klsfpay ��ժҪ˵����
	/// </summary>
	public class klsfpay
	{
		private readonly BCW.DAL.klsfpay dal=new BCW.DAL.klsfpay();
		public klsfpay()
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
        /// �����ֶ�ͳ���ж��������ݷ�������
        /// </summary>
        /// <param name="strWhere">ͳ������</param>
        /// <returns>ͳ�ƽ��</returns>
        public int GetRecordCount(string strWhere)
        {
            return dal.GetRecordCount(strWhere);
        }

		/// <summary>
		/// �Ƿ���ڸü�¼
		/// </summary>
		public bool Exists(int ID)
		{
			return dal.Exists(ID);
		}

        /// <summary>
        /// �Ƿ���ڶҽ���¼
        /// </summary>
        public bool ExistsState(int ID, int UsID)
        {
            return dal.ExistsState(ID,UsID);
        }

        /// <summary>
        /// ������������ұ���ֵ
        /// </summary>
        public long GetSumPrices(string strWhere)
        {
            return dal.GetSumPrices(strWhere);
        }

        /// <summary>
        /// ������������Ӯȡ��ֵ
        /// </summary>
        public long GetSumWinCent(string strWhere)
        {
            return dal.GetSumWinCent(strWhere);
        }

		/// <summary>
		/// ����һ������
		/// </summary>
		public int  Add(BCW.Model.klsfpay model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Update(BCW.Model.klsfpay model)
		{
			dal.Update(model);
		}

        /// <summary>
        /// �����û��ҽ���ʶ
        /// </summary>
        public void UpdateState(int ID, int State)
        {
            dal.UpdateState(ID,State);
        }

        /// <summary>
        /// ���¿�������
        /// </summary>
        public void UpdateResult(string klsfId, string Result)
        {
            dal.UpdateResult(klsfId, Result);
        }

        /// <summary>
        /// ������Ϸ�����ñ�
        /// </summary>
        public void UpdateWinCent(int ID, long WinCent, string WinNotes)
        {
            dal.UpdateWinCent(ID,WinCent,WinNotes);
        }
		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public void Delete(int ID)
		{
			
			dal.Delete(ID);
		}

        /// <summary>
        /// ĳ��ĳID��Ͷ�˶��ٱ�
        /// </summary>
        public long GetSumPrices(int UsID, int klsfId)
        {
            return dal.GetSumPrices(UsID,klsfId);
        }
        ///<summary>
        ///ĳ��ĳ��Ͷע��ʽͶ�˶��ٱ�
        /// </summary>
        public long GetSumPricebyTypes(int Types, int klsfId)
        {
            return dal.GetSumPricebyTypes(Types, klsfId);
        }

        /// <summary>
        /// ����ID�õ�klsfId
        /// </summary>
        public int GetklsfId(int ID)
        {
            return dal.GetklsfId(ID);
        }
        /// <summary>
        /// �õ�һ��WinCent
        /// </summary>
        public long GetWinCent(int ID)
        {
            return dal.GetWinCent(ID);
        }
		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public BCW.Model.klsfpay Getklsfpay(int ID)
		{
			return dal.Getklsfpay(ID);
		}
        /// <summary>
        /// ���ڻ�����
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="WinUserID"></param>
        /// <returns></returns>
        public bool ExistsReBot(int ID, int UsID)
        {
            return dal.ExistsReBot(ID, UsID);
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
		/// <returns>IList klsfpay</returns>
		public IList<BCW.Model.klsfpay> Getklsfpays(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.Getklsfpays(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

         /// <summary>
        /// ȡ�����м�¼
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">��ҳ��С</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <returns>IList SSCpay</returns>
        public IList<BCW.Model.klsfpay> GetklsfpaysTop(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetklsfpaysTop(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }

		#endregion  ��Ա����
	}
}

