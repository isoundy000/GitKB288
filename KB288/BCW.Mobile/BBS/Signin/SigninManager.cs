using System;
using System.Collections.Generic;
using System.Text;
using BCW.Mobile.Protocol;
using BCW.Common;

namespace BCW.Mobile.BBS.Signin
{
    public class SigninManager
    {
        private static SigninManager mInstance;
        private string xmlPath = "../../Controls/finance.xml";

        public static SigninManager Instance()
        {
            if (mInstance == null)
                mInstance = new SigninManager();
            return mInstance;
        }


        public RspSignin UserSignin(ReqSignin _reqData)
        {
            RspSignin _rspData = new RspSignin();

            //验证用户ID格式
            if (_reqData.userId < 0)
            {
                _rspData.header.status = ERequestResult.faild;
                _rspData.header.statusCode = Error.MOBILE_ERROR_CODE.MOBILE_PARAMS_ERROR;
                return _rspData;
            }

            //检查是否登录状态
            if (BCW.Mobile.Common.CheckLogin(_reqData.userId, _reqData.userKey) == 0)
            {
                _rspData.header.status = ERequestResult.faild;
                _rspData.header.statusCode = Error.MOBILE_ERROR_CODE.SYS_USER_NOLOGIN;
                return _rspData;
            }

            BCW.Model.User model = new BCW.BLL.User().GetSignData(_reqData.userId);
            if (string.IsNullOrEmpty(model.SignTime.ToString()))
            {
                model.SignTime = DateTime.Now.AddDays(-1);
            }
            if (model.SignTime > DateTime.Parse(DateTime.Now.ToLongDateString()))
            {
                _rspData.header.status = ERequestResult.faild;
                _rspData.header.statusCode = Error.MOBILE_ERROR_CODE.BBS_SIGNIN_HAS_TODAY;
                return _rspData;
            }
            int SignKeep = 1;
            int SignTotal = model.SignTotal + 1;
            if (model.SignTime >= DateTime.Parse(DateTime.Now.ToLongDateString()).AddDays(-1))
            {
                SignKeep = model.SignKeep + 1;
            }
            //更新签到信息
            new BCW.BLL.User().UpdateSingData(_reqData.userId, SignTotal, SignKeep);
            _rspData.signinRewardStr = BCW.User.Users.GetWinCent(12, _reqData.userId);

            _rspData.header.status = ERequestResult.success;

            //积分操作
            new BCW.User.Cent().UpdateCent(BCW.User.Cent.enumRole.Cent_Sign, _reqData.userId);

            if (SignKeep % 30 == 0)     //连续签到1个月奖励
            {
                _rspData.monthRewardStr = BCW.User.Users.GetWinCent(14, _reqData.userId);
                //动态记录
                new BCW.BLL.Action().Add(_reqData.userId, "在空间连续一个月签到获得奖励");
                //积分操作
                new BCW.User.Cent().UpdateCent(BCW.User.Cent.enumRole.Cent_Sign3, _reqData.userId);
            }
            else if (SignKeep % 7 == 0)
            {
                _rspData.weekRewardStr = BCW.User.Users.GetWinCent(13, _reqData.userId);

                //动态记录
                new BCW.BLL.Action().Add(_reqData.userId, "在空间连续一周签到获得奖励");
                //积分操作
                new BCW.User.Cent().UpdateCent(BCW.User.Cent.enumRole.Cent_Sign2, _reqData.userId);
            }
            else
            {
                //动态记录
                new BCW.BLL.Action().Add(_reqData.userId, "在空间签到获得奖励");
            }

            _rspData.keepDay = SignKeep;

            //银行利息更新
            string ForumSet = new BCW.BLL.User().GetForumSet(_reqData.userId);
            object BankTime = BCW.User.Users.GetForumSet2(ForumSet, 10);
            if (BankTime != null)
            {
                int iDay = 1;
                if (ub.GetSub("FinanceBankType", xmlPath) == "0")
                {
                    iDay = 7;
                }
                DateTime getBankTime = Convert.ToDateTime(BankTime);
                if (DT.TwoDateDiff(DateTime.Now, getBankTime) >= iDay)
                {
                    long iBank = new BCW.BLL.User().GetBank(_reqData.userId);
                    double iTar = Convert.ToDouble(ub.GetSub("FinanceBankTar", xmlPath));
                    long intBank = Convert.ToInt64(iBank * (iTar / 1000));
                    new BCW.BLL.User().UpdateiBank(_reqData.userId, intBank);
                    string GetForumSet = BCW.User.Users.GetForumSetData(ForumSet, DateTime.Now.ToString(), 10);
                    new BCW.BLL.User().UpdateForumSet(_reqData.userId, GetForumSet);
                }
            }

            //VIP成长更新
            BCW.Model.User vip = new BCW.BLL.User().GetVipData(_reqData.userId);
            if (vip != null)
            {
                if (string.IsNullOrEmpty(vip.UpdateDayTime.ToString()) || DT.TwoDateDiff(DateTime.Now, vip.UpdateDayTime) >= 1)
                {
                    if (vip.VipDate > DateTime.Now)
                    {
                        new BCW.BLL.User().UpdateVipGrow(_reqData.userId, vip.VipDayGrow);
                    }
                }
            }
            

            return _rspData;
        }  
    }
}
