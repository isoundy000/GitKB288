using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Draw.Model;
namespace BCW.Draw.BLL
{
	/// <summary>
	/// 业务逻辑类DrawUser 的摘要说明。
	/// </summary>
	public class DrawUser
	{
		private readonly BCW.Draw.DAL.DrawUser dal=new BCW.Draw.DAL.DrawUser();
		public DrawUser()
		{}
		#region  成员方法

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
			return dal.GetMaxId();
		}
        //-----------------------------------------------------//
        /// <summary>
        /// 得到用
        /// </summary>
        public BCW.Draw.Model.DrawUser GetOnTime(int GoodsCounts)
        {
            return dal.GetOnTime(GoodsCounts);
        }
        public BCW.Draw.Model.DrawUser GetInTime(int GoodsCounts)
        {
            return dal.GetInTime(GoodsCounts);
        }
        /// <summary>
        /// 根据编号得到奖品类型
        /// </summary>
        public int GetMyGoodsType(int GoodsCounts)
        {
            return dal.GetMyGoodsType(GoodsCounts);
        }
        public int GetMyStatue(int GoodsCounts)
        {
            return dal.GetMyStatue(GoodsCounts);
        }
        /// <summary>
        /// 根据编号得到奖品类型
        /// </summary>
        public DateTime Getontime(int GoodsCounts)
        {
            return dal.Getontime(GoodsCounts);
        }
        /// <summary>
        /// 根据编号得到奖品价值
        /// </summary>
        public int GetMyGoodsValue(int GoodsCounts)
        {
            return dal.GetMyGoodsValue(GoodsCounts);
        }
        /// <summary>
        /// 得到
        /// </summary>
        public string  GetMyGoods(int GoodsCounts)
        {
            return dal.GetMyGoods(GoodsCounts);
        }


        /// <summary>
        /// 得到用
        /// </summary>
        public BCW.Draw.Model.DrawUser GetOnTimebynum(int Num)
        {
            return dal.GetOnTimebynum(Num);
        }
        public BCW.Draw.Model.DrawUser GetInTimebynum(int Num)
        {
            return dal.GetInTimebynum(Num);
        }
        /// <summary>
        /// 根据编号得到奖品类型
        /// </summary>
        public int GetMyGoodsTypebynum(int Num)
        {
            return dal.GetMyGoodsTypebynum(Num);
        }
        public int GetMyStatuebynum(int Num)
        {
            return dal.GetMyStatuebynum(Num);
        }
        /// <summary>
        /// 根据编号得到奖品类型
        /// </summary>
        public DateTime Getontimebynum(int Num)
        {
            return dal.Getontimebynum(Num);
        }
        /// <summary>
        /// 根据编号得到奖品价值
        /// </summary>
        public int GetMyGoodsValuebynum(int Num)
        {
            return dal.GetMyGoodsValuebynum(Num);
        }
        /// <summary>
        /// 根据编号得到奖品价值
        /// </summary>
        public int GetMyGoodsNumbynum(int Num)
        {
            return dal.GetMyGoodsNumbynum(Num);
        }
        /// <summary>
        /// 得到
        /// </summary>
        public string GetMyGoodsbynum(int Num)
        {
            return dal.GetMyGoodsbynum(Num);
        }


        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void UpdateMessage(int GoodsCounts, string Address, string Phone, string Email,string RealName , int MyGoodsStatue)
        {
            dal.UpdateMessage(GoodsCounts, Address, Phone, Email,RealName , MyGoodsStatue);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void UpdateExpress(int GoodsCounts, string Express, string Numbers, int MyGoodsStatue)
        {
            dal.UpdateExpress(GoodsCounts, Express, Numbers, MyGoodsStatue);
        }

        public void UpdateIntime(int GoodsCounts, DateTime InTime)
        {
            dal.UpdateIntime(GoodsCounts ,InTime);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void UpdateMyGoodsStatue(int GoodsCounts, int MyGoodsStatue)
        {
            dal.UpdateMyGoodsStatue(GoodsCounts, MyGoodsStatue);
        }


        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void UpdateMessagebynum(int Num, string Address, string Phone, string Email, string RealName, int MyGoodsStatue)
        {
            dal.UpdateMessagebynum(Num, Address, Phone, Email, RealName, MyGoodsStatue);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void UpdateExpressbynum(int Num, string Express, string Numbers, int MyGoodsStatue)
        {
            dal.UpdateExpressbynum(Num, Express, Numbers, MyGoodsStatue);
        }

        public void UpdateIntimebynum(int Num, DateTime InTime)
        {
            dal.UpdateIntimebynum(Num, InTime);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void UpdateMyGoodsStatuebynum(int Num, int MyGoodsStatue)
        {
            dal.UpdateMyGoodsStatuebynum(Num, MyGoodsStatue);
        }

        //-----------------------------------------------------//
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
        public bool Existsnum(int Num)
        {
            return dal.Existsnum(Num);
        }
		/// <summary>
		/// 增加一条数据
		/// </summary>
		public void Add(BCW.Draw.Model.DrawUser model)
		{
			dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(BCW.Draw.Model.DrawUser model)
		{
			dal.Update(model);
		}
        /// <summary>
        /// me_初始化某数据表
        /// </summary>
        /// <param name="TableName">数据表名称</param>
        public void ClearTable(string TableName)
        {
            dal.ClearTable(TableName);
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
		public BCW.Draw.Model.DrawUser GetDrawUser(int ID)
		{
			
			return dal.GetDrawUser(ID);
		}

     

        ///--------------------------根据编号取数据
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.Draw.Model.DrawUser GetDrawUserbyCounts(int GoodsCounts)
        {

            return dal.GetDrawUserbyCounts(GoodsCounts);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.Draw.Model.DrawUser GetDrawUserbynum(int Num)
        {

            return dal.GetDrawUserbynum(Num);
        }
        /// <summary>
        /// me_计算和值出现次数并排序
        /// </summary>
        public IList<BCW.Draw.Model.DrawUser> Get_UsID(string _where)
        {
            return dal.Get_UsID(_where);
        }


        /// <summary>
        /// 取得排行记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList HcPay</returns>
        public IList<BCW.Draw.Model.DrawUser> GetUserTop(int p_pageIndex, int p_pageSize,string _where, string strWhere, out int p_recordCount)
        {
            return dal.GetUserTop(p_pageIndex, p_pageSize, _where,strWhere, out p_recordCount);
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
		/// <returns>IList DrawUser</returns>
        public IList<BCW.Draw.Model.DrawUser> GetDrawUsers(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
		{
			return dal.GetDrawUsers(p_pageIndex, p_pageSize, strWhere, strOrder, out p_recordCount);
		}

        /// <summary>
        /// 取得每页记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList DrawUser</returns>
        public IList<BCW.Draw.Model.DrawUser> GetDrawUsers1(int p_pageIndex, int p_pageSize, string strWhere, string strOrder,int iSCounts, out int p_recordCount)
        {
            return dal.GetDrawUsers1(p_pageIndex, p_pageSize, strWhere, strOrder,iSCounts, out p_recordCount);
        }

		#endregion  成员方法
	}
}

