using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Baccarat.Model;
namespace BCW.Baccarat.BLL
{
    /// <summary>
    /// 业务逻辑类BJL_Room 的摘要说明。
    /// </summary>
    public class BJL_Room
    {
        private readonly BCW.Baccarat.DAL.BJL_Room dal = new BCW.Baccarat.DAL.BJL_Room();
        public BJL_Room()
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
        public int Add(BCW.Baccarat.Model.BJL_Room model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(BCW.Baccarat.Model.BJL_Room model)
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
        public BCW.Baccarat.Model.BJL_Room GetBJL_Room(int ID)
        {
            return dal.GetBJL_Room(ID);
        }

        /// <summary>
        /// 根据字段取数据列表
        /// </summary>
        public DataSet GetList(string strField, string strWhere)
        {
            return dal.GetList(strField, strWhere);
        }

        //-------------------------------------------------
        public DataSet GetList2(string strField, string strWhere)
        {
            return dal.GetList2(strField, strWhere);
        }
        /// <summary>
        /// me_初始化某数据表
        /// </summary>
        /// <param name="TableName">数据表名称</param>
        public void ClearTable(string TableName)
        {
            dal.ClearTable(TableName);
        }
        /// <summary>
        /// 计算手续费
        /// </summary>
        public long GetPrice(string ziduan, string strWhere)
        {
            return dal.GetPrice(ziduan, strWhere);
        }
        /// <summary>
        ///  me_根据字段修改数据列表
        /// </summary>
        public DataSet update_zd(string strField, string strWhere)
        {
            return dal.update_zd(strField, strWhere);
        }
        /// <summary>
        /// me_计算某房间的彩池
        /// </summary>
        public long Getcaichi(int RoomID)
        {
            return dal.Getcaichi(RoomID);
        }
        /// <summary>
        /// me_得到开庄数
        /// </summary>
        public int Get_kz_num(int meid, int ID)
        {
            return dal.Get_kz_num(meid, ID);
        }
        /// <summary>
        /// me_得到一个对象实体
        /// </summary>
        public BCW.Baccarat.Model.BJL_Room GetBJL_Room(int ID, int meid)
        {

            return dal.GetBJL_Room(ID, meid);
        }
        //-------------------------------------------------

        /// <summary>
        /// 取得每页记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList BJL_Room</returns>
        public IList<BCW.Baccarat.Model.BJL_Room> GetBJL_Rooms(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
        {
            return dal.GetBJL_Rooms(p_pageIndex, p_pageSize, strWhere, strOrder, out p_recordCount);
        }

        #endregion  成员方法
    }
}

