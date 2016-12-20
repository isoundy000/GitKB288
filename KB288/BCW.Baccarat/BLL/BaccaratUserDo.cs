using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Baccarat.Model;
namespace BCW.Baccarat.BLL
{
	/// <summary>
	/// ҵ���߼���BaccaratUserDo ��ժҪ˵����
	/// </summary>
	public class BaccaratUserDo
	{
		private readonly BCW.Baccarat.DAL.BaccaratUserDo dal=new BCW.Baccarat.DAL.BaccaratUserDo();
		public BaccaratUserDo()
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
		public bool Exists(int DoID)
		{
			return dal.Exists(DoID);
		}

        /// <summary>
        /// ĳ�û���ĳһ������������Ƿ�����˸����͵�����
        /// </summary>
        public bool ExistsMessage(int UsID, int RoomID, int RoomDoTable, string BetTypes)
        {
            return dal.ExistsMessage(UsID, RoomID, RoomDoTable, BetTypes);
        }

        /// <summary>
        /// ��ȡ���������̨��
        /// </summary>
        /// <param name="RoomID"></param>
        /// <returns></returns>
        public BCW.Baccarat.Model.BaccaratUserDo NewRoomMessage(int RoomID)
        {
            return dal.NewRoomMessage(RoomID);
        }

        /// <summary>
        /// ��ȡ��ȷ���û������䡢�������ע���͵���������
        /// </summary>
        public BCW.Baccarat.Model.BaccaratUserDo GetBetTypesMessage(int UsID, int RoomID, int RoomDoTable, string BetTypes)
        {
            return dal.GetBetTypesMessage(UsID, RoomID, RoomDoTable, BetTypes);
        }

		/// <summary>
		/// ����һ������
		/// </summary>
		public int  Add(BCW.Baccarat.Model.BaccaratUserDo model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Update(BCW.Baccarat.Model.BaccaratUserDo model)
		{
			dal.Update(model);
		}

        /// <summary>
        /// ����ĳһ�����ĳһ�ض��ֶ�
        /// </summary>
        public DataSet UpdateBonus(string strField, string strWhere)
        {
            return dal.UpdateBonus(strField, strWhere);
        }

		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public void Delete(int DoID)
		{
			
			dal.Delete(DoID);
		}

		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public BCW.Baccarat.Model.BaccaratUserDo GetBaccaratUserDo(int DoID)
		{
			
			return dal.GetBaccaratUserDo(DoID);
		}

        /// <summary>
        /// ���ؾ�����Ϣ
        /// </summary>
        /// <param name="UsID"></param>
        /// <param name="RoomID"></param>
        /// <param name="RoomTable"></param>
        /// <param name="BetType"></param>
        /// <returns></returns>
        public BCW.Baccarat.Model.BaccaratUserDo GetUserDo(int UsID, int RoomID, int RoomTable, string BetType)
        {
            return dal.GetUserDo(UsID, RoomID, RoomTable, BetType);
        }

        /// <summary>
        /// ��ȡ�ض���������µ�����
        /// </summary>
        public BCW.Baccarat.Model.BaccaratUserDo GetRoomMessage(int RoomID)
        {
            return dal.GetRoomMessage(RoomID);
        }

        /// <summary>
        ///�õ��ض�����ID������table�����µ�����
        /// </summary>
        public BCW.Baccarat.Model.BaccaratUserDo GetRoomtableMessage(int RoomID,int RoomDoTable)
        {
            return dal.GetRoomtableMessage(RoomID, RoomDoTable);
        }

        /// <summary>
       /// ��ȡ�ض��û�ID���ض�����ID��̨��Table����������
       /// </summary>
        public BCW.Baccarat.Model.BaccaratUserDo GetUserMessage(int UsID, int RoomID, int RoomDoTable)
        {
            return dal.GetUserMessage(UsID, RoomID, RoomDoTable);
        }

        /// <summary>
        ///�õ��ض�����ID������table�����µ���עʱ��
        /// </summary>
        public DateTime GetMaxBetTime(int RoomID,int RoomDoTable,int type)
        {
            return dal.GetMaxBetTime(RoomID, RoomDoTable,type);
        }

        /// <summary>
        ///�õ��ض�����ID������table����ɵ���עʱ��
        /// </summary>
        public DateTime GetMinBetTime(int RoomID, int RoomDoTable, int type)
        {
            return dal.GetMinBetTime(RoomID, RoomDoTable,type);
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
        public List<BCW.Baccarat.Model.BaccaratUserDo> List(string strWhere)
        {
            DataSet da = dal.GetList(strWhere);
            return DataTableToList(da.Tables[0]);
            
        }

        /// <summary>
        /// ��������б�
        /// </summary>
        public List<BCW.Baccarat.Model.BaccaratUserDo> DataTableToList(DataTable dt)
        {
            List<BCW.Baccarat.Model.BaccaratUserDo> modelList = new List<BCW.Baccarat.Model.BaccaratUserDo>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                BCW.Baccarat.Model.BaccaratUserDo model;
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
		/// <returns>IList BaccaratUserDo</returns>
		public IList<BCW.Baccarat.Model.BaccaratUserDo> GetBaccaratUserDos(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetBaccaratUserDos(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}
        public List<int> GetListBang(int RoomID, int RoomDoTable)
        {
            return dal.GetListBang(RoomID, RoomDoTable);
        }
        #endregion  ��Ա����
    }
}

