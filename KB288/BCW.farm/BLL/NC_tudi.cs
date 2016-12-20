using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.farm.Model;
namespace BCW.farm.BLL
{
    /// <summary>
    /// ҵ���߼���NC_tudi ��ժҪ˵����
    /// </summary>
    public class NC_tudi
    {
        private readonly BCW.farm.DAL.NC_tudi dal = new BCW.farm.DAL.NC_tudi();
        public NC_tudi()
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
        public int Add(BCW.farm.Model.NC_tudi model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public void Update(BCW.farm.Model.NC_tudi model)
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
        public BCW.farm.Model.NC_tudi GetNC_tudi(int ID)
        {

            return dal.GetNC_tudi(ID);
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
        /// me_�����ֶ�ȡ�����б�
        /// </summary>
        public DataSet GetList(string strField, string strname, string strWhere)
        {
            return dal.GetList(strField, strname, strWhere);
        }
        /// <summary>
        /// me_�������Ƿ������
        /// </summary>
        public bool Exists_jin(int meid, int tudi)
        {
            return dal.Exists_jin(meid, tudi);
        }
        /// <summary>
        /// me_�������Ƿ������
        /// </summary>
        public bool Exists_hei(int meid, int tudi)
        {
            return dal.Exists_hei(meid, tudi);
        }
        /// <summary>
        /// me_�������Ƿ������
        /// </summary>
        public bool Exists_hong(int meid, int tudi)
        {
            return dal.Exists_hong(meid, tudi);
        }
        /// <summary>
        /// me_����usid��ѯ�м��������
        /// </summary>
        public BCW.farm.Model.NC_tudi Get_htd(int usid)
        {
            return dal.Get_htd(usid);
        }
        /// <summary>
        /// me_����usid��ѯ�м��������
        /// </summary>
        public BCW.farm.Model.NC_tudi Get_heitd(int usid)
        {
            return dal.Get_heitd(usid);
        }
        /// <summary>
        /// me_����usid��ѯ�м��������
        /// </summary>
        public BCW.farm.Model.NC_tudi Get_jtd(int usid)
        {
            return dal.Get_jtd(usid);
        }
        /// <summary>
        /// me_����һ������
        /// </summary>
        public void Update_1(BCW.farm.Model.NC_tudi model)
        {
            dal.Update_1(model);
        }
        /// <summary>
        /// me_����usid�õ�һ������ʵ��
        /// </summary>
        public BCW.farm.Model.NC_tudi Getusid(int usid)
        {
            return dal.Getusid(usid);
        }
        /// <summary>
        /// me_����usid��ѯ�м�������
        /// </summary>
        public BCW.farm.Model.NC_tudi Get_tudinum(int usid)
        {
            return dal.Get_tudinum(usid);
        }
        /// <summary>
        /// me_����usid��ѯ�м�������
        /// </summary>
        public long Get_xianjing(int usid)
        {
            return dal.Get_xianjing(usid);
        }
        /// <summary>
        ///  me_�����ֶ��޸������б�
        /// </summary>
        public DataSet update_tudi(string strField, string strWhere)
        {
            return dal.update_tudi(strField, strWhere);
        }
        /// <summary>
        /// me_�Ƿ�������س��ݼ�¼
        /// </summary>
        public bool Exists_chucao(int tudi, int UsID)
        {
            return dal.Exists_chucao(tudi, UsID);
        }
        /// <summary>
        /// me_�ж������Ƿ���ڿ��ֲ�
        /// </summary>
        public bool Exists_zhongcao(int tudi, int UsID)
        {
            return dal.Exists_zhongcao(tudi, UsID);
        }
        /// <summary>
        /// me_�Ƿ�������س��ݼ�¼_һ��
        /// </summary>
        public bool Exists_chucao_1(int usid)
        {
            return dal.Exists_chucao_1(usid);
        }
        /// <summary>
        /// me_�Ƿ�������ؽ�ˮ��¼
        /// </summary>
        public bool Exists_jiaoshui(int tudi, int UsID)
        {
            return dal.Exists_jiaoshui(tudi, UsID);
        }
        /// <summary>
        /// me_�Ƿ�������ؽ�ˮ��¼_һ��
        /// </summary>
        public bool Exists_jiaoshui_1(int usid)
        {
            return dal.Exists_jiaoshui_1(usid);
        }
        /// <summary>
        /// me_�Ƿ�������س����¼
        /// </summary>
        public bool Exists_chuchong(int tudi, int UsID)
        {
            return dal.Exists_chuchong(tudi, UsID);
        }
        /// <summary>
        /// me_�Ƿ�������س����¼_һ��
        /// </summary>
        public bool Exists_chuchong_1(int usid)
        {
            return dal.Exists_chuchong_1(usid);
        }
        /// <summary>
        /// me_�Ƿ����һ���ջ��¼_һ��
        /// </summary>
        public bool Exists_shouhuo_1(int usid)
        {
            return dal.Exists_shouhuo_1(usid);
        }
        /// <summary>
        /// me_��ѯ�м������ؿ���͵ȡ
        /// </summary>
        public BCW.farm.Model.NC_tudi tou_tudinum1(int usid)
        {
            return dal.tou_tudinum1(usid);
        }
        /// <summary>
        /// me_��ѯ��͵�������ؿ���
        /// </summary>
        public BCW.farm.Model.NC_tudi tou_tudinum2(int usid, int meid_usid)
        {
            return dal.tou_tudinum2(usid, meid_usid);
        }
        /// <summary>
        /// me_��ѯ�м������ؿ���(͵)����
        /// </summary>
        public BCW.farm.Model.NC_tudi cao_tudinum1(int usid)
        {
            return dal.cao_tudinum1(usid);
        }
        /// <summary>
        /// me_��ѯ�м������ؿ���(͵)��ˮ
        /// </summary>
        public BCW.farm.Model.NC_tudi shui_tudinum1(int usid)
        {
            return dal.shui_tudinum1(usid);
        }
        /// <summary>
        /// me_��ѯ�м������ؿ���(͵)����
        /// </summary>
        public BCW.farm.Model.NC_tudi chong_tudinum1(int usid)
        {
            return dal.chong_tudinum1(usid);
        }
        /// <summary>
        /// me_��ѯ�м������ؿ���(͵)�Ų�
        /// </summary>
        public BCW.farm.Model.NC_tudi fangcao_num1(int usid)
        {
            return dal.fangcao_num1(usid);
        }
        /// <summary>
        /// me_��ѯ�м������ؿ���(͵)�ų�
        /// </summary>
        public BCW.farm.Model.NC_tudi fangcao_num2(int usid)
        {
            return dal.fangcao_num2(usid);
        }
        /// <summary>
        /// me_�Ƿ�������ؼ�¼
        /// </summary>
        public bool Exists_tudi(int tudi, int UsID)
        {
            return dal.Exists_tudi(tudi, UsID);
        }
        /// <summary>
        /// me_�Ƿ����һ�����ؼ�¼_һ��
        /// </summary>
        public bool Exists_chandi_1(int usid)
        {
            return dal.Exists_chandi_1(usid);
        }
        /// <summary>
        /// me_�ж��Ƿ��Լ��ֵĲ�
        /// </summary>
        public bool Exists_zcao(int tudi, int UsID, int meid)
        {
            return dal.Exists_zcao(tudi, UsID, meid);
        }
        /// <summary>
        /// me_�ж��Ƿ��Լ��ŵĳ�
        /// </summary>
        public bool Exists_zchong(int tudi, int UsID, int meid)
        {
            return dal.Exists_zchong(tudi, UsID, meid);
        }
        /// <summary>
        /// me_�Ƿ�������ؿɲ��ؼ�¼
        /// </summary>
        public bool Exists_chandi(int tudi, int UsID)
        {
            return dal.Exists_chandi(tudi, UsID);
        }
        /// <summary>
        /// me_�Ƿ����һ�����ּ�¼_һ��
        /// </summary>
        public bool Exists_bozhong_1(int usid)
        {
            return dal.Exists_bozhong_1(usid);
        }
        /// <summary>
        /// me_�Ƿ��������ʩ�ʼ�¼_һ��
        /// </summary>
        public bool Exists_shifei_1(int usid)
        {
            return dal.Exists_shifei_1(usid);
        }
        /// <summary>
        /// me_�Ƿ�������ؿ�ʩ�ʼ�¼
        /// </summary>
        public bool Exists_shifei(int tudi, int UsID)
        {
            return dal.Exists_shifei(tudi, UsID);
        }
        /// <summary>
        /// me_�Ƿ��������
        /// </summary>
        public bool Exists_xianjing(int tudi, int UsID)
        {
            return dal.Exists_xianjing(tudi, UsID);
        }
        /// <summary>
        /// me_�Ƿ��������
        /// </summary>
        public bool Exists_xianjing2(int tudi, int UsID)
        {
            return dal.Exists_xianjing2(tudi, UsID);
        }
        /// <summary>
        /// me_����usid��ѯ�м�����Բ��ֵ�����
        /// </summary>
        public BCW.farm.Model.NC_tudi Get_tudinum_bz(int usid)
        {
            return dal.Get_tudinum_bz(usid);
        }
        /// <summary>
        /// me_�Ƿ���ڸ�ID�ĸ����ؿ���ֲ
        /// </summary>
        public bool Exists_zhongzhi(int tudi, int UsID)
        {
            return dal.Exists_zhongzhi(tudi, UsID);
        }
        /// <summary>
        /// me_�Ƿ���ڸ�ID�ĸ����ؿ����ջ�
        /// </summary>
        public bool Exists_shouhuo(int tudi, int UsID)
        {
            return dal.Exists_shouhuo(tudi, UsID);
        }
        /// <summary>
        /// me_����usid�����صõ�һ������ʵ��
        /// </summary>
        public BCW.farm.Model.NC_tudi Get_td(int usid, int tudi)
        {
            return dal.Get_td(usid, tudi);
        }


        /// <summary>
        /// me_ȡ��ÿҳ��¼
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">��ҳ��С</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <returns>IList NC_tudi</returns>
        public IList<BCW.farm.Model.NC_tudi> GetNC_tudis(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
        {
            return dal.GetNC_tudis(p_pageIndex, p_pageSize, strWhere, strOrder, out p_recordCount);
        }
        //====================================
        #endregion  ��Ա����
    }
}

