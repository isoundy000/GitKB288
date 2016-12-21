using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.farm.Model;
namespace BCW.farm.BLL
{
    /// <summary>
    /// ҵ���߼���NC_daoju ��ժҪ˵����
    /// </summary>
    public class NC_daoju
    {
        private readonly BCW.farm.DAL.NC_daoju dal = new BCW.farm.DAL.NC_daoju();
        public NC_daoju()
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
        public int Add(BCW.farm.Model.NC_daoju model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public void Update(BCW.farm.Model.NC_daoju model)
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
        public BCW.farm.Model.NC_daoju GetNC_daoju(int ID)
        {

            return dal.GetNC_daoju(ID);
        }

        /// <summary>
        /// �����ֶ�ȡ�����б�
        /// </summary>
        public DataSet GetList(string strField, string strWhere)
        {
            return dal.GetList(strField, strWhere);
        }

        //=======================================
        /// <summary>
        /// �Ƿ���ڸü�¼
        /// //�����ж�-�Ƿ�Ϊ������߼�¼type=10
        /// </summary>
        public bool Exists(int ID)
        {
            return dal.Exists(ID);
        }
        /// <summary>
        /// �Ƿ���ڸü�¼
        /// //��̨�����жϸ�id�Ƿ����
        /// </summary>
        public bool Exists2(int ID)
        {
            return dal.Exists2(ID);
        }
        /// <summary>
        /// me_�Ƿ���ڸõ�������
        /// </summary>
        public bool Exists_djmc(string name)
        {
            return dal.Exists_djmc(name);
        }
        /// <summary>
        ///  me_�����ֶ��޸������б�
        /// </summary>
        public DataSet update_daoju(string strField, string strWhere)
        {
            return dal.update_daoju(strField, strWhere);
        }
        /// <summary>
        /// me_�õ�����ͼƬ·��
        /// </summary>
        public string Get_picture(int id)
        {
            return dal.Get_picture(id);
        }
        //=======================================


        /// <summary>
        /// ȡ��ÿҳ��¼
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">��ҳ��С</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <returns>IList NC_daoju</returns>
        public IList<BCW.farm.Model.NC_daoju> GetNC_daojus(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetNC_daojus(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }

        #endregion  ��Ա����
    }
}

