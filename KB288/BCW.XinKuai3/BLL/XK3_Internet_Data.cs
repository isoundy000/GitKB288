using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.XinKuai3.Model;
namespace BCW.XinKuai3.BLL
{
    /// <summary>
    /// 业务逻辑类XK3_Internet_Data 的摘要说明。
    /// </summary>
    public class XK3_Internet_Data
    {
        private readonly BCW.XinKuai3.DAL.XK3_Internet_Data dal = new BCW.XinKuai3.DAL.XK3_Internet_Data();
        public XK3_Internet_Data()
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
        public int Add(BCW.XinKuai3.Model.XK3_Internet_Data model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(BCW.XinKuai3.Model.XK3_Internet_Data model)
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
        public BCW.XinKuai3.Model.XK3_Internet_Data GetXK3_Internet_Data(int ID)
        {

            return dal.GetXK3_Internet_Data(ID);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.XinKuai3.Model.XK3_Internet_Data GetXK3_Internet_Data2(int ID)
        {
            return dal.GetXK3_Internet_Data2(ID);
        }

        /// <summary>
        /// 根据字段取数据列表
        /// </summary>
        public DataSet GetList(string strField, string strWhere)
        {
            return dal.GetList(strField, strWhere);
        }



        //============================================
        /// <summary>
        /// 根据字段取数据列表2
        /// </summary>
        public DataSet GetList2(string strField, string strWhere)
        {
            return dal.GetList2(strField, strWhere);
        }
        /// <summary>
        /// me_得到一个开奖号码对象实体
        /// </summary>
        public BCW.XinKuai3.Model.XK3_Internet_Data Getxk3kainum(string _where)
        {
            return dal.Getxk3kainum(_where);
        }        /// <summary>
                 /// me_后台人工开奖，根据开奖号码对应的该条开奖数据22
                 /// </summary>
        public BCW.XinKuai3.Model.XK3_Internet_Data Getxk3all_num2(string _where)
        {
            return dal.Getxk3all_num2(_where);
        }
        /// <summary>
        /// me_得到最后一期对象实体
        /// </summary>
        public BCW.XinKuai3.Model.XK3_Internet_Data Getxk3listLast(string _where)
        {
            return dal.Getxk3listLast(_where);
        }
        /// <summary>
        /// me_增加一条网上开奖数据
        /// </summary>
        public int Add_num(BCW.XinKuai3.Model.XK3_Internet_Data model)
        {
            return dal.Add_num(model);
        }
        /// <summary>
        /// me_更新一条网上开奖数据
        /// </summary>
        public void update_num2(BCW.XinKuai3.Model.XK3_Internet_Data model)
        {
            dal.update_num2(model);
        }
        /// <summary>
        /// me_是否存在该开奖记录
        /// </summary>
        public bool Exists_num(string Lottery_issue)
        {
            return dal.Exists_num(Lottery_issue);
        }
        /// <summary>
        /// me_获奖程序--获取开奖结果，更新中奖数据
        /// </summary>
        public void Update_num(BCW.XinKuai3.Model.XK3_Internet_Data model)
        {
            dal.Update_num(model);
        }
        /// <summary>
        /// me_后台---获取开奖结果，更新中奖数据
        /// </summary>
        public void Update_num2(BCW.XinKuai3.Model.XK3_Internet_Data model)
        {
            dal.Update_num2(model);
        }
        /// <summary>
        /// me_后台---获取开奖结果，更新中奖数据
        /// </summary>
        public void Update_num3(BCW.XinKuai3.Model.XK3_Internet_Data model)
        {
            dal.Update_num3(model);
        }
        /// <summary>
        /// me_开奖后，获取最后一期
        /// </summary>
        public BCW.XinKuai3.Model.XK3_Internet_Data Getxk3listLast2()
        {
            return dal.Getxk3listLast2();
        }
        /// <summary>
        /// me_变动赔率
        /// </summary>
        public BCW.XinKuai3.Model.XK3_Internet_Data Getxk3listLast3()
        {
            return dal.Getxk3listLast3();
        }
        /// <summary>
        /// me_计算和值出现次数并排序
        /// </summary>
        public IList<BCW.XinKuai3.Model.XK3_Internet_Data> Getxk3all(string _where)
        {
            return dal.Getxk3all(_where);
        }
        /// <summary>
        /// me_计算二同号出现的次数
        /// </summary>
        public BCW.XinKuai3.Model.XK3_Internet_Data Getxk3Two_Same_All(string _where)
        {
            return dal.Getxk3Two_Same_All(_where);
        }
        /// <summary>
        /// me_计算二不同号出现的次数
        /// </summary>
        public BCW.XinKuai3.Model.XK3_Internet_Data Getxk3Two_dissame(string _where)
        {
            return dal.Getxk3Two_dissame(_where);
        }
        /// <summary>
        /// me_计算三同号出现的次数
        /// </summary>
        public BCW.XinKuai3.Model.XK3_Internet_Data Getxk3Three_Same_All(string _where)
        {
            return dal.Getxk3Three_Same_All(_where);
        }
        /// <summary>
        /// me_计算三不同号出现的次数
        /// </summary>
        public BCW.XinKuai3.Model.XK3_Internet_Data Getxk3where_Three_Same_Not(string _where)
        {
            return dal.Getxk3where_Three_Same_Not(_where);
        }
        /// <summary>
        /// me_计算三连号出现的次数
        /// </summary>
        public BCW.XinKuai3.Model.XK3_Internet_Data Getxk3where_Three_Continue_All(string _where)
        {
            return dal.Getxk3where_Three_Continue_All(_where);
        }
        /// <summary>
        /// me_计算大出现的次数
        /// </summary>
        public BCW.XinKuai3.Model.XK3_Internet_Data Getxk3da(string _where)
        {
            return dal.Getxk3da(_where);
        }
        /// <summary>
        /// me_计算小出现的次数
        /// </summary>
        public BCW.XinKuai3.Model.XK3_Internet_Data Getxk3xiao(string _where)
        {
            return dal.Getxk3xiao(_where);
        }
        /// <summary>
        /// me_计算双出现的次数
        /// </summary>
        public BCW.XinKuai3.Model.XK3_Internet_Data Getxk3shuang(string _where)
        {
            return dal.Getxk3shuang(_where);
        }
        /// <summary>
        /// me_计算单出现的次数
        /// </summary>
        public BCW.XinKuai3.Model.XK3_Internet_Data Getxk3dan(string _where)
        {
            return dal.Getxk3dan(_where);
        }
        /// <summary>
        /// me_计算通吃出现的次数
        /// </summary>
        public BCW.XinKuai3.Model.XK3_Internet_Data Getxk3tongchi(string _where)
        {
            return dal.Getxk3tongchi(_where);
        }

        /// <summary>
        /// me_获取最近10期开奖情况
        /// </summary>
        public IList<BCW.XinKuai3.Model.XK3_Internet_Data> Getxk3listTop(string _where)
        {
            return dal.Getxk3listTop(_where);
        }
        //============================================




        /// <summary>
        /// 取得每页记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList XK3_Internet_Data</returns>
        public IList<BCW.XinKuai3.Model.XK3_Internet_Data> GetXK3_Internet_Datas(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetXK3_Internet_Datas(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }

        #endregion  成员方法
    }
}

