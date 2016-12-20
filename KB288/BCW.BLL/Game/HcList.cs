using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model.Game;
namespace BCW.BLL.Game
{
	/// <summary>
	/// ҵ���߼���HcList ��ժҪ˵����
	/// </summary>
	public class HcList
	{
        /// <summary>
        /// wdy 20160524
        /// </summary>
		private readonly BCW.DAL.Game.HcList dal=new BCW.DAL.Game.HcList();
		public HcList()
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
		public bool Exists(int id)
		{
			return dal.Exists(id);
		}
        /// <summary>
        /// �Ƿ���ڸÿ�������
        /// </summary>
        public bool Existsm(int num)
        {
            return dal.Existsm(num);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public int  Add(BCW.Model.Game.HcList model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Update(BCW.Model.Game.HcList model)
		{
			dal.Update(model);
		}

        /// <summary>
        /// ����һ������
        /// </summary>
        public void Update1(int CID, long payCent, int payCount)
        {
            dal.Update1(CID, payCent, payCount);
        }

        /// <summary>
        /// �õ���һ��CID
        /// </summary>
        public int CID()
        {
            return dal.CID();
        }
        /// <summary>
        /// ����һ������
        /// </summary>
        public void Update2(int CID, long payCent, int payCount)
        {
            dal.Update2(CID, payCent, payCount);
        }
        /// <summary>
        /// ����һ������
        /// </summary>
        public void UpdateResult(int CID, int Result)
        {
            dal.UpdateResult(CID, Result);
        }
        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public void Delete(int id)
		{
			
			dal.Delete(id);
		}
        /// <summary>
        /// ��ʼ��ĳ���ݱ�
        /// </summary>
        /// <param name="TableName">���ݱ�����</param>
        public void ClearTable(string TableName)
        {
            dal.ClearTable(TableName);
        }

        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public BCW.Model.Game.HcList GetHcList(int id)
		{
			
			return dal.GetHcList(id);
		}
        /// <summary>
        /// �õ�������Ͷע����
        /// </summary>
        public int GetcountRebot(int usid)
        {
            return dal.GetcountRebot(usid);
        }

        /// <summary>
        /// �õ�һ�����¶���ʵ��
        /// </summary>
        public BCW.Model.Game.HcList GetHcListNew(int State)
        {
            return dal.GetHcListNew(State);
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
		public DataSet GetList1(string strField, string strWhere)
        {
            return dal.GetList1(strField, strWhere);
        }

        /// <summary>
        /// ȡ�ù̶��б��¼
        /// </summary>
        /// <param name="SizeNum">�б��¼��</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <returns>IList HcList</returns>
        public IList<BCW.Model.Game.HcList> GetHcLists(int SizeNum, string strWhere)
        {
            return dal.GetHcLists(SizeNum, strWhere);
        }


		/// <summary>
		/// ȡ��ÿҳ��¼
		/// </summary>
		/// <param name="p_pageIndex">��ǰҳ</param>
		/// <param name="p_pageSize">��ҳ��С</param>
		/// <param name="p_recordCount">�����ܼ�¼��</param>
		/// <param name="strWhere">��ѯ����</param>
		/// <returns>IList HcList</returns>
		public IList<BCW.Model.Game.HcList> GetHcLists(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetHcLists(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

		#endregion  ��Ա����
	}
}

