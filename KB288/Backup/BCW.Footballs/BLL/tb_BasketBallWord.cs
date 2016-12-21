using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
	/// <summary>
	/// 业务逻辑类tb_BasketBallWord 的摘要说明。
	/// </summary>
	public class tb_BasketBallWord
	{
		private readonly BCW.DAL.tb_BasketBallWord dal=new BCW.DAL.tb_BasketBallWord();
		public tb_BasketBallWord()
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
		/// 是否存在该name_en记录的其中一句
		/// </summary>
		public bool ExistsName_enOne(int ID)
        {
            return dal.ExistsName_enOne(ID);
        }
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool ExistsName(int name_enId)
        {
            return dal.ExistsName(name_enId);
        }
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int  Add(BCW.Model.tb_BasketBallWord model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(BCW.Model.tb_BasketBallWord model)
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
		public BCW.Model.tb_BasketBallWord Gettb_BasketBallWord(int ID)
		{
			
			return dal.Gettb_BasketBallWord(ID);
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
		/// <returns>IList tb_BasketBallWord</returns>
		public IList<BCW.Model.tb_BasketBallWord> Gettb_BasketBallWords(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.Gettb_BasketBallWords(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

		#endregion  成员方法
	}
}

