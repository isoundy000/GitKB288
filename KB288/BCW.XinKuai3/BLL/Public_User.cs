using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.XinKuai3.Model;
namespace BCW.XinKuai3.BLL
{
	/// <summary>
	/// 业务逻辑类Public_User 的摘要说明。
	/// </summary>
	public class Public_User
	{
		private readonly BCW.XinKuai3.DAL.Public_User dal=new BCW.XinKuai3.DAL.Public_User();
		public Public_User()
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
		/// 增加一条数据
		/// </summary>
		public int  Add(BCW.XinKuai3.Model.Public_User model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(BCW.XinKuai3.Model.Public_User model)
		{
			dal.Update(model);
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
		public BCW.XinKuai3.Model.Public_User GetPublic_User(int ID)
		{
			
			return dal.GetPublic_User(ID);
		}

		/// <summary>
		/// 根据字段取数据列表
		/// </summary>
		public DataSet GetList(string strField, string strWhere)
		{
			return dal.GetList(strField, strWhere);
		}

        /// <summary>
        /// me_更新快捷下注
        /// </summary>
        public void Update_1(int ID, string Settings, int type)
        {
            dal.Update_1(ID, Settings, type);
        }
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID ,int type)
        {
            return dal.Exists(ID, type);
        }

        /// <summary>
        /// 取得每页记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList Public_User</returns>
        public IList<BCW.XinKuai3.Model.Public_User> GetPublic_Users(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetPublic_Users(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

		#endregion  成员方法
	}
}

