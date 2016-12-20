using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.bydr.Model;
namespace BCW.bydr.BLL
{
	/// <summary>
	/// 业务逻辑类CmgToplist 的摘要说明。
	/// </summary>
	public class CmgToplist
	{
		private readonly BCW.bydr.DAL.CmgToplist dal=new BCW.bydr.DAL.CmgToplist();
		public CmgToplist()
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
        public bool ExistsusID(int usID)
        {
            return dal.ExistsusID(usID);
        }
        /// <summary>
        /// 是否存在vip时间
        /// </summary>
        public bool Existsusvip(int usID,string stime)
        {
            return dal.Existsusvip(usID,stime);
        }
        /// <summary>
        /// 是否存在该特殊id
        /// </summary>
        public bool ExistsusID1(int usID,int sid)
        {
            return dal.ExistsusID1(usID,sid);
        }
		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(BCW.bydr.Model.CmgToplist model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(BCW.bydr.Model.CmgToplist model)
		{
			dal.Update(model);
		}
        /// <summary>
        /// 更新等级
        /// </summary>
        public void Updatestype(int usID, int stype)
        {
            dal.Updatestype( usID,stype);
        }
        /// <summary>
        /// 更新总收集币
        /// </summary>
        public void UpdateAllcolletGold(int usID, long AllcolletGold)
        {
            dal.UpdateAllcolletGold(usID, AllcolletGold);
        }
        /// <summary>
        /// 更新每日收集币
        /// </summary>
        public void UpdateDcolletGold(int usID, long DcolletGold)
        {
            dal.UpdateDcolletGold(usID, DcolletGold);
        }
        /// <summary>
        /// 更新赞助币
        /// </summary>
        public void UpdateYcolletGold(int usID, long YcolletGold)
        {
            dal.UpdateYcolletGold(usID, YcolletGold);
        }
        /// <summary>
        /// 更新sid
        /// </summary>
        public void Updatesid(int usID, int sid)
        {
            dal.Updatesid(usID, sid);
        }
        /// <summary>
        /// 更新时间
        /// </summary>
        public void Updatetime(int usID, DateTime updatetime)
        {
            dal.Updatetime(usID,updatetime);
        }
        /// <summary>
        /// 更新签到时间
        /// </summary>
        public void UpdateSigntime(int usID, DateTime Signtime)
        {
            dal.UpdateSigntime(usID, Signtime);
        }
        /// <summary>
        /// 更新体力值
        /// </summary>
        public void Updatevit(int usID, int vit)
        {
            dal.Updatevit(usID, vit);
        }
        /// <summary>
        /// 更新体力值
        /// </summary>
        public void Updatevit1(int usID, int vit)
        {
            dal.Updatevit1(usID, vit);
        }
        /// <summary>
        /// 更新sid
        /// </summary>
        public void Updatesid1( int sid)
        {
            dal.Updatesid1( sid);
        }
        /// <summary>
        /// 每日收集币清除
        /// </summary>
        public void UpdateDcolletGold1( long DcolletGold)
        {
            dal.UpdateDcolletGold1( DcolletGold);
        }
        /// <summary>
        /// 更新每月收集币
        /// </summary>
        public void UpdateMcolletGold(int usID, long McolletGold)
        {
            dal.UpdateMcolletGold(usID, McolletGold);
        }
        /// <summary>
        /// 每月收集币清除
        /// </summary>
        public void UpdateMcolletGold1(long McolletGold)
        {
            dal.UpdateMcolletGold1( McolletGold);
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
		public BCW.bydr.Model.CmgToplist GetCmgToplist(int ID)
		{
			
			return dal.GetCmgToplist(ID);
		}
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.bydr.Model.CmgToplist GetCmgToplistusID(int usID)
        {

            return dal.GetCmgToplistusID(usID);
        }
        /// <summary>
        /// 得到总收集币
        /// </summary>
        public BCW.bydr.Model.CmgToplist GetCmgTopAllcolletGold(int usID)
        {

            return dal.GetCmgTopAllcolletGold(usID);
        }
        /// <summary>
        /// 得到体力值
        /// </summary>
        public int Getvit(int usID)
        {

            return dal.Getvit(usID);
        }
        /// <summary>
        /// 得到签到时间
        /// </summary>
        public string GetSigntime(int usID)
        {

            return dal.GetSigntime(usID);
        }
        /// <summary>
        /// 得到每日收集币
        /// </summary>
        public long GetCmgTopDcolletGold(int usID,string time)
        {

            return dal.GetCmgTopDcolletGold(usID,time);
        }
        /// <summary>
        /// 得到每月集币
        /// </summary>
        public long GetCmgTopMcolletGold(int usID)
        {

            return dal.GetCmgTopMcolletGold(usID);
        }
        /// <summary>
        /// 得到id
        /// </summary>
        public int Gettoplistid(int sid)
        {

            return dal.Gettoplistid(sid);
        }
        /// <summary>
        /// 得到收集总币
        /// </summary>
        public long GettoplistAllcolletGoldsum()
        {
            return dal.GettoplistAllcolletGoldsum();
        }
        /// <summary>
        /// 得到赞助总币
        /// </summary>
        public long GettoplistYcolletGoldsum()
        {
            return dal.GettoplistYcolletGoldsum();
        }
        /// <summary>
        /// 得到全部今日收集币
        /// </summary>
        public long GettoplistDcolletGoldsum()
        {
            return dal.GettoplistDcolletGoldsum();
        }
        /// <summary>
        /// 得到全部本月收集币
        /// </summary>
        public long GettoplistMcolletGoldsum()
        {
            return dal.GettoplistMcolletGoldsum();
        }

        /// <summary>
        /// 初始化某数据表
        /// </summary>
        /// <param name="TableName">数据表名称</param>
        public void ClearTable(string TableName)
        {
            dal.ClearTable(TableName);
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
		/// <returns>IList CmgToplist</returns>
		public IList<BCW.bydr.Model.CmgToplist> GetCmgToplists(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetCmgToplists(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}
        /// <summary>
        /// 取得每页记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList CmgToplist</returns>
        public IList<BCW.bydr.Model.CmgToplist> GetCmgToplists1(int p_pageIndex, int p_pageSize, string strWhere,string strOrder, out int p_recordCount)
        {
            return dal.GetCmgToplists1(p_pageIndex, p_pageSize, strWhere,strOrder, out p_recordCount);
        }

		#endregion  成员方法
	}
}

