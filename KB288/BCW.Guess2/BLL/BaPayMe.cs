using System;
using System.Data;
using System.Collections.Generic;
using TPR2.Common;
using TPR2.Model.guess;
namespace TPR2.BLL.guess
{
    /// <summary>
    /// ҵ���߼���BaPayMe ��ժҪ˵����
    /// </summary>
    public class BaPayMe
    {
        private readonly TPR2.DAL.guess.BaPayMe dal = new TPR2.DAL.guess.BaPayMe();
        public BaPayMe()
        { }
        #region  ��Ա����
               
        /// <summary>
        /// �Ƿ���ڳ������½�����¼
        /// </summary>
        public bool Exists(int bcid, int payusid)
        {
            return dal.Exists(bcid, payusid);
        }


        /// <summary>
        /// �õ�ĳһ������ע����ñ�
        /// </summary>
        public decimal GetBaPayMeMoney(int id, int payusid)
        {
            return dal.GetBaPayMeMoney(id, payusid);
        }
             
        /// <summary>
        /// �õ�ĳ���¸���Ŀ�ļ�¼��
        /// </summary>
        public int GetCount(int ibcid, int iptype, int ipaytype)
        {
            return dal.GetCount(ibcid, iptype, ipaytype);
        }
        /// <summary>
        /// �õ�ĳID��ĳ���¸���Ŀ�ļ�¼��
        /// </summary>
        public int GetCount(int ibcid, int iptype, int ipaytype, int iusid)
        {
            return dal.GetCount(ibcid, iptype, ipaytype, iusid);
        }
        /// <summary>
        /// �õ�ĳһ������ע��ע��
        /// </summary>
        public int GetBaPayMeNum(int ibcid, int iptype)
        {
            return dal.GetBaPayMeNum(ibcid, iptype);
        }

        /// <summary>
        /// �õ�ĳһ������ע�ܽ��
        /// </summary>
        public long GetBaPayMeCent(int ibcid, int iptype)
        {
            return dal.GetBaPayMeCent(ibcid, iptype);
        }
        
        /// <summary>
        /// �õ�ĳһ����ĳ����ע�ܽ��
        /// </summary>
        public long GetBaPayMeCent(int ibcid, int iptype, int ipaytype)
        {
            return dal.GetBaPayMeCent(ibcid, iptype, ipaytype);
        }

        /// <summary>
        /// ���������õ�������ע��
        /// </summary>
        public int GetBaPayMeCount(string strWhere)
        {
            return dal.GetBaPayMeCount(strWhere);
        }
        /// <summary>
        /// ���������õ�������ע�ܽ��
        /// </summary>
        public long GetBaPayMepayCent(string strWhere)
        {
            return dal.GetBaPayMepayCent(strWhere);
        }
        /// <summary>
        /// ���������õ�������עӯ����
        /// </summary>
        public long GetBaPayMegetMoney(string strWhere)
        {
            return dal.GetBaPayMegetMoney(strWhere);
        }

        /// <summary>
        /// �Ƿ���ڶҽ���¼
        /// </summary>
        public bool ExistsIsCase(int ID, int payusid)
        {
            return dal.ExistsIsCase(ID, payusid);
        }

        /// <summary>
        /// �����û��ҽ�
        /// </summary>
        public void UpdateIsCase(int ID)
        {
            dal.UpdateIsCase(ID);
        }

        /// <summary>
        /// �����ߵ���עΪ����
        /// </summary>
        public void Updatestate(int ID)
        {
            dal.Updatestate(ID);
        }

        /// <summary>
        /// ����ƽ��ҵ��
        /// </summary>
        public void UpdatePPCase(TPR2.Model.guess.BaPayMe model)
        {
            dal.UpdatePPCase(model);
        }

        /// <summary>
        /// ���¿���ҵ��
        /// </summary>
        public void UpdateCase(TPR2.Model.guess.BaPayMe model, string p_strVal, out decimal p_intDuVal, out int p_intWin, out string WinType)
        {
            dal.UpdateCase(model, p_strVal, out p_intDuVal, out p_intWin, out WinType);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public int Add(TPR2.Model.guess.BaPayMe model)
        {
            return dal.Add(model);
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
        public void DeleteStr(string strWhere)
        {
            dal.DeleteStr(strWhere);
        }

        /// <summary>
        /// ɾ��һ������,��������ID
        /// </summary>
        public void Deletebcid(int gid)
        {
            dal.Deletebcid(gid);
        }

        /// <summary>
        /// �õ�һ��getMoney
        /// </summary>
        public decimal Getp_getMoney(int ID)
        {
            return dal.Getp_getMoney(ID);
        }
                
        /// <summary>
        /// �õ�һ��Types
        /// </summary>
        public int GetTypes(int ID)
        {
            return dal.GetTypes(ID);
        }

        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public TPR2.Model.guess.BaPayMe GetModelIsCase(int ID)
        {
            return dal.GetModelIsCase(ID);
        }
                
        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public TPR2.Model.guess.BaPayMe GetModel(int ID)
        {
            return dal.GetModel(ID);
        }

        /// <summary>
        /// �����ֶ�ȡ�����б�
        /// </summary>
        public DataSet GetBaPayMeList(string strField, string strWhere)
        {
            return dal.GetBaPayMeList(strField, strWhere);
        }

        /// <summary>
        /// �����ֶ�ȡ�����б�
        /// </summary>
        public DataSet GetBaPayMeList(string strField, string strWhere, string filedOrder)
        {
            return dal.GetBaPayMeList(strField, strWhere, filedOrder);
        }

        /// <summary>
        /// ȡ��ÿҳ��¼
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">��ҳ��С</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <returns>IList</returns>
        public IList<TPR2.Model.guess.BaPayMe> GetBaPayMes(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetBaPayMes(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }

        /// <summary>
        /// ȡ��ÿҳ��¼
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">��ҳ��С</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <returns>IList</returns>
        public IList<TPR2.Model.guess.BaPayMe> GetBaPayMeViews(int p_pageIndex, int p_pageSize, string strWhere, int itype, out int p_recordCount)
        {
            return dal.GetBaPayMeViews(p_pageIndex, p_pageSize, strWhere, itype, out p_recordCount);
        }
    
        /// <summary>
        /// ȡ����ϸ���а��¼
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">��ҳ��С</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <returns>IList BaPayMeTop</returns>
        public IList<TPR2.Model.guess.BaPayMe> GetBaPayMeTop(int p_pageIndex, int p_pageSize, string strWhere, int itype, out int p_recordCount)
        {
            return dal.GetBaPayMeTop(p_pageIndex, p_pageSize, strWhere, itype, out p_recordCount);
        }

        /// <summary>
        /// ȡ����ϸ���а��¼
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">��ҳ��С</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <returns>IList BaPayMeTop</returns>
        public IList<TPR2.Model.guess.BaPayMe> GetBaPayMeTop2(int p_pageIndex, int p_pageSize, string strWhere, int itype, out int p_recordCount)
        {
            return dal.GetBaPayMeTop2(p_pageIndex, p_pageSize, strWhere, itype, out p_recordCount);
        }

        #endregion  ��Ա����
    }
}
