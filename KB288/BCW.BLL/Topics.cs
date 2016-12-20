using System;
using System.Data;
using System.Collections.Generic;
using BCW.Model;
using BCW.Common;
namespace BCW.BLL
{
	/// <summary>
	/// 业务逻辑类Topics 的摘要说明。
	/// </summary>
	public class Topics
	{
		private readonly BCW.DAL.Topics dal=new BCW.DAL.Topics();
		public Topics()
		{}
		#region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return dal.GetMaxId();
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            return dal.Exists(ID);
        }

        /// <summary>
        /// 是否存在该节点记录
        /// </summary>
        public bool ExistsTypes(int ID)
        {
            return dal.ExistsTypes(ID);
        }
               
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool ExistsIdLeibie(int ID, int Leibie)
        {
            return dal.ExistsIdLeibie(ID, Leibie);
        }

        /// <summary>
        /// 菜单里是否存在页面菜单
        /// </summary>
        public bool ExistsTypesIn(int ID)
        {
            return dal.ExistsTypesIn(ID);
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool ExistsIdTypes(int ID, int Types)
        {
            return dal.ExistsIdTypes(ID, Types);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public void Add(BCW.Model.Topics model)
        {
            dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(BCW.Model.Topics model)
        {
            dal.Update(model);
        }

        /// <summary>
        /// 更新节点
        /// </summary>
        public void UpdateNodeId(int ID, int NodeId)
        {
            dal.UpdateNodeId(ID, NodeId);
        }

        /// <summary>
        /// 更新排序
        /// </summary>
        public void UpdatePaixu(int ID, int Paixu)
        {
            dal.UpdatePaixu(ID, Paixu);
        }

        /// <summary>
        /// 更新购买ID
        /// </summary>
        public void UpdatePayId(int ID, string PayId)
        {
            dal.UpdatePayId(ID, PayId);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int ID)
        {
            dal.Delete(ID);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void DeleteNodeId(int ID)
        {
            dal.DeleteNodeId(ID);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.Model.Topics GetTopics(int ID)
        {
            return dal.GetTopics(ID);
        }

        /// <summary>
        /// 得到节点名称
        /// </summary>
        public string GetTitle(int ID)
        {
            return dal.GetTitle(ID);
        }

        /// <summary>
        /// 得到节点NodeId
        /// </summary>
        public int GetNodeId(int ID)
        {
            return dal.GetNodeId(ID);
        }

        /// <summary>
        /// 得到类型Types
        /// </summary>
        public int GetTypes(int ID)
        {
            return dal.GetTypes(ID);
        }

        /// <summary>
        /// 得到大类型Leibie
        /// </summary>
        public int GetLeibie(int ID)
        {
            return dal.GetLeibie(ID);
        }

        ///// <summary>
        ///// 得到一个对象实体，从缓存中。
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
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            return dal.GetList(strWhere);
        }

        
        /// <summary>
        /// 根据字段取数据列表
        /// </summary>
        public DataSet GetList(string strField, string strWhere)
        {
            return dal.GetList(strField, strWhere);
        }

        /// <summary>
        /// 取得每页记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <returns>IList Topics</returns>
        public IList<BCW.Model.Topics> GetTopicss(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetTopicss(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }
		#endregion  成员方法
	}
}

