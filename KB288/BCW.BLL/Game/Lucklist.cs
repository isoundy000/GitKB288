using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model.Game;
namespace BCW.BLL.Game
{
    /// <summary>
    /// ҵ���߼���Lucklist ��ժҪ˵����
    /// </summary>
    public class Lucklist
    {
        private readonly BCW.DAL.Game.Lucklist dal = new BCW.DAL.Game.Lucklist();
        public Lucklist()
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
        /// ͳ������
        /// </summary>
        /// <returns></returns>
        public int GetCount()
        {
            return dal.GetCount();
        }

        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool Exists()
        {
            return dal.Exists();
        }
           /// <summary>
        /// �Ƿ���ڵ��ں�
        /// </summary>
        public bool ExistsBJQH(int Bjkl8Qihao)
        {
            return dal.ExistsBJQH(Bjkl8Qihao);
        }
        /// <summary>
        /// �Ƿ���ڸ�ʱ��ε��ں�
        /// </summary>
        public bool ExistsEndTime(string EndTime)
        {
            return dal.ExistsEndTime(EndTime);
        }
         /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool Existspanduan(string panduan)
        {
            return dal.Existspanduan(panduan);
        }
        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool Exists(int ID)
        {
            return dal.Exists(ID);
        }
          /// <summary>
        /// �Ƿ���ڸ�ץȡ���ں�
        /// </summary>
        public bool ExistsQihao(string Qihao)
        {
            return dal.ExistsQihao(Qihao);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public int Add(BCW.Model.Game.Lucklist model)
        {
            return dal.Add(model);
        }
        /// <summary>
        /// ����һ������
        /// </summary>
        public int Add2(BCW.Model.Game.Lucklist model)
        {
            return dal.Add2(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public void Update(BCW.Model.Game.Lucklist model)
        {
            dal.Update(model);
        }
        /// <summary>
        /// ����Bjkl8Qihao����
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Update3(BCW.Model.Game.Lucklist model)
        {
           return dal.Update3(model);
        }
        /// <summary>
        /// ���±�������
        /// </summary>
        public int Update2(BCW.Model.Game.Lucklist model)
        {
            return dal.Update2(model);
        }

        /// <summary>
        /// ���±��ڼ�¼
        /// </summary>
        public void Update(int ID, int SumNum)
        {
            dal.Update(ID, SumNum);
        }


        /// <summary>
        /// ���½��ػ���
        /// </summary>
        public void UpdatePool(int ID, long Pool)
        {
            dal.UpdatePool(ID, Pool);
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
        public BCW.Model.Game.Lucklist GetLucklist(int ID)
        {

            return dal.GetLucklist(ID);
        }
         /// <summary>
        /// �õ���һ��δ����������
        /// </summary>
        public BCW.Model.Game.Lucklist GetNextLucklist()
        {
            return dal.GetNextLucklist();
        }
          /// <summary>
        /// ����ʱ���ȡ����һ������
        /// </summary>
        public BCW.Model.Game.Lucklist GetLucklistByTime()
        {
           return dal.GetLucklistByTime();
        }
         /// <summary>
        /// �õ����ڶ���ʵ��
        /// </summary>
        public BCW.Model.Game.Lucklist GetLucklistSecond()
        {
            return dal.GetLucklistSecond();
        }
           /// <summary>
        /// �õ�����һ�ڵ�״̬
        /// </summary>
        public BCW.Model.Game.Lucklist GetLucklistState()
        {
            return dal.GetLucklistState();
        }
        /// <summary>
        /// �õ����δ��������ʵ��
        /// </summary>
        public BCW.Model.Game.Lucklist GetLucklist()
        {
            return dal.GetLucklist();
        }
        ///// <summary>
        ///// �õ����ڶ���ʵ��
        ///// </summary>
        //public BCW.Model.Game.Lucklist GetLucklist1()
        //{

        //    return dal.GetLucklist1();
        //}



        /// <summary>
        /// ����Bjkl8Qihao�õ���Ӧ���ݵ�ID
        /// </summary>
        public int GetID(int Bjkl8Qihao)
        {
            return dal.GetID(Bjkl8Qihao);
        }
        /// <summary>
        /// �õ�����Ŀ�ʼ����
        /// </summary>
        public string GetPanduan()
        {
            return dal.GetPanduan();
        }
        /// <summary>
        /// �õ����ػ���
        /// </summary>
        public long GetPool(int ID)
        {
            return dal.GetPool(ID);
        }
        /// <summary>
        /// �����ֶ�ȡ�����б�
        /// </summary>
        public DataSet GetList(string strField, string strWhere)
        {
            return dal.GetList(strField, strWhere);
        }

        /// <summary>
        /// ȡ�ù̶��б��¼
        /// </summary>
        /// <param name="SizeNum">�б��¼��</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <returns>IList Lucklist</returns>
        public IList<BCW.Model.Game.Lucklist> GetLucklists(int SizeNum, string strWhere)
        {
            return dal.GetLucklists(SizeNum, strWhere);
        }

        /// <summary>
        /// ȡ��ÿҳ��¼
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">��ҳ��С</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <returns>IList Lucklist</returns>
        public IList<BCW.Model.Game.Lucklist> GetLucklists(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetLucklists(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }

        #endregion  ��Ա����
    }
}

