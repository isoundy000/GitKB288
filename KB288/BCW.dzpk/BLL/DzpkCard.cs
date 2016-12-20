using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.dzpk.Model;
namespace BCW.dzpk.BLL
{
	/// <summary>
	/// ҵ���߼���DzpkCard ��ժҪ˵����
	/// </summary>
	public class DzpkCard
	{
		private readonly BCW.dzpk.DAL.DzpkCard dal=new BCW.dzpk.DAL.DzpkCard();
		public DzpkCard()
		{}
		#region  ��Ա����

        /// <summary>
        /// ɾ���ñ��Ӧ���������
        /// </summary>
        public void DeleteByRmID(int ID)
        {
            dal.DeleteByRmID(ID);
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
		public int  Add(BCW.dzpk.Model.DzpkCard model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Update(BCW.dzpk.Model.DzpkCard model)
		{
			dal.Update(model);
		}

		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public void Delete(int ID,int RmID)
		{

            dal.Delete(ID, RmID);
		}

		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public BCW.dzpk.Model.DzpkCard GetDzpkCard(int ID,int RmID)
		{

            return dal.GetDzpkCard(ID, RmID);
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
		/// <returns>IList DzpkCard</returns>
		public IList<BCW.dzpk.Model.DzpkCard> GetDzpkCards(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetDzpkCards(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

		#endregion  ��Ա����
	}
}

