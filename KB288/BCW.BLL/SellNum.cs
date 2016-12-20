using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
	/// <summary>
	/// ҵ���߼���SellNum ��ժҪ˵����
	/// </summary>
	public class SellNum
	{
		private readonly BCW.DAL.SellNum dal=new BCW.DAL.SellNum();
		public SellNum()
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
        public bool Exists(int Types, int ID)
        {
            return dal.Exists(Types, ID);
        }

        /// <summary>
        /// �Ƿ���ڸ��ύ��¼
        /// </summary>
        public bool Exists(int Types, int BuyUID, int State)
        {
            return dal.Exists(Types, BuyUID, State);
        }

        /// <summary>
        /// �Ƿ���ڸ��ύ��¼
        /// </summary>
        public bool Exists(int Types, int UsID, int BuyUID, int State)
        {
            return dal.Exists(Types, UsID, BuyUID, State);
        }
                
        /// <summary>
        /// ����ĳ��Ա����һ��˶��ٳ�ֵ��Q��
        /// </summary>
        public int GetSumBuyUID(int Types, int UsID)
        {
            return dal.GetSumBuyUID(Types, UsID);
        }
                    
        /// <summary>
        /// ÿ��IDÿ��ֻ��Ϊ2��QQ�Ž��п�ͨ����
        /// </summary>
        public int GetSumQQCount(int UsID)
        {
            return dal.GetSumQQCount(UsID);
        }

        /// <summary>
        /// ����ĳ��Ա�����ѯ���ٴα���
        /// </summary>
        public int GetCount(int Types, int UsID)
        {
            return dal.GetCount(Types, UsID);
        }
                
        /// <summary>
        /// ����ĳQQ��ÿ������ͨ���·�����
        /// </summary>
        public int GetSumBuyUIDQQ(int Tags, string Mobile, int UsID)
        {
            return dal.GetSumBuyUIDQQ(Tags, Mobile, UsID);
        }

		/// <summary>
		/// ����һ������
		/// </summary>
		public int  Add(BCW.Model.SellNum model)
		{
			return dal.Add(model);
		}

        /// <summary>
        /// ����һ������
        /// </summary>
        public int Add2(BCW.Model.SellNum model)
        {
            return dal.Add2(model);
        }

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Update(BCW.Model.SellNum model)
		{
			dal.Update(model);
		}

        /// <summary>
        /// �����ѱ���
        /// </summary>
        public void UpdateState2(int ID, long Price)
        {
            dal.UpdateState2(ID, Price);
        }

        /// <summary>
        /// ���³�Ϊ������һ�
        /// </summary>
        public void UpdateState3(int ID, string Mobile)
        {
            dal.UpdateState3(ID, Mobile);
        }
               
        /// <summary>
        /// ���³�Ϊ�ѳɹ�
        /// </summary>
        public void UpdateState4(int ID, string Notes)
        {
            dal.UpdateState4(ID, Notes);
        }
                
        /// <summary>
        /// ���³�Ϊ�ѳ���
        /// </summary>
        public void UpdateState9(int ID)
        {
            dal.UpdateState9(ID);
        }

        /// <summary>
        /// ����Notes
        /// </summary>
        public void UpdateNotes(int ID, string Notes)
        {
            dal.UpdateNotes(ID, Notes);
        }
                
        /// <summary>
        /// ����QQ��������
        /// </summary>
        public void UpdateTags(int ID, int Tags)
        {
            dal.UpdateTags(ID, Tags);
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
		public BCW.Model.SellNum GetSellNum(int ID)
		{
			
			return dal.GetSellNum(ID);
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
		/// <returns>IList SellNum</returns>
		public IList<BCW.Model.SellNum> GetSellNums(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetSellNums(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

		#endregion  ��Ա����
	}
}

