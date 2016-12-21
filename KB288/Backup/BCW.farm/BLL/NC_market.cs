using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.farm.Model;
namespace BCW.farm.BLL
{
    /// <summary>
    /// ҵ���߼���NC_market ��ժҪ˵����
    /// </summary>
    public class NC_market
    {
        private readonly BCW.farm.DAL.NC_market dal = new BCW.farm.DAL.NC_market();
        public NC_market()
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
        public int Add(BCW.farm.Model.NC_market model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public void Update(BCW.farm.Model.NC_market model)
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
        public BCW.farm.Model.NC_market GetNC_market(int ID)
        {

            return dal.GetNC_market(ID);
        }

        /// <summary>
        /// �����ֶ�ȡ�����б�
        /// </summary>
        public DataSet GetList(string strField, string strWhere)
        {
            return dal.GetList(strField, strWhere);
        }

        //===============================

        /// <summary>
        /// me_�õ���̯����
        /// </summary>
        public long Get_btcs(int usid)
        {
            return dal.Get_btcs(usid);
        }
        /// <summary>
        /// me_�Ƿ���ڰ�̯��¼
        /// </summary>
        public bool Exists_baitan(int id, int UsID)
        {
            return dal.Exists_baitan(id, UsID);
        }
        /// <summary>
        /// me_�Ƿ���ڰ�̯��¼
        /// </summary>
        public bool Exists_baitan(int id)
        {
            return dal.Exists_baitan(id);
        }
        /// <summary>
        ///  me_�����ֶ��޸������б�
        /// </summary>
        public DataSet update_market(string strField, string strWhere)
        {
            return dal.update_market(strField, strWhere);
        }
        /// <summary>
        /// me_����̯λ���ߵ�����
        /// </summary>
        public void Update_twdj(int usid, int num, int huafei_id)
        {
            dal.Update_twdj(usid, num, huafei_id);
        }
        /// <summary>
        /// me_�жϵ����Ƿ�Ϊ0
        /// </summary>
        public BCW.farm.Model.NC_market Get_djsl(int meid, int huafei_id)
        {

            return dal.Get_djsl(meid, huafei_id);
        }
        //===============================


        /// <summary>
        /// ȡ��ÿҳ��¼
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">��ҳ��С</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <returns>IList NC_market</returns>
        public IList<BCW.farm.Model.NC_market> GetNC_markets(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
        {
            return dal.GetNC_markets(p_pageIndex, p_pageSize, strWhere, strOrder, out p_recordCount);
        }

        #endregion  ��Ա����
    }
}

