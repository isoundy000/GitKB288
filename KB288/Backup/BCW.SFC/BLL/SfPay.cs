using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.SFC.Model;
namespace BCW.SFC.BLL
{
    /// <summary>
    /// ҵ���߼���SfPay ��ժҪ˵����
    /// </summary>
    public class SfPay
    {
        private readonly BCW.SFC.DAL.SfPay dal = new BCW.SFC.DAL.SfPay();
        public SfPay()
        { }
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
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool Exists1(int CID)
        {
            return dal.Exists1(CID);
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
        public bool Exists(int CID, int IsPrize)
        {
            return dal.Exists(CID, IsPrize);
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
        /// ����һ������
        /// </summary>
        public void Add(BCW.SFC.Model.SfPay model)
        {
            dal.Add(model);
        }
        /// <summary>
        /// �õ��н�ע��
        /// </summary>
        /// <param name="CID"></param>
        /// <param name="IsPrize"></param>
        /// <returns></returns>
        public int countPrize(int CID, int IsPrize)
        {
            return dal.countPrize(CID, IsPrize);
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
        /// ��ȡͶע��
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public int VoteNum(int ID, int CID)
        {
            return dal.VoteNum(ID, CID);
        }

        /// <summary>
        /// ��ȡ�е�ʽ��ע��
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public int DanVoteNum(int i, int CID)
        {
            return dal.DanVoteNum(i, CID);
        }
        /// <summary>
        /// ����ע��
        /// </summary>
        /// <returns></returns>
        public long AllPrice()
        {
            return dal.AllPrice();
        }
        /// <summary>
        /// ����ע��
        /// </summary>
        /// <returns></returns>
        public long AllPrice(int CID)
        {
            return dal.AllPrice(CID);
        }

        public int GetMaxCID()
        {
            return dal.GetMaxCID();
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
        ///������ͨ��ID��������
        public bool RoBotByID(int id)
        {
            return dal.RoBotByID(id);
        }
        /// <summary>
        /// me_��ѯ�����˹������
        /// </summary>
        public int GetSFCRobotCount(string strWhere)
        {
            return dal.GetSFCRobotCount(strWhere);
        }
        /// <summary>
        /// �����ֶ�ȡ�����б�
        /// </summary>
        public DataSet GetList1(string strField)
        {
            return dal.GetList1(strField);
        }
        /// <summary>
        /// ���н���
        /// </summary>
        /// <returns></returns>
        public long AllWinCent()
        {
            return dal.AllWinCent();
        }
        public long AllWinCentbyCID(int CID)
        {
            return dal.AllWinCentbyCID(CID);
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
        /// �õ�һ��WinCent
        /// </summary>
        public long GetWinCent(int ID)
        {
            return dal.GetWinCent(ID);
        }
        /// <summary>
        /// ����ID�õ�CID
        /// </summary>
        public int GetCID(int ID)
        {
            return dal.GetCID(ID);
        }
        /// <summary>
        /// �õ�һ��WinCent
        /// </summary>
        public long GetWinCent(string time1, string time2)
        {
            return dal.GetWinCent(time1, time2);
        }
        /// <summary>
        /// �õ�һ��WinCent
        /// </summary>
        public long GetPayCent(string time1, string time2)
        {
            return dal.GetPayCent(time1, time2);
        }
        /// <summary>
        /// ����һ������
        /// </summary>
        public void Update(BCW.SFC.Model.SfPay model)
        {
            dal.Update(model);
        }
        /// <summary>
        /// ����һ������
        /// </summary>
        public void UpdateChange(int id, string i)
        {
            dal.UpdateChange(id, i);
        }
        /// <summary>
        /// ����һ������
        /// </summary>
        public void UpdateResult(int id, string i)
        {
            dal.UpdateResult(id, i);
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
        /// �õ�һ������ʵ��
        /// </summary>
        public BCW.SFC.Model.SfPay GetSfPay(int id)
        {

            return dal.GetSfPay(id);
        }
        /// <summary>
        /// �õ�һ��WinCent5
        /// </summary>
        public long GetPayCentlast5()
        {
            return dal.GetPayCentlast5();
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
        /// <returns>IList SfPay</returns>
        public IList<BCW.SFC.Model.SfPay> GetSfPays(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetSfPays(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }
        /// <summary>
        /// ȡ��ÿҳ��¼
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">��ҳ��С</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <returns>IList SfPay</returns>
        public IList<BCW.SFC.Model.SfPay> GetSfPays1(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
        {
            return dal.GetSfPays1(p_pageIndex, p_pageSize, strWhere, strOrder, out p_recordCount);
        }
        /// <summary>
        /// ȡ�����м�¼
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">��ҳ��С</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <returns>IList HcPay</returns>
        public IList<BCW.SFC.Model.SfPay> GetSFPaysTop(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetSFPaysTop(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }
        #endregion  ��Ա����
    }
}

