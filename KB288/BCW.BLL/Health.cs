using System;
using System.Data;
using System.Collections.Generic;
using BCW.Model;
using BCW.Common;
namespace BCW.BLL
{
    /// <summary>
    /// 业务逻辑类Health 的摘要说明。
    /// </summary>
    public class Health
    {
        private readonly BCW.DAL.Health dal = new BCW.DAL.Health();
        public Health()
        { }
        #region  成员方法

        /// <summary>
        /// 删除数据
        /// </summary>
        public void Delete()
        {

            dal.Delete();
        }
        /// <summary>
        /// 取得每页记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <returns>IList Topics</returns>
        public IList<BCW.Model.Health> GetHealths(int p_pageIndex, int p_pageSize, out int p_recordCount)
        {
            return dal.GetHealths(p_pageIndex, p_pageSize, out p_recordCount);
        }

        #endregion  成员方法
    }
}
