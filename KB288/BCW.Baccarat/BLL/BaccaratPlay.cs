using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Baccarat.Model;
namespace BCW.Baccarat.BLL
{
	/// <summary>
	/// ҵ���߼���BaccaratPlay ��ժҪ˵����
	/// </summary>
	public class BaccaratPlay
	{
		private readonly BCW.Baccarat.DAL.BaccaratPlay dal=new BCW.Baccarat.DAL.BaccaratPlay();
		public BaccaratPlay()
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
		public bool Exists(int ID)
		{
			return dal.Exists(ID);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Add(BCW.Baccarat.Model.BaccaratPlay model)
		{
			dal.Add(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Update(BCW.Baccarat.Model.BaccaratPlay model)
		{
			dal.Update(model);
		}

		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public void Delete(int ID)
		{
			
			dal.Delete(ID);
		}

        /// <summary>
        /// ���ص�ǰ�û�û�����ķ���ĸ���
        /// </summary>
        /// <param name="UsID"></param>
        /// <returns></returns>
        public BCW.Baccarat.Model.BaccaratPlay Times(int UsID)
        {
            return dal.Times(UsID);
        }

        /// <summary>
        /// ���ع̶��û�ID������
        /// </summary>
        /// <param name="UsID"></param>
        /// <returns></returns>
        public BCW.Baccarat.Model.BaccaratPlay GetOwnMessage(int UsID)
        {
            return dal.GetOwnMessage(UsID);
        }

        /// <summary>
        /// ����ĳ����Ĳʳص��ʽ�
        /// </summary>
        public void UpdateTotal(int RoomID, int RoomDoTotal)
        {
            dal.UpdateTotal(RoomID, RoomDoTotal);
        }

        /// <summary>
        /// ���������ע
        /// </summary>
        /// <param name="RoomID"></param>
        /// <param name="RoomDoHigh"></param>
        public void UpadteHigh(int RoomID, int RoomDoHigh)
        {
            dal.UpadteHigh(RoomID, RoomDoHigh);
        }

        /// <summary>
        /// ���������ע
        /// </summary>
        /// <param name="RoomID"></param>
        /// <param name="RoomDoLow"></param>
        public void UpdateLow(int RoomID, int RoomDoLow)
        {
            dal.UpdateLow(RoomID, RoomDoLow);
        }

        /// <summary>
        /// ֱ�ӷ�ׯ
        /// </summary>
        /// <param name="RoomID"></param>
        /// <param name="RoomDoTable"></param>
        /// <param name="ActID"></param>
        public void UpdateRoom(int RoomID, int RoomDoTable, int ActID)
        {
            dal.UpdateRoom(RoomID, RoomDoTable, ActID);
        }

        /// <summary>
        /// ���¹���
        /// </summary>
        /// <param name="RoomID"></param>
        /// <param name="title"></param>
        /// <param name="announces"></param>
        public void updateannounce(int RoomID, string title, string announces)
        {
            dal.updateannounce(RoomID, title, announces);
        }

        /// <summary>
        /// ���·�����������Ϣ
        /// </summary>
        /// <param name="roomid"></param>
        /// <param name="table"></param>
        /// <param name="actid"></param>
        public void updateActID(int roomid, int table, int actid)
        {
            dal.updateActID(roomid, table, actid);
        }

		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
        public BCW.Baccarat.Model.BaccaratPlay GetModel(int ID)
		{
			
			return dal.GetModel(ID);
		}

        /// <summary>
        /// ��ȡ�ض�����Ķ���ʵ��
        /// </summary>
        public BCW.Baccarat.Model.BaccaratPlay GetPlay(int RoomID)
        {
            return dal.GetPlay(RoomID);
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
        public List<BCW.Baccarat.Model.BaccaratPlay> GetModelList(string strWhere)
        {
            DataSet ds = dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }

        /// <summary>
        /// ��������б�
        /// </summary>
        /// <param name="strWhere"></param>
        /// <param name="strOrder"></param>
        /// <returns></returns>
        public List<BCW.Baccarat.Model.BaccaratPlay> GetPlayList(string strWhere, string strOrder)
        {
            DataSet ds = dal.GetPlayList(strWhere, strOrder);
            return DataTableToList(ds.Tables[0]);
        }
        
        /// <summary>
        /// ��������б�
        /// </summary>
        public List<BCW.Baccarat.Model.BaccaratPlay> DataTableToList(DataTable dt)
        {
            List<BCW.Baccarat.Model.BaccaratPlay> modelList = new List<BCW.Baccarat.Model.BaccaratPlay>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                BCW.Baccarat.Model.BaccaratPlay model;
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
		/// ȡ��ÿҳ��¼
		/// </summary>
		/// <param name="p_pageIndex">��ǰҳ</param>
		/// <param name="p_pageSize">��ҳ��С</param>
		/// <param name="p_recordCount">�����ܼ�¼��</param>
		/// <param name="strWhere">��ѯ����</param>
		/// <returns>IList BaccaratPlay</returns>
		public IList<BCW.Baccarat.Model.BaccaratPlay> GetBaccaratPlays(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
		{
			return dal.GetBaccaratPlays(p_pageIndex, p_pageSize, strWhere, strOrder, out p_recordCount);
		}

		#endregion  ��Ա����
	}
}

