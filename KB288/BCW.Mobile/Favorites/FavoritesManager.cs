using System;
using System.Collections.Generic;
using System.Text;
using BCW.Mobile.Protocol;
using System.Data;
using BCW.Common;
using BCW.Mobile.BBS.Thread;

namespace BCW.Mobile.Favorites
{
    public class FavoritesManager
    {
        private static FavoritesManager mInstance;

        public static FavoritesManager Instance()
        {
            if (mInstance == null)
                mInstance = new FavoritesManager();
            return mInstance;
        }

        /// <summary>
        /// 获取收藏数据
        /// </summary>
        /// <param name="_reqData"></param>
        /// <returns></returns>
        public RspFavoritesList GetFavoritesList(ReqFavoritesList _reqData)
        {
            RspFavoritesList _rspData = new RspFavoritesList();

            //验证用户ID格式
            if (_reqData.userId < 0)
            {
                _rspData.header.status = ERequestResult.faild;
                _rspData.header.statusCode = Error.MOBILE_ERROR_CODE.MOBILE_PARAMS_ERROR;
                return _rspData;
            }

            //检查是否登录状态
            if (Common.Common.CheckLogin(_reqData.userId, _reqData.userKey) == 0)
            {
                _rspData.header.status = ERequestResult.faild;
                _rspData.header.statusCode = Error.MOBILE_ERROR_CODE.SYS_USER_NOLOGIN;
                return _rspData;
            }

            //获取动态列表
            string strWhere = string.Empty;
            string strOrder = " order by AddTime desc";
            strWhere += "Types=1 and UsId = " + _reqData.userId;

            if (_reqData.favoritesId > 0)
                strWhere += " and ID<" + _reqData.favoritesId;    //因为是倒序显示，所以是<

            DataSet _ds = new BCW.BLL.Favorites().GetList("TOP 10 ID,Types,NodeId,UsID,Title,PUrl,AddTime", strWhere + strOrder);
            if (_ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < _ds.Tables[0].Rows.Count; i++)
                {

                    string _url = _ds.Tables[0].Rows[i]["PUrl"].ToString();
                    if (!_url.Contains("bid="))
                        continue;

                    int _threadId = 0;
                    try
                    {
                        //截取ID
                        _threadId = int.Parse(_url.Substring(_url.IndexOf("bid=") + 4));
                    }
                    catch
                    {
                        continue;
                    }


                    BCW.Model.Text _text = new BCW.BLL.Text().GetText(_threadId);
                    EssencePostItem _item = EssencePost.AssembleItem(_text);
                    _rspData.lstThread.Add(_item);

                    //检查是否到底
                    if (i == _ds.Tables[0].Rows.Count - 1)
                    {
                        if (strWhere.Contains("1=1") == false)
                            strWhere += " and 1=1";
                        DataSet _dsCheck = new BCW.BLL.Favorites().GetList(" TOP 10 ID,Types,NodeId,UsID,Title,PUrl,AddTime ", strWhere.Replace("1=1", "ID<" + _reqData.favoritesId) + strOrder);
                        _rspData.isFinish = _dsCheck.Tables[0].Rows.Count <= 0;
                    }
                }
            }

            _rspData.serverTime = Common.Common.GetLongTime(DateTime.Now);
            _rspData.header.status = ERequestResult.success;
            return _rspData;
        }

