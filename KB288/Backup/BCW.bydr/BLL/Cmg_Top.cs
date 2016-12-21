using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.bydr.Model;
namespace BCW.bydr.BLL
{
    /// <summary>
    /// 业务逻辑类Cmg_Top 的摘要说明。
    /// </summary>
    public class Cmg_Top
    {
        private readonly BCW.bydr.DAL.Cmg_Top dal = new BCW.bydr.DAL.Cmg_Top();
        public Cmg_Top()
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

        public bool ExistsusID(int meid)
        {
            return dal.ExistsusID(meid);
        }

        /// <summary>
        /// 是否存在该Bid
        /// </summary>
        public bool ExistsBid(int id, int usid)
        {
            return dal.ExistsBid(id, usid);
        }
        /// <summary>
        /// 是否存在玩家未完记录
        /// </summary>
        public bool ExistsusID1(int usid)
        {
            return dal.ExistsusID1(usid);

        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(BCW.bydr.Model.Cmg_Top model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(BCW.bydr.Model.Cmg_Top model)
        {
            dal.Update(model);
        }
        /// <summary>
        /// 更新jid
        /// </summary>

        public void UpdateJid(int Bid, int jID)
        {
            dal.UpdateJid(Bid, jID);
        }
        /// <summary>
        /// 更新Bid控制兑奖字段
        /// </summary>

        public void UpdateBid(int Bid, int id, int usid)
        {
            dal.UpdateBid(Bid, id, usid);
        }
        /// <summary>
        /// 更新重复控制兑奖字段
        /// </summary>

        public void UpdateExpiry(int Expiry, int id)
        {
            dal.UpdateExpiry(Expiry, id);
        }
        /// <summary>
        /// 更新总收集币
        /// </summary>
        public void UpdateAllcolletGold(int id, long AllcolletGold)
        {
            dal.UpdateAllcolletGold(id, AllcolletGold);
        }
        /// <summary>
        /// 更新ColletGold
        /// </summary>
        public void UpdateColletGold(int id, long ColletGold)
        {
            dal.UpdateColletGold(id, ColletGold);
        }
        /// <summary>
        /// 更新Updateranddaoju
        /// </summary>
        public void Updateranddaoju(string randdaoju, int id)
        {
            dal.Updateranddaoju(randdaoju, id);
        }
        /// <summary>
        /// 更新Updaterandten
        /// </summary>
        public void Updaterandten(string randten, int id)
        {
            dal.Updaterandten(randten, id);
        }
        /// <summary>
        /// 更新UpdaterandyuID
        /// </summary>
        public void UpdaterandyuID(string randyuID, int id)
        {
            dal.UpdaterandyuID(randyuID, id);
        }
        /// <summary>
        /// 更新玩家游戏次数
        /// </summary>
        public void UpdateDcolletGold(int id, long DcolletGold)
        {
            dal.UpdateDcolletGold(id, DcolletGold);
        }
        /// <summary>
        /// 更新防刷字段
        /// </summary>
        public void UpdateYcolletGold(int id, long YcolletGold)
        {
            dal.UpdateYcolletGold(id, YcolletGold);
        }
        /// <summary>
        /// 更新每月收集币
        /// </summary>
        public void UpdateMcolletGold(int Bid, long McolletGold)
        {
            dal.UpdateMcolletGold(Bid, McolletGold);
        }
        /// <summary>
        /// 更新防刷时间
        /// </summary>
        public void updatetime(int id, DateTime updatetime)
        {
            dal.updatetime(id, updatetime);
        }
        /// <summary>
        /// 更新时间
        /// </summary>
        public void updatetime1(int id, DateTime updatetime)
        {
            dal.updatetime1(id, updatetime);
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
        public BCW.bydr.Model.Cmg_Top GetCmg_Top(int ID)
        {

            return dal.GetCmg_Top(ID);
        }
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.bydr.Model.Cmg_Top GetCmg_Top(int ID, int usid)
        {

            return dal.GetCmg_Top(ID, usid);
        }
        /// <summary>
        /// 得到一个今日收集币
        /// </summary>
        public long GetCmg_AllcolletGold(int usID, string time)
        {
            return dal.GetCmg_AllcolletGold(usID, time);
        }
        /// <summary>
        /// 得到今日收集币
        /// </summary>
        public long GetCmg_AllcolletGoldday(string time1, string time2)
        {
            return dal.GetCmg_AllcolletGoldday(time1, time2);
        }
        /// <summary>
        /// 得到id玩的次数
        /// </summary>
        public int GetCmgcount(int usID)
        {
            return dal.GetCmgcount(usID);
        }
        /// <summary>
        /// 得到usid今天玩的次数_邵广林20160813
        /// </summary>
        public int GetCmgcount1(int usID)
        {
            return dal.GetCmgcount1(usID);
        }
        /// <summary>
        /// 得到一个本月收集币
        /// </summary>
        public long GetCmg_AllcolletGoldmonth(int usID, string time1, string time2)
        {
            return dal.GetCmg_AllcolletGoldmonth(usID, time1, time2);
        }
        /// <summary>
        /// 得到本月收集币
        /// </summary>
        public long GetCmg_AllcolletGoldmonth1(string time1, string time2)
        {
            return dal.GetCmg_AllcolletGoldmonth1(time1, time2);
        }

        /// <summary>
        /// 得到总收集币
        /// </summary>
        public long GetCmg_AllcolletGold1(int usID)
        {
            return dal.GetCmg_AllcolletGold1(usID);
        }
        /// <summary>
        /// 得到总收集币
        /// </summary>
        public long GetCmg_AllcolletGold2()
        {
            return dal.GetCmg_AllcolletGold2();
        }
        ///// <summary>
        ///// 得到最大收集币
        ///// </summary>
        //public BCW.bydr.Model.Cmg_Top GetCmgAllcolletGold(int ID)
        //{
        //    return dal.GetCmgAllcolletGold(ID);
        //}
        /// <summary>
        /// 根据字段取数据列表
        /// </summary>
        public DataSet GetList(string strField, string strWhere)
        {
            return dal.GetList(strField, strWhere);
        }

        /// <summary>
        /// 分页条件获取排行榜数据列表
        /// </summary>
        public DataSet GetListByPage(string s1, string s2)
        {
            return dal.GetListByPage(s1, s2);
        }
        /// <summary>
        /// 取得每页记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList Cmg_Top</returns>
        public IList<BCW.bydr.Model.Cmg_Top> GetCmg_Tops(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetCmg_Tops(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }
        /// <summary>
        /// 取得每页记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList Cmg_Top</returns>
        public IList<BCW.bydr.Model.Cmg_Top> GetCmg_Tops2(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetCmg_Tops2(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }
        /// <summary>
        /// 取得相同usid最大收集记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList Cmg_Top</returns>
        public IList<BCW.bydr.Model.Cmg_Top> GetCmg_Tops1(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
        {
            return dal.GetCmg_Tops1(p_pageIndex, p_pageSize, strWhere, strOrder, out p_recordCount);
        }

        #endregion  成员方法



    }
}

