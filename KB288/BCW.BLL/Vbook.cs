using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
	/// <summary>
	/// ҵ���߼���Vbook ��ժҪ˵����
	/// </summary>
	public class Vbook
	{
		private readonly BCW.DAL.Vbook dal=new BCW.DAL.Vbook();
		public Vbook()
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
        public int Add2(BCW.Model.Vbook model)
        {
            return dal.Add2(model);
        }

		/// <summary>
		/// ����һ������
		/// </summary>
		public int  Add(BCW.Model.Vbook model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Update(BCW.Model.Vbook model)
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
		public BCW.Model.Vbook GetVbook(int ID)
		{
			
			return dal.GetVbook(ID);
		}

		/// <summary>
		/// ��������б�
		/// </summary>
		public DataSet GetList(string strWhere)
		{
			return dal.GetList(strWhere);
		}

		/// <summary>
		/// ȡ��ÿҳ��¼
		/// </summary>
		/// <param name="p_pageIndex">��ǰҳ</param>
		/// <param name="p_pageSize">��ҳ��С</param>
		/// <param name="p_recordCount">�����ܼ�¼��</param>
		/// <param name="strWhere">��ѯ����</param>
		/// <returns>IList Vbook</returns>
		public IList<BCW.Model.Vbook> GetVbooks(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetVbooks(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

		#endregion  ��Ա����
	}
}

