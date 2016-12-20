using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.bydr.Model;
namespace BCW.bydr.BLL
{
	/// <summary>
	/// 业务逻辑类Cmg_notes 的摘要说明。
	/// </summary>
	public class Cmg_notes
	{
		private readonly BCW.bydr.DAL.Cmg_notes dal=new BCW.bydr.DAL.Cmg_notes();
		public Cmg_notes()
		{}
		#region  成员方法

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
			return dal.GetMaxId();
		}
        public int GetMaxID(int usID)
        {
            return dal.GetMaxID(usID);
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
        public bool Exists1(int meid)
        {
            return dal.Exists1(meid);
        }
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists2(int n, int usid, string coID)
        {
            return dal.Exists2(n, usid, coID);
        }

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(BCW.bydr.Model.Cmg_notes model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(BCW.bydr.Model.Cmg_notes model)
		{
			dal.Update(model);
		}
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Updatestype(int ID ,int stype)
        {
            dal.Updatestype(ID,stype);
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void updateAllGold(int ID,long AllGold)
        {
            dal.updateAllGold(ID, AllGold);
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void updateAllGold1(int ID, long AllGold)
        {
            dal.updateAllGold1(ID, AllGold);
        }
         /// <summary>
        /// 通过id更新体力
        /// </summary>
        public void UpdateVit(int ID, int Vit)
        {
            dal.UpdateVit(ID, Vit);
        }
         /// <summary>
        /// 更新签到时间
        /// </summary>
        public void UpdateSigntime(int ID, DateTime Signtime)
        {
            dal.UpdateSigntime(ID,Signtime);
        }
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int ID)
		{
			
			dal.Delete(ID);
		}
        /// <summary>
        /// 删除一个时间段的数据
        /// </summary>
        public void Delete1(string stime,string otime)
        {

            dal.Delete1(stime,otime);
        }
		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public BCW.bydr.Model.Cmg_notes GetCmg_notes(int ID)
		{
			
			return dal.GetCmg_notes(ID);
		}
        /// <summary>
        /// 得到最后usID
        /// </summary>
        public BCW.bydr.Model.Cmg_notes GetusID(int usID)
        {
            return dal.GetusID(usID);
        }
        /// <summary>
        /// 通过字段得到id
        /// </summary>
        public int GetID1(int usID, int stype)
        {
            return dal.GetID1(usID, stype);
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
		/// <returns>IList Cmg_notes</returns>
		public IList<BCW.bydr.Model.Cmg_notes> GetCmg_notess(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetCmg_notess(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

		#endregion  成员方法
	}
}

