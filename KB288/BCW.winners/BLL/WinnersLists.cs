using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
	/// <summary>
	/// ҵ���߼���tb_WinnersLists ��ժҪ˵����
	/// </summary>
	public class tb_WinnersLists
	{
		private readonly BCW.DAL.tb_WinnersLists dal=new BCW.DAL.tb_WinnersLists();
		public tb_WinnersLists()
		{}
		#region  ��Ա����
		/// <summary>
		/// �Ƿ���ڸü�¼
		/// </summary>
		public bool Exists(long Id)
		{
			return dal.Exists(Id);
		}
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
        public bool ExistsUserID(int ID)
        {
            return dal.ExistsUserID(ID);
        }
		/// <summary>
		/// ����һ������
		/// </summary>
		public int  Add(BCW.Model.tb_WinnersLists model)
		{
			return dal.Add(model);
		}
        /// <summary>
        /// �õ�ÿ��ÿ��񽱴���
        /// </summary>
        public int GetMaxCounts(int UserID)
        {
            return dal.GetMaxCounts(UserID);
        }
        /// <summary>
        /// �õ����û��ϴ����ݵĴ�����isGet���ֶε�һ��
        /// </summary>
        public BCW.Model.tb_WinnersLists GetLastIsGet(int Id)
        {
            return dal.GetLastIsGet(Id);
        }
		/// <summary>
		/// ����һ������
		/// </summary>
		public void Update(BCW.Model.tb_WinnersLists model)
		{
			dal.Update(model);
		}
        /// <summary>
		/// ����һ������
		/// </summary>
        public void UpdateIdent(int ID,int Ident)
		{
            dal.UpdateIdent(ID,Ident);
		}
        

		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public void Delete(long Id)
		{
			
			dal.Delete(Id);
		}

		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public BCW.Model.tb_WinnersLists Gettb_WinnersLists(long Id)
		{
			
			return dal.Gettb_WinnersLists(Id);
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
		/// <returns>IList tb_WinnersLists</returns>
		public IList<BCW.Model.tb_WinnersLists> Gettb_WinnersListss(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.Gettb_WinnersListss(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

		#endregion  ��Ա����
	}
}

