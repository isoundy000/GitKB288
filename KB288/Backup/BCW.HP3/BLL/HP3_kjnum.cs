using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.HP3.Model;

namespace BCW.HP3.BLL
{
    public class HP3_kjnum
    {
        private readonly BCW.HP3.DAL.HP3_kjnum dal = new BCW.HP3.DAL.HP3_kjnum();
        public HP3_kjnum()
        {
        }
        #region  成员方法
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(string datenum)
        {
            return dal.Exists(datenum);
        }
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists2(string datenum)
        {
            return dal.Exists2(datenum);
        }
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists3(string datenum)
        {
            return dal.Exists3(datenum);
        }      

        /// <summary>
        /// 增加一条新开奖信息
        /// </summary>
        public bool Add(BCW.HP3.Model.HP3_kjnum model)
        {
            return dal.Add(model);
        }
               /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(BCW.HP3.Model.HP3_kjnum model)
        {
            return dal.Update(model);
        }
        /// <summary>
        /// 是否存在要更新结果的记录
        /// </summary>
        public bool ExistsUpdateResult()
        {
            return dal.ExistsUpdateResult();
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(string datenum)
        {

            return dal.Delete(datenum);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            return dal.GetList(strWhere);
        }

        /// <summary>
        /// 根据字段取数据列表
        /// </summary>
        public DataSet GetList(string strField, string strWhere)
        {
            return dal.GetList(strField, strWhere);
        }
        /// <summary>
        ///  获得最后一行数据列表
        /// </summary>
        public BCW.HP3.Model.HP3_kjnum GetListLast()
        {
            return dal.GetListLast();
        }
        /// <summary>
        ///  获得最后一行数据列表
        /// </summary>
        public BCW.HP3.Model.HP3_kjnum GetListLastNull()
        {
            return dal.GetListLastNull();
        }
                /// <summary>
        /// 根据期号取数据
        /// </summary>
        public BCW.HP3.Model.HP3_kjnum GetDataByState(string datenum)
        {
            return dal.GetDataByState(datenum);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(int TopNum)
        {
            return dal.GetList(TopNum);
        }
        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        public DataSet GetListByPage(string strWhere, string orderby, int startIndex, int endIndex)
        {
            return dal.GetListByPage(strWhere, orderby, startIndex, endIndex);
        }
        /// <summary>
        /// 分页获取历史开奖
        /// </summary>
        public DataSet GetListHistory(string strWhere,int endIndex)
        {
            string orderby = "datenum desc";
            int startIndex = 0; 
            return dal.GetListByPage(strWhere, orderby, startIndex, endIndex);
        }
        public IList<BCW.HP3.Model.HP3_kjnum> GetHP3ListByPage(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetHP3ListByPage(p_pageIndex,p_pageSize,strWhere, out p_recordCount);
        }
            /// <summary>
        /// 获取记录总数
        /// </summary>
        public int GetRecordCount(string strWhere)
        {
            return dal.GetRecordCount(strWhere);
        }
        /// <summary>
        /// 获取第X个期数
        /// </summary>
        public int GetXXCID(string strWhere)
        {
            return dal.GetXXCID(strWhere);
        }
        //根据日期查询期号
        public DataSet GetDatenumByDate(string strWhere)
        {
            return dal.GetDatenumByDate(strWhere);
        }
               /// <summary>
        /// 初始化某数据表
        /// </summary>
        /// <param name="TableName">数据表名称</param>
        public void ClearTable(string TableName)
        {
            dal.ClearTable(TableName);
        }
        #endregion  成员方法
    }
}
