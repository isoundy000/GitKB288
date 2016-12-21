using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model.Game;
namespace BCW.BLL.Game
{
	/// <summary>
	/// ҵ���߼���Horsepay ��ժҪ˵����
	/// </summary>
	public class Horsepay
	{
		private readonly BCW.DAL.Game.Horsepay dal=new BCW.DAL.Game.Horsepay();
		public Horsepay()
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
        public bool ExistsState(int HorseId)
        {
            return dal.ExistsState(HorseId);
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
        public bool Exists(int StkId, int UsID, int bzType, int Types)
        {
            return dal.Exists(StkId, UsID, bzType, Types);
        }
               
        /// <summary>
        /// ����ĳ�ڹ�������
        /// </summary>
        public int GetCount(int HorseId)
        {
            return dal.GetCount(HorseId);
        }

        /// <summary>
        /// ����ĳ��ĳѡ�������
        /// </summary>
        public int GetCount(int HorseId, int Types)
        {
            return dal.GetCount(HorseId, Types);
        }

        /// <summary>
        /// ����ĳ��ĳѡ������
        /// </summary>
        public long GetSumBuyCent(int HorseId, int bzType, int Types)
        {
            return dal.GetSumBuyCent(HorseId, bzType, Types);
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
		public int  Add(BCW.Model.Game.Horsepay model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Update(BCW.Model.Game.Horsepay model)
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
		/// �õ�һ������ʵ��
		/// </summary>
		public BCW.Model.Game.Horsepay GetHorsepay(int ID)
		{
			
			return dal.GetHorsepay(ID);
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
		/// <returns>IList Horsepay</returns>
		public IList<BCW.Model.Game.Horsepay> GetHorsepays(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetHorsepays(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

                /// <summary>
        /// ȡ�����м�¼
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">��ҳ��С</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <returns>IList Horsepay</returns>
        public IList<BCW.Model.Game.Horsepay> GetHorsepaysTop(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetHorsepaysTop(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }

		#endregion  ��Ա����
	}
}

