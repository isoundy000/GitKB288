using System.Data;
using System.Collections.Generic;
namespace BCW.BQC.BLL
{
    /// <summary>
    /// ҵ���߼���BQCList ��ժҪ˵����
    /// </summary>
    public class BQCList
    {
        private readonly DAL.BQCList dal = new DAL.BQCList();
        public BQCList()
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
        /// �����ںŵõ�id
        /// </summary>
        public int id(int CID)
        {
            return dal.id(CID);
        }
        /// <summary>
        /// �õ����ڽ��ؽ���
        /// </summary>
        public int getnextprize(int CID)
        {
            return dal.getnextprize(CID);
        }
        /// <summary>
        /// �õ�����Ͷ���ʶ
        /// </summary>
        public int getsysstate(int CID)
        {
            return dal.getsysstate(CID);
        }
        /// <summary>
        /// �õ�����Ͷ���ʶ
        /// </summary>
        public int getState(int CID)
        {
            return dal.getState(CID);
        }
        /// <summary>
        /// �õ���һ��CID
        /// </summary>
        public int CIDnew()
        {
            return dal.CIDnew();
        }
        /// <summary>
        /// �õ��������
        /// </summary>
        public string result(int CID)
        {
            return dal.result(CID);
        }
        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool Exists(int id)
        {
            return dal.Exists(id);
        }      /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool Existslist(int id)
        {
            return dal.Existslist(id);
        }
        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool Existsjilu()
        {
            return dal.Existsjilu();
        }
        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool Existsysprize(int CID)
        {
            return dal.Existsysprize(CID);
        }
        /// <summary>
        /// �Ƿ����ϵͳͶע��¼
        /// </summary>
        public bool ExistsSysprize(int CID)
        {
            return dal.ExistsSysprize(CID);
        }

        /// <summary>
        /// �õ���ǰ�����ܶ�
        /// </summary>
        public long nowprize(int CID)
        {
            return dal.nowprize(CID);
        }
        /// <summary>
        /// ����ϵͳͶע״̬
        /// </summary>
        public void UpdateSysprizestatue(int CID, int sysprizestatue)
        {
            dal.UpdateSysprizestatue(CID, sysprizestatue);
        }
        /// <summary>
        /// ����ϵͳͶע
        /// </summary>
        public void UpdateSysstaprize(int CID, int sysprizestatue, long sysprize)
        {
            dal.UpdateSysstaprize(CID, sysprizestatue, sysprize);
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

        /// <summary>
        /// ������ע��
        /// </summary>
        /// <param name="PayCount"></param>
        /// <param name="CID"></param>
        public void UpdatePayCount(int PayCount, int CID)
        {
            dal.UpdatePayCount(PayCount, CID);
        }

        public void updateNowprize(long nowprize, int CID)
        {
            dal.updateNowprize(nowprize, CID);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public void UpdateResult(int id, string Result)
        {
            dal.UpdateResult(id, Result);
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
        /// ����һ������
        /// </summary>
        public void Add(Model.BQCList model)
        {
            dal.Add(model);
        }
        /// <summary>
        /// ����һ������
        /// </summary>
        public void Update(Model.BQCList model)
        {
            dal.Update(model);
        }

        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateMatchs(Model.BQCList model)
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
        /// ɾ��һ������
        /// </summary>
        public void Delete(int id)
        {

            dal.Delete(id);
        }

        /// <summary>
        /// �õ����������ں�
        /// </summary>
        public int CID()
        {
            return dal.CID();
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
        /// �õ�һ������ʵ��
        /// </summary>
        public Model.BQCList GetBQCList(int id)
        {
            return dal.GetBQCList(id);
        }

        /// <summary>
        /// �����ںŵõ�һ������ʵ��
        /// </summary>
        /// <param name="cid"></param>
        /// <returns></returns>
        public Model.BQCList GetBQCListByCID(int cid)
        {
            return dal.GetBQCListByCID(cid);
        }
        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public BCW.BQC.Model.BQCList GetBQCList1(int CID)
        {
            return dal.GetBQCList1(CID);
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
        /// <returns>IList BQCList</returns>
        public IList<Model.BQCList> GetBQCLists(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetBQCLists(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }

        #endregion  ��Ա����
    }
}

