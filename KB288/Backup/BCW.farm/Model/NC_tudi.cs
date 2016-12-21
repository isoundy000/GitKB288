using System;
namespace BCW.farm.Model
{
    /// <summary>
    /// ʵ����NC_tudi ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
    /// </summary>
    [Serializable]
    public class NC_tudi
    {
        public NC_tudi()
        { }
        #region Model
        private int _id;
        private int _usid;
        private int _tudi;
        private int _tudi_type;
        private string _zuowu;
        private int _zuowu_ji;
        private int _iscao;
        private int _iswater;
        private int _isinsect;
        private int _ischandi;
        private string _output;
        private int _zuowu_experience;
        private int _harvest;
        private DateTime _updatetime;
        private int _zuowu_time;
        private int _isshifei;
        private string _touid;
        private int _xianjing;
        private string _caoid;
        private string _chongid;
        private DateTime _z_caotime;
        private DateTime _z_chongtime;
        private DateTime _z_shuitime;
        private int _z_shuinum;
        private int _aa;
        /// <summary>
        /// 
        /// </summary>
        public int ID
        {
            set { _id = value; }
            get { return _id; }
        }
        public int aa
        {
            set { _aa = value; }
            get { return _aa; }
        }
        /// <summary>
        /// �û�ID
        /// </summary>
        public int usid
        {
            set { _usid = value; }
            get { return _usid; }
        }
        /// <summary>
        /// ����
        /// </summary>
        public int tudi
        {
            set { _tudi = value; }
            get { return _tudi; }
        }
        /// <summary>
        /// ��������
        /// </summary>
        public int tudi_type
        {
            set { _tudi_type = value; }
            get { return _tudi_type; }
        }
        /// <summary>
        /// ����
        /// </summary>
        public string zuowu
        {
            set { _zuowu = value; }
            get { return _zuowu; }
        }
        /// <summary>
        /// ���������ļ���
        /// </summary>
        public int zuowu_ji
        {
            set { _zuowu_ji = value; }
            get { return _zuowu_ji; }
        }
        /// <summary>
        /// ����
        /// </summary>
        public int iscao
        {
            set { _iscao = value; }
            get { return _iscao; }
        }
        /// <summary>
        /// �Ƿ�ˮ
        /// </summary>
        public int iswater
        {
            set { _iswater = value; }
            get { return _iswater; }
        }
        /// <summary>
        /// ����
        /// </summary>
        public int isinsect
        {
            set { _isinsect = value; }
            get { return _isinsect; }
        }
        /// <summary>
        /// ����(0��1��2��ή)
        /// </summary>
        public int ischandi
        {
            set { _ischandi = value; }
            get { return _ischandi; }
        }
        /// <summary>
        /// ��/ʣ
        /// </summary>
        public string output
        {
            set { _output = value; }
            get { return _output; }
        }
        /// <summary>
        /// ���ﾭ��
        /// </summary>
        public int zuowu_experience
        {
            set { _zuowu_experience = value; }
            get { return _zuowu_experience; }
        }
        /// <summary>
        /// �ջ񼾶�
        /// </summary>
        public int harvest
        {
            set { _harvest = value; }
            get { return _harvest; }
        }
        /// <summary>
        /// ��ֲʱ��
        /// </summary>
        public DateTime updatetime
        {
            set { _updatetime = value; }
            get { return _updatetime; }
        }
        /// <summary>
        /// ��������ʱ���ܳ�
        /// </summary>
        public int zuowu_time
        {
            set { _zuowu_time = value; }
            get { return _zuowu_time; }
        }
        /// <summary>
        /// 0Ϊ�ý׶�δʩ��1Ϊ��ʩ
        /// </summary>
        public int isshifei
        {
            set { _isshifei = value; }
            get { return _isshifei; }
        }
        /// <summary>
        /// ͵���˵�ID
        /// </summary>
        public string touID
        {
            set { _touid = value; }
            get { return _touid; }
        }
        /// <summary>
        /// ����(0û��1��)
        /// </summary>
        public int xianjing
        {
            set { _xianjing = value; }
            get { return _xianjing; }
        }
        /// <summary>
        /// �Ųݵ�ID
        /// </summary>
        public string caoID
        {
            set { _caoid = value; }
            get { return _caoid; }
        }
        /// <summary>
        /// �ų��ID
        /// </summary>
        public string chongID
        {
            set { _chongid = value; }
            get { return _chongid; }
        }
        /// <summary>
        /// ˢ�»��ֲ�ʱ��
        /// </summary>
        public DateTime z_caotime
        {
            set { _z_caotime = value; }
            get { return _z_caotime; }
        }
        /// <summary>
        /// ˢ�»��ų�ʱ��
        /// </summary>
        public DateTime z_chongtime
        {
            set { _z_chongtime = value; }
            get { return _z_chongtime; }
        }
        /// <summary>
        /// ˢ�»�ȱˮʱ��
        /// </summary>
        public DateTime z_shuitime
        {
            set { _z_shuitime = value; }
            get { return _z_shuitime; }
        }
        /// <summary>
        /// ȱˮ����
        /// </summary>
        public int z_shuinum
        {
            set { _z_shuinum = value; }
            get { return _z_shuinum; }
        }
        #endregion Model

    }
}

