using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.farm.Model;
namespace BCW.farm.BLL
{
    /// <summary>
    /// ҵ���߼���NC_user ��ժҪ˵����
    /// </summary>
    public class NC_user
    {
        private readonly BCW.farm.DAL.NC_user dal = new BCW.farm.DAL.NC_user();
        public NC_user()
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
        /// ����һ������
        /// </summary>
        public int Add(BCW.farm.Model.NC_user model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public void Update(BCW.farm.Model.NC_user model)
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
        public BCW.farm.Model.NC_user GetNC_user(int ID)
        {

            return dal.GetNC_user(ID);
        }

        /// <summary>
        /// �����ֶ�ȡ�����б�
        /// </summary>
        public DataSet GetList(string strField, string strWhere)
        {
            return dal.GetList(strField, strWhere);
        }

        //====================================
        /// <summary>
        ///  me_�����ֶ��޸������б�
        /// </summary>
        public DataSet update_zd(string strField, string strWhere)
        {
            return dal.update_zd(strField, strWhere);
        }
        /// <summary>
        /// me_������������
        /// </summary>
        public void Update_tdlx(int usid, int tuditype)
        {
            dal.Update_tdlx(usid, tuditype);
        }
        /// <summary>
        /// me_����һ������
        /// </summary>
        public int Add_1(BCW.farm.Model.NC_user model)
        {
            return dal.Add_1(model);
        }
        /// <summary>
        /// me_�õ�һ������ʵ��
        /// </summary>
        public BCW.farm.Model.NC_user Get_user(int usid)
        {

            return dal.Get_user(usid);
        }
        /// <summary>
        /// me_�Ƿ���ڸü�¼
        /// </summary>
        public bool Exists(int ID)
        {
            return dal.Exists(ID);
        }
        /// <summary>
        /// me_�����û�����
        /// </summary>
        public void Update_Experience(int usid, long experience)
        {
            dal.Update_Experience(usid, experience);
        }
        /// <summary>
        /// me_����ʩ�ʴ���
        /// </summary>
        public void Update_shifeinum(int usid, int num)
        {
            dal.Update_shifeinum(usid, num);
        }
        /// <summary>
        /// me_�����û��ȼ�������
        /// </summary>
        public void Update_dengji(int usid)
        {
            dal.Update_dengji(usid);
        }
        /// <summary>
        /// me_�����û��ȼ�������
        /// </summary>
        public void Update_dengji2(int usid)
        {
            dal.Update_dengji2(usid);
        }
        /// <summary>
        /// me_�����û����
        /// </summary>
        public void Update_jinbi(int usid, long iGoid)
        {
            dal.Update_jinbi(usid, iGoid);
        }
        /// <summary>
        /// me_�õ��û���
        /// </summary>
        public long GetGold(int usid)
        {
            return dal.GetGold(usid);
        }
        /// <summary>
        /// me_�õ�ʹ�û��ʴ���
        /// </summary>
        public int Get_hfnum(int usid)
        {
            return dal.Get_hfnum(usid);
        }
        /// <summary>
        /// me_�õ��ֲݷų���� �۹��� 20160826
        /// </summary>
        public int Get_zcfcnum(int usid)
        {
            return dal.Get_zcfcnum(usid);
        }
        /// <summary>
        /// me_�õ����ݳ������ �۹��� 20160826
        /// </summary>
        public int Get_ccccnum(int usid)
        {
            return dal.Get_ccccnum(usid);
        }
        /// <summary>
        /// me_�õ��Ƿ���԰�̯
        /// </summary>
        public long Get_baitang(int usid)
        {
            return dal.Get_baitang(usid);
        }
        /// <summary>
        /// me_�õ��ȼ�
        /// </summary>
        public long GetGrade(int usid)
        {
            return dal.GetGrade(usid);
        }
        /// <summary>
        /// me_�õ��ո�״̬
        /// </summary>
        public int Getshoutype(int usid)
        {
            return dal.Getshoutype(usid);
        }
        /// <summary>
        /// me_�õ�����
        /// </summary>
        public long Getjingyan(int usid)
        {
            return dal.Getjingyan(usid);
        }
        /// <summary>
        /// me_�õ���æ�ľ���
        /// </summary>
        public long Get_bmjingyan(int usid)
        {
            return dal.Get_bmjingyan(usid);
        }
        /// <summary>
        /// me_�õ����ֵľ���
        /// </summary>
        public long Get_bzjingyan(int usid)
        {
            return dal.Get_bzjingyan(usid);
        }
        /// <summary>
        /// me_�õ�ʹ���ľ���
        /// </summary>
        public long Get_shjingyan(int usid)
        {
            return dal.Get_shjingyan(usid);
        }
        /// <summary>
        /// me_�õ��Լ������ľ���
        /// </summary>
        public long Get_zjjingyan(int usid)
        {
            return dal.Get_zjjingyan(usid);
        }
        /// <summary>
        ///  me_�õ�ũ������
        /// </summary>
        public string Get_jiyu(int usid)
        {
            return dal.Get_jiyu(usid);
        }
        /// <summary>
        /// me_�Ƿ�һ������
        /// </summary>
        public long Getchucao(int usid)
        {
            return dal.Getchucao(usid);
        }
        /// <summary>
        /// me_�Ƿ�һ����ˮ
        /// </summary>
        public long Getjiaoshui(int usid)
        {
            return dal.Getjiaoshui(usid);
        }
        /// <summary>
        /// me_�Ƿ�һ������
        /// </summary>
        public long Getchuchong(int usid)
        {
            return dal.Getchuchong(usid);
        }
        /// <summary>
        /// me_��ͨһ������
        /// </summary>
        public void Update_chucao_1(int usid)
        {
            dal.Update_chucao_1(usid);
        }
        /// <summary>
        /// me_��ͨһ����ˮ
        /// </summary>
        public void Update_jiaoshui_1(int usid)
        {
            dal.Update_jiaoshui_1(usid);
        }
        /// <summary>
        /// me_��ͨһ������
        /// </summary>
        public void Update_chuchong_1(int usid)
        {
            dal.Update_chuchong_1(usid);
        }
        /// <summary>
        /// me_�Ƿ�һ���ջ�
        /// </summary>
        public long Getshou(int usid)
        {
            return dal.Getshou(usid);
        }
        /// <summary>
        /// me_��ͨһ���ջ�
        /// </summary>
        public void Update_shouhuo_1(int usid)
        {
            dal.Update_shouhuo_1(usid);
        }
        /// <summary>
        /// me_�Ƿ�һ������
        /// </summary>
        public long Getchandi(int usid)
        {
            return dal.Getchandi(usid);
        }
        /// <summary>
        /// me_��ͨһ������
        /// </summary>
        public void Update_chandi_1(int usid)
        {
            dal.Update_chandi_1(usid);
        }
        /// <summary>
        /// me_�Ƿ�һ��ʩ��
        /// </summary>
        public long Getshifei(int usid)
        {
            return dal.Getshifei(usid);
        }
        /// <summary>
        /// me_��ͨһ��ʩ��
        /// </summary>
        public void Update_shifei_1(int usid)
        {
            dal.Update_shifei_1(usid);
        }
        /// <summary>
        /// me_���½�Ҳ���¼���Ѽ�¼
        /// </summary>
        public void UpdateiGold(int ID, string UsName, long iGold, string AcText, int BbTag)
        {
            //�����û������
            dal.Update_jinbi(ID, iGold);
            //�������Ѽ�¼
            BCW.farm.Model.NC_Goldlog model = new BCW.farm.Model.NC_Goldlog();
            model.BbTag = BbTag;
            model.Types = 30;//ũ��type30
            model.PUrl = Utils.getPageUrl();//�������ļ���
            model.UsId = ID;
            model.UsName = UsName;
            model.AcGold = iGold;
            model.AfterGold = GetGold(ID);//���º�ı���
            model.AcText = AcText;
            model.AddTime = DateTime.Now;
            new BCW.farm.BLL.NC_Goldlog().Add(model);
        }
        /// <summary>
        /// me_�õ�ǩ����Ϣ
        /// </summary>
        public BCW.farm.Model.NC_user GetSignData(int ID)
        {
            return dal.GetSignData(ID);
        }
        /// <summary>
        /// me_����ǩ����Ϣ
        /// </summary>
        public void UpdateSingData(int ID, int SignTotal, int SignKeep)
        {
            dal.UpdateSingData(ID, SignTotal, SignKeep);
        }
        /// <summary>
        /// me_��Ա���а�ʹ��
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">��ҳ��С</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <param name="strOrder">��������</param>
        /// <returns>IList User</returns>
        public IList<BCW.farm.Model.NC_user> GetUsers(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
        {
            return dal.GetUsers(p_pageIndex, p_pageSize, strWhere, strOrder, out p_recordCount);
        }

        /// <summary>
        /// me_ȡ��ÿҳ��¼
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">��ҳ��С</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <returns>IList NC_user</returns>
        public IList<BCW.farm.Model.NC_user> GetNC_users(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
        {
            return dal.GetNC_users(p_pageIndex, p_pageSize, strWhere, strOrder, out p_recordCount);
        }

        #endregion  ��Ա����


        //====================================
    }
}

