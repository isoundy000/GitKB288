using System;
using System.Data;
using System.Collections.Generic;
using BCW.Model;
using BCW.Common;
namespace BCW.BLL
{
	/// <summary>
	/// ҵ���߼���Topics ��ժҪ˵����
	/// </summary>
	public class Topics
	{
		private readonly BCW.DAL.Topics dal=new BCW.DAL.Topics();
		public Topics()
		{}
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
        /// �Ƿ���ڸýڵ��¼
        /// </summary>
        public bool ExistsTypes(int ID)
        {
            return dal.ExistsTypes(ID);
        }
               
        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool ExistsIdLeibie(int ID, int Leibie)
        {
            return dal.ExistsIdLeibie(ID, Leibie);
        }

        /// <summary>
        /// �˵����Ƿ����ҳ��˵�
        /// </summary>
        public bool ExistsTypesIn(int ID)
        {
            return dal.ExistsTypesIn(ID);
        }

        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool ExistsIdTypes(int ID, int Types)
        {
            return dal.ExistsIdTypes(ID, Types);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public void Add(BCW.Model.Topics model)
        {
            dal.Add(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public void Update(BCW.Model.Topics model)
        {
            dal.Update(model);
        }

        /// <summary>
        /// ���½ڵ�
        /// </summary>
        public void UpdateNodeId(int ID, int NodeId)
        {
            dal.UpdateNodeId(ID, NodeId);
        }

        /// <summary>
        /// ��������
        /// </summary>
        public void UpdatePaixu(int ID, int Paixu)
        {
            dal.UpdatePaixu(ID, Paixu);
        }

        /// <summary>
        /// ���¹���ID
        /// </summary>
        public void UpdatePayId(int ID, string PayId)
        {
            dal.UpdatePayId(ID, PayId);
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
        public void DeleteNodeId(int ID)
        {
            dal.DeleteNodeId(ID);
        }

        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public BCW.Model.Topics GetTopics(int ID)
        {
            return dal.GetTopics(ID);
        }

        /// <summary>
        /// �õ��ڵ�����
        /// </summary>
        public string GetTitle(int ID)
        {
            return dal.GetTitle(ID);
        }

        /// <summary>
        /// �õ��ڵ�NodeId
        /// </summary>
        public int GetNodeId(int ID)
        {
            return dal.GetNodeId(ID);
        }

        /// <summary>
        /// �õ�����Types
        /// </summary>
        public int GetTypes(int ID)
        {
            return dal.GetTypes(ID);
        }

        /// <summary>
        /// �õ�������Leibie
        /// </summary>
        public int GetLeibie(int ID)
        {
            return dal.GetLeibie(ID);
        }

        ///// <summary>
        ///// �õ�һ������ʵ�壬�ӻ����С�
        ///// </summary>
        //public BCW.Model.Topics GetTopicsByCache(int ID)
        //{
        //    string CacheKey = CacheName.App_TopicsModel(ID);
        //    object objModel = DataCache.GetCache(CacheKey);
        //    if (objModel == null)
        //    {
        //        try
        //        {
        //            objModel = dal.GetTopics(ID);
        //            if (objModel != null)
        //            {
        //                int ModelCache = ConfigHelper.GetConfigInt("ModelCache");
        //                DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
        //            }
        //        }
        //        catch { }
        //    }
        //    return (BCW.Model.Topics)objModel;
        //}

        /// <summary>
        /// ��������б�
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            return dal.GetList(strWhere);
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
        /// <returns>IList Topics</returns>
        public IList<BCW.Model.Topics> GetTopicss(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetTopicss(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }
		#endregion  ��Ա����
	}
}

