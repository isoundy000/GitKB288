using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL.Game
{
    /// <summary>
    /// 业务逻辑类ktv789 的摘要说明。
    /// </summary>
    public class ktv789
    {
        private readonly BCW.DAL.Game.ktv789 dal = new BCW.DAL.Game.ktv789();
        public ktv789()
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
        /// 是否存在该记录
        /// </summary>
        public int Getktv789Id(int userId)
        {
            return dal.Getktv789Id(userId);
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID, int ptype)
        {
            return dal.Exists(ID, ptype);
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID, int userId, int ptype)
        {
            return dal.Exists(ID, userId, ptype);
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public int GetCount(int Types)
        {
            return dal.GetCount(Types);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(BCW.Model.Game.ktv789 model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(BCW.Model.Game.ktv789 model)
        {
            dal.Update(model);
        }

        /// <summary>
        /// 坐下位置1
        /// </summary>
        public void UpdateOne(BCW.Model.Game.ktv789 model)
        {
            dal.UpdateOne(model);
        }

        /// <summary>
        /// 坐下位置2
        /// </summary>
        public void UpdateTwo(BCW.Model.Game.ktv789 model)
        {
            dal.UpdateTwo(model);
        }

        /// <summary>
        /// 坐下位置3
        /// </summary>
        public void UpdateThr(BCW.Model.Game.ktv789 model)
        {
            dal.UpdateThr(model);
        }

        /// <summary>
        /// 出手1
        /// </summary>
        public void UpdateOneShot(BCW.Model.Game.ktv789 model)
        {
            dal.UpdateOneShot(model);
        }

        /// <summary>
        /// 出手2
        /// </summary>
        public void UpdateTwoShot(BCW.Model.Game.ktv789 model)
        {
            dal.UpdateTwoShot(model);
        }

        /// <summary>
        /// 出手3
        /// </summary>
        public void UpdateThrShot(BCW.Model.Game.ktv789 model)
        {
            dal.UpdateThrShot(model);
        }

        /// <summary>
        /// 退出游戏1
        /// </summary>
        public void UpdateOneExit(int ID)
        {
            dal.UpdateOneExit(ID);
        }

        /// <summary>
        /// 退出游戏2
        /// </summary>
        public void UpdateTwoExit(int ID)
        {
            dal.UpdateTwoExit(ID);
        }

        /// <summary>
        /// 退出游戏3
        /// </summary>
        public void UpdateThrExit(int ID)
        {
            dal.UpdateThrExit(ID);
        }

        /// <summary>
        /// 更新轮流PK1
        /// </summary>
        public void UpdateLLone(int ID)
        {
            dal.UpdateLLone(ID);
        }

        /// <summary>
        /// 更新轮流PK2
        /// </summary>
        public void UpdateLLtwo(int ID)
        {
            dal.UpdateLLtwo(ID);
        }

        /// <summary>
        /// 更新轮流PK3
        /// </summary>
        public void UpdateLLthr(int ID)
        {
            dal.UpdateLLthr(ID);
        }

        /// <summary>
        /// 重置桌子
        /// </summary>
        public void UpdateShot(int ID)
        {
            dal.UpdateShot(ID);
        }

        /// <summary>
        /// 清空桌面
        /// </summary>
        public void UpdateClear(int ID)
        {
            dal.UpdateClear(ID);
        }

        /// <summary>
        /// 更新下注额
        /// </summary>
        public void UpdatePayCent(int ID, int PayCent)
        {
            dal.UpdatePayCent(ID, PayCent);
        }

        /// <summary>
        /// 更新下次出手桌子
        /// </summary>
        public void UpdateNextShot(int ID, int NextShot)
        {
            dal.UpdateNextShot(ID, NextShot);
        }

        /// <summary>
        /// 更新超时时间
        /// </summary>
        public void UpdateTime(int ID)
        {
            dal.UpdateTime(ID);
        }

        /// <summary>
        /// 更新桌子在线
        /// </summary>
        public void UpdateLines(int ID, string Lines)
        {
            dal.UpdateLines(ID, Lines);
        }

        /// <summary>
        /// 更新桌子在线人数
        /// </summary>
        public void UpdateOnline(int ID, int Online)
        {
            dal.UpdateOnline(ID, Online);
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
        public BCW.Model.Game.ktv789 Getktv789(int ID)
        {

            return dal.Getktv789(ID);
        }

        /// <summary>
        /// 得到房间StName
        /// </summary>
        public string GetStName(int ID)
        {

            return dal.GetStName(ID);
        }

        /// <summary>
        /// 得到在线Lines
        /// </summary>
        public string GetLines(int ID)
        {

            return dal.GetLines(ID);
        }

        /// <summary>
        /// 得到Types
        /// </summary>
        public int GetTypes(int ID)
        {

            return dal.GetTypes(ID);
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
        /// <returns>IList ktv789</returns>
        public IList<BCW.Model.Game.ktv789> Getktv789s(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.Getktv789s(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }

        #endregion  成员方法
    }
}

