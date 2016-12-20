using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
	/// <summary>
	/// 业务逻辑类Diary 的摘要说明。
	/// </summary>
	public class Diary
	{
		private readonly BCW.DAL.Diary dal=new BCW.DAL.Diary();
		public Diary()
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
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID, int UsID)
        {
            return dal.Exists(ID, UsID);
        }

        /// <summary>
        /// 计算某用户日记数量
        /// </summary>
        public int GetCount(int UsID)
        {
            return dal.GetCount(UsID);
        }
                     
        /// <summary>
        /// 计算某用户今天日记数量
        /// </summary>
        public int GetTodayCount(int UsID)
        {
            return dal.GetTodayCount(UsID);
        }
  
        /// <summary>
        /// 计算某用户某分组日记数量
        /// </summary>
        public int GetCount(int UsID, int NodeId)
        {
            return dal.GetCount(UsID, NodeId);
        }

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(BCW.Model.Diary model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(BCW.Model.Diary model)
		{
			dal.Update(model);
		}

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update2(BCW.Model.Diary model)
        {
            dal.Update2(model);
        }

        /// <summary>
        /// 更新日记内容
        /// </summary>
        public void UpdateContent(int ID, string Content)
        {
            dal.UpdateContent(ID, Content);
        }

        /// <summary>
        /// 更新回复数
        /// </summary>
        public void UpdateReplyNum(int ID, int ReplyNum)
        {
            dal.UpdateReplyNum(ID, ReplyNum);
        }

        /// <summary>
        /// 更新阅读数
        /// </summary>
        public void UpdateReadNum(int ID, int ReadNum)
        {
            dal.UpdateReadNum(ID, ReadNum);
        }

        /// <summary>
        /// 置顶/去顶
        /// </summary>
        public void UpdateIsTop(int ID, int IsTop)
        {
            dal.UpdateIsTop(ID, IsTop);
        }

        /// <summary>
        /// 更新某分组日记成为默认分类
        /// </summary>
        public void UpdateNodeId(int UsID, int NodeId)
        {
            dal.UpdateNodeId(UsID, NodeId);
        }

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int ID)
		{
			
			dal.Delete(ID);
		}
                
        /// <summary>
        /// 删除一组数据
        /// </summary>
        public void Delete(int UsID, int NodeId)
        {
            dal.Delete(UsID, NodeId);
        }

        /// <summary>
        /// 删除一组数据
        /// </summary>
        public void DeleteStr(string strWhere)
        {

            dal.DeleteStr(strWhere);
        }

        /// <summary>
        /// 得到Title
        /// </summary>
        public string GetTitle(int ID)
        {
            return dal.GetTitle(ID);
        }
                
        /// <summary>
        /// 得到一个用户ID
        /// </summary>
        public int GetUsID(int ID)
        {
            return dal.GetUsID(ID);
        }
                
        /// <summary>
        /// 得到一个NodeId
        /// </summary>
        public int GetNodeId(int ID)
        {
            return dal.GetNodeId(ID);
        }

        /// <summary>
        /// 得到IsTop
        /// </summary>
        public int GetIsTop(int ID)
        {
            return dal.GetIsTop(ID);
        }

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public BCW.Model.Diary GetDiary(int ID)
		{
			
			return dal.GetDiary(ID);
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
		/// <param name="strWhere">查询条件</param>
        /// <param name="strOrder">排序条件</param>
		/// <returns>IList Diary</returns>
		public IList<BCW.Model.Diary> GetDiarys(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
		{
			return dal.GetDiarys(p_pageIndex, p_pageSize, strWhere, strOrder, out p_recordCount);
		}
               
        /// <summary>
        /// 显示N条新日记
        /// </summary>
        /// <param name="p_Size">显示条数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList Diary</returns>
        public IList<BCW.Model.Diary> GetDiarysTop(int p_Size, string strWhere)
        {
            return dal.GetDiarysTop(p_Size, strWhere);
        }
		#endregion  成员方法
	}
}

