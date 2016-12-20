using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
	/// <summary>
	/// ҵ���߼���dawnlifeDo ��ժҪ˵����
	/// </summary>
	public class dawnlifeDo
	{
		private readonly BCW.DAL.dawnlifeDo dal=new BCW.DAL.dawnlifeDo();
		public dawnlifeDo()
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
		public int  Add(BCW.Model.dawnlifeDo model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Update(BCW.Model.dawnlifeDo model)
		{
			dal.Update(model);
		}
        public void UpdateStock(int ID, int stock)
        {
            dal.UpdateStock(ID, stock);
        }
        /// ���ݲ�ѯӰ�������
        /// </summary>
        public int GetRowByUsID(int UsID, long coin)
        {
            return dal.GetRowByUsID(UsID, coin);
        }
        /// ���ݲ�ѯӰ�������
        /// </summary>
        public int GetByUsID(int UsID)
        {
            return dal.GetByUsID(UsID);
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
		public BCW.Model.dawnlifeDo GetdawnlifeDo(int ID)
		{
			
			return dal.GetdawnlifeDo(ID);
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
		/// <returns>IList dawnlifeDo</returns>
		public IList<BCW.Model.dawnlifeDo> GetdawnlifeDos(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetdawnlifeDos(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

		#endregion  ��Ա����
	}
}

