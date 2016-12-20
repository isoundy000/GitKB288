using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
	/// <summary>
	/// 业务逻辑类tb_QuestionControl 的摘要说明。
	/// </summary>
	public class tb_QuestionControl
	{
		private readonly BCW.DAL.tb_QuestionControl dal=new BCW.DAL.tb_QuestionControl();
		public tb_QuestionControl()
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
		public int  Add(BCW.Model.tb_QuestionControl model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(BCW.Model.tb_QuestionControl model)
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
        public int GetIDForUsid(int usid)
        {
          return  dal.GetIDForUsid(usid);
        }
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.Model.tb_QuestionControl Gettb_QuestionControl(int ID)
		{
			
			return dal.Gettb_QuestionControl(ID);
		}
        /// <summary>
        ///更新answerList
        /// </summary>
        public void UpdateAnswerList(int ID, string answerList)
        {
             dal.UpdateAnswerList(ID, answerList);
        }
        
        /// <summary>
        ///更新isDone
        /// </summary>
        public void UpdateIsFlase(int ID, int isFalse)
        {
            dal.UpdateIsFlase(ID, isFalse);
        }
        /// <summary>
        ///更新isDone
        /// </summary>
        public void UpdateIsDone(int ID, int isDone)
        {
            dal.UpdateIsDone(ID, isDone);
        }
        /// <summary>
        ///更新judge
        /// </summary>
        //public void UpdateJudge(int ID, string judge)
        //{
        //    dal.UpdateJudge(ID, judge);
        //}
        /// <summary>
        ///更新isDone
        /// </summary>
        public void UpdateIsTrue(int ID, int isTrue)
        {
            dal.UpdateIsTrue(ID, isTrue);
        }

        /// <summary>
        ///更新qNow
        /// </summary>
        public void UpdateqNow(int ID, int qNow)
        {
            dal.UpdateqNow(ID,qNow);
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
		/// <returns>IList tb_QuestionControl</returns>
		public IList<BCW.Model.tb_QuestionControl> Gettb_QuestionControls(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.Gettb_QuestionControls(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

		#endregion  成员方法
	}
}

