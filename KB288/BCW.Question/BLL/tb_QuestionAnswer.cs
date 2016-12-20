using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
	/// <summary>
	/// 业务逻辑类tb_QuestionAnswer 的摘要说明。
	/// </summary>
	public class tb_QuestionAnswer
	{
		private readonly BCW.DAL.tb_QuestionAnswer dal=new BCW.DAL.tb_QuestionAnswer();
		public tb_QuestionAnswer()
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
		/// 增加一条数据
		/// </summary>
		public int  Add(BCW.Model.tb_QuestionAnswer model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(BCW.Model.tb_QuestionAnswer model)
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
        /// 得到每人获奖答题总次数
        /// </summary>
        public int GetAllCounts(int usid)
        {
            return dal.GetAllCounts(usid);
        }
        /// <summary>
        /// 得到每人获奖答对题总次数
        /// </summary>
        public int GetTrueCounts(int usid)
        {
            return dal.GetTrueCounts(usid);
        }
        /// <summary>
        /// 得到每人获奖答错题总次数
        /// </summary>
        public int GetflaseCounts(int usid)
        {
            return dal.GetflaseCounts(usid);
        }
        ///// <summary>
        ///// 得到每人获奖总奖励
        ///// </summary>
        //public int GetAllAwardGold(int usid)
        //{
        //    return dal.GetAllAwardGold(usid);
        //}
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.Model.tb_QuestionAnswer Gettb_QuestionAnswer(int ID)
		{
			
			return dal.Gettb_QuestionAnswer(ID);
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
		/// <returns>IList tb_QuestionAnswer</returns>
		public IList<BCW.Model.tb_QuestionAnswer> Gettb_QuestionAnswers(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.Gettb_QuestionAnswers(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

		#endregion  成员方法
	}
}

