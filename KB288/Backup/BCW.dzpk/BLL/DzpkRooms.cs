using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.dzpk.Model;
namespace BCW.dzpk.BLL
{
	/// <summary>
	/// ҵ���߼���DzpkRooms ��ժҪ˵����
	/// </summary>
	public class DzpkRooms
	{
		private readonly BCW.dzpk.DAL.DzpkRooms dal=new BCW.dzpk.DAL.DzpkRooms();
		public DzpkRooms()
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
        /// ������Ϸ��ʶ��÷�����
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        public BCW.dzpk.Model.DzpkRooms GetRoom(string r)
        {
            return dal.GetRoom(r);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public int  Add(BCW.dzpk.Model.DzpkRooms model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Update(BCW.dzpk.Model.DzpkRooms model)
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
		/// �õ�һ������ʵ��
		/// </summary>
		public BCW.dzpk.Model.DzpkRooms GetDzpkRooms(int ID)
		{
			return dal.GetDzpkRooms(ID);
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
		/// <returns>IList DzpkRooms</returns>
		public IList<BCW.dzpk.Model.DzpkRooms> GetDzpkRoomss(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetDzpkRoomss(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

		#endregion  ��Ա����
	}
}

