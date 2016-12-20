using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using BCW.Common;

namespace BCW.Guess3
{
    public class OnceBf
    {
        /// <summary>
        /// 取得足球半场即时比分
        /// </summary>
        /// <param name="p_TPRtime">联赛时间</param>
        /// <param name="p_id">比赛ID</param>
        /// <returns></returns>
        public string HalfBf(int Types, DateTime p_TPRtime, int p_id)
        {
            string bfstat = "未";
            if (p_TPRtime < DateTime.Now)
                bfstat = "0:0";

            //取即时比分接口

            string bf = new TPR3.Collec.TempFootBf().GetFootBfOnce();
            string strpatternbf = @"A\[1\]\=""([\s\S]+)B\[1\]\=";
            Match mtitlebf = Regex.Match(bf, strpatternbf, RegexOptions.IgnoreCase);
            if (mtitlebf.Success)
            {
                string p_str = mtitlebf.Groups[1].Value;
                p_str = Regex.Replace(p_str, @"A\[\d*\]\=""", "");
                string[] Temp = Regex.Split(p_str, @"\.split\('\^'\);");
                for (int i = 0; i < Temp.Length; i++)
                {
                    string[] temp = Regex.Split(Temp[i], @"\^");
                    try
                    {
                        if (Utils.ParseInt(temp[0].Replace("\r\n", "")) == p_id)
                        {
                            //取开场时间(用来计算进行了多少分钟/(上半场时为开上半场的时间,下半场时为开下半场的时间）)
                            string stime = temp[12];
                            //取比赛状态
                            string state = OutState(temp[13]);

                            string[] dTemp = stime.Split(',');
                            string dt = dTemp[0] + "-" + (Convert.ToInt32(dTemp[1]) + 1) + "-" + dTemp[2] + " " + dTemp[3] + ":" + dTemp[4] + ":" + dTemp[5];
                            DateTime vtime = Convert.ToDateTime(dt);
                            string strmin = "";
                            if (state == "上半场")
                            {
                                strmin = DT.DateDiff(DateTime.Now, vtime, 3) + "'";
                            }
                            else if (state == "下半场")
                            {
                                long Minute = DT.DateDiff(DateTime.Now, vtime, 3) + 45;
                                if (Minute > 90)
                                    strmin = "90+'";
                                else
                                    strmin = "" + Minute.ToString() + "'";
                            }
                            else
                            {
                                strmin = state;
                            }

                            //取主队比分
                            string hbf = temp[14];
                            //取客队比分
                            string gbf = temp[15];
                            //取主队上半场比分
                            string hbf2 = temp[16];
                            //取客队上半场比分
                            string gbf2 = temp[17];
                            if (Types == 1)
                            {
                                if (hbf2 != "" && gbf2 != "")
                                {
                                    bfstat = "" + hbf2 + "-" + gbf2 + "#" + strmin;
                                }
                                else
                                {
                                    bfstat = "" + hbf + "-" + gbf + "#" + strmin;
                                }
                            }
                            else
                            {
                                if (hbf != "" && gbf != "")
                                {
                                    bfstat = "" + hbf + "-" + gbf + "#" + strmin;
                                }
                                else
                                {
                                    bfstat = "" + hbf2 + "-" + gbf2 + "#" + strmin;
                                }
                            }
             
                            break;
                        }
                    }
                    catch { }


                }

            }
            if (bfstat == "-")
                bfstat = "未";

            return bfstat;
        }


        /// <summary>
        /// 取得篮球每节比分
        /// </summary>
        /// <param name="Types">1一节，2二节，3上半场，4第三节</param>
        /// <param name="p_TPRtime">联赛时间</param>
        /// <param name="p_id">比赛ID</param>
        /// <returns></returns>
        public string DJBf(int Types, DateTime p_TPRtime, int p_id)
        {
            string bfstat = "未";

            if (p_TPRtime < DateTime.Now)
            {
                bfstat = "0:0";
                string p_xml = GetSourceTextByUrl("http://bf.titan007.com/nba/today.xml", "GB2312");
                TPR3.Model.guess.Tempbakbf n = new TPR3.Model.guess.Tempbakbf();
                using (XmlReaderExtend reader = new XmlReaderExtend(p_xml))
                {
                    while (reader.ReadToFollowing("h"))
                    {
                        string p_str = reader.GetElementValue();

                        string[] Temp = Regex.Split(p_str, @"\^");
                        int id = 0;

                        id = Convert.ToInt32(Temp[0]);

                        if (id == p_id)
                        {
                            if (Types == 0)//全场比分
                            {
                                bfstat = Temp[11] + "-" + Temp[12];
                            }
                            else if (Types == 1)
                            {
                                bfstat = Temp[13] + "-" + Temp[14];
                            }
                            else if (Types == 2)
                            {
                                bfstat = Temp[15] + "-" + Temp[16];
                            }
                            else if (Types == 3)
                            {
                                bfstat = (Utils.ParseInt(Temp[13]) + Utils.ParseInt(Temp[15])) + "-" + (Utils.ParseInt(Temp[14]) + Utils.ParseInt(Temp[16]));
                            }
                            else if (Types == 4)
                            {
                                bfstat = Temp[17] + "-" + Temp[18];
                            }

                            break;
                        }
                    }
                }
            }

            if (bfstat == "-")
                bfstat = "未";

            return bfstat;
        }

