using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.BQC.Model;
namespace BCW.BQC.BLL
{
	/// <summary>
	/// 业务逻辑类BQCPay 的摘要说明。
	/// </summary>
	public class BQCPay
	{
		private readonly BCW.BQC.DAL.BQCPay dal=new BCW.BQC.DAL.BQCPay();
		public BQCPay()
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
        /// 得到最大ID
        /// </summary>
        public int GetMaxId(int usid)
        {
            return dal.GetMaxId(usid);
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int id)
		{
			return dal.Exists(id);
		}
        /// <summary>
        /// 根据ID得到CID
        /// </summary>
        public int GetCID(int ID)
        {
            return dal.GetCID(ID);
        }
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int CID, int IsPrize)
        {
            return dal.Exists(CID, IsPrize);
        }
        /// <summary>
        /// 存在机器人
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="WinUserID"></param>
        /// <returns></returns>
        public bool ExistsReBot(int id, int usID)
        {
            return dal.ExistsReBot(id, usID);
        }
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists1(int id, int IsPrize)
        {
            return dal.Exists1(id, IsPrize);
        }
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists2(int id, int IsPrize2)
        {
            return dal.Exists2(id, IsPrize2);
        }
        /// <summary>
        /// me_查询机器人购买次数
        /// </summary>
        public int GetBQCRobotCount(string strWhere)
        {
            return dal.GetBQCRobotCount(strWhere);
        }
        /// <summary>
        /// 获得每期下注总额
        /// </summary>
        /// <param name="CID"></param>
        /// <returns></returns>
        public long PayCents(int CID)
        {
            return dal.PayCents(CID);
        }

        /// <summary>
        /// 获得每期下注总额
        /// </summary>
        /// <param name="CID"></param>
        /// <returns></returns>
        public int VoteNum(int CID)
        {
            return dal.VoteNum(CID);
        }

        /// <summary>
        /// 获得每期中奖注数
        /// </summary>
        /// <param name="cid"></param>
        /// <returns></returns>
        public int PrizeNum(int cid)
        {
            return dal.PrizeNum(cid);
        }

        /// <summary>
        /// 获取投注数
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public int VoteNum(int ID, int CID)
        {
            return dal.VoteNum(ID, CID);
        }
        /// <summary>
        /// 获取投注数
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public int VoteNum1(int ID, int CID)
        {
            return dal.VoteNum1(ID, CID);
        }
        /// <summary>
        /// 获取投注数
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public int VoteNum2(int ID, int CID)
        {
            return dal.VoteNum2(ID, CID);
        }
        /// <summary>
        /// 是否存在兑奖记录
        /// </summary>
        public bool ExistsState(int id, int UsID)
        {
            return dal.ExistsState(id, UsID);
        }

        /// <summary>
        /// 更新用户兑奖标识
        /// </summary>
        public void UpdateState(int id)
        {
            dal.UpdateState(id);
        }

        /// <summary>
        /// 得到一个WinCent
        /// </summary>
        public long GetWinCent(int ID)
        {
            return dal.GetWinCent(ID);
        }

        /// <summary>
        /// 得到一个WinCent
        /// </summary>
        public long GetPayCent(string time1, string time2)
        {
            return dal.GetPayCent(time1, time2);
        }

        //盈利分析---------------------------------
        /// <summary>
        /// 得到一个WinCent
        /// </summary>
        public long GetPayCentlast()
        {
            return dal.GetPayCentlast();
        }

        /// <summary>
        /// 计算投注总币值
        /// </summary>
        public long GetPrice(string ziduan, string strWhere)
        {
            return dal.GetPrice(ziduan, strWhere);
        }

