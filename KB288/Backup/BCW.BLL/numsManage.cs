using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
	/// <summary>
	/// 业务逻辑类tb_numsManage 的摘要说明。
	/// </summary>
	public class numsManage
	{
		private readonly BCW.DAL.numsManage dal =new BCW.DAL.numsManage();
		public numsManage()
		{}
		#region  成员方法
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
        public bool ExistsByUsID(int UsID)
        {
            return dal.ExistsByUsID(UsID);
        }
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int  Add(BCW.Model.numsManage model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(BCW.Model.numsManage model)
		{
			dal.Update(model);
		}
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void UpdateByUI(BCW.Model.numsManage model)
        {
            dal.UpdateByUI(model);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int ID)
		{
			
			dal.Delete(ID);
		}
        /// <summary>
        /// 更新管理密码
        /// </summary>
        public void UpdatePwd(int UsID, string Pwd)
        {
            dal.UpdatePwd(UsID,Pwd);
        }
        /// <summary>
        /// 更新在线时间
        /// </summary>
        public void UpdateTime(int ID)
        {
            dal.UpdateTime(ID);
        }
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.Model.numsManage GetByUsID(int UsID)
        {
            return dal.GetByUsID(UsID);
        }
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.Model.numsManage Gettb_numsManage(int ID)
		{
			
			return dal.Gettb_numsManage(ID);
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
		/// <returns>IList tb_numsManage</returns>
		public IList<BCW.Model.numsManage> Gettb_numsManages(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.Gettb_numsManages(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

		#endregion  成员方法
	}
}