        /// <summary>
        /// 取得篮球进行第几节
        /// </summary>
        /// <param name="p_TPRtime">联赛时间</param>
        /// <param name="p_id">比赛ID</param>
        /// <returns></returns>
        public string LqOutJie(DateTime p_TPRtime, int p_id)
        {
            string bfstat = "";
            if (p_TPRtime < DateTime.Now)
            {
                string p_xml = GetSourceTextByUrl("http://bf.titan007.com/nba/today.xml", "GB2312");
                TPR3.Model.guess.Tempbakbf n = new TPR3.Model.guess.Tempbakbf();
                using (XmlReaderExtend reader = new XmlReaderExtend(p_xml))
                {
                    while (reader.ReadToFollowing("h"))
                    {
                        string p_str = reader.GetElementValue();

                        string[] Temp = Regex.Split(p_str, @"\^");
                        int id = 0;

                        id = Convert.ToInt32(Temp[0]);

                        if (id == p_id)
                        {
                            //取第几节
                            string jie = OutJie(Temp[5]);
                            string remaintime = Temp[6];
                            if (remaintime != "")
                                bfstat = jie + "(" + remaintime + "')";
                            else
                                bfstat = jie;

                            break;
                        }
                    }
                }

            }
            return bfstat;
        }

        private string OutJie(string matchstate)
        {
            string outText = "";
            if (matchstate == "0")
                outText = "未";
            else if (matchstate == "1")
                outText = "一节";
            else if (matchstate == "2")
                outText = "二节";
            else if (matchstate == "3")
                outText = "三节";
            else if (matchstate == "4")
                outText = "四节";
            else if (matchstate == "5")
                outText = "1'OT";
            else if (matchstate == "6")
                outText = "2'OT";
            else if (matchstate == "7")
                outText = "3'OT";
            else if (matchstate == "8")
                outText = "4'OT";
            else if (matchstate == "9")
                outText = "5'OT";
            else if (matchstate == "-1")
                outText = "完";
            else if (matchstate == "-2")
                outText = "待定";
            else if (matchstate == "-3")
                outText = "中断";
            else if (matchstate == "-4")
                outText = "取消";
            else if (matchstate == "-5")
                outText = "推迟";
            else if (matchstate == "50")
                outText = "中场";

            return outText;
        }

        private string OutState(string state)
        {
            string OutText = "";
            if (state == "0")
                OutText = "未";
            else if (state == "1")
                OutText = "上半场";
            else if (state == "2")
                OutText = "中场";
            else if (state == "3")
                OutText = "下半场";
            else if (state == "4")
                OutText = "加";
            else if (state == "-10")
                OutText = "取消";
            else if (state == "-11")
                OutText = "待定";
            else if (state == "-12")
                OutText = "腰斩";
            else if (state == "-13")
                OutText = "中断";
            else if (state == "-14")
                OutText = "推迟";
            else if (state == "-1")
                OutText = "完";

            return OutText;
        }

        /// <summary>
        /// 抓取一个网页源码
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private string GetSourceTextByUrl(string url, string Encoding)
        {
            try
            {
                System.Net.WebRequest request = System.Net.WebRequest.Create(url);
                request.Timeout = 20000;
                System.Net.WebResponse response = request.GetResponse();

                System.IO.Stream resStream = response.GetResponseStream();
                System.IO.StreamReader sr = new System.IO.StreamReader(resStream, System.Text.Encoding.GetEncoding(Encoding));
                return sr.ReadToEnd();
            }
            catch
            {
                return "";
            }
        }

    }
}
