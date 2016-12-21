using System;
using System.Data;
using System.Collections.Generic;
using TPR2.Common;
using TPR2.Model.guess;

/// <summary>
/// 
/// 修改设置是否自动开奖功能 黄国军 20160322
/// </summary>
namespace TPR2.BLL.guess
{
    /// <summary>
    /// 业务逻辑类BaList 的摘要说明。
    /// </summary>
    public class BaList
    {
        public static String[] TYPE_NAMES = { "全场让球盘", "全场大小盘", "全场标准盘", "半场让球盘", "半场大小盘", "半场标准盘", "第一节让球盘", "第一节大小盘", "第二节让球盘", "第二节大小盘", "第三节让球盘", "第三节大小盘" };
        private readonly TPR2.DAL.guess.BaList dal = new TPR2.DAL.guess.BaList();
        public BaList()
        { }
        #region  成员方法

        /// <summary>
        /// 获取历史记录
        /// </summary>
        /// <param name="p_id">球队ID</param>
        /// <param name="ptype">类型（1全场让球盘，2全场大小盘，3全场标准盘；4半场让球盘，5半场大小盘，6半场标准盘；7第一节让球盘，8第一节大小盘；9第二节让球盘，10第二节大小盘；11第三节让球盘，12第三节大小盘）</param>
        /// <returns></returns>
        public IList<TPR2.Model.guess.TBaListNew_History> GetHistory(int p_id, int ptype)
        {
            return dal.GetHistory(p_id, ptype);
        }

        /// <summary>
        /// 得到是否单节半场比赛
        /// </summary>
        public int Getp_basketve(int ID)
        {
            return dal.Getp_basketve(ID);
        }
        /// <summary>
        /// 更新受限ID
        /// </summary>
        public void UpdatexID(int ID, string xID, int Types)
        {
            dal.UpdatexID(ID, xID, Types);
        }

        /// <summary>
        /// 更新全局封盘
        /// </summary>
        public void Updatep_isluck(int ID, int p_isluck)
        {
            dal.Updatep_isluck(ID, p_isluck);
        }

        /// <summary>
        /// 更新状态
        /// </summary>
        public void UpdateOnce(int p_basketve, int p_id, string p_once)
        {
            dal.UpdateOnce(p_basketve, p_id, p_once);
        }
        /// <summary>
        /// 更新单节完场比分
        /// </summary>
        public int UpdateBoResult(int p_basketve, int p_id, int p_result_one, int p_result_two)
        {
            return dal.UpdateBoResult(p_basketve, p_id, p_result_one, p_result_two);
        }

        /// <summary>
        /// 更新单节即时比分
        /// </summary>
        public void UpdateBoResult3(int p_basketve, int p_id, int p_result_temp1, int p_result_temp2)
        {
            dal.UpdateBoResult3(p_basketve, p_id, p_result_temp1, p_result_temp2);
        }
        /// <summary>
        /// 更新一条数据（单节使用）
        /// </summary>
        public void BasketUpdateOdds(TPR2.Model.guess.BaList model)
        {
            dal.BasketUpdateOdds(model);
        }
        /// <summary>
        /// 是不是走地
        /// </summary>
        public bool Existsp_ison(int p_id, int p_basketve)
        {
            return dal.Existsp_ison(p_id, p_basketve);
        }
        /// <summary>
        /// 更新为滚球模式(足球半场)
        /// </summary>
        public void FootOnceType4(int p_id, DateTime dt)
        {
            dal.FootOnceType4(p_id, dt);
        }
        /// <summary>
        /// 更新即时比分8波半场
        /// </summary>
        public void UpdateBoResultHalf(int p_id, int p_result_temp1, int p_result_temp2)
        {
            dal.UpdateBoResultHalf(p_id, p_result_temp1, p_result_temp2);
        }
        /// <summary>
        /// 自动更新赛事比分
        /// </summary>
        public int UpdateZDResult2(TPR2.Model.guess.BaList model)
        {
            return dal.UpdateZDResult2(model);
        }

        /// <summary>
        /// 更新一条数据(载入上半场页面使用)
        /// </summary>
        public void UpdateFalf(TPR2.Model.guess.BaList model)
        {
            dal.UpdateFalf(model);

        }
        /// <summary>
        /// 更新一条数据(载入上半场页面使用)
        /// </summary>
        public void UpdateFalf1(TPR2.Model.guess.BaList model)
        {
            dal.UpdateFalf1(model);

        }

