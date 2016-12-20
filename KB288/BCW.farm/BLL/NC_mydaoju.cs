using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.farm.Model;
namespace BCW.farm.BLL
{
    /// <summary>
    /// 业务逻辑类NC_mydaoju 的摘要说明。
    /// </summary>
    public class NC_mydaoju
    {
        private readonly BCW.farm.DAL.NC_mydaoju dal = new BCW.farm.DAL.NC_mydaoju();
        public NC_mydaoju()
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
        public int Add(BCW.farm.Model.NC_mydaoju model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(BCW.farm.Model.NC_mydaoju model)
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
        public BCW.farm.Model.NC_mydaoju GetNC_mydaoju(int ID)
        {

            return dal.GetNC_mydaoju(ID);
        }

        /// <summary>
        /// 根据字段取数据列表
        /// </summary>
        public DataSet GetList(string strField, string strWhere)
        {
            return dal.GetList(strField, strWhere);
        }


        //===================================
        /// <summary>
        /// me_删除一条数据
        /// </summary>
        public void Delete(int ID, int usid)
        {

            dal.Delete(ID, usid);
        }
        /// <summary>
        /// me_删除一条数据
        /// </summary>
        public void Delete2(int ID, int usid)
        {

            dal.Delete2(ID, usid);
        }
        /// <summary>
        /// me_根据字段取数据列表
        /// </summary>
        public DataSet GetList2(string strField, string strWhere)
        {
            return dal.GetList2(strField, strWhere);
        }
        /// <summary>
        /// me_是否存在该记录
        /// </summary>
        public bool Exists(int ID, int meid)
        {
            return dal.Exists(ID, meid);
        }
        /// <summary>
        /// me_是否存在该记录
        /// </summary>
        public bool Exists2(int ID, int meid)
        {
            return dal.Exists2(ID, meid);
        }
        /// <summary>
        /// me_根据name_id得到一个对象实体
        /// </summary>
        public BCW.farm.Model.NC_mydaoju Getname_id(int meid, int ID)
        {
            return dal.Getname_id(meid, ID);
        }
        /// <summary>
        /// me_得到种子数量
        /// </summary>
        public int Get_daoju_num(int meid, int ID)
        {
            return dal.Get_daoju_num(meid, ID);
        }
        /// <summary>
        /// me_得到道具数量
        /// </summary>
        public int Get_daojunum2(int meid, int ID, int zs)
        {
            return dal.Get_daojunum2(meid, ID, zs);
        }
        /// <summary>
        /// me_得到是否锁定
        /// </summary>
        public int Get_suoding(int meid, int ID)
        {
            return dal.Get_suoding(meid, ID);
        }
        /// <summary>
        /// me_更新种植的种子
        /// </summary>
        public void Update_zz(int usid, int num, int name_id)
        {
            dal.Update_zz(usid, num, name_id);
        }
        /// <summary>
        /// me_更新种植的种子
        /// </summary>
        public void Update_zz2(int usid, int num, int name_id)
        {
            dal.Update_zz2(usid, num, name_id);
        }
        /// <summary>
        /// me_是否存在化肥该记录
        /// </summary>
        public bool Exists_hf(int ID, int usid, int zs)
        {
            return dal.Exists_hf(ID, usid, zs);
        }
        /// <summary>
        /// me_是否存在化肥该记录
        /// </summary>
        public bool Exists_hf(int ID, int usid)
        {
            return dal.Exists_hf(ID, usid);
        }
        /// <summary>
        /// me_是否存在化肥该记录2
        /// </summary>
        public bool Exists_hf2(int ID, int usid, int zs)
        {
            return dal.Exists_hf2(ID, usid, zs);
        }
        /// <summary>
        /// me_查询是否有道具(可赠送和不可赠送)
        /// </summary>
        public bool Exists_hf3(int ID, int usid)
        {
            return dal.Exists_hf3(ID, usid);
        }
        /// <summary>
        /// me_是否存在银钥匙
        /// </summary>
        public BCW.farm.Model.NC_mydaoju Get_yys(int meid, int huafei_id)
        {
            return dal.Get_yys(meid, huafei_id);
        }
        /// <summary>
        /// me_是否存在宝箱钥匙
        /// </summary>
        public BCW.farm.Model.NC_mydaoju Get_bxys(int meid, int huafei_id)
        {

            return dal.Get_bxys(meid, huafei_id);
        }
        /// <summary>
        /// me_根据huafei_id得到一个对象实体
        /// </summary>
        public BCW.farm.Model.NC_mydaoju Gethf_id(int meid, int ID, int zs)
        {
            return dal.Gethf_id(meid, ID, zs);
        }
        /// <summary>
        /// me_更新化肥数量
        /// </summary>
        public void Update_hf(int usid, int num, int huafei_id, int zs)
        {
            dal.Update_hf(usid, num, huafei_id, zs);
        }
        /// <summary>
        /// me_更新化肥数量
        /// </summary>
        public void Update_hf2(int usid, int num, int huafei_id)
        {
            dal.Update_hf2(usid, num, huafei_id);
        }
        /// <summary>
        /// me_是否改作物种子
        /// </summary>
        public bool Exists_zz(int ID)
        {
            return dal.Exists_zz(ID);
        }
        /// <summary>
        /// me_锁定
        /// </summary>
        public void Update_sd(int usid, int name_id)
        {
            dal.Update_sd(usid, name_id);
        }
        /// <summary>
        /// me_解锁
        /// </summary>
        public void Update_jiesuo(int meid, int id)
        {
            dal.Update_jiesuo(meid, id);
        }

        /// <summary>
        /// me_取得每页记录2
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList NC_mydaoju</returns>
        public IList<BCW.farm.Model.NC_mydaoju> GetNC_mydaojus2(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
        {
            return dal.GetNC_mydaojus2(p_pageIndex, p_pageSize, strWhere, strOrder, out p_recordCount);
        }

        /// <summary>
        /// me_取得每页记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList NC_mydaoju</returns>
        public IList<BCW.farm.Model.NC_mydaoju> GetNC_mydaojus(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetNC_mydaojus(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }


        #endregion  成员方法

        //===================================

    }
}

