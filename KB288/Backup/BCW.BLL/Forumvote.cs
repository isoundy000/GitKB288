using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
	/// <summary>
	/// ҵ���߼���Forumvote ��ժҪ˵����
	/// </summary>
	public class Forumvote
	{
		private readonly BCW.DAL.Forumvote dal=new BCW.DAL.Forumvote();
		public Forumvote()
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
        /// ĳ��Աĳ��̳�ϴ��Ƿ��н�
        /// </summary>
        public bool Exists(int ForumID, int BID, int UsID)
        {
            return dal.Exists(ForumID, BID, UsID);
        }

        /// <summary>
        /// ����ĳ��̳����ĳ�û��н�����
        /// </summary>
        public int GetMonthCount(int ForumID, int BID, int UsID)
        {
            return dal.GetMonthCount(ForumID, BID, UsID);
        }

		/// <summary>
		/// ����һ������
		/// </summary>
		public int  Add(BCW.Model.Forumvote model)
		{
			return dal.Add(model);
		}
                
        /// <summary>
        /// ����һ������
        /// </summary>
        public void UpdateNotes(int ID, string Notes)
        {
            dal.UpdateNotes(ID, Notes);
        }
        
        /// <summary>
        /// ���¿������Ƿ��н�
        /// </summary>
        public void UpdateIsWin(int ID, int IsWin)
        {
            dal.UpdateIsWin(ID, IsWin);
        }
        
        /// <summary>
        /// �Ƿ����δ����
        /// </summary>
        public bool ExistsKz()
        {
            return dal.ExistsKz();
        }
                           
        /// <summary>
        /// ĳ���Ƿ����δ����
        /// </summary>
        public bool ExistsKz(int qiNum)
        {
            return dal.ExistsKz(qiNum);
        }

        /// <summary>
        /// ĳ���Ƿ��ѿ���
        /// </summary>
        public bool ExistsKz(int ForumID, int qiNum)
        {
            return dal.ExistsKz(ForumID, qiNum);
        }

        /// <summary>
        /// ����ĳ��̳����ȫ���ѿ���
        /// </summary>
        public void UpdateState(int qiNum, int ForumID, int state, string sNum)
        {
            dal.UpdateState(qiNum, ForumID, state, sNum);
        }

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Update(BCW.Model.Forumvote model)
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
        /// �õ�ĳ�ڿ������
        /// </summary>
        public string GetsNum(int ForumID, int qiNum)
        {
            return dal.GetsNum(ForumID, qiNum);
        }

		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public BCW.Model.Forumvote GetForumvote(int ID)
		{
			
			return dal.GetForumvote(ID);
		}

		/// <summary>
		/// �����ֶ�ȡ�����б�
		/// </summary>
		public DataSet GetList(string strField, string strWhere)
		{
			return dal.GetList(strField, strWhere);
		}

        /// <summary>
        /// ȡ��N����¼
        /// </summary>
        /// <param name="UsID">��ԱID</param>
        /// <param name="BID">����ID</param>
        /// <param name="SizeNum">ȡN��</param>
        /// <returns></returns>
        public IList<BCW.Model.Forumvote> GetForumvotes(int UsID, int BID, int SizeNum)
        {
            return dal.GetForumvotes(UsID, BID, SizeNum);
        }

		/// <summary>
		/// ȡ��ÿҳ��¼
		/// </summary>
		/// <param name="p_pageIndex">��ǰҳ</param>
		/// <param name="p_pageSize">��ҳ��С</param>
		/// <param name="p_recordCount">�����ܼ�¼��</param>
		/// <param name="strWhere">��ѯ����</param>
		/// <returns>IList Forumvote</returns>
		public IList<BCW.Model.Forumvote> GetForumvotes(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetForumvotes(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

		#endregion  ��Ա����
	}
}

