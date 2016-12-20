using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.JQC.Model;
namespace BCW.JQC.BLL
{
    /// <summary>
    /// 业务逻辑类JQC_Jackpot 的摘要说明。
    /// </summary>
    public class JQC_Jackpot
    {
        private readonly BCW.JQC.DAL.JQC_Jackpot dal = new BCW.JQC.DAL.JQC_Jackpot();
        public JQC_Jackpot()
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
        public bool Exists(int id)
        {
            return dal.Exists(id);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(BCW.JQC.Model.JQC_Jackpot model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(BCW.JQC.Model.JQC_Jackpot model)
        {
            dal.Update(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int id)
        {

            dal.Delete(id);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.JQC.Model.JQC_Jackpot GetJQC_Jackpot(int id)
        {

            return dal.GetJQC_Jackpot(id);
        }

        /// <summary>
        /// 根据字段取数据列表
        /// </summary>
        public DataSet GetList(string strField, string strWhere)
        {
            return dal.GetList(strField, strWhere);
        }

        //==    
        /// <summary>
        /// 计算投注总币值
        /// </summary>
        public long GetPrice(string ziduan, string strWhere)
        {
            return dal.GetPrice(ziduan, strWhere);
        }

        /// <summary>
        /// me_是否存在系统投进记录
        /// </summary>
        public bool Exists_jc(int id)
        {
            return dal.Exists_jc(id);
        }
        /// <summary>
        /// me_是否存在系统扣除记录
        /// </summary>
        public bool Exists_kc(int id)
        {
            return dal.Exists_kc(id);
        }
        /// <summary>
        /// me_根据id得到奖池
        /// </summary>
        public long GetGold()
        {
            return dal.GetGold();
        }
        /// <summary>
        /// me_根据期号得到奖池
        /// </summary>
        public long GetGold_phase(int phase)
        {
            return dal.GetGold_phase(phase);
        }
        /// <summary>
        /// me_得到系统投进的次数
        /// </summary>
        public int Getxitong_toujin()
        {
            return dal.Getxitong_toujin();
        }
        /// <summary>
        /// me_得到系统回收的次数
        /// </summary>
        public int Getxitong_huishou()
        {
            return dal.Getxitong_huishou();
        }
        /// <summary>
        /// me_根据投注ID得到奖池——20160713
        /// </summary>
        public long Get_BetID(int BetID)
        {
            return dal.Get_BetID(BetID);
        }
        /// <summary>
        /// me_根据期号得到未开奖的奖池——20160713
        /// </summary>
        public long Getweikai_phase(int phase)
        {
            return dal.Getweikai_phase(phase);
        }
        /// <summary>
        /// me_得到该期系统收取
        /// </summary>
        public long Get_xtshouqu(int phase)
        {
            return dal.Get_xtshouqu(phase);
        }
        //==================================

        /// <summary>
        /// 取得每页记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList JQC_Jackpot</returns>
        public IList<BCW.JQC.Model.JQC_Jackpot> GetJQC_Jackpots(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
        {
            return dal.GetJQC_Jackpots(p_pageIndex, p_pageSize, strWhere, strOrder, out p_recordCount);
        }

        #endregion  成员方法
    }
}

