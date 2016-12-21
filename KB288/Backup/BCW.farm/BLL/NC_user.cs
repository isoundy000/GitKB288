using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.farm.Model;
namespace BCW.farm.BLL
{
    /// <summary>
    /// 业务逻辑类NC_user 的摘要说明。
    /// </summary>
    public class NC_user
    {
        private readonly BCW.farm.DAL.NC_user dal = new BCW.farm.DAL.NC_user();
        public NC_user()
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
        public int Add(BCW.farm.Model.NC_user model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(BCW.farm.Model.NC_user model)
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
        public BCW.farm.Model.NC_user GetNC_user(int ID)
        {

            return dal.GetNC_user(ID);
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
        ///  me_根据字段修改数据列表
        /// </summary>
        public DataSet update_zd(string strField, string strWhere)
        {
            return dal.update_zd(strField, strWhere);
        }
        /// <summary>
        /// me_更新土地类型
        /// </summary>
        public void Update_tdlx(int usid, int tuditype)
        {
            dal.Update_tdlx(usid, tuditype);
        }
        /// <summary>
        /// me_增加一条数据
        /// </summary>
        public int Add_1(BCW.farm.Model.NC_user model)
        {
            return dal.Add_1(model);
        }
        /// <summary>
        /// me_得到一个对象实体
        /// </summary>
        public BCW.farm.Model.NC_user Get_user(int usid)
        {

            return dal.Get_user(usid);
        }
        /// <summary>
        /// me_是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            return dal.Exists(ID);
        }
        /// <summary>
        /// me_更新用户经验
        /// </summary>
        public void Update_Experience(int usid, long experience)
        {
            dal.Update_Experience(usid, experience);
        }
        /// <summary>
        /// me_更新施肥次数
        /// </summary>
        public void Update_shifeinum(int usid, int num)
        {
            dal.Update_shifeinum(usid, num);
        }
        /// <summary>
        /// me_更新用户等级——加
        /// </summary>
        public void Update_dengji(int usid)
        {
            dal.Update_dengji(usid);
        }
        /// <summary>
        /// me_更新用户等级——减
        /// </summary>
        public void Update_dengji2(int usid)
        {
            dal.Update_dengji2(usid);
        }
        /// <summary>
        /// me_更新用户金币
        /// </summary>
        public void Update_jinbi(int usid, long iGoid)
        {
            dal.Update_jinbi(usid, iGoid);
        }
        /// <summary>
        /// me_得到用户币
        /// </summary>
        public long GetGold(int usid)
        {
            return dal.GetGold(usid);
        }
        /// <summary>
        /// me_得到使用化肥次数
        /// </summary>
        public int Get_hfnum(int usid)
        {
            return dal.Get_hfnum(usid);
        }
        /// <summary>
        /// me_得到种草放虫次数 邵广林 20160826
        /// </summary>
        public int Get_zcfcnum(int usid)
        {
            return dal.Get_zcfcnum(usid);
        }
        /// <summary>
        /// me_得到除草除虫次数 邵广林 20160826
        /// </summary>
        public int Get_ccccnum(int usid)
        {
            return dal.Get_ccccnum(usid);
        }
        /// <summary>
        /// me_得到是否可以摆摊
        /// </summary>
        public long Get_baitang(int usid)
        {
            return dal.Get_baitang(usid);
        }
        /// <summary>
        /// me_得到等级
        /// </summary>
        public long GetGrade(int usid)
        {
            return dal.GetGrade(usid);
        }
        /// <summary>
        /// me_得到收割状态
        /// </summary>
        public int Getshoutype(int usid)
        {
            return dal.Getshoutype(usid);
        }
        /// <summary>
        /// me_得到经验
        /// </summary>
        public long Getjingyan(int usid)
        {
            return dal.Getjingyan(usid);
        }
        /// <summary>
        /// me_得到帮忙的经验
        /// </summary>
        public long Get_bmjingyan(int usid)
        {
            return dal.Get_bmjingyan(usid);
        }
        /// <summary>
        /// me_得到播种的经验
        /// </summary>
        public long Get_bzjingyan(int usid)
        {
            return dal.Get_bzjingyan(usid);
        }
        /// <summary>
        /// me_得到使坏的经验
        /// </summary>
        public long Get_shjingyan(int usid)
        {
            return dal.Get_shjingyan(usid);
        }
        /// <summary>
        /// me_得到自己操作的经验
        /// </summary>
        public long Get_zjjingyan(int usid)
        {
            return dal.Get_zjjingyan(usid);
        }
        /// <summary>
        ///  me_得到农场寄语
        /// </summary>
        public string Get_jiyu(int usid)
        {
            return dal.Get_jiyu(usid);
        }
        /// <summary>
        /// me_是否一键除草
        /// </summary>
        public long Getchucao(int usid)
        {
            return dal.Getchucao(usid);
        }
        /// <summary>
        /// me_是否一键浇水
        /// </summary>
        public long Getjiaoshui(int usid)
        {
            return dal.Getjiaoshui(usid);
        }
        /// <summary>
        /// me_是否一键除虫
        /// </summary>
        public long Getchuchong(int usid)
        {
            return dal.Getchuchong(usid);
        }
        /// <summary>
        /// me_开通一键除草
        /// </summary>
        public void Update_chucao_1(int usid)
        {
            dal.Update_chucao_1(usid);
        }
        /// <summary>
        /// me_开通一键浇水
        /// </summary>
        public void Update_jiaoshui_1(int usid)
        {
            dal.Update_jiaoshui_1(usid);
        }
        /// <summary>
        /// me_开通一键除虫
        /// </summary>
        public void Update_chuchong_1(int usid)
        {
            dal.Update_chuchong_1(usid);
        }
        /// <summary>
        /// me_是否一键收获
        /// </summary>
        public long Getshou(int usid)
        {
            return dal.Getshou(usid);
        }
        /// <summary>
        /// me_开通一键收获
        /// </summary>
        public void Update_shouhuo_1(int usid)
        {
            dal.Update_shouhuo_1(usid);
        }
        /// <summary>
        /// me_是否一键铲地
        /// </summary>
        public long Getchandi(int usid)
        {
            return dal.Getchandi(usid);
        }
        /// <summary>
        /// me_开通一键铲地
        /// </summary>
        public void Update_chandi_1(int usid)
        {
            dal.Update_chandi_1(usid);
        }
        /// <summary>
        /// me_是否一键施肥
        /// </summary>
        public long Getshifei(int usid)
        {
            return dal.Getshifei(usid);
        }
        /// <summary>
        /// me_开通一键施肥
        /// </summary>
        public void Update_shifei_1(int usid)
        {
            dal.Update_shifei_1(usid);
        }
        /// <summary>
        /// me_更新金币并记录消费记录
        /// </summary>
        public void UpdateiGold(int ID, string UsName, long iGold, string AcText, int BbTag)
        {
            //更新用户虚拟币
            dal.Update_jinbi(ID, iGold);
            //更新消费记录
            BCW.farm.Model.NC_Goldlog model = new BCW.farm.Model.NC_Goldlog();
            model.BbTag = BbTag;
            model.Types = 30;//农场type30
            model.PUrl = Utils.getPageUrl();//操作的文件名
            model.UsId = ID;
            model.UsName = UsName;
            model.AcGold = iGold;
            model.AfterGold = GetGold(ID);//更新后的币数
            model.AcText = AcText;
            model.AddTime = DateTime.Now;
            new BCW.farm.BLL.NC_Goldlog().Add(model);
        }
        /// <summary>
        /// me_得到签到信息
        /// </summary>
        public BCW.farm.Model.NC_user GetSignData(int ID)
        {
            return dal.GetSignData(ID);
        }
        /// <summary>
        /// me_更新签到信息
        /// </summary>
        public void UpdateSingData(int ID, int SignTotal, int SignKeep)
        {
            dal.UpdateSingData(ID, SignTotal, SignKeep);
        }
        /// <summary>
        /// me_会员排行榜使用
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <param name="strOrder">排序条件</param>
        /// <returns>IList User</returns>
        public IList<BCW.farm.Model.NC_user> GetUsers(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
        {
            return dal.GetUsers(p_pageIndex, p_pageSize, strWhere, strOrder, out p_recordCount);
        }

        /// <summary>
        /// me_取得每页记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList NC_user</returns>
        public IList<BCW.farm.Model.NC_user> GetNC_users(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
        {
            return dal.GetNC_users(p_pageIndex, p_pageSize, strWhere, strOrder, out p_recordCount);
        }

        #endregion  成员方法


        //====================================
    }
}

