using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.bydr.Model;
namespace BCW.bydr.BLL
{
	/// <summary>
	/// ҵ���߼���Cmg_notes ��ժҪ˵����
	/// </summary>
	public class Cmg_notes
	{
		private readonly BCW.bydr.DAL.Cmg_notes dal=new BCW.bydr.DAL.Cmg_notes();
		public Cmg_notes()
		{}
		#region  ��Ա����

		/// <summary>
		/// �õ����ID
		/// </summary>
		public int GetMaxId()
		{
			return dal.GetMaxId();
		}
        public int GetMaxID(int usID)
        {
            return dal.GetMaxID(usID);
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
        public bool Exists1(int meid)
        {
            return dal.Exists1(meid);
        }
        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool Exists2(int n, int usid, string coID)
        {
            return dal.Exists2(n, usid, coID);
        }

		/// <summary>
		/// ����һ������
		/// </summary>
		public int  Add(BCW.bydr.Model.Cmg_notes model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Update(BCW.bydr.Model.Cmg_notes model)
		{
			dal.Update(model);
		}
        /// <summary>
        /// ����һ������
        /// </summary>
        public void Updatestype(int ID ,int stype)
        {
            dal.Updatestype(ID,stype);
        }
        /// <summary>
        /// ����һ������
        /// </summary>
        public void updateAllGold(int ID,long AllGold)
        {
            dal.updateAllGold(ID, AllGold);
        }
        /// <summary>
        /// ����һ������
        /// </summary>
        public void updateAllGold1(int ID, long AllGold)
        {
            dal.updateAllGold1(ID, AllGold);
        }
         /// <summary>
        /// ͨ��id��������
        /// </summary>
        public void UpdateVit(int ID, int Vit)
        {
            dal.UpdateVit(ID, Vit);
        }
         /// <summary>
        /// ����ǩ��ʱ��
        /// </summary>
        public void UpdateSigntime(int ID, DateTime Signtime)
        {
            dal.UpdateSigntime(ID,Signtime);
        }
		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public void Delete(int ID)
		{
			
			dal.Delete(ID);
		}
        /// <summary>
        /// ɾ��һ��ʱ��ε�����
        /// </summary>
        public void Delete1(string stime,string otime)
        {

            dal.Delete1(stime,otime);
        }
		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public BCW.bydr.Model.Cmg_notes GetCmg_notes(int ID)
		{
			
			return dal.GetCmg_notes(ID);
		}
        /// <summary>
        /// �õ����usID
        /// </summary>
        public BCW.bydr.Model.Cmg_notes GetusID(int usID)
        {
            return dal.GetusID(usID);
        }
        /// <summary>
        /// ͨ���ֶεõ�id
        /// </summary>
        public int GetID1(int usID, int stype)
        {
            return dal.GetID1(usID, stype);
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
		/// <returns>IList Cmg_notes</returns>
		public IList<BCW.bydr.Model.Cmg_notes> GetCmg_notess(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetCmg_notess(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

		#endregion  ��Ա����
	}
}

