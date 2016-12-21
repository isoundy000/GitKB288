using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.farm.Model;
namespace BCW.farm.BLL
{
    /// <summary>
    /// ҵ���߼���NC_hecheng ��ժҪ˵����
    /// </summary>
    public class NC_hecheng
    {
        private readonly BCW.farm.DAL.NC_hecheng dal = new BCW.farm.DAL.NC_hecheng();
        public NC_hecheng()
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
        public int Add(BCW.farm.Model.NC_hecheng model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public void Update(BCW.farm.Model.NC_hecheng model)
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
        public BCW.farm.Model.NC_hecheng GetNC_hecheng(int ID)
        {
            return dal.GetNC_hecheng(ID);
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
        /// me_�Ƿ���ڸü�¼
        /// </summary>
        public bool Exists_ID(int ID, int meid)
        {
            return dal.Exists_ID(ID, meid);
        }
        /// <summary>
        ///  me_�����ֶ��޸������б�
        /// </summary>
        public DataSet update_ID(string strField, string strWhere)
        {
            return dal.update_ID(strField, strWhere);
        }
        /// <summary>
        /// me_���¹�ʵ����
        /// </summary>
        public void Update_gs(int usid, int num, int name_id)
        {
            dal.Update_gs(usid, num, name_id);
        }
        /// <summary>
        /// me_�õ���������
        /// </summary>
        public int Get_daoju_num(int meid, int ID)
        {
            return dal.Get_daoju_num(meid, ID);
        }
        /// me_�Ƿ���ڸ�����2
        /// </summary>
        public bool Exists_zuowu2(int id, int meid)
        {
            return dal.Exists_zuowu2(id, meid);
        }
        /// <summary>
        /// me_�õ�һ������ʵ��
        /// </summary>
        public BCW.farm.Model.NC_hecheng GetNC_hecheng2(int ID, int meid)
        {
            return dal.GetNC_hecheng2(ID, meid);
        }
        //==================================

        /// <summary>
        /// ȡ��ÿҳ��¼
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">��ҳ��С</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <returns>IList NC_hecheng</returns>
        public IList<BCW.farm.Model.NC_hecheng> GetNC_hechengs(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
        {
            return dal.GetNC_hechengs(p_pageIndex, p_pageSize, strWhere, strOrder, out p_recordCount);
        }

        #endregion  ��Ա����
    }
}

