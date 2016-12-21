using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.JS.Model;
namespace BCW.JS.BLL
{
	/// <summary>
	/// ҵ���߼���bossrobot ��ժҪ˵����
	/// </summary>
	public class bossrobot
	{
		private readonly BCW.JS.DAL.bossrobot dal=new BCW.JS.DAL.bossrobot();
		public bossrobot()
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
		public int  Add(BCW.JS.Model.bossrobot model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Update(BCW.JS.Model.bossrobot model)
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
		public BCW.JS.Model.bossrobot Getbossrobot(int ID)
		{
			
			return dal.Getbossrobot(ID);
		}

		/// <summary>
		/// �����ֶ�ȡ�����б�
		/// </summary>
		public DataSet GetList(string strField, string strWhere)
		{
			return dal.GetList(strField, strWhere);
		}

        /// <summary>
        ///  me_�����ֶ��޸������б�
        /// </summary>
        public DataSet update_zd(string strField, string strWhere)
        {
            return dal.update_zd(strField, strWhere);
        }
        /// <summary>
        /// me_�õ����û�����ID���ܸ���
        /// </summary>
        public int Get_num()
        {
            return dal.Get_num();
        }
        /// <summary>
        /// me_�õ�������Ϸ���ܸ���
        /// </summary>
        public int Get_yx()
        {
            return dal.Get_yx();
        }
        /// <summary>
        /// ȡ��ÿҳ��¼
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">��ҳ��С</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <returns>IList bossrobot</returns>
        public IList<BCW.JS.Model.bossrobot> Getbossrobots(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.Getbossrobots(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

		#endregion  ��Ա����
	}
}

