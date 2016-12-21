using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using BCW.Data;
using BCW.Common;
namespace BCW.BLL
{
	/// <summary>
	/// ҵ���߼���yg_BuyLists ��ժҪ˵����
	/// </summary>
	public class yg_BuyLists
	{
		private readonly BCW.DAL.yg_BuyLists dal=new BCW.DAL.yg_BuyLists();
		public yg_BuyLists()
		{}
		#region  ��Ա����
		/// <summary>
		/// �Ƿ���ڸü�¼
		/// </summary>
		public bool Exists(long Id)
		{
			return dal.Exists(Id);
		}
        /// <summary>
        /// �õ����ID
        /// </summary>
        public int GetMaxId()
        {
            return dal.GetMaxId();
        }
        /// <summary>
        /// ͨ��Id���UserId
        /// </summary>
        public long GetUserId(long Id)
        {
            return dal.GetUserId(Id);
        }
		/// <summary>
		/// ����һ������
		/// </summary>
		public int  Add(BCW.Model.yg_BuyLists model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Update(BCW.Model.yg_BuyLists model)
		{
			dal.Update(model);
		}
        
        /// <summary>
        /// ����һAddress==1
        /// </summary>
        public void UpdateAddress(long Id,string address)
        {
            dal.UpdateAddress(Id,address);
        }

		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public void Delete(long Id)
		{
			
			dal.Delete(Id);
		}

		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public BCW.Model.yg_BuyLists Getyg_BuyLists(long Id)
		{
			
			return dal.Getyg_BuyLists(Id);
		}
        /// <summary>
        /// �õ�ĳgoods�����¹�����
        /// </summary>
        public BCW.Model.yg_BuyLists Getyg_BuyListsForGoods(long Id)
        {

            return dal.Getyg_BuyListsForGoods(Id);
        }
        /// <summary>
        /// �õ�ĳgoods�Ƿ�����ƹ���
        /// </summary>
        public bool Getyg_BuyListsForYungouma(int Id, int GoodsNum)//
        {
            return dal.Getyg_BuyListsForYungouma(Id, GoodsNum);
        }
        /// <summary>
        /// //ͨ���ƹ����жϷ�����
        /// </summary>  
        public long GetUserId_yg_BuyListsForYungouma(int Id, int GoodsNum)
        {
            return dal.GetUserId_yg_BuyListsForYungouma(Id, GoodsNum);
        }
		/// <summary>
		/// �����ֶ�ȡ�����б�
		/// </summary>
		public DataSet GetList(string strField, string strWhere)
		{
			return dal.GetList(strField, strWhere);
		}
        /// <summary>
        /// �����ֶ�ȡ�����б�
        /// </summary>
        public DataSet GetListTop(string strField, string strWhere)
        {
            return dal.GetListTop(strField, strWhere);
        }
        /// <summary>
        /// ����Idȡ��Idǰ��100����¼
        /// </summary>
        public DataSet GetListTopId(long Id)
        {
            return dal.GetListTopId(Id);
        }

		/// <summary>
		/// ȡ��ÿҳ��¼
		/// </summary>
		/// <param name="p_pageIndex">��ǰҳ</param>
		/// <param name="p_pageSize">��ҳ��С</param>
		/// <param name="p_recordCount">�����ܼ�¼��</param>
		/// <param name="strWhere">��ѯ����</param>
		/// <returns>IList yg_BuyLists</returns>
		public IList<BCW.Model.yg_BuyLists> Getyg_BuyListss(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.Getyg_BuyListss(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}


		#endregion  ��Ա����
	}
}

