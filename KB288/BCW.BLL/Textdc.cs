using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
	/// <summary>
	/// ҵ���߼���Textdc ��ժҪ˵����
	/// </summary>
	public class Textdc
	{
		private readonly BCW.DAL.Textdc dal=new BCW.DAL.Textdc();
		public Textdc()
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
        /// �Ƿ����δ�������мҼ�¼
        /// </summary>
        public bool Exists2(int BID)
        {
            return dal.Exists2(BID);
        }

        /// <summary>
        /// ����ĳ���Ӿ��µ�c�мұ�֤���ܶ�
        /// </summary>
        public long GetCents(int BID, int IsZtid)
        {
            return dal.GetCents(BID, IsZtid);
        }

		/// <summary>
		/// ����һ������
		/// </summary>
		public int  Add(BCW.Model.Textdc model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Update(BCW.Model.Textdc model)
		{
			dal.Update(model);
		}
               
        /// <summary>
        /// ����һ������
        /// </summary>
        public void UpdateOutCent(BCW.Model.Textdc model)
        {
            dal.UpdateOutCent(model);
        }
              
        /// <summary>
        /// ����һ������
        /// </summary>
        public void UpdateState(int ID, int State)
        {
            dal.UpdateState(ID, State);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public void UpdateState(int ID, int State, long AcCent)
        {
            dal.UpdateState(ID, State, AcCent);
        }

        /// <summary>
        /// ��¼��־
        /// </summary>
        public void UpdateLogText(int BID, int UsID, string LogText)
        {
            dal.UpdateLogText(BID, UsID, LogText);
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
		public BCW.Model.Textdc GetTextdc(int ID)
		{
			
			return dal.GetTextdc(ID);
		}
               
        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public BCW.Model.Textdc GetTextdc(int BID, int IsZtid)
        {

            return dal.GetTextdc(BID, IsZtid);
        }
               
        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public BCW.Model.Textdc GetTextdc(int BID, int IsZtid, int UsID)
        {

            return dal.GetTextdc(BID, IsZtid, UsID);
        }
                
        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public BCW.Model.Textdc GetTextdc2(int ID, int BID)
        {

            return dal.GetTextdc2(ID, BID);
        }
                
        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public string GetLogText(int BID)
        {
            return dal.GetLogText(BID);
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
		/// <returns>IList Textdc</returns>
		public IList<BCW.Model.Textdc> GetTextdcs(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetTextdcs(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

		#endregion  ��Ա����
	}
}

