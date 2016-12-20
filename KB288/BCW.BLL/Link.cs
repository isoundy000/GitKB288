using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
	/// <summary>
	/// ҵ���߼���Link ��ժҪ˵����
	/// </summary>
	public class Link
	{
		private readonly BCW.DAL.Link dal=new BCW.DAL.Link();
		public Link()
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
        public bool Exists(string LinkUrl)
        {
            return dal.Exists(LinkUrl);
        }

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Add(BCW.Model.Link model)
		{
			dal.Add(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Update(BCW.Model.Link model)
		{
			dal.Update(model);
		}

        /// <summary>
        /// ������ϸͳ��
        /// </summary>
        public void UpdateReStats(int ID, string ReStats, string ReLastIP)
        {
            dal.UpdateReStats(ID, ReStats, ReLastIP);
        }

        /// <summary>
        /// ��������
        /// </summary>
        public void UpdateLinkIn(int ID)
        {
            dal.UpdateLinkIn(ID);
        }

        /// <summary>
        /// ��������
        /// </summary>
        public void UpdateLinkOut(int ID)
        {
            dal.UpdateLinkOut(ID);
        }

        /// <summary>
        /// ��������
        /// </summary>
        public void UpdateHidden(int ID)
        {
            dal.UpdateHidden(ID);
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
        public BCW.Model.Link GetLink(int ID)
        {

            return dal.GetLink(ID);
        }

        /// <summary>
        /// �õ�����ȫ��
        /// </summary>
        public string GetLinkName(int ID)
        {

            return dal.GetLinkName(ID);
        }

		/// <summary>
		/// �õ�������ַ
		/// </summary>
		public string GetLinkUrl(int ID)
		{
			
			return dal.GetLinkUrl(ID);
		}

        /// <summary>
        /// ��������б�
        /// </summary>
        public DataSet GetList(int TopNum, string strWhere)
        {
            return dal.GetList(TopNum, strWhere);
        }

        /// <summary>
        /// ��������б�
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
        /// <returns>IList Topics</returns>
        public IList<BCW.Model.Link> GetLinks(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
        {
            return dal.GetLinks(p_pageIndex, p_pageSize, strWhere, strOrder, out p_recordCount);
        }

		#endregion  ��Ա����
	}
}

