using System;
using System.Collections.Generic;
using System.Text;
using BCW.Common;
using System.Data;

namespace BCW.Baccarat
{
    public class baccarat
    {
        #region  扑克牌类型说明 Card
        /// <summary>
        /// 扑克牌类 ACE A,最小TWO 2
        /// <summary>
        public class Card
        {
            #region 扑克牌定义变量
            /// <summary>
            /// 方块(钻石)
            /// </summary>
            public const int DIAMOND = 1;  // 方块(钻石)
            /// <summary>
            /// 梅花
            /// </summary>
            public const int CLUB = 2;     // 梅花
            /// <summary>
            /// 红桃(红心)
            /// </summary>
            public const int HEART = 3;    // 红桃(红心)
            /// <summary>
            /// 黑桃(花锄)
            /// </summary>
            public const int SPADE = 4;    // 黑桃(花锄)  

            public const int DEUCE = 2;    // 2
            public const int THREE = 3;    // 3
            public const int FOUR = 4;     // 4
            public const int FIVE = 5;     // 5
            public const int SIX = 6;      // 6
            public const int SEVEN = 7;    // 7
            public const int EIGHT = 8;    // 8
            public const int NINE = 9;     // 9
            public const int TEN = 10;      // 10
            public const int JACK = 11;     // J
            public const int QUEEN = 12;   // Q
            public const int KING = 13;    // K
            public const int ACE = 14;     // A
            #endregion

            #region
            /** 花色 1代表方块，2代表梅花，3代表红桃，4代表黑桃，5：王 */
            private int suit;
            /** 点数 规定：2代表2,3代表3,4代表4...... */
            private int rank;

            public Card()
            {
            }

            public Card(int suit, int rank)
            {
                setRank(rank);
                setSuit(suit);
            }

            public int getSuit()
            {
                return suit;
            }

            public void setSuit(int suit)
            {
                if (suit < DIAMOND || suit > SPADE)
                    throw new Exception("花色超过范围!");
                this.suit = suit;
            }

            public int getRank()
            {
                return rank;
            }

            public void setRank(int rank)
            {
                if (rank < DEUCE || rank > ACE)
                    throw new Exception("点数超出范围!");
                this.rank = rank;
            }

            private static String[] RANK_NAMES = { "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K", "A" };
            private static String[] SUIT_NAMES = { "方块", "梅花", "红桃", "黑桃" };

            //覆盖Object 类的toString方法，实现对象的文本描述
            public String toString()
            {
                return SUIT_NAMES[suit] + RANK_NAMES[rank];
            }

            #endregion

            #region 输出图片Html toHtml()
            /// <summary>
            /// 输出图片
            /// </summary>
            /// <param name="sr"></param>
            /// <returns></returns>
            public static String toHtml(string sr)
            {
                //设置XML地址
                ub xml = new ub();
                string xmlPath = "/Controls/baccarat.xml";
                xml.ReloadSub(xmlPath);//加载配置
                return "<img src=..\\,,\\" + xml.dss["baccarat_img"] + sr + ".jpg \" alt=\"load\" />";
            }
            #endregion

            #region 获取点数
            /// <summary>
            /// 获取点数
            /// </summary>
            /// <param name="rank"></param>
            /// <returns><returns>
            public static String GetRank(string rank)
            {
                string r = "";
                if ((rank.Length - 1) < 2)
                {
                    r = "0";
                }
                r += rank.Substring(1, rank.Length - 1);
                return r;
            }
            #endregion

            #region 获取花色GetSuit() 1方块，2梅花，3红桃，4黑桃
            /// <summary>
            /// 获取花色 1方块，2梅花，3红桃，4黑桃
            /// </summary>
            /// <param name="Suit"></param>
            /// <returns></returns>
            public static String GetSuit(string Suit)
            {
                return Suit.Substring(0, 1);
            }
            #endregion

        }

        #endregion

