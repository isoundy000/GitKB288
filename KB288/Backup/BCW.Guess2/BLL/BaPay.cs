using System;
using System.Data;
using System.Collections.Generic;
using TPR2.Common;
using TPR2.Model.guess;
/// <summary>
/// �����˹�ȷ�ϱ�ʶ
/// 
/// �ƹ��� 20160324
/// </summary>
namespace TPR2.BLL.guess
{
    /// <summary>
    /// ҵ���߼���BaPay ��ժҪ˵����
    /// </summary>
    public class BaPay
    {
        private readonly TPR2.DAL.guess.BaPay dal = new TPR2.DAL.guess.BaPay();
        public BaPay()
        { }
        #region  ��Ա����

        /// <summary>
        /// �õ�ĳһ������ע����ñ�
        /// </summary>
        public decimal GetBaPayMoney(int id, int payusid)
        {
            return dal.GetBaPayMoney(id, payusid);
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
        public int GetBaPayNum(int ibcid, int iptype)
        {
            return dal.GetBaPayNum(ibcid, iptype);
        }

        /// <summary>
        /// �õ�ĳһ������ע��ע������������ 20160817 �۹���
        /// </summary>
        public int GetBaPayNum_robot(int ibcid, int iptype)
        {
            return dal.GetBaPayNum_robot(ibcid, iptype);
        }

        /// <summary>
        /// �õ�ĳһ������ע�ܽ��
        /// </summary>
        public long GetBaPayCent(int ibcid, int iptype)
        {
            return dal.GetBaPayCent(ibcid, iptype);
        }

        /// <summary>
        /// �õ�ĳһ������ע�ܽ��������� 20160817 �۹���
        /// </summary>
        public long GetBaPayCent_robot(int ibcid, int iptype)
        {
            return dal.GetBaPayCent_robot(ibcid, iptype);
        }

        /// <summary>
        /// �õ�ĳһ�����ߵ���ע�ܽ��
        /// </summary>
        public long GetBaPayCent(int ibcid, int iptype, DateTime dt)
        {
            return dal.GetBaPayCent(ibcid, iptype, dt);
        }

        /// <summary>
        /// �õ�ĳһ����ĳID��ע�ܽ��
        /// </summary>
        public long GetBaPayCent(int ibcid, int iptype, int payusid)
        {
            return dal.GetBaPayCent(ibcid, iptype, payusid);
        }

        /// <summary>
        /// �õ�ĳһ����ĳ����ע�ܽ��
        /// </summary>
        public long GetBaPayCent2(int ibcid, int iptype, int ipaytype)
        {
            return dal.GetBaPayCent2(ibcid, iptype, ipaytype);
        }

        /// <summary>
        /// �õ�ĳһ����ĳ����ע�ܽ��
        /// </summary>
        public long GetBaPayCent2(int ibcid, int iptype, int ipaytype, DateTime dt)
        {
            return dal.GetBaPayCent2(ibcid, iptype, ipaytype, dt);
        }

        /// <summary>
        /// ���������õ�������ע��
        /// </summary>
        public int GetBaPayCount(string strWhere)
        {
            return dal.GetBaPayCount(strWhere);
        }
        /// <summary>
        /// ���������õ�������ע�ܽ��
        /// </summary>
        public long GetBaPaypayCent(string strWhere)
        {
            return dal.GetBaPaypayCent(strWhere);
        }
        /// <summary>
        /// ���������õ�������עӯ����
        /// </summary>
        public long GetBaPaygetMoney(string strWhere)
        {
            return dal.GetBaPaygetMoney(strWhere);
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
        /// ���¹�����עΪ����
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public void Updatestate(int ID)
        {
            dal.Updatestate(ID);
        }

        /// <summary>
        /// �����˹���עȷ�ϱ�ʶ
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public void UpdateSure(int ID)
        {
            dal.UpdateSure(ID);
        }

        /// <summary>
        /// ����ƽ��ҵ��
        /// </summary>
        public void UpdatePPCase(TPR2.Model.guess.BaPay model)
        {
            dal.UpdatePPCase(model);
        }

        /// <summary>
        /// ���¿���ҵ��
        /// </summary>
        public void UpdateCase(TPR2.Model.guess.BaPay model, string p_strVal, out decimal p_intDuVal, out int p_intWin)
        {
            dal.UpdateCase(model, p_strVal, out p_intDuVal, out p_intWin);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public int Add(TPR2.Model.guess.BaPay model)
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
        /// �õ�һ��bcid
        /// </summary>
        public int Getbcid(int ID)
        {
            return dal.Getbcid(ID);
        }

        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public TPR2.Model.guess.BaPay GetModelIsCase(int ID)
        {
            return dal.GetModelIsCase(ID);
        }

        /// <summary>
        /// �����ֶ�ȡ�����б�
        /// </summary>
        public DataSet GetBaPayList(string strField, string strWhere)
        {
            return dal.GetBaPayList(strField, strWhere);
        }

        /// <summary>
        /// �����ֶ�ȡ�����б�
        /// </summary>
        public DataSet GetBaPayList(string strField, string strWhere, string filedOrder)
        {
            return dal.GetBaPayList(strField, strWhere, filedOrder);
        }

        /// <summary>
        /// ȡ��ÿҳ��¼
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">��ҳ��С</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <returns>IList</returns>
        public IList<TPR2.Model.guess.BaPay> GetBaPays(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetBaPays(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }

        /// <summary>
        /// ȡ��ÿҳ��¼
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">��ҳ��С</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <returns>IList</returns>
        public IList<TPR2.Model.guess.BaPay> GetBaPayViews(int p_pageIndex, int p_pageSize, string strWhere, int itype, out int p_recordCount)
        {
            return dal.GetBaPayViews(p_pageIndex, p_pageSize, strWhere, itype, out p_recordCount);
        }

        /// <summary>
        /// ȡ����ϸ���а��¼
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">��ҳ��С</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <returns>IList BaPayTop</returns>
        public IList<TPR2.Model.guess.BaPay> GetBaPayTop(int p_pageIndex, int p_pageSize, string strWhere, int itype, out int p_recordCount)
        {
            return dal.GetBaPayTop(p_pageIndex, p_pageSize, strWhere, itype, out p_recordCount);
        }

        /// <summary>
        /// ȡ����ϸ���а��¼
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">��ҳ��С</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <returns>IList BaPayTop</returns>
        public IList<TPR2.Model.guess.BaPay> GetBaPayTop2(int p_pageIndex, int p_pageSize, string strWhere, int itype, out int p_recordCount)
        {
            return dal.GetBaPayTop2(p_pageIndex, p_pageSize, strWhere, itype, out p_recordCount);
        }

        #endregion  ��Ա����
    }
}
