using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Baccarat.Model;
namespace BCW.Baccarat.BLL
{
    /// <summary>
    /// ҵ���߼���BJL_Room ��ժҪ˵����
    /// </summary>
    public class BJL_Room
    {
        private readonly BCW.Baccarat.DAL.BJL_Room dal = new BCW.Baccarat.DAL.BJL_Room();
        public BJL_Room()
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
        public int Add(BCW.Baccarat.Model.BJL_Room model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public void Update(BCW.Baccarat.Model.BJL_Room model)
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
        public BCW.Baccarat.Model.BJL_Room GetBJL_Room(int ID)
        {
            return dal.GetBJL_Room(ID);
        }

        /// <summary>
        /// �����ֶ�ȡ�����б�
        /// </summary>
        public DataSet GetList(string strField, string strWhere)
        {
            return dal.GetList(strField, strWhere);
        }

        //-------------------------------------------------
        public DataSet GetList2(string strField, string strWhere)
        {
            return dal.GetList2(strField, strWhere);
        }
        /// <summary>
        /// me_��ʼ��ĳ���ݱ�
        /// </summary>
        /// <param name="TableName">���ݱ�����</param>
        public void ClearTable(string TableName)
        {
            dal.ClearTable(TableName);
        }
        /// <summary>
        /// ����������
        /// </summary>
        public long GetPrice(string ziduan, string strWhere)
        {
            return dal.GetPrice(ziduan, strWhere);
        }
        /// <summary>
        ///  me_�����ֶ��޸������б�
        /// </summary>
        public DataSet update_zd(string strField, string strWhere)
        {
            return dal.update_zd(strField, strWhere);
        }
        /// <summary>
        /// me_����ĳ����Ĳʳ�
        /// </summary>
        public long Getcaichi(int RoomID)
        {
            return dal.Getcaichi(RoomID);
        }
        /// <summary>
        /// me_�õ���ׯ��
        /// </summary>
        public int Get_kz_num(int meid, int ID)
        {
            return dal.Get_kz_num(meid, ID);
        }
        /// <summary>
        /// me_�õ�һ������ʵ��
        /// </summary>
        public BCW.Baccarat.Model.BJL_Room GetBJL_Room(int ID, int meid)
        {

            return dal.GetBJL_Room(ID, meid);
        }
        //-------------------------------------------------

        /// <summary>
        /// ȡ��ÿҳ��¼
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">��ҳ��С</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <returns>IList BJL_Room</returns>
        public IList<BCW.Baccarat.Model.BJL_Room> GetBJL_Rooms(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
        {
            return dal.GetBJL_Rooms(p_pageIndex, p_pageSize, strWhere, strOrder, out p_recordCount);
        }

        #endregion  ��Ա����
    }
}

