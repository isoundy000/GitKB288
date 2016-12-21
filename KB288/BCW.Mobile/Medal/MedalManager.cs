using System;
using System.Collections.Generic;
using System.Text;
using BCW.Mobile.Protocol;
using System.Data;
using BCW.Common;

namespace BCW.Mobile.Medal
{
    public class MedalManager
    {
        private static MedalManager mInstance;

        public static MedalManager Instance()
        {
            if (mInstance == null)
                mInstance = new MedalManager();
            return mInstance;
        }

        /// <summary>
        /// 获取某个用户勋章
        /// </summary>
        /// <returns></returns>
        public RspMedalLog GetUserMedal(ReqMedalLog _reqData)
        {
            RspMedalLog _rspData = new RspMedalLog();

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


            //获取勋章列表
            string strWhere = string.Empty;
            string strOrder = " order by AddTime desc";
            strWhere += "UsId = " + _reqData.userId;

            if (_reqData.medalId > 0)
                strWhere += " and ID<" + _reqData.medalId;    //因为是倒序显示，所以是<


            DataSet _ds = new BCW.BLL.Medalget().GetList("TOP 10 ID,Types,UsId,MedalId,Notes,AddTime", strWhere + strOrder);
            if (_ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < _ds.Tables[0].Rows.Count; i++)
                {
                    MedalData _medalData = new MedalData();
                    _medalData.medalId = int.Parse(_ds.Tables[0].Rows[i]["ID"].ToString());
                    _medalData.userId = int.Parse(_ds.Tables[0].Rows[i]["UsId"].ToString());
                    _medalData.userName = new BCW.BLL.User().GetUsName(_medalData.userId);
                    _medalData.content = Out.SysUBB(_ds.Tables[0].Rows[i]["Notes"].ToString());
                    _medalData.addTime = Common.Common.GetLongTime(DateTime.Parse(_ds.Tables[0].Rows[i]["AddTime"].ToString()));

                    _rspData.lstMedal.Add(_medalData);


                    //检查是否到底
                    if (i == _ds.Tables[0].Rows.Count - 1)
                    {
                        if (strWhere.Contains("1=1") == false)
                            strWhere += " and 1=1";
                        DataSet _dsCheck = new BCW.BLL.Medalget().GetList(" TOP 10 ID,Types,UsId,MedalId,Notes,AddTime ", strWhere.Replace("1=1", "ID<" + _reqData.medalId) + strOrder);
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
