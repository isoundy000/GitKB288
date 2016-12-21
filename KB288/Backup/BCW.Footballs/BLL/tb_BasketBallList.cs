using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
    /// <summary>
    /// ҵ���߼���tb_BasketBallList ��ժҪ˵����
    /// </summary>
    public class tb_BasketBallList
    {
        private readonly BCW.DAL.tb_BasketBallList dal = new BCW.DAL.tb_BasketBallList();
        public tb_BasketBallList()
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
		public bool ExistsName(int name_en)
        {
            return dal.ExistsName(name_en);
        }
        /// <summary>
        /// ����һ������
        /// </summary>
        public int Add(BCW.Model.tb_BasketBallList model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// ����connectId
        /// ���������������
        /// </summary>
        public void UpdateConnectId(int ID, int LinkId)
        {
            dal.UpdateConnectId(ID, LinkId);
        }

        /// <summary>
        /// ����isHidden ������ʾ������
        /// </summary>
        public void UpdateHidden(int ID, int isHidden)
        {
            dal.UpdateHidden(ID, isHidden);
        }
        /// <summary>
        /// ���±ȷ�1-4�����ͱȷ� 
        /// </summary>
        public void UpdateOneScore(int ID, string homeone, string guestone)
        {
            dal.UpdateOneScore(ID, homeone, guestone);
        }
        /// <summary>
        /// ���±ȷ�2�����ͱȷ� 
        /// </summary>
        public void UpdateTwoScore(int ID, string hometwo, string guesttwo)
        {
            dal.UpdateTwoScore(ID, hometwo, guesttwo);
        }
        /// <summary>
        /// ���±ȷ�3�����ͱȷ� 
        /// </summary>
        public void UpdateThreeScore(int ID, string homethree, string guestthree)
        {
            dal.UpdateThreeScore(ID, homethree, guestthree);
        }
        /// <summary>
        /// ���±ȷ�4�����ͱȷ� 
        /// </summary>
        public void UpdateFourScore(int ID, string homefour, string guestfour)
        {
            dal.UpdateThreeScore(ID, homefour, guestfour);
        }
        /// <summary>
        /// ���±ȷ����� 
        /// </summary>
        public void UpdateScore(int ID, int home, int guest)
        {
            dal.UpdateScore(ID, home, guest);
        }
        /// <summary>
        /// ���½�������explain 1
        /// </summary>
        public void UpdateExplain(int ID, string explain, string explain2)
        {
            dal.UpdateExplain(ID, explain, explain2);
        }
        /// <summary>
        /// ����result
        /// </summary>
        public void UpdateResult(int ID, string result)
        {
            dal.UpdateResult(ID, result);
        }
        /// <summary>
        /// ����isDone
        /// </summary>
        public void UpdateisDone(int ID, string isDone)
        {
            dal.UpdateisDone(ID, isDone);
        }
        /// <summary>
        /// ����matchstate
        /// </summary>
        public void Updatematchstate(int ID, string matchstate)
        {
            dal.Updatematchstate(ID, matchstate);
        }
        /// <summary>
        /// ����Europe
        /// </summary>
        public void UpdateEurope(int ID, string homeEurope, string guestEurope)
        {
            dal.UpdateEurope(ID, homeEurope, guestEurope);
        }
        /// <summary>
        /// ����һ������
        /// </summary>
        public void Update(BCW.Model.tb_BasketBallList model)
        {
            dal.Update(model);
        }
        /// <summary>
		/// ����һ������Name_en
		/// </summary>
		public void UpdateName_en(BCW.Model.tb_BasketBallList model)
        {
            dal.UpdateName_en(model);
        }
        /// <summary>
        /// ����һ������Name_en1
        /// </summary>
        public void UpdateName_en1(BCW.Model.tb_BasketBallList model)
        {
            dal.Updatename_en1(model);
        }
        /// <summary>
        /// ����һ������Name_en1
        /// ���� ����ʱ�� �ȷ� ŷ�� 
        /// </summary>
        public void UpdateName_en2(BCW.Model.tb_BasketBallList model)
        {
            dal.Updatename_en2(model);
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
        public BCW.Model.tb_BasketBallList Gettb_BasketBallList(int ID)
        {

            return dal.Gettb_BasketBallList(ID);
        }

        /// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public BCW.Model.tb_BasketBallList Gettb_BasketBallListForName_en(int Name_en)
        {

            return dal.Gettb_BasketBallListForName_en(Name_en);
        }
        /// <summary>
		/// �õ����
		/// </summary>
        public int GetName_enFromId(int ID)
        {
            return dal.GetName_enFromId(ID);
        }

        /// <summary>
		/// �õ�����״̬
		/// </summary>
        public string GetStateFromId(int ID)
        {
            return dal.GetStateFromId(ID);
        }

        /// <summary>
        /// �õ����
        /// </summary>
        public int GetIDFromName_en(int name_en)
        {
            return dal.GetIDFromName_en(name_en);
        }
        /// <summary>
		/// ͨ��ID�õ����ӱȷ�
		/// </summary>
        public int GetHomeScoreFromId(int ID)
        {
            return dal.GetHomeScoreFromId(ID);
        }
        /// <summary>
		/// �õ��Ͷ�ʵ��
		/// </summary>
        public int GetGuestScoreFromId(int ID)
        {
            return dal.GetGuestScoreFromId(ID);
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
        /// <returns>IList tb_BasketBallList</returns>
        public IList<BCW.Model.tb_BasketBallList> Gettb_BasketBallLists(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.Gettb_BasketBallLists(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }

        #endregion  ��Ա����
    }
}

