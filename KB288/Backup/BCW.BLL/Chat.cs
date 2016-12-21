using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
    /// <summary>
    /// 业务逻辑类Chat 的摘要说明。
    /// </summary>
    public class Chat
    {
        private readonly BCW.DAL.Chat dal = new BCW.DAL.Chat();
        public Chat()
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
        public bool Exists2(int ID)
        {
            return dal.Exists2(ID);
        }
                
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists3(int UsID)
        {
            return dal.Exists3(UsID);
        }
        /// <summary>
        /// 存在几条记录
        /// 2016/3/1 戴少宇
        /// </summary>
        public int Exists4(int UsID)
        {
            return dal.Exists4(UsID);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(BCW.Model.Chat model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(BCW.Model.Chat model)
        {
            dal.Update(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void UpdateBasic(BCW.Model.Chat model)
        {
            dal.UpdateBasic(model);
        }
                
        /// <summary>
        /// 更新抢币设置
        /// </summary>
        public void UpdateCb(BCW.Model.Chat model)
        {
            dal.UpdateCb(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void UpdateAdmin(BCW.Model.Chat model)
        {
            dal.UpdateAdmin(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void UpdateAdmin2(BCW.Model.Chat model)
        {
            dal.UpdateAdmin2(model);
        }

        /// <summary>
        /// 更新基金数目
        /// </summary>
        public void UpdateChatCent(int ID, long ChatCent)
        {
            dal.UpdateChatCent(ID, ChatCent);
        }

        /// <summary>
        /// 更新基金密码
        /// </summary>
        public void UpdateCentPwd(int ID, string CentPwd)
        {
            dal.UpdateCentPwd(ID, CentPwd);
        }

        /// <summary>
        /// 更新在线人数
        /// </summary>
        public void UpdateLine(int ID, int Line)
        {
            dal.UpdateLine(ID, Line);
        }

        /// <summary>
        /// 在线人数减1
        /// </summary>
        public void UpdateLine(int ID)
        {
            dal.UpdateLine(ID);
        }
                
        /// <summary>
        /// 更新聊室密码
        /// </summary>
        public void UpdateChatPwd(int ID, string ChatPwd)
        {
            dal.UpdateChatPwd(ID, ChatPwd);
        }
                
        /// <summary>
        /// 更新使用密码进入的ID
        /// </summary>
        public void UpdatePwdID(int ID, string PwdID)
        {
            dal.UpdatePwdID(ID, PwdID);
        }
        
        /// <summary>
        /// 更新过期时间(续费)
        /// </summary>
        public void UpdateExitTime(int ID, DateTime ExTime)
        {
            dal.UpdateExitTime(ID, ExTime);
        }
                
        /// <summary>
        /// 更新抢币结束时间
        /// </summary>
        public void UpdateChatCTime(int ID, DateTime ChatCTime)
        {
            dal.UpdateChatCTime(ID, ChatCTime);
        }
               
        /// <summary>
        /// 更新抢币购买ID
        /// </summary>
        public void UpdateChatCId(int ID, string ChatCId)
        {
            dal.UpdateChatCId(ID, ChatCId);
        }
                
        /// <summary>
        /// 重置抢币
        /// </summary>
        public void UpdateCb(int ID, string ChatCId, DateTime ChatCTime)
        {
            dal.UpdateCb(ID, ChatCId, ChatCTime);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int ID)
        {

            dal.Delete(ID);
        }

        /// <summary>
        /// 得到聊天室密码
        /// </summary>
        public string GetChatPwd(int ID)
        {
            return dal.GetChatPwd(ID);
        }
                
        /// <summary>
        /// 得到聊天室名称
        /// </summary>
        public string GetChatName(int ID)
        {
            return dal.GetChatName(ID);
        }
        /// <summary>
        /// 得到聊天室类型
        /// </summary>
        public int GetChatType(int ID)
        {
            return dal.GetChatType(ID);
        }
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.Model.Chat GetChat(int ID)
        {

            return dal.GetChat(ID);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.Model.Chat GetChatBasic(int ID)
        {
            return dal.GetChatBasic(ID);
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
        /// <param name="strOrder">排序条件</param>
        /// <returns>IList Chat</returns>
        public IList<BCW.Model.Chat> GetChats(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
        {
            return dal.GetChats(p_pageIndex, p_pageSize, strWhere, strOrder, out p_recordCount);
        }

        #endregion  成员方法
    }
}

