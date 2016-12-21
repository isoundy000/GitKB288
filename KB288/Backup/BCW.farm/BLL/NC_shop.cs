using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.farm.Model;
namespace BCW.farm.BLL
{
    /// <summary>
    /// ҵ���߼���NC_shop ��ժҪ˵����
    /// </summary>
    public class NC_shop
    {
        private readonly BCW.farm.DAL.NC_shop dal = new BCW.farm.DAL.NC_shop();
        public NC_shop()
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
        public int Add(BCW.farm.Model.NC_shop model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public void Update(BCW.farm.Model.NC_shop model)
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
        public BCW.farm.Model.NC_shop GetNC_shop(int ID)
        {

            return dal.GetNC_shop(ID);
        }

        /// <summary>
        /// �����ֶ�ȡ�����б�
        /// </summary>
        public DataSet GetList(string strField, string strWhere)
        {
            return dal.GetList(strField, strWhere);
        }

        //================================
        /// <summary>
        /// me_����õ�һ������ʵ��
        /// </summary>
        public BCW.farm.Model.NC_shop Getsd_suiji(int ID)
        {
            return dal.Getsd_suiji(ID);
        }
        /// <summary>
        /// me_����usid��ѯ��Ǯ
        /// </summary>
        public long get_usergoid(int meid)
        {
            return dal.get_usergoid(meid);
        }
        /// <summary>
        /// me_�������Ͳ�ѯ����
        /// </summary>
        public long get_typenum(int type)
        {
            return dal.get_typenum(type);
        }
        /// <summary>
        /// me_����name_id�õ�һ������ʵ��
        /// </summary>
        public BCW.farm.Model.NC_shop GetNC_shop1(int ID)
        {
            return dal.GetNC_shop1(ID);
        }
        /// <summary>
        /// me_����name�õ�һ������ʵ��
        /// </summary>
        public BCW.farm.Model.NC_shop GetNC_shop2(string name)
        {

            return dal.GetNC_shop2(name);
        }

        /// <summary>
        /// me_�Ƿ���ڸ�����ID
        /// //ǰ̨�����ж�
        /// </summary>
        public bool Exists_zzid(int ID)
        {
            return dal.Exists_zzid(ID);
        }
        /// <summary>
        /// me_�Ƿ���ڸ�����ID
        /// //��̨�����ж�
        /// </summary>
        public bool Exists_zzid2(int ID)
        {
            return dal.Exists_zzid2(ID);
        }
        /// <summary>
        /// me_�Ƿ���ڸ���������
        /// </summary>
        public bool Exists_zzmc(string name)
        {
            return dal.Exists_zzmc(name);
        }
        /// <summary>
        /// me_�õ����һ������ʵ��
        /// </summary>
        public BCW.farm.Model.NC_shop GetNC_shop_last(int ID)
        {

            return dal.GetNC_shop_last(ID);
        }
        /// <summary>
        ///  me_�����ֶ��޸������б�
        /// </summary>
        public DataSet update_shop(string strField, string strWhere)
        {
            return dal.update_shop(strField, strWhere);
        }


        /// <summary>
        /// me_ȡ��ÿҳ��¼
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">��ҳ��С</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <returns>IList NC_shop</returns>
        public IList<BCW.farm.Model.NC_shop> GetNC_shops(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
        {
            return dal.GetNC_shops(p_pageIndex, p_pageSize, strWhere, strOrder, out p_recordCount);
        }


        #endregion  ��Ա����
    }
}

