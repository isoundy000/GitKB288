using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.JQC.Model;
namespace BCW.JQC.BLL
{
    /// <summary>
    /// 业务逻辑类JQC_Bet 的摘要说明。
    /// </summary>
    public class JQC_Bet
    {
        private readonly BCW.JQC.DAL.JQC_Bet dal = new BCW.JQC.DAL.JQC_Bet();
        public JQC_Bet()
        { }
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
        public int Add(BCW.JQC.Model.JQC_Bet model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(BCW.JQC.Model.JQC_Bet model)
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
        public BCW.JQC.Model.JQC_Bet GetJQC_Bet(int ID)
        {
            return dal.GetJQC_Bet(ID);
        }

        /// <summary>
        /// 根据字段取数据列表
        /// </summary>
        public DataSet GetList(string strField, string strWhere)
        {
            return dal.GetList(strField, strWhere);
        }

        //=====================================
        /// <summary>
        /// me_后台分页条件获取排行榜数据列表
        /// </summary>
        public DataSet GetListByPage2(int startIndex, int endIndex, string s1, string s2)
        {
            return dal.GetListByPage2(startIndex, endIndex, s1, s2);
        }
        /// <summary>
        /// me_随机得到一个对象实体
        /// </summary>
        public BCW.JQC.Model.JQC_Bet GetNC_suiji()
        {
            return dal.GetNC_suiji();
        }
        /// <summary>
        /// me_是否存在开奖后没有返奖的
        /// </summary>
        public bool Exists_num(int Lottery_issue)
        {
            return dal.Exists_num(Lottery_issue);
        }
        /// <summary>
        /// me_是否存在兑奖记录
        /// </summary>
        public bool ExistsState(int ID, int UsID)
        {
            return dal.ExistsState(ID, UsID);
        }
        /// <summary>
        /// me_得到该期总中奖人数
        /// </summary>
        public int count_zhu(int Lottery_issue)
        {
            return dal.count_zhu(Lottery_issue);
        }
        /// <summary>
        /// me_得到该期某注的中奖人数
        /// </summary>
        public int count_renshu(int Lottery_issue, string Prize)
        {
            return dal.count_renshu(Lottery_issue, Prize);
        }
        /// <summary>
        /// me_更新中奖状态
        /// </summary>
        public void Update_win(int ID, int State)
        {
            dal.Update_win(ID, State);
        }
        /// <summary>
        ///  me_根据字段修改数据列表
        /// </summary>
        public DataSet update_GetMoney(string strField, string strWhere)
        {
            return dal.update_GetMoney(strField, strWhere);
        }
        /// <summary>
        /// me_查询机器人购买次数
        /// </summary>
        public int GetRecordCount(string strWhere)
        {
            return dal.GetRecordCount(strWhere);
        }
        /// <summary>
        /// me_后台，根据开奖的期号，对应该期购买的人数
        /// </summary>
        public BCW.JQC.Model.JQC_Bet Get_tounum(int Lottery_issue)
        {
            return dal.Get_tounum(Lottery_issue);
        }
        /// <summary>
        /// me_根据期号得到一个对象实体
        /// </summary>
        public BCW.JQC.Model.JQC_Bet Get_qihao(int Lottery_issue)
        {
            return dal.Get_qihao(Lottery_issue);
        }
        /// <summary>
        /// me_根据期号得到派奖数
        /// </summary>
        public long Get_paijiang(int Lottery_issue)
        {
            return dal.Get_paijiang(Lottery_issue);
        }
        /// <summary>
        /// 计算投注总币值
        /// </summary>
        public long GetPrice(string ziduan, string strWhere)
        {
            return dal.GetPrice(ziduan, strWhere);
        }
        //==

        /// <summary>
        /// 取得每页记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList JQC_Bet</returns>
        public IList<BCW.JQC.Model.JQC_Bet> GetJQC_Bets(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
        {
            return dal.GetJQC_Bets(p_pageIndex, p_pageSize, strWhere, strOrder, out p_recordCount);
        }

        #endregion  成员方法
    }
}

