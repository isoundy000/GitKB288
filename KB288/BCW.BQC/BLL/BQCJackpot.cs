using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.BQC.Model;
namespace BCW.BQC.BLL
{
    /// <summary>
    /// ҵ���߼���BQCJackpot ��ժҪ˵����
    /// </summary>
    public class BQCJackpot
    {
        private readonly BCW.BQC.DAL.BQCJackpot dal = new BCW.BQC.DAL.BQCJackpot();
        public BQCJackpot()
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
        public bool Exists1(int usid)
        {
            return dal.Exists1(usid);
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
        /// �Ƿ�������ڼ�¼
        /// </summary>
        public bool Exists4()
        {
            return dal.Exists4();
        }
        /// <summary>
        /// ����һ������
        /// </summary>
        public int Add(BCW.BQC.Model.BQCJackpot model)
        {
            return dal.Add(model);
        }
        /// <summary>
        /// �õ�һ��allmoney
        /// </summary>
        public long Getallmoney(int CID)
        {
            return dal.Getallmoney(CID);
        }
        /// <summary>
        /// ����һ������
        /// </summary>
        public void Update(BCW.BQC.Model.BQCJackpot model)
        {
            dal.Update(model);
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
        /// ϵͳ�ܻ��ն�
        /// </summary>
        /// <returns></returns>
        public long SysWin(int CID)
        {
            return dal.SysWin(CID);
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
        /// ɾ��һ������
        /// </summary>
        public void Delete(int id)
        {

            dal.Delete(id);
        }

        /// <summary>
        /// ϵͳ��Ͷע��
        /// </summary>
        /// <returns></returns>
        public long SysPrice()
        {
            return dal.SysPrice();
        }

        /// <summary>
        /// ϵͳ�ܻ��ն�
        /// </summary>
        /// <returns></returns>
        public long SysWin()
        {
            return dal.SysWin();
        }
        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool Exists4(int CID)
        {
            return dal.Exists4(CID);
        }
        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public BCW.BQC.Model.BQCJackpot GetBQCJackpot(int id)
        {

            return dal.GetBQCJackpot(id);
        }

        /// <summary>
        /// �õ�һ��WinCent
        /// </summary>
        public long GetWinCent(string time1, string time2)
        {
            return dal.GetWinCent(time1, time2);
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
        /// ɾ��һ������
        /// </summary>
        public void Delete(string strWhere)
        {

            dal.Delete(strWhere);
        }
        /// <summary>
        /// �õ�һ��WinCent
        /// </summary>
        public long GetPayCent(string time1, string time2)
        {
            return dal.GetPayCent(time1, time2);
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
        /// <returns>IList BQCJackpot</returns>
        public IList<BCW.BQC.Model.BQCJackpot> GetBQCJackpots(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
        {
            return dal.GetBQCJackpots(p_pageIndex, p_pageSize, strWhere, strOrder, out p_recordCount);
        }

        #endregion  ��Ա����
    }
}

