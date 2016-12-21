using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
	/// <summary>
	/// 业务逻辑类dawnlifeUser 的摘要说明。
	/// </summary>
	public class dawnlifeUser
	{
		private readonly BCW.DAL.dawnlifeUser dal=new BCW.DAL.dawnlifeUser();
		public dawnlifeUser()
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
		public int  Add(BCW.Model.dawnlifeUser model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(BCW.Model.dawnlifeUser model)
		{
			dal.Update(model);
		}
        //更新一个字段
        public void Updatedebt(int ID, long debt)
        {
            dal.Updatedebt(ID, debt);
        }
        public void Updatemoney(int ID, long money)
        {
            dal.Updatemoney(ID, money);
        }
        public void Updatecoin(int ID, long coin)
        {
            dal.Updatemoney(ID, coin);
        }
        public void UpdateStock(int ID, string stock)
        {
            dal.UpdateStock(ID, stock);
        }
        public void UpdateStorehouse(int ID, string storehouse)
        {
            dal.UpdateStorehouse(ID, storehouse);
        }
        public void Updatehealth(int ID, int health)
        {
            dal.Updatehealth(ID, health);
        }
        public void Updatereputation(int ID, int reputation)
        {
            dal.Updatereputation(ID, reputation);
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
		public BCW.Model.dawnlifeUser GetdawnlifeUser(int ID)
		{
			
			return dal.GetdawnlifeUser(ID);
		}


        /// 根据查询影响的行数
        /// </summary>
        public int GetRowByUsID(int UsID)
        {
            return dal.GetRowByUsID(UsID);
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
		/// <returns>IList dawnlifeUser</returns>
		public IList<BCW.Model.dawnlifeUser> GetdawnlifeUsers(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetdawnlifeUsers(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

		#endregion  成员方法
	}
}

