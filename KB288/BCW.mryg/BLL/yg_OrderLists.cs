using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
	/// <summary>
	/// ҵ���߼���yg_OrderLists ��ժҪ˵����
	/// </summary>
	public class yg_OrderLists
	{
		private readonly BCW.DAL.yg_OrderLists dal=new BCW.DAL.yg_OrderLists();
		public yg_OrderLists()
		{}
		#region  ��Ա����
		/// <summary>
		/// �Ƿ���ڸü�¼
		/// </summary>
		public bool Exists(int Id)
		{
			return dal.Exists(Id);
		}
         /// <summary>
		/// �Ƿ���ڸü�¼
		/// </summary>
        public bool ExistsGoodsId(int GoodsId)
		{
            return dal.ExistsGoodsId(GoodsId);
		}
        
		/// <summary>
		/// ����һ������
		/// </summary>
		public int  Add(BCW.Model.yg_OrderLists model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Update(BCW.Model.yg_OrderLists model)
		{
			dal.Update(model);
		}

		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public void Delete(int Id)
		{
			
			dal.Delete(Id);
		}
        /// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
        public BCW.Model.yg_OrderLists Getyg_OrderListsForGoodsListId(long GoodsListId)
		{

            return dal.Getyg_OrderListsForGoodsListId(GoodsListId);
		}

		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public BCW.Model.yg_OrderLists Getyg_OrderLists(int Id)
		{
			
			return dal.Getyg_OrderLists(Id);
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
		/// <returns>IList yg_OrderLists</returns>
		public IList<BCW.Model.yg_OrderLists> Getyg_OrderListss(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.Getyg_OrderListss(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

		#endregion  ��Ա����
	}
}

