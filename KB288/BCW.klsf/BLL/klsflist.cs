using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
	/// <summary>
	/// ҵ���߼���klsflist ��ժҪ˵����
	/// </summary>
	public class klsflist
	{
		private readonly BCW.DAL.klsflist dal=new BCW.DAL.klsflist();
		public klsflist()
		{}
		#region  ��Ա����
        // <summary>
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
        /// �Ƿ���ڸ���
        /// </summary>
        public bool ExistsklsfId(int klsfId)
        {
            return dal.ExistsklsfId(klsfId);
        }

        /// <summary>
        /// �Ƿ����Ҫ���½���ļ�¼
        /// </summary>
        public bool ExistsUpdateResult()
        {
            return dal.ExistsUpdateResult();
        }

		/// <summary>
		/// ����һ������
		/// </summary>
		public int  Add(BCW.Model.klsflist model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Update(BCW.Model.klsflist model)
		{
			dal.Update(model);
		}

        /// <summary>
        /// ���¿�������
        /// </summary>
        public void UpdateResult(string klsfId, string Result)
        {
            dal.UpdateResult(klsfId, Result);
        }
        /// <summary>
        /// me_��ʼ��ĳ���ݱ�
        /// </summary>
        /// <param name="TableName">���ݱ�����</param>
        public void ClearTable(string TableName)
        {
            dal.ClearTable(TableName);
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
		public BCW.Model.klsflist Getklsflist(int ID)
		{
			
			return dal.Getklsflist(ID);
		}

        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public BCW.Model.klsflist Getklsflistbyklsfid(int klsfId)
        {

            return dal.Getklsflistbyklsfid(klsfId);
        }
        /// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
        public BCW.Model.klsflist GetklsflistLast()
        {
            return dal.GetklsflistLast();
        }

        /// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
        public BCW.Model.klsflist GetklsflistLast2()
        {
            return dal.GetklsflistLast2();
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
		/// <param name="p_pageIndex">��ǰҳ</param>
		/// <param name="p_pageSize">��ҳ��С</param>
		/// <param name="p_recordCount">�����ܼ�¼��</param>
		/// <param name="strWhere">��ѯ����</param>
		/// <returns>IList klsflist</returns>
		public IList<BCW.Model.klsflist> Getklsflists(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.Getklsflists(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

        /// <summary>
		/// ȡ�ü�¼
		/// </summary>
        /// <param name="SizeNum">���ؼ�¼��</param>
		/// <param name="strWhere">��ѯ����</param>
		/// <returns>IList klsflist</returns>
        public IList<BCW.Model.klsflist> Getklsflists(int SizeNum, string strWhere)
        {
            return dal.Getklsflists(SizeNum, strWhere);
        }

		#endregion  ��Ա����
	}
}

