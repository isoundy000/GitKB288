using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model.Game;
namespace BCW.BLL.Game
{
	/// <summary>
	/// 业务逻辑类flowuser 的摘要说明。
	/// </summary>
	public class flowuser
	{
		private readonly BCW.DAL.Game.flowuser dal=new BCW.DAL.Game.flowuser();
		public flowuser()
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
        /// 得到排名
        /// </summary>
        public int GetTop(int UsID, string Field)
        {
            return dal.GetTop(UsID, Field);
        }

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(BCW.Model.Game.flowuser model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(BCW.Model.Game.flowuser model)
		{
			dal.Update(model);
		}
              
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void UpdateScore(int UsID, int Types, int Score)
        {
            dal.UpdateScore(UsID, Types, Score);
        }
               
        /// <summary>
        /// 更新花盆数量
        /// </summary>
        public void UpdateiFlows(int UsID, int iFlows)
        {
            dal.UpdateiFlows(UsID, iFlows);
        }
              
        /// <summary>
        /// 更新今天被玩次数
        /// </summary>
        public void UpdateiBw(int UsID, int iBw)
        {
            dal.UpdateiBw(UsID, iBw);
        }


		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int ID)
		{
			
			dal.Delete(ID);
		}
                      
        /// <summary>
        /// 得到花盆数量
        /// </summary>
        public int GetiFlows(int UsID)
        {

            return dal.GetiFlows(UsID);
        } 

        /// <summary>
        /// 得到技能积分
        /// </summary>
        public int GetScore(int UsID)
        {

            return dal.GetScore(UsID);
        }

        /// <summary>
        /// 得到风采积分
        /// </summary>
        public int GetScore2(int UsID)
        {

            return dal.GetScore2(UsID);
        }

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
        public BCW.Model.Game.flowuser Getflowuser(int UsID)
		{

            return dal.Getflowuser(UsID);
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
		/// <returns>IList flowuser</returns>
		public IList<BCW.Model.Game.flowuser> GetflowusersTop(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetflowusersTop(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

		#endregion  成员方法
	}
}

