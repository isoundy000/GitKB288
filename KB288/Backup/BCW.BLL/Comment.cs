using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
	/// <summary>
	/// ҵ���߼���Comment ��ժҪ˵����
	/// </summary>
	public class Comment
	{
		private readonly BCW.DAL.Comment dal=new BCW.DAL.Comment();
		public Comment()
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
        /// ����ĳ��ԱID�����������
        /// </summary>
        public int GetCount(int UserId)
        {
            return dal.GetCount(UserId);
        }

		/// <summary>
		/// ����һ������
		/// </summary>
		public int  Add(BCW.Model.Comment model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Update(BCW.Model.Comment model)
		{
			dal.Update(model);
		}
                
        /// <summary>
        /// ���»ظ�����
        /// </summary>
        public void UpdateReText(int ID, string ReText)
        {
            dal.UpdateReText(ID, ReText);
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
        public void Delete2(int DetailId)
        {

            dal.Delete2(DetailId);
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public void Delete3(int NodeId)
        {

            dal.Delete3(NodeId);
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public void DeleteStr(string strWhere)
        {

            dal.DeleteStr(strWhere);
        }

        /// <summary>
        /// �õ�DetailId
        /// </summary>
        public int GetDetailId(int ID)
        {
            return dal.GetDetailId(ID);
        }
                       
        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public BCW.Model.Comment GetCommentMe(int ID)
        {

            return dal.GetCommentMe(ID);
        }

        /// <summary>
        /// ��������б�
        /// </summary>
        public DataSet GetList(int DetailId, int TopNum, int Types)
        {
            return dal.GetList(DetailId, TopNum, Types);
        }

        /// <summary>
        /// ȡ��ÿҳ��¼
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">��ҳ��С</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <returns>IList Topics</returns>
        public IList<BCW.Model.Comment> GetComments(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetComments(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }
		#endregion  ��Ա����
	}
}

