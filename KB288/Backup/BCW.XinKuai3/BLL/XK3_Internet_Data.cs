using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.XinKuai3.Model;
namespace BCW.XinKuai3.BLL
{
    /// <summary>
    /// ҵ���߼���XK3_Internet_Data ��ժҪ˵����
    /// </summary>
    public class XK3_Internet_Data
    {
        private readonly BCW.XinKuai3.DAL.XK3_Internet_Data dal = new BCW.XinKuai3.DAL.XK3_Internet_Data();
        public XK3_Internet_Data()
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
        public int Add(BCW.XinKuai3.Model.XK3_Internet_Data model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public void Update(BCW.XinKuai3.Model.XK3_Internet_Data model)
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
        public BCW.XinKuai3.Model.XK3_Internet_Data GetXK3_Internet_Data(int ID)
        {

            return dal.GetXK3_Internet_Data(ID);
        }

        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public BCW.XinKuai3.Model.XK3_Internet_Data GetXK3_Internet_Data2(int ID)
        {
            return dal.GetXK3_Internet_Data2(ID);
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
        /// �����ֶ�ȡ�����б�2
        /// </summary>
        public DataSet GetList2(string strField, string strWhere)
        {
            return dal.GetList2(strField, strWhere);
        }
        /// <summary>
        /// me_�õ�һ�������������ʵ��
        /// </summary>
        public BCW.XinKuai3.Model.XK3_Internet_Data Getxk3kainum(string _where)
        {
            return dal.Getxk3kainum(_where);
        }        /// <summary>
                 /// me_��̨�˹����������ݿ��������Ӧ�ĸ�����������22
                 /// </summary>
        public BCW.XinKuai3.Model.XK3_Internet_Data Getxk3all_num2(string _where)
        {
            return dal.Getxk3all_num2(_where);
        }
        /// <summary>
        /// me_�õ����һ�ڶ���ʵ��
        /// </summary>
        public BCW.XinKuai3.Model.XK3_Internet_Data Getxk3listLast(string _where)
        {
            return dal.Getxk3listLast(_where);
        }
        /// <summary>
        /// me_����һ�����Ͽ�������
        /// </summary>
        public int Add_num(BCW.XinKuai3.Model.XK3_Internet_Data model)
        {
            return dal.Add_num(model);
        }
        /// <summary>
        /// me_����һ�����Ͽ�������
        /// </summary>
        public void update_num2(BCW.XinKuai3.Model.XK3_Internet_Data model)
        {
            dal.update_num2(model);
        }
        /// <summary>
        /// me_�Ƿ���ڸÿ�����¼
        /// </summary>
        public bool Exists_num(string Lottery_issue)
        {
            return dal.Exists_num(Lottery_issue);
        }
        /// <summary>
        /// me_�񽱳���--��ȡ��������������н�����
        /// </summary>
        public void Update_num(BCW.XinKuai3.Model.XK3_Internet_Data model)
        {
            dal.Update_num(model);
        }
        /// <summary>
        /// me_��̨---��ȡ��������������н�����
        /// </summary>
        public void Update_num2(BCW.XinKuai3.Model.XK3_Internet_Data model)
        {
            dal.Update_num2(model);
        }
        /// <summary>
        /// me_��̨---��ȡ��������������н�����
        /// </summary>
        public void Update_num3(BCW.XinKuai3.Model.XK3_Internet_Data model)
        {
            dal.Update_num3(model);
        }
        /// <summary>
        /// me_�����󣬻�ȡ���һ��
        /// </summary>
        public BCW.XinKuai3.Model.XK3_Internet_Data Getxk3listLast2()
        {
            return dal.Getxk3listLast2();
        }
        /// <summary>
        /// me_�䶯����
        /// </summary>
        public BCW.XinKuai3.Model.XK3_Internet_Data Getxk3listLast3()
        {
            return dal.Getxk3listLast3();
        }
        /// <summary>
        /// me_�����ֵ���ִ���������
        /// </summary>
        public IList<BCW.XinKuai3.Model.XK3_Internet_Data> Getxk3all(string _where)
        {
            return dal.Getxk3all(_where);
        }
        /// <summary>
        /// me_�����ͬ�ų��ֵĴ���
        /// </summary>
        public BCW.XinKuai3.Model.XK3_Internet_Data Getxk3Two_Same_All(string _where)
        {
            return dal.Getxk3Two_Same_All(_where);
        }
        /// <summary>
        /// me_�������ͬ�ų��ֵĴ���
        /// </summary>
        public BCW.XinKuai3.Model.XK3_Internet_Data Getxk3Two_dissame(string _where)
        {
            return dal.Getxk3Two_dissame(_where);
        }
        /// <summary>
        /// me_������ͬ�ų��ֵĴ���
        /// </summary>
        public BCW.XinKuai3.Model.XK3_Internet_Data Getxk3Three_Same_All(string _where)
        {
            return dal.Getxk3Three_Same_All(_where);
        }
        /// <summary>
        /// me_��������ͬ�ų��ֵĴ���
        /// </summary>
        public BCW.XinKuai3.Model.XK3_Internet_Data Getxk3where_Three_Same_Not(string _where)
        {
            return dal.Getxk3where_Three_Same_Not(_where);
        }
        /// <summary>
        /// me_���������ų��ֵĴ���
        /// </summary>
        public BCW.XinKuai3.Model.XK3_Internet_Data Getxk3where_Three_Continue_All(string _where)
        {
            return dal.Getxk3where_Three_Continue_All(_where);
        }
        /// <summary>
        /// me_�������ֵĴ���
        /// </summary>
        public BCW.XinKuai3.Model.XK3_Internet_Data Getxk3da(string _where)
        {
            return dal.Getxk3da(_where);
        }
        /// <summary>
        /// me_����С���ֵĴ���
        /// </summary>
        public BCW.XinKuai3.Model.XK3_Internet_Data Getxk3xiao(string _where)
        {
            return dal.Getxk3xiao(_where);
        }
        /// <summary>
        /// me_����˫���ֵĴ���
        /// </summary>
        public BCW.XinKuai3.Model.XK3_Internet_Data Getxk3shuang(string _where)
        {
            return dal.Getxk3shuang(_where);
        }
        /// <summary>
        /// me_���㵥���ֵĴ���
        /// </summary>
        public BCW.XinKuai3.Model.XK3_Internet_Data Getxk3dan(string _where)
        {
            return dal.Getxk3dan(_where);
        }
        /// <summary>
        /// me_����ͨ�Գ��ֵĴ���
        /// </summary>
        public BCW.XinKuai3.Model.XK3_Internet_Data Getxk3tongchi(string _where)
        {
            return dal.Getxk3tongchi(_where);
        }

        /// <summary>
        /// me_��ȡ���10�ڿ������
        /// </summary>
        public IList<BCW.XinKuai3.Model.XK3_Internet_Data> Getxk3listTop(string _where)
        {
            return dal.Getxk3listTop(_where);
        }
        //============================================




        /// <summary>
        /// ȡ��ÿҳ��¼
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">��ҳ��С</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <returns>IList XK3_Internet_Data</returns>
        public IList<BCW.XinKuai3.Model.XK3_Internet_Data> GetXK3_Internet_Datas(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetXK3_Internet_Datas(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }

        #endregion  ��Ա����
    }
}

