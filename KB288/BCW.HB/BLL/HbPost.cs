using System;
using System.Data;
using System.Collections.Generic;
using BCW.HB.Model;
namespace BCW.HB.BLL
{
    /// <summary>
    /// HbPost
    /// 
    /// 2016-08-23 增加点值抽奖发红包接口 蒙宗将
    /// </summary>
    public partial class HbPost
    {
        private readonly BCW.HB.DAL.HbPost dal = new BCW.HB.DAL.HbPost();
        public HbPost()
        { }
        #region  BasicMethod
        /// <summary>
        /// 是否存在该ID记录
        /// </summary>
        public bool Exists(int ID)
        {
            return dal.Exists(ID);
        }
        /// <summary>
        /// 是否存在符合该条件的记录
        /// </summary>
        public bool Exists(int UserID, int State)
        {
            return dal.Exists2(UserID,State);
        }
        /// <summary>
        /// 记录条数
        /// </summary>
        public int GetCount()
        {
            return dal.GetCount();
        }
        /// <summary>
        /// 是否存在符合该条件的记录
        /// </summary>
        public bool Exists2(int ID, int State)
        {
            return dal.Exists3(ID, State);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(BCW.HB.Model.HbPost model)
        {
            int id= dal.Add(model);

            try
            {
                int usid = model.UserID;
                string username = new BCW.BLL.User().GetUsName(usid);
                string Notes = "发红包者";
                new BCW.Draw.draw().AddjfbyTz(usid, username, Notes);//点值抽奖
            }
            catch { }

            return id;
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(BCW.HB.Model.HbPost model)
        {
            return dal.Update(model);
        }
        /// <summary>
        /// 更新字段GetIDList
        /// </summary>
        public void UpdateGetIDList(int ID, string GetIDList)
        {
            dal.UpdateGetIDList(ID, GetIDList);
        }
        /// <summary>
        /// 更新字段State
        /// </summary>
        public void UpdateState(int ID, int State)
        {
            dal.UpdateState(ID, State);
        }
        /// <summary>
        /// 红包玩花样更新
        /// </summary>
        public void UpdateStyle(int ID, int Style, string Keys)
        {
            dal.UpdateStyle(ID,Style,Keys);
        }
        /// <summary>
        /// 更新Chat表的IShb字段
        /// </summary>
        public void UpdateChatIsHb(int ID, int IsHb)
        {
            dal.UpdateChatIsHb(ID,IsHb);
        }
        /// <summary>
        /// 更新Chat表的ChatCmoney字段
        /// </summary>
        public void UpdateChatCmoney(int ID, string ChatCmoney)
        {
            dal.UpdateChatCmoney(ID, ChatCmoney);
        }
        /// <summary>
        /// 重置抢币钱数
        /// </summary>
        public void UpdateCb(int ID, string ChatCmoney, DateTime ChatCTime)
        {
            dal.UpdateCb(ID, ChatCmoney, ChatCTime);
        }
        /// <summary>
        /// 获得Chat数据列表
        /// </summary>
        public DataSet GetChatList(int ID)
        {
            return dal.GetChatList(ID);
        }
        /// <summary>
        /// 得到聊天室是否私人
        /// </summary>
        public int GetChatGS(int ID)
        {
            return dal.GetChatGS(ID);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int ID)
        {
            return dal.Delete(ID);
        }
        /// <summary>
        ///批量删除数据
        /// </summary>
        public bool DeleteList(string IDlist)
        {
            return dal.DeleteList(IDlist);
        }
        /// <summary>
        /// 获取手气发送总钱数
        /// </summary>
        public int PostMoney(string strWhere)
        {
            return dal.PostMoney(strWhere);
        }
        /// <summary>
        /// 获取普通发送总钱数
        /// </summary>
        public int PostMoney2(string strWhere)
        {
            return dal.PostMoney2(strWhere);
        }
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.HB.Model.HbPost GetModel(int ID)
        {

            return dal.GetModel(ID);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            return dal.GetList(strWhere);
        }
        /// <summary>
        /// 获得前几行数据
        /// </summary>
        public DataSet GetList(int Top, string strWhere, string filedOrder)
        {
            return dal.GetList(Top, strWhere, filedOrder);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<BCW.HB.Model.HbPost> GetModelList(string strWhere)
        {
            DataSet ds = dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<BCW.HB.Model.HbPost> DataTableToList(DataTable dt)
        {
            List<BCW.HB.Model.HbPost> modelList = new List<BCW.HB.Model.HbPost>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                BCW.HB.Model.HbPost model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = dal.DataRowToModel(dt.Rows[n]);
                    if (model != null)
                    {
                        modelList.Add(model);
                    }
                }
            }
            return modelList;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetAllList()
        {
            return GetList("");
        }

        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        public int GetRecordCount(string strWhere)
        {
            return dal.GetRecordCount(strWhere);
        }
        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        public DataSet GetListByPage(string strWhere, string orderby, int startIndex, int endIndex)
        {
            return dal.GetListByPage(strWhere, orderby, startIndex, endIndex);
        }
        /// <summary>
        /// 分页条件获取次数排行榜数据列表
        /// </summary>
        public DataSet GetListByPage1(int startIndex, int endIndex)
        {
            return dal.GetListByPage1(startIndex,endIndex);
        }
        /// <summary>
        /// 分页条件获取钱数排行榜数据列表
        /// </summary>
        public DataSet GetListByPage2(int startIndex, int endIndex)
        {
            return dal.GetListByPage2(startIndex, endIndex);
        }
        /// <summary>
        /// 取得每页记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList ChatText</returns>
        public IList<BCW.HB.Model.HbPost> GetListByPage(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetListByPage(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }
        #endregion  BasicMethod

    }
}


