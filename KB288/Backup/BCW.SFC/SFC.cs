using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Text;
using System.Net;
using BCW.Common;

namespace BCW.SFC
{
   public class GetSFC
    {
      
        /// <summary>
        /// 构造方法
        /// </summary>
        public GetSFC()
        {
        }
   
        public bool True(string votenum, string resultnum)
        {
            if ((votenum).Contains(resultnum))
                return true;
            return false;
        }
        /// <summary>
        /// 抓取页面
        /// </summary>
        /// <returns></returns>
        public string GetNewsUrl()
        {
            string strUrl = "http://s.apiplus.cn/sf/?token=2bbadaee337d9127";
            string str = "";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(strUrl);
            //request.Timeout = 30000;
            request.AllowAutoRedirect = true;
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
        /// <summary>
        /// 获取最新比赛数据XML
        /// </summary>
        /// <returns></returns>
        public string GetStageS(int i)
        {
            String s = GetNewsUrl();
            String stage = null;
            switch (i)
            {
                case 1:
                    {
                        stage = @"index=""1"" match=""[\s\S]+";
                    }
                    break;
                case 2:
                    {
                        stage = @"index=""2"" match=""[\s\S]+";
                    }
                    break;
                case 3:
                    {
                        stage = @"index=""3"" match=""[\s\S]+";
                    }
                    break;
                case 4:
                    {
                        stage = @"index=""4"" match=""[\s\S]+";
                    }
                    break;
                case 5:
                    {
                        stage = @"index=""5"" match=""[\s\S]+";
                    }
                    break;
                case 6:
                    {
                        stage = @"index=""6"" match=""[\s\S]+";
                    }
                    break;
                case 7:
                    {
                        stage = @"index=""7"" match=""[\s\S]+";
                    }
                    break;
                case 8:
                    {
                        stage = @"index=""8"" match=""[\s\S]+";
                    }
                    break;
                case 9:
                    {
                        stage = @"index=""9"" match=""[\s\S]+";
                    }
                    break;
                case 10:
                    {
                        stage = @"index=""10"" match=""[\s\S]+";
                    }
                    break;
                case 11:
                    {
                        stage = @"index=""11"" match=""[\s\S]+";
                    }
                    break;
                case 12:
                    {
                        stage = @"index=""12"" match=""[\s\S]+";
                    }
                    break;
                case 13:
                    {
                        stage = @"index=""13"" match=""[\s\S]+";
                    }
                    break;
                case 14:
                    {
                        stage = @"index=""14"" match=""[\s\S]+";
                    }
                    break;

            }
            Match stages = Regex.Match(s, stage);
            String stageto = @"[\s\S]+";
            Match stageok = Regex.Match(stages.Value, stageto);
            if (stageok.Value != null)
            {
                return stageok.Value;
            }
            else
            {
                return "0";
            }

        }
        /// <summary>
        /// 获取最新数据1：赛事，2：主队，3：客队，4：比赛时间
        /// </summary>
        /// <param name="i">比赛场次</param>
        /// <param name="j"></param>
        /// <returns></returns>
        public string GetMatch(int i, int j)
        {
            String s = null;
            String stage = null;
            Match stages = null;
            String stageto = null;
            switch (j)
            {
                case 1://获取最新赛事
                    {
                        s = GetStageS(i);
                        stage = @"match=""[\s\S]+?"" ";
                        stages = Regex.Match(s, stage);
                        stageto = @"[^\x00-\xff]+";
                    }
                    break;
                case 2://获取主场队
                    {
                        s = GetStageS(i);
                        stage = @"team_home=""[\s\S]+?"" ";
                        stages = Regex.Match(s, stage);
                        stageto = @"[^\x00-\xff]+";
                    }
                    break;
                case 3://获取客场队
                    {
                        s = GetStageS(i);
                        stage = @"team_away=""[\s\S]+?"" ";
                        stages = Regex.Match(s, stage);
                        stageto = @"[^\x00-\xff]+";
                    }
                    break;
                case 4://比赛开始时间
                    {
                        s = GetStageS(i);
                        stage = @"start_time=""[\d]{4}-[\d]{2}-[\d]{1,2} [\d]{2}:[\d]{2}:[\d]{2}";
                        stages = Regex.Match(s, stage);
                        stageto = @"[\d]{4}-[\d]{2}-[\d]{1,2} [\d]{2}:[\d]{2}:[\d]{2}";
                    }
                    break;
            }

            Match stageok = Regex.Match(stages.Value, stageto);
            if (stageok.Value != null)
            {
                return stageok.Value;
            }
            else
            {
                return "0";
            }
        }
        /// <summary>
        /// 获得最新期号
        /// </summary>
        /// <returns></returns>
        public string GetStage()
        {
            String s = GetNewsUrl();
            String stage = @" phase=""[\d]{7}";
            Match stages = Regex.Match(s, stage);
            String stageto = @"[\d]{7}";
            Match stageok = Regex.Match(stages.Value, stageto);
            if (stageok.Value != null)
            {
                return stageok.Value;
            }
            else
            {
                return "0";
            }
        }
        /// <summary>
        /// 获得最新期场次
        /// </summary>
        /// <returns></returns>
        public string GetRows()
        {
            String s = GetNewsUrl();
            String stage = @" rows=""[\d]{2}";
            Match stages = Regex.Match(s, stage);
            String stageto = @"[\d]{2}";
            Match stageok = Regex.Match(stages.Value, stageto);
            if (stageok.Value != null)
            {
                return stageok.Value;
            }
            else
            {
                return "0";
            }
        }
        /// <summary>
        /// 获取最新开奖信息i=1,开奖结果 i=2，开奖期号
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public string GetResult(int i)
        {
            String s = GetStage2();
            String stage = null;
            Match stages = null;
            String stageto = null;
            switch (i)
            {
                case 1:
                    {
                        stage = @"result=""[\d],[\d],[\d],[\d],[\d],[\d],[\d],[\d],[\d],[\d],[\d],[\d],[\d],[\d]";
                        stages = Regex.Match(s, stage);
                        stageto = @"[\d],[\d],[\d],[\d],[\d],[\d],[\d],[\d],[\d],[\d],[\d],[\d],[\d],[\d]";
                    }
                    break;
                case 2:
                    {
                        stage = @"[\d]{7}";
                        stages = Regex.Match(s, stage);
                        stageto = @"[\d]{7}";
                    }
                    break;
            }

            Match stageok = Regex.Match(stages.Value, stageto);
            if (stageok.Value != null)
            {
                return stageok.Value;
            }
            else
            {
                return "0";
            }
        }
        public string GetResultQihao()
        {
            String s = GetStage2();
            String stage = @"[\d]{7}";
            Match stages = Regex.Match(s, stage);
            String stageto = @"[\d]{7}";
            Match stageok = Regex.Match(stages.Value, stageto);
            if (stageok.Value != null)
            {
                return stageok.Value;
            }
            else
            {
                return "0";
            }

        }
        /// <summary>
        /// 开奖期号，最新开奖结果，开始时间，投注结束时间
        /// </summary>
        /// <returns></returns>
        public string GetStage2()
        {
            String s = GetNewsUrl();
            String stage = @"[\d]{7}"" result=""[\d],[\d],[\d],[\d],[\d],[\d],[\d],[\d],[\d],[\d],[\d],[\d],[\d],[\d]"" sale_start=""[\d]{4}-[\d]{2}-[\d]{1,2} [\d]{2}:[\d]{2}:[\d]{2}"" sale_end=""[\d]{4}-[\d]{2}-[\d]{1,2} [\d]{2}:[\d]{2}:[\d]{2}";
            Match stages = Regex.Match(s, stage);
            String stageto = @"[\s\S]+";
            Match stageok = Regex.Match(stages.Value, stageto);
            if (stageok.Value != null)
            {
                return stageok.Value;
            }
            else
            {
                return "0";
            }
        }
        /// <summary>
        /// 获取最新开始时间，最新结束时间XML
        /// </summary>
        /// <returns></returns>
        public string GetStage3()
        {
            String s = GetNewsUrl();
            String stage = @"[\d]{7}"" result="""" sale_start=""[\d]{4}-[\d]{2}-[\d]{1,2} [\d]{2}:[\d]{2}:[\d]{2}"" sale_end=""[\d]{4}-[\d]{2}-[\d]{1,2} [\d]{2}:[\d]{2}:[\d]{2}";
            Match stages = Regex.Match(s, stage);
            String stageto = @"[\s\S]+";
            Match stageok = Regex.Match(stages.Value, stageto);
            if (stageok.Value != null)
            {
                return stageok.Value;
            }
            else
            {
                return "0";
            }
        }
        /// <summary>
        /// 获取最新开始时间，最新结束时间i=1开始时间，i=2结束时间，i=3该期期号
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public string GetTime(int i)
        {
            String s = GetStage3();
            String stage = null;
            Match stages = null;
            String stageto = null;
            switch (i)
            {
                case 1:
                    {
                        stage = @"sale_start=""[\d]{4}-[\d]{2}-[\d]{1,2} [\d]{2}:[\d]{2}:[\d]{2}";
                        stages = Regex.Match(s, stage);
                        stageto = @"[\d]{4}-[\d]{2}-[\d]{1,2} [\d]{2}:[\d]{2}:[\d]{2}";
                    }
                    break;
                case 2:
                    {
                        stage = @"sale_end=""[\d]{4}-[\d]{2}-[\d]{1,2} [\d]{2}:[\d]{2}:[\d]{2}";
                        stages = Regex.Match(s, stage);
                        stageto = @"[\d]{4}-[\d]{2}-[\d]{1,2} [\d]{2}:[\d]{2}:[\d]{2}";
                    }
                    break;
                case 3:
                    {
                        stage = @"[\d]{7}";
                        stages = Regex.Match(s, stage);
                        stageto = @"[\d]{7}";
                    }
                    break;

            }
            Match stageok = Regex.Match(stages.Value, stageto);
            if (stageok.Value != null)
            {
                return stageok.Value;
            }
            else
            {
                return "0";
            }
        }
        public string GetStage4()
        {
            String s = GetNewsUrl();
            String stage = @"[\d]{7}"" result="""" sale_start=""[\d]{4}-[\d]{2}-[\d]{1,2} [\d]{2}:[\d]{2}:[\d]{2}"" sale_end=""[\d]{4}-[\d]{2}-[\d]{1,2} [\d]{2}:[\d]{2}:[\d]{2}"" ma";
            Match stages = Regex.Match(s, stage);
            String stageto = @"[\s\S]+";
            Match stageok = Regex.Match(stages.Value, stageto);
            if (stageok.Value != null)
            {
                return stageok.Value;
            }
            else
            {
                return "0";
            }
        }
    }
}
