using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model.Game;
namespace BCW.BLL.Game
{
	/// <summary>
	/// ҵ���߼���Ballpay ��ժҪ˵����
	/// </summary>
	public class Ballpay
	{
		private readonly BCW.DAL.Game.Ballpay dal=new BCW.DAL.Game.Ballpay();
		public Ballpay()
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
        /// �Ƿ���ڶҽ���¼
        /// </summary>
        public bool ExistsState(int ID, int UsID)
        {
            return dal.ExistsState(ID, UsID);
        }
                
        /// <summary>
        /// �Ƿ����δ����¼
        /// </summary>
        public bool ExistsState(int BallId)
        {
            return dal.ExistsState(BallId);
        }
                
        /// <summary>
        /// �Ƿ����Ӧ���ֹ����¼
        /// </summary>
        public bool ExistsBuyNum(int BallId, int BuyNum, int UsID)
        {
            return dal.ExistsBuyNum(BallId, BuyNum, UsID);
        }

        /// <summary>
        /// ����ĳ��ĳ���ֵĹ�������
        /// </summary>
        public int GetCount(int BallId, int BuyNum)
        {
            return dal.GetCount(BallId, BuyNum);
        }

		/// <summary>
		/// ����һ������
		/// </summary>
		public int  Add(BCW.Model.Game.Ballpay model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Update(BCW.Model.Game.Ballpay model)
		{
			dal.Update(model);
		}
                        
        /// <summary>
        /// ���¿���
        /// </summary>
        public void Update(int ID, long WinCent, int State)
        {
            dal.Update(ID, WinCent, State);
        }

        /// <summary>
        /// �����û��ҽ���ʶ
        /// </summary>
        public void UpdateState(int ID)
        {
            dal.UpdateState(ID);
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
        public void Delete(string strWhere)
        {

            dal.Delete(strWhere);
        }

        /// <summary>
        /// �õ�һ��BuyCount
        /// </summary>
        public int GetBuyCount(int BallId,int UsID)
        {

            return dal.GetBuyCount(BallId, UsID);
        }

        /// <summary>
        /// �õ�һ��WinCent
        /// </summary>
        public long GetWinCent(int ID)
        {
            return dal.GetWinCent(ID);
        }

		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public BCW.Model.Game.Ballpay GetBallpay(int ID)
		{
			
			return dal.GetBallpay(ID);
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
		/// <returns>IList Ballpay</returns>
		public IList<BCW.Model.Game.Ballpay> GetBallpays(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetBallpays(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

		#endregion  ��Ա����
	}
}

