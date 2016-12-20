using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using TPR2.Model.guess;
namespace TPR2.BLL.guess
{
	/// <summary>
	/// ҵ���߼���SuperOrder ��ժҪ˵����
	/// </summary>
	public class SuperOrder
	{
		private readonly TPR2.DAL.guess.SuperOrder dal=new TPR2.DAL.guess.SuperOrder();
		public SuperOrder()
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
		public int  Add(TPR2.Model.guess.SuperOrder model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Update(TPR2.Model.guess.SuperOrder model)
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
		/// ɾ��һ������
		/// </summary>
		public void DeleteStr()
		{
			
			dal.DeleteStr();
		}

		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public TPR2.Model.guess.SuperOrder GetSuperOrder(int ID)
		{
			
			return dal.GetSuperOrder(ID);
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
		/// <returns>IList SuperOrder</returns>
		public IList<TPR2.Model.guess.SuperOrder> GetSuperOrders(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetSuperOrders(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

		#endregion  ��Ա����
	}
}

