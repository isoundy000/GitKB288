using System;
using System.Data;
using System.Collections.Generic;
using BCW.HB.Model;
namespace BCW.HB.BLL
{
    /// <summary>
    /// ChatMe
    /// </summary>
    public partial class ChatMe
    {
        private readonly BCW.HB.DAL.ChatMe dal = new BCW.HB.DAL.ChatMe();
        public ChatMe()
        { }
        #region  BasicMethod

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
        public bool Exists(int ID,int meid)
        {
            return dal.Exists(ID,meid);
        }
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(BCW.HB.Model.ChatMe model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(BCW.HB.Model.ChatMe model)
        {
            return dal.Update(model);
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(int chatid, int uid)
        {
            return dal.Update(chatid,uid);
        }
        /// 更新ChatTextType
        /// </summary>
        public bool UpdateChatTextType(int id)
        {
            return dal.UpdateChatTextType(id);
        }
        /// <summary>
        /// 更新SpeakTextType
        /// </summary>
        public bool UpdateSpeakType(int id)
        {
            return dal.UpdateSpeakType(id);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(int chatid, int uid,int score)
        {
            return dal.Update(chatid, uid, score);
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update2(int chatid, int uid)
        {
            return dal.Update2(chatid, uid);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int ID)
        {

            return dal.Delete(ID);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int ID, int meid)
        {

            return dal.Delete(ID,meid);
        }
        /// <summary>
        /// 删除同聊天室数据
        /// </summary>
        public bool Delete2(int ChatID)
        {

            return dal.Delete2(ChatID);
        }
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.HB.Model.ChatMe GetModel(int ID)
        {

            return dal.GetModel(ID);
        }
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.HB.Model.ChatMe GetModel(int ID,int uid)
        {

            return dal.GetModel(ID,uid);
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
        public List<BCW.HB.Model.ChatMe> GetModelList(string strWhere)
        {
            DataSet ds = dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<BCW.HB.Model.ChatMe> DataTableToList(DataTable dt)
        {
            List<BCW.HB.Model.ChatMe> modelList = new List<BCW.HB.Model.ChatMe>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                BCW.HB.Model.ChatMe model;
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
        /// 分页获取数据列表
        /// </summary>
        //public DataSet GetList(int PageSize,int PageIndex,string strWhere)
        //{
        //return dal.GetList(PageSize,PageIndex,strWhere);
        //}

        #endregion  BasicMethod
        #region  聊天室相关方法
        /// <summary>
        /// 更新邀请权限 
        /// |0|仅群主可以邀请
        /// |1|仅群主和室主可以邀请
        /// |2|仅群主、室主和见习室主可以邀请
        /// |3|仅群主、室主、见习室主和临管可以邀请
        /// |4|所有成员可以邀请
        /// </summary>
        public bool UpdateInvite(int Invite, int Chatid)
        {
            return dal.UpdateInvite(Invite,Chatid);
        }
        /// <summary>
        /// 得到邀请权限 
        /// |0|仅群主可以邀请
        /// |1|仅群主和室主可以邀请
        /// |2|仅群主、室主和见习室主可以邀请
        /// |3|仅群主、室主、见习室主和临管可以邀请
        /// |4|所有成员可以邀请
        /// </summary>
        public int GetInvite(int ID)
        {
            return dal.GetInvite(ID);
        }
        #endregion
    }
}

