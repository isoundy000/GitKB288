using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
	/// <summary>
	/// ҵ���߼���GoodsList ��ժҪ˵����
	/// </summary>
	public class GoodsList
	{
		private readonly BCW.DAL.GoodsList dal=new BCW.DAL.GoodsList();
		public GoodsList()
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
        /// ͨ����Ʒ��Ż�ȡ��Ʒstatue
        /// </summary>
        ///  
         public string Getstatue(long Id)
        {
            return dal.Getstatue(Id);
        }
        /// <summary>
        /// ͨ����Ʒ��Ż�ȡ��Ʒ����
        /// </summary>
        ///  
       public string GetGoodsName(long Id)
        {
            return dal.GetGoodsName(Id);
        }
       /// <summary>
       /// ͨ����Ʒ��Ż�ȡ��Ʒ����
       /// </summary>
       ///  
       public long Getperiods(long Id)
       {
           return dal.Getperiods(Id);
       }

		/// <summary>
		/// ����һ������
		/// </summary>
		public int  Add(BCW.Model.GoodsList model)
		{
			return dal.Add(model);
		}
              /// <summary>
        /// ����һ������
        /// </summary>
        public void UpdateGoodsType(long Id, int GoodsType)
        {
            dal.UpdateGoodsType(Id, GoodsType);
        }
         /// <summary>
        /// ����һ������
        /// </summary>
        public void UpdateisDone(long Id, int isDone)
        {
            dal.UpdateisDone(Id,isDone);
        }
        /// <summary>
        /// ����һ������
        /// </summary>
        public void UpdateNum(long Id,long number)
        {
            dal.UpdateNum(Id,number);
        }
        /// <summary>
        /// ����һ������
        /// </summary>
        public void UpdateYunGouMa(long Id, string StockYungouma)
        {
            dal.UpdateYunGouMa(Id, StockYungouma);
        }

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Update(BCW.Model.GoodsList model)
		{
			dal.Update(model);
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
		public BCW.Model.GoodsList GetGoodsList(long Id)
		{

            return dal.GetGoodsList(Id);
		}
        public string GetGoodsStatue(int Id)
        {
            return dal.Getstatue(Id);
        }
		/// <summary>
		/// �����ֶ�ȡ�����б�
		/// </summary>
		public DataSet GetList(string strField, string strWhere)
		{
			return dal.GetList(strField, strWhere);
		}
        /// <summary>
        /// ȡ��ÿҳ������¼
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">��ҳ��С</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <returns>IList GoodsList</returns>
        public IList<BCW.Model.GoodsList> GetGoodsListsForGoodsOpen(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetGoodsListsForGoodsOpen(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }


		/// <summary>
		/// ȡ��ÿҳ��¼
		/// </summary>
		/// <param name="p_pageIndex">��ǰҳ</param>
		/// <param name="p_pageSize">��ҳ��С</param>
		/// <param name="p_recordCount">�����ܼ�¼��</param>
		/// <param name="strWhere">��ѯ����</param>
		/// <returns>IList GoodsList</returns>
		public IList<BCW.Model.GoodsList> GetGoodsLists(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetGoodsLists(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

		#endregion  ��Ա����
	}
}

