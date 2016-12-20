using System;
using System.Data;
using System.Collections.Generic;
using TPR2.Common;
using TPR2.Model.guess;
namespace TPR2.BLL.guess
{
    /// <summary>
    /// 业务逻辑类BaPayMe 的摘要说明。
    /// </summary>
    public class BaPayMe
    {
        private readonly TPR2.DAL.guess.BaPayMe dal = new TPR2.DAL.guess.BaPayMe();
        public BaPayMe()
        { }
        #region  成员方法
               
        /// <summary>
        /// 是否存在超级竞猜结束记录
        /// </summary>
        public bool Exists(int bcid, int payusid)
        {
            return dal.Exists(bcid, payusid);
        }


        /// <summary>
        /// 得到某一赛事下注结果得币
        /// </summary>
        public decimal GetBaPayMeMoney(int id, int payusid)
        {
            return dal.GetBaPayMeMoney(id, payusid);
        }
             
        /// <summary>
        /// 得到某赛事各项目的记录数
        /// </summary>
        public int GetCount(int ibcid, int iptype, int ipaytype)
        {
            return dal.GetCount(ibcid, iptype, ipaytype);
        }
        /// <summary>
        /// 得到某ID在某赛事各项目的记录数
        /// </summary>
        public int GetCount(int ibcid, int iptype, int ipaytype, int iusid)
        {
            return dal.GetCount(ibcid, iptype, ipaytype, iusid);
        }
        /// <summary>
        /// 得到某一赛事下注总注数
        /// </summary>
        public int GetBaPayMeNum(int ibcid, int iptype)
        {
            return dal.GetBaPayMeNum(ibcid, iptype);
        }

        /// <summary>
        /// 得到某一赛事下注总金额
        /// </summary>
        public long GetBaPayMeCent(int ibcid, int iptype)
        {
            return dal.GetBaPayMeCent(ibcid, iptype);
        }
        
        /// <summary>
        /// 得到某一赛事某项下注总金额
        /// </summary>
        public long GetBaPayMeCent(int ibcid, int iptype, int ipaytype)
        {
            return dal.GetBaPayMeCent(ibcid, iptype, ipaytype);
        }

        /// <summary>
        /// 根据条件得到赛事下注数
        /// </summary>
        public int GetBaPayMeCount(string strWhere)
        {
            return dal.GetBaPayMeCount(strWhere);
        }
        /// <summary>
        /// 根据条件得到赛事下注总金额
        /// </summary>
        public long GetBaPayMepayCent(string strWhere)
        {
            return dal.GetBaPayMepayCent(strWhere);
        }
        /// <summary>
        /// 根据条件得到赛事下注盈利额
        /// </summary>
        public long GetBaPayMegetMoney(string strWhere)
        {
            return dal.GetBaPayMegetMoney(strWhere);
        }

        /// <summary>
        /// 是否存在兑奖记录
        /// </summary>
        public bool ExistsIsCase(int ID, int payusid)
        {
            return dal.ExistsIsCase(ID, payusid);
        }

        /// <summary>
        /// 更新用户兑奖
        /// </summary>
        public void UpdateIsCase(int ID)
        {
            dal.UpdateIsCase(ID);
        }

        /// <summary>
        /// 更新走地下注为正常
        /// </summary>
        public void Updatestate(int ID)
        {
            dal.Updatestate(ID);
        }

        /// <summary>
        /// 更新平盘业务
        /// </summary>
        public void UpdatePPCase(TPR2.Model.guess.BaPayMe model)
        {
            dal.UpdatePPCase(model);
        }

        /// <summary>
        /// 更新开奖业务
        /// </summary>
        public void UpdateCase(TPR2.Model.guess.BaPayMe model, string p_strVal, out decimal p_intDuVal, out int p_intWin, out string WinType)
        {
            dal.UpdateCase(model, p_strVal, out p_intDuVal, out p_intWin, out WinType);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(TPR2.Model.guess.BaPayMe model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int ID)
        {
            dal.Delete(ID);
        }

        /// <summary>
        /// 删除一组数据
        /// </summary>
        public void DeleteStr(string strWhere)
        {
            dal.DeleteStr(strWhere);
        }

        /// <summary>
        /// 删除一条数据,根据赛事ID
        /// </summary>
        public void Deletebcid(int gid)
        {
            dal.Deletebcid(gid);
        }

        /// <summary>
        /// 得到一个getMoney
        /// </summary>
        public decimal Getp_getMoney(int ID)
        {
            return dal.Getp_getMoney(ID);
        }
                
        /// <summary>
        /// 得到一个Types
        /// </summary>
        public int GetTypes(int ID)
        {
            return dal.GetTypes(ID);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public TPR2.Model.guess.BaPayMe GetModelIsCase(int ID)
        {
            return dal.GetModelIsCase(ID);
        }
                
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public TPR2.Model.guess.BaPayMe GetModel(int ID)
        {
            return dal.GetModel(ID);
        }

        /// <summary>
        /// 根据字段取数据列表
        /// </summary>
        public DataSet GetBaPayMeList(string strField, string strWhere)
        {
            return dal.GetBaPayMeList(strField, strWhere);
        }

        /// <summary>
        /// 根据字段取数据列表
        /// </summary>
        public DataSet GetBaPayMeList(string strField, string strWhere, string filedOrder)
        {
            return dal.GetBaPayMeList(strField, strWhere, filedOrder);
        }

        /// <summary>
        /// 取得每页记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <returns>IList</returns>
        public IList<TPR2.Model.guess.BaPayMe> GetBaPayMes(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetBaPayMes(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }

        /// <summary>
        /// 取得每页记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <returns>IList</returns>
        public IList<TPR2.Model.guess.BaPayMe> GetBaPayMeViews(int p_pageIndex, int p_pageSize, string strWhere, int itype, out int p_recordCount)
        {
            return dal.GetBaPayMeViews(p_pageIndex, p_pageSize, strWhere, itype, out p_recordCount);
        }
    
        /// <summary>
        /// 取得详细排行榜记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <returns>IList BaPayMeTop</returns>
        public IList<TPR2.Model.guess.BaPayMe> GetBaPayMeTop(int p_pageIndex, int p_pageSize, string strWhere, int itype, out int p_recordCount)
        {
            return dal.GetBaPayMeTop(p_pageIndex, p_pageSize, strWhere, itype, out p_recordCount);
        }

        /// <summary>
        /// 取得详细排行榜记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <returns>IList BaPayMeTop</returns>
        public IList<TPR2.Model.guess.BaPayMe> GetBaPayMeTop2(int p_pageIndex, int p_pageSize, string strWhere, int itype, out int p_recordCount)
        {
            return dal.GetBaPayMeTop2(p_pageIndex, p_pageSize, strWhere, itype, out p_recordCount);
        }

        #endregion  成员方法
    }
}
