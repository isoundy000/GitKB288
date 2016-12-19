using System;
using System.Collections.Generic;
using System.Text;
using BCW.Mobile.Protocol;
using BCW.Common;
using System.Data;

namespace BCW.Mobile.Action
{
    public class ActionManager
    {
        private static ActionManager mInstance;

        public static ActionManager Instance()
        {
            if (mInstance == null)
                mInstance = new ActionManager();
            return mInstance;
        }

        public rspAction GetActionData(reqAction _reqData)
        {
            rspAction _rspData = new rspAction();

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

            BCW.Model.User model = new BCW.BLL.User().GetBasic(_reqData.userId);
            _rspData.totalClick = model.Click + "/今天" + new BCW.BLL.Visitor().GetTodayCount(_reqData.userId) + "/" + new BCW.BLL.User().GetClickTop(_reqData.userId) + "名";
            _rspData.onLineTime = BCW.User.Users.ChangeDayff(model.OnTime);

            //获取动态列表
            string strWhere = string.Empty;
            string strOrder = " order by AddTime desc";
            strWhere += "UsId = "+_reqData.userId;

            if (_reqData.actionId > 0)
                strWhere += " and ID<" + _reqData.actionId;    //因为是倒序显示，所以是<

            DataSet _ds = new BCW.BLL.Action().GetList("TOP 10 ID,Types,UsId,UsName,Notes,AddTime", strWhere+ strOrder);
            if (_ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < _ds.Tables[0].Rows.Count; i++)
                {
                    ActionData _actionData = new ActionData();
                    _actionData.actionId = int.Parse(_ds.Tables[0].Rows[i]["ID"].ToString());
                    _actionData.userId = int.Parse(_ds.Tables[0].Rows[i]["UsId"].ToString());
                    _actionData.userName =_ds.Tables[0].Rows[i]["UsName"].ToString();
                    _actionData.content = Out.SysUBB(_ds.Tables[0].Rows[i]["Notes"].ToString());
                    _actionData.addTime = Common.Common.GetLongTime(DateTime.Parse(_ds.Tables[0].Rows[i]["AddTime"].ToString()));

                    _rspData.lstAction.Add(_actionData);


                    //检查是否到底
                    if (i == _ds.Tables[0].Rows.Count - 1)
                    {
                        if (strWhere.Contains("1=1") == false)
                            strWhere += " and 1=1";
                        DataSet _dsCheck = new BCW.BLL.Action().GetList(" TOP 10 ID,Types,UsId,UsName,Notes,AddTime ", strWhere.Replace("1=1", "ID<" + _reqData.actionId) + strOrder);
                        _rspData.isFinish = _dsCheck.Tables[0].Rows.Count <= 0;                       
                    }
                }
            }

            _rspData.serverTime = Common.Common.GetLongTime(DateTime.Now);
            _rspData.header.status = ERequestResult.success;
            return _rspData;
        }
    }
}
