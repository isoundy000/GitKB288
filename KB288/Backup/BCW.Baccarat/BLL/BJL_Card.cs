using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Baccarat.Model;
namespace BCW.Baccarat.BLL
{
    /// <summary>
    /// ҵ���߼���BJL_Card ��ժҪ˵����
    /// </summary>
    public class BJL_Card
    {
        private readonly BCW.Baccarat.DAL.BJL_Card dal = new BCW.Baccarat.DAL.BJL_Card();
        public BJL_Card()
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
        /// ����һ������
        /// </summary>
        public int Add(BCW.Baccarat.Model.BJL_Card model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public void Update(BCW.Baccarat.Model.BJL_Card model)
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
        /// �õ�һ������ʵ��
        /// </summary>
        public BCW.Baccarat.Model.BJL_Card GetBJL_Card(int ID)
        {

            return dal.GetBJL_Card(ID);
        }

        /// <summary>
        /// �����ֶ�ȡ�����б�
        /// </summary>
        public DataSet GetList(string strField, string strWhere)
        {
            return dal.GetList(strField, strWhere);
        }

        //============================================
        /// <summary>
        /// �Ƿ����ĳ����ĳ������˿���
        /// </summary>
        /// <param name="RoomID"></param>
        /// <param name="RoomDoTable"></param>
        /// <returns></returns>
        public bool ExistsCard(int RoomID, int RoomDoTable)
        {
            return dal.ExistsCard(RoomID, RoomDoTable);
        }
        /// <summary>
        ///�õ��ض�����ID������table�����µ�����
        /// </summary>
        public BCW.Baccarat.Model.BJL_Card GetCardMessage(int RoomID, int RoomDoTable)
        {
            return dal.GetCardMessage(RoomID, RoomDoTable);
        }
        //============================================

        /// <summary>
        /// ȡ��ÿҳ��¼
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">��ҳ��С</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <returns>IList BJL_Card</returns>
        public IList<BCW.Baccarat.Model.BJL_Card> GetBJL_Cards(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetBJL_Cards(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }

        #endregion  ��Ա����
    }
}

