using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
	/// <summary>
	/// ҵ���߼���Diary ��ժҪ˵����
	/// </summary>
	public class Diary
	{
		private readonly BCW.DAL.Diary dal=new BCW.DAL.Diary();
		public Diary()
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
        public bool Exists(int ID, int UsID)
        {
            return dal.Exists(ID, UsID);
        }

        /// <summary>
        /// ����ĳ�û��ռ�����
        /// </summary>
        public int GetCount(int UsID)
        {
            return dal.GetCount(UsID);
        }
                     
        /// <summary>
        /// ����ĳ�û������ռ�����
        /// </summary>
        public int GetTodayCount(int UsID)
        {
            return dal.GetTodayCount(UsID);
        }
  
        /// <summary>
        /// ����ĳ�û�ĳ�����ռ�����
        /// </summary>
        public int GetCount(int UsID, int NodeId)
        {
            return dal.GetCount(UsID, NodeId);
        }

		/// <summary>
		/// ����һ������
		/// </summary>
		public int  Add(BCW.Model.Diary model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Update(BCW.Model.Diary model)
		{
			dal.Update(model);
		}

        /// <summary>
        /// ����һ������
        /// </summary>
        public void Update2(BCW.Model.Diary model)
        {
            dal.Update2(model);
        }

        /// <summary>
        /// �����ռ�����
        /// </summary>
        public void UpdateContent(int ID, string Content)
        {
            dal.UpdateContent(ID, Content);
        }

        /// <summary>
        /// ���»ظ���
        /// </summary>
        public void UpdateReplyNum(int ID, int ReplyNum)
        {
            dal.UpdateReplyNum(ID, ReplyNum);
        }

        /// <summary>
        /// �����Ķ���
        /// </summary>
        public void UpdateReadNum(int ID, int ReadNum)
        {
            dal.UpdateReadNum(ID, ReadNum);
        }

        /// <summary>
        /// �ö�/ȥ��
        /// </summary>
        public void UpdateIsTop(int ID, int IsTop)
        {
            dal.UpdateIsTop(ID, IsTop);
        }

        /// <summary>
        /// ����ĳ�����ռǳ�ΪĬ�Ϸ���
        /// </summary>
        public void UpdateNodeId(int UsID, int NodeId)
        {
            dal.UpdateNodeId(UsID, NodeId);
        }

		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public void Delete(int ID)
		{
			
			dal.Delete(ID);
		}
                
        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public void Delete(int UsID, int NodeId)
        {
            dal.Delete(UsID, NodeId);
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public void DeleteStr(string strWhere)
        {

            dal.DeleteStr(strWhere);
        }

        /// <summary>
        /// �õ�Title
        /// </summary>
        public string GetTitle(int ID)
        {
            return dal.GetTitle(ID);
        }
                
        /// <summary>
        /// �õ�һ���û�ID
        /// </summary>
        public int GetUsID(int ID)
        {
            return dal.GetUsID(ID);
        }
                
        /// <summary>
        /// �õ�һ��NodeId
        /// </summary>
        public int GetNodeId(int ID)
        {
            return dal.GetNodeId(ID);
        }

        /// <summary>
        /// �õ�IsTop
        /// </summary>
        public int GetIsTop(int ID)
        {
            return dal.GetIsTop(ID);
        }

		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public BCW.Model.Diary GetDiary(int ID)
		{
			
			return dal.GetDiary(ID);
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
        /// <param name="strOrder">��������</param>
		/// <returns>IList Diary</returns>
		public IList<BCW.Model.Diary> GetDiarys(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
		{
			return dal.GetDiarys(p_pageIndex, p_pageSize, strWhere, strOrder, out p_recordCount);
		}
               
        /// <summary>
        /// ��ʾN�����ռ�
        /// </summary>
        /// <param name="p_Size">��ʾ����</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <returns>IList Diary</returns>
        public IList<BCW.Model.Diary> GetDiarysTop(int p_Size, string strWhere)
        {
            return dal.GetDiarysTop(p_Size, strWhere);
        }
		#endregion  ��Ա����
	}
}

