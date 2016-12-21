using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Draw.Model;
namespace BCW.Draw.BLL
{
	/// <summary>
	/// 业务逻辑类DrawBox 的摘要说明。
	/// </summary>
	public class DrawBox
	{
		private readonly BCW.Draw.DAL.DrawBox dal=new BCW.Draw.DAL.DrawBox();
		public DrawBox()
		{}
		#region  成员方法

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
			return dal.GetMaxId();
		}

//----------------------------------------------------------------------------------------------//
        /// <summary>
        /// 得到用
        /// </summary>
        public string GetGoodsName(int GoodsCounts)
        {
            return dal.GetGoodsName(GoodsCounts);
        }
        /// <summary>
        /// 得到用
        /// </summary>
        public string GetExplain(int GoodsCounts)
        {
            return dal.GetExplain(GoodsCounts);
        }    /// <summary>
        /// 得到用
        /// </summary>
        public string GetGoodsImg(int GoodsCounts)
        {
            return dal.GetGoodsImg(GoodsCounts);
        }    /// <summary>
        /// 得到用
        /// </summary>
        public int GetGoodsType(int GoodsCounts)
        {
            return dal.GetGoodsType(GoodsCounts);
        }    /// <summary>
        /// 得到用
        /// </summary>
        public int GetGoodsValue(int GoodsCounts)
        {
            return dal.GetGoodsValue(GoodsCounts);
        }  
        /// <summary>
        /// 得到用
        /// </summary>
        public int GetGoodsNum(int GoodsCounts)
        {
            return dal.GetGoodsNum(GoodsCounts);
        }
        /// <summary>
        /// 得到用
        /// </summary>
        public int GetRank(int GoodsCounts)
        {
            return dal.GetRank(GoodsCounts);
        }
        /// <summary>
        /// 得到用
        /// </summary>
        public int GetID(int GoodsCounts)
        {
            return dal.GetID(GoodsCounts);
        }
        /// <summary>
        /// 得到一个奖品数量
        /// </summary>
        public int GetAllNumbyC(int GoodsCounts)
        {
            return dal.GetAllNumbyC(GoodsCounts);
        }
        /// <summary>
        /// 得到正在进行中的奖品数量
        /// </summary>
        public int GetAllNum(int lun)
        {
            return dal.GetAllNum(lun);
        }
        /// <summary>
        /// 得到正在进行中的奖品余量
        /// </summary>
        public int GetAllNumS(int lun)
        {
            return dal.GetAllNumS(lun );
        }
        /// <summary>
        /// 得到第几轮奖池
        /// </summary>
        public int Getlun()
        {
            return dal.Getlun();
        }
        /// <summary>
        /// 得到用
        /// </summary>
        public int GetStatue(int GoodsCounts)
        {
            return dal.GetStatue(GoodsCounts);
        }
 //----------------------------------------------------------------------------------------------//

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int ID)
		{
			return dal.Exists(ID);
		}
        /// <summary>
        /// 是否存在该记录根据编号
        /// </summary>
        public bool CountsExists(int GoodsCounts)
        {
            return dal.CountsExists(GoodsCounts);
        }

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public void Add(BCW.Draw.Model.DrawBox model)
		{
			dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(BCW.Draw.Model.DrawBox model)
		{
			dal.Update(model);
		}

        //////---------------

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void UpdateStatue(int GoodsCounts,int Statue)
        {
            dal.UpdateStatue(GoodsCounts,Statue);
        }
        /// <summary>
        /// 更新所有商品下架
        /// </summary>
        public void UpdateStatueAllgo()
        {
            dal.UpdateStatueAllgo();
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void UpdateGoodsNum(int GoodsCounts, int GoodsNum)
        {
            dal.UpdateGoodsNum(GoodsCounts, GoodsNum);
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void UpdateGoodsNumgo(int lun)
        {
            dal.UpdateGoodsNumgo(lun);
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void UpdateOverTime(int GoodsCounts, DateTime OverTime)
        {
            dal.UpdateOverTime(GoodsCounts, OverTime);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void UpdateReceiveTime(int GoodsCounts, DateTime ReceiveTime)
        {
            dal.UpdateReceiveTime(GoodsCounts, ReceiveTime);
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
		public BCW.Draw.Model.DrawBox GetDrawBox(int ID)
		{
			
			return dal.GetDrawBox(ID);
		}
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.Draw.Model.DrawBox GetDrawBoxbyC(int GoodsCounts)
        {

            return dal.GetDrawBoxbyC(GoodsCounts);
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
		/// <returns>IList DrawBox</returns>
        public IList<BCW.Draw.Model.DrawBox> GetDrawBoxs(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
		{
			return dal.GetDrawBoxs(p_pageIndex, p_pageSize, strWhere,strOrder, out p_recordCount);
		}

		#endregion  成员方法
	}
}

