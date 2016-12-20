using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model.Game;
namespace BCW.BLL.Game
{
    /// <summary>
    /// ҵ���߼���Luckpay ��ժҪ˵����
    /// </summary>
    public class Luckpay
    {
        private readonly BCW.DAL.Game.Luckpay dal = new BCW.DAL.Game.Luckpay();
        public Luckpay()
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
        /// �Ƿ���ڶҽ���¼
        /// </summary>
        public bool ExistsState(int ID, int UsID)
        {
            return dal.ExistsState(ID, UsID);
        }

        /// <summary>
        /// �Ƿ����δ����¼
        /// </summary>
        public bool ExistsState(int LuckId)
        {
            return dal.ExistsState(LuckId);
        }
          /// <summary>
        /// �õ�������Ͷע����
        /// </summary>
        public int GetRobotbuy(int usid,int luckid)
        {
            return dal.GetRobotbuy(usid,luckid);
        }
        /// <summary>
        /// ����ĳ�ڹ����������ֵ��ܱ���
        /// </summary>
        public long GetSumBuyCent(int LuckId, string BuyNum)
        {
            return dal.GetSumBuyCent(LuckId, BuyNum);
        }
        /// <summary>
        /// ����ĳ�ڹ�����ѡ�������ֵ��ܱ���
        /// </summary>
        public long GetSumBuyCentbychoose(int LuckId, string BuyNum)
        {
            return dal.GetSumBuyCentbychoose(LuckId, BuyNum);
        }
         /// <summary>
         /// ����ĳ�ڹ���ĳ���͵��ܱ���
         /// </summary>
        public long GetSumBuyTypeCent(int LuckId, string BuyType)
        {
            return dal.GetSumBuyTypeCent(LuckId,BuyType);
        }

        /// <summary>
        /// ����ĳ��ĳID������ܱ���
        /// </summary>
        public long GetSumBuyCent(int LuckId, int UsID)
        {
            return dal.GetSumBuyCent(LuckId, UsID);
        }
         /// <summary>
        /// ����ĳ�����������ע���
        /// </summary>
        public long GetAllBuyCent(int LuckId)
        {
            return dal.GetAllBuyCent(LuckId);
        }
        /// <summary>
        /// ����ĳ�ڹ�������
        /// </summary>
        public int GetCount(int LuckId)
        {
            return dal.GetCount(LuckId);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public int Add(BCW.Model.Game.Luckpay model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public void Update(BCW.Model.Game.Luckpay model)
        {
            dal.Update(model);
        }

        /// <summary>
        /// ���¿���
        /// </summary>
        public void Update(int ID, long WinCent, int State)
        {
            dal.Update(ID, WinCent, State);
        }
        /// <summary>
        /// ����δ�ҽ���
        /// </summary>
        public void UpdateOverDay(string AddTime)
        {
            dal.UpdateOverDay(AddTime);
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
        public BCW.Model.Game.Luckpay GetLuckpay(int ID)
        {

            return dal.GetLuckpay(ID);
        }
         /// <summary>
        /// ����Ͷע�ܱ�ֵ
        /// </summary>
        public long GetPrice(string ziduan, string strWhere)
        {
          return   dal.GetPrice(ziduan,strWhere);
        }
        /// <summary>
        /// ��̨����Ͷע�ܱ�ֵ,������ϵͳ��
        /// </summary>
        public long ManGetPrice(string ziduan, string strWhere)
        {
            return dal.ManGetPrice(ziduan, strWhere);
        }
        /// <summary>
        /// �õ�һ��WinCent
        /// </summary>
        public long GetWinCent(int ID)
        {
            return dal.GetWinCent(ID);
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
        /// <returns>IList Luckpay</returns>
        public IList<BCW.Model.Game.Luckpay> GetLuckpays(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetLuckpays(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }

        /// <summary>
        /// ȡ�����м�¼
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">��ҳ��С</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <returns>IList Luckpay</returns>
        public IList<BCW.Model.Game.Luckpay> GetLuckpaysTop(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetLuckpaysTop(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }

        #endregion  ��Ա����
    }
}

