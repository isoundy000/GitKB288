using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.dzpk.Model;
namespace BCW.dzpk.BLL
{
	/// <summary>
	/// �������а�
	/// </summary>
	public class DzpkRankList
	{
		private readonly BCW.dzpk.DAL.DzpkRankList dal=new BCW.dzpk.DAL.DzpkRankList();
		public DzpkRankList()
		{}
		#region  ��Ա����
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
		public int  Add(BCW.dzpk.Model.DzpkRankList model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Update(BCW.dzpk.Model.DzpkRankList model)
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
		/// �õ�һ������ʵ��
		/// </summary>
		public BCW.dzpk.Model.DzpkRankList GetDzpkRankList(int ID)
		{
			
			return dal.GetDzpkRankList(ID);
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
		/// <returns>IList DzpkRankList</returns>
		public IList<BCW.dzpk.Model.DzpkRankList> GetDzpkRankLists(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetDzpkRankLists(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

        /// <summary>
		/// ȡ�����а�ϼƵ�ÿҳ��¼
		/// </summary>
		/// <param name="p_pageIndex">��ǰҳ</param>
		/// <param name="p_pageSize">��ҳ��С</param>
		/// <param name="p_recordCount">�����ܼ�¼��</param>
		/// <param name="strWhere">��ѯ����</param>
		/// <returns>IList DzpkRankList</returns>
        public IList<BCW.dzpk.Model.DzpkRankList> GetDzpkRankLists_Total(int p_pageIndex, int p_pageSize, string strWhere,string Sort,string OrderBy, out int p_recordCount,out string strsql)
		{
            return dal.GetDzpkRankLists_Total(p_pageIndex, p_pageSize, strWhere, Sort, OrderBy, out p_recordCount, out strsql);
		}

        public IList<BCW.dzpk.Model.DzpkRankList> GetDzpkRankLists_Total_All(string strWhere, string Sort, string OrderBy)
        {
            return dal.GetDzpkRankLists_Total_All(strWhere, Sort, OrderBy);
        }

        #endregion  ��Ա����
    }
}

