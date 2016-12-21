using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model.Game;
namespace BCW.BLL.Game
{
	/// <summary>
	/// ҵ���߼���GiftFlows ��ժҪ˵����
	/// </summary>
	public class GiftFlows
	{
		private readonly BCW.DAL.Game.GiftFlows dal=new BCW.DAL.Game.GiftFlows();
		public GiftFlows()
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
        public bool Exists(int Types, int UsID)
        {
            return dal.Exists(Types, UsID);
        }
 
        /// <summary>
        /// �Ƿ���ڸü�¼(�涨����)
        /// </summary>
        public bool Exists(int Types, int UsID, int Totall)
        {
            return dal.Exists(Types, UsID, Totall);
        }
                
        /// <summary>
        /// �Ƿ���ڸü�¼(N����)
        /// </summary>
        public bool ExistsSec(int Types, int UsID, int Sec)
        {
            return dal.ExistsSec(Types, UsID, Sec);
        }

        /// <summary>
        /// ���㲻ͬ��Ʒ�ж��ٸ�����
        /// </summary>
        public int GetTypesTotal(int UsID)
        {
            return dal.GetTypesTotal(UsID);
        }

        /// <summary>
        /// ����ĳ�û���������
        /// </summary>
        public int GetTotal(int UsID)
        {
            return dal.GetTotal(UsID);
        }

        /// <summary>
        /// ����ĳ�û�����ʣ����
        /// </summary>
        public int GetTotall(int UsID)
        {
            return dal.GetTotall(UsID);
        }

		/// <summary>
		/// ����һ������
		/// </summary>
		public int  Add(BCW.Model.Game.GiftFlows model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Update(BCW.Model.Game.GiftFlows model)
		{
			dal.Update(model);
		}
                
        /// <summary>
        /// ����һ������
        /// </summary>
        public int UpdateTotall(int Types, int UsID, int Totall)
        {
           return dal.UpdateTotall(Types, UsID, Totall);
        }
               
        /// <summary>
        /// ����һ������
        /// </summary>
        public int UpdateTotall(int ID, int Totall)
        {
            return dal.UpdateTotall(ID, Totall);
        }

		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public void Delete(int Types,int ID)
		{
			
			dal.Delete(Types,ID);
		}

		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public BCW.Model.Game.GiftFlows GetGiftFlows(int Types,int ID)
		{
			
			return dal.GetGiftFlows(Types,ID);
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
		/// <returns>IList GiftFlows</returns>
		public IList<BCW.Model.Game.GiftFlows> GetGiftFlowss(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetGiftFlowss(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}
 
        /// <summary>
        /// ȡ�����а�
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">ÿҳ��ʾ��¼��</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <returns>List</returns>
        public IList<BCW.Model.Game.GiftFlows> GetGiftFlowssTop(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetGiftFlowssTop(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }

        /// <summary>
        /// ȡ�����а�
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">ÿҳ��ʾ��¼��</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <returns>List</returns>
        public IList<BCW.Model.Game.GiftFlows> GetGiftFlowssTop2(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetGiftFlowssTop2(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }

		#endregion  ��Ա����
	}
}

