using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
	/// <summary>
	/// ҵ���߼���tb_Help ��ժҪ˵����
	/// </summary>
	public class tb_Help
	{
		private readonly BCW.DAL.tb_Help dal=new BCW.DAL.tb_Help();
		public tb_Help()
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
		public void Add(BCW.Model.tb_Help model)
		{
			dal.Add(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Update(BCW.Model.tb_Help model)
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
		public BCW.Model.tb_Help Gettb_Help(int ID)
		{
			
			return dal.Gettb_Help(ID);
		}

		/// <summary>
		/// �����ֶ�ȡ�����б�
		/// </summary>
		public DataSet GetList(string strField, string strWhere)
		{
			return dal.GetList(strField, strWhere);
		}
         /// <summary>
        /// ȡ�ù̶��б��¼
        /// </summary>
        /// <param name="SizeNum">�б��¼��</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <returns>IList Brag</returns>
        public IList<BCW.Model.tb_Help> GetHelps(int SizeNum, string strWhere)
        {
            return dal.GetHelps(SizeNum, strWhere);
        }
		/// <summary>
		/// ȡ��ÿҳ��¼
		/// </summary>
		/// <param name="p_pageIndex">��ǰҳ</param>
		/// <param name="p_pageSize">��ҳ��С</param>
		/// <param name="p_recordCount">�����ܼ�¼��</param>
		/// <param name="strWhere">��ѯ����</param>
		/// <returns>IList tb_Help</returns>
		public IList<BCW.Model.tb_Help> Gettb_Helps(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.Gettb_Helps(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

		#endregion  ��Ա����
	}
}

