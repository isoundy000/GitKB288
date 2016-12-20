using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Baccarat.Model;
namespace BCW.Baccarat.BLL
{
	/// <summary>
	/// ҵ���߼���BaccaratRoomDo ��ժҪ˵����
	/// </summary>
	public class BaccaratRoomDo
	{
		private readonly BCW.Baccarat.DAL.BaccaratRoomDo dal=new BCW.Baccarat.DAL.BaccaratRoomDo();
		public BaccaratRoomDo()
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
		public bool Exists(int RoomDoID)
		{
			return dal.Exists(RoomDoID);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public int  Add(BCW.Baccarat.Model.BaccaratRoomDo model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Update(BCW.Baccarat.Model.BaccaratRoomDo model)
		{
			dal.Update(model);
		}

		/// <summary>
		/// ɾ��һ������
		/// </summary>
        public bool Delete(int RoomDoID)
		{
			
			return dal.Delete(RoomDoID);
		}

        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public BCW.Baccarat.Model.BaccaratRoomDo GetModel(int RoomDoID)
        {

            return dal.GetModel(RoomDoID);
        }

        /// <summary>
        /// ��ȡ�ض�����Ķ���ʵ��
        /// </summary>
        public BCW.Baccarat.Model.BaccaratRoomDo GetBaccaratRoom(int RoomID)
        {
            return dal.GetBaccaratRoom(RoomID);
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
        public List<BCW.Baccarat.Model.BaccaratRoomDo> GetModelList(string strWhere)
        {
            DataSet ds = dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        /// ��������б�
        /// </summary>
        public List<BCW.Baccarat.Model.BaccaratRoomDo> DataTableToList(DataTable dt)
        {
            List<BCW.Baccarat.Model.BaccaratRoomDo> modelList = new List<BCW.Baccarat.Model.BaccaratRoomDo>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                BCW.Baccarat.Model.BaccaratRoomDo model;
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
		/// <returns>IList BaccaratRoomDo</returns>
		public IList<BCW.Baccarat.Model.BaccaratRoomDo> GetBaccaratRoomDos(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
		{
			return dal.GetBaccaratRoomDos(p_pageIndex, p_pageSize, strWhere, strOrder, out p_recordCount);
		}

		#endregion  ��Ա����
	}
}

