using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.BQC.Model;
namespace BCW.BQC.BLL
{
	/// <summary>
	/// ҵ���߼���BQCPay ��ժҪ˵����
	/// </summary>
	public class BQCPay
	{
		private readonly BCW.BQC.DAL.BQCPay dal=new BCW.BQC.DAL.BQCPay();
		public BQCPay()
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
        /// �õ����ID
        /// </summary>
        public int GetMaxId(int usid)
        {
            return dal.GetMaxId(usid);
        }

        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool Exists(int id)
		{
			return dal.Exists(id);
		}
        /// <summary>
        /// ����ID�õ�CID
        /// </summary>
        public int GetCID(int ID)
        {
            return dal.GetCID(ID);
        }
        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool Exists(int CID, int IsPrize)
        {
            return dal.Exists(CID, IsPrize);
        }
        /// <summary>
        /// ���ڻ�����
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="WinUserID"></param>
        /// <returns></returns>
        public bool ExistsReBot(int id, int usID)
        {
            return dal.ExistsReBot(id, usID);
        }
        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool Exists1(int id, int IsPrize)
        {
            return dal.Exists1(id, IsPrize);
        }
        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool Exists2(int id, int IsPrize2)
        {
            return dal.Exists2(id, IsPrize2);
        }
        /// <summary>
        /// me_��ѯ�����˹������
        /// </summary>
        public int GetBQCRobotCount(string strWhere)
        {
            return dal.GetBQCRobotCount(strWhere);
        }
        /// <summary>
        /// ���ÿ����ע�ܶ�
        /// </summary>
        /// <param name="CID"></param>
        /// <returns></returns>
        public long PayCents(int CID)
        {
            return dal.PayCents(CID);
        }

        /// <summary>
        /// ���ÿ����ע�ܶ�
        /// </summary>
        /// <param name="CID"></param>
        /// <returns></returns>
        public int VoteNum(int CID)
        {
            return dal.VoteNum(CID);
        }

        /// <summary>
        /// ���ÿ���н�ע��
        /// </summary>
        /// <param name="cid"></param>
        /// <returns></returns>
        public int PrizeNum(int cid)
        {
            return dal.PrizeNum(cid);
        }

        /// <summary>
        /// ��ȡͶע��
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public int VoteNum(int ID, int CID)
        {
            return dal.VoteNum(ID, CID);
        }
        /// <summary>
        /// ��ȡͶע��
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public int VoteNum1(int ID, int CID)
        {
            return dal.VoteNum1(ID, CID);
        }
        /// <summary>
        /// ��ȡͶע��
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public int VoteNum2(int ID, int CID)
        {
            return dal.VoteNum2(ID, CID);
        }
        /// <summary>
        /// �Ƿ���ڶҽ���¼
        /// </summary>
        public bool ExistsState(int id, int UsID)
        {
            return dal.ExistsState(id, UsID);
        }

        /// <summary>
        /// �����û��ҽ���ʶ
        /// </summary>
        public void UpdateState(int id)
        {
            dal.UpdateState(id);
        }

        /// <summary>
        /// �õ�һ��WinCent
        /// </summary>
        public long GetWinCent(int ID)
        {
            return dal.GetWinCent(ID);
        }

        /// <summary>
        /// �õ�һ��WinCent
        /// </summary>
        public long GetPayCent(string time1, string time2)
        {
            return dal.GetPayCent(time1, time2);
        }

        //ӯ������---------------------------------
        /// <summary>
        /// �õ�һ��WinCent
        /// </summary>
        public long GetPayCentlast()
        {
            return dal.GetPayCentlast();
        }

        /// <summary>
        /// ����Ͷע�ܱ�ֵ
        /// </summary>
        public long GetPrice(string ziduan, string strWhere)
        {
            return dal.GetPrice(ziduan, strWhere);
        }

