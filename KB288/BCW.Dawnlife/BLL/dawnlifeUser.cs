using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
	/// <summary>
	/// ҵ���߼���dawnlifeUser ��ժҪ˵����
	/// </summary>
	public class dawnlifeUser
	{
		private readonly BCW.DAL.dawnlifeUser dal=new BCW.DAL.dawnlifeUser();
		public dawnlifeUser()
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
		/// ����һ������
		/// </summary>
		public int  Add(BCW.Model.dawnlifeUser model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Update(BCW.Model.dawnlifeUser model)
		{
			dal.Update(model);
		}
        //����һ���ֶ�
        public void Updatedebt(int ID, long debt)
        {
            dal.Updatedebt(ID, debt);
        }
        public void Updatemoney(int ID, long money)
        {
            dal.Updatemoney(ID, money);
        }
        public void Updatecoin(int ID, long coin)
        {
            dal.Updatemoney(ID, coin);
        }
        public void UpdateStock(int ID, string stock)
        {
            dal.UpdateStock(ID, stock);
        }
        public void UpdateStorehouse(int ID, string storehouse)
        {
            dal.UpdateStorehouse(ID, storehouse);
        }
        public void Updatehealth(int ID, int health)
        {
            dal.Updatehealth(ID, health);
        }
        public void Updatereputation(int ID, int reputation)
        {
            dal.Updatereputation(ID, reputation);
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
		public BCW.Model.dawnlifeUser GetdawnlifeUser(int ID)
		{
			
			return dal.GetdawnlifeUser(ID);
		}


        /// ���ݲ�ѯӰ�������
        /// </summary>
        public int GetRowByUsID(int UsID)
        {
            return dal.GetRowByUsID(UsID);
        }
        /// <summary>
        /// me_��ʼ��ĳ���ݱ�
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
		/// <returns>IList dawnlifeUser</returns>
		public IList<BCW.Model.dawnlifeUser> GetdawnlifeUsers(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetdawnlifeUsers(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

		#endregion  ��Ա����
	}
}

