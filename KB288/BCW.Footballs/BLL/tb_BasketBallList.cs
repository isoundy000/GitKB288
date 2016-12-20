using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
    /// <summary>
    /// 业务逻辑类tb_BasketBallList 的摘要说明。
    /// </summary>
    public class tb_BasketBallList
    {
        private readonly BCW.DAL.tb_BasketBallList dal = new BCW.DAL.tb_BasketBallList();
        public tb_BasketBallList()
        { }
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
		public bool ExistsName(int name_en)
        {
            return dal.ExistsName(name_en);
        }
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(BCW.Model.tb_BasketBallList model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新connectId
        /// 与官网的球赛关联
        /// </summary>
        public void UpdateConnectId(int ID, int LinkId)
        {
            dal.UpdateConnectId(ID, LinkId);
        }

        /// <summary>
        /// 更新isHidden 球赛显示与隐藏
        /// </summary>
        public void UpdateHidden(int ID, int isHidden)
        {
            dal.UpdateHidden(ID, isHidden);
        }
        /// <summary>
        /// 更新比分1-4节主客比分 
        /// </summary>
        public void UpdateOneScore(int ID, string homeone, string guestone)
        {
            dal.UpdateOneScore(ID, homeone, guestone);
        }
        /// <summary>
        /// 更新比分2节主客比分 
        /// </summary>
        public void UpdateTwoScore(int ID, string hometwo, string guesttwo)
        {
            dal.UpdateTwoScore(ID, hometwo, guesttwo);
        }
        /// <summary>
        /// 更新比分3节主客比分 
        /// </summary>
        public void UpdateThreeScore(int ID, string homethree, string guestthree)
        {
            dal.UpdateThreeScore(ID, homethree, guestthree);
        }
        /// <summary>
        /// 更新比分4节主客比分 
        /// </summary>
        public void UpdateFourScore(int ID, string homefour, string guestfour)
        {
            dal.UpdateThreeScore(ID, homefour, guestfour);
        }
        /// <summary>
        /// 更新比分主客 
        /// </summary>
        public void UpdateScore(int ID, int home, int guest)
        {
            dal.UpdateScore(ID, home, guest);
        }
        /// <summary>
        /// 更新进球数据explain 1
        /// </summary>
        public void UpdateExplain(int ID, string explain, string explain2)
        {
            dal.UpdateExplain(ID, explain, explain2);
        }
        /// <summary>
        /// 更新result
        /// </summary>
        public void UpdateResult(int ID, string result)
        {
            dal.UpdateResult(ID, result);
        }
        /// <summary>
        /// 更新isDone
        /// </summary>
        public void UpdateisDone(int ID, string isDone)
        {
            dal.UpdateisDone(ID, isDone);
        }
        /// <summary>
        /// 更新matchstate
        /// </summary>
        public void Updatematchstate(int ID, string matchstate)
        {
            dal.Updatematchstate(ID, matchstate);
        }
        /// <summary>
        /// 更新Europe
        /// </summary>
        public void UpdateEurope(int ID, string homeEurope, string guestEurope)
        {
            dal.UpdateEurope(ID, homeEurope, guestEurope);
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(BCW.Model.tb_BasketBallList model)
        {
            dal.Update(model);
        }
        /// <summary>
		/// 更新一条数据Name_en
		/// </summary>
		public void UpdateName_en(BCW.Model.tb_BasketBallList model)
        {
            dal.UpdateName_en(model);
        }
        /// <summary>
        /// 更新一条数据Name_en1
        /// </summary>
        public void UpdateName_en1(BCW.Model.tb_BasketBallList model)
        {
            dal.Updatename_en1(model);
        }
        /// <summary>
        /// 更新一条数据Name_en1
        /// 更新 开赛时间 比分 欧赔 
        /// </summary>
        public void UpdateName_en2(BCW.Model.tb_BasketBallList model)
        {
            dal.Updatename_en2(model);
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
        public BCW.Model.tb_BasketBallList Gettb_BasketBallList(int ID)
        {

            return dal.Gettb_BasketBallList(ID);
        }

        /// <summary>
		/// 得到一个对象实体
		/// </summary>
		public BCW.Model.tb_BasketBallList Gettb_BasketBallListForName_en(int Name_en)
        {

            return dal.Gettb_BasketBallListForName_en(Name_en);
        }
        /// <summary>
		/// 得到编号
		/// </summary>
        public int GetName_enFromId(int ID)
        {
            return dal.GetName_enFromId(ID);
        }

        /// <summary>
		/// 得到比赛状态
		/// </summary>
        public string GetStateFromId(int ID)
        {
            return dal.GetStateFromId(ID);
        }

        /// <summary>
        /// 得到编号
        /// </summary>
        public int GetIDFromName_en(int name_en)
        {
            return dal.GetIDFromName_en(name_en);
        }
        /// <summary>
		/// 通过ID得到主队比分
		/// </summary>
        public int GetHomeScoreFromId(int ID)
        {
            return dal.GetHomeScoreFromId(ID);
        }
        /// <summary>
		/// 得到客队实体
		/// </summary>
        public int GetGuestScoreFromId(int ID)
        {
            return dal.GetGuestScoreFromId(ID);
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
        /// <returns>IList tb_BasketBallList</returns>
        public IList<BCW.Model.tb_BasketBallList> Gettb_BasketBallLists(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.Gettb_BasketBallLists(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }

        #endregion  成员方法
    }
}

