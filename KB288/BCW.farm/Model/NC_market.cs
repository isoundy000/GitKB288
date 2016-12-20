using System;
namespace BCW.farm.Model
{
    /// <summary>
    /// ʵ����NC_market ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
    /// </summary>
    [Serializable]
    public class NC_market
    {
        public NC_market()
        { }
        #region Model
        private int _id;
        private int _usid;
        private int _daoju_id;
        private int _daoju_num;
        private long _daoju_price;
        private DateTime _add_time;
        private int _type;
        private string _daoju_name;
        private decimal _sui;
        /// <summary>
        /// 
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
        /// ���ߵ�ID
        /// </summary>
        public int daoju_id
        {
            set { _daoju_id = value; }
            get { return _daoju_id; }
        }
        /// <summary>
        /// ���ߵ�����
        /// </summary>
        public int daoju_num
        {
            set { _daoju_num = value; }
            get { return _daoju_num; }
        }
        /// <summary>
        /// �����ۼ�
        /// </summary>
        public long daoju_price
        {
            set { _daoju_price = value; }
            get { return _daoju_price; }
        }
        /// <summary>
        /// ���ʱ��
        /// </summary>
        public DateTime add_time
        {
            set { _add_time = value; }
            get { return _add_time; }
        }
        /// <summary>
        /// ����0Ϊ�¼�1Ϊ�ϼ�
        /// </summary>
        public int type
        {
            set { _type = value; }
            get { return _type; }
        }
        /// <summary>
        /// ��������
        /// </summary>
        public string daoju_name
        {
            set { _daoju_name = value; }
            get { return _daoju_name; }
        }
        /// <summary>
        /// ʵʱ��˰
        /// </summary>
        public decimal sui
        {
            set { _sui = value; }
            get { return _sui; }
        }
        #endregion Model

    }
}