        /// <summary>
        /// 更新一条数据(载入上半场页面使用)
        /// </summary>
        public void UpdateFalf2(TPR2.Model.guess.BaList model)
        {
            dal.UpdateFalf2(model);

        }

        /// <summary>
        /// 更新一条数据(载入上半场页面使用)
        /// </summary>
        public void UpdateFalf3(TPR2.Model.guess.BaList model)
        {
            dal.UpdateFalf3(model);

        }

        /// <summary>
        /// 更新主队红牌数量
        /// </summary>
        public void Updatep_hp_one(int p_id, int p_hp_one)
        {
            dal.Updatep_hp_one(p_id, p_hp_one);
        }

        /// <summary>
        /// 更新客队红牌数量
        /// </summary>
        public void Updatep_hp_two(int p_id, int p_hp_two)
        {
            dal.Updatep_hp_two(p_id, p_hp_two);
        }

        //---------------------------超级投注使用----------------------
        /// <summary>
        /// 更新已确定超级投注会员ID
        /// </summary>
        public void Updatep_usid(int ID, string p_usid)
        {
            dal.Updatep_usid(ID, p_usid);
        }

        /// <summary>
        /// 得到超级投注会员ID
        /// </summary>
        public string Getp_usid(int ID)
        {
            return dal.Getp_usid(ID);
        }

