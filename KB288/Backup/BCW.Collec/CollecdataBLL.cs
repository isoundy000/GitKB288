using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model.Collec;
namespace BCW.BLL.Collec
{
	/// <summary>
	/// ҵ���߼���Collecdata ��ժҪ˵����
	/// </summary>
	public class Collecdata
	{
		private readonly BCW.DAL.Collec.Collecdata dal=new BCW.DAL.Collec.Collecdata();
		public Collecdata()
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
        /// ����ĳ�ɼ���Ŀ�Ĳɼ���
        /// </summary>
        public int GetCount(int ItemId)
        {
            return dal.GetCount(ItemId);
        }

		/// <summary>
		/// ����һ������
		/// </summary>
		public int  Add(BCW.Model.Collec.Collecdata model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Update(BCW.Model.Collec.Collecdata model)
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
        /// ɾ��һ������
        /// </summary>
        public void Delete(string strWhere)
        {

            dal.Delete(strWhere);
        }

		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public BCW.Model.Collec.Collecdata GetCollecdata(int ID)
		{
			
			return dal.GetCollecdata(ID);
		}
                
        /// <summary>
        /// �õ�һ��Pics
        /// </summary>
        public string GetPics(int ID)
        {

            return dal.GetPics(ID);
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
		/// <returns>IList Collecdata</returns>
		public IList<BCW.Model.Collec.Collecdata> GetCollecdatas(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetCollecdatas(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

		#endregion  ��Ա����
	}
}

