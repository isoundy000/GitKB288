using System;
namespace TPR.Model.guess
{
    /// <summary>
    /// ʵ����BaOrder ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
    /// </summary>
    [Serializable]
    public class BaOrder
    {
        public BaOrder()
        { }
        #region Model
        private int _id;
        private int? _orderusid;
        private string _orderusname;
        private decimal? _orderbanum;
        private decimal? _orderfanum;
        private decimal? _orderjbnum;
        private int? _orderstats;
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
        public int? Orderusid
        {
            set { _orderusid = value; }
            get { return _orderusid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Orderusname
        {
            set { _orderusname = value; }
            get { return _orderusname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? Orderbanum
        {
            set { _orderbanum = value; }
            get { return _orderbanum; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? Orderfanum
        {
            set { _orderfanum = value; }
            get { return _orderfanum; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? Orderjbnum
        {
            set { _orderjbnum = value; }
            get { return _orderjbnum; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? Orderstats
        {
            set { _orderstats = value; }
            get { return _orderstats; }
        }
        #endregion Model

    }
}

