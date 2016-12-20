using System;
using System.Data;
using System.Collections.Generic;
using TPR2.Common;
using TPR2.Model.guess;

/// <summary>
/// 
/// �޸������Ƿ��Զ��������� �ƹ��� 20160322
/// </summary>
namespace TPR2.BLL.guess
{
    /// <summary>
    /// ҵ���߼���BaList ��ժҪ˵����
    /// </summary>
    public class BaList
    {
        public static String[] TYPE_NAMES = { "ȫ��������", "ȫ����С��", "ȫ����׼��", "�볡������", "�볡��С��", "�볡��׼��", "��һ��������", "��һ�ڴ�С��", "�ڶ���������", "�ڶ��ڴ�С��", "������������", "�����ڴ�С��" };
        private readonly TPR2.DAL.guess.BaList dal = new TPR2.DAL.guess.BaList();
        public BaList()
        { }
        #region  ��Ա����

        /// <summary>
        /// ��ȡ��ʷ��¼
        /// </summary>
        /// <param name="p_id">���ID</param>
        /// <param name="ptype">���ͣ�1ȫ�������̣�2ȫ����С�̣�3ȫ����׼�̣�4�볡�����̣�5�볡��С�̣�6�볡��׼�̣�7��һ�������̣�8��һ�ڴ�С�̣�9�ڶ��������̣�10�ڶ��ڴ�С�̣�11�����������̣�12�����ڴ�С�̣�</param>
        /// <returns></returns>
        public IList<TPR2.Model.guess.TBaListNew_History> GetHistory(int p_id, int ptype)
        {
            return dal.GetHistory(p_id, ptype);
        }

        /// <summary>
        /// �õ��Ƿ񵥽ڰ볡����
        /// </summary>
        public int Getp_basketve(int ID)
        {
            return dal.Getp_basketve(ID);
        }
        /// <summary>
        /// ��������ID
        /// </summary>
        public void UpdatexID(int ID, string xID, int Types)
        {
            dal.UpdatexID(ID, xID, Types);
        }

        /// <summary>
        /// ����ȫ�ַ���
        /// </summary>
        public void Updatep_isluck(int ID, int p_isluck)
        {
            dal.Updatep_isluck(ID, p_isluck);
        }

        /// <summary>
        /// ����״̬
        /// </summary>
        public void UpdateOnce(int p_basketve, int p_id, string p_once)
        {
            dal.UpdateOnce(p_basketve, p_id, p_once);
        }
        /// <summary>
        /// ���µ����곡�ȷ�
        /// </summary>
        public int UpdateBoResult(int p_basketve, int p_id, int p_result_one, int p_result_two)
        {
            return dal.UpdateBoResult(p_basketve, p_id, p_result_one, p_result_two);
        }

        /// <summary>
        /// ���µ��ڼ�ʱ�ȷ�
        /// </summary>
        public void UpdateBoResult3(int p_basketve, int p_id, int p_result_temp1, int p_result_temp2)
        {
            dal.UpdateBoResult3(p_basketve, p_id, p_result_temp1, p_result_temp2);
        }
        /// <summary>
        /// ����һ�����ݣ�����ʹ�ã�
        /// </summary>
        public void BasketUpdateOdds(TPR2.Model.guess.BaList model)
        {
            dal.BasketUpdateOdds(model);
        }
        /// <summary>
        /// �ǲ����ߵ�
        /// </summary>
        public bool Existsp_ison(int p_id, int p_basketve)
        {
            return dal.Existsp_ison(p_id, p_basketve);
        }
        /// <summary>
        /// ����Ϊ����ģʽ(����볡)
        /// </summary>
        public void FootOnceType4(int p_id, DateTime dt)
        {
            dal.FootOnceType4(p_id, dt);
        }
        /// <summary>
        /// ���¼�ʱ�ȷ�8���볡
        /// </summary>
        public void UpdateBoResultHalf(int p_id, int p_result_temp1, int p_result_temp2)
        {
            dal.UpdateBoResultHalf(p_id, p_result_temp1, p_result_temp2);
        }
        /// <summary>
        /// �Զ��������±ȷ�
        /// </summary>
        public int UpdateZDResult2(TPR2.Model.guess.BaList model)
        {
            return dal.UpdateZDResult2(model);
        }

