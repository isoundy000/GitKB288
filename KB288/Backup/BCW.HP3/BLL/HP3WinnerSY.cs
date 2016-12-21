using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.HP3.Model;

namespace BCW.HP3.BLL
{
    public class HP3WinnerSY
    {
        private readonly BCW.HP3.DAL.HP3WinnerSY dal = new BCW.HP3.DAL.HP3WinnerSY();
        public HP3WinnerSY()
        {
        }
        #region  成员方法
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            return dal.Exists(ID);
        }
        public bool Exists2(string Wdate)
        {
            return dal.Exists2(Wdate);
        }
        public bool Exists3(int ID, int WinUserID)
        {
            return dal.Exists3(ID,WinUserID);
        }
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(BCW.HP3.Model.HP3WinnerSY model)
        {    
                return dal.Add(model);
        }
        public bool Update(BCW.HP3.Model.HP3WinnerSY model)
        {
            return dal.Update(model);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int ID)
        {

            return dal.Delete(ID);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(string swhere)
        {

            return dal.Delete(swhere);
        }
        ///通过ID更新数据
        public bool UpdateByID(int ID)
        {
            return dal.UpdateByID(ID);
        }
        ///机器人通过ID更新数据
        public bool RoBotByID(int ID)
        {
            return dal.RoBotByID(ID);
        }
        ///通过ID更新WinZhu数据
        public bool UpdateWinZhu(int ID, int winzhu)
        {
            return dal.UpdateWinZhu(ID,winzhu);
        }
        ///通过期号更新数据
        public void UpdateByWinDate(string WinDate)
        {
           dal.UpdateByWinDate(WinDate);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {    
            return dal.GetList(strWhere);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetLists(string strWhere)
        {
            return dal.GetLists(strWhere);
        }
        //取排行榜数据列表
        public DataSet GetListBang()
        {
            return dal.GetListBang();
        }
        //取排行榜数据列表
        public DataSet GetListBang2(string s1,string s2)
        {
            return dal.GetListBang2(s1,s2);
        }
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.HP3.Model.HP3WinnerSY GetModel(int ID)
        {

            return dal.GetModel(ID);
        }
                /// <summary>
        /// 分页获取数据列表
        /// </summary>
        public DataSet GetListByPage(int startIndex, int endIndex)
        {
            return dal.GetListByPage(startIndex,endIndex);
        }
             /// <summary>
        /// 分页条件获取排行榜数据列表
        /// </summary>
        public DataSet GetListByPage2(int startIndex, int endIndex, string s1, string s2)
        {
            return dal.GetListByPage2(startIndex,endIndex,s1,s2);
        }
               //查询我的中奖未兑换订单
        public DataSet GetMyWinList(string strWhere)
        {
            return dal.GetMyWinList(strWhere);
        }
        /// 取得每页记录
        public IList<BCW.HP3.Model.HP3WinnerSY> GetListNes(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetListNes(p_pageIndex,p_pageSize,strWhere, out p_recordCount);
        }
        ///由用户ID取获奖总钱数
        public DataSet GetMyAllGet(int WinUserID)
        {
            return dal.GetMyAllGet(WinUserID);
        }
        /// <summary>
        /// 得到酷币支出
        /// </summary>
        public DataSet GetMoney(string strWhere)
        {
            return dal.GetMoney(strWhere);
        }
        /// <summary>
        /// 得到酷币支出2
        /// </summary>
        public DataSet GetMoney2(string strWhere, string strWhere2)
        {
            return dal.GetMoney2(strWhere,strWhere2);
        }
        #endregion  成员方法
    }
}
