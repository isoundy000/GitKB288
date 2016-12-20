using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
    /// <summary>
    /// 业务逻辑类tb_Validate 的摘要说明。
    /// </summary>
    public class tb_Validate
    {
        private readonly BCW.DAL.tb_Validate dal = new BCW.DAL.tb_Validate();
        public tb_Validate()
        { }
        #region  成员方法
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            return dal.Exists(ID);
        }
        /// <summary>
        /// 是否存在该记录
        /// type 1 注册，2 修改密码 3 修改密保 4修改手机
        ///      5 忘记密码
        /// </summary>
        public bool ExistsPhone(string Phone,int type)
        {
            return dal.ExistsPhone(Phone, type);
        }
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool ExistsPhone(string Phone)
        {
            return dal.ExistsPhone(Phone);
        }
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(BCW.Model.tb_Validate model)
        {
            return dal.Add(model);
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void UpdateFlag(int Flag, int ID)
        {
            dal.UpdateFlag(Flag,ID);
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(BCW.Model.tb_Validate model)
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
        public BCW.Model.tb_Validate Gettb_Validate(int ID)
        {

            return dal.Gettb_Validate(ID);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.Model.tb_Validate Gettb_Validate(string Phone, int type)
        {
            
            return dal.Gettb_Validate(Phone,  type);
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
        /// <returns>IList tb_Validate</returns>
        public IList<BCW.Model.tb_Validate> Gettb_Validates(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.Gettb_Validates(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }

        #endregion  成员方法
    }
}