        #region  打乱扑克牌排序方法
        /// <summary>
        /// 打乱
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="inputList"></param>
        /// <returns></returns>
        public static List<T> GetRandomList<T>(List<T> inputList)
        {
            //Copy to a array
            T[] copyArray = new T[inputList.Count];
            inputList.CopyTo(copyArray);

            //Add range
            List<T> copyList = new List<T>();
            copyList.AddRange(copyArray);

            //Set outputList and random
            List<T> outputList = new List<T>();
            Random rd = new Random(DateTime.Now.Millisecond);

            while (copyList.Count > 0)
            {
                //Select an index and item
                int rdIndex = rd.Next(0, copyList.Count);
                T remove = copyList[rdIndex];

                //remove it from copyList and add it to output
                copyList.Remove(remove);
                outputList.Add(remove);
            }
            return outputList;
        }
        #endregion

        #region 洗牌方法 ShufflingProcess()
        /// <summary>
        /// 洗牌
        /// </summary>
        /// <param name="Room_Model">当前房间Model</param>
        /// <returns>返回OK正常</returns>
        public string ShufflingProcess()
        {
            string r = "";

            //定义一个庄家扑克变量
            string BankerPoker = "";

            //定义一个闲家扑克变量
            string HunterPoker = "";

            int k = 1;

            //定义一个庄家扑克点数变量
            int BankerPoint = 0;

            //定义一个闲家扑克点数变量
            int HunterPoint = 0;

            //定义一个获取扑克花色变量
            int suit = 0;

            //定义一个扑克点数变量
            int rank = 0;

            try
            {

                List<Card> cards = new List<Card>();

                //生成八副牌 最小为2 最大为A
                for (int j = 0; j < 8; j++)
                {

                    //生成一副牌
                    for (int i = Card.DEUCE; i <= Card.ACE; i++)
                    {
                        //方块
                        cards.Add(new Card(Card.DIAMOND, i));

                        //梅花
                        cards.Add(new Card(Card.CLUB, i));

                        //红桃
                        cards.Add(new Card(Card.HEART, i));

                        //黑桃
                        cards.Add(new Card(Card.SPADE, i));
                    }
                }

                //打乱扑克牌，获得新牌
                List<Card> NewCardslist = GetRandomList(cards);

                //加入数据
                foreach (Card d in NewCardslist)
                {

                    //前期发庄闲家手牌并计算点数
                    if (k <= 4)
                    {
                        //判断是否是给庄家发牌
                        if (k % 2 == 0)
                        {
                            rank = d.getRank();
                            
                            suit = d.getSuit();

                            if (rank == 10 || rank == 11 || rank == 12 || rank == 13)
                            {
                                BankerPoint = BankerPoint + 0; 
                            }

                            else if (rank == 14)
                                BankerPoint = (BankerPoint + 1) % 10;

                            else
                                BankerPoint = (BankerPoint + rank) % 10;

                            if(BankerPoker=="")
                                BankerPoker = Poker(suit, rank);

                            else
                                BankerPoker = BankerPoker + "," + Poker(suit, rank);
                            
                        }
                        //判断是否给闲家发牌
                        else
                        {
                            rank = d.getRank();

                            suit = d.getSuit();

                            if (rank == 10 || rank == 11 || rank == 12 || rank == 13)
                            {
                                HunterPoint = HunterPoint + 0;
                            }

                            else if (rank == 14)
                                HunterPoint = (HunterPoint + 1) % 10;

                            else
                                HunterPoint = (HunterPoint + rank) % 10;

                            if(HunterPoker=="")
                                HunterPoker = Poker(suit, rank);
                            else
                                HunterPoker = HunterPoker + "," + Poker(suit, rank);
                        }
                    }

                    else
                    {
                        //判断是否为 “天生赢家”
                        if (HunterPoint == 8 || HunterPoint == 9 || BankerPoint == 8 || BankerPoint == 9)
                            break;

                        //闲家起手牌点数为6点或7点，闲家不须补牌，此条件下庄家起手牌点数为5或5点以下，庄家必须补第三张牌。
                        else if ((HunterPoint == 6 || HunterPoint == 7) && BankerPoint <= 5)
                        {
                                rank = d.getRank();

                                suit = d.getSuit();

                                BankerPoint = (BankerPoint + rank) % 10;

                                BankerPoker = BankerPoker + "," + Poker(suit, rank);

                                break;
                        }

                        //判断 闲家点数在补牌范围内，庄家不用补牌的点数
                        else if (HunterPoint < 6 && BankerPoint == 7)
                        {
                            rank = d.getRank();

                            suit = d.getSuit();

                            HunterPoint = (HunterPoint + rank) % 10;

                            HunterPoker = HunterPoker + "," + Poker(suit, rank);

                            break;
                        }

                        //当闲家补得第三张牌是0.1.2.3.4.5.8.9，不须补牌；其余则须补牌
                        else if (HunterPoint < 6 && BankerPoint == 6)
                        {
                            rank = d.getRank();

                            suit = d.getSuit();

                            HunterPoint = (HunterPoint + rank) % 10;

                            HunterPoker = HunterPoker + "," + Poker(suit, rank);

                            if (rank == 6 || rank == 7)
                            {
                                rank = d.getRank();

                                suit = d.getSuit();

                                BankerPoint = (BankerPoint + rank) % 10;

                                BankerPoker = BankerPoker + "," + Poker(suit, rank);

                                break;
                            }

                            else
                                break;
                        }

                        //当闲家补得第三张牌是0.1.2.3.8.9，不须补牌；其余则须补牌
                        else if (HunterPoint < 6 && BankerPoint == 5)
                        {
                            rank = d.getRank();

                            suit = d.getSuit();

                            HunterPoint = (HunterPoint + rank) % 10;

                            HunterPoker = HunterPoker + "," + Poker(suit, rank);

                            if (rank == 4 || rank == 5 || rank == 6 || rank == 7)
                            {
                                rank = d.getRank();

                                suit = d.getSuit();

                                BankerPoint = (BankerPoint + rank) % 10;

                                BankerPoker = BankerPoker + "," + Poker(suit, rank);

                                break;
                            }
                            else
                                break;
                        }

                        //当闲家补得第三张牌是0.1.8.9，不须补牌；其余则须补牌
                        else if (HunterPoint < 6 && BankerPoint == 4)
                        {
                            rank = d.getRank();

                            suit = d.getSuit();

                            HunterPoint = (HunterPoint + rank) % 10;

                            HunterPoker = HunterPoker + "," + Poker(suit, rank);

                            if (rank == 0 || rank == 1 || rank == 8 || rank == 9)
                                break;

                            else
                            {
                                rank = d.getRank();

                                suit = d.getSuit();

                                BankerPoint = (BankerPoint + rank) % 10;

                                BankerPoker = BankerPoker + "," + Poker(suit, rank);

                                break;
                            }
                                
                        }

                        //当闲家补得第三张牌是8，不须补牌；其余则须补牌
                        else if (HunterPoint < 6 && BankerPoint == 3)
                        {
                            rank = d.getRank();

                            suit = d.getSuit();

                            HunterPoint = (HunterPoint + rank) % 10;

                            HunterPoker = HunterPoker + "," + Poker(suit, rank);
                            
                            if (rank == 8)
                                break;

                            else
                            {
                                rank = d.getRank();

                                suit = d.getSuit();

                                BankerPoint = (BankerPoint + rank) % 10;

                                BankerPoker = BankerPoker + "," + Poker(suit, rank);

                                break;
                            }
                        }

                        //当闲家的点数小于6，而且庄家的点数少于3时，两家都要补牌
                        else if (HunterPoint < 6 && BankerPoint < 3)
                        {
                            rank = d.getRank();

                            suit = d.getSuit();

                            HunterPoint = (HunterPoint + rank) % 10;

                            HunterPoker = HunterPoker + "," + Poker(suit, rank);

                            rank = d.getRank();

                            suit = d.getSuit();

                            BankerPoint = (BankerPoint + rank) % 10;

                            BankerPoker = BankerPoker + "," + Poker(suit, rank);

                            break;
                        }
                    }
                    if (k < 5)
                    {
                        k++;
                    }
                    
                }
                r = HunterPoker + "#" + BankerPoker;
            }
            catch
            {
                r = "ERROR";
            }
            return r;
        }
        #endregion

        ///<summary>
        ///返回一张完整的扑克
        ///</summary>
        private string Poker(int suit, int rank)
        {
            string poker = "";

            //把花色和点数存进hunt的字符串里
            poker = suit.ToString() +","+ rank.ToString();

            return poker;

            ////把花色和点数存进hunt的字符串里面
            //if (hunt == "")
            //    hunt = suit.ToString() + rank.ToString();
            //else
            //{
            //    hunt = hunt + suit.ToString();
            //    hunt = hunt + rank.ToString();
            //}
        }
    }
}
