using System;
using System.Collections.Generic;
using System.Text;

namespace BCW.TexasPoker
{
    /// <summary>
    /// 德州扑克游戏
    /// </summary>
    public class TexasPokerGame
    {
        private static TexasPokerGame mInstance;

        public RoomManager roomMgr;                    //房间管理

        public static TexasPokerGame Instance()
        {
            if( mInstance == null )
                mInstance = new TexasPokerGame();
            return mInstance;
        }

        public TexasPokerGame()
        {
            roomMgr = new RoomManager();
        }


        /// <summary>
        /// 定时刷新
        /// </summary>
        public void Update()
        {
          
        }

    }
}
