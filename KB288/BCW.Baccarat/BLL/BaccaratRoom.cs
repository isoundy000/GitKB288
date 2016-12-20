using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Baccarat.Model;
namespace BCW.Baccarat.BLL
{
	/// <summary>
	/// ҵ���߼���BaccaratRoom ��ժҪ˵����
	/// </summary>
	public class BaccaratRoom
	{
		private readonly BCW.Baccarat.DAL.BaccaratRoom dal=new BCW.Baccarat.DAL.BaccaratRoom();
		public BaccaratRoom()
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
		public bool Exists(int RoomID)
		{
			return dal.Exists(RoomID);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public int  Add(BCW.Baccarat.Model.BaccaratRoom model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Update(BCW.Baccarat.Model.BaccaratRoom model)
		{
			dal.Update(model);
		}

		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public void Delete(int RoomID)
		{
			
			dal.Delete(RoomID);
		}

		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public BCW.Baccarat.Model.BaccaratRoom GetBaccaratRoom(int RoomID)
		{
			
			return dal.GetBaccaratRoom(RoomID);
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
		/// <returns>IList BaccaratRoom</returns>
        public IList<BCW.Baccarat.Model.BaccaratRoom> GetBaccaratRooms(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
		{
            return dal.GetBaccaratRooms(p_pageIndex, p_pageSize, strWhere, strOrder, out p_recordCount);
		}

		#endregion  ��Ա����
	}
}

