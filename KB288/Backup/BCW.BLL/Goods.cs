using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
    /// <summary>
    /// ҵ���߼���Goods ��ժҪ˵����
    /// </summary>
    public class Goods
    {
        private readonly BCW.DAL.Goods dal = new BCW.DAL.Goods();
        public Goods()
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
        public bool Exists(string Title)
        {
            return dal.Exists(Title);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public int Add(BCW.Model.Goods model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public void Update(BCW.Model.Goods model)
        {
            dal.Update(model);
        }

        /// <summary>
        /// ���³�������
        /// </summary>
        public void UpdateSellCount(int ID, int SellCount)
        {
            dal.UpdateSellCount(ID, SellCount);
        }

        /// <summary>
        /// ������ϸͳ��
        /// </summary>
        public void UpdateReStats(int ID, string ReStats, string ReLastIP)
        {
            dal.UpdateReStats(ID, ReStats, ReLastIP);
        }

        /// <summary>
        /// ���µ����
        /// </summary>
        public void UpdateReadcount(int ID, int Readcount)
        {
            dal.UpdateReadcount(ID, Readcount);
        }

        /// <summary>
        /// ������������
        /// </summary>
        public void UpdateRecount(int ID, int Recount)
        {
            dal.UpdateRecount(ID, Recount);
        }

        /// <summary>
        /// ���¹�������
        /// </summary>
        public void UpdatePaycount(int ID, int Paycount)
        {
            dal.UpdatePaycount(ID, Paycount);
        }

        /// <summary>
        /// ������������
        /// </summary>
        public void UpdateEvcount(int ID, int Evcount)
        {
            dal.UpdateEvcount(ID, Evcount);
        }

        /// <summary>
        /// �����ļ�
        /// </summary>
        public void UpdateFiles(int ID, string Files)
        {
            dal.UpdateFiles(ID, Files);
        }

        /// <summary>
        /// ���·���
        /// </summary>
        public void UpdateCover(int ID, string Cover)
        {
            dal.UpdateCover(ID, Cover);
        }

        /// <summary>
        /// ���½ڵ�
        /// </summary>
        public void UpdateNodeId(int ID, int NodeId)
        {
            dal.UpdateNodeId(ID, NodeId);
        }

        /// <summary>
        /// �������½ڵ�
        /// </summary>
        public void UpdateNodeIds(int NewNodeId, int OrdNodeId)
        {
            dal.UpdateNodeIds(NewNodeId, OrdNodeId);
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public void Delete(int ID)
        {

            dal.Delete(ID);
        }


        /// <summary>
        /// ɾ���ļ�
        /// </summary>
        public void DeleteFiles(int ID)
        {

            dal.DeleteFiles(ID);
        }

        /// <summary>
        /// ɾ���ļ�
        /// </summary>
        public void DeleteNodeId(int ID)
        {

            dal.DeleteNodeId(ID);
        }

        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public BCW.Model.Goods GetGoods(int ID)
        {

            return dal.GetGoods(ID);
        }

        /// <summary>
        /// �õ�Title����
        /// </summary>
        public string GetTitle(int ID)
        {
            return dal.GetTitle(ID);
        }

        /// <summary>
        /// �õ��ڵ�NodeId
        /// </summary>
        public int GetNodeId(int ID)
        {
            return dal.GetNodeId(ID);
        }

        /// <summary>
        /// �õ��ļ�Files
        /// </summary>
        public string GetFiles(int ID)
        {
            return dal.GetFiles(ID);
        }

        /// <summary>
        /// �õ�����Cover
        /// </summary>
        public string GetCover(int ID)
        {
            return dal.GetCover(ID);
        }

        /// <summary>
        /// ��������б�
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            return dal.GetList(strWhere);
        }

        /// <summary>
        /// ��������б�
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
        /// <returns>IList Goods</returns>
        public IList<BCW.Model.Goods> GetGoodss(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetGoodss(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }

        #endregion  ��Ա����
    }
}