        //---------------------------超级投注使用----------------------
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
        /// 是否已开奖
        /// </summary>
        public bool ExistsIsOpen(int ID)
        {
            return dal.ExistsIsOpen(ID);
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool ExistsByp_id(int p_id)
        {
            return dal.ExistsByp_id(p_id);
        }


        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool ExistsByp_id(int p_id, int p_basketve)
        {
            return dal.ExistsByp_id(p_id, p_basketve);

        }

        /// <summary>
        /// 是否已隐藏大小盘
        /// </summary>
        public bool ExistsDX(int p_id)
        {
            return dal.ExistsDX(p_id);
        }

        /// <summary>
        /// 是否已隐藏标准盘
        /// </summary>
        public bool ExistsBZ(int p_id)
        {
            return dal.ExistsBZ(p_id);
        }

        /// <summary>
        /// 得到usid每场比赛的次数_邵广林20160815
        /// </summary>
        public int Getzqcount(int usID, int bcid)
        {
            return dal.Getzqcount(usID, bcid);
        }

        /// <summary>
        /// 得到查询的记录数
        /// </summary>
        public int GetCount(TPR2.Model.guess.BaList model)
        {
            return dal.GetCount(model);
        }

        /// <summary>
        /// 得到查询的记录数
        /// </summary>
        public int GetCountByp_title(string p_title)
        {
            return dal.GetCountByp_title(p_title);
        }

        /// <summary>
        /// 根据条件得到赛事总注数
        /// </summary>
        public int GetBaListCount(string strWhere)
        {
            return dal.GetBaListCount(strWhere);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(TPR2.Model.guess.BaList model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 增加足球让球盘一条数据
        /// </summary>
        public int FootAdd(TPR2.Model.guess.BaList model)
        {
            return dal.FootAdd(model);
        }

        /// <summary>
        /// 更新赛事隐藏与显示
        /// </summary>
        public void Updatep_del(TPR2.Model.guess.BaList model)
        {
            dal.Updatep_del(model);
        }

        /// <summary>
        /// 更新赛事状态结果
        /// </summary>
        public void Updatep_active(int ID, int p_active)
        {
            dal.Updatep_active(ID, p_active);
        }

        /// <summary>
        /// 更新波胆
        /// </summary>
        public void Updatep_score(int ID, string p_score)
        {
            dal.Updatep_score(ID, p_score);
        }
        /// <summary>
        /// 更新波胆
        /// </summary>
        public void Updatep_score2(int p_id, string p_score)
        {
            dal.Updatep_score2(p_id, p_score);
        }

        /// <summary>
        /// 更新赛事抓取与不抓取
        /// </summary>
        public void Updatep_jc(TPR2.Model.guess.BaList model)
        {
            dal.Updatep_jc(model);
        }

        /// <summary>
        /// 更新赛事人工或自动开奖
        /// </summary>
        public void Updatep_dr(TPR2.Model.guess.BaList model)
        {
            dal.Updatep_dr(model);
        }

        /// <summary>
        /// 更新赛事结果状态
        /// </summary>
        public void Updatep_zd(TPR2.Model.guess.BaList model)
        {
            dal.Updatep_zd(model);
        }

        /// <summary>
        /// 更新赛事比分
        /// </summary>
        public void UpdateResult(TPR2.Model.guess.BaList model)
        {
            dal.UpdateResult(model);
        }

        /// <summary>
        /// 自动更新赛事比分
        /// </summary>
        public int UpdateZDResult(TPR2.Model.guess.BaList model)
        {
            return dal.UpdateZDResult(model);
        }

        /// <summary>
        /// 更新完场比分8波
        /// </summary>
        public int UpdateBoResult(int p_id, int p_result_one, int p_result_two)
        {
            return dal.UpdateBoResult(p_id, p_result_one, p_result_two);
        }

        /// <summary>
        /// 更新即时比分8波
        /// </summary>
        public void UpdateBoResult2(int p_id, int p_result_temp1, int p_result_temp2)
        {
            dal.UpdateBoResult2(p_id, p_result_temp1, p_result_temp2);
        }
        /// <summary>
        /// 更新即时比分8波
        /// </summary>
        public void UpdateBoResult3(int p_id, int p_result_temp1, int p_result_temp2)
        {
            dal.UpdateBoResult3(p_id, p_result_temp1, p_result_temp2);
        }
        /// <summary>
        /// 更新进球时间集合
        /// </summary>
        public void UpdateOnce(TPR2.Model.guess.BaList model)
        {
            dal.UpdateOnce(model);
        }

        /// <summary>
        /// 更新进球时间集合
        /// </summary>
        public void UpdateOnce(int p_id, string p_once)
        {
            dal.UpdateOnce(p_id, p_once);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(TPR2.Model.guess.BaList model)
        {
            dal.Update(model);
        }

        /// <summary>
        /// 更新特殊数据
        /// </summary>
        public void Update2(TPR2.Model.guess.BaList model)
        {
            dal.Update2(model);
        }

        /// <summary>
        /// 更新篮球一条数据
        /// </summary>
        public void BasketUpdate(TPR2.Model.guess.BaList model)
        {
            dal.BasketUpdate(model);
        }

        /// <summary>
        /// 更新足球让球盘
        /// </summary>
        public void FootUpdate(TPR2.Model.guess.BaList model)
        {
            dal.FootUpdate(model);
        }

        /// <summary>
        /// 更新足球大小盘
        /// </summary>
        public void FootdxUpdate(TPR2.Model.guess.BaList model)
        {
            dal.FootdxUpdate(model);
        }

        /// <summary>
        /// 更新足球标准盘
        /// </summary>
        public void FootbzUpdate(TPR2.Model.guess.BaList model)
        {
            dal.FootbzUpdate(model);
        }

        /// <summary>
        /// 更新手工开奖时间
        /// </summary>
        public void Updatep_opentime(int ID)
        {
            dal.Updatep_opentime(ID);
        }

        /// <summary>
        /// 更新滚球比赛的水位变化时间
        /// </summary>
        public void Updatep_updatetime(int p_id)
        {
            dal.Updatep_updatetime(p_id);
        }

        /// <summary>
        /// 更新为滚球模式
        /// </summary>
        public void FootOnceType(int ID, DateTime dt)
        {
            dal.FootOnceType(ID, dt);
        }

        /// <summary>
        /// 更新为滚球模式
        /// </summary>
        public void FootOnceType2(int p_id, DateTime dt)
        {
            dal.FootOnceType2(p_id, dt);
        }

        /// <summary>
        /// 更新是否封盘
        /// </summary>
        public void Updatep_isluck(int p_id, int state, int Types)
        {
            dal.Updatep_isluck(p_id, state, Types, 0, DateTime.Now);
        }
        /// <summary>
        /// 更新是否封盘
        /// </summary>
        public void Updatep_isluck(int p_id, int state, int Types, int p_basketve, DateTime p_temptime)
        {
            dal.Updatep_isluck(p_id, state, Types, p_basketve, p_temptime);
        }

        /// <summary>
        /// 更新是否封盘
        /// </summary>
        public void Updatep_isluck2(int id, int state, int Types)
        {
            dal.Updatep_isluck2(id, state, Types);
        }

        /// <summary>
        /// 更新即时比分（滚球使用）
        /// </summary>
        public void FootOnceUpdate(TPR2.Model.guess.BaList model)
        {
            dal.FootOnceUpdate(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int ID)
        {
            dal.Delete(ID);
        }

        /// <summary>
        /// 删除一组数据
        /// </summary>
        public void DeleteStr(string strWhere)
        {
            dal.DeleteStr(strWhere);
        }

        /// <summary>
        /// 得到球类型
        /// </summary>
        public int GetPtype(int ID)
        {
            return dal.GetPtype(ID);
        }

        /// <summary>
        /// 得到比赛ID
        /// </summary>
        public int GetID(int p_id)
        {
            return dal.GetID(p_id);
        }

        /// <summary>
        /// 得到联赛时间
        /// </summary>
        public DateTime Getp_TPRtime(int p_id)
        {
            return dal.Getp_TPRtime(p_id);
        }

        /// <summary>
        /// 得到p_id/第二种标准盘更新用
        /// </summary>
        public int Getp_id(DateTime p_TPRtime, string p_one, string p_two)
        {
            return dal.Getp_id(p_TPRtime, p_one, p_two);
        }

        /// <summary>
        /// 得到篮球开奖所需的实体
        /// </summary>
        public TPR2.Model.guess.BaList GetBasketOpen(DateTime p_TPRtime, string p_one, string p_two)
        {
            return dal.GetBasketOpen(p_TPRtime, p_one, p_two);
        }

        /// <summary>
        /// 得到足球开奖所需的实体
        /// </summary>
        public TPR2.Model.guess.BaList GetFootOpen(string p_title, string p_one, string p_two)
        {
            return dal.GetFootOpen(p_title, p_one, p_two);
        }

        /// <summary>
        /// 得到是否开启滚球
        /// </summary>
        public int Getison(int ID)
        {
            return dal.Getison(ID);
        }

        /// <summary>
        /// 得到是否开启滚球
        /// </summary>
        public int Getisonht(int ID)
        {
            return dal.Getisonht(ID);
        }

        /// <summary>
        /// 得到滚球更新时间集合
        /// </summary>
        public string Getonce(int ID)
        {
            return dal.Getonce(ID);
        }

        /// <summary>
        /// 得到比分更新时间集合
        /// </summary>
        public string Getp_temptimes(int ID)
        {
            return dal.Getp_temptimes(ID);
        }

        /// <summary>
        /// 得到最新比分更新时间
        /// </summary>
        public DateTime Getp_temptime(int ID)
        {
            return dal.Getp_temptime(ID);
        }

        /// <summary>
        /// 得到p_temptime_p_id
        /// </summary>
        public TPR2.Model.guess.BaList Getp_temptime_p_id(int ID)
        {
            return dal.Getp_temptime_p_id(ID);
        }

        /// <summary>
        /// 得到即时比分
        /// </summary>
        public TPR2.Model.guess.BaList GetTemp(int ID)
        {
            return dal.GetTemp(ID, 0);
        }
        /// <summary>
        /// 得到即时比分
        /// </summary>
        public TPR2.Model.guess.BaList GetTemp(int ID, int p_basketve)
        {
            return dal.GetTemp(ID, p_basketve);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public TPR2.Model.guess.BaList GetModel(int ID)
        {
            return dal.GetModel(ID, 0);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public TPR2.Model.guess.BaList GetModelByp_id(int p_id)
        {
            return dal.GetModel(p_id, 1);
        }

        /// <summary>
        /// 获取球赛封盘标识 1为系统封盘 2为人工封盘
        /// </summary>
        /// <param name="p_id"></param>
        /// <param name="p_basketve"></param>
        /// <returns></returns>
        public TPR2.Model.guess.BaList Getluck(int p_id, int p_basketve)
        {
            return dal.Getluck(p_id, p_basketve);
        }

        /// <summary>
        /// 根据字段取数据列表
        /// </summary>
        public DataSet GetBaListList(string strField, string strWhere)
        {
            return dal.GetBaListList(strField, strWhere);
        }

        /// <summary>
        /// 取得每页记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <returns>IList</returns>
        public IList<TPR2.Model.guess.BaList> GetBaLists(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
        {
            return dal.GetBaLists(p_pageIndex, p_pageSize, strWhere, strOrder, out p_recordCount);
        }

        /// <summary>
        /// 取得联赛记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <returns>IList</returns>
        public IList<TPR2.Model.guess.BaList> GetBaListLX(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetBaListLX(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }

        /// <summary>
        /// 取得未开奖赛事记录
        /// </summary>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <returns>IList</returns>
        public IList<TPR2.Model.guess.BaList> GetBaListBF(string strWhere, out int p_recordCount)
        {
            return dal.GetBaListBF(strWhere, out p_recordCount);
        }

        #endregion  成员方法
    }
}

