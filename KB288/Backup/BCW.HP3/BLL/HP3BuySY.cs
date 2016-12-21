using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.HP3.Model;

namespace BCW.HP3.BLL
{
    public class HP3BuySY
    {
        private readonly BCW.HP3.DAL.HP3BuySY dal = new BCW.HP3.DAL.HP3BuySY();
        public HP3BuySY()
        {
        }
        #region  成员方法
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            return dal.Exists(ID);
        }

        /// <summary>
        /// 增加一条购彩记录
        /// </summary>
        public int Add(BCW.HP3.Model.HP3BuySY model)
        {
            return dal.Add(model);
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(BCW.HP3.Model.HP3BuySY model)
        {
            return dal.Update(model);
        }
        public void UpdateWillGet(int ID, long WillGet)
        {
             dal.UpdateWillGet(ID, WillGet);
        }
        public void UpdateIsRot(int ID, int IsRot)
        {
            dal.UpdateIsRot(ID, IsRot);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int ID)
        {

            return dal.Delete(ID);
        }
        /// <summary>
        /// 根据期数删除数据
        /// </summary>
        public bool DeleteBuyDate(string BuyDate)
        {

            return dal.DeleteBuyDate(BuyDate);
        }
        //根据字段取数据列表
        public DataSet GetList(string strField, string strWhere)
        {
            return dal.GetList(strField, strWhere);
        }
        //取排行榜数据列表
        public DataSet GetListBang()
        {
            return dal.GetListBang();
        }
        //取排行榜数据列表
        public DataSet GetBang(string s1, string s2)
        {
            return dal.GetBang(s1,s2);
        }
        //根据ID取数据列表
        public DataSet GetListByID(string strField, int id,string buydate)
        {
            String strWhere=" BuyID="+id+" and BuyDate='"+buydate+"'";
            return dal.GetList(strField, strWhere);
        }
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="p_pageIndex"></param>
        /// <param name="p_pageSize"></param>
        /// <param name="strWhere"></param>
        /// <param name="p_recordCount"></param>
        /// <returns></returns>
        public IList<BCW.HP3.Model.HP3BuySY> GetHP3ListByPage(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetHP3ListByPage(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }
        public int GetRecordCount(string strWhere)
        {
            return dal.GetRecordCount(strWhere);
        }
        public DataSet GetDaXiao(string qihao, string goumai)
        {
            return dal.GetDaXiao(qihao,goumai);
        }
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.HP3.Model.HP3BuySY GetModel(int ID)
        {

            return dal.GetModel(ID);
        }
        /// 由用户ID获取用户总钱数
        public DataSet GetMyAllPost(int BuyID)
        {
            return dal.GetMyAllPost(BuyID);
        }
        /// <summary>
        /// 得到酷币收入
        /// </summary>
        public DataSet GetMoney(string strWhere)
        {
            return dal.GetMoney(strWhere);
        }
        /// <summary>
        /// 得到酷币收入2
        /// </summary>
        public DataSet GetMoney2(string strWhere, string strWhere2)
        {
            return dal.GetMoney2(strWhere,strWhere2);
        }
        public DataSet GetListBang2(string s1, string s2)
        {
            return dal.GetListBang2(s1,s2);
        }
        /// <summary>
        /// 分页按条件获取排行榜数据列表
        /// </summary>
        public DataSet GetListByPage(int startIndex, int endIndex, string s1, string s2)
        {
            return dal.GetListByPage(startIndex, endIndex,s1,s2);
        }
        public DataSet GetBangByPage(int startIndex, int endIndex, string s1, string s2)
        {
            return dal.GetBangByPage(startIndex, endIndex, s1, s2);
        }
        #endregion  成员方法
    }
}