        //盈利分析---------------------------------
        /// <summary>
        /// 得到一个WinCent5
        /// </summary>
        public long GetPayCentlast5()
        {
            return dal.GetPayCentlast5();
        }
        /// <summary>
        /// 得到一个WinCent
        /// </summary>
        public long GetWinCent(string time1, string time2)
        {
            return dal.GetWinCent(time1, time2);
        }
        /// <summary>
        /// 得到一个WinCentlast
        /// </summary>
        public long GetWinCentlast()
        {
            return dal.GetWinCentlast();
        }
        /// <summary>
        /// 得到一个WinCentlast5
        /// </summary>
        public long GetWinCentlast5()
        {
            return dal.GetWinCentlast5();
        }
        /// <summary>
        /// 获得总下注额根据UsID
        /// </summary>
        /// <returns></returns>
        public long getAllPricebyusID(int UsID)
        {
            return dal.getAllPricebyusID(UsID);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(BCW.BQC.Model.BQCPay model)
		{
			return dal.Add(model);
		}

        /// <summary>
        /// 获得总下注数
        /// </summary>
        /// <param name="CID"></param>
        /// <returns></returns>
        public long getAllPrice(int CID)
        {
            return dal.getAllPrice(CID);
        }

        public long AllWinCentbyusID(int usID)
        {
            return dal.AllWinCentbyusID(usID);
        }

        public int AllVoteNumbyusID(int usID)
        {
            return dal.AllVoteNumbyusID(usID);
        }

        public int AllVoteNum(int cid)
        {
            return dal.AllVoteNum(cid);
        }


        public long AllWinCentbyCID(int CID)
        {
            return dal.AllWinCentbyCID(CID);
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(BCW.BQC.Model.BQCPay model)
		{
			dal.Update(model);
		}

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void UpdateChange(int id, int i)
        {
            dal.UpdateChange(id, i);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int id)
		{
			
			dal.Delete(id);
		}

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(string strWhere)
        {

            dal.Delete(strWhere);
        }

        /// <summary>
        /// 得到中1等奖注数
        /// </summary>
        /// <param name="CID"></param>
        /// <param name="IsPrize"></param>
        /// <returns></returns>
        public int countPrize(int CID, int IsPrize)
        {
            return dal.countPrize(CID, IsPrize);
        }
        /// <summary>
        /// 得到中2等奖注数
        /// </summary>
        /// <param name="CID"></param>
        /// <param name="IsPrize"></param>
        /// <returns></returns>
        public int countPrize2(int CID)
        {
            return dal.countPrize2(CID);
        }
        /// <summary>
        /// 得到中奖的币
        /// </summary>
        /// <param name="CID"></param>
        /// <param name="Isprize"></param>
        /// <returns></returns>
        public long Gold(int CID, int Isprize)
        {
            return dal.Gold(CID, Isprize);
        }

        /// <summary>
        /// 总下注额
        /// </summary>
        /// <returns></returns>
        public long AllPrice(int CID)
        {
            return dal.AllPrice(CID);
        }
        /// <summary>
        /// 总中奖额
        /// </summary>
        /// <returns></returns>
        public long AllWinCent()
        {
            return dal.AllWinCent();
        }

        /// <summary>
        /// 获得总总奖额
        /// </summary>
        /// <returns></returns>
        public long AllWinCent(int CID)
        {
            return dal.AllWinCent(CID);
        }
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.BQC.Model.BQCPay GetBQCPay(int id)
		{
			
			return dal.GetBQCPay(id);
		}

		/// <summary>
		/// 根据字段取数据列表
		/// </summary>
		public DataSet GetList(string strField, string strWhere)
		{
			return dal.GetList(strField, strWhere);
		}

        public DataSet GetList(int CID)
        {
            return dal.GetList(CID);
        }

        /// <summary>
        /// 取得每页记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList BQCPay</returns>
        public IList<BCW.BQC.Model.BQCPay> GetBQCPays(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetBQCPays(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}
        /// <summary>
        /// 取得每页记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList BQCPay</returns>
        public IList<BCW.BQC.Model.BQCPay> GetBQCPays1(int p_pageIndex, int p_pageSize, string strWhere,string strOrder, out int p_recordCount)
        {
            return dal.GetBQCPays1(p_pageIndex, p_pageSize, strWhere,strOrder, out p_recordCount);
        }
        /// <summary>
        /// 取得排行记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList HcPay</returns>
        public IList<BCW.BQC.Model.BQCPay> GetBQCPaysTop(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetBQCPaysTop(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }

        #endregion  成员方法
    }
}

