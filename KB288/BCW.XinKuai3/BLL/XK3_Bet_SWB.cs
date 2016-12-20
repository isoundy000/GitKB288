using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.XinKuai3.Model;
namespace BCW.XinKuai3.BLL
{
	/// <summary>
	/// ҵ���߼���XK3_Bet_SWB ��ժҪ˵����
	/// </summary>
	public class XK3_Bet_SWB
	{
		private readonly BCW.XinKuai3.DAL.XK3_Bet_SWB dal=new BCW.XinKuai3.DAL.XK3_Bet_SWB();
		public XK3_Bet_SWB()
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
		public int  Add(BCW.XinKuai3.Model.XK3_Bet_SWB model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Update(BCW.XinKuai3.Model.XK3_Bet_SWB model)
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
		public BCW.XinKuai3.Model.XK3_Bet_SWB GetXK3_Bet_SWB(int ID)
		{
			
			return dal.GetXK3_Bet_SWB(ID);
		}



        //===============================
        /// <summary>
        /// me_����������һ������
        /// </summary>
        public int Add_Robot(BCW.XinKuai3.Model.XK3_Bet_SWB model)
        {
            return dal.Add_Robot(model);
        }
        /// <summary>
        /// me_����״̬
        /// </summary>
        public void UpdateState(int ID, int State)
        {
            dal.UpdateState(ID, State);
        }

        /// <summary>
        /// me_�Ƿ���ڶҽ���¼
        /// </summary>
        public bool ExistsState(int ID, int UsID)
        {
            return dal.ExistsState(ID, UsID);
        }

        /// <summary>
        /// me_�Ƿ���ڿ�����û�з�����
        /// </summary>
        public bool Exists_num(string Lottery_issue)
        {
            return dal.Exists_num(Lottery_issue);
        }
        /// <summary>
        /// me_�����н�״̬
        /// </summary>
        public void Update_win(int ID, long GetMoney)
        {
            dal.Update_win(ID, GetMoney);
        }
        /// <summary>
        /// me_�����������ʷ��¼��ͨ�������ںŲ�ѯ��Ӧ��Ͷע���
        /// </summary>
        public BCW.XinKuai3.Model.XK3_Bet_SWB GetXK3_Bet_SWB_num(string Lottery_issue)
        {
            return dal.GetXK3_Bet_SWB_num(Lottery_issue);
        }
        /// <summary>
        /// me_�����н��󣬳�7��δ�콱��id
        /// </summary>
        public void UpdateExceed_num(string _where)
        {
            dal.UpdateExceed_num(_where);
        }
        /// <summary>
        /// ����Ͷע�ܱ�ֵ
        /// </summary>
        public long GetPrice(string ziduan,string strWhere)
        {
            return dal.GetPrice(ziduan,strWhere);
        }
        /// <summary>
        /// me_��̨�����ݿ������ںţ���Ӧ���ڹ��������
        /// </summary>
        public BCW.XinKuai3.Model.XK3_Bet_SWB GetXK3_Bet_SWB_hounum(string Lottery_issue)
        {
            return dal.GetXK3_Bet_SWB_hounum(Lottery_issue);
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
        /// me_��ѯ�����˹������
        /// </summary>
        public int GetXK3_Bet_SWB_GetRecordCount(string strWhere)
        {
            return dal.GetXK3_Bet_SWB_GetRecordCount(strWhere);
        }

        /// <summary>
        /// me_ȡ��ÿ�����Ͷע�ļ�¼��������
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">��ҳ��С</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <returns>IList XK3_Bet_SWB</returns>
        public IList<BCW.XinKuai3.Model.XK3_Bet_SWB> GetXK3_Bet_SWB_playnum1(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetXK3_Bet_SWB_playnum1(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }
        public IList<BCW.XinKuai3.Model.XK3_Bet_SWB> GetXK3_Bet_SWB_playnum2(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetXK3_Bet_SWB_playnum2(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }
        public IList<BCW.XinKuai3.Model.XK3_Bet_SWB> GetXK3_Bet_SWB_playnum3(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetXK3_Bet_SWB_playnum3(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }

        /// <summary>
        /// me_��̨��ҳ������ȡ���а������б�
        /// </summary>
        public DataSet GetListByPage2(int startIndex, int endIndex, string s1, string s2)
        {
            return dal.GetListByPage2(startIndex, endIndex, s1, s2);
        }



        //================================




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
		/// <returns>IList XK3_Bet_SWB</returns>
		public IList<BCW.XinKuai3.Model.XK3_Bet_SWB> GetXK3_Bet_SWBs(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetXK3_Bet_SWBs(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

		#endregion  ��Ա����
	}
}

