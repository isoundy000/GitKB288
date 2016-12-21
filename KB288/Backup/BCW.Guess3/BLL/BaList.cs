using System;
using System.Data;
using System.Collections.Generic;
using TPR3.Common;
using TPR3.Model.guess;
namespace TPR3.BLL.guess
{
    /// <summary>
    /// ҵ���߼���BaList ��ժҪ˵����
    /// </summary>
    public class BaList
    {
        private readonly TPR3.DAL.guess.BaList dal = new TPR3.DAL.guess.BaList();
        public BaList()
        { }
        #region  ��Ա����

              
        /// <summary>
        /// �ǲ����ߵ�
        /// </summary>
        public bool Existsp_ison(int p_id)
        {
            return dal.Existsp_ison(p_id);
        }
 
        /// <summary>
        /// ���±���״̬
        /// </summary>
        public void UpdateOnce(int p_id, string p_once)
        {
            dal.UpdateOnce(p_id, p_once);
        }


        /// <summary>
        /// ����һ������(�����ϰ볡ҳ��ʹ��)
        /// </summary>
        public void UpdateFalf(TPR3.Model.guess.BaList model)
        {
            dal.UpdateFalf(model);
        }

        /// <summary>
        /// ����һ������(�����ϰ볡ҳ��ʹ��)
        /// </summary>
        public void UpdateFalf1(TPR3.Model.guess.BaList model)
        {
            dal.UpdateFalf1(model);

        }

        /// <summary>
        /// ����һ������(�����ϰ볡ҳ��ʹ��)
        /// </summary>
        public void UpdateFalf2(TPR3.Model.guess.BaList model)
        {
            dal.UpdateFalf2(model);

        }

        /// <summary>
        /// ����һ������(�����ϰ볡ҳ��ʹ��)
        /// </summary>
        public void UpdateFalf3(TPR3.Model.guess.BaList model)
        {
            dal.UpdateFalf3(model);

        }

        /// <summary>
        /// ��������������
        /// </summary>
        public void BasketUpdateYp(TPR3.Model.guess.BaList model)
        {
            dal.BasketUpdateYp(model);
        }

        /// <summary>
        /// ���������С��
        /// </summary>
        public void BasketUpdateDx(TPR3.Model.guess.BaList model)
        {
            dal.BasketUpdateDx(model);
        }

        /// <summary>
        /// ����Ϊ����ģʽ������ʹ�ã�
        /// </summary>
        public void FootOnceType3(int p_id, DateTime dt)
        {
            dal.FootOnceType3(p_id, dt);
        }

