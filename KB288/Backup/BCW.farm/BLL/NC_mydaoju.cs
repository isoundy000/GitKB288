using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.farm.Model;
namespace BCW.farm.BLL
{
    /// <summary>
    /// ҵ���߼���NC_mydaoju ��ժҪ˵����
    /// </summary>
    public class NC_mydaoju
    {
        private readonly BCW.farm.DAL.NC_mydaoju dal = new BCW.farm.DAL.NC_mydaoju();
        public NC_mydaoju()
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
        public int Add(BCW.farm.Model.NC_mydaoju model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public void Update(BCW.farm.Model.NC_mydaoju model)
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
        public BCW.farm.Model.NC_mydaoju GetNC_mydaoju(int ID)
        {

            return dal.GetNC_mydaoju(ID);
        }

        /// <summary>
        /// �����ֶ�ȡ�����б�
        /// </summary>
        public DataSet GetList(string strField, string strWhere)
        {
            return dal.GetList(strField, strWhere);
        }


        //===================================
        /// <summary>
        /// me_ɾ��һ������
        /// </summary>
        public void Delete(int ID, int usid)
        {

            dal.Delete(ID, usid);
        }
        /// <summary>
        /// me_ɾ��һ������
        /// </summary>
        public void Delete2(int ID, int usid)
        {

            dal.Delete2(ID, usid);
        }
        /// <summary>
        /// me_�����ֶ�ȡ�����б�
        /// </summary>
        public DataSet GetList2(string strField, string strWhere)
        {
            return dal.GetList2(strField, strWhere);
        }
        /// <summary>
        /// me_�Ƿ���ڸü�¼
        /// </summary>
        public bool Exists(int ID, int meid)
        {
            return dal.Exists(ID, meid);
        }
        /// <summary>
        /// me_�Ƿ���ڸü�¼
        /// </summary>
        public bool Exists2(int ID, int meid)
        {
            return dal.Exists2(ID, meid);
        }
        /// <summary>
        /// me_����name_id�õ�һ������ʵ��
        /// </summary>
        public BCW.farm.Model.NC_mydaoju Getname_id(int meid, int ID)
        {
            return dal.Getname_id(meid, ID);
        }
        /// <summary>
        /// me_�õ���������
        /// </summary>
        public int Get_daoju_num(int meid, int ID)
        {
            return dal.Get_daoju_num(meid, ID);
        }
        /// <summary>
        /// me_�õ���������
        /// </summary>
        public int Get_daojunum2(int meid, int ID, int zs)
        {
            return dal.Get_daojunum2(meid, ID, zs);
        }
        /// <summary>
        /// me_�õ��Ƿ�����
        /// </summary>
        public int Get_suoding(int meid, int ID)
        {
            return dal.Get_suoding(meid, ID);
        }
        /// <summary>
        /// me_������ֲ������
        /// </summary>
        public void Update_zz(int usid, int num, int name_id)
        {
            dal.Update_zz(usid, num, name_id);
        }
        /// <summary>
        /// me_������ֲ������
        /// </summary>
        public void Update_zz2(int usid, int num, int name_id)
        {
            dal.Update_zz2(usid, num, name_id);
        }
        /// <summary>
        /// me_�Ƿ���ڻ��ʸü�¼
        /// </summary>
        public bool Exists_hf(int ID, int usid, int zs)
        {
            return dal.Exists_hf(ID, usid, zs);
        }
        /// <summary>
        /// me_�Ƿ���ڻ��ʸü�¼
        /// </summary>
        public bool Exists_hf(int ID, int usid)
        {
            return dal.Exists_hf(ID, usid);
        }
        /// <summary>
        /// me_�Ƿ���ڻ��ʸü�¼2
        /// </summary>
        public bool Exists_hf2(int ID, int usid, int zs)
        {
            return dal.Exists_hf2(ID, usid, zs);
        }
        /// <summary>
        /// me_��ѯ�Ƿ��е���(�����ͺͲ�������)
        /// </summary>
        public bool Exists_hf3(int ID, int usid)
        {
            return dal.Exists_hf3(ID, usid);
        }
        /// <summary>
        /// me_�Ƿ������Կ��
        /// </summary>
        public BCW.farm.Model.NC_mydaoju Get_yys(int meid, int huafei_id)
        {
            return dal.Get_yys(meid, huafei_id);
        }
        /// <summary>
        /// me_�Ƿ���ڱ���Կ��
        /// </summary>
        public BCW.farm.Model.NC_mydaoju Get_bxys(int meid, int huafei_id)
        {

            return dal.Get_bxys(meid, huafei_id);
        }
        /// <summary>
        /// me_����huafei_id�õ�һ������ʵ��
        /// </summary>
        public BCW.farm.Model.NC_mydaoju Gethf_id(int meid, int ID, int zs)
        {
            return dal.Gethf_id(meid, ID, zs);
        }
        /// <summary>
        /// me_���»�������
        /// </summary>
        public void Update_hf(int usid, int num, int huafei_id, int zs)
        {
            dal.Update_hf(usid, num, huafei_id, zs);
        }
        /// <summary>
        /// me_���»�������
        /// </summary>
        public void Update_hf2(int usid, int num, int huafei_id)
        {
            dal.Update_hf2(usid, num, huafei_id);
        }
        /// <summary>
        /// me_�Ƿ����������
        /// </summary>
        public bool Exists_zz(int ID)
        {
            return dal.Exists_zz(ID);
        }
        /// <summary>
        /// me_����
        /// </summary>
        public void Update_sd(int usid, int name_id)
        {
            dal.Update_sd(usid, name_id);
        }
        /// <summary>
        /// me_����
        /// </summary>
        public void Update_jiesuo(int meid, int id)
        {
            dal.Update_jiesuo(meid, id);
        }

        /// <summary>
        /// me_ȡ��ÿҳ��¼2
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">��ҳ��С</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <returns>IList NC_mydaoju</returns>
        public IList<BCW.farm.Model.NC_mydaoju> GetNC_mydaojus2(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
        {
            return dal.GetNC_mydaojus2(p_pageIndex, p_pageSize, strWhere, strOrder, out p_recordCount);
        }

        /// <summary>
        /// me_ȡ��ÿҳ��¼
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">��ҳ��С</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <returns>IList NC_mydaoju</returns>
        public IList<BCW.farm.Model.NC_mydaoju> GetNC_mydaojus(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetNC_mydaojus(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }


        #endregion  ��Ա����

        //===================================

    }
}

