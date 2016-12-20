using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.farm.Model;
namespace BCW.farm.BLL
{
    /// <summary>
    /// 业务逻辑类NC_messagelog 的摘要说明。
    /// </summary>
    public class NC_messagelog
    {
        private readonly BCW.farm.DAL.NC_messagelog dal = new BCW.farm.DAL.NC_messagelog();
        public NC_messagelog()
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
        public int Add(BCW.farm.Model.NC_messagelog model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(BCW.farm.Model.NC_messagelog model)
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
        public BCW.farm.Model.NC_messagelog GetNC_messagelog(int ID)
        {

            return dal.GetNC_messagelog(ID);
        }

        /// <summary>
        /// 根据字段取数据列表
        /// </summary>
        public DataSet GetList(string strField, string strWhere)
        {
            return dal.GetList(strField, strWhere);
        }

        //================================
        /// <summary>
        /// me_增加消息
        /// </summary>
        public void addmessage(int ID, string UsName, string AcText, int BbTag)
        {
            //增加记录
            BCW.farm.Model.NC_messagelog model = new BCW.farm.Model.NC_messagelog();
            model.type = BbTag;
            model.UsId = ID;
            model.UsName = UsName;
            model.AcText = AcText;
            model.AddTime = DateTime.Now;
            new BCW.farm.BLL.NC_messagelog().Add(model);
        }
        //================================


        /// <summary>
        /// 取得每页记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList NC_messagelog</returns>
        public IList<BCW.farm.Model.NC_messagelog> GetNC_messagelogs(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetNC_messagelogs(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }

        #endregion  成员方法
    }
}

