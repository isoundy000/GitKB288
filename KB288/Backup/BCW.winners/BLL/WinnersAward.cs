using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
    /// <summary>
    /// ҵ���߼���tb_WinnersAward ��ժҪ˵����
    /// </summary>
    public class tb_WinnersAward
    {
        private readonly BCW.DAL.tb_WinnersAward dal = new BCW.DAL.tb_WinnersAward();
        public tb_WinnersAward()
        { }
        #region  ��Ա����
        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool Exists(int Id)
        {
            return dal.Exists(Id);
        }
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
        public int Add(BCW.Model.tb_WinnersAward model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// ����isDone
        /// </summary>
        public void UpdateIsDone(int ID, int isDone)
        {
            dal.UpdateIsDone(ID, isDone);
        }
        /// <summary>
        /// �õ��������
        /// </summary>
        public string GetRemarks(int Id)
        {
            return dal.GetRemarks(Id);
        }
        /// <summary>
        /// �õ��������Ͷ����½���
        /// </summary>
        public int GetMaxAwardForType(string type)
        {
            return dal.GetMaxAwardForType(type);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public void Update(BCW.Model.tb_WinnersAward model)
        {
            dal.Update(model);
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public void Delete(int Id)
        {

            dal.Delete(Id);
        }

        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public BCW.Model.tb_WinnersAward Gettb_WinnersAward(int Id)
        {

            return dal.Gettb_WinnersAward(Id);
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
        /// <returns>IList tb_WinnersAward</returns>
        public IList<BCW.Model.tb_WinnersAward> Gettb_WinnersAwards(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.Gettb_WinnersAwards(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }

        #endregion  ��Ա����
    }
}

