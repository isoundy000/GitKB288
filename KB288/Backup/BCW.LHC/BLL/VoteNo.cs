using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using LHC.Model;
namespace LHC.BLL
{
	/// <summary>
	/// ҵ���߼���VoteNo ��ժҪ˵����
	/// </summary>
	public class VoteNo
	{
		private readonly LHC.DAL.VoteNo dal=new LHC.DAL.VoteNo();
		public VoteNo()
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
		public int  Add(LHC.Model.VoteNo model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Update(LHC.Model.VoteNo model)
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
        public void UpdateOpen(LHC.Model.VoteNo model)
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
		public LHC.Model.VoteNo GetVoteNo(int ID)
		{
			
			return dal.GetVoteNo(ID);
		}
                
        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public LHC.Model.VoteNo GetVoteNoNew(int State)
        {
            return dal.GetVoteNoNew(State);
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
		/// <returns>IList VoteNo</returns>
		public IList<LHC.Model.VoteNo> GetVoteNos(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetVoteNos(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

		#endregion  ��Ա����
	}
}

