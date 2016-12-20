using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.SFC.Model;
namespace BCW.SFC.BLL
{
    /// <summary>
    /// ҵ���߼���SfJackpot ��ժҪ˵����
    /// </summary>
    public class SfJackpot
    {
        private readonly BCW.SFC.DAL.SfJackpot dal = new BCW.SFC.DAL.SfJackpot();
        public SfJackpot()
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
        public bool Exists(int id)
        {
            return dal.Exists(id);
        }
        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool Exists2(int CID)
        {
            return dal.Exists2(CID);
        }
        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool Exists3(int CID)
        {
            return dal.Exists3(CID);
        }
        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool Exists4(int CID)
        {
            return dal.Exists4(CID);
        }
        /// <summary>
        /// �Ƿ�������ڼ�¼
        /// </summary>
        public bool Exists4()
        {
            return dal.Exists4();
        }
        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool Existsgun(int CID)
        {
            return dal.Existsgun(CID);
        }
        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool Exists1(int usid)
        {
            return dal.Exists1(usid);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public int Add(BCW.SFC.Model.SfJackpot model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public void Update(BCW.SFC.Model.SfJackpot model)
        {
            dal.Update(model);
        }
        /// <summary>
        /// ����usid
        /// </summary>
        /// <param name="id"></param>
        /// <param name="usid"></param>
        public void UpdateusID(int id, int usid)
        {
            dal.UpdateusID(id, usid);
        }

        /// <summary>
        /// ����Ԥ���ڵĽ���
        /// </summary>
        /// <param name="allmoney"></param>
        /// <param name="CID"></param>
        public void updateallmoney(long allmoney, int CID, int usID, int id)
        {
            dal.updateallmoney(allmoney, CID, usID, id);
        }
        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public void Delete(int id)
        {
            dal.Delete(id);
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
        public BCW.SFC.Model.SfJackpot GetSfJackpot(int id)
        {

            return dal.GetSfJackpot(id);
        }
        /// <summary>
        /// �õ�һ��WinCent
        /// </summary>
        public long GetWinCent(string time1, string time2)
        {
            return dal.GetWinCent(time1, time2);
        }
        /// <summary>
        /// �õ�һ��WinCent������ʱ��
        /// </summary>
        public long GetWinCentall()
        {
            return dal.GetWinCentall();
        }
        /// <summary>
        /// �õ�һ��allmoney
        /// </summary>
        public long Getallmoney(int CID)
        {
            return dal.Getallmoney(CID);
        }
        /// <summary>
        /// �õ�һ��WinCent
        /// </summary>
        public long GetPayCent(string time1, string time2)
        {
            return dal.GetPayCent(time1, time2);
        }
        /// <summary>
        /// �õ�һ��WinCent������ʱ��
        /// </summary>
        public long GetPayCentall()
        {
            return dal.GetPayCentall();
        }
        /// <summary>
        /// ϵͳ��Ͷע��
        /// </summary>
        /// <returns></returns>
        public long SysPrice(int CID)
        {
            return dal.SysPrice(CID);
        }
        /// <summary>
        /// ��ѯ���ڽ���
        /// </summary>
        /// <param name="CID"></param>
        /// <returns></returns>
        public long SysNowprize(int CID)
        {
            return dal.SysNowprize(CID);
        }
        /// <summary>
        /// �õ�һ��GetWinCentlast
        /// </summary>
        public long GetWinCentlast()
        {
            return dal.GetWinCentlast();
        }
        /// <summary>
        /// �õ�һ��GetWinCentlast5
        /// </summary>
        public long GetWinCentlast5()
        {
            return dal.GetWinCentlast5();
        }
        /// <summary>
        /// ϵͳ�ܻ��ն�
        /// </summary>
        /// <returns></returns>
        public long SysWin(int CID)
        {
            return dal.SysWin(CID);
        }
        /// <summary>
        /// �����ֶ�ȡ�����б�
        /// </summary>
        public DataSet GetList(string strField, string strWhere)
        {
            return dal.GetList(strField, strWhere);
        }

        /// <summary>
        /// ȡ��ÿҳ��¼
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">��ҳ��С</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <returns>IList SfJackpot</returns>
        public IList<BCW.SFC.Model.SfJackpot> GetSfJackpots(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
        {
            return dal.GetSfJackpots(p_pageIndex, p_pageSize, strWhere, strOrder, out p_recordCount);
        }

        #endregion  ��Ա����
    }
}

