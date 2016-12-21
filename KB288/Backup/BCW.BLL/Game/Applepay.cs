using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model.Game;
namespace BCW.BLL.Game
{
	/// <summary>
	/// ҵ���߼���Applepay ��ժҪ˵����
	/// </summary>
	public class Applepay
	{
		private readonly BCW.DAL.Game.Applepay dal=new BCW.DAL.Game.Applepay();
		public Applepay()
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
        public bool Exists(int Types, int UsID, int AppleId)
        {
            return dal.Exists(Types, UsID, AppleId);
		}
                       
        /// <summary>
        /// �Ƿ����δ����¼
        /// </summary>
        public bool ExistsState(int AppleId)
        {
            return dal.ExistsState(AppleId);
        }

        /// <summary>
        /// �Ƿ���ڶҽ���¼
        /// </summary>
        public bool ExistsState(int ID, int UsID)
        {
            return dal.ExistsState(ID, UsID);
        }
        
        /// <summary>
        /// ����ĳID�ڱ�����ע��
        /// </summary>
        public int GetCount(int UsID, int AppleId)
        {
            return dal.GetCount(UsID, AppleId);
        }
                
        /// <summary>
        /// ���㱾��ĳ������ע��
        /// </summary>
        public long GetCount2(int Types, int AppleId)
        {
            return dal.GetCount2(Types, AppleId);
        }

		/// <summary>
		/// ����һ������
		/// </summary>
		public int  Add(BCW.Model.Game.Applepay model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Update(BCW.Model.Game.Applepay model)
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
        /// ���¿���
        /// </summary>
        public void Update(int AppleId, int Types)
        {
            dal.Update(AppleId, Types);
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
		public void Delete(int Types,int ID)
		{
			
			dal.Delete(Types,ID);
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
		public BCW.Model.Game.Applepay GetApplepay(int Types,int ID)
		{
			
			return dal.GetApplepay(Types,ID);
		}

        /// <summary>
        /// �õ�һ��WinCent
        /// </summary>
        public long GetWinCent(int ID)
        {
            return dal.GetWinCent(ID);
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
		/// <returns>IList Applepay</returns>
		public IList<BCW.Model.Game.Applepay> GetApplepays(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetApplepays(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

		#endregion  ��Ա����
	}
}