        //ӯ������---------------------------------
        /// <summary>
        /// �õ�һ��WinCent5
        /// </summary>
        public long GetPayCentlast5()
        {
            return dal.GetPayCentlast5();
        }
        /// <summary>
        /// �õ�һ��WinCent
        /// </summary>
        public long GetWinCent(string time1, string time2)
        {
            return dal.GetWinCent(time1, time2);
        }
        /// <summary>
        /// �õ�һ��WinCentlast
        /// </summary>
        public long GetWinCentlast()
        {
            return dal.GetWinCentlast();
        }
        /// <summary>
        /// �õ�һ��WinCentlast5
        /// </summary>
        public long GetWinCentlast5()
        {
            return dal.GetWinCentlast5();
        }
        /// <summary>
        /// �������ע�����UsID
        /// </summary>
        /// <returns></returns>
        public long getAllPricebyusID(int UsID)
        {
            return dal.getAllPricebyusID(UsID);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public int Add(BCW.BQC.Model.BQCPay model)
		{
			return dal.Add(model);
		}

        /// <summary>
        /// �������ע��
        /// </summary>
        /// <param name="CID"></param>
        /// <returns></returns>
        public long getAllPrice(int CID)
        {
            return dal.getAllPrice(CID);
        }

        public long AllWinCentbyusID(int usID)
        {
            return dal.AllWinCentbyusID(usID);
        }

        public int AllVoteNumbyusID(int usID)
        {
            return dal.AllVoteNumbyusID(usID);
        }

        public int AllVoteNum(int cid)
        {
            return dal.AllVoteNum(cid);
        }


        public long AllWinCentbyCID(int CID)
        {
            return dal.AllWinCentbyCID(CID);
        }
        /// <summary>
        /// ����һ������
        /// </summary>
        public void Update(BCW.BQC.Model.BQCPay model)
		{
			dal.Update(model);
		}

        /// <summary>
        /// ����һ������
        /// </summary>
        public void UpdateChange(int id, int i)
        {
            dal.UpdateChange(id, i);
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public void Delete(int id)
		{
			
			dal.Delete(id);
		}

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public void Delete(string strWhere)
        {

            dal.Delete(strWhere);
        }

        /// <summary>
        /// �õ���1�Ƚ�ע��
        /// </summary>
        /// <param name="CID"></param>
        /// <param name="IsPrize"></param>
        /// <returns></returns>
        public int countPrize(int CID, int IsPrize)
        {
            return dal.countPrize(CID, IsPrize);
        }
        /// <summary>
        /// �õ���2�Ƚ�ע��
        /// </summary>
        /// <param name="CID"></param>
        /// <param name="IsPrize"></param>
        /// <returns></returns>
        public int countPrize2(int CID)
        {
            return dal.countPrize2(CID);
        }
        /// <summary>
        /// �õ��н��ı�
        /// </summary>
        /// <param name="CID"></param>
        /// <param name="Isprize"></param>
        /// <returns></returns>
        public long Gold(int CID, int Isprize)
        {
            return dal.Gold(CID, Isprize);
        }

        /// <summary>
        /// ����ע��
        /// </summary>
        /// <returns></returns>
        public long AllPrice(int CID)
        {
            return dal.AllPrice(CID);
        }
        /// <summary>
        /// ���н���
        /// </summary>
        /// <returns></returns>
        public long AllWinCent()
        {
            return dal.AllWinCent();
        }

        /// <summary>
        /// ������ܽ���
        /// </summary>
        /// <returns></returns>
        public long AllWinCent(int CID)
        {
            return dal.AllWinCent(CID);
        }
        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public BCW.BQC.Model.BQCPay GetBQCPay(int id)
		{
			
			return dal.GetBQCPay(id);
		}

		/// <summary>
		/// �����ֶ�ȡ�����б�
		/// </summary>
		public DataSet GetList(string strField, string strWhere)
		{
			return dal.GetList(strField, strWhere);
		}

        public DataSet GetList(int CID)
        {
            return dal.GetList(CID);
        }

        /// <summary>
        /// ȡ��ÿҳ��¼
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">��ҳ��С</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <returns>IList BQCPay</returns>
        public IList<BCW.BQC.Model.BQCPay> GetBQCPays(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetBQCPays(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}
        /// <summary>
        /// ȡ��ÿҳ��¼
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">��ҳ��С</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <returns>IList BQCPay</returns>
        public IList<BCW.BQC.Model.BQCPay> GetBQCPays1(int p_pageIndex, int p_pageSize, string strWhere,string strOrder, out int p_recordCount)
        {
            return dal.GetBQCPays1(p_pageIndex, p_pageSize, strWhere,strOrder, out p_recordCount);
        }
        /// <summary>
        /// ȡ�����м�¼
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">��ҳ��С</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <returns>IList HcPay</returns>
        public IList<BCW.BQC.Model.BQCPay> GetBQCPaysTop(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetBQCPaysTop(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }

        #endregion  ��Ա����
    }
}

