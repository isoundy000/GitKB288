using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.bydr.Model;
namespace BCW.bydr.BLL
{
    /// <summary>
    /// ҵ���߼���Cmg_Top ��ժҪ˵����
    /// </summary>
    public class Cmg_Top
    {
        private readonly BCW.bydr.DAL.Cmg_Top dal = new BCW.bydr.DAL.Cmg_Top();
        public Cmg_Top()
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

        public bool ExistsusID(int meid)
        {
            return dal.ExistsusID(meid);
        }

        /// <summary>
        /// �Ƿ���ڸ�Bid
        /// </summary>
        public bool ExistsBid(int id, int usid)
        {
            return dal.ExistsBid(id, usid);
        }
        /// <summary>
        /// �Ƿ�������δ���¼
        /// </summary>
        public bool ExistsusID1(int usid)
        {
            return dal.ExistsusID1(usid);

        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public int Add(BCW.bydr.Model.Cmg_Top model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public void Update(BCW.bydr.Model.Cmg_Top model)
        {
            dal.Update(model);
        }
        /// <summary>
        /// ����jid
        /// </summary>

        public void UpdateJid(int Bid, int jID)
        {
            dal.UpdateJid(Bid, jID);
        }
        /// <summary>
        /// ����Bid���ƶҽ��ֶ�
        /// </summary>

        public void UpdateBid(int Bid, int id, int usid)
        {
            dal.UpdateBid(Bid, id, usid);
        }
        /// <summary>
        /// �����ظ����ƶҽ��ֶ�
        /// </summary>

        public void UpdateExpiry(int Expiry, int id)
        {
            dal.UpdateExpiry(Expiry, id);
        }
        /// <summary>
        /// �������ռ���
        /// </summary>
        public void UpdateAllcolletGold(int id, long AllcolletGold)
        {
            dal.UpdateAllcolletGold(id, AllcolletGold);
        }
        /// <summary>
        /// ����ColletGold
        /// </summary>
        public void UpdateColletGold(int id, long ColletGold)
        {
            dal.UpdateColletGold(id, ColletGold);
        }
        /// <summary>
        /// ����Updateranddaoju
        /// </summary>
        public void Updateranddaoju(string randdaoju, int id)
        {
            dal.Updateranddaoju(randdaoju, id);
        }
        /// <summary>
        /// ����Updaterandten
        /// </summary>
        public void Updaterandten(string randten, int id)
        {
            dal.Updaterandten(randten, id);
        }
        /// <summary>
        /// ����UpdaterandyuID
        /// </summary>
        public void UpdaterandyuID(string randyuID, int id)
        {
            dal.UpdaterandyuID(randyuID, id);
        }
        /// <summary>
        /// ���������Ϸ����
        /// </summary>
        public void UpdateDcolletGold(int id, long DcolletGold)
        {
            dal.UpdateDcolletGold(id, DcolletGold);
        }
        /// <summary>
        /// ���·�ˢ�ֶ�
        /// </summary>
        public void UpdateYcolletGold(int id, long YcolletGold)
        {
            dal.UpdateYcolletGold(id, YcolletGold);
        }
        /// <summary>
        /// ����ÿ���ռ���
        /// </summary>
        public void UpdateMcolletGold(int Bid, long McolletGold)
        {
            dal.UpdateMcolletGold(Bid, McolletGold);
        }
        /// <summary>
        /// ���·�ˢʱ��
        /// </summary>
        public void updatetime(int id, DateTime updatetime)
        {
            dal.updatetime(id, updatetime);
        }
        /// <summary>
        /// ����ʱ��
        /// </summary>
        public void updatetime1(int id, DateTime updatetime)
        {
            dal.updatetime1(id, updatetime);
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
        public BCW.bydr.Model.Cmg_Top GetCmg_Top(int ID)
        {

            return dal.GetCmg_Top(ID);
        }
        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public BCW.bydr.Model.Cmg_Top GetCmg_Top(int ID, int usid)
        {

            return dal.GetCmg_Top(ID, usid);
        }
        /// <summary>
        /// �õ�һ�������ռ���
        /// </summary>
        public long GetCmg_AllcolletGold(int usID, string time)
        {
            return dal.GetCmg_AllcolletGold(usID, time);
        }
        /// <summary>
        /// �õ������ռ���
        /// </summary>
        public long GetCmg_AllcolletGoldday(string time1, string time2)
        {
            return dal.GetCmg_AllcolletGoldday(time1, time2);
        }
        /// <summary>
        /// �õ�id��Ĵ���
        /// </summary>
        public int GetCmgcount(int usID)
        {
            return dal.GetCmgcount(usID);
        }
        /// <summary>
        /// �õ�usid������Ĵ���_�۹���20160813
        /// </summary>
        public int GetCmgcount1(int usID)
        {
            return dal.GetCmgcount1(usID);
        }
        /// <summary>
        /// �õ�һ�������ռ���
        /// </summary>
        public long GetCmg_AllcolletGoldmonth(int usID, string time1, string time2)
        {
            return dal.GetCmg_AllcolletGoldmonth(usID, time1, time2);
        }
        /// <summary>
        /// �õ������ռ���
        /// </summary>
        public long GetCmg_AllcolletGoldmonth1(string time1, string time2)
        {
            return dal.GetCmg_AllcolletGoldmonth1(time1, time2);
        }

        /// <summary>
        /// �õ����ռ���
        /// </summary>
        public long GetCmg_AllcolletGold1(int usID)
        {
            return dal.GetCmg_AllcolletGold1(usID);
        }
        /// <summary>
        /// �õ����ռ���
        /// </summary>
        public long GetCmg_AllcolletGold2()
        {
            return dal.GetCmg_AllcolletGold2();
        }
        ///// <summary>
        ///// �õ�����ռ���
        ///// </summary>
        //public BCW.bydr.Model.Cmg_Top GetCmgAllcolletGold(int ID)
        //{
        //    return dal.GetCmgAllcolletGold(ID);
        //}
        /// <summary>
        /// �����ֶ�ȡ�����б�
        /// </summary>
        public DataSet GetList(string strField, string strWhere)
        {
            return dal.GetList(strField, strWhere);
        }

        /// <summary>
        /// ��ҳ������ȡ���а������б�
        /// </summary>
        public DataSet GetListByPage(string s1, string s2)
        {
            return dal.GetListByPage(s1, s2);
        }
        /// <summary>
        /// ȡ��ÿҳ��¼
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">��ҳ��С</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <returns>IList Cmg_Top</returns>
        public IList<BCW.bydr.Model.Cmg_Top> GetCmg_Tops(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetCmg_Tops(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }
        /// <summary>
        /// ȡ��ÿҳ��¼
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">��ҳ��С</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <returns>IList Cmg_Top</returns>
        public IList<BCW.bydr.Model.Cmg_Top> GetCmg_Tops2(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetCmg_Tops2(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }
        /// <summary>
        /// ȡ����ͬusid����ռ���¼
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">��ҳ��С</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <returns>IList Cmg_Top</returns>
        public IList<BCW.bydr.Model.Cmg_Top> GetCmg_Tops1(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
        {
            return dal.GetCmg_Tops1(p_pageIndex, p_pageSize, strWhere, strOrder, out p_recordCount);
        }

        #endregion  ��Ա����



    }
}

