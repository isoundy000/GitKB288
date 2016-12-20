using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model.Game;
namespace BCW.BLL.Game
{
    /// <summary>
    /// ҵ���߼���Stkpay ��ժҪ˵����
    /// </summary>
    public class Stkpay
    {
        private readonly BCW.DAL.Game.Stkpay dal = new BCW.DAL.Game.Stkpay();
        public Stkpay()
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
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool Exists(int ID)
        {
            return dal.Exists(ID);
        }

        /// <summary>
        /// �Ƿ����δ����¼
        /// </summary>
        public bool ExistsState(int StkId)
        {
            return dal.ExistsState(StkId);
        }

        /// <summary>
        /// �Ƿ���ڶҽ���¼
        /// </summary>
        public bool ExistsState(int ID, int UsID)
        {
            return dal.ExistsState(ID, UsID);
        }

        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool Exists(int HorseId, int UsID, int bzType, int Types, decimal Odds)
        {
            return dal.Exists(HorseId, UsID, bzType, Types, Odds);
        }

        /// <summary>
        /// ����ĳ���͵�Ͷע��
        /// </summary>
        public int GetCent(int StkId, int Types)
        {
            return dal.GetCent(StkId, Types);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public int Add(BCW.Model.Game.Stkpay model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public void Update(BCW.Model.Game.Stkpay model)
        {
            dal.Update(model);
        }

        /// <summary>
        /// ���¿���
        /// </summary>
        public void Update(int ID, long WinCent, int State, int WinNum)
        {
            dal.Update(ID, WinCent, State, WinNum);
        }

        /// <summary>
        /// �����û��ҽ���ʶ
        /// </summary>
        public void UpdateState(int ID)
        {
            dal.UpdateState(ID);
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public void Delete(int ID)
        {

            dal.Delete(ID);
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
        public BCW.Model.Game.Stkpay GetStkpay(int ID)
        {

            return dal.GetStkpay(ID);
        }

        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public BCW.Model.Game.Stkpay GetStkpaybystkid(int StkId)
        {

            return dal.GetStkpaybystkid(StkId);
        }

        /// <summary>
        /// �õ�һ��WinCent
        /// </summary>
        public long GetWinCent(int ID)
        {
            return dal.GetWinCent(ID);
        }

        /// <summary>
        /// �õ�һ��bzType
        /// </summary>
        public int GetbzType(int ID)
        {
            return dal.GetbzType(ID);
        }

        /// <summary>
        /// �õ�һ��WinCent
        /// </summary>
        public long GetPayCentlast()
        {
            return dal.GetPayCentlast();
        }

        /// <summary>
        /// �õ�һ��WinCentlast
        /// </summary>
        public long GetWinCentlast()
        {
            return dal.GetWinCentlast();
        }

        /// <summary>
        /// �õ�һ��WinCent5
        /// </summary>
        public long GetPayCentlast5()
        {
            return dal.GetPayCentlast5();
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
        public long GetPayCent(string time1, string time2)
        {
            return dal.GetPayCent(time1, time2);
        }

        /// <summary>
        /// �õ�һ��WinCent
        /// </summary>
        public long GetWinCent(string time1, string time2)
        {
            return dal.GetWinCent(time1, time2);
        }

        /// <summary>
        /// ����Ͷע�ܱ�ֵ
        /// </summary>
        public long GetPrice(string ziduan, string strWhere)
        {
            return dal.GetPrice(ziduan, strWhere);
        }

        /// <summary>
        /// ĳ��ĳID��Ͷ�˶��ٱ�
        /// </summary>
        public long GetSumPrices(int UsID, int StkId)
        {
            return dal.GetSumPrices(UsID, StkId);
        }

        /// <summary>
        /// ĳ��ĳ�淨ĳID��Ͷ�˶��ٱ�
        /// </summary>
        public long GetSumPrices(int UsID, int StkId, int Types)
        {
            return dal.GetSumPrices(UsID, StkId, Types);
        }

        /// <summary>
        /// ĳ��ĳͶע��ʽ��Ͷ�˶��ٱ�
        /// </summary>
        public long GetSumPricesbytype(int Types, int StkId)
        {
            return dal.GetSumPricesbytype(Types, StkId);
        }

        /// <summary>
        /// �����ֶ�ͳ���ж��������ݷ�������
        /// </summary>
        /// <param name="strWhere">ͳ������</param>
        /// <returns>ͳ�ƽ��</returns>
        public int GetRecordCount(string strWhere)
        {
            return dal.GetRecordCount(strWhere);
        }
        public int getState(int ID)
        {
            return dal.getState(ID);
        }

        /// <summary>
        /// ���ڻ�����
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="WinUserID"></param>
        /// <returns></returns>
        public bool ExistsReBot(int ID, int UsID)
        {
            return dal.ExistsReBot(ID, UsID);
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
        /// <returns>IList Stkpay</returns>
        public IList<BCW.Model.Game.Stkpay> GetStkpays(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetStkpays(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }

        /// <summary>
        /// ȡ�����м�¼
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">��ҳ��С</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <returns>IList SSCpay</returns>
        public IList<BCW.Model.Game.Stkpay> GetStkpaysTop(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetStkpaysTop(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }

        #endregion  ��Ա����
    }
}

