using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.farm.Model;
namespace BCW.farm.BLL
{
    /// <summary>
    /// ҵ���߼���NC_messagelog ��ժҪ˵����
    /// </summary>
    public class NC_messagelog
    {
        private readonly BCW.farm.DAL.NC_messagelog dal = new BCW.farm.DAL.NC_messagelog();
        public NC_messagelog()
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
        public int Add(BCW.farm.Model.NC_messagelog model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public void Update(BCW.farm.Model.NC_messagelog model)
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
        public BCW.farm.Model.NC_messagelog GetNC_messagelog(int ID)
        {

            return dal.GetNC_messagelog(ID);
        }

        /// <summary>
        /// �����ֶ�ȡ�����б�
        /// </summary>
        public DataSet GetList(string strField, string strWhere)
        {
            return dal.GetList(strField, strWhere);
        }

        //================================
        /// <summary>
        /// me_������Ϣ
        /// </summary>
        public void addmessage(int ID, string UsName, string AcText, int BbTag)
        {
            //���Ӽ�¼
            BCW.farm.Model.NC_messagelog model = new BCW.farm.Model.NC_messagelog();
            model.type = BbTag;
            model.UsId = ID;
            model.UsName = UsName;
            model.AcText = AcText;
            model.AddTime = DateTime.Now;
            new BCW.farm.BLL.NC_messagelog().Add(model);
        }
        //================================


        /// <summary>
        /// ȡ��ÿҳ��¼
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">��ҳ��С</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <returns>IList NC_messagelog</returns>
        public IList<BCW.farm.Model.NC_messagelog> GetNC_messagelogs(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetNC_messagelogs(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }

        #endregion  ��Ա����
    }
}