        /// <summary>
        /// ����һ������(�����ϰ볡ҳ��ʹ��)
        /// </summary>
        public void UpdateFalf(TPR2.Model.guess.BaList model)
        {
            dal.UpdateFalf(model);

        }
        /// <summary>
        /// ����һ������(�����ϰ볡ҳ��ʹ��)
        /// </summary>
        public void UpdateFalf1(TPR2.Model.guess.BaList model)
        {
            dal.UpdateFalf1(model);

        }

        /// <summary>
        /// ����һ������(�����ϰ볡ҳ��ʹ��)
        /// </summary>
        public void UpdateFalf2(TPR2.Model.guess.BaList model)
        {
            dal.UpdateFalf2(model);

        }

        /// <summary>
        /// ����һ������(�����ϰ볡ҳ��ʹ��)
        /// </summary>
        public void UpdateFalf3(TPR2.Model.guess.BaList model)
        {
            dal.UpdateFalf3(model);

        }

        /// <summary>
        /// �������Ӻ�������
        /// </summary>
        public void Updatep_hp_one(int p_id, int p_hp_one)
        {
            dal.Updatep_hp_one(p_id, p_hp_one);
        }

        /// <summary>
        /// ���¿ͶӺ�������
        /// </summary>
        public void Updatep_hp_two(int p_id, int p_hp_two)
        {
            dal.Updatep_hp_two(p_id, p_hp_two);
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
        /// �Ƿ��ѿ���
        /// </summary>
        public bool ExistsIsOpen(int ID)
        {
            return dal.ExistsIsOpen(ID);
        }

        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool ExistsByp_id(int p_id)
        {
            return dal.ExistsByp_id(p_id);
        }


        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool ExistsByp_id(int p_id, int p_basketve)
        {
            return dal.ExistsByp_id(p_id, p_basketve);

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
        /// �õ�usidÿ�������Ĵ���_�۹���20160815
        /// </summary>
        public int Getzqcount(int usID, int bcid)
        {
            return dal.Getzqcount(usID, bcid);
        }

        /// <summary>
        /// �õ���ѯ�ļ�¼��
        /// </summary>
        public int GetCount(TPR2.Model.guess.BaList model)
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
        public int Add(TPR2.Model.guess.BaList model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// ��������������һ������
        /// </summary>
        public int FootAdd(TPR2.Model.guess.BaList model)
        {
            return dal.FootAdd(model);
        }

        /// <summary>
        /// ����������������ʾ
        /// </summary>
        public void Updatep_del(TPR2.Model.guess.BaList model)
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
        /// ���²���
        /// </summary>
        public void Updatep_score(int ID, string p_score)
        {
            dal.Updatep_score(ID, p_score);
        }
        /// <summary>
        /// ���²���
        /// </summary>
        public void Updatep_score2(int p_id, string p_score)
        {
            dal.Updatep_score2(p_id, p_score);
        }

        /// <summary>
        /// ��������ץȡ�벻ץȡ
        /// </summary>
        public void Updatep_jc(TPR2.Model.guess.BaList model)
        {
            dal.Updatep_jc(model);
        }

        /// <summary>
        /// ���������˹����Զ�����
        /// </summary>
        public void Updatep_dr(TPR2.Model.guess.BaList model)
        {
            dal.Updatep_dr(model);
        }

        /// <summary>
        /// �������½��״̬
        /// </summary>
        public void Updatep_zd(TPR2.Model.guess.BaList model)
        {
            dal.Updatep_zd(model);
        }

        /// <summary>
        /// �������±ȷ�
        /// </summary>
        public void UpdateResult(TPR2.Model.guess.BaList model)
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
        public void UpdateOnce(TPR2.Model.guess.BaList model)
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
        public void Update(TPR2.Model.guess.BaList model)
        {
            dal.Update(model);
        }

        /// <summary>
        /// ������������
        /// </summary>
        public void Update2(TPR2.Model.guess.BaList model)
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
        /// �����ֹ�����ʱ��
        /// </summary>
        public void Updatep_opentime(int ID)
        {
            dal.Updatep_opentime(ID);
        }

        /// <summary>
        /// ���¹��������ˮλ�仯ʱ��
        /// </summary>
        public void Updatep_updatetime(int p_id)
        {
            dal.Updatep_updatetime(p_id);
        }

        /// <summary>
        /// ����Ϊ����ģʽ
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
        /// �����Ƿ����
        /// </summary>
        public void Updatep_isluck(int p_id, int state, int Types)
        {
            dal.Updatep_isluck(p_id, state, Types, 0, DateTime.Now);
        }
        /// <summary>
        /// �����Ƿ����
        /// </summary>
        public void Updatep_isluck(int p_id, int state, int Types, int p_basketve, DateTime p_temptime)
        {
            dal.Updatep_isluck(p_id, state, Types, p_basketve, p_temptime);
        }

        /// <summary>
        /// �����Ƿ����
        /// </summary>
        public void Updatep_isluck2(int id, int state, int Types)
        {
            dal.Updatep_isluck2(id, state, Types);
        }

        /// <summary>
        /// ���¼�ʱ�ȷ֣�����ʹ�ã�
        /// </summary>
        public void FootOnceUpdate(TPR2.Model.guess.BaList model)
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
        /// �õ�����ID
        /// </summary>
        public int GetID(int p_id)
        {
            return dal.GetID(p_id);
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
        public TPR2.Model.guess.BaList GetBasketOpen(DateTime p_TPRtime, string p_one, string p_two)
        {
            return dal.GetBasketOpen(p_TPRtime, p_one, p_two);
        }

        /// <summary>
        /// �õ����򿪽������ʵ��
        /// </summary>
        public TPR2.Model.guess.BaList GetFootOpen(string p_title, string p_one, string p_two)
        {
            return dal.GetFootOpen(p_title, p_one, p_two);
        }

        /// <summary>
        /// �õ��Ƿ�������
        /// </summary>
        public int Getison(int ID)
        {
            return dal.Getison(ID);
        }

        /// <summary>
        /// �õ��Ƿ�������
        /// </summary>
        public int Getisonht(int ID)
        {
            return dal.Getisonht(ID);
        }

        /// <summary>
        /// �õ��������ʱ�伯��
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
        public TPR2.Model.guess.BaList Getp_temptime_p_id(int ID)
        {
            return dal.Getp_temptime_p_id(ID);
        }

        /// <summary>
        /// �õ���ʱ�ȷ�
        /// </summary>
        public TPR2.Model.guess.BaList GetTemp(int ID)
        {
            return dal.GetTemp(ID, 0);
        }
        /// <summary>
        /// �õ���ʱ�ȷ�
        /// </summary>
        public TPR2.Model.guess.BaList GetTemp(int ID, int p_basketve)
        {
            return dal.GetTemp(ID, p_basketve);
        }

        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public TPR2.Model.guess.BaList GetModel(int ID)
        {
            return dal.GetModel(ID, 0);
        }

        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public TPR2.Model.guess.BaList GetModelByp_id(int p_id)
        {
            return dal.GetModel(p_id, 1);
        }

        /// <summary>
        /// ��ȡ�������̱�ʶ 1Ϊϵͳ���� 2Ϊ�˹�����
        /// </summary>
        /// <param name="p_id"></param>
        /// <param name="p_basketve"></param>
        /// <returns></returns>
        public TPR2.Model.guess.BaList Getluck(int p_id, int p_basketve)
        {
            return dal.Getluck(p_id, p_basketve);
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
        public IList<TPR2.Model.guess.BaList> GetBaLists(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
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
        public IList<TPR2.Model.guess.BaList> GetBaListLX(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetBaListLX(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }

        /// <summary>
        /// ȡ��δ�������¼�¼
        /// </summary>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <returns>IList</returns>
        public IList<TPR2.Model.guess.BaList> GetBaListBF(string strWhere, out int p_recordCount)
        {
            return dal.GetBaListBF(strWhere, out p_recordCount);
        }

        #endregion  ��Ա����
    }
}

