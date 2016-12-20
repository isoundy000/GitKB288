using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model.Game;
namespace BCW.BLL.Game
{
    /// <summary>
    /// ҵ���߼���Mora ��ժҪ˵����
    /// </summary>
    public class Mora
    {
        private readonly BCW.DAL.Game.Mora dal = new BCW.DAL.Game.Mora();
        public Mora()
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
        /// �Ƿ���ڶҽ���¼
        /// </summary>
        public bool ExistsState(int ID, int UsID)
        {
            return dal.ExistsState(ID, UsID);
        }

        /// <summary>
        /// ����ĳ�û������ȭ����
        /// </summary>
        public int GetCount(int UsID)
        {
            return dal.GetCount(UsID);
        }

        /// <summary>
        /// ����ĳ�û������ȭ����
        /// </summary>
        public int GetCount2(int UsID)
        {
            return dal.GetCount2(UsID);
        }

        /// <summary>
        /// ��������ȭ����
        /// </summary>
        public int GetCount()
        {
            return dal.GetCount();
        }

        /// <summary>
        /// ��������ȭ�ܱ�ֵ
        /// </summary>
        public long GetPrice(int Types)
        {
            return dal.GetPrice(Types);
        }
        
        /// <summary>
        /// �����ȭ�ܱ�ֵ
        /// </summary>
        public long GetPrice(string strWhere)
        {
            return dal.GetPrice(strWhere);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public int Add(BCW.Model.Game.Mora model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public void Update(BCW.Model.Game.Mora model)
        {
            dal.Update(model);
        }
      
        /// <summary>
        /// ����ׯ�ҳ�ȭ
        /// </summary>
        public void UpdateTrueType(int ID, int TrueType)
        {
            dal.UpdateTrueType(ID, TrueType);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public void UpdateState(BCW.Model.Game.Mora model)
        {
            dal.UpdateState(model);
        }

        /// <summary>
        /// ����Ϊ������ȭ
        /// </summary>
        public void UpdateState2(int ID)
        {
            dal.UpdateState2(ID);
        }

        /// <summary>
        /// ����״̬
        /// </summary>
        public void UpdateState(int ID, int State)
        {
            dal.UpdateState(ID, State);
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
        public BCW.Model.Game.Mora GetMora(int ID)
        {

            return dal.GetMora(ID);
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
        /// <returns>IList Mora</returns>
        public IList<BCW.Model.Game.Mora> GetMoras(int SizeNum, string strWhere)
        {
            return dal.GetMoras(SizeNum, strWhere);
        }

        /// <summary>
        /// ȡ��ÿҳ��¼
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">��ҳ��С</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <returns>IList Mora</returns>
        public IList<BCW.Model.Game.Mora> GetMoras(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetMoras(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }

        #endregion  ��Ա����
    }
}

