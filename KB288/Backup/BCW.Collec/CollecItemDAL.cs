using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using BCW.Data;
using BCW.Common;
namespace BCW.DAL.Collec
{
    /// <summary>
    /// 数据访问类CollecItem。
    /// </summary>
    public class CollecItem
    {
        public CollecItem()
        { }
        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("ID", "tb_CollecItem");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_CollecItem");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(BCW.Model.Collec.CollecItem model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_CollecItem(");
            strSql.Append("ItemName,Types,NodeId,WebEncode,WebName,WebUrl,ItemRemark,Script_Html,CollecNum,IsSaveImg,IsDesc,State)");
            strSql.Append(" values (");
            strSql.Append("@ItemName,@Types,@NodeId,@WebEncode,@WebName,@WebUrl,@ItemRemark,@Script_Html,@CollecNum,@IsSaveImg,@IsDesc,@State)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@ItemName", SqlDbType.NVarChar,50),
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@NodeId", SqlDbType.Int,4),
					new SqlParameter("@WebEncode", SqlDbType.Int,4),
					new SqlParameter("@WebName", SqlDbType.NVarChar,50),
					new SqlParameter("@WebUrl", SqlDbType.NVarChar,50),
					new SqlParameter("@ItemRemark", SqlDbType.NVarChar,50),
					new SqlParameter("@Script_Html", SqlDbType.NVarChar,200),
					new SqlParameter("@CollecNum", SqlDbType.Int,4),
					new SqlParameter("@IsSaveImg", SqlDbType.Int,4),
					new SqlParameter("@IsDesc", SqlDbType.Int,4),
					new SqlParameter("@State", SqlDbType.Int,4)};
            parameters[0].Value = model.ItemName;
            parameters[1].Value = model.Types;
            parameters[2].Value = model.NodeId;
            parameters[3].Value = model.WebEncode;
            parameters[4].Value = model.WebName;
            parameters[5].Value = model.WebUrl;
            parameters[6].Value = model.ItemRemark;
            parameters[7].Value = model.Script_Html;
            parameters[8].Value = model.CollecNum;
            parameters[9].Value = model.IsSaveImg;
            parameters[10].Value = model.IsDesc;
            parameters[11].Value = model.State;

            object obj = SqlHelper.GetSingle(strSql.ToString(), parameters);
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(obj);
            }
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(BCW.Model.Collec.CollecItem model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_CollecItem set ");
            strSql.Append("ItemName=@ItemName,");
            strSql.Append("Types=@Types,");
            strSql.Append("NodeId=@NodeId,");
            strSql.Append("WebEncode=@WebEncode,");
            strSql.Append("WebName=@WebName,");
            strSql.Append("WebUrl=@WebUrl,");
            strSql.Append("ItemRemark=@ItemRemark,");
            strSql.Append("ListUrl=@ListUrl,");
            strSql.Append("ListStart=@ListStart,");
            strSql.Append("ListEnd=@ListEnd,");
            strSql.Append("LinkStart=@LinkStart,");
            strSql.Append("LinkEnd=@LinkEnd,");
            strSql.Append("TitleStart=@TitleStart,");
            strSql.Append("TitleEnd=@TitleEnd,");
            strSql.Append("KeyWordStart=@KeyWordStart,");
            strSql.Append("KeyWordEnd=@KeyWordEnd,");
            strSql.Append("DateRegex=@DateRegex,");
            strSql.Append("NextListRegex=@NextListRegex,");
            strSql.Append("ContentStart=@ContentStart,");
            strSql.Append("ContentEnd=@ContentEnd,");
            strSql.Append("RemoveBodyStart=@RemoveBodyStart,");
            strSql.Append("RemoveBodyEnd=@RemoveBodyEnd,");
            strSql.Append("RemoveTitle=@RemoveTitle,");
            strSql.Append("RemoveContent=@RemoveContent,");
            strSql.Append("NextPageRegex=@NextPageRegex,");
            strSql.Append("Script_Html=@Script_Html,");
            strSql.Append("CollecNum=@CollecNum,");
            strSql.Append("IsSaveImg=@IsSaveImg,");
            strSql.Append("IsDesc=@IsDesc");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@ItemName", SqlDbType.NVarChar,50),
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@NodeId", SqlDbType.Int,4),
					new SqlParameter("@WebEncode", SqlDbType.Int,4),
					new SqlParameter("@WebName", SqlDbType.NVarChar,50),
					new SqlParameter("@WebUrl", SqlDbType.NVarChar,50),
					new SqlParameter("@ItemRemark", SqlDbType.NVarChar,50),
					new SqlParameter("@ListUrl", SqlDbType.NVarChar,300),
					new SqlParameter("@ListStart", SqlDbType.NVarChar,300),
					new SqlParameter("@ListEnd", SqlDbType.NVarChar,300),
					new SqlParameter("@LinkStart", SqlDbType.NVarChar,300),
					new SqlParameter("@LinkEnd", SqlDbType.NVarChar,50),
					new SqlParameter("@TitleStart", SqlDbType.NVarChar,50),
					new SqlParameter("@TitleEnd", SqlDbType.NVarChar,50),
					new SqlParameter("@KeyWordStart", SqlDbType.NVarChar,300),
					new SqlParameter("@KeyWordEnd", SqlDbType.NVarChar,300),
					new SqlParameter("@DateRegex", SqlDbType.NVarChar,300),
					new SqlParameter("@NextListRegex", SqlDbType.NVarChar,300),
					new SqlParameter("@ContentStart", SqlDbType.NVarChar,50),
					new SqlParameter("@ContentEnd", SqlDbType.NText),
					new SqlParameter("@RemoveBodyStart", SqlDbType.NVarChar,300),
					new SqlParameter("@RemoveBodyEnd", SqlDbType.NVarChar,300),
					new SqlParameter("@RemoveTitle", SqlDbType.NVarChar,300),
					new SqlParameter("@RemoveContent", SqlDbType.NVarChar,300),
					new SqlParameter("@NextPageRegex", SqlDbType.NVarChar,300),
					new SqlParameter("@Script_Html", SqlDbType.NVarChar,200),
					new SqlParameter("@CollecNum", SqlDbType.Int,4),
					new SqlParameter("@IsSaveImg", SqlDbType.Int,4),
					new SqlParameter("@IsDesc", SqlDbType.Int,4)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.ItemName;
            parameters[2].Value = model.Types;
            parameters[3].Value = model.NodeId;
            parameters[4].Value = model.WebEncode;
            parameters[5].Value = model.WebName;
            parameters[6].Value = model.WebUrl;
            parameters[7].Value = model.ItemRemark;
            parameters[8].Value = model.ListUrl;
            parameters[9].Value = model.ListStart;
            parameters[10].Value = model.ListEnd;
            parameters[11].Value = model.LinkStart;
            parameters[12].Value = model.LinkEnd;
            parameters[13].Value = model.TitleStart;
            parameters[14].Value = model.TitleEnd;
            parameters[15].Value = model.KeyWordStart;
            parameters[16].Value = model.KeyWordEnd;
            parameters[17].Value = model.DateRegex;
            parameters[18].Value = model.NextListRegex;
            parameters[19].Value = model.ContentStart;
            parameters[20].Value = model.ContentEnd;
            parameters[21].Value = model.RemoveBodyStart;
            parameters[22].Value = model.RemoveBodyEnd;
            parameters[23].Value = model.RemoveTitle;
            parameters[24].Value = model.RemoveContent;
            parameters[25].Value = model.NextPageRegex;
            parameters[26].Value = model.Script_Html;
            parameters[27].Value = model.CollecNum;
            parameters[28].Value = model.IsSaveImg;
            parameters[29].Value = model.IsDesc;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 列表设置
        /// </summary>
        public void UpdateListSet(BCW.Model.Collec.CollecItem model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_CollecItem set ");
            strSql.Append("ListUrl=@ListUrl,");
            strSql.Append("ListStart=@ListStart,");
            strSql.Append("ListEnd=@ListEnd,");
            strSql.Append("NextListRegex=@NextListRegex");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@ListUrl", SqlDbType.NVarChar,300),
					new SqlParameter("@ListStart", SqlDbType.NVarChar,300),
					new SqlParameter("@ListEnd", SqlDbType.NVarChar,300),
					new SqlParameter("@NextListRegex", SqlDbType.NVarChar,300)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.ListUrl;
            parameters[2].Value = model.ListStart;
            parameters[3].Value = model.ListEnd;
            parameters[4].Value = model.NextListRegex;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 链接设置
        /// </summary>
        public void UpdateLinkSet(BCW.Model.Collec.CollecItem model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_CollecItem set ");
            strSql.Append("LinkStart=@LinkStart,");
            strSql.Append("LinkEnd=@LinkEnd");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@LinkStart", SqlDbType.NVarChar,300),
					new SqlParameter("@LinkEnd", SqlDbType.NVarChar,300)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.LinkStart;
            parameters[2].Value = model.LinkEnd;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 正文设置
        /// </summary>
        public void UpdateContentSet(BCW.Model.Collec.CollecItem model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_CollecItem set ");
            strSql.Append("TitleStart=@TitleStart,");
            strSql.Append("TitleEnd=@TitleEnd,");
            strSql.Append("KeyWordStart=@KeyWordStart,");
            strSql.Append("KeyWordEnd=@KeyWordEnd,");
            strSql.Append("DateRegex=@DateRegex,");
            strSql.Append("ContentStart=@ContentStart,");
            strSql.Append("ContentEnd=@ContentEnd,");
            strSql.Append("RemoveBodyStart=@RemoveBodyStart,");
            strSql.Append("RemoveBodyEnd=@RemoveBodyEnd,");
            strSql.Append("RemoveTitle=@RemoveTitle,");
            strSql.Append("RemoveContent=@RemoveContent,");
            strSql.Append("NextPageRegex=@NextPageRegex");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@TitleStart", SqlDbType.NVarChar,50),
					new SqlParameter("@TitleEnd", SqlDbType.NVarChar,50),
					new SqlParameter("@KeyWordStart", SqlDbType.NVarChar,300),
					new SqlParameter("@KeyWordEnd", SqlDbType.NVarChar,300),
					new SqlParameter("@DateRegex", SqlDbType.NVarChar,300),
					new SqlParameter("@ContentStart", SqlDbType.NVarChar,50),
					new SqlParameter("@ContentEnd", SqlDbType.NText),
					new SqlParameter("@RemoveBodyStart", SqlDbType.NVarChar,300),
					new SqlParameter("@RemoveBodyEnd", SqlDbType.NVarChar,300),
					new SqlParameter("@RemoveTitle", SqlDbType.NVarChar,300),
					new SqlParameter("@RemoveContent", SqlDbType.NVarChar,300),
					new SqlParameter("@NextPageRegex", SqlDbType.NVarChar,300)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.TitleStart;
            parameters[2].Value = model.TitleEnd;
            parameters[3].Value = model.KeyWordStart;
            parameters[4].Value = model.KeyWordEnd;
            parameters[5].Value = model.DateRegex;
            parameters[6].Value = model.ContentStart;
            parameters[7].Value = model.ContentEnd;
            parameters[8].Value = model.RemoveBodyStart;
            parameters[9].Value = model.RemoveBodyEnd;
            parameters[10].Value = model.RemoveTitle;
            parameters[11].Value = model.RemoveContent;
            parameters[12].Value = model.NextPageRegex;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新1可用/0不可用
        /// </summary>
        public void UpdateState(int ID, int State)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_CollecItem set ");
            strSql.Append("State=@State");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@State", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = State;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_CollecItem ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.Model.Collec.CollecItem GetCollecItem(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,ItemName,Types,NodeId,WebEncode,WebName,WebUrl,ItemRemark,ListUrl,ListStart,ListEnd,LinkStart,LinkEnd,TitleStart,TitleEnd,KeyWordStart,KeyWordEnd,DateRegex,NextListRegex,ContentStart,ContentEnd,RemoveBodyStart,RemoveBodyEnd,RemoveTitle,RemoveContent,NextPageRegex,Script_Html,CollecNum,IsSaveImg,IsDesc,State from tb_CollecItem ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.Model.Collec.CollecItem model = new BCW.Model.Collec.CollecItem();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.ItemName = reader.GetString(1);
                    model.Types = reader.GetInt32(2);
                    model.NodeId = reader.GetInt32(3);
                    model.WebEncode = reader.GetInt32(4);
                    model.WebName = reader.GetString(5);
                    model.WebUrl = reader.GetString(6);
                    model.ItemRemark = reader.GetString(7);
                    if (!reader.IsDBNull(8))
                        model.ListUrl = reader.GetString(8);
                    if (!reader.IsDBNull(9))
                        model.ListStart = reader.GetString(9);
                    if (!reader.IsDBNull(10))
                        model.ListEnd = reader.GetString(10);
                    if (!reader.IsDBNull(11))
                        model.LinkStart = reader.GetString(11);
                    if (!reader.IsDBNull(12))
                        model.LinkEnd = reader.GetString(12);
                    if (!reader.IsDBNull(13))
                        model.TitleStart = reader.GetString(13);
                    if (!reader.IsDBNull(14))
                        model.TitleEnd = reader.GetString(14);
                    if (!reader.IsDBNull(15))
                        model.KeyWordStart = reader.GetString(15);
                    if (!reader.IsDBNull(16))
                        model.KeyWordEnd = reader.GetString(16);
                    if (!reader.IsDBNull(17))
                        model.DateRegex = reader.GetString(17);
                    if (!reader.IsDBNull(18))
                        model.NextListRegex = reader.GetString(18);
                    if (!reader.IsDBNull(19))
                        model.ContentStart = reader.GetString(19);
                    if (!reader.IsDBNull(20))
                        model.ContentEnd = reader.GetString(20);

                    if (!reader.IsDBNull(21))
                        model.RemoveBodyStart = reader.GetString(21);
                    else
                        model.RemoveBodyStart = "";

                    if (!reader.IsDBNull(22))
                        model.RemoveBodyEnd = reader.GetString(22);
                    else
                        model.RemoveBodyEnd = "";

                    if (!reader.IsDBNull(23))
                        model.RemoveTitle = reader.GetString(23);
                    else
                        model.RemoveTitle = "";

                    if (!reader.IsDBNull(24))
                        model.RemoveContent = reader.GetString(24);
                    else
                        model.RemoveContent = "";

                    if (!reader.IsDBNull(25))
                        model.NextPageRegex = reader.GetString(25);
                    if (!reader.IsDBNull(26))
                        model.Script_Html = reader.GetString(26);
                    model.CollecNum = reader.GetInt32(27);
                    model.IsSaveImg = reader.GetInt32(28);
                    model.IsDesc = reader.GetInt32(29);
                    model.State = reader.GetInt32(30);
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 得到一个WebEncode
        /// </summary>
        public int GetWebEncode(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 WebEncode from tb_CollecItem ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    return reader.GetInt32(0);

                }
                else
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// 根据字段取数据列表
        /// </summary>
        public DataSet GetList(string strField, string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  " + strField + " ");
            strSql.Append(" FROM tb_CollecItem ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return SqlHelper.Query(strSql.ToString());
        }

        /// <summary>
        /// 取得每页记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList CollecItem</returns>
        public IList<BCW.Model.Collec.CollecItem> GetCollecItems(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.Model.Collec.CollecItem> listCollecItems = new List<BCW.Model.Collec.CollecItem>();
            string sTable = "tb_CollecItem";
            string sPkey = "id";
            string sField = "ID,ItemName,ItemRemark,State";
            string sCondition = strWhere;
            string sOrder = "ID Desc";
            int iSCounts = 0;
            using (SqlDataReader reader = SqlHelper.RunProcedureMe(sTable, sPkey, sField, p_pageIndex, p_pageSize, sCondition, sOrder, iSCounts, out p_recordCount))
            {
                //计算总页数
                if (p_recordCount > 0)
                {
                    int pageCount = BasePage.CalcPageCount(p_recordCount, p_pageSize, ref p_pageIndex);
                }
                else
                {
                    return listCollecItems;
                }
                while (reader.Read())
                {
                    BCW.Model.Collec.CollecItem objCollecItem = new BCW.Model.Collec.CollecItem();
                    objCollecItem.ID = reader.GetInt32(0);
                    objCollecItem.ItemName = reader.GetString(1);
                    objCollecItem.ItemRemark = reader.GetString(2);
                    objCollecItem.State = reader.GetInt32(3);
                    listCollecItems.Add(objCollecItem);
                }
            }
            return listCollecItems;
        }

        #endregion  成员方法
    }
}

