using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.farm.Model;
namespace BCW.farm.BLL
{
    /// <summary>
    /// 业务逻辑类NC_GetCrop 的摘要说明。
    /// </summary>
    public class NC_GetCrop
    {
        private readonly BCW.farm.DAL.NC_GetCrop dal = new BCW.farm.DAL.NC_GetCrop();
        public NC_GetCrop()
        { }
        #region  成员方法
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(long id)
        {
            return dal.Exists(id);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(BCW.farm.Model.NC_GetCrop model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(BCW.farm.Model.NC_GetCrop model)
        {
            dal.Update(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(long id)
        {

            dal.Delete(id);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.farm.Model.NC_GetCrop GetNC_GetCrop(long id)
        {

            return dal.GetNC_GetCrop(id);
        }

        /// <summary>
        /// 根据字段取数据列表
        /// </summary>
        public DataSet GetList(string strField, string strWhere)
        {
            return dal.GetList(strField, strWhere);
        }

        /// <summary>
        /// 根据字段取数据列表2
        /// </summary>
        public DataSet GetList2(string strField, string strWhere)
        {
            return dal.GetList2(strField, strWhere);
        }

        //==============================
        /// <summary>
        /// me_根据name_id和usid删除一条数据
        /// </summary>
        public void Delete(int id, int usid)
        {

            dal.Delete(id, usid);
        }
        /// me_是否存在该作物
		/// </summary>
		public bool Exists_zuowu(string name, int usid)
        {
            return dal.Exists_zuowu(name, usid);
        }
        /// <summary>
        /// me_更新一条作物数据
        /// </summary>
        public void Update1(BCW.farm.Model.NC_GetCrop model)
        {
            dal.Update1(model);
        }
        /// me_是否存在该作物2
        /// </summary>
        public bool Exists_zuowu2(int id, int meid)
        {
            return dal.Exists_zuowu2(id, meid);
        }
        /// me_是否存在该作物3
        /// </summary>
        public bool Exists_zuowu3(int id, int meid)
        {
            return dal.Exists_zuowu3(id, meid);
        }
        /// <summary>
        /// me_锁定
        /// </summary>
        public void Update_suoding(int meid, int id)
        {
            dal.Update_suoding(meid, id);
        }
        /// <summary>
        /// me_解锁
        /// </summary>
        public void Update_jiesuo(int meid, int id)
        {
            dal.Update_jiesuo(meid, id);
        }
        /// <summary>
        /// me_得到总价钱
        /// </summary>
        public long Get_allprice(int usid)
        {
            return dal.Get_allprice(usid);
        }
        /// <summary>
        /// me_卖出
        /// </summary>
        public void Update_maichu(int meid, int num, int id)
        {
            dal.Update_maichu(meid, num, id);
        }
        /// <summary>
        /// me_得到一个对象实体
        /// </summary>
        public BCW.farm.Model.NC_GetCrop GetNC_GetCrop2(int id, int usid)
        {

            return dal.GetNC_GetCrop2(id, usid);
        }
        /// <summary>
        /// me_更新果实数量
        /// </summary>
        public void Update_gs(int usid, int num, int name_id)
        {
            dal.Update_gs(usid, num, name_id);
        }
        /// <summary>
        /// me_更新果实数量
        /// </summary>
        public void Update_gs2(int usid, int num, int name_id)
        {
            dal.Update_gs2(usid, num, name_id);
        }

        /// <summary>
        /// me_取得每页记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList NC_GetCrop</returns>
        public IList<BCW.farm.Model.NC_GetCrop> GetNC_GetCrops(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
        {
            return dal.GetNC_GetCrops(p_pageIndex, p_pageSize, strWhere, strOrder, out p_recordCount);
        }

        #endregion  成员方法

        //==============================
    }
}

