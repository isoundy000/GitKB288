using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using LHC.Model;
namespace LHC.BLL
{
	/// <summary>
	/// ҵ���߼���VoteNo49 ��ժҪ˵����
	/// </summary>
	public class VoteNo49
	{
		private readonly LHC.DAL.VoteNo49 dal=new LHC.DAL.VoteNo49();
		public VoteNo49()
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
		public int  Add(LHC.Model.VoteNo49 model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Update(LHC.Model.VoteNo49 model)
		{
			dal.Update(model);
		}

        /// <summary>
        /// ����һ������
        /// </summary>
        public void Update(int qiNo, long payCent, int payCount)
        {
            dal.Update(qiNo, payCent, payCount);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public void Update2(int qiNo, long payCent2, int payCount2)
        {
            dal.Update2(qiNo, payCent2, payCount2);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public void UpdateOpen(LHC.Model.VoteNo49 model)
        {
            dal.UpdateOpen(model);
        }
   
        /// <summary>
        /// ���¸���Ϊ����
        /// </summary>
        public void UpdateState(int qiNo)
        {
            dal.UpdateState(qiNo);
        }

		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public void Delete(int ID)
		{
			
			dal.Delete(ID);
		}
               
        /// <summary>
        /// �õ�һ��qiNo
        /// </summary>
        public int GetqiNo(int ID)
        {
            return dal.GetqiNo(ID);
        }
                
        /// <summary>
        /// �õ�һ��payCount
        /// </summary>
        public int GetpayCount(int ID)
        {
            return dal.GetpayCount(ID);
        }

		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public LHC.Model.VoteNo49 GetVoteNo49(int ID)
		{
			
			return dal.GetVoteNo49(ID);
		}
                
        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public LHC.Model.VoteNo49 GetVoteNo49New(int State)
        {
            return dal.GetVoteNo49New(State);
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
		/// <returns>IList VoteNo49</returns>
		public IList<LHC.Model.VoteNo49> GetVoteNo49s(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetVoteNo49s(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

		#endregion  ��Ա����
	}
}

