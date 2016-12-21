using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using Book.Model;
namespace Book.BLL
{
	/// <summary>
	/// ҵ���߼���Contents ��ժҪ˵����
	/// </summary>
	public class Contents
	{
		private readonly Book.DAL.Contents dal=new Book.DAL.Contents();
		public Contents()
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
        /// �ָ���ɾ��
        /// </summary>
        public void Updateisdel(int id, int isdel)
        {
            dal.Updateisdel(id, isdel);
        }

		/// <summary>
		/// �Ƿ���ڸü�¼
		/// </summary>
		public bool Exists(int id)
		{
			return dal.Exists(id);
		}

        /// <summary>
        /// ����δ��˵��½�����
        /// </summary>
        public int GetCount(int shi)
        {
            return dal.GetCount(shi);
        }
        
        /// <summary>
        /// ����ĳ�鱾�½ڻ�־�����types(0����/1�־�)
        /// </summary>
        public int GetCount(int shi, int jid, int types)
        {
            return dal.GetCount(shi, jid, types);
        }

		/// <summary>
		/// ����һ������
		/// </summary>
		public int  Add(Book.Model.Contents model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Update(Book.Model.Contents model)
		{
			dal.Update(model);
		}
                
        /// <summary>
        /// ����½�
        /// </summary>
        public void Updatestate(int id, int state)
        {
            dal.Updatestate(id, state);
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
        /// �õ��������
        /// </summary>
        public string GetTitle(int id)
        {
            return dal.GetTitle(id);
        }

		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public Book.Model.Contents GetContents(int id)
		{
			
			return dal.GetContents(id);
		}

		/// <summary>
		/// �����ֶ�ȡ�����б�
		/// </summary>
		public DataSet GetList(string strField, string strWhere)
		{
			return dal.GetList(strField, strWhere);
		}

        /// <summary>
        /// ȡ����(��)һ����¼
        /// </summary>
        public Book.Model.Contents GetPreviousNextContents(int ID, int shi, int jid, bool p_next)
        {
            return dal.GetPreviousNextContents(ID, shi, jid, p_next);
        }

		/// <summary>
		/// ȡ��ÿҳ��¼
		/// </summary>
		/// <param name="p_pageIndex">��ǰҳ</param>
		/// <param name="p_pageSize">��ҳ��С</param>
		/// <param name="p_recordCount">�����ܼ�¼��</param>
		/// <param name="strWhere">��ѯ����</param>
		/// <returns>IList Contents</returns>
		public IList<Book.Model.Contents> GetContentss(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetContentss(p_pageIndex, p_pageSize, strWhere, "pid desc", out p_recordCount);
		}

        /// <summary>
        /// ȡ��ÿҳ��¼
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">��ҳ��С</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <param name="strOrder">��������</param>
        /// <returns>IList Contents</returns>
        public IList<Book.Model.Contents> GetContentss(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
        {
            return dal.GetContentss(p_pageIndex, p_pageSize, strWhere, strOrder, out p_recordCount);
        }

		#endregion  ��Ա����
	}
}

