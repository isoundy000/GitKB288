using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.farm.Model;
namespace BCW.farm.BLL
{
    /// <summary>
    /// 业务逻辑类NC_daoju 的摘要说明。
    /// </summary>
    public class NC_daoju
    {
        private readonly BCW.farm.DAL.NC_daoju dal = new BCW.farm.DAL.NC_daoju();
        public NC_daoju()
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
        /// 增加一条数据
        /// </summary>
        public int Add(BCW.farm.Model.NC_daoju model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(BCW.farm.Model.NC_daoju model)
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
        public BCW.farm.Model.NC_daoju GetNC_daoju(int ID)
        {

            return dal.GetNC_daoju(ID);
        }

        /// <summary>
        /// 根据字段取数据列表
        /// </summary>
        public DataSet GetList(string strField, string strWhere)
        {
            return dal.GetList(strField, strWhere);
        }

        //=======================================
        /// <summary>
        /// 是否存在该记录
        /// //购买判断-是否为宝箱道具记录type=10
        /// </summary>
        public bool Exists(int ID)
        {
            return dal.Exists(ID);
        }
        /// <summary>
        /// 是否存在该记录
        /// //后台增加判断改id是否存在
        /// </summary>
        public bool Exists2(int ID)
        {
            return dal.Exists2(ID);
        }
        /// <summary>
        /// me_是否存在该道具名称
        /// </summary>
        public bool Exists_djmc(string name)
        {
            return dal.Exists_djmc(name);
        }
        /// <summary>
        ///  me_根据字段修改数据列表
        /// </summary>
        public DataSet update_daoju(string strField, string strWhere)
        {
            return dal.update_daoju(strField, strWhere);
        }
        /// <summary>
        /// me_得到道具图片路径
        /// </summary>
        public string Get_picture(int id)
        {
            return dal.Get_picture(id);
        }
        //=======================================


        /// <summary>
        /// 取得每页记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList NC_daoju</returns>
        public IList<BCW.farm.Model.NC_daoju> GetNC_daojus(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetNC_daojus(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }

        #endregion  成员方法
    }
}

