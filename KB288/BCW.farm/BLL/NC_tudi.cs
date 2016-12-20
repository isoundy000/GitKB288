using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.farm.Model;
namespace BCW.farm.BLL
{
    /// <summary>
    /// 业务逻辑类NC_tudi 的摘要说明。
    /// </summary>
    public class NC_tudi
    {
        private readonly BCW.farm.DAL.NC_tudi dal = new BCW.farm.DAL.NC_tudi();
        public NC_tudi()
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
        public int Add(BCW.farm.Model.NC_tudi model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(BCW.farm.Model.NC_tudi model)
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
        public BCW.farm.Model.NC_tudi GetNC_tudi(int ID)
        {

            return dal.GetNC_tudi(ID);
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
        /// me_根据字段取数据列表
        /// </summary>
        public DataSet GetList(string strField, string strname, string strWhere)
        {
            return dal.GetList(strField, strname, strWhere);
        }
        /// <summary>
        /// me_该土地是否金土地
        /// </summary>
        public bool Exists_jin(int meid, int tudi)
        {
            return dal.Exists_jin(meid, tudi);
        }
        /// <summary>
        /// me_该土地是否黑土地
        /// </summary>
        public bool Exists_hei(int meid, int tudi)
        {
            return dal.Exists_hei(meid, tudi);
        }
        /// <summary>
        /// me_该土地是否红土地
        /// </summary>
        public bool Exists_hong(int meid, int tudi)
        {
            return dal.Exists_hong(meid, tudi);
        }
        /// <summary>
        /// me_根据usid查询有几块红土地
        /// </summary>
        public BCW.farm.Model.NC_tudi Get_htd(int usid)
        {
            return dal.Get_htd(usid);
        }
        /// <summary>
        /// me_根据usid查询有几块黑土地
        /// </summary>
        public BCW.farm.Model.NC_tudi Get_heitd(int usid)
        {
            return dal.Get_heitd(usid);
        }
        /// <summary>
        /// me_根据usid查询有几块金土地
        /// </summary>
        public BCW.farm.Model.NC_tudi Get_jtd(int usid)
        {
            return dal.Get_jtd(usid);
        }
        /// <summary>
        /// me_更新一条数据
        /// </summary>
        public void Update_1(BCW.farm.Model.NC_tudi model)
        {
            dal.Update_1(model);
        }
        /// <summary>
        /// me_根据usid得到一个对象实体
        /// </summary>
        public BCW.farm.Model.NC_tudi Getusid(int usid)
        {
            return dal.Getusid(usid);
        }
        /// <summary>
        /// me_根据usid查询有几块土地
        /// </summary>
        public BCW.farm.Model.NC_tudi Get_tudinum(int usid)
        {
            return dal.Get_tudinum(usid);
        }
        /// <summary>
        /// me_根据usid查询有几个陷阱
        /// </summary>
        public long Get_xianjing(int usid)
        {
            return dal.Get_xianjing(usid);
        }
        /// <summary>
        ///  me_根据字段修改数据列表
        /// </summary>
        public DataSet update_tudi(string strField, string strWhere)
        {
            return dal.update_tudi(strField, strWhere);
        }
        /// <summary>
        /// me_是否存在土地除草记录
        /// </summary>
        public bool Exists_chucao(int tudi, int UsID)
        {
            return dal.Exists_chucao(tudi, UsID);
        }
        /// <summary>
        /// me_判断土地是否存在可种草
        /// </summary>
        public bool Exists_zhongcao(int tudi, int UsID)
        {
            return dal.Exists_zhongcao(tudi, UsID);
        }
        /// <summary>
        /// me_是否存在土地除草记录_一键
        /// </summary>
        public bool Exists_chucao_1(int usid)
        {
            return dal.Exists_chucao_1(usid);
        }
        /// <summary>
        /// me_是否存在土地浇水记录
        /// </summary>
        public bool Exists_jiaoshui(int tudi, int UsID)
        {
            return dal.Exists_jiaoshui(tudi, UsID);
        }
        /// <summary>
        /// me_是否存在土地浇水记录_一键
        /// </summary>
        public bool Exists_jiaoshui_1(int usid)
        {
            return dal.Exists_jiaoshui_1(usid);
        }
        /// <summary>
        /// me_是否存在土地除虫记录
        /// </summary>
        public bool Exists_chuchong(int tudi, int UsID)
        {
            return dal.Exists_chuchong(tudi, UsID);
        }
        /// <summary>
        /// me_是否存在土地除虫记录_一键
        /// </summary>
        public bool Exists_chuchong_1(int usid)
        {
            return dal.Exists_chuchong_1(usid);
        }
        /// <summary>
        /// me_是否存在一键收获记录_一键
        /// </summary>
        public bool Exists_shouhuo_1(int usid)
        {
            return dal.Exists_shouhuo_1(usid);
        }
        /// <summary>
        /// me_查询有几块土地可以偷取
        /// </summary>
        public BCW.farm.Model.NC_tudi tou_tudinum1(int usid)
        {
            return dal.tou_tudinum1(usid);
        }
        /// <summary>
        /// me_查询已偷过的土地块数
        /// </summary>
        public BCW.farm.Model.NC_tudi tou_tudinum2(int usid, int meid_usid)
        {
            return dal.tou_tudinum2(usid, meid_usid);
        }
        /// <summary>
        /// me_查询有几块土地可以(偷)除草
        /// </summary>
        public BCW.farm.Model.NC_tudi cao_tudinum1(int usid)
        {
            return dal.cao_tudinum1(usid);
        }
        /// <summary>
        /// me_查询有几块土地可以(偷)浇水
        /// </summary>
        public BCW.farm.Model.NC_tudi shui_tudinum1(int usid)
        {
            return dal.shui_tudinum1(usid);
        }
        /// <summary>
        /// me_查询有几块土地可以(偷)除虫
        /// </summary>
        public BCW.farm.Model.NC_tudi chong_tudinum1(int usid)
        {
            return dal.chong_tudinum1(usid);
        }
        /// <summary>
        /// me_查询有几块土地可以(偷)放草
        /// </summary>
        public BCW.farm.Model.NC_tudi fangcao_num1(int usid)
        {
            return dal.fangcao_num1(usid);
        }
        /// <summary>
        /// me_查询有几块土地可以(偷)放虫
        /// </summary>
        public BCW.farm.Model.NC_tudi fangcao_num2(int usid)
        {
            return dal.fangcao_num2(usid);
        }
        /// <summary>
        /// me_是否存在土地记录
        /// </summary>
        public bool Exists_tudi(int tudi, int UsID)
        {
            return dal.Exists_tudi(tudi, UsID);
        }
        /// <summary>
        /// me_是否存在一键铲地记录_一键
        /// </summary>
        public bool Exists_chandi_1(int usid)
        {
            return dal.Exists_chandi_1(usid);
        }
        /// <summary>
        /// me_判断是否自己种的草
        /// </summary>
        public bool Exists_zcao(int tudi, int UsID, int meid)
        {
            return dal.Exists_zcao(tudi, UsID, meid);
        }
        /// <summary>
        /// me_判断是否自己放的虫
        /// </summary>
        public bool Exists_zchong(int tudi, int UsID, int meid)
        {
            return dal.Exists_zchong(tudi, UsID, meid);
        }
        /// <summary>
        /// me_是否存在土地可铲地记录
        /// </summary>
        public bool Exists_chandi(int tudi, int UsID)
        {
            return dal.Exists_chandi(tudi, UsID);
        }
        /// <summary>
        /// me_是否存在一键播种记录_一键
        /// </summary>
        public bool Exists_bozhong_1(int usid)
        {
            return dal.Exists_bozhong_1(usid);
        }
        /// <summary>
        /// me_是否存在土地施肥记录_一键
        /// </summary>
        public bool Exists_shifei_1(int usid)
        {
            return dal.Exists_shifei_1(usid);
        }
        /// <summary>
        /// me_是否存在土地可施肥记录
        /// </summary>
        public bool Exists_shifei(int tudi, int UsID)
        {
            return dal.Exists_shifei(tudi, UsID);
        }
        /// <summary>
        /// me_是否存在陷阱
        /// </summary>
        public bool Exists_xianjing(int tudi, int UsID)
        {
            return dal.Exists_xianjing(tudi, UsID);
        }
        /// <summary>
        /// me_是否存在陷阱
        /// </summary>
        public bool Exists_xianjing2(int tudi, int UsID)
        {
            return dal.Exists_xianjing2(tudi, UsID);
        }
        /// <summary>
        /// me_根据usid查询有几块可以播种的土地
        /// </summary>
        public BCW.farm.Model.NC_tudi Get_tudinum_bz(int usid)
        {
            return dal.Get_tudinum_bz(usid);
        }
        /// <summary>
        /// me_是否存在该ID的该土地可种植
        /// </summary>
        public bool Exists_zhongzhi(int tudi, int UsID)
        {
            return dal.Exists_zhongzhi(tudi, UsID);
        }
        /// <summary>
        /// me_是否存在该ID的该土地可以收获
        /// </summary>
        public bool Exists_shouhuo(int tudi, int UsID)
        {
            return dal.Exists_shouhuo(tudi, UsID);
        }
        /// <summary>
        /// me_根据usid和土地得到一个对象实体
        /// </summary>
        public BCW.farm.Model.NC_tudi Get_td(int usid, int tudi)
        {
            return dal.Get_td(usid, tudi);
        }


        /// <summary>
        /// me_取得每页记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList NC_tudi</returns>
        public IList<BCW.farm.Model.NC_tudi> GetNC_tudis(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
        {
            return dal.GetNC_tudis(p_pageIndex, p_pageSize, strWhere, strOrder, out p_recordCount);
        }
        //====================================
        #endregion  成员方法
    }
}

