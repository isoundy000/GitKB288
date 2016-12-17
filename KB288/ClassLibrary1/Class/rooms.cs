using System;
using System.Collections.Generic;
using System.Text;

namespace BCW.TexasPoker
{
    /// <summary>
    /// 房间对象
    /// </summary>
    public class Room
    {
        public int id;
        private Dictionary<int, Desk> dctDesks;


        public void Update()
        {

        }
    }


    /// <summary>
    /// 房间管理
    /// </summary>
    public class RoomManager
    {
        public Dictionary<int, Room> dctRooms;

        /// <summary>
        /// 构造函数
        /// </summary>
        public RoomManager()
        {
            dctRooms = new Dictionary<int, Room>();
        }

        /// <summary>
        /// 从数据库中初始化所有房间
        /// </summary>
        public void InitRoomFormDB()
        {
           
        }

        public Room CreateRoom()
        {
            Room _room = new Room();
            _room.id = dctRooms.Count + 1;
            dctRooms.Add(_room.id , _room );
            return null;
        }

        /// <summary>
        /// 根据Id获取房间对象
        /// </summary>
        /// <param name="_roomId"></param>
        /// <returns></returns>
        public Room GetRoomById(int _roomId)
        {
            if( dctRooms.ContainsKey( _roomId ) )
                return dctRooms[ _roomId ];
            return null;
        }

       /// <summary>
       /// 踢除房间所有玩家
       /// </summary>
       /// <returns></returns>
        public bool TickUser()
        {
            return true;
        }

        /// <summary>
        /// 剔除房间指定玩家
        /// </summary>
        /// <param name="_userId"></param>
        /// <returns></returns>
        public bool TickUser( int _userId )
        {
            return true;
        }


    }   



}