        public RspAddFavorites AddFavorites(ReqAddFavorites _reqData)
        {
            RspAddFavorites _rspData = new RspAddFavorites();

            //验证用户ID格式
            if (_reqData.userId < 0)
            {
                _rspData.header.status = ERequestResult.faild;
                _rspData.header.statusCode = Error.MOBILE_ERROR_CODE.MOBILE_PARAMS_ERROR;
                return _rspData;
            }

            //检查是否登录状态
            if (Common.Common.CheckLogin(_reqData.userId, _reqData.userKey) == 0)
            {
                _rspData.header.status = ERequestResult.faild;
                _rspData.header.statusCode = Error.MOBILE_ERROR_CODE.SYS_USER_NOLOGIN;
                return _rspData;
            }

            //检查帖子有效性
            BCW.Model.Text model = new BCW.BLL.Text().GetText(_reqData.threadId);//GetTextMe
            if (model == null)
            {
                _rspData.header.status = ERequestResult.faild;
                _rspData.header.statusCode = Error.MOBILE_ERROR_CODE.BBS_THREAD_NOT_FOUND;
                return _rspData;
            }

            //是否存在此收藏记录
            DataSet _ds = null;
            _ds = new BCW.BLL.Favorites().GetList("TOP 1 ID", string.Format(" PUrl like '%bid={0}' and Types=1 and UsID={1}", _reqData.threadId, _reqData.userId));
            if (_ds != null && _ds.Tables[0].Rows.Count > 0)
            {
                _rspData.header.status = ERequestResult.faild;
                _rspData.header.statusCode = Error.MOBILE_ERROR_CODE.BBS_FAVORITES_EXISTS;
                return _rspData;
            }


            //如果没有默认收藏夹，先帮其创建一个再进行收藏
            int _groupId = 0;

            _ds = new BCW.BLL.Favgroup().GetList("Top 1 ID,Title", " UsID=" + _reqData.userId + " and Title='默认收藏'");
            if (_ds == null || _ds.Tables[0].Rows.Count == 0)
            {
                BCW.Model.Favgroup _groupModel = new BCW.Model.Favgroup();
                _groupModel.Types = 0;
                _groupModel.Title = "默认收藏";
                _groupModel.UsID = _reqData.userId;
                _groupModel.Paixu = 0;
                _groupModel.AddTime = DateTime.Now;
                _groupId = new BCW.BLL.Favgroup().Add(_groupModel);
            }
            else
                _groupId = int.Parse(_ds.Tables[0].Rows[0]["ID"].ToString());



            //保存收藏
            BCW.Model.Favorites _favoritesModel = new BCW.Model.Favorites();
            _favoritesModel.Types = 1;
            _favoritesModel.NodeId = _groupId;
            _favoritesModel.UsID = _reqData.userId;
            _favoritesModel.Title = model.Title;
            _favoritesModel.PUrl = string.Format("/bbs/topic.aspx?forumid={0}&amp;bid={1}", model.ForumId, _reqData.threadId);
            _favoritesModel.AddTime = DateTime.Now;
            int favid = new BCW.BLL.Favorites().Add(_favoritesModel);

            _rspData.threadId = _reqData.threadId;
            _rspData.isFavorites = true;
            _rspData.header.status = ERequestResult.success;
            return _rspData;
        }


        public RspDelFavorites DelFavorites(ReqDelFavorites _reqData)
        {
            RspDelFavorites _rspData = new RspDelFavorites();

            //验证用户ID格式
            if (_reqData.userId < 0)
            {
                _rspData.header.status = ERequestResult.faild;
                _rspData.header.statusCode = Error.MOBILE_ERROR_CODE.MOBILE_PARAMS_ERROR;
                return _rspData;
            }

            //检查是否登录状态
            if (Common.Common.CheckLogin(_reqData.userId, _reqData.userKey) == 0)
            {
                _rspData.header.status = ERequestResult.faild;
                _rspData.header.statusCode = Error.MOBILE_ERROR_CODE.SYS_USER_NOLOGIN;
                return _rspData;
            }

            //检查帖子有效性
            BCW.Model.Text model = new BCW.BLL.Text().GetText(_reqData.userId);//GetTextMe
            if (model == null)
            {
                _rspData.header.status = ERequestResult.faild;
                _rspData.header.statusCode = Error.MOBILE_ERROR_CODE.BBS_THREAD_NOT_FOUND;
                return _rspData;
            }

            //是否存在此收藏记录
            DataSet _ds = new BCW.BLL.Favorites().GetList("TOP 1 ID", string.Format(" PUrl like '%bid={0}' and Types=1 and UsID={1}", _reqData.threadId, _reqData.userId));
            if (_ds == null || _ds.Tables[0].Rows.Count == 0)
            {
                _rspData.header.status = ERequestResult.faild;
                _rspData.header.statusCode = Error.MOBILE_ERROR_CODE.BBS_FAVORITES_NOT_EXISTS;
                return _rspData;
            }

            int _favoritesId = int.Parse(_ds.Tables[0].Rows[0]["ID"].ToString());

            new BCW.BLL.Favorites().Delete(_favoritesId);

            _rspData.threadId = _reqData.threadId;
            _rspData.isFavorites = false;
            _rspData.header.status = ERequestResult.success;
            return _rspData;
        }

    }
}