using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.QuickBet.Model;
namespace BCW.QuickBet.BLL
{
	/// <summary>
	/// ҵ���߼���QuickBet ��ժҪ˵����
	/// </summary>
	public class QuickBet
	{
		private readonly BCW.QuickBet.DAL.QuickBet dal=new BCW.QuickBet.DAL.QuickBet();
		public QuickBet()
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
        /// �õ�Ĭ����Ϸ
        /// </summary>
        public string GetGame()
        {
            string str="1#2#3#4#5#6#7#8#9#10";//֧��ʮ����Ϸ���
            return str;
        }

        /// <summary>
        /// �õ�Ĭ�Ͽ����ע
        /// </summary>
        public string GetBety()
        {
            string str = "100|500|1000|10000|1000000|0|0|0|0|0#100|500|1000|10000|1000000|0|0|0|0|0#100|500|1000|10000|1000000|0|0|0|0|0#100|500|1000|10000|1000000|0|0|0|0|0#100|500|1000|10000|1000000|0|0|0|0|0#100|500|1000|10000|1000000|0|0|0|0|0#100|500|1000|10000|1000000|0|0|0|0|0#100|500|1000|10000|1000000|0|0|0|0|0#100|500|1000|10000|1000000|0|0|0|0|0#100|500|1000|10000|1000000|0|0|0|0|0";
            return str;
        }

		/// <summary>
		/// �Ƿ���ڸü�¼
		/// </summary>
		public bool Exists(int ID)
		{
			return dal.Exists(ID);
		}

        /// <summary>
        /// �Ƿ���ڸ��û�
        /// </summary>
        public bool ExistsUsID(int UsID)
        {
            return dal.ExistsUsID(UsID);
        }

		/// <summary>
		/// ����һ������
		/// </summary>
		public int  Add(BCW.QuickBet.Model.QuickBet model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Update(BCW.QuickBet.Model.QuickBet model)
		{
			dal.Update(model);
		}

        /// <summary>
        /// ����Game
        /// </summary>
        public void UpdateGame(int UsID,string Game)
        {
            dal.UpdateGame(UsID,Game);
        }

        /// <summary>
        /// ����Bet
        /// </summary>
        public void UpdateBet(int UsID, string Bet)
        {
            dal.UpdateBet(UsID, Bet);
        }

		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public void Delete(int ID)
		{
			
			dal.Delete(ID);
		}

        /// <summary>
        /// �����û�ID�õ�Game
        /// </summary>
        public string GetGame(int UsID)
        {
          return  dal.GetGame(UsID);
        }

        /// <summary>
        /// �����û�ID�õ�Bet
        /// </summary>
        public string  GetBet(int UsID)
        {
           return  dal.GetBet(UsID);
        }

		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public BCW.QuickBet.Model.QuickBet GetQuickBet(int ID)
		{
			
			return dal.GetQuickBet(ID);
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
		/// <returns>IList QuickBet</returns>
		public IList<BCW.QuickBet.Model.QuickBet> GetQuickBets(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetQuickBets(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

		#endregion  ��Ա����
	}
}

