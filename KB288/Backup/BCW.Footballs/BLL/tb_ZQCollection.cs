using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
	/// <summary>
	/// ҵ���߼���tb_ZQCollection ��ժҪ˵����
	/// </summary>
	public class tb_ZQCollection
	{
		private readonly BCW.DAL.tb_ZQCollection dal=new BCW.DAL.tb_ZQCollection();
		public tb_ZQCollection()
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
		/// �Ƿ���ڸü�¼
		/// </summary>
		public bool ExistsUsIdAndFootId(int UsId,int FootId)
        {
            return dal.ExistsUsIdAndFootId(UsId, FootId);
        }
        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public int CountUsIdAndFootId(int FootId)
        {
            return dal.CountUsIdAndFootId(FootId);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public void Add(BCW.Model.tb_ZQCollection model)
		{
			dal.Add(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Update(BCW.Model.tb_ZQCollection model)
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
		public BCW.Model.tb_ZQCollection Gettb_ZQCollection(int ID)
		{
			
			return dal.Gettb_ZQCollection(ID);
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
		/// <returns>IList tb_ZQCollection</returns>
		public IList<BCW.Model.tb_ZQCollection> Gettb_ZQCollections(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.Gettb_ZQCollections(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

		#endregion  ��Ա����
	}
}

