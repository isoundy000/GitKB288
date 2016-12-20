using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL.Game
{
    /// <summary>
    /// ҵ���߼���Stone ��ժҪ˵����
    /// </summary>
    public class Stone
    {
        private readonly BCW.DAL.Game.Stone dal = new BCW.DAL.Game.Stone();
        public Stone()
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
        public bool Exists(int ID, int ptype)
        {
            return dal.Exists(ID, ptype);
        }

        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool Exists(int ID, int userId, int ptype)
        {
            return dal.Exists(ID, userId, ptype);
        }

        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public int GetStoneId(int userId)
        {
            return dal.GetStoneId(userId);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public void Add(BCW.Model.Game.Stone model)
        {
            dal.Add(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public void Update(BCW.Model.Game.Stone model)
        {
            dal.Update(model);
        }

        /// <summary>
        /// ����λ��1
        /// </summary>
        public void UpdateOne(BCW.Model.Game.Stone model)
        {
            dal.UpdateOne(model);
        }

        /// <summary>
        /// ����λ��2
        /// </summary>
        public void UpdateTwo(BCW.Model.Game.Stone model)
        {
            dal.UpdateTwo(model);
        }

        /// <summary>
        /// ����λ��3
        /// </summary>
        public void UpdateThr(BCW.Model.Game.Stone model)
        {
            dal.UpdateThr(model);
        }

        /// <summary>
        /// ����1
        /// </summary>
        public void UpdateOneShot(BCW.Model.Game.Stone model)
        {
            dal.UpdateOneShot(model);
        }

        /// <summary>
        /// ����2
        /// </summary>
        public void UpdateTwoShot(BCW.Model.Game.Stone model)
        {
            dal.UpdateTwoShot(model);
        }

        /// <summary>
        /// ����3
        /// </summary>
        public void UpdateThrShot(BCW.Model.Game.Stone model)
        {
            dal.UpdateThrShot(model);
        }

        /// <summary>
        /// ��ս����1
        /// </summary>
        public void UpdateOneShot2(BCW.Model.Game.Stone model)
        {
            dal.UpdateOneShot2(model);
        }

        /// <summary>
        /// ��ս����2
        /// </summary>
        public void UpdateTwoShot2(BCW.Model.Game.Stone model)
        {
            dal.UpdateTwoShot2(model);
        }

        /// <summary>
        /// �˳���Ϸ1
        /// </summary>
        public void UpdateOneExit(int ID)
        {
            dal.UpdateOneExit(ID);
        }

        /// <summary>
        /// �˳���Ϸ2
        /// </summary>
        public void UpdateTwoExit(int ID)
        {
            dal.UpdateTwoExit(ID);
        }

        /// <summary>
        /// �˳���Ϸ3
        /// </summary>
        public void UpdateThrExit(int ID)
        {
            dal.UpdateThrExit(ID);
        }

        /// <summary>
        /// ����1/2PK��ʶ
        /// </summary>
        public void UpdatePKab(int ID)
        {
            dal.UpdatePKab(ID);
        }

        /// <summary>
        /// ����2/3PK��ʶ
        /// </summary>
        public void UpdatePKbc(int ID)
        {
            dal.UpdatePKbc(ID);
        }

        /// <summary>
        /// ����1/3PK��ʶ
        /// </summary>
        public void UpdatePKac(int ID)
        {
            dal.UpdatePKac(ID);
        }

        /// <summary>
        /// ��������PK1
        /// </summary>
        public void UpdateLLone(int ID)
        {
            dal.UpdateLLone(ID);
        }

        /// <summary>
        /// ��������PK2
        /// </summary>
        public void UpdateLLtwo(int ID)
        {
            dal.UpdateLLtwo(ID);
        }

        /// <summary>
        /// ��������PK3
        /// </summary>
        public void UpdateLLthr(int ID)
        {
            dal.UpdateLLthr(ID);
        }

        /// <summary>
        /// ��������
        /// </summary>
        public void UpdateShot(int ID)
        {
            dal.UpdateShot(ID);
        }

        /// <summary>
        /// �������
        /// </summary>
        public void UpdateClear(int ID)
        {
            dal.UpdateClear(ID);
        }

        /// <summary>
        /// ֹͣPK״̬
        /// </summary>
        public void UpdateStat(int ID)
        {
            dal.UpdateStat(ID);
        }

        /// <summary>
        /// ������ע��
        /// </summary>
        public void UpdatePayCent(int ID, int PayCent)
        {
            dal.UpdatePayCent(ID, PayCent);
        }

        /// <summary>
        /// �����´γ�������
        /// </summary>
        public void UpdateNextShot(int ID, int NextShot)
        {
            dal.UpdateNextShot(ID, NextShot);
        }

        /// <summary>
        /// ���³�ʱʱ��
        /// </summary>
        public void UpdateTime(int ID)
        {
            dal.UpdateTime(ID);
        }

        /// <summary>
        /// ������������
        /// </summary>
        public void UpdateLines(int ID, string Lines)
        {
            dal.UpdateLines(ID, Lines);
        }

        /// <summary>
        /// ����������������
        /// </summary>
        public void UpdateOnline(int ID, int Online)
        {
            dal.UpdateOnline(ID, Online);
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
        public BCW.Model.Game.Stone GetStone(int ID)
        {

            return dal.GetStone(ID);
        }

        /// <summary>
        /// �õ�����StName
        /// </summary>
        public string GetStName(int ID)
        {

            return dal.GetStName(ID);
        }

        /// <summary>
        /// �õ�����Lines
        /// </summary>
        public string GetLines(int ID)
        {

            return dal.GetLines(ID);
        }

        /// <summary>
        /// �õ�Types
        /// </summary>
        public int GetTypes(int ID)
        {

            return dal.GetTypes(ID);
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
        /// <param name="strWhere">��ѯ����</param>
        /// <returns>IList Stone</returns>
        public IList<BCW.Model.Game.Stone> GetStones(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetStones(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }

        #endregion  ��Ա����
    }
}

