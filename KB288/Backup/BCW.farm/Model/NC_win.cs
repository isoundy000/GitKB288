using System;
namespace BCW.farm.Model
{
    /// <summary>
    /// ʵ����NC_win ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
    /// </summary>
    [Serializable]
    public class NC_win
    {
        public NC_win()
        { }
        #region Model
        private int _id;
        private int _usid;
        private int _prize_id;
        private string _prize_name;
        private DateTime _addtime;
        private int _prize_type;
        /// <summary>
        /// �û�ID
        /// </summary>
        public int ID
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int usid
        {
            set { _usid = value; }
            get { return _usid; }
        }
        /// <summary>
        /// ��ƷID
        /// </summary>
        public int prize_id
        {
            set { _prize_id = value; }
            get { return _prize_id; }
        }
        /// <summary>
        /// ��Ʒ����
        /// </summary>
        public string prize_name
        {
            set { _prize_name = value; }
            get { return _prize_name; }
        }
        /// <summary>
        /// �н�ʱ��
        /// </summary>
        public DateTime addtime
        {
            set { _addtime = value; }
            get { return _addtime; }
        }
        /// <summary>
        /// �齱������0Ϊ��1Ϊ����2Ϊ����
        /// </summary>
        public int prize_type
        {
            set { _prize_type = value; }
            get { return _prize_type; }
        }
        #endregion Model

    }
}

