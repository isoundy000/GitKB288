using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.farm.Model;
namespace BCW.farm.BLL
{
    /// <summary>
    /// 业务逻辑类NC_hecheng 的摘要说明。
    /// </summary>
    public class NC_hecheng
    {
        private readonly BCW.farm.DAL.NC_hecheng dal = new BCW.farm.DAL.NC_hecheng();
        public NC_hecheng()
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
        public int Add(BCW.farm.Model.NC_hecheng model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(BCW.farm.Model.NC_hecheng model)
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
        public BCW.farm.Model.NC_hecheng GetNC_hecheng(int ID)
        {
            return dal.GetNC_hecheng(ID);
        }

        /// <summary>
        /// 根据字段取数据列表
        /// </summary>
        public DataSet GetList(string strField, string strWhere)
        {
            return dal.GetList(strField, strWhere);
        }

        //=====================================

        /// <summary>
        /// me_是否存在该记录
        /// </summary>
        public bool Exists_ID(int ID, int meid)
        {
            return dal.Exists_ID(ID, meid);
        }
        /// <summary>
        ///  me_根据字段修改数据列表
        /// </summary>
        public DataSet update_ID(string strField, string strWhere)
        {
            return dal.update_ID(strField, strWhere);
        }
        /// <summary>
        /// me_更新果实数量
        /// </summary>
        public void Update_gs(int usid, int num, int name_id)
        {
            dal.Update_gs(usid, num, name_id);
        }
        /// <summary>
        /// me_得到种子数量
        /// </summary>
        public int Get_daoju_num(int meid, int ID)
        {
            return dal.Get_daoju_num(meid, ID);
        }
        /// me_是否存在该作物2
        /// </summary>
        public bool Exists_zuowu2(int id, int meid)
        {
            return dal.Exists_zuowu2(id, meid);
        }
        /// <summary>
        /// me_得到一个对象实体
        /// </summary>
        public BCW.farm.Model.NC_hecheng GetNC_hecheng2(int ID, int meid)
        {
            return dal.GetNC_hecheng2(ID, meid);
        }
        //==================================

        /// <summary>
        /// 取得每页记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList NC_hecheng</returns>
        public IList<BCW.farm.Model.NC_hecheng> GetNC_hechengs(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
        {
            return dal.GetNC_hechengs(p_pageIndex, p_pageSize, strWhere, strOrder, out p_recordCount);
        }

        #endregion  成员方法
    }
}

