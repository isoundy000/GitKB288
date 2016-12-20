using System;
using System.Data;
using System.Collections.Generic;
using TPR.Common;
using TPR.Model.guess;
namespace TPR.BLL.guess
{
    /// <summary>
    /// ҵ���߼���BaList ��ժҪ˵����
    /// </summary>
    public class BaList
    {
        private readonly TPR.DAL.guess.BaList dal = new TPR.DAL.guess.BaList();
        public BaList()
        { }
        #region  ��Ա����

               
        //---------------------------����Ͷעʹ��----------------------
        /// <summary>
        /// ������ȷ������Ͷע��ԱID
        /// </summary>
        public void Updatep_usid(int ID, string p_usid)
        {
            dal.Updatep_usid(ID, p_usid);
        }
              
        /// <summary>
        /// �õ�����Ͷע��ԱID
        /// </summary>
        public string Getp_usid(int ID)
        {
            return dal.Getp_usid(ID);
        }

        //---------------------------����Ͷעʹ��----------------------
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
        public bool ExistsByp_id(int p_id)
        {
            return dal.ExistsByp_id(p_id);
        }
               
        /// <summary>
        /// �Ƿ������ش�С��
        /// </summary>
        public bool ExistsDX(int p_id)
        {
            return dal.ExistsDX(p_id);
        }
               
        /// <summary>
        /// �Ƿ������ر�׼��
        /// </summary>
        public bool ExistsBZ(int p_id)
        {
            return dal.ExistsBZ(p_id);
        }

        /// <summary>
        /// �õ���ѯ�ļ�¼��
        /// </summary>
        public int GetCount(TPR.Model.guess.BaList model)
        {
            return dal.GetCount(model);
        }

        /// <summary>
        /// �õ���ѯ�ļ�¼��
        /// </summary>
        public int GetCountByp_title(string p_title)
        {
            return dal.GetCountByp_title(p_title);
        }

