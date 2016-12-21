using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.SFC.Model;
using System.Text.RegularExpressions;
using System.Net;

namespace BCW.SFC.BLL
{
    /// <summary>
    /// ҵ���߼���SfList ��ժҪ˵����
    /// </summary>
    public class SfList
    {
        private readonly BCW.SFC.DAL.SfList dal = new BCW.SFC.DAL.SfList();
        public SfList()
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
        ///// <summary>
        ///// �Ƿ���ڸü�¼
        ///// </summary>
        //public int Existslist()
        //{
        //    return dal.Existslist();
        //}
        /// <summary>
        /// �Ƿ���ڸ�����
        /// </summary>
        public bool ExistsCID(int CID)
        {
            return dal.ExistsCID(CID);
        }
        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool Existsjilu()
        {
            return dal.Existsjilu();
        }
        /// <summary>
        /// �Ƿ����ϵͳͶע��¼
        /// </summary>
        public bool ExistsSysprize(int CID)
        {
            return dal.ExistsSysprize(CID);
        }
        /// <summary>
        /// �õ�State
        /// </summary>
        public int getState(int CID)
        {
            return dal.getState(CID);
        }
        /// <summary>
        /// ����һ������
        /// </summary>
        public void Add(BCW.SFC.Model.SfList model)
        {
            dal.Add(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public void Update(BCW.SFC.Model.SfList model)
        {
            dal.Update(model);
        }
        /// <summary>
        /// ���´����ں���Ϣ����
        /// </summary>
        public void UpdateXinXi(string Match, string Team_Home, string Team_Away, string Start_time, DateTime EndTime, DateTime Sale_StartTime, int CID)
        {
            dal.UpdateXinXi(Match, Team_Home, Team_Away, Start_time, EndTime, Sale_StartTime, CID);
        }
        /// <summary>
        /// ���´����ں���Ϣ����
        /// </summary>
        public void UpdateXinXi(string Match, string Team_Home, string Team_Away, string Start_time, int CID)
        {
            dal.UpdateXinXi(Match, Team_Home, Team_Away, Start_time, CID);
        }
        /// <summary>
        /// ����sale_starttime
        /// </summary>
        public void Updatesale_starttime(DateTime sale_starttime, int CID)
        {
            dal.Updatesale_starttime(sale_starttime, CID);
        }
        /// <summary>
        /// ����end_time
        /// </summary>
        /// <param name="end_time"></param>
        /// <param name="CID"></param>
        public void Updateend_time(DateTime end_time, int CID)
        {
            dal.Updateend_time(end_time, CID);
        }
        /// <summary>
        /// ������ע��
        /// </summary>
        /// <param name="PayCount"></param>
        /// <param name="CID"></param>
        public void UpdatePayCount(int PayCount, int CID)
        {
            dal.UpdatePayCount(PayCount, CID);
        }
        /// <summary>
        /// ������ע�ܶ�
        /// </summary>
        /// <param name="PayCent"></param>
        /// <param name="CID"></param>
        public void UpdatePayCent(long PayCent, int CID)
        {
            dal.UpdatePayCent(PayCent, CID);
        }
        public void updateNowprize(long nowprize, int CID)
        {
            dal.updateNowprize(nowprize, CID);
        }
        public void updateother(long other, int CID)
        {
            dal.updateother(other, CID);
        }
        /// <summary>
        /// ����һ������
        /// </summary>
        public void UpdateResult(int id, string Result)
        {
            dal.UpdateResult(id, Result);
        }
        /// <summary>
        /// ����ϵͳͶע״̬
        /// </summary>
        public void UpdateSysprizestatue(int CID, int sysprizestatue)
        {
            dal.UpdateSysprizestatue(CID, sysprizestatue);
        }

        /// <summary>
        /// ���µ��ڽ��ؽ���
        /// </summary>
        public void UpdateNextprize(int CID, long nextprize)
        {
            dal.UpdateNextprize(CID, nextprize);
        }     /// <summary>
        /// ���µ���ϵͳ��ȡ����
        /// </summary>
        public void Updatesysdayprize(int CID, long sysdayprize)
        {
            dal.Updatesysdayprize(CID, sysdayprize);
        }
        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool Existsysprize(int CID)
        {
            return dal.Existsysprize(CID);
        }
        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public void Delete(int id)
        {

            dal.Delete(id);
        }
        /// ����ϵͳͶע
        /// </summary>
        public void UpdateSysstaprize(int CID, int sysprizestatue, long sysprize)
        {
            dal.UpdateSysstaprize(CID, sysprizestatue, sysprize);
        }
        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public BCW.SFC.Model.SfList GetSfList(int id)
        {
            return dal.GetSfList(id);
        }
        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public BCW.SFC.Model.SfList GetSfList1(int CID)
        {
            return dal.GetSfList1(CID);
        }

        /// <summary>
        /// �����ֶ�ȡ�����б�
        /// </summary>
        public DataSet GetList(string strField, string strWhere)
        {
            return dal.GetList(strField, strWhere);
        }
        /// <summary>
        /// �����ֶ�ȡ�����б�
        /// </summary>
        public DataSet GetList1(string strField)
        {
            return dal.GetList1(strField);
        }
        /// <summary>
        /// �õ���һ��CID
        /// </summary>
        public int CIDnew()
        {
            return dal.CIDnew();
        }
        /// <summary>
        /// �õ����������ں�
        /// </summary>
        public int CID()
        {
            return dal.CID();
        }
        /// <summary>
        /// �õ��������
        /// </summary>
        public string result(int CID)
        {
            return dal.result(CID);
        }
        /// <summary>
        /// �õ���ǰ�����ܶ�
        /// </summary>
        public long nowprize(int CID)
        {
            return dal.nowprize(CID);
        }
        /// <summary>
        /// �õ�����ϵͳͶ��
        /// </summary>
        public long getsysprize(int CID)
        {
            return dal.getsysprize(CID);
        }
        /// <summary>
        /// �õ�����ϵͳͶ��״̬
        /// </summary>
        public int getsysprizestatue(int CID)
        {
            return dal.getsysprizestatue(CID);
        }
        /// <summary>
        /// �õ�һ��GetSysPaylast
        /// </summary>
        public long GetSysPaylast()
        {
            return dal.GetSysPaylast();
        }
        /// <summary>
        /// ����Ͷע�ܱ�ֵ
        /// </summary>
        public long GetPrice(string ziduan, string strWhere)
        {
            return dal.GetPrice(ziduan, strWhere);
        }
        /// <summary>
        /// �õ�һ��GetSysPaylast5
        /// </summary>
        public long GetSysPaylast5()
        {
            return dal.GetSysPaylast5();
        }
        /// <summary>
        /// �õ�һ��GetSysdayprizelast
        /// </summary>
        public long GetSysdayprizelast()
        {
            return dal.GetSysdayprizelast();
        }
        /// <summary>
        /// �õ�һ��GetSysdayprizelast5
        /// </summary>
        public long GetSysdayprizelast5()
        {
            return dal.GetSysdayprizelast5();
        }
        /// <summary>
        /// �õ����ڽ��ؽ���
        /// </summary>
        public int getnextprize(int CID)
        {
            return dal.getnextprize(CID);
        }
        /// <summary>
        /// �����ںŵõ�id
        /// </summary>
        public int id(int CID)
        {
            return dal.id(CID);
        }
        /// <summary>
        /// ȡ��ÿҳ��¼
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">��ҳ��С</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <returns>IList SfList</returns>
        public IList<BCW.SFC.Model.SfList> GetSfLists(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetSfLists(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }


        /// <summary>
        /// ����ָ�������ı������
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string FindResultByPhase(int CID)
        {
            return dal.FindResultByPhase(CID);
        }

        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateMatchs(Model.SfList model)
        {
            if (dal.UpdateMatchs(model) != 0)
                return true;
            else
                return false;
        }
        /// <summary>
        /// ���¿���״̬
        /// </summary>
        /// <param name="state">״̬</param>
        /// <param name="cid">�ں�</param>
        /// <returns></returns>
        public bool UpdateState(int state, int cid)
        {
            if (dal.UpdateState(state, cid) != 0)
                return true;
            else
                return false;
        }
        /// <summary>
        /// �õ�����Ͷ���ʶ
        /// </summary>
        public int getsysstate(int CID)
        {
            return dal.getsysstate(CID);
        }
        #endregion  ��Ա����

    }
}

