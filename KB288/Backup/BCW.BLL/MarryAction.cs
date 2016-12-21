using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
using System.Text.RegularExpressions;
namespace BCW.BLL
{
	/// <summary>
	/// 业务逻辑类MarryAction 的摘要说明。
    /// 婚恋加入活跃抽奖--姚志光20160528
	/// </summary>
	public class MarryAction
	{
		private readonly BCW.DAL.MarryAction dal=new BCW.DAL.MarryAction();
		public MarryAction()
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
        /// 婚恋动态加入活跃抽奖20160528
		/// </summary>
		public int  Add(BCW.Model.MarryAction model)
		{
			int id= dal.Add(model);
            string Notes = model.Content;
            int UsId = 0;
            string xmlPath = "/Controls/winners.xml";
            string TextForUbb = (ub.GetSub("TextForUbb", xmlPath));//设置内线提示的文字
            string WinnersStatus = (ub.GetSub("WinnersStatus", xmlPath));//状态1维护2测试0正常
            string WinnersOpenOrClose = (ub.GetSub("WinnersOpenOrClose", xmlPath));//0|停止放送机会|1|开启放送机会
            string WinnersOpenChoose = (ub.GetSub("WinnersOpenChoose", xmlPath));//1全社区2社区3仅游戏 
            string WinnersGuessOpen = (ub.GetSub("WinnersGuessOpen", xmlPath));//1发内线2不发内线 
            string ActionText = (ub.GetSub("ActionText", xmlPath));//Action语句
            string ActionOpen = (ub.GetSub("ActionOpen", xmlPath));//Action语句开关
            //活跃抽奖开关
            if (WinnersStatus != "1" && WinnersOpenOrClose == "1")
            {
                try
                {
                    if (UsId == 0)//会员ID为空返回3
                    {
                        //url=/bbs/uinfo.aspx?uid=" + meid +             
                        Match m;
                        Match m1;
                        string reg = "uid=[0-9]\\d*";
                        string reg1 = "[0-9]\\d*";
                        m = Regex.Match(Notes, reg);
                        m1 = Regex.Match(m.Groups[0].ToString(), reg1);
                        UsId = Convert.ToInt32(m1.Groups[0].ToString());
                        try
                        {
                            if (!new BCW.BLL.tb_WinnersLists().ExistsUserID(UsId))
                            {
                                return id;
                            }
                        }
                        catch { }
                    }
                    if (UsId == 0)//会员ID为空返回
                    { return id; }
                    //是否中奖：返回1中将
                    string UsName = new BCW.BLL.User().GetUsName(UsId);
                    int isHit = new BCW.winners.winners().CheckActionForAll(0, 0, UsId, UsName, Notes, id);
                    if (isHit == 1)
                    {
                        if (WinnersGuessOpen == "1")
                        {
                            new BCW.BLL.Guest().Add(0, UsId, UsName, TextForUbb);//发内线到该ID
                            //if (ActionOpen == "1")
                            //{
                            //    Add(UsId, ActionText);
                            //}
                        }
                    }
                    return id;
                }
                catch
                {
                    return id;
                }
            }
            else
            {
                return id;
            }
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(BCW.Model.MarryAction model)
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
		public BCW.Model.MarryAction GetMarryAction(int ID)
		{
			
			return dal.GetMarryAction(ID);
		}

		/// <summary>
		/// 根据字段取数据列表
		/// </summary>
		public DataSet GetList(string strField, string strWhere)
		{
			return dal.GetList(strField, strWhere);
		}

                
        /// <summary>
        /// 取得固定列表记录
        /// </summary>
        /// <param name="SizeNum">列表记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList MarryAction</returns>
        public IList<BCW.Model.MarryAction> GetMarryActions(int SizeNum, string strWhere)
        {
            return dal.GetMarryActions(SizeNum, strWhere);
        }

		/// <summary>
		/// 取得每页记录
		/// </summary>
		/// <param name="p_pageIndex">当前页</param>
		/// <param name="p_pageSize">分页大小</param>
		/// <param name="p_recordCount">返回总记录数</param>
		/// <param name="strWhere">查询条件</param>
		/// <returns>IList MarryAction</returns>
		public IList<BCW.Model.MarryAction> GetMarryActions(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetMarryActions(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

		#endregion  成员方法
	}
}