        /// <summary>
        /// ���������õ�������ע��
        /// </summary>
        public int GetBaListCount(string strWhere)
        {
            return dal.GetBaListCount(strWhere);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public int Add(TPR.Model.guess.BaList model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// ��������������һ������
        /// </summary>
        public int FootAdd(TPR.Model.guess.BaList model)
        {
            return dal.FootAdd(model);
        }

        /// <summary>
        /// ����������������ʾ
        /// </summary>
        public void Updatep_del(TPR.Model.guess.BaList model)
        {
            dal.Updatep_del(model);
        }

        /// <summary>
        /// ��������ץȡ�벻ץȡ
        /// </summary>
        public void Updatep_jc(TPR.Model.guess.BaList model)
        {
            dal.Updatep_jc(model);
        }

        /// <summary>
        /// �������½��״̬
        /// </summary>
        public void Updatep_zd(TPR.Model.guess.BaList model)
        {
            dal.Updatep_zd(model);
        }

        /// <summary>
        /// �������±ȷ�
        /// </summary>
        public void UpdateResult(TPR.Model.guess.BaList model)
        {
            dal.UpdateResult(model);
        }

        /// <summary>
        /// �Զ��������±ȷ�
        /// </summary>
        public int UpdateZDResult(TPR.Model.guess.BaList model)
        {
            return dal.UpdateZDResult(model);
        }

        /// <summary>
        /// ���½���ʱ�伯��
        /// </summary>
        public void UpdateOnce(TPR.Model.guess.BaList model)
        {
            dal.UpdateOnce(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public void Update(TPR.Model.guess.BaList model)
        {
            dal.Update(model);
        }

        /// <summary>
        /// ��������һ������
        /// </summary>
        public void BasketUpdate(TPR.Model.guess.BaList model)
        {
            dal.BasketUpdate(model);
        }

        /// <summary>
        /// ��������������
        /// </summary>
        public void FootUpdate(TPR.Model.guess.BaList model)
        {
            dal.FootUpdate(model);
        }

        /// <summary>
        /// ���������С��
        /// </summary>
        public void FootdxUpdate(TPR.Model.guess.BaList model)
        {
            dal.FootdxUpdate(model);
        }

        /// <summary>
        /// ���������׼��
        /// </summary>
        public void FootbzUpdate(TPR.Model.guess.BaList model)
        {
            dal.FootbzUpdate(model);
        }

        /// <summary>
        /// ����Ϊ�ߵ�ģʽ
        /// </summary>
        public void FootOnceType(int ID, DateTime dt)
        {
            dal.FootOnceType(ID, dt);
        }

        /// <summary>
        /// ���¼�ʱ�ȷ֣��ߵ�ʹ�ã�
        /// </summary>
        public void FootOnceUpdate(TPR.Model.guess.BaList model)
        {
            dal.FootOnceUpdate(model);
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
        public void DeleteStr(string strWhere)
        {
            dal.DeleteStr(strWhere);
        }
               
        /// <summary>
        /// �õ�������
        /// </summary>
        public int GetPtype(int ID)
        {
            return dal.GetPtype(ID);
        }

        /// <summary>
        /// �õ�����ʱ��
        /// </summary>
        public DateTime Getp_TPRtime(int p_id)
        {
            return dal.Getp_TPRtime(p_id);
        }

        /// <summary>
        /// �õ�p_id/�ڶ��ֱ�׼�̸�����
        /// </summary>
        public int Getp_id(DateTime p_TPRtime, string p_one, string p_two)
        {
            return dal.Getp_id(p_TPRtime, p_one, p_two);
        }

        /// <summary>
        /// �õ����򿪽������ʵ��
        /// </summary>
        public TPR.Model.guess.BaList GetBasketOpen(DateTime p_TPRtime, string p_one, string p_two)
        {
            return dal.GetBasketOpen(p_TPRtime, p_one, p_two);
        }

        /// <summary>
        /// �õ����򿪽������ʵ��
        /// </summary>
        public TPR.Model.guess.BaList GetFootOpen(string p_title, string p_one, string p_two)
        {
            return dal.GetFootOpen(p_title, p_one, p_two);
        }

        /// <summary>
        /// �õ��Ƿ����ߵ�
        /// </summary>
        public int Getison(int ID)
        {
            return dal.Getison(ID);
        }

        /// <summary>
        /// �õ��Ƿ����ߵ�
        /// </summary>
        public int Getisonht(int ID)
        {
            return dal.Getisonht(ID);
        }

        /// <summary>
        /// �õ��ߵظ���ʱ�伯��
        /// </summary>
        public string Getonce(int ID)
        {
            return dal.Getonce(ID);
        }
        
        /// <summary>
        /// �õ��ȷָ���ʱ�伯��
        /// </summary>
        public string Getp_temptimes(int ID)
        {
            return dal.Getp_temptimes(ID);
        }

        /// <summary>
        /// �õ���ʱ�ȷ�
        /// </summary>
        public TPR.Model.guess.BaList GetTemp(int ID)
        {
            return dal.GetTemp(ID);
        }

        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public TPR.Model.guess.BaList GetModel(int ID)
        {
            return dal.GetModel(ID);
        }

        /// <summary>
        /// ȡ��ÿҳ��¼
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">��ҳ��С</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <returns>IList</returns>
        public IList<TPR.Model.guess.BaList> GetBaLists(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
        {
            return dal.GetBaLists(p_pageIndex, p_pageSize, strWhere, strOrder, out p_recordCount);
        }

        /// <summary>
        /// ȡ��������¼
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">��ҳ��С</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <returns>IList</returns>
        public IList<TPR.Model.guess.BaList> GetBaListLX(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetBaListLX(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }

        /// <summary>
        /// ȡ��δ�������¼�¼
        /// </summary>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <returns>IList</returns>
        public IList<TPR.Model.guess.BaList> GetBaListBF(string strWhere, out int p_recordCount)
        {
            return dal.GetBaListBF(strWhere, out p_recordCount);
        }

        #endregion  ��Ա����
    }
}

