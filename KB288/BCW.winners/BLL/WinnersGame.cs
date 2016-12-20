using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
    /// <summary>
    /// ҵ���߼���tb_WinnersGame ��ժҪ˵����
    /// </summary>
    public class tb_WinnersGame
    {
        private readonly BCW.DAL.tb_WinnersGame dal = new BCW.DAL.tb_WinnersGame();
        public tb_WinnersGame()
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
        /// �Ƿ���ڸ���Ϸ
        /// </summary>
        public bool ExistsGameName(string GameName)
        {
            return dal.ExistsGameName(GameName);
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
        public int Add(BCW.Model.tb_WinnersGame model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// ͨ��Id����
        /// price��ֵ
        /// /// </summary>
        public void UpdatePrice(int ID, long price)
        {
            dal.UpdatePrice(ID, price);
        }

        /// <summary>
        /// ͨ��Id����
        /// price��ֵ
        /// /// </summary>
        public void UpdatePriceForIdent(string Ident, long price)
        {
            dal.UpdatePriceForIdent(Ident, price);
        }
        /// <summary>
        /// ͨ��Id����
        /// price ptype��ֵ
        /// /// </summary>
        public void UpdatePricePtypeForIdent(string Ident, long price, int ptype)
        {
            dal.UpdatePricePtypeForIdent(Ident, price, ptype);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public void Update(BCW.Model.tb_WinnersGame model)
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
        public BCW.Model.tb_WinnersGame Gettb_WinnersGame(int ID)
        {

            return dal.Gettb_WinnersGame(ID);
        }

        /// <summary>
        /// ��GameName�õ�һ�����Ͷע
        /// </summary>
        public long GetPrice(string name)
        {
            return dal.GetPrice(name);
        }

        /// <summary>
        /// ��GameName�õ�һ�����Ͷע
        /// </summary>
        public int GetPtype(string name)
        {
            return dal.GetPtype(name);
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
        /// <returns>IList tb_WinnersGame</returns>
        public IList<BCW.Model.tb_WinnersGame> Gettb_WinnersGames(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.Gettb_WinnersGames(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }

        #endregion  ��Ա����
    }
}

