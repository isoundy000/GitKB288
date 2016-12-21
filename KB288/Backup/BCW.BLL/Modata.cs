using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
	/// <summary>
	/// ҵ���߼���Modata ��ժҪ˵����
	/// </summary>
	public class Modata
	{
		private readonly BCW.DAL.Modata dal=new BCW.DAL.Modata();
		public Modata()
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
        /// �õ����Types
        /// </summary>
        public int GetMaxTypes()
        {
            return dal.GetMaxTypes();
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
        public bool Exists2(int Types)
        {
            return dal.Exists2(Types);
        }

        /// <summary>
        /// �õ��ܼ�¼��
        /// </summary>
        public int GetCount()
        {
            return dal.GetCount();
        }

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Add(BCW.Model.Modata model)
		{
			dal.Add(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Update(BCW.Model.Modata model)
		{
			dal.Update(model);
		}
               
        /// <summary>
        /// ����ѡ������
        /// </summary>
        public void UpdatePhoneClick(int ID)
        {
            dal.UpdatePhoneClick(ID);
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
		public BCW.Model.Modata GetModata(int ID)
		{
			
			return dal.GetModata(ID);
		}

        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public BCW.Model.Modata GetModata2(string Model)
        {

            return dal.GetModata2(Model);
        }
     
        /// <summary>
        /// �õ�һ��Types
        /// </summary>
        public int GetTypes(string Brand)
        {
            return dal.GetTypes(Brand);
        }
               
        /// <summary>
        /// ����Types�õ�һ��Brand
        /// </summary>
        public string GetPhoneBrand(int Types)
        {
            return dal.GetPhoneBrand(Types);
        }

        /// <summary>
        /// ����Types�õ�һ��Model
        /// </summary>
        public string GetPhoneModel(int Types)
        {
            return dal.GetPhoneModel(Types);
        }

        /// <summary>
        /// �����ֶ�ȡ�����б�
        /// </summary>
        public DataSet GetList(string strField, string strWhere)
        {
            return dal.GetList(strField, strWhere);
        }

        /// <summary>
        /// ȡ��Ʒ�Ƽ�¼
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">��ҳ��С</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <returns>IList Modata</returns>
        public IList<BCW.Model.Modata> GetBrand(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetBrand(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }

		/// <summary>
		/// ȡ��ÿҳ��¼
		/// </summary>
		/// <param name="p_pageIndex">��ǰҳ</param>
		/// <param name="p_pageSize">��ҳ��С</param>
		/// <param name="p_recordCount">�����ܼ�¼��</param>
		/// <param name="strWhere">��ѯ����</param>
		/// <returns>IList Modata</returns>
		public IList<BCW.Model.Modata> GetModatas(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetModatas(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

		#endregion  ��Ա����
	}
}

