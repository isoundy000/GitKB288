using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.farm.Model;
namespace BCW.farm.BLL
{
    /// <summary>
    /// ҵ���߼���NC_GetCrop ��ժҪ˵����
    /// </summary>
    public class NC_GetCrop
    {
        private readonly BCW.farm.DAL.NC_GetCrop dal = new BCW.farm.DAL.NC_GetCrop();
        public NC_GetCrop()
        { }
        #region  ��Ա����
        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool Exists(long id)
        {
            return dal.Exists(id);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public int Add(BCW.farm.Model.NC_GetCrop model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public void Update(BCW.farm.Model.NC_GetCrop model)
        {
            dal.Update(model);
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public void Delete(long id)
        {

            dal.Delete(id);
        }

        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public BCW.farm.Model.NC_GetCrop GetNC_GetCrop(long id)
        {

            return dal.GetNC_GetCrop(id);
        }

        /// <summary>
        /// �����ֶ�ȡ�����б�
        /// </summary>
        public DataSet GetList(string strField, string strWhere)
        {
            return dal.GetList(strField, strWhere);
        }

        /// <summary>
        /// �����ֶ�ȡ�����б�2
        /// </summary>
        public DataSet GetList2(string strField, string strWhere)
        {
            return dal.GetList2(strField, strWhere);
        }

        //==============================
        /// <summary>
        /// me_����name_id��usidɾ��һ������
        /// </summary>
        public void Delete(int id, int usid)
        {

            dal.Delete(id, usid);
        }
        /// me_�Ƿ���ڸ�����
		/// </summary>
		public bool Exists_zuowu(string name, int usid)
        {
            return dal.Exists_zuowu(name, usid);
        }
        /// <summary>
        /// me_����һ����������
        /// </summary>
        public void Update1(BCW.farm.Model.NC_GetCrop model)
        {
            dal.Update1(model);
        }
        /// me_�Ƿ���ڸ�����2
        /// </summary>
        public bool Exists_zuowu2(int id, int meid)
        {
            return dal.Exists_zuowu2(id, meid);
        }
        /// me_�Ƿ���ڸ�����3
        /// </summary>
        public bool Exists_zuowu3(int id, int meid)
        {
            return dal.Exists_zuowu3(id, meid);
        }
        /// <summary>
        /// me_����
        /// </summary>
        public void Update_suoding(int meid, int id)
        {
            dal.Update_suoding(meid, id);
        }
        /// <summary>
        /// me_����
        /// </summary>
        public void Update_jiesuo(int meid, int id)
        {
            dal.Update_jiesuo(meid, id);
        }
        /// <summary>
        /// me_�õ��ܼ�Ǯ
        /// </summary>
        public long Get_allprice(int usid)
        {
            return dal.Get_allprice(usid);
        }
        /// <summary>
        /// me_����
        /// </summary>
        public void Update_maichu(int meid, int num, int id)
        {
            dal.Update_maichu(meid, num, id);
        }
        /// <summary>
        /// me_�õ�һ������ʵ��
        /// </summary>
        public BCW.farm.Model.NC_GetCrop GetNC_GetCrop2(int id, int usid)
        {

            return dal.GetNC_GetCrop2(id, usid);
        }
        /// <summary>
        /// me_���¹�ʵ����
        /// </summary>
        public void Update_gs(int usid, int num, int name_id)
        {
            dal.Update_gs(usid, num, name_id);
        }
        /// <summary>
        /// me_���¹�ʵ����
        /// </summary>
        public void Update_gs2(int usid, int num, int name_id)
        {
            dal.Update_gs2(usid, num, name_id);
        }

        /// <summary>
        /// me_ȡ��ÿҳ��¼
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">��ҳ��С</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <returns>IList NC_GetCrop</returns>
        public IList<BCW.farm.Model.NC_GetCrop> GetNC_GetCrops(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
        {
            return dal.GetNC_GetCrops(p_pageIndex, p_pageSize, strWhere, strOrder, out p_recordCount);
        }

        #endregion  ��Ա����

        //==============================
    }
}

