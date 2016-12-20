using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model.Collec;
namespace BCW.BLL.Collec
{
	/// <summary>
	/// ҵ���߼���CollecItem ��ժҪ˵����
	/// </summary>
	public class CollecItem
	{
		private readonly BCW.DAL.Collec.CollecItem dal=new BCW.DAL.Collec.CollecItem();
		public CollecItem()
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
		public int Add(BCW.Model.Collec.CollecItem model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Update(BCW.Model.Collec.CollecItem model)
		{
			dal.Update(model);
		}
        
        /// <summary>
        /// �б�����
        /// </summary>
        public void UpdateListSet(BCW.Model.Collec.CollecItem model)
        {
            dal.UpdateListSet(model);
        }
                
        /// <summary>
        /// ��������
        /// </summary>
        public void UpdateLinkSet(BCW.Model.Collec.CollecItem model)
        {
            dal.UpdateLinkSet(model);
        }
        
        /// <summary>
        /// ��������
        /// </summary>
        public void UpdateContentSet(BCW.Model.Collec.CollecItem model)
        {
            dal.UpdateContentSet(model);
        }
                
        /// <summary>
        /// ����1����/0������
        /// </summary>
        public void UpdateState(int ID, int State)
        {
            dal.UpdateState(ID, State);
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
		public BCW.Model.Collec.CollecItem GetCollecItem(int ID)
		{
			
			return dal.GetCollecItem(ID);
		}
                
        /// <summary>
        /// �õ�һ��WebEncode
        /// </summary>
        public int GetWebEncode(int ID)
        {
            return dal.GetWebEncode(ID);
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
		/// <returns>IList CollecItem</returns>
		public IList<BCW.Model.Collec.CollecItem> GetCollecItems(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetCollecItems(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

		#endregion  ��Ա����
	}
}

