using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Shop.Model;
namespace BCW.Shop.BLL
{
	/// <summary>
	/// ҵ���߼���Shopgoods ��ժҪ˵����
	/// </summary>
	public class Shopgoods
	{
		private readonly BCW.Shop.DAL.Shopgoods dal=new BCW.Shop.DAL.Shopgoods();
		public Shopgoods()
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
        public bool Existsgd(int ShopGiftId, int UsID)
        {
            return dal.Existsgd(ShopGiftId, UsID);
        }
		/// <summary>
		/// ����һ������
		/// </summary>
		public int  Add(BCW.Shop.Model.Shopgoods model)
		{
			return dal.Add(model);
		}
        /// <summary>
        /// ����һ������
        /// </summary>
        public void UpdateMessagebyid(int ID, string RealName, string Address, string Phone, string Email, string Message)
        {
            dal.UpdateMessagebyid(ID, RealName, Address, Phone, Email, Message);
        }
        /// <summary>
        /// ����һ������
        /// </summary>
        public void UpdateMessagebyID(int ID, string Express, string Expressnum,DateTime SendTime)
        {
            dal.UpdateMessagebyID(ID, Express, Expressnum, SendTime);
        }
        /// <summary>
        /// ����һ������
        /// </summary>
        public void UpdateReceivebyID(int ID,DateTime ReceiveTime)
        {
            dal.UpdateReceivebyID(ID, ReceiveTime);
        }
		/// <summary>
		/// ����һ������
		/// </summary>
		public void Update(BCW.Shop.Model.Shopgoods model)
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
		public BCW.Shop.Model.Shopgoods GetShopgoods(int ID)
		{
			
			return dal.GetShopgoods(ID);
		}
        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public BCW.Shop.Model.Shopgoods GetShopgoods1(int ShopGiftId)
        {

            return dal.GetShopgoods1(ShopGiftId);
        }
		/// <summary>
		/// �����ֶ�ȡ�����б�
		/// </summary>
		public DataSet GetList(string strField, string strWhere)
		{
			return dal.GetList(strField, strWhere);
		}
        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool Exists(int UsID, int ID)
        {
            return dal.Exists(UsID, ID);
        }
		/// <summary>
		/// ȡ��ÿҳ��¼
		/// </summary>
		/// <param name="p_pageIndex">��ǰҳ</param>
		/// <param name="p_pageSize">��ҳ��С</param>
		/// <param name="p_recordCount">�����ܼ�¼��</param>
		/// <param name="strWhere">��ѯ����</param>
		/// <returns>IList Shopgoods</returns>
		public IList<BCW.Shop.Model.Shopgoods> GetShopgoodss(int p_pageIndex, int p_pageSize, string strWhere,string strOrder, out int p_recordCount)
		{
			return dal.GetShopgoodss(p_pageIndex, p_pageSize, strWhere,strOrder, out p_recordCount);
		}

		#endregion  ��Ա����
	}
}

