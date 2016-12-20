using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Baccarat.Model;
namespace BCW.Baccarat.BLL
{
	/// <summary>
	/// ҵ���߼���BaccaratHistory ��ժҪ˵����
	/// </summary>
	public class BaccaratHistory
	{
		private readonly BCW.Baccarat.DAL.BaccaratHistory dal=new BCW.Baccarat.DAL.BaccaratHistory();
		public BaccaratHistory()
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
		public bool Exists(int HID)
		{
			return dal.Exists(HID);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public int Add(BCW.Baccarat.Model.BaccaratHistory model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Update(BCW.Baccarat.Model.BaccaratHistory model)
		{
			dal.Update(model);
		}

		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public void Delete(int HID)
		{
			
			dal.Delete(HID);
		}

        /// <summary>
        /// ����һ����������Ϣ
        /// </summary>
        /// <param name="RoomID"></param>
        /// <param name="RoomTable"></param>
        /// <param name="UsID"></param>
        /// <param name="BetType"></param>
        /// <returns></returns>
        public BCW.Baccarat.Model.BaccaratHistory GetHistory(int RoomID, int RoomTable, int UsID, string BetType)
        {
            return dal.GetHistory(RoomID, RoomTable, UsID, BetType);
        }

        /// <summary>
        /// �õ��ض�����ID������table����ɵ���עʱ��
        /// </summary>
        /// <param name="RoomID"></param>
        /// <param name="RoomDoTable"></param>
        /// <returns></returns>
        public DateTime GetMinTime(int RoomID, int RoomDoTable)
        {
            return dal.GetMinTime(RoomID, RoomDoTable);
        }

        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public BCW.Baccarat.Model.BaccaratHistory GetModel(int HID)
        {

            return dal.GetModel(HID);
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
        public List<BCW.Baccarat.Model.BaccaratHistory> GetModelList(string strWhere)
        {
            DataSet ds = dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        /// ��������б�
        /// </summary>
        public List<BCW.Baccarat.Model.BaccaratHistory> DataTableToList(DataTable dt)
        {
            List<BCW.Baccarat.Model.BaccaratHistory> modelList = new List<BCW.Baccarat.Model.BaccaratHistory>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                BCW.Baccarat.Model.BaccaratHistory model;
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
		/// <returns>IList BaccaratHistory</returns>
		public IList<BCW.Baccarat.Model.BaccaratHistory> GetBaccaratHistorys(int p_pageIndex, int p_pageSize, string strWhere,string strOrder, out int p_recordCount)
		{
            return dal.GetBaccaratHistorys(p_pageIndex, p_pageSize, strWhere, strOrder, out p_recordCount);
		}

		#endregion  ��Ա����
	}
}

