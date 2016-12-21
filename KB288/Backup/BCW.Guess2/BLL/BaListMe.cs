using System;
using System.Data;
using System.Collections.Generic;
using TPR2.Common;
using TPR2.Model.guess;
namespace TPR2.BLL.guess
{
    /// <summary>
    /// ҵ���߼���BaListMe ��ժҪ˵����
    /// </summary>
    public class BaListMe
    {
        private readonly TPR2.DAL.guess.BaListMe dal = new TPR2.DAL.guess.BaListMe();
        public BaListMe()
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
        public int GetCount(TPR2.Model.guess.BaListMe model)
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
        public int GetBaListMeCount(string strWhere)
        {
            return dal.GetBaListMeCount(strWhere);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public int Add(TPR2.Model.guess.BaListMe model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// ��������������һ������
        /// </summary>
        public int FootAdd(TPR2.Model.guess.BaListMe model)
        {
            return dal.FootAdd(model);
        }

        /// <summary>
        /// ����������������ʾ
        /// </summary>
        public void Updatep_del(TPR2.Model.guess.BaListMe model)
        {
            dal.Updatep_del(model);
        }

        /// <summary>
        /// ��������ץȡ�벻ץȡ
        /// </summary>
        public void Updatep_jc(TPR2.Model.guess.BaListMe model)
        {
            dal.Updatep_jc(model);
        }

        /// <summary>
        /// �������½��״̬
        /// </summary>
        public void Updatep_zd(TPR2.Model.guess.BaListMe model)
        {
            dal.Updatep_zd(model);
        }

        /// <summary>
        /// �������±ȷ�
        /// </summary>
        public void UpdateResult(TPR2.Model.guess.BaListMe model)
        {
            dal.UpdateResult(model);
        }

        /// <summary>
        /// �Զ��������±ȷ�
        /// </summary>
        public int UpdateZDResult(TPR2.Model.guess.BaList model)
        {
            return dal.UpdateZDResult(model);
        }

        /// <summary>
        /// ���½���ʱ�伯��
        /// </summary>
        public void UpdateOnce(TPR2.Model.guess.BaListMe model)
        {
            dal.UpdateOnce(model);
        }
               
        /// <summary>
        /// ���½���ʱ�伯��
        /// </summary>
        public void UpdateOnce(int p_id, string p_once)
        {
            dal.UpdateOnce(p_id, p_once);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public void Update(TPR2.Model.guess.BaListMe model)
        {
            dal.Update(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public void Update2(TPR2.Model.guess.BaListMe model)
        {
            dal.Update2(model);
        }

        /// <summary>
        /// ��������һ������
        /// </summary>
        public void BasketUpdate(TPR2.Model.guess.BaList model)
        {
            dal.BasketUpdate(model);
        }

        /// <summary>
        /// ��������������
        /// </summary>
        public void FootUpdate(TPR2.Model.guess.BaList model)
        {
            dal.FootUpdate(model);
        }

        /// <summary>
        /// ���������С��
        /// </summary>
        public void FootdxUpdate(TPR2.Model.guess.BaList model)
        {
            dal.FootdxUpdate(model);
        }

        /// <summary>
        /// ���������׼��
        /// </summary>
        public void FootbzUpdate(TPR2.Model.guess.BaList model)
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
        /// �����Ƿ����
        /// </summary>
        public void Updatep_isluck(int p_id, int state, int Types)
        {
            dal.Updatep_isluck(p_id, state, Types);
        }

        /// <summary>
        /// �����Ƿ����
        /// </summary>
        public void Updatep_isluck2(int id, int state, int Types)
        {
            dal.Updatep_isluck2(id, state, Types);
        }

        /// <summary>
        /// ���¼�ʱ�ȷ֣��ߵ�ʹ�ã�
        /// </summary>
        public void FootOnceUpdate(TPR2.Model.guess.BaList model)
        {
            dal.FootOnceUpdate(model);
        }
               
        /// <summary>
        /// ���¼�ʱ�ȷ�8��
        /// </summary>
        public int UpdateBoResult2(int p_id, int p_result_temp1, int p_result_temp2)
        {
            return dal.UpdateBoResult2(p_id, p_result_temp1, p_result_temp2);
        }
        /// <summary>
        /// ���¼�ʱ�ȷ�8��
        /// </summary>
        public int UpdateBoResult3(int p_id, int p_result_temp1, int p_result_temp2)
        {
            return dal.UpdateBoResult3(p_id, p_result_temp1, p_result_temp2);
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
        public TPR2.Model.guess.BaListMe GetBasketOpen(DateTime p_TPRtime, string p_one, string p_two)
        {
            return dal.GetBasketOpen(p_TPRtime, p_one, p_two);
        }

        /// <summary>
        /// �õ����򿪽������ʵ��
        /// </summary>
        public TPR2.Model.guess.BaListMe GetFootOpen(string p_title, string p_one, string p_two)
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
        /// �õ����±ȷָ���ʱ��
        /// </summary>
        public DateTime Getp_temptime(int ID)
        {
            return dal.Getp_temptime(ID);
        }

        /// <summary>
        /// �õ�p_temptime_p_id
        /// </summary>
        public TPR2.Model.guess.BaListMe Getp_temptime_p_id(int ID)
        {
            return dal.Getp_temptime_p_id(ID);
        }

        /// <summary>
        /// �õ���ʱ�ȷ�
        /// </summary>
        public TPR2.Model.guess.BaListMe GetTemp(int ID)
        {
            return dal.GetTemp(ID);
        }

        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public TPR2.Model.guess.BaListMe GetModel(int ID)
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
        public IList<TPR2.Model.guess.BaListMe> GetBaListMes(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
        {
            return dal.GetBaListMes(p_pageIndex, p_pageSize, strWhere, strOrder, out p_recordCount);
        }

        /// <summary>
        /// ȡ��������¼
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">��ҳ��С</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <returns>IList</returns>
        public IList<TPR2.Model.guess.BaListMe> GetBaListMeLX(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetBaListMeLX(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }

        /// <summary>
        /// ȡ��δ�������¼�¼
        /// </summary>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <returns>IList</returns>
        public IList<TPR2.Model.guess.BaListMe> GetBaListMeBF(string strWhere, out int p_recordCount)
        {
            return dal.GetBaListMeBF(strWhere, out p_recordCount);
        }

        #endregion  ��Ա����
    }
}

