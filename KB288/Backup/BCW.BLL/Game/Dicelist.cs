using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model.Game;
namespace BCW.BLL.Game
{
	/// <summary>
	/// 业务逻辑类Dicelist 的摘要说明。
	/// </summary>
	public class Dicelist
	{
		private readonly BCW.DAL.Game.Dicelist dal=new BCW.DAL.Game.Dicelist();
		public Dicelist()
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
		/// 增加一条数据
		/// </summary>
		public int  Add(BCW.Model.Game.Dicelist model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(BCW.Model.Game.Dicelist model)
		{
			dal.Update(model);
		}
                        
        /// <summary>
        /// 更新本期记录
        /// </summary>
        public void Update2(BCW.Model.Game.Dicelist model)
        {
            dal.Update2(model);
        }

        /// <summary>
        /// 更新总押注金额
        /// </summary>
        public void UpdatePool(int ID, long Pool)
        {
            dal.UpdatePool(ID, Pool);
        }
               
        /// <summary>
        /// 更新总押注金额2
        /// </summary>
        public void UpdateWinPool(int ID, long WinPool)
        {
            dal.UpdateWinPool(ID, WinPool);
        }

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int ID)
		{
			
			dal.Delete(ID);
		}
                
        /// <summary>
        /// 得到上一期对象实体
        /// </summary>
        public BCW.Model.Game.Dicelist GetDicelistBf(int ID)
        {
            return dal.GetDicelistBf(ID);
        }

        /// <summary>
        /// 得到一个本期实体
        /// </summary>
        public BCW.Model.Game.Dicelist GetDicelist()
        {
            return dal.GetDicelist();
        }
                
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public int GetWinNum(int ID)
        {
            return dal.GetWinNum(ID);
        }

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public BCW.Model.Game.Dicelist GetDicelist(int ID)
		{
			
			return dal.GetDicelist(ID);
		}

		/// <summary>
		/// 根据字段取数据列表
		/// </summary>
		public DataSet GetList(string strField, string strWhere)
		{
			return dal.GetList(strField, strWhere);
		}

        /// <summary>
        /// 取得固定列表记录
        /// </summary>
        /// <param name="SizeNum">列表记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList Dicelist</returns>
        public IList<BCW.Model.Game.Dicelist> GetDicelists(int SizeNum, string strWhere)
        {
            return dal.GetDicelists(SizeNum, strWhere);
        }

		/// <summary>
		/// 取得每页记录
		/// </summary>
		/// <param name="p_pageIndex">当前页</param>
		/// <param name="p_pageSize">分页大小</param>
		/// <param name="p_recordCount">返回总记录数</param>
		/// <param name="strWhere">查询条件</param>
		/// <returns>IList Dicelist</returns>
		public IList<BCW.Model.Game.Dicelist> GetDicelists(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetDicelists(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

		#endregion  成员方法
	}
}

