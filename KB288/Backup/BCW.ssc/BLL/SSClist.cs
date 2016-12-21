using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
namespace BCW.ssc.BLL
{
	/// <summary>
	/// ҵ���߼���SSClist ��ժҪ˵����
	/// </summary>
	public class SSClist
	{
		private readonly BCW.ssc.DAL.SSClist dal=new BCW.ssc.DAL.SSClist();
		public SSClist()
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
        /// �Ƿ���ڸ���
        /// </summary>
        public bool ExistsSSCId(int SSCId)
        {
            return dal.ExistsSSCId(SSCId);
        }

        /// <summary>
        /// �Ƿ���ڸ�����δ���¿���
        /// </summary>
        public bool ExistsSSCId(int SSCId,int k)
        {
            return dal.ExistsSSCId(SSCId,k);
        }

        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public BCW.ssc.Model.SSClist GetSSClistbySSCId(int SSCId)
        {

            return dal.GetSSClistbySSCId(SSCId);
        }

        /// <summary>
        /// �����ںŵõ�id
        /// </summary>
        /// <param name="SSCId"></param>
        /// <returns></returns>
        public int id(int SSCId)
         {
        return dal.id(SSCId);
         }

        /// <summary>
        /// �����ںŵõ��������ͺͿ���ʱ��
        /// </summary>
        /// <param name="SSCId"></param>
        /// <returns></returns>
        public string GetStateTime(int SSCId)
        {
            return dal.GetStateTime(SSCId);
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
		public int  Add(BCW.ssc.Model.SSClist model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Update(BCW.ssc.Model.SSClist model)
		{
			dal.Update(model);
		}
                
        /// <summary>
        /// ���¿������
        /// </summary>
        public void UpdateResult(int SSCId, string Result)
        {
            dal.UpdateResult(SSCId, Result);
        }

        /// <summary>
        /// ���¿������ͺͿ���ʱ��
        /// </summary>
        public void UpdateStateTime(int SSCId, string Result)
        {
            dal.UpdateStateTime(SSCId, Result);
        }

        /// <summary>
        /// ���¿������
        /// </summary>
        public void UpdateResult1(int SSCId, string Result)
        {
            dal.UpdateResult1(SSCId, Result);
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
		public BCW.ssc.Model.SSClist GetSSClist(int ID)
		{
			
			return dal.GetSSClist(ID);
		}

        
        /// <summary>
        /// �õ����һ�ڶ���ʵ��
        /// </summary>
        public BCW.ssc.Model.SSClist GetSSClistLast()
        {

            return dal.GetSSClistLast();
        }
                
        /// <summary>
        /// �õ����ڿ���
        /// </summary>
        public BCW.ssc.Model.SSClist GetSSClistLast2()
        {
            return dal.GetSSClistLast2();
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
        /// <returns>IList SSClist</returns>
        public IList<BCW.ssc.Model.SSClist> GetSSClists(int SizeNum, string strWhere)
        {
            return dal.GetSSClists(SizeNum, strWhere);
        }

		/// <summary>
		/// ȡ��ÿҳ��¼
		/// </summary>
		/// <param name="p_pageIndex">��ǰҳ</param>
		/// <param name="p_pageSize">��ҳ��С</param>
		/// <param name="p_recordCount">�����ܼ�¼��</param>
		/// <param name="strWhere">��ѯ����</param>
		/// <returns>IList SSClist</returns>
		public IList<BCW.ssc.Model.SSClist> GetSSClists(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetSSClists(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

		#endregion  ��Ա����
	}
}

