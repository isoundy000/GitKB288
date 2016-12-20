using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model.Game;
namespace BCW.BLL.Game
{
	/// <summary>
	/// 业务逻辑类flowmyzz 的摘要说明。
	/// </summary>
	public class flowmyzz
	{
		private readonly BCW.DAL.Game.flowmyzz dal=new BCW.DAL.Game.flowmyzz();
		public flowmyzz()
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
        public bool Exists(int UsID, int Types, int zid)
        {
            return dal.Exists(UsID, Types, zid);
        }

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(BCW.Model.Game.flowmyzz model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(BCW.Model.Game.flowmyzz model)
		{
			dal.Update(model);
		}
        
        /// <summary>
        /// 更新种子或花
        /// </summary>
        public void Updateznum(int UsID, int zid, int Types, int znum)
        {

            dal.Updateznum(UsID, zid, Types, znum);
        }

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int ID)
		{
			
			dal.Delete(ID);
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
        public BCW.Model.Game.flowmyzz Getflowmyzz(int UsID, int Types, int zid)
        {

            return dal.Getflowmyzz(UsID, Types, zid);
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
		/// <returns>IList flowmyzz</returns>
		public IList<BCW.Model.Game.flowmyzz> Getflowmyzzs(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.Getflowmyzzs(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

		#endregion  成员方法
	}
}

