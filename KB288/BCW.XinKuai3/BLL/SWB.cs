using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.XinKuai3.Model;
namespace BCW.XinKuai3.BLL
{
	/// <summary>
	/// 业务逻辑类SWB 的摘要说明。
	/// </summary>
	public class SWB
	{
		private readonly BCW.XinKuai3.DAL.SWB dal=new BCW.XinKuai3.DAL.SWB();
		public SWB()
		{}
		#region  成员方法

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public void Add(BCW.XinKuai3.Model.SWB model)
		{
			dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(BCW.XinKuai3.Model.SWB model)
		{
			dal.Update(model);
		}

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int UserID)
        {

            dal.Delete(UserID);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.XinKuai3.Model.SWB GetSWB(int UserID)
        {

            return dal.GetSWB(UserID);
        }

		/// <summary>
		/// 根据字段取数据列表
		/// </summary>
		public DataSet GetList(string strField, string strWhere)
		{
			return dal.GetList(strField, strWhere);
		}


        //=========================================
        /// <summary>
        /// me_得到用户币
        /// </summary>
        public long GetGold(int UserID)
        {
            return dal.GetGold(UserID);
        }
        /// <summary>
        /// me_是否存在该记录
        /// </summary>
        public bool Exists(int UserID)
        {
            return dal.Exists(UserID);
        }
        /// <summary>
        /// me_更新用户虚拟币/更新消费记录
        /// </summary>
        /// <param name="ID">用户ID</param>
        /// <param name="iGold">操作币</param>
        public void UpdateiGold(int UserID, long iGold)
        {
            dal.UpdateiGold(UserID, iGold);
        }
        /// <summary>
        /// me_更新用户试玩领取的时间
        /// </summary>
        /// <param name="ID">用户ID</param>
        /// <param name="cishu">次数</param>
        public void Updatecishu(int UserID)
        {
            dal.Updatecishu(UserID);
        }
        /// <summary>
        /// me_增加一条数据
        /// </summary>
        public void Add_num(BCW.XinKuai3.Model.SWB model)
        {
            dal.Add_num(model);
        }

        //=========================================


		/// <summary>
		/// 取得每页记录
		/// </summary>
		/// <param name="p_pageIndex">当前页</param>
		/// <param name="p_pageSize">分页大小</param>
		/// <param name="p_recordCount">返回总记录数</param>
		/// <param name="strWhere">查询条件</param>
		/// <returns>IList SWB</returns>
		public IList<BCW.XinKuai3.Model.SWB> GetSWBs(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetSWBs(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

		#endregion  成员方法
	}
}

