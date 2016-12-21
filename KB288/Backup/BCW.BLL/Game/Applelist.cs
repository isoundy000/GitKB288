using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model.Game;
namespace BCW.BLL.Game
{
	/// <summary>
	/// 业务逻辑类Applelist 的摘要说明。
	/// </summary>
	public class Applelist
	{
		private readonly BCW.DAL.Game.Applelist dal=new BCW.DAL.Game.Applelist();
		public Applelist()
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
        public bool Exists()
        {
            return dal.Exists();
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
		public bool Exists(int State,int ID)
		{
			return dal.Exists(State,ID);
		}

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(BCW.Model.Game.Applelist model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(BCW.Model.Game.Applelist model)
		{
			dal.Update(model);
		}
                       
        /// <summary>
        /// 更新本期记录
        /// </summary>
        public void Update2(BCW.Model.Game.Applelist model)
        {
            dal.Update2(model);
        }

        /// <summary>
        /// 更新总下注额
        /// </summary>
        public void Update(int ID, long PayCent, int Types, int Num)
        {
            dal.Update(ID, PayCent, Types, Num);
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
		public BCW.Model.Game.Applelist GetApplelist(int ID)
		{
			
			return dal.GetApplelist(ID);
		}

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.Model.Game.Applelist GetApplelistBQ(int State)
        {

            return dal.GetApplelistBQ(State);
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
		/// <returns>IList Applelist</returns>
		public IList<BCW.Model.Game.Applelist> GetApplelists(int p_pageIndex, int p_pageSize, string strWhere, int Num, out int p_recordCount)
		{
			return dal.GetApplelists(p_pageIndex, p_pageSize, strWhere, Num, out p_recordCount);
		}

		#endregion  成员方法
	}
}

