using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Baccarat.Model;
namespace BCW.Baccarat.BLL
{
	/// <summary>
	/// ҵ���߼���BaccaratUser ��ժҪ˵����
	/// </summary>
	public class BaccaratUser
	{
		private readonly BCW.Baccarat.DAL.BaccaratUser dal=new BCW.Baccarat.DAL.BaccaratUser();
		public BaccaratUser()
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
        /// �ж��Ƿ���ڸ��û���¼
        /// </summary>
        /// <param name="UsID"></param>
        /// <returns></returns>
        public bool ExistsUser(int UsID)
        {
            return dal.ExistsUser(UsID);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public int Add(BCW.Baccarat.Model.BaccaratUser model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public bool Update(BCW.Baccarat.Model.BaccaratUser model)
        {
            return dal.Update(model);
        }

        /// <summary>
        /// �����û���ׯȨ��
        /// </summary>
        /// <param name="UsID"></param>
        /// <param name="RoomTimes"></param>
        /// <returns></returns>
        public bool UpdateTimes(int UsID, int RoomTimes)
        {
            return dal.UpdateTimes(UsID, RoomTimes);
        }

        /// <summary>
        /// �����û���������
        /// </summary>
        /// <param name="UsID"></param>
        /// <param name="SetID"></param>
        /// <returns></returns>
        public bool UpdateSet(int UsID, int SetID)
        {
            return dal.UpdateSet(UsID, SetID);
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public bool Delete(int ID)
        {

            return dal.Delete(ID);
        }

        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public BCW.Baccarat.Model.BaccaratUser GetModel(int ID)
        {

            return dal.GetModel(ID);
        }

        /// <summary>
        /// ��ȡ�û�Ȩ����Ϣ
        /// </summary>
        public BCW.Baccarat.Model.BaccaratUser GetUser(int UsID)
        {
            return dal.GetUser(UsID);
        }

        /// <summary>
		/// ��������б�
		/// </summary>
		public DataSet GetList(string strWhere)
        {
            return dal.GetList(strWhere);
        }
        /// <summary>
        /// ���ǰ��������
        /// </summary>
        public DataSet GetList(int Top, string strWhere, string filedOrder)
        {
            return dal.GetList(Top, strWhere, filedOrder);
        }
        /// <summary>
        /// ��������б�
        /// </summary>
        public List<BCW.Baccarat.Model.BaccaratUser> GetModelList(string strWhere)
        {
            DataSet ds = dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        /// ��������б�
        /// </summary>
        public List<BCW.Baccarat.Model.BaccaratUser> DataTableToList(DataTable dt)
        {
            List<BCW.Baccarat.Model.BaccaratUser> modelList = new List<BCW.Baccarat.Model.BaccaratUser>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                BCW.Baccarat.Model.BaccaratUser model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = dal.DataRowToModel(dt.Rows[n]);
                    if (model != null)
                    {
                        modelList.Add(model);
                    }
                }
            }
            return modelList;
        }

        /// <summary>
        /// ��������б�
        /// </summary>
        public DataSet GetAllList()
        {
            return GetList("");
        }

        /// <summary>
        /// ��ҳ��ȡ�����б�
        /// </summary>
        public int GetRecordCount(string strWhere)
        {
            return dal.GetRecordCount(strWhere);
        }
        /// <summary>
        /// ��ҳ��ȡ�����б�
        /// </summary>
        public DataSet GetListByPage(string strWhere, string orderby, int startIndex, int endIndex)
        {
            return dal.GetListByPage(strWhere, orderby, startIndex, endIndex);
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
		/// <returns>IList BaccaratUser</returns>
		public IList<BCW.Baccarat.Model.BaccaratUser> GetBaccaratUsers(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetBaccaratUsers(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

		#endregion  ��Ա����
	}
}

