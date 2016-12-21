using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.farm.Model;
namespace BCW.farm.BLL
{
    /// <summary>
    /// 业务逻辑类NC_shop 的摘要说明。
    /// </summary>
    public class NC_shop
    {
        private readonly BCW.farm.DAL.NC_shop dal = new BCW.farm.DAL.NC_shop();
        public NC_shop()
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
        public int Add(BCW.farm.Model.NC_shop model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(BCW.farm.Model.NC_shop model)
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
        public BCW.farm.Model.NC_shop GetNC_shop(int ID)
        {

            return dal.GetNC_shop(ID);
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
        /// me_随机得到一个对象实体
        /// </summary>
        public BCW.farm.Model.NC_shop Getsd_suiji(int ID)
        {
            return dal.Getsd_suiji(ID);
        }
        /// <summary>
        /// me_根据usid查询金钱
        /// </summary>
        public long get_usergoid(int meid)
        {
            return dal.get_usergoid(meid);
        }
        /// <summary>
        /// me_根据类型查询数量
        /// </summary>
        public long get_typenum(int type)
        {
            return dal.get_typenum(type);
        }
        /// <summary>
        /// me_根据name_id得到一个对象实体
        /// </summary>
        public BCW.farm.Model.NC_shop GetNC_shop1(int ID)
        {
            return dal.GetNC_shop1(ID);
        }
        /// <summary>
        /// me_根据name得到一个对象实体
        /// </summary>
        public BCW.farm.Model.NC_shop GetNC_shop2(string name)
        {

            return dal.GetNC_shop2(name);
        }

        /// <summary>
        /// me_是否存在该种子ID
        /// //前台购买判断
        /// </summary>
        public bool Exists_zzid(int ID)
        {
            return dal.Exists_zzid(ID);
        }
        /// <summary>
        /// me_是否存在该种子ID
        /// //后台增加判断
        /// </summary>
        public bool Exists_zzid2(int ID)
        {
            return dal.Exists_zzid2(ID);
        }
        /// <summary>
        /// me_是否存在该种子名称
        /// </summary>
        public bool Exists_zzmc(string name)
        {
            return dal.Exists_zzmc(name);
        }
        /// <summary>
        /// me_得到最后一个对象实体
        /// </summary>
        public BCW.farm.Model.NC_shop GetNC_shop_last(int ID)
        {

            return dal.GetNC_shop_last(ID);
        }
        /// <summary>
        ///  me_根据字段修改数据列表
        /// </summary>
        public DataSet update_shop(string strField, string strWhere)
        {
            return dal.update_shop(strField, strWhere);
        }


        /// <summary>
        /// me_取得每页记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList NC_shop</returns>
        public IList<BCW.farm.Model.NC_shop> GetNC_shops(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
        {
            return dal.GetNC_shops(p_pageIndex, p_pageSize, strWhere, strOrder, out p_recordCount);
        }


        #endregion  成员方法
    }
}

