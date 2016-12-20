using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.bydr.Model;
namespace BCW.bydr.BLL
{
	/// <summary>
	/// ҵ���߼���CmgToplist ��ժҪ˵����
	/// </summary>
	public class CmgToplist
	{
		private readonly BCW.bydr.DAL.CmgToplist dal=new BCW.bydr.DAL.CmgToplist();
		public CmgToplist()
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
        public bool ExistsusID(int usID)
        {
            return dal.ExistsusID(usID);
        }
        /// <summary>
        /// �Ƿ����vipʱ��
        /// </summary>
        public bool Existsusvip(int usID,string stime)
        {
            return dal.Existsusvip(usID,stime);
        }
        /// <summary>
        /// �Ƿ���ڸ�����id
        /// </summary>
        public bool ExistsusID1(int usID,int sid)
        {
            return dal.ExistsusID1(usID,sid);
        }
		/// <summary>
		/// ����һ������
		/// </summary>
		public int  Add(BCW.bydr.Model.CmgToplist model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Update(BCW.bydr.Model.CmgToplist model)
		{
			dal.Update(model);
		}
        /// <summary>
        /// ���µȼ�
        /// </summary>
        public void Updatestype(int usID, int stype)
        {
            dal.Updatestype( usID,stype);
        }
        /// <summary>
        /// �������ռ���
        /// </summary>
        public void UpdateAllcolletGold(int usID, long AllcolletGold)
        {
            dal.UpdateAllcolletGold(usID, AllcolletGold);
        }
        /// <summary>
        /// ����ÿ���ռ���
        /// </summary>
        public void UpdateDcolletGold(int usID, long DcolletGold)
        {
            dal.UpdateDcolletGold(usID, DcolletGold);
        }
        /// <summary>
        /// ����������
        /// </summary>
        public void UpdateYcolletGold(int usID, long YcolletGold)
        {
            dal.UpdateYcolletGold(usID, YcolletGold);
        }
        /// <summary>
        /// ����sid
        /// </summary>
        public void Updatesid(int usID, int sid)
        {
            dal.Updatesid(usID, sid);
        }
        /// <summary>
        /// ����ʱ��
        /// </summary>
        public void Updatetime(int usID, DateTime updatetime)
        {
            dal.Updatetime(usID,updatetime);
        }
        /// <summary>
        /// ����ǩ��ʱ��
        /// </summary>
        public void UpdateSigntime(int usID, DateTime Signtime)
        {
            dal.UpdateSigntime(usID, Signtime);
        }
        /// <summary>
        /// ��������ֵ
        /// </summary>
        public void Updatevit(int usID, int vit)
        {
            dal.Updatevit(usID, vit);
        }
        /// <summary>
        /// ��������ֵ
        /// </summary>
        public void Updatevit1(int usID, int vit)
        {
            dal.Updatevit1(usID, vit);
        }
        /// <summary>
        /// ����sid
        /// </summary>
        public void Updatesid1( int sid)
        {
            dal.Updatesid1( sid);
        }
        /// <summary>
        /// ÿ���ռ������
        /// </summary>
        public void UpdateDcolletGold1( long DcolletGold)
        {
            dal.UpdateDcolletGold1( DcolletGold);
        }
        /// <summary>
        /// ����ÿ���ռ���
        /// </summary>
        public void UpdateMcolletGold(int usID, long McolletGold)
        {
            dal.UpdateMcolletGold(usID, McolletGold);
        }
        /// <summary>
        /// ÿ���ռ������
        /// </summary>
        public void UpdateMcolletGold1(long McolletGold)
        {
            dal.UpdateMcolletGold1( McolletGold);
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
		public BCW.bydr.Model.CmgToplist GetCmgToplist(int ID)
		{
			
			return dal.GetCmgToplist(ID);
		}
        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public BCW.bydr.Model.CmgToplist GetCmgToplistusID(int usID)
        {

            return dal.GetCmgToplistusID(usID);
        }
        /// <summary>
        /// �õ����ռ���
        /// </summary>
        public BCW.bydr.Model.CmgToplist GetCmgTopAllcolletGold(int usID)
        {

            return dal.GetCmgTopAllcolletGold(usID);
        }
        /// <summary>
        /// �õ�����ֵ
        /// </summary>
        public int Getvit(int usID)
        {

            return dal.Getvit(usID);
        }
        /// <summary>
        /// �õ�ǩ��ʱ��
        /// </summary>
        public string GetSigntime(int usID)
        {

            return dal.GetSigntime(usID);
        }
        /// <summary>
        /// �õ�ÿ���ռ���
        /// </summary>
        public long GetCmgTopDcolletGold(int usID,string time)
        {

            return dal.GetCmgTopDcolletGold(usID,time);
        }
        /// <summary>
        /// �õ�ÿ�¼���
        /// </summary>
        public long GetCmgTopMcolletGold(int usID)
        {

            return dal.GetCmgTopMcolletGold(usID);
        }
        /// <summary>
        /// �õ�id
        /// </summary>
        public int Gettoplistid(int sid)
        {

            return dal.Gettoplistid(sid);
        }
        /// <summary>
        /// �õ��ռ��ܱ�
        /// </summary>
        public long GettoplistAllcolletGoldsum()
        {
            return dal.GettoplistAllcolletGoldsum();
        }
        /// <summary>
        /// �õ������ܱ�
        /// </summary>
        public long GettoplistYcolletGoldsum()
        {
            return dal.GettoplistYcolletGoldsum();
        }
        /// <summary>
        /// �õ�ȫ�������ռ���
        /// </summary>
        public long GettoplistDcolletGoldsum()
        {
            return dal.GettoplistDcolletGoldsum();
        }
        /// <summary>
        /// �õ�ȫ�������ռ���
        /// </summary>
        public long GettoplistMcolletGoldsum()
        {
            return dal.GettoplistMcolletGoldsum();
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
		/// <returns>IList CmgToplist</returns>
		public IList<BCW.bydr.Model.CmgToplist> GetCmgToplists(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetCmgToplists(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}
        /// <summary>
        /// ȡ��ÿҳ��¼
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">��ҳ��С</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <returns>IList CmgToplist</returns>
        public IList<BCW.bydr.Model.CmgToplist> GetCmgToplists1(int p_pageIndex, int p_pageSize, string strWhere,string strOrder, out int p_recordCount)
        {
            return dal.GetCmgToplists1(p_pageIndex, p_pageSize, strWhere,strOrder, out p_recordCount);
        }

		#endregion  ��Ա����
	}
}

