using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
	/// <summary>
	/// ҵ���߼���tb_numsManage ��ժҪ˵����
	/// </summary>
	public class numsManage
	{
		private readonly BCW.DAL.numsManage dal =new BCW.DAL.numsManage();
		public numsManage()
		{}
		#region  ��Ա����
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
        public bool ExistsByUsID(int UsID)
        {
            return dal.ExistsByUsID(UsID);
        }
        /// <summary>
        /// ����һ������
        /// </summary>
        public int  Add(BCW.Model.numsManage model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Update(BCW.Model.numsManage model)
		{
			dal.Update(model);
		}
        /// <summary>
        /// ����һ������
        /// </summary>
        public void UpdateByUI(BCW.Model.numsManage model)
        {
            dal.UpdateByUI(model);
        }
        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public void Delete(int ID)
		{
			
			dal.Delete(ID);
		}
        /// <summary>
        /// ���¹�������
        /// </summary>
        public void UpdatePwd(int UsID, string Pwd)
        {
            dal.UpdatePwd(UsID,Pwd);
        }
        /// <summary>
        /// ��������ʱ��
        /// </summary>
        public void UpdateTime(int ID)
        {
            dal.UpdateTime(ID);
        }
        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public BCW.Model.numsManage GetByUsID(int UsID)
        {
            return dal.GetByUsID(UsID);
        }
        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public BCW.Model.numsManage Gettb_numsManage(int ID)
		{
			
			return dal.Gettb_numsManage(ID);
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
		/// <returns>IList tb_numsManage</returns>
		public IList<BCW.Model.numsManage> Gettb_numsManages(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.Gettb_numsManages(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

		#endregion  ��Ա����
	}
}

