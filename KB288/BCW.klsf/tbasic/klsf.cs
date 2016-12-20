using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;

namespace BCW.tbasic
{
    public class klsf
    {
        /// <summary>
        /// 更新期数
        /// </summary>
        public void klsfLISTPAGE()
        {
            int klsfId = 0;
            DateTime EndTime = DateTime.Now;
            try
            {
                string stage1 = GetStage1(); //通过方法1取期数
                string stage2 = GetStage2(); //通过方法2取期数
                Model.klsflist model = new Model.klsflist();  //创建一个对象来存储信息
                Model.klsflist modeL = new Model.klsflist();//取数据库内的最新一条数据
                Model.klsflist Last = new Model.klsflist(); //取数据库内最新一条有结果的数据
                if (new BLL.klsflist().Exists(1))
                {
                    modeL= new BLL.klsflist().GetklsflistLast();
                    Last = new BLL.klsflist().GetklsflistLast2();
                }
                klsfId = Convert.ToInt32(stage1);
                string klsftimes = klsfstop1(); //通过方法取结束时间
                DateTime time = Convert.ToDateTime(klsftimes);
                if (modeL.klsfId - Last.klsfId < 3|| !(new BLL.klsflist().Exists(1)))
                //if (true)
                {       
                    EndTime = Convert.ToDateTime(klsftimes);
                    model.klsfId = klsfId;
                    model.Result = "";
                    model.Notes = "";
                    model.EndTime = EndTime.AddMinutes(-2);
                    model.State = 0;
                    bool s = new BLL.klsflist().ExistsklsfId(model.klsfId);

                    if (s == false)
                    {
                        //if (modeL.ID != 0)
                        //{
                        //    IsComplete(modeL.klsfId, klsfId, modeL.EndTime); //判断和上一期之间是否有断层，有则补充上，断层超过20期就不补充了
                        //}
                        new BCW.BLL.klsflist().Add(model); //如果数据库内没有相同期号的数据则添加上去
                    }
                    IsLastest(); //如果数据库内的最新一条数据的截至时间比现在时间早，则添加新的数据
                }
                //if (true)
                else
                {
                    klsftimes = klsfstop2(); //通过方法取结束时间
                    klsfId = Convert.ToInt32(stage2);
                    EndTime = Convert.ToDateTime(klsftimes);
                    model.klsfId = klsfId;
                    model.Result = "";
                    model.Notes = "";
                    model.EndTime = EndTime;
                    model.State = 0;
                    bool s = new BCW.BLL.klsflist().ExistsklsfId(model.klsfId);

                    if (s == false)
                    {
                        new BLL.klsflist().Add(model); //如果数据库内没有相同期号的数据则添加上去
                    }
                    IsLastest(); //如果数据库内的最新一条数据的截至时间比现在时间早，则添加新的数据
                }
            }
            catch { }
        }

        #region 补充断层 用不到
        ///// <summary>
        ///// 判断数据库内最新一期的和捉取到的最新一期之间有没有断层
        ///// </summary>
        ///// <param name="oldklsfId">数据库内最新一期</param>
        ///// <param name="klsfId">捉取到的最新一期</param>
        //private static void IsComplete(int oldklsfId,int klsfId,DateTime EndT)
        //{
        //    if (klsfId - oldklsfId > 20 || klsfId - oldklsfId < -20) 
        //    {
        //        return;
        //    }      
        //    else
        //    {
        //        if (klsfId != oldklsfId + 1)
        //        {
        //            try
        //            {
        //                while (klsfId != oldklsfId + 1)
        //                {
        //                    oldklsfId = oldklsfId + 1;
        //                    EndT = EndT.AddMinutes(10);

        //                    if (GLT(oldklsfId) == 98)
        //                    {
        //                        String dt = "20" + oldklsfId.ToString().Remove(6, 2).Insert(2, "-").Insert(5, "-");
        //                        EndT = DateTime.Parse(dt).AddDays(1).Add(new TimeSpan(0, 0, 20));
        //                        dt = EndT.ToString("yyyyMMdd");
        //                        oldklsfId = Convert.ToInt32(dt.Remove(0, 2) + "01");
        //                    }
        //                    else if (GLT(oldklsfId) == 14)
        //                    {
        //                        String dt = "20" + oldklsfId.ToString().Remove(6, 2).Insert(2, "-").Insert(5, "-");
        //                        EndT = DateTime.Parse(dt).Add(new TimeSpan(10, 0, 20));
        //                    }
        //                    NewLastest(oldklsfId, EndT);
        //                }
        //            }
        //            catch
        //            { }
        //        }
        //        else
        //            return;
        //    }
        //}
        #endregion

