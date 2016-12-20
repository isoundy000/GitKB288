using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.dzpk.Model;
namespace BCW.dzpk.BLL
{
	/// <summary>
	/// ҵ���߼���DzpkAct ��ժҪ˵����
	/// </summary>
	public class DzpkAct
	{
		private readonly BCW.dzpk.DAL.DzpkAct dal=new BCW.dzpk.DAL.DzpkAct();
		public DzpkAct()
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
		public int  Add(BCW.dzpk.Model.DzpkAct model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Update(BCW.dzpk.Model.DzpkAct model)
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
		public BCW.dzpk.Model.DzpkAct GetDzpkAct(int ID)
		{
			
			return dal.GetDzpkAct(ID);
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
		/// <returns>IList DzpkAct</returns>
		public IList<BCW.dzpk.Model.DzpkAct> GetDzpkActs(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetDzpkActs(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

        /// <summary>
        /// ȡ��ÿҳ��¼
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">��ҳ��С</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <returns>IList DzpkAct</returns>
        public IList<BCW.dzpk.Model.DzpkAct> GetDzpkActs(int p_pageIndex, int p_pageSize, string strWhere,string sOrder, out int p_recordCount)
        {
            return dal.GetDzpkActs(p_pageIndex, p_pageSize, strWhere, sOrder, out p_recordCount);
        }

        #endregion  ��Ա����
    }
}

