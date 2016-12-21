using System.Data;
using System.Collections.Generic;
namespace BCW.BQC.BLL
{
    /// <summary>
    /// 业务逻辑类BQCList 的摘要说明。
    /// </summary>
    public class BQCList
    {
        private readonly DAL.BQCList dal = new DAL.BQCList();
        public BQCList()
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
        /// 根据期号得到id
        /// </summary>
        public int id(int CID)
        {
            return dal.id(CID);
        }
        /// <summary>
        /// 得到当期奖池结余
        /// </summary>
        public int getnextprize(int CID)
        {
            return dal.getnextprize(CID);
        }
        /// <summary>
        /// 得到首期投入标识
        /// </summary>
        public int getsysstate(int CID)
        {
            return dal.getsysstate(CID);
        }
        /// <summary>
        /// 得到首期投入标识
        /// </summary>
        public int getState(int CID)
        {
            return dal.getState(CID);
        }
        /// <summary>
        /// 得到下一期CID
        /// </summary>
        public int CIDnew()
        {
            return dal.CIDnew();
        }
        /// <summary>
        /// 得到开奖结果
        /// </summary>
        public string result(int CID)
        {
            return dal.result(CID);
        }
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int id)
        {
            return dal.Exists(id);
        }      /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Existslist(int id)
        {
            return dal.Existslist(id);
        }
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Existsjilu()
        {
            return dal.Existsjilu();
        }
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Existsysprize(int CID)
        {
            return dal.Existsysprize(CID);
        }
        /// <summary>
        /// 是否存在系统投注记录
        /// </summary>
        public bool ExistsSysprize(int CID)
        {
            return dal.ExistsSysprize(CID);
        }

        /// <summary>
        /// 得到当前奖池总额
        /// </summary>
        public long nowprize(int CID)
        {
            return dal.nowprize(CID);
        }
        /// <summary>
        /// 更新系统投注状态
        /// </summary>
        public void UpdateSysprizestatue(int CID, int sysprizestatue)
        {
            dal.UpdateSysprizestatue(CID, sysprizestatue);
        }
        /// <summary>
        /// 更新系统投注
        /// </summary>
        public void UpdateSysstaprize(int CID, int sysprizestatue, long sysprize)
        {
            dal.UpdateSysstaprize(CID, sysprizestatue, sysprize);
        }
        /// <summary>
        /// 更新下注总额
        /// </summary>
        /// <param name="PayCent"></param>
        /// <param name="CID"></param>
        public void UpdatePayCent(long PayCent, int CID)
        {
            dal.UpdatePayCent(PayCent, CID);
        }

        /// <summary>
        /// 更新下注数
        /// </summary>
        /// <param name="PayCount"></param>
        /// <param name="CID"></param>
        public void UpdatePayCount(int PayCount, int CID)
        {
            dal.UpdatePayCount(PayCount, CID);
        }

        public void updateNowprize(long nowprize, int CID)
        {
            dal.updateNowprize(nowprize, CID);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void UpdateResult(int id, string Result)
        {
            dal.UpdateResult(id, Result);
        }

        /// <summary>
        /// 查找指定期数的比赛结果
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string FindResultByPhase(int CID)
        {
            return dal.FindResultByPhase(CID);
        }
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public void Add(Model.BQCList model)
        {
            dal.Add(model);
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(Model.BQCList model)
        {
            dal.Update(model);
        }

        /// <summary>
        /// 更新赛事
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateMatchs(Model.BQCList model)
        {
            if (dal.UpdateMatchs(model) != 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 更新开奖状态
        /// </summary>
        /// <param name="state">状态</param>
        /// <param name="cid">期号</param>
        /// <returns></returns>
        public bool UpdateState(int state, int cid)
        {
            if (dal.UpdateState(state, cid) != 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int id)
        {

            dal.Delete(id);
        }

        /// <summary>
        /// 得到开奖最新期号
        /// </summary>
        public int CID()
        {
            return dal.CID();
        }
        /// <summary>
        /// 得到一个GetSysPaylast
        /// </summary>
        public long GetSysPaylast()
        {
            return dal.GetSysPaylast();
        }
        /// <summary>
        /// 计算投注总币值
        /// </summary>
        public long GetPrice(string ziduan, string strWhere)
        {
            return dal.GetPrice(ziduan, strWhere);
        }
        /// <summary>
        /// 得到一个GetSysPaylast5
        /// </summary>
        public long GetSysPaylast5()
        {
            return dal.GetSysPaylast5();
        }
        /// <summary>
        /// 得到一个GetSysdayprizelast
        /// </summary>
        public long GetSysdayprizelast()
        {
            return dal.GetSysdayprizelast();
        }
        /// <summary>
        /// 得到一个GetSysdayprizelast5
        /// </summary>
        public long GetSysdayprizelast5()
        {
            return dal.GetSysdayprizelast5();
        }
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Model.BQCList GetBQCList(int id)
        {
            return dal.GetBQCList(id);
        }

        /// <summary>
        /// 根据期号得到一个对象实体
        /// </summary>
        /// <param name="cid"></param>
        /// <returns></returns>
        public Model.BQCList GetBQCListByCID(int cid)
        {
            return dal.GetBQCListByCID(cid);
        }
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.BQC.Model.BQCList GetBQCList1(int CID)
        {
            return dal.GetBQCList1(CID);
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
        /// <returns>IList BQCList</returns>
        public IList<Model.BQCList> GetBQCLists(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetBQCLists(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }

        #endregion  成员方法
    }
}