        /// <summary>
        /// 获取最新一期的期数
        /// </summary>
        /// <returns></returns>
        public static int LastestStage()
        {
            Model.klsflist model = new BLL.klsflist().GetklsflistLast();

            return model.klsfId;
        }

        /// <summary>
        /// 添加新数据到数据库
        /// </summary>
        /// <param name="klsfID"></param>
        /// <param name="EndTime"></param>
        private static void NewLastest(int klsfID,DateTime EndTime) 
        {
            BCW.Model.klsflist model1 = new BCW.Model.klsflist();
            model1.klsfId = klsfID;
            model1.Result = "";
            model1.Notes = "";
            model1.EndTime = EndTime;
            model1.State = 0;
            bool s = new BCW.BLL.klsflist().ExistsklsfId(model1.klsfId);
            switch (s)
            {
                case false:
                    new BCW.BLL.klsflist().Add(model1);
                    break;
                case true:
                    break;
            }
        }

        /// <summary>
        /// 添加新一期
        /// </summary>
        public static void NewLastest2()
        {
            Model.klsflist model1 = new Model.klsflist();
            Model.klsflist model2 = new BLL.klsflist().GetklsflistLast();

            if (model2.EndTime<DateTime.Now)
            {
                new klsf().klsfLISTPAGE();
                model2 = new BLL.klsflist().GetklsflistLast();
            }

            string dt = string.Empty;

            int klsfId = model2.klsfId + 1;
            if (GLT(klsfId) == 14)
            {
                dt = DateTime.Now.ToString("yyyy-MM-dd");
                model1.EndTime = DateTime.Parse(dt).Add(new TimeSpan(10, 0, 20));
            }    
            else if (GLT(klsfId) == 98)
            {
                dt = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
                model1.EndTime = DateTime.Parse(dt).Add(new TimeSpan(0, 0, 20));
                dt = DateTime.Now.AddDays(1).ToString("yyyyMMdd");
                klsfId = Convert.ToInt32(dt.Remove(0, 2) + "01");
            }
            else
                model1.EndTime = model2.EndTime.AddMinutes(10);

            model1.klsfId = klsfId;
            model1.Result = "";
            model1.Notes = "";
            model1.State = 0;
            bool s = new BCW.BLL.klsflist().ExistsklsfId(model1.klsfId);
            switch (s)
            {
                case false:
                    new BCW.BLL.klsflist().Add(model1);
                    break;
                case true:
                    break;
            }
        }

        /// <summary>
        /// 判断是否最新一期,不是则添加到最新一期
        /// </summary>
        private static void IsLastest()
        {
            Model.klsflist model = new BLL.klsflist().GetklsflistLast();

            int klsfId = model.klsfId;
            DateTime EndTime = model.EndTime;

            // 通过数据库内最新一期的截止日期与现在时间的比较判断来决定是否添加没有结果的待更新的新一期的数据
            try
            {
                string dt = DateTime.Now.ToString("yyyy-MM-dd");
                string lt = LastestTimes().ToString("yyyy-MM-dd");

                while (EndTime < DateTime.Now.AddSeconds(10))
                {
                    if (dt == lt)
                    {
                        klsfId = klsfId + 1;
                        if (GLT(klsfId) == 14)//&& Convert.ToInt32(DateTime.Now.ToString("HHmmss")) > 095220
                            EndTime = DateTime.Parse(dt).Add(new TimeSpan(10, 0, 20));
                        else if (GLT(klsfId) == 98)// && Convert.ToInt32(DateTime.Now.ToString("HHmmss")) > 235220
                        {
                            EndTime = DateTime.Parse(dt).AddDays(1).Add(new TimeSpan(0, 0, 20));
                            dt = DateTime.Now.AddDays(1).ToString("yyyyMMdd");
                            klsfId = Convert.ToInt32(dt.Remove(0, 2) + "01");
                        }
                        else
                            EndTime = EndTime.AddMinutes(10);
                    }
                    else
                        break;
                    NewLastest(klsfId, EndTime);
                    EndTime = LastestTimes();
                    klsfId = LastestStage();
                }
            }
            catch
            { }
        }

