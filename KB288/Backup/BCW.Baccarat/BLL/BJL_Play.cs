using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Baccarat.Model;
namespace BCW.Baccarat.BLL
{
    /// <summary>
    /// 业务逻辑类BJL_Play 的摘要说明。
    /// </summary>
    public class BJL_Play
    {
        private readonly BCW.Baccarat.DAL.BJL_Play dal = new BCW.Baccarat.DAL.BJL_Play();
        public BJL_Play()
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
        /// 增加一条数据
        /// </summary>
        public int Add(BCW.Baccarat.Model.BJL_Play model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(BCW.Baccarat.Model.BJL_Play model)
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
        /// 删除一组数据
        /// </summary>
        public void Delete(string strWhere)
        {
            dal.Delete(strWhere);
        }
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.Baccarat.Model.BJL_Play GetBJL_Play(int ID)
        {
            return dal.GetBJL_Play(ID);
        }
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.Baccarat.Model.BJL_Play GetBJL_Play3(int ID)
        {
            return dal.GetBJL_Play3(ID);
        }
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.Baccarat.Model.BJL_Play GetBJL_Play2(int ID)
        {

            return dal.GetBJL_Play2(ID);
        }

        /// <summary>
        /// 根据字段取数据列表
        /// </summary>
        public DataSet GetList(string strField, string strWhere)
        {
            return dal.GetList(strField, strWhere);
        }
        /// <summary>
        /// 根据字段取数据列表
        /// </summary>
        public DataSet GetList2(int strField, string strWhere)
        {
            return dal.GetList2(strField, strWhere);
        }
        //-----------------------------------------------------
        /// <summary>
        /// me_是否存在兑奖记录
        /// </summary>
        public bool ExistsState(int ID, int UsID)
        {
            return dal.ExistsState(ID, UsID);
        }
        /// <summary>
        /// me_后台分页条件获取排行榜数据列表
        /// </summary>
        public DataSet GetListByPage2(int startIndex, int endIndex, string s1, string s2)
        {
            return dal.GetListByPage2(startIndex, endIndex, s1, s2);
        }
        /// <summary>
        /// me_是否存在该房间
        /// </summary>
        public bool Exists(int ID)
        {
            return dal.Exists(ID);
        }
        /// <summary>
        /// me_是否存在有下注的房间
        /// </summary>
        public bool Exists_xz(int room, int table)
        {
            return dal.Exists_xz(room, table);
        }
        /// <summary>
        /// me_是否存在该房间
        /// </summary>
        public bool Exists_id(int ID)
        {
            return dal.Exists_id(ID);
        }
        /// <summary>
        /// me_是否存在玩家在玩
        /// </summary>
        public bool Exists_wj(int ID)
        {
            return dal.Exists_wj(ID);
        }
        /// <summary>
        /// me_是否存在该房间
        /// </summary>
        public bool Exists()
        {
            return dal.Exists();
        }
        /// <summary>
        /// me_是否存在该房间该局未开奖
        /// </summary>
        public bool Exists(int ID, int roomtable)
        {
            return dal.Exists(ID, roomtable);
        }
        /// <summary>
        /// me_得到一个对象实体
        /// </summary>
        public BCW.Baccarat.Model.BJL_Play GetBJL_Play(int ID, int usid)
        {

            return dal.GetBJL_Play(ID, usid);
        }
        /// <summary>
        ///me_得到特定房间ID和桌面table的最旧的下注时间
        /// </summary>
        public DateTime GetMinBetTime(int RoomID, int Table)
        {
            return dal.GetMinBetTime(RoomID, Table);
        }
        /// <summary>
        /// me_计算投注总币值
        /// </summary>
        public long GetPrice(int RoomID, int Table)
        {
            return dal.GetPrice(RoomID, Table);
        }
        /// <summary>
        /// me_计算中奖总币值
        /// </summary>
        public long Getmoney(int RoomID, int Table)
        {
            return dal.Getmoney(RoomID, Table);
        }
        /// <summary>
        /// me_计算手续费总币值
        /// </summary>
        public long Getsxf(int RoomID, int Table)
        {
            return dal.Getsxf(RoomID, Table);
        }
        /// <summary>
        ///  me_根据字段修改数据列表
        /// </summary>
        public DataSet update_zd(string strField, string strWhere)
        {
            return dal.update_zd(strField, strWhere);
        }
        //------------------------------------------------------

        /// <summary>
        /// 取得每页记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList BJL_Play</returns>
        public IList<BCW.Baccarat.Model.BJL_Play> GetBJL_Plays(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
        {
            return dal.GetBJL_Plays(p_pageIndex, p_pageSize, strWhere, strOrder, out p_recordCount);
        }

        #endregion  成员方法
    }
}

