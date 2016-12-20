using System;
using System.Data;
using System.Collections.Generic;
using BCW.HB.Model;

namespace BCW.HB.BLL
{
    /// <summary>
    /// Shared
    /// </summary>
    public partial class Shared
    {
        private readonly BCW.HB.DAL.Shared dal = new BCW.HB.DAL.Shared();
        public Shared()
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
        public bool Exists(int UserID)
        {
            return dal.Exists(UserID);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(BCW.HB.Model.Shared model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(BCW.HB.Model.Shared model)
        {
            return dal.Update(model);
        }
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.HB.Model.Shared GetModel(int UserID)
        {

            return dal.GetModel(UserID);
        }
        #endregion  BasicMethod
    }
}