        /// <summary>
        /// 获取日期最后两位
        /// </summary>
        public static int GLT(int num)
        {
            String s = Convert.ToString(num);
            String tmp = s.Substring(s.Length - 2, 2);
            num = Convert.ToInt32(tmp);
            return num;
        }

        /// <summary>
        /// 获取期数
        /// </summary>
        /// <returns>期数</returns>    
        private static string GetStage1()
        {
            try
            {
                String s = GetNewsUrl("http://baidu.lecai.com/lottery/draw/view/566");
                String stage = @"latest_draw_phase = '1[\d]{8}';";
                Match stages = Regex.Match(s, stage);
                String stageto = @"1[\d]{8}";
                Match stageok = Regex.Match(stages.Value, stageto);
                string stagef = stageok.Value;
                stagef = stagef.Remove(6, 1);
                return stagef;
            }
            catch
            {
                return "0";
            } 
        }

        /// <summary>
        /// 获取期数的方法二
        /// </summary>
        /// <returns>期数</returns>
        private static string GetStage2()
        {
            try
            {
                Dictionary<int, string> Stages = GetResult2();
                int stage = int.MinValue;
                foreach (var dict in Stages)
                {
                    if (dict.Key>stage)
                    {
                        stage = dict.Key;
                    }
                }
                string Stage = Convert.ToString(stage);
                return Stage;
            }
            catch 
            {
                return "0";
            }
        }

        /// <summary>
        /// 从百度彩票网上获取截止时间
        /// </summary>
        private static string klsfstop1()
        {
            String s = GetNewsUrl("http://baidu.lecai.com/lottery/draw/view/566");
            String timess = @"latest_draw_time = '[\d]{4}-[\d]{2}-[\d]{1,2} [\d]{2}:[\d]{2}:[\d]{2}';";
            Match times = Regex.Match(s, timess);
            String timeto = @"[\d]{4}-[\d]{2}-[\d]{1,2} [\d]{2}:[\d]{2}:[\d]{2}";
            Match timeok = Regex.Match(times.Value, timeto);
            return timeok.Value;
        }

