using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
	/// <summary>
	/// ҵ���߼���Visitor ��ժҪ˵����
	/// </summary>
	public class Visitor
	{
		private readonly BCW.DAL.Visitor dal=new BCW.DAL.Visitor();
		public Visitor()
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
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool ExistsVisitId(int UsID, int VisitId)
        {
            return dal.ExistsVisitId(UsID, VisitId);
        }
                
        /// <summary>
        /// �����������
        /// </summary>
        public int GetTodayCount(int UsID)
        {
            return dal.GetTodayCount(UsID);
        }

		/// <summary>
		/// ����һ������
		/// </summary>
		public int  Add(BCW.Model.Visitor model)
		{
			//return dal.Add(model);
            int ID = dal.Add(model);
            try
            {
                int usid = model.VisitId;
                string username = model.VisitName;
                string Notes = "̽������";
                int id = new BCW.BLL.Action().GetMaxId();
                int isHit = new BCW.winners.winners().CheckActionForAll(0, 0, usid, username, Notes, id);
                return ID;
            }
            catch { return ID; }
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Update(BCW.Model.Visitor model)
		{
			dal.Update(model);
		}
                
        /// <summary>
        /// ����һ������
        /// </summary>
        public void Update(int UsID, int VisitId)
        {
            dal.Update(UsID, VisitId);
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
		public BCW.Model.Visitor GetVisitor(int ID)
		{
			
			return dal.GetVisitor(ID);
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
		/// <returns>IList Visitor</returns>
		public IList<BCW.Model.Visitor> GetVisitors(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetVisitors(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

		#endregion  ��Ա����
	}
}

