using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.JQC.Model;
namespace BCW.JQC.BLL
{
    /// <summary>
    /// 业务逻辑类JQC_Internet 的摘要说明。
    /// </summary>
    public class JQC_Internet
    {
        private readonly BCW.JQC.DAL.JQC_Internet dal = new BCW.JQC.DAL.JQC_Internet();
        public JQC_Internet()
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
        public int Add(BCW.JQC.Model.JQC_Internet model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(BCW.JQC.Model.JQC_Internet model)
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
        public BCW.JQC.Model.JQC_Internet GetJQC_Internet(int ID)
        {
            return dal.GetJQC_Internet(ID);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.JQC.Model.JQC_Internet GetJQC_Internet2(int ID)
        {
            return dal.GetJQC_Internet2(ID);
        }

        /// <summary>
        /// 根据字段取数据列表
        /// </summary>
        public DataSet GetList(string strField, string strWhere)
        {
            return dal.GetList(strField, strWhere);
        }


        //====================================
        /// <summary>
        /// me_更新一条数据
        /// </summary>
        public void Update_ht(BCW.JQC.Model.JQC_Internet model)
        {
            dal.Update_ht(model);
        }
        /// <summary>
        /// me_初始化某数据表
        /// </summary>
        /// <param name="TableName">数据表名称</param>
        public void ClearTable(string TableName)
        {
            dal.ClearTable(TableName);
        }
        /// <summary>
        /// me_得到最后一期已开奖的数据——20160805
        /// </summary>
        public int Get_kaiID()
        {
            return dal.Get_kaiID();
        }
        /// <summary>
        /// me_得到最后一期未开奖的数据——20160714
        /// </summary>
        public int Get_newID()
        {
            return dal.Get_newID();
        }
        /// <summary>
        /// me_后台奖池查询——20160808
        /// </summary>
        public int Get_oldID()
        {
            return dal.Get_oldID();
        }
        /// <summary>
        /// me_后台奖池查询——20160808
        /// </summary>
        public int Get_oldID2()
        {
            return dal.Get_oldID2();
        }
        /// <summary>
        /// me_是否存在未开奖的期号 20161013邵广林
        /// </summary>
        public bool Exists_wei(int ID)
        {
            return dal.Exists_wei(ID);
        }
        /// <summary>
        /// me_是否存在该记录——最新期号
        /// </summary>
        public bool Exists_phase(int ID)
        {
            return dal.Exists_phase(ID);
        }
        /// <summary>
        /// me_是否存在该记录——是否开奖
        /// </summary>
        public bool Exists_Result(int ID)
        {
            return dal.Exists_Result(ID);
        }
        /// <summary>
        ///  me_根据字段修改数据列表
        /// </summary>
        public DataSet Update_Result(string strField, string strWhere)
        {
            return dal.Update_Result(strField, strWhere);
        }
        /// <summary>
        /// me_得到一个对象实体
        /// </summary>
        public BCW.JQC.Model.JQC_Internet GetJQC_Internet_model(string where)
        {
            return dal.GetJQC_Internet_model(where);
        }
        /// <summary>
        /// me_得到一个开奖号码对象实体
        /// </summary>
        public BCW.JQC.Model.JQC_Internet Get_kainum(int Lottery_issue)
        {
            return dal.Get_kainum(Lottery_issue);
        }
        /// <summary>
        /// me_得到每注金额
        /// </summary>
        public string get_zhumeney(int Lottery_issue)
        {
            return dal.get_zhumeney(Lottery_issue);
        }
        /// <summary>
        /// me_得到每期奖池
        /// </summary>
        public int get_jiangchi(int Lottery_issue)
        {
            return dal.get_jiangchi(Lottery_issue);
        }
        //====================================

        /// <summary>
        /// 取得每页记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList JQC_Internet</returns>
        public IList<BCW.JQC.Model.JQC_Internet> GetJQC_Internets(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetJQC_Internets(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }

        #endregion  成员方法
    }
}

