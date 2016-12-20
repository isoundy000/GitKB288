using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Baccarat.Model;
namespace BCW.Baccarat.BLL
{
	/// <summary>
	/// ҵ���߼���BaccaratTop ��ժҪ˵����
	/// </summary>
	public class BaccaratTop
	{
		private readonly BCW.Baccarat.DAL.BaccaratTop dal=new BCW.Baccarat.DAL.BaccaratTop();
		public BaccaratTop()
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
        /// ������¼
        /// </summary>
        /// <param name="TableName"></param>
        public void ClearTable(string TableName)
        {
            dal.ClearTable(TableName);
        }

		/// <summary>
		/// �Ƿ���ڸü�¼
		/// </summary>
		public bool Exists(int TopID)
		{
			return dal.Exists(TopID);
		}

        /// <summary>
        /// �Ƿ����ĳ�û�������
        /// </summary>
        public bool ExistsUser(int UsID)
        {
            return dal.ExistsUser(UsID);
        }

		/// <summary>
		/// ����һ������
		/// </summary>
		public int  Add(BCW.Baccarat.Model.BaccaratTop model)
		{
			return dal.Add(model);
		}

        /// <summary>
        /// ����һ������
        /// </summary>
        public bool Update(BCW.Baccarat.Model.BaccaratTop model)
        {
            return dal.Update(model);
        }

        /// <summary>
        /// ����ĳһ�����ĳһ�ض��ֶ�
        /// </summary>
        public void UpdateTop(int UsID, int TopBonusSum)
        {
            dal.UpdateTop(UsID, TopBonusSum);
        }

		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public void Delete(int TopID)
		{
			
			dal.Delete(TopID);
		}

        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public BCW.Baccarat.Model.BaccaratTop GetModel(int TopID)
        {

            return dal.GetModel(TopID);
        }

		/// <summary>
		/// �����ֶ�ȡ�����б�
		/// </summary>
		public DataSet GetList(string strField, string strWhere)
		{
			return dal.GetList(strField, strWhere);
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
        public List<BCW.Baccarat.Model.BaccaratTop> GetModelList(string strWhere)
        {
            DataSet ds = dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        /// ��������б�
        /// </summary>
        public List<BCW.Baccarat.Model.BaccaratTop> DataTableToList(DataTable dt)
        {
            List<BCW.Baccarat.Model.BaccaratTop> modelList = new List<BCW.Baccarat.Model.BaccaratTop>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                BCW.Baccarat.Model.BaccaratTop model;
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

        // <summary>
        /// ��ȡ���м�¼
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">��ҳ��С</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <returns></returns>
        public IList<BCW.Baccarat.Model.BaccaratTop> GetUserTop(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetUserTop(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }

		/// <summary>
		/// ȡ��ÿҳ��¼
		/// </summary>
		/// <param name="p_pageIndex">��ǰҳ</param>
		/// <param name="p_pageSize">��ҳ��С</param>
		/// <param name="p_recordCount">�����ܼ�¼��</param>
		/// <param name="strWhere">��ѯ����</param>
		/// <returns>IList BaccaratTop</returns>
		public IList<BCW.Baccarat.Model.BaccaratTop> GetBaccaratTops(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetBaccaratTops(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

		#endregion  ��Ա����
	}
}