        /// <summary>
        /// 直接算出开奖时间
        /// </summary>
        /// <returns>开奖时间</returns>
        private static string klsfstop2()
        {
            try
            {
                string stage = GetStage2();
                stage = stage.Substring(stage.Length - 2, 2);
                int stagei = Convert.ToInt32(stage);
                string eTime = DateTime.Now.ToString("yyyy-MM-dd");
                DateTime time01 = DateTime.Parse(eTime).Add(new TimeSpan(0, 0, 20));
                DateTime time14 = DateTime.Parse(eTime).Add(new TimeSpan(10, 0, 20));
                DateTime endTime = DateTime.Now;
                if (stagei <= 13 && stagei >= 01)
                {
                    for (int i = 0; i < stagei; i++)
                    {
                        endTime = time01.AddMinutes(10);
                    }
                }
                else if (stagei >= 14 && stagei <= 97)
                {
                    for (int i = 0; i < stagei - 14; i++)
                    {
                        endTime = time14.AddMinutes(10);
                    }
                }
                eTime = endTime.ToString();
                return eTime;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 返回一个最新的记录的截止时间
        /// </summary>
        /// <returns>数据库内最新一期的截止时间</returns>
        public static DateTime LastestTimes()
        {
            Model.klsflist model = new BLL.klsflist().GetklsflistLast();

            return model.EndTime;
        }

        /// <summary>
        /// 更新开奖结果
        /// </summary>
        public void klsfOPENPAGE()
        {
            int klsfId = 0;
            string klsfResult = "";

            try
            {
                string klsfId1 = GetStage1();
                string klsfId2 = GetStage2();

                Model.klsflist model = new BLL.klsflist().GetklsflistLast();
                Model.klsflist Last = new BLL.klsflist().GetklsflistLast2();//取数据库内最新一条有结果的数据
                ////model.klsfId - Last.klsfId < 3 ||
                //if ( Last.klsfId == 0)
                ////if (true)
                //{
                //    klsfId = Convert.ToInt32(klsfId1);
                //    klsfResult = GetResult1();
                //    new BLL.klsflist().UpdateResult(klsfId.ToString(), klsfResult);
                //    new BLL.klsfpay().UpdateResult(klsfId.ToString(), klsfResult);
                //}
                //else
                if (true)
                {
                    Dictionary<int, string> sResult = GetResult2();
                    foreach (var klsf in sResult)
                    {
                        klsfId = klsf.Key;
                        klsfResult = klsf.Value;
                        bool s = new BLL.klsflist().ExistsklsfId(klsfId);
                        switch (s)
                        {
                            case true:
                                new BCW.BLL.klsflist().UpdateResult(klsfId.ToString(), klsfResult);
                                new BCW.BLL.klsfpay().UpdateResult(klsfId.ToString(), klsfResult);
                                break;
                            case false:
                                break;
                        }
                    }
                }
            }
            catch
            { }
        }

        /// <summary>
        /// 获取结果的方法一，百度彩票（可能会崩）
        /// </summary>
        /// <returns>开奖结果</returns>
        private static string GetResult1()
        {
            try
            {
                String s = GetNewsUrl("http://baidu.lecai.com/lottery/draw/view/566");
                String fResult = @"latest_draw_result = [\D]{2}red[\D]{4}[\d]{2}[\D]{3}[\d]{2}[\D]{3}[\d]{2}[\D]{3}[\d]{2}[\D]{3}[\d]{2}[\D]{3}[\d]{2}[\D]{3}[\d]{2}[\D]{3}[\d]{2}";
                Match sResult = Regex.Match(s, fResult);
                String tResult = @"[\d]{2}";
                MatchCollection Resultok = Regex.Matches(sResult.Value, tResult);
                String[] finalResult = new String[8];

                for (int i = 0; i < 8; i++)
                {
                    if (Resultok[i].Value != null)
                    {
                        finalResult[i] = Resultok[i].Value;
                    }
                }
                String ffinalResult = String.Join(",", finalResult);
                return ffinalResult;
            }
            catch
            {
                return null;
            }   
        }

        /// <summary>
        /// 获取结果的方法二，重庆彩票网（崩的几率不大）
        /// </summary>
        /// <returns>开奖结果</returns>
        private static Dictionary<int,string> GetResult2()
        {
            try
            {
                String s = GetNewsUrl("http://www.cqcp.net/Trend/Xync/Xync.aspx?sType=ZH&type=QP");
                String fResult = @"var Con_BonusCode = \D(1[\d]{8}\D[\d]{2}\D[\d]{2}\D[\d]{2}\D[\d]{2}\D[\d]{2}\D[\d]{2}\D[\d]{2}\D[\d]{2}\D)+"; //从源代码中获取有关期数和结果的信息
                Match sResult = Regex.Match(s, fResult);
                String tResult = @"1[\d]{8}\D[\d]{2},[\d]{2},[\d]{2},[\d]{2},[\d]{2},[\d]{2},[\d]{2},[\d]{2}"; //取其中某一期的信息
                MatchCollection ResultAB = Regex.Matches(sResult.Value, tResult);
                String StageA = @"1[\d]{8}"; //期号
                String ResultA = @"[\d]{2},[\d]{2},[\d]{2},[\d]{2},[\d]{2},[\d]{2},[\d]{2},[\d]{2}"; //结果
                Dictionary<int, string> stages = new Dictionary<int, string>();
                for (int j = 0; j < ResultAB.Count; j++)
                {
                    Match StageB = Regex.Match(ResultAB[j].Value, StageA);
                    String StageC = StageB.Value.Remove(6, 1);
                    int Stage = Convert.ToInt32(StageC);
                    Match ResultB = Regex.Match(ResultAB[j].Value, ResultA);
                    stages.Add(Stage, ResultB.Value);
                }
                return stages;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 抓取一个网页源码
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private static string GetNewsUrl(string strUrl)
        {
            //string strUrl = "http://baidu.lecai.com/lottery/draw/view/566";
            string str = "";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(strUrl);
            request.Timeout = 30000;
            request.AllowAutoRedirect = false;
            request.KeepAlive = false;
            request.ProtocolVersion = HttpVersion.Version11;
            request.Headers["Accept-Language"] = "zh-cn";
            request.UserAgent = "mozilla/4.0 (compatible; msie 6.0; windows nt 5.1; sv1; .net clr 1.0.3705; .net clr 2.0.50727; .net clr 1.1.4322)";
            try
            {
                WebResponse response = request.GetResponse();
                System.IO.Stream stream = response.GetResponseStream();
                System.IO.StreamReader sr = new System.IO.StreamReader(stream, Encoding.GetEncoding("UTF-8"));
                str = sr.ReadToEnd();
                stream.Close();
                sr.Close();
            }
            catch (Exception e)
            {
                str = e.ToString().Replace("\n", "<BR>");
            }
            return str;
        }

    }
}
