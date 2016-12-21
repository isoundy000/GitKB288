using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Baccarat.Model;
namespace BCW.Baccarat.BLL
{
    /// <summary>
    /// ҵ���߼���BJL_Play ��ժҪ˵����
    /// </summary>
    public class BJL_Play
    {
        private readonly BCW.Baccarat.DAL.BJL_Play dal = new BCW.Baccarat.DAL.BJL_Play();
        public BJL_Play()
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
        public int Add(BCW.Baccarat.Model.BJL_Play model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public void Update(BCW.Baccarat.Model.BJL_Play model)
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
        /// ɾ��һ������
        /// </summary>
        public void Delete(string strWhere)
        {
            dal.Delete(strWhere);
        }
        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public BCW.Baccarat.Model.BJL_Play GetBJL_Play(int ID)
        {
            return dal.GetBJL_Play(ID);
        }
        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public BCW.Baccarat.Model.BJL_Play GetBJL_Play3(int ID)
        {
            return dal.GetBJL_Play3(ID);
        }
        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public BCW.Baccarat.Model.BJL_Play GetBJL_Play2(int ID)
        {

            return dal.GetBJL_Play2(ID);
        }

        /// <summary>
        /// �����ֶ�ȡ�����б�
        /// </summary>
        public DataSet GetList(string strField, string strWhere)
        {
            return dal.GetList(strField, strWhere);
        }
        /// <summary>
        /// �����ֶ�ȡ�����б�
        /// </summary>
        public DataSet GetList2(int strField, string strWhere)
        {
            return dal.GetList2(strField, strWhere);
        }
        //-----------------------------------------------------
        /// <summary>
        /// me_�Ƿ���ڶҽ���¼
        /// </summary>
        public bool ExistsState(int ID, int UsID)
        {
            return dal.ExistsState(ID, UsID);
        }
        /// <summary>
        /// me_��̨��ҳ������ȡ���а������б�
        /// </summary>
        public DataSet GetListByPage2(int startIndex, int endIndex, string s1, string s2)
        {
            return dal.GetListByPage2(startIndex, endIndex, s1, s2);
        }
        /// <summary>
        /// me_�Ƿ���ڸ÷���
        /// </summary>
        public bool Exists(int ID)
        {
            return dal.Exists(ID);
        }
        /// <summary>
        /// me_�Ƿ��������ע�ķ���
        /// </summary>
        public bool Exists_xz(int room, int table)
        {
            return dal.Exists_xz(room, table);
        }
        /// <summary>
        /// me_�Ƿ���ڸ÷���
        /// </summary>
        public bool Exists_id(int ID)
        {
            return dal.Exists_id(ID);
        }
        /// <summary>
        /// me_�Ƿ�����������
        /// </summary>
        public bool Exists_wj(int ID)
        {
            return dal.Exists_wj(ID);
        }
        /// <summary>
        /// me_�Ƿ���ڸ÷���
        /// </summary>
        public bool Exists()
        {
            return dal.Exists();
        }
        /// <summary>
        /// me_�Ƿ���ڸ÷���þ�δ����
        /// </summary>
        public bool Exists(int ID, int roomtable)
        {
            return dal.Exists(ID, roomtable);
        }
        /// <summary>
        /// me_�õ�һ������ʵ��
        /// </summary>
        public BCW.Baccarat.Model.BJL_Play GetBJL_Play(int ID, int usid)
        {

            return dal.GetBJL_Play(ID, usid);
        }
        /// <summary>
        ///me_�õ��ض�����ID������table����ɵ���עʱ��
        /// </summary>
        public DateTime GetMinBetTime(int RoomID, int Table)
        {
            return dal.GetMinBetTime(RoomID, Table);
        }
        /// <summary>
        /// me_����Ͷע�ܱ�ֵ
        /// </summary>
        public long GetPrice(int RoomID, int Table)
        {
            return dal.GetPrice(RoomID, Table);
        }
        /// <summary>
        /// me_�����н��ܱ�ֵ
        /// </summary>
        public long Getmoney(int RoomID, int Table)
        {
            return dal.Getmoney(RoomID, Table);
        }
        /// <summary>
        /// me_�����������ܱ�ֵ
        /// </summary>
        public long Getsxf(int RoomID, int Table)
        {
            return dal.Getsxf(RoomID, Table);
        }
        /// <summary>
        ///  me_�����ֶ��޸������б�
        /// </summary>
        public DataSet update_zd(string strField, string strWhere)
        {
            return dal.update_zd(strField, strWhere);
        }
        //------------------------------------------------------

        /// <summary>
        /// ȡ��ÿҳ��¼
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">��ҳ��С</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <returns>IList BJL_Play</returns>
        public IList<BCW.Baccarat.Model.BJL_Play> GetBJL_Plays(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
        {
            return dal.GetBJL_Plays(p_pageIndex, p_pageSize, strWhere, strOrder, out p_recordCount);
        }

        #endregion  ��Ա����
    }
}

