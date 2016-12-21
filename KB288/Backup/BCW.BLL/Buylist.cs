using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
    /// <summary>
    /// ҵ���߼���Buylist ��ժҪ˵����
    /// </summary>
    public class Buylist
    {
        private readonly BCW.DAL.Buylist dal = new BCW.DAL.Buylist();
        public Buylist()
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
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool Exists(int ID, int UserId)
        {
            return dal.Exists(ID, UserId);
        }

        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool Exists(int ID, int UserId, int AcStats)
        {
            return dal.Exists(ID, UserId, AcStats);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public int Add(BCW.Model.Buylist model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public void Update(BCW.Model.Buylist model)
        {
            dal.Update(model);
        }

        /// <summary>
        /// ����ǰ̨����״̬
        /// </summary>
        public void UpdateStats(BCW.Model.Buylist model)
        {
            dal.UpdateStats(model);
        }

        /// <summary>
        /// �����û�����
        /// </summary>
        public void UpdateBuy(BCW.Model.Buylist model)
        {
            dal.UpdateBuy(model);
        }

        /// <summary>
        /// ���º�̨����״̬
        /// </summary>
        public void UpdateMStats(BCW.Model.Buylist model)
        {
            dal.UpdateMStats(model);
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
        public void Delete2(int GoodsId)
        {

            dal.Delete2(GoodsId);
        }
                
        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public void Delete3(int NodeId)
        {

            dal.Delete3(NodeId);
        }

        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public BCW.Model.Buylist GetBuylist(int ID)
        {

            return dal.GetBuylist(ID, 0);
        }

        /// <summary>
        /// �õ�һ��ʵ��
        /// </summary>
        public BCW.Model.Buylist GetBuylistMe(int ID)
        {
            return dal.GetBuylistMe(ID);
        }

        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public BCW.Model.Buylist GetBuylist(int ID, int UserId)
        {

            return dal.GetBuylist(ID, UserId);
        }

        /// <summary>
        /// ��������б�
        /// </summary>
        public DataSet GetList(int GoodsId, int TopNum)
        {
            return dal.GetList(GoodsId, TopNum);
        }

        /// <summary>
        /// ȡ��ÿҳ��¼
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">��ҳ��С</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <returns>IList Buylist</returns>
        public IList<BCW.Model.Buylist> GetBuylists(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetBuylists(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }

        #endregion  ��Ա����
    }
}
