using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
    /// <summary>
    /// 业务逻辑类tb_QuestionsAnswerCtr 的摘要说明。
    /// </summary>
    public class tb_QuestionsAnswerCtr
    {
        private readonly BCW.DAL.tb_QuestionsAnswerCtr dal = new BCW.DAL.tb_QuestionsAnswerCtr();
        public tb_QuestionsAnswerCtr()
        { }
        #region  成员方法
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            return dal.Exists(ID);
        }
        /// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool ExistsID(int uid, int cid)
        {
            return dal.ExistsID(uid, cid);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(BCW.Model.tb_QuestionsAnswerCtr model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(BCW.Model.tb_QuestionsAnswerCtr model)
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

        public int GetAllAwardGold(int uid)
        {
            return dal.GetAllAwardGold(uid);

        }
        /// <summary>
        /// 从uid  得到一个最新对象
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="contrID"></param>
        /// <returns></returns>
        public BCW.Model.tb_QuestionsAnswerCtr Gettb_QuestionsAnswerCtrByUid(int uid)
        {

            return dal.Gettb_QuestionsAnswerCtrByUid(uid);
        }


        /// <summary>
        /// 从uid cid 得到一个对象
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="contrID"></param>
        /// <returns></returns>
        public BCW.Model.tb_QuestionsAnswerCtr Gettb_QuestionsAnswerCtrByUidCid(int uid, int contrID)
        {

            return dal.Gettb_QuestionsAnswerCtrByUidCid(uid, contrID);
        }
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.Model.tb_QuestionsAnswerCtr Gettb_QuestionsAnswerCtr(int ID)
        {

            return dal.Gettb_QuestionsAnswerCtr(ID);
        }

        /// <summary>
        /// 根据字段取数据列表
        /// </summary>
        public DataSet GetList(string strField, string strWhere)
        {
            return dal.GetList(strField, strWhere);
        }

        /// <summary>
        /// 取得每页记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList tb_QuestionsAnswerCtr</returns>
        public IList<BCW.Model.tb_QuestionsAnswerCtr> Gettb_QuestionsAnswerCtrs(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.Gettb_QuestionsAnswerCtrs(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }

        #endregion  成员方法
    }
}

