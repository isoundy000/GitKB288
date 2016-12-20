using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model.Game;
namespace BCW.BLL.Game
{
	/// <summary>
	/// ҵ���߼���Dicepay ��ժҪ˵����
	/// </summary>
	public class Dicepay
	{
		private readonly BCW.DAL.Game.Dicepay dal=new BCW.DAL.Game.Dicepay();
		public Dicepay()
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
        /// �Ƿ����δ����¼
        /// </summary>
        public bool ExistsState(int DiceId)
        {
            return dal.ExistsState(DiceId);
        }

        /// <summary>
        /// �Ƿ���ڶҽ���¼
        /// </summary>
        public bool ExistsState(int ID, int UsID)
        {
            return dal.ExistsState(ID, UsID);
        }

        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool Exists(int DiceId, int UsID, int bzType, int Types, int BuyNum)
        {
            return dal.Exists(DiceId, UsID, bzType, Types, BuyNum);
        }
                
        /// <summary>
        /// ����ĳ�ڹ�������
        /// </summary>
        public int GetCount(int DiceId)
        {
            return dal.GetCount(DiceId);
        }

        /// <summary>
        /// ����ĳ��ĳѡ�������
        /// </summary>
        public int GetCount(int DiceId, int Types, int BuyNum)
        {
            return dal.GetCount(DiceId, Types, BuyNum);
        }
                
        /// <summary>
        /// ����ĳ��ĳѡ������
        /// </summary>
        public long GetSumBuyCent(int DiceId, int bzType, int Types, int BuyNum)
        {
            return dal.GetSumBuyCent(DiceId, bzType, Types, BuyNum);
        }

        /// <summary>
        /// ����ĳ��ĳѡ�������
        /// </summary>
        public int GetCount(int DiceId, int Types, string BuyNum)
        {
            return dal.GetCount(DiceId, Types, BuyNum);
        }

        /// <summary>
        /// ����ĳ��ĳѡ������
        /// </summary>
        public long GetSumBuyCent(int DiceId, int bzType, int Types, string BuyNum)
        {
            return dal.GetSumBuyCent(DiceId, bzType, Types, BuyNum);
        }
        
        /// <summary>
        /// ���������ע�ܱҶ�
        /// </summary>
        public long GetSumBuyCent(int BzType)
        {
            return dal.GetSumBuyCent(BzType);
        }

        /// <summary>
        /// ���������ע�����ܱҶ�
        /// </summary>
        public long GetSumWinCent(int BzType)
        {
            return dal.GetSumWinCent(BzType);
        }
        
        /// <summary>
        /// ������������ұ���ֵ
        /// </summary>
        public long GetSumBuyCent(string strWhere)
        {
            return dal.GetSumBuyCent(strWhere);
        }
               
        /// <summary>
        /// �����������㷵��ֵ
        /// </summary>
        public long GetSumWinCent(string strWhere)
        {
            return dal.GetSumWinCent(strWhere);
        }

		/// <summary>
		/// ����һ������
		/// </summary>
		public int  Add(BCW.Model.Game.Dicepay model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Update(BCW.Model.Game.Dicepay model)
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
        /// �õ�һ��WinCent
        /// </summary>
        public long GetWinCent(int ID)
        {
            return dal.GetWinCent(ID);
        }
                
        /// <summary>
        /// �õ�һ��bzType
        /// </summary>
        public int GetbzType(int ID)
        {
            return dal.GetbzType(ID);
        }

		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public BCW.Model.Game.Dicepay GetDicepay(int ID)
		{
			
			return dal.GetDicepay(ID);
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
		/// <returns>IList Dicepay</returns>
		public IList<BCW.Model.Game.Dicepay> GetDicepays(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetDicepays(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}
                /// <summary>
        /// ȡ�����м�¼
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">��ҳ��С</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <returns>IList Dicepay</returns>
        public IList<BCW.Model.Game.Dicepay> GetDicepaysTop(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetDicepaysTop(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }
		#endregion  ��Ա����
	}
}