        /// <summary>
        /// �õ���������
        /// </summary>
        public string Getp_title(int p_id)
        {
            return dal.Getp_title(p_id);
        }

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
        /// �õ�p_temptime_p_id
        /// </summary>
        public TPR3.Model.guess.BaList Getp_temptime_p_id(int ID)
        {
            return dal.Getp_temptime_p_id(ID);
        }

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
        public bool ExistsByp_id(int p_id, int p_se)
        {
            return dal.ExistsByp_id(p_id, p_se);
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
        public int GetCount(TPR3.Model.guess.BaList model)
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
        public int Add(TPR3.Model.guess.BaList model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// ��������������һ������
        /// </summary>
        public int FootAdd(TPR3.Model.guess.BaList model)
        {
            return dal.FootAdd(model);
        }

        /// <summary>
        /// ����������������ʾ
        /// </summary>
        public void Updatep_del(TPR3.Model.guess.BaList model)
        {
            dal.Updatep_del(model);
        }
               
        /// <summary>
        /// ��������״̬���
        /// </summary>
        public void Updatep_active(int ID, int p_active)
        {
            dal.Updatep_active(ID, p_active);
        }

        /// <summary>
        /// ��������ץȡ�벻ץȡ
        /// </summary>
        public void Updatep_jc(TPR3.Model.guess.BaList model)
        {
            dal.Updatep_jc(model);
        }

        /// <summary>
        /// �������½��״̬
        /// </summary>
        public void Updatep_zd(TPR3.Model.guess.BaList model)
        {
            dal.Updatep_zd(model);
        }

        /// <summary>
        /// �������±ȷ�
        /// </summary>
        public void UpdateResult(TPR3.Model.guess.BaList model)
        {
            dal.UpdateResult(model);
        }

        /// <summary>
        /// �Զ��������±ȷ�
        /// </summary>
        public int UpdateZDResult(TPR3.Model.guess.BaList model)
        {
            return dal.UpdateZDResult(model);
        }
                
        /// <summary>
        /// �����곡�ȷ�8��
        /// </summary>
        public int UpdateBoResult(int p_id, int p_result_one, int p_result_two)
        {
            return dal.UpdateBoResult(p_id, p_result_one, p_result_two);
        }
               
        /// <summary>
        /// ���¼�ʱ�ȷ�8��
        /// </summary>
        public void UpdateBoResult2(int p_id, int p_result_temp1, int p_result_temp2)
        {
            dal.UpdateBoResult2(p_id, p_result_temp1, p_result_temp2);
        }
        /// <summary>
        /// ���¼�ʱ�ȷ�8��
        /// </summary>
        public void UpdateBoResult3(int p_id, int p_result_temp1, int p_result_temp2)
        {
            dal.UpdateBoResult3(p_id, p_result_temp1, p_result_temp2);
        }
        /// <summary>
        /// ���½���ʱ�伯��
        /// </summary>
        public void UpdateOnce(TPR3.Model.guess.BaList model)
        {
            dal.UpdateOnce(model);
        }
               
        ///// <summary>
        ///// ���½���ʱ�伯��
        ///// </summary>
        //public void UpdateOnce(int p_id, string p_once)
        //{
        //    dal.UpdateOnce(p_id, p_once);
        //}

        /// <summary>
        /// ����һ������
        /// </summary>
        public void Update(TPR3.Model.guess.BaList model)
        {
            dal.Update(model);
        }

        /// <summary>
        /// ��������һ������
        /// </summary>
        public void BasketUpdate(TPR3.Model.guess.BaList model)
        {
            dal.BasketUpdate(model);
        }
                
        /// <summary>
        /// ����һ������
        /// </summary>
        public void BasketUpdateOdds(TPR3.Model.guess.BaList model)
        {
            dal.BasketUpdateOdds(model);
        }

        /// <summary>
        /// ��������������
        /// </summary>
        public void FootUpdate(TPR3.Model.guess.BaList model)
        {
            dal.FootUpdate(model);
        }
               
        /// <summary>
        /// ����������������
        /// </summary>
        public void FootypUpdate(TPR3.Model.guess.BaList model)
        {
            dal.FootypUpdate(model);
        }

        /// <summary>
        /// ���������С��
        /// </summary>
        public void FootdxUpdate(TPR3.Model.guess.BaList model)
        {
            dal.FootdxUpdate(model);
        }

        /// <summary>
        /// ���������׼��
        /// </summary>
        public void FootbzUpdate(TPR3.Model.guess.BaList model)
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
        /// ����Ϊ����ģʽ
        /// </summary>
        public void FootOnceType2(int p_id, DateTime dt)
        {
            dal.FootOnceType2(p_id, dt);
        }
                
        /// <summary>
        /// ���º���
        /// </summary>
        public void UpdateRed(int p_id, int p_zred, int p_kred)
        {
            dal.UpdateRed(p_id, p_zred, p_kred);
        }
 
        /// <summary>
        /// ���»���
        /// </summary>
        public void UpdateYellow(int p_id, int p_zyellow, int p_kyellow)
        {
            dal.UpdateYellow(p_id, p_zyellow, p_kyellow);
        }

        /// <summary>
        /// �����Ƿ����
        /// </summary>
        public void Updatep_isluck(int p_id, int state, int Types,DateTime p_temptime)
        {
            dal.Updatep_isluck(p_id, state, Types, p_temptime);
        }
        /// <summary>
        /// �����Ƿ����
        /// </summary>
        public void Updatep_isluck(int p_id, int state, int Types)
        {
            dal.Updatep_isluck(p_id, state, Types, DateTime.Now);
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
        public void FootOnceUpdate(TPR3.Model.guess.BaList model)
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
        /// �õ�p_id
        /// </summary>
        public int Getp_id(int ID)
        {
            return dal.Getp_id(ID);
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
        public TPR3.Model.guess.BaList GetBasketOpen(DateTime p_TPRtime, string p_one, string p_two)
        {
            return dal.GetBasketOpen(p_TPRtime, p_one, p_two);
        }

        /// <summary>
        /// �õ����򿪽������ʵ��
        /// </summary>
        public TPR3.Model.guess.BaList GetFootOpen(string p_title, string p_one, string p_two)
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
        /// �õ���ʱ�ȷ�
        /// </summary>
        public TPR3.Model.guess.BaList GetTemp(int ID)
        {
            return dal.GetTemp(ID);
        }
  
        /// <summary>
        /// �õ����ƻ��ơ���ʱ�ȷ�
        /// </summary>
        public TPR3.Model.guess.BaList GetRedYellow(int p_id)
        {
            return dal.GetRedYellow(p_id);
        }

        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public TPR3.Model.guess.BaList GetModel(int ID)
        {
            return dal.GetModel(ID, -1);
        }

        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public TPR3.Model.guess.BaList GetModelByp_id(int p_id)
        {
            return dal.GetModel(p_id, 0);
        }

        /// <summary>
        /// �����ֶ�ȡ�����б�
        /// </summary>
        public DataSet GetBaListList(string strField, string strWhere)
        {
            return dal.GetBaListList(strField, strWhere);
        }

        /// <summary>
        /// ȡ��ÿҳ��¼
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">��ҳ��С</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <returns>IList</returns>
        public IList<TPR3.Model.guess.BaList> GetBaLists(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
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
        public IList<TPR3.Model.guess.BaList> GetBaListLX(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetBaListLX(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }

        /// <summary>
        /// ȡ��δ�������¼�¼
        /// </summary>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <returns>IList</returns>
        public IList<TPR3.Model.guess.BaList> GetBaListBF(string strWhere, out int p_recordCount)
        {
            return dal.GetBaListBF(strWhere, out p_recordCount);
        }

        #endregion  ��Ա����
    }
}

