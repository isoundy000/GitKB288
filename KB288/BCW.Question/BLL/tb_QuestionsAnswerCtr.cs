using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
    /// <summary>
    /// ҵ���߼���tb_QuestionsAnswerCtr ��ժҪ˵����
    /// </summary>
    public class tb_QuestionsAnswerCtr
    {
        private readonly BCW.DAL.tb_QuestionsAnswerCtr dal = new BCW.DAL.tb_QuestionsAnswerCtr();
        public tb_QuestionsAnswerCtr()
        { }
        #region  ��Ա����
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
		public bool ExistsID(int uid, int cid)
        {
            return dal.ExistsID(uid, cid);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public int Add(BCW.Model.tb_QuestionsAnswerCtr model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public void Update(BCW.Model.tb_QuestionsAnswerCtr model)
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

        public int GetAllAwardGold(int uid)
        {
            return dal.GetAllAwardGold(uid);

        }
        /// <summary>
        /// ��uid  �õ�һ�����¶���
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="contrID"></param>
        /// <returns></returns>
        public BCW.Model.tb_QuestionsAnswerCtr Gettb_QuestionsAnswerCtrByUid(int uid)
        {

            return dal.Gettb_QuestionsAnswerCtrByUid(uid);
        }


        /// <summary>
        /// ��uid cid �õ�һ������
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="contrID"></param>
        /// <returns></returns>
        public BCW.Model.tb_QuestionsAnswerCtr Gettb_QuestionsAnswerCtrByUidCid(int uid, int contrID)
        {

            return dal.Gettb_QuestionsAnswerCtrByUidCid(uid, contrID);
        }
        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public BCW.Model.tb_QuestionsAnswerCtr Gettb_QuestionsAnswerCtr(int ID)
        {

            return dal.Gettb_QuestionsAnswerCtr(ID);
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
        /// <returns>IList tb_QuestionsAnswerCtr</returns>
        public IList<BCW.Model.tb_QuestionsAnswerCtr> Gettb_QuestionsAnswerCtrs(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.Gettb_QuestionsAnswerCtrs(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }

        #endregion  ��Ա����
    }
}

