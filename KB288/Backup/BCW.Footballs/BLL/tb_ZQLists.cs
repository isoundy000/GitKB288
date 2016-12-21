using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
	/// <summary>
	/// ҵ���߼���tb_ZQLists ��ժҪ˵����
	/// </summary>
	public class tb_ZQLists
	{
		private readonly BCW.DAL.tb_ZQLists dal=new BCW.DAL.tb_ZQLists();
		public tb_ZQLists()
		{}
		#region  ��Ա����
		/// <summary>
		/// �Ƿ���ڸü�¼
		/// </summary>
		public bool Exists(int Id)
		{
			return dal.Exists(Id);
		}
        /// <summary>
		/// �Ƿ���ڸ���ӱ�����¼
		/// </summary>
		public bool Exists_ft_bianhao(int ft_bianhao)
        {
            return dal.Exists_ft_bianhao(ft_bianhao);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public int  Add(BCW.Model.tb_ZQLists model)
		{
			return dal.Add(model);
		}
        /// <summary>
		/// ����Id�Ķ���+1
		/// </summary>
		public void UpdateHit(int Id)
        {
            dal.UpdateHit(Id);
        }
        /// <summary>
        /// ����һ������
        /// </summary>
        public void Update(BCW.Model.tb_ZQLists model)
		{
			dal.Update(model);
		}

		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public void Delete(int Id)
		{
			
			dal.Delete(Id);
		}
        /// <summary>
        /// ͨ����Ż�ȡId
        /// </summary>
        ///  
        public int GetIdFromBianhao(int bianhao)
        {
            return dal.GetIdFromBianhao(bianhao);
        }
        /// <summary>
        /// ͨ��Id��ȡ���
        /// </summary>
        ///  
        public int GetBianhaoFromId(int Id)
        {
            return dal.GetBianhaoFromId(Id);
        }
        /// <summary>
        /// ͨ��Id��ȡ����result
        /// </summary>
        ///  
        public string GetResultFromId(int Id)
        {
            return dal.GetResultFromId(Id);
        }
        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public BCW.Model.tb_ZQLists Gettb_ZQLists(int Id)
		{
			
			return dal.Gettb_ZQLists(Id);
		}
        /// <summary>
        /// �õ�һ������ʵ��bianhao
        /// </summary>
        public BCW.Model.tb_ZQLists Gettb_ZQListsFromBianhao(int Id)
        {

            return dal.Gettb_ZQListsFromBianhao(Id);
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
		/// <returns>IList tb_ZQLists</returns>
		public IList<BCW.Model.tb_ZQLists> Gettb_ZQListss(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.Gettb_ZQListss(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

        public IList<BCW.Model.tb_ZQLists> Gettb_ZQListss2(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
        {
            return dal.Gettb_ZQListss2(p_pageIndex, p_pageSize, strWhere, strOrder,out p_recordCount);
        }
        #endregion  ��Ա����
    }
}

