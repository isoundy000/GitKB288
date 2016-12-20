using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using Book.Model;
namespace Book.BLL
{
	/// <summary>
	/// 业务逻辑类ShuBbs 的摘要说明。
    /// 
    /// 2016-08-23 增加点值抽奖发表书评接口 蒙宗将
	/// </summary>
	public class ShuBbs
	{
		private readonly Book.DAL.ShuBbs dal=new Book.DAL.ShuBbs();
		public ShuBbs()
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
		public bool Exists(int id)
		{
			return dal.Exists(id);
		}

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(Book.Model.ShuBbs model)
		{
			int id= dal.Add(model);

            try
            {
                int usid = model.usid;
                string username = model.usname;
                string Notes = "发表书评";
                new BCW.Draw.draw().AddjfbyTz(usid, username, Notes);//点值抽奖
            }
            catch { }

            return id;
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(Book.Model.ShuBbs model)
		{
			dal.Update(model);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int id)
		{
			
			dal.Delete(id);
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public Book.Model.ShuBbs GetShuBbs(int id)
		{
			
			return dal.GetShuBbs(id);
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
		/// <returns>IList ShuBbs</returns>
		public IList<Book.Model.ShuBbs> GetShuBbss(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetShuBbss(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

		#endregion  成员方法
	}
}

