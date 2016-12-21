using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.JQC.Model;
namespace BCW.JQC.BLL
{
    /// <summary>
    /// ҵ���߼���JQC_Bet ��ժҪ˵����
    /// </summary>
    public class JQC_Bet
    {
        private readonly BCW.JQC.DAL.JQC_Bet dal = new BCW.JQC.DAL.JQC_Bet();
        public JQC_Bet()
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
        /// ����һ������
        /// </summary>
        public int Add(BCW.JQC.Model.JQC_Bet model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public void Update(BCW.JQC.Model.JQC_Bet model)
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
        public BCW.JQC.Model.JQC_Bet GetJQC_Bet(int ID)
        {
            return dal.GetJQC_Bet(ID);
        }

        /// <summary>
        /// �����ֶ�ȡ�����б�
        /// </summary>
        public DataSet GetList(string strField, string strWhere)
        {
            return dal.GetList(strField, strWhere);
        }

        //=====================================
        /// <summary>
        /// me_��̨��ҳ������ȡ���а������б�
        /// </summary>
        public DataSet GetListByPage2(int startIndex, int endIndex, string s1, string s2)
        {
            return dal.GetListByPage2(startIndex, endIndex, s1, s2);
        }
        /// <summary>
        /// me_����õ�һ������ʵ��
        /// </summary>
        public BCW.JQC.Model.JQC_Bet GetNC_suiji()
        {
            return dal.GetNC_suiji();
        }
        /// <summary>
        /// me_�Ƿ���ڿ�����û�з�����
        /// </summary>
        public bool Exists_num(int Lottery_issue)
        {
            return dal.Exists_num(Lottery_issue);
        }
        /// <summary>
        /// me_�Ƿ���ڶҽ���¼
        /// </summary>
        public bool ExistsState(int ID, int UsID)
        {
            return dal.ExistsState(ID, UsID);
        }
        /// <summary>
        /// me_�õ��������н�����
        /// </summary>
        public int count_zhu(int Lottery_issue)
        {
            return dal.count_zhu(Lottery_issue);
        }
        /// <summary>
        /// me_�õ�����ĳע���н�����
        /// </summary>
        public int count_renshu(int Lottery_issue, string Prize)
        {
            return dal.count_renshu(Lottery_issue, Prize);
        }
        /// <summary>
        /// me_�����н�״̬
        /// </summary>
        public void Update_win(int ID, int State)
        {
            dal.Update_win(ID, State);
        }
        /// <summary>
        ///  me_�����ֶ��޸������б�
        /// </summary>
        public DataSet update_GetMoney(string strField, string strWhere)
        {
            return dal.update_GetMoney(strField, strWhere);
        }
        /// <summary>
        /// me_��ѯ�����˹������
        /// </summary>
        public int GetRecordCount(string strWhere)
        {
            return dal.GetRecordCount(strWhere);
        }
        /// <summary>
        /// me_��̨�����ݿ������ںţ���Ӧ���ڹ��������
        /// </summary>
        public BCW.JQC.Model.JQC_Bet Get_tounum(int Lottery_issue)
        {
            return dal.Get_tounum(Lottery_issue);
        }
        /// <summary>
        /// me_�����ںŵõ�һ������ʵ��
        /// </summary>
        public BCW.JQC.Model.JQC_Bet Get_qihao(int Lottery_issue)
        {
            return dal.Get_qihao(Lottery_issue);
        }
        /// <summary>
        /// me_�����ںŵõ��ɽ���
        /// </summary>
        public long Get_paijiang(int Lottery_issue)
        {
            return dal.Get_paijiang(Lottery_issue);
        }
        /// <summary>
        /// ����Ͷע�ܱ�ֵ
        /// </summary>
        public long GetPrice(string ziduan, string strWhere)
        {
            return dal.GetPrice(ziduan, strWhere);
        }
        //==

        /// <summary>
        /// ȡ��ÿҳ��¼
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">��ҳ��С</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <returns>IList JQC_Bet</returns>
        public IList<BCW.JQC.Model.JQC_Bet> GetJQC_Bets(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
        {
            return dal.GetJQC_Bets(p_pageIndex, p_pageSize, strWhere, strOrder, out p_recordCount);
        }

        #endregion  ��Ա����
    }
}

