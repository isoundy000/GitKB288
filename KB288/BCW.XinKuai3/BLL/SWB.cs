using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.XinKuai3.Model;
namespace BCW.XinKuai3.BLL
{
	/// <summary>
	/// ҵ���߼���SWB ��ժҪ˵����
	/// </summary>
	public class SWB
	{
		private readonly BCW.XinKuai3.DAL.SWB dal=new BCW.XinKuai3.DAL.SWB();
		public SWB()
		{}
		#region  ��Ա����

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Add(BCW.XinKuai3.Model.SWB model)
		{
			dal.Add(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Update(BCW.XinKuai3.Model.SWB model)
		{
			dal.Update(model);
		}

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public void Delete(int UserID)
        {

            dal.Delete(UserID);
        }

        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public BCW.XinKuai3.Model.SWB GetSWB(int UserID)
        {

            return dal.GetSWB(UserID);
        }

		/// <summary>
		/// �����ֶ�ȡ�����б�
		/// </summary>
		public DataSet GetList(string strField, string strWhere)
		{
			return dal.GetList(strField, strWhere);
		}


        //=========================================
        /// <summary>
        /// me_�õ��û���
        /// </summary>
        public long GetGold(int UserID)
        {
            return dal.GetGold(UserID);
        }
        /// <summary>
        /// me_�Ƿ���ڸü�¼
        /// </summary>
        public bool Exists(int UserID)
        {
            return dal.Exists(UserID);
        }
        /// <summary>
        /// me_�����û������/�������Ѽ�¼
        /// </summary>
        /// <param name="ID">�û�ID</param>
        /// <param name="iGold">������</param>
        public void UpdateiGold(int UserID, long iGold)
        {
            dal.UpdateiGold(UserID, iGold);
        }
        /// <summary>
        /// me_�����û�������ȡ��ʱ��
        /// </summary>
        /// <param name="ID">�û�ID</param>
        /// <param name="cishu">����</param>
        public void Updatecishu(int UserID)
        {
            dal.Updatecishu(UserID);
        }
        /// <summary>
        /// me_����һ������
        /// </summary>
        public void Add_num(BCW.XinKuai3.Model.SWB model)
        {
            dal.Add_num(model);
        }

        //=========================================


		/// <summary>
		/// ȡ��ÿҳ��¼
		/// </summary>
		/// <param name="p_pageIndex">��ǰҳ</param>
		/// <param name="p_pageSize">��ҳ��С</param>
		/// <param name="p_recordCount">�����ܼ�¼��</param>
		/// <param name="strWhere">��ѯ����</param>
		/// <returns>IList SWB</returns>
		public IList<BCW.XinKuai3.Model.SWB> GetSWBs(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetSWBs(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

		#endregion  ��Ա����
	}
}

