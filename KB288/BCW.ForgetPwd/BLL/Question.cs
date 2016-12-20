using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
    /// <summary>
    /// 业务逻辑类tb_Question 的摘要说明。
    /// </summary>
    public class tb_Question
    {
        private readonly BCW.DAL.tb_Question dal = new BCW.DAL.tb_Question();
        public tb_Question()
        { }
        #region  成员方法
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(string account)
        {
            return dal.Exists(account);
        }
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            return dal.Exists(ID);
        }
         /// <summary>
        /// 更新密码保护答案为手机后六位
        /// </summary>
        public void UpdateAnswer(string Answer, string Mobile)
        {
            dal.UpdateAnswer(Answer, Mobile);
        }
        /// <summary>
        /// 更新验证码
        /// </summary>
        public void UpdateCode(string code, string Mobile)
        {
            dal.UpdateCode(code,Mobile);
        }
        /// <summary>
        /// 更新手机号
        /// </summary>
        public void UpdateMobile(int ID, string Mobile)
        {
            dal.UpdateMobile(ID, Mobile);
        }
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public void Add(BCW.Model.tb_Question model)
        {
            dal.Add(model);
        }
        /// <summary>
        /// 更新修改密码的次数
        /// </summary>
        public void UpdateChangeCount(int changecount, int account)
        {
            dal.UpdateChangeCount(changecount, account);
        }
        /// <summary>
        /// 更新上一次修改密码的天数
        /// </summary>
        public void UpdateLastChange(int lastchange, string Mobile)
        {
            dal.UpdateLastChange(lastchange, Mobile);
        }
        /// <summary>
        /// 根据ID更新一条数据
        /// </summary>
        public void Update(BCW.Model.tb_Question model)
        {
            dal.Update(model);
        }
        /// <summary>
        /// 根据账号得到一天修改密码的次数
        /// </summary>
        public int GetChangeCount(int account)
        {
            return dal.GetChangeCount(account);
        }
         /// <summary>
        /// 根据号码得到验证码
        /// </summary>
        public string GetCode(string Mypmobile)
        {
           return  dal.GetCode(Mypmobile);
        }
        /// <summary>
        /// 根据号码得到你上一次修改密码的天数（在一年中的天数）
        /// </summary>
        public int GetLastChange(int account)
        {
            return dal.GetLastChange(account);
        }
        /// <summary>
        /// 根据号码得到你的问题
        /// </summary>
        public string GetQuestion(int account)
        {
            return dal.GetQuestion(account);
        }
        /// <summary>
        /// 根据号码得到你的答案
        /// </summary>
        public string GetAnswer(int account)
        {
            return dal.GetAnswer(account);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(string Mobile)
        {

            dal.Delete(Mobile);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.Model.tb_Question Gettb_Question(string Mobile)
        {

            return dal.Gettb_Question(Mobile);
        }

        /// <summary>
        /// 根据字段取数据列表
        /// </summary>
        public DataSet GetList(string strField, string strWhere)
        {
            return dal.GetList(strField, strWhere);
        }
        /// <summary>
        /// 取得固定列表记录
        /// </summary>
        /// <param name="SizeNum">列表记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList Brag</returns>
        public IList<BCW.Model.tb_Question> GetBrags(int SizeNum, string strWhere)
        {
            return dal.GetBrags(SizeNum, strWhere);
        }
        /// <summary>
        /// 取得每页记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList tb_Question</returns>
        public IList<BCW.Model.tb_Question> Gettb_Questions(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.Gettb_Questions(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }

        #endregion  成员方法
    }
}

