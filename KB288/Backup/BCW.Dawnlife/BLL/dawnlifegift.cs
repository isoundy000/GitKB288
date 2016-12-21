using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
	/// <summary>
	/// ҵ���߼���dawnlifegift ��ժҪ˵����
	/// </summary>
	public class dawnlifegift
	{
		private readonly BCW.DAL.dawnlifegift dal=new BCW.DAL.dawnlifegift();
		public dawnlifegift()
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
		public int  Add(BCW.Model.dawnlifegift model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Update(BCW.Model.dawnlifegift model)
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
		public BCW.Model.dawnlifegift Getdawnlifegift(int ID)
		{
			
			return dal.Getdawnlifegift(ID);
		}
        /// <summary>
        /// ����coin��ֵ
        /// </summary>
        public long GetPrice(string coin, string strWhere)
        {
            return dal.GetPrice(coin, strWhere);
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
		/// <returns>IList dawnlifegift</returns>
		public IList<BCW.Model.dawnlifegift> Getdawnlifegifts(int p_pageIndex, int p_pageSize, string strWhere,string strOrder, out int p_recordCount)
		{
			return dal.Getdawnlifegifts(p_pageIndex, p_pageSize, strWhere,strOrder, out p_recordCount);
		}

		#endregion  ��Ա����
	}
}

