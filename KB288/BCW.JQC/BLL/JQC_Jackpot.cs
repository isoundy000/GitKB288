using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.JQC.Model;
namespace BCW.JQC.BLL
{
    /// <summary>
    /// ҵ���߼���JQC_Jackpot ��ժҪ˵����
    /// </summary>
    public class JQC_Jackpot
    {
        private readonly BCW.JQC.DAL.JQC_Jackpot dal = new BCW.JQC.DAL.JQC_Jackpot();
        public JQC_Jackpot()
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
        public bool Exists(int id)
        {
            return dal.Exists(id);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public int Add(BCW.JQC.Model.JQC_Jackpot model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public void Update(BCW.JQC.Model.JQC_Jackpot model)
        {
            dal.Update(model);
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public void Delete(int id)
        {

            dal.Delete(id);
        }

        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public BCW.JQC.Model.JQC_Jackpot GetJQC_Jackpot(int id)
        {

            return dal.GetJQC_Jackpot(id);
        }

        /// <summary>
        /// �����ֶ�ȡ�����б�
        /// </summary>
        public DataSet GetList(string strField, string strWhere)
        {
            return dal.GetList(strField, strWhere);
        }

        //==    
        /// <summary>
        /// ����Ͷע�ܱ�ֵ
        /// </summary>
        public long GetPrice(string ziduan, string strWhere)
        {
            return dal.GetPrice(ziduan, strWhere);
        }

        /// <summary>
        /// me_�Ƿ����ϵͳͶ����¼
        /// </summary>
        public bool Exists_jc(int id)
        {
            return dal.Exists_jc(id);
        }
        /// <summary>
        /// me_�Ƿ����ϵͳ�۳���¼
        /// </summary>
        public bool Exists_kc(int id)
        {
            return dal.Exists_kc(id);
        }
        /// <summary>
        /// me_����id�õ�����
        /// </summary>
        public long GetGold()
        {
            return dal.GetGold();
        }
        /// <summary>
        /// me_�����ںŵõ�����
        /// </summary>
        public long GetGold_phase(int phase)
        {
            return dal.GetGold_phase(phase);
        }
        /// <summary>
        /// me_�õ�ϵͳͶ���Ĵ���
        /// </summary>
        public int Getxitong_toujin()
        {
            return dal.Getxitong_toujin();
        }
        /// <summary>
        /// me_�õ�ϵͳ���յĴ���
        /// </summary>
        public int Getxitong_huishou()
        {
            return dal.Getxitong_huishou();
        }
        /// <summary>
        /// me_����ͶעID�õ����ء���20160713
        /// </summary>
        public long Get_BetID(int BetID)
        {
            return dal.Get_BetID(BetID);
        }
        /// <summary>
        /// me_�����ںŵõ�δ�����Ľ��ء���20160713
        /// </summary>
        public long Getweikai_phase(int phase)
        {
            return dal.Getweikai_phase(phase);
        }
        /// <summary>
        /// me_�õ�����ϵͳ��ȡ
        /// </summary>
        public long Get_xtshouqu(int phase)
        {
            return dal.Get_xtshouqu(phase);
        }
        //==================================

        /// <summary>
        /// ȡ��ÿҳ��¼
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">��ҳ��С</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <returns>IList JQC_Jackpot</returns>
        public IList<BCW.JQC.Model.JQC_Jackpot> GetJQC_Jackpots(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
        {
            return dal.GetJQC_Jackpots(p_pageIndex, p_pageSize, strWhere, strOrder, out p_recordCount);
        }

        #endregion  ��Ա����
    }
}

