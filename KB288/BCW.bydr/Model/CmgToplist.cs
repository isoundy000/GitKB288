using System;
namespace BCW.bydr.Model
{
    /// <summary>
    /// ʵ����CmgToplist ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
    /// </summary>
    [Serializable]
    public class CmgToplist
    {
        public CmgToplist()
        { }
        #region Model
        private int _id;
        private long _allcolletgold;
        private long _mcolletgold;
        private long _dcolletgold;
        private long _ycolletgold;
        private int? _usid;
        private int? _stype;
        private DateTime _Time;
        private int _sid;
        private DateTime _updatetime;
        private int _vit;
        private DateTime _Signtime;
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
        public long AllcolletGold
        {
            set { _allcolletgold = value; }
            get { return _allcolletgold; }
        }
        /// <summary>
        /// 
        /// </summary>
        public long McolletGold
        {
            set { _mcolletgold = value; }
            get { return _mcolletgold; }
        }
        /// <summary>
        /// 
        /// </summary>
        public long DcolletGold
        {
            set { _dcolletgold = value; }
            get { return _dcolletgold; }
        }
        /// <summary>
        /// 
        /// </summary>
        public long YcolletGold
        {
            set { _ycolletgold = value; }
            get { return _ycolletgold; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? usID
        {
            set { _usid = value; }
            get { return _usid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? stype
        {
            set { _stype = value; }
            get { return _stype; }
        }
        public DateTime Time
        {
            set { _Time = value; }
            get { return _Time; }
        }
        /// <summary>
        /// �ж�ycolletGold�仯
        /// </summary>
        public int sid
        {
            set { _sid = value; }
            get { return _sid; }
        }
        /// <summary>
        /// �����ռ���ʱ��
        /// </summary>
        public DateTime updatetime
        {
            set { _updatetime = value; }
            get { return _updatetime; }
        }
        /// <summary>
        /// ����ֵ
        /// </summary>
        public int vit
        {
            set { _vit = value; }
            get { return _vit; }
        }
        /// <summary>
        /// ǩ��ʱ��
        /// </summary>
        public DateTime Signtime
        {
            set { _Signtime = value; }
            get { return _Signtime; }
        }
        #endregion Model

    }
}

