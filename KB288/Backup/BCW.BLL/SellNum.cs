using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
	/// <summary>
	/// 业务逻辑类SellNum 的摘要说明。
	/// </summary>
	public class SellNum
	{
		private readonly BCW.DAL.SellNum dal=new BCW.DAL.SellNum();
		public SellNum()
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
        public bool Exists(int Types, int ID)
        {
            return dal.Exists(Types, ID);
        }

        /// <summary>
        /// 是否存在该提交记录
        /// </summary>
        public bool Exists(int Types, int BuyUID, int State)
        {
            return dal.Exists(Types, BuyUID, State);
        }

        /// <summary>
        /// 是否存在该提交记录
        /// </summary>
        public bool Exists(int Types, int UsID, int BuyUID, int State)
        {
            return dal.Exists(Types, UsID, BuyUID, State);
        }
                
        /// <summary>
        /// 计算某会员今天兑换了多少充值或Q币
        /// </summary>
        public int GetSumBuyUID(int Types, int UsID)
        {
            return dal.GetSumBuyUID(Types, UsID);
        }
                    
        /// <summary>
        /// 每个ID每月只能为2个QQ号进行开通服务
        /// </summary>
        public int GetSumQQCount(int UsID)
        {
            return dal.GetSumQQCount(UsID);
        }

        /// <summary>
        /// 计算某会员今天查询多少次报价
        /// </summary>
        public int GetCount(int Types, int UsID)
        {
            return dal.GetCount(Types, UsID);
        }
                
        /// <summary>
        /// 计算某QQ号每个服务开通的月份数量
        /// </summary>
        public int GetSumBuyUIDQQ(int Tags, string Mobile, int UsID)
        {
            return dal.GetSumBuyUIDQQ(Tags, Mobile, UsID);
        }

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(BCW.Model.SellNum model)
		{
			return dal.Add(model);
		}

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add2(BCW.Model.SellNum model)
        {
            return dal.Add2(model);
        }

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(BCW.Model.SellNum model)
		{
			dal.Update(model);
		}

        /// <summary>
        /// 更新已报价
        /// </summary>
        public void UpdateState2(int ID, long Price)
        {
            dal.UpdateState2(ID, Price);
        }

        /// <summary>
        /// 更新成为已申请兑换
        /// </summary>
        public void UpdateState3(int ID, string Mobile)
        {
            dal.UpdateState3(ID, Mobile);
        }
               
        /// <summary>
        /// 更新成为已成功
        /// </summary>
        public void UpdateState4(int ID, string Notes)
        {
            dal.UpdateState4(ID, Notes);
        }
                
        /// <summary>
        /// 更新成为已撤销
        /// </summary>
        public void UpdateState9(int ID)
        {
            dal.UpdateState9(ID);
        }

        /// <summary>
        /// 更新Notes
        /// </summary>
        public void UpdateNotes(int ID, string Notes)
        {
            dal.UpdateNotes(ID, Notes);
        }
                
        /// <summary>
        /// 更新QQ服务类型
        /// </summary>
        public void UpdateTags(int ID, int Tags)
        {
            dal.UpdateTags(ID, Tags);
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
		public BCW.Model.SellNum GetSellNum(int ID)
		{
			
			return dal.GetSellNum(ID);
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
		/// <returns>IList SellNum</returns>
		public IList<BCW.Model.SellNum> GetSellNums(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetSellNums(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

		#endregion  成员方法
	}
}

