using System;
namespace BCW.farm.Model
{
    /// <summary>
    /// ʵ����NC_shop ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
    /// </summary>
    [Serializable]
    public class NC_shop
    {
        public NC_shop()
        { }
        #region Model
        private int _id;
        private string _name;
        private int _num_id;
        private int _grade;
        private string _picture;
        private int _jidu;
        private int _jidu_time;
        private long _price_in;
        private long _price_out;
        private int _experience;
        private string _output;
        private int _type;
        private int _iszengsong;
        private int _zhonglei;
        private int _caotime;
        private int _chongtime;
        private int _shuitime;
        private int _meili;
        private int _tili;
        /// <summary>
        /// 
        /// </summary>
        public int ID
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// ��������
        /// </summary>
        public string name
        {
            set { _name = value; }
            get { return _name; }
        }
        /// <summary>
        /// ����ID
        /// </summary>
        public int num_id
        {
            set { _num_id = value; }
            get { return _num_id; }
        }
        /// <summary>
        /// ��ֲ�ȼ�
        /// </summary>
        public int grade
        {
            set { _grade = value; }
            get { return _grade; }
        }
        /// <summary>
        /// ��Ƭ·��
        /// </summary>
        public string picture
        {
            set { _picture = value; }
            get { return _picture; }
        }
        /// <summary>
        /// ���＾��
        /// </summary>
        public int jidu
        {
            set { _jidu = value; }
            get { return _jidu; }
        }
        /// <summary>
        /// ����ʱ��
        /// </summary>
        public int jidu_time
        {
            set { _jidu_time = value; }
            get { return _jidu_time; }
        }
        /// <summary>
        /// ���뵥��
        /// </summary>
        public long price_in
        {
            set { _price_in = value; }
            get { return _price_in; }
        }
        /// <summary>
        /// ��������
        /// </summary>
        public long price_out
        {
            set { _price_out = value; }
            get { return _price_out; }
        }
        /// <summary>
        /// ���ﾭ��
        /// </summary>
        public int experience
        {
            set { _experience = value; }
            get { return _experience; }
        }
        /// <summary>
        /// �������
        /// </summary>
        public string output
        {
            set { _output = value; }
            get { return _output; }
        }
        /// <summary>
        /// ��������(1��2��3��4��10��ϡ)
        /// </summary>
        public int type
        {
            set { _type = value; }
            get { return _type; }
        }
        /// <summary>
        /// 0��������1����
		/// </summary>
        public int iszengsong
        {
            set { _iszengsong = value; }
            get { return _iszengsong; }
        }
        /// <summary>
        /// 1ũ����2ˮ��3��4�л�5����
        /// </summary>
        public int zhonglei
        {
            set { _zhonglei = value; }
            get { return _zhonglei; }
        }
        /// <summary>
        /// ������ٷ��ӳ���
        /// </summary>
        public int caotime
        {
            set { _caotime = value; }
            get { return _caotime; }
        }
        /// <summary>
        /// ������ٷ��ӳ���
        /// </summary>
        public int chongtime
        {
            set { _chongtime = value; }
            get { return _chongtime; }
        }
        /// <summary>
        /// ������ٷ���ȱˮ
        /// </summary>
        public int shuitime
        {
            set { _shuitime = value; }
            get { return _shuitime; }
        }
        /// <summary>
        /// ����ֵ
        /// </summary>
        public int meili
        {
            set { _meili = value; }
            get { return _meili; }
        }
        /// <summary>
        /// ����ֵ
        /// </summary>
        public int tili
        {
            set { _tili = value; }
            get { return _tili; }
        }
        #endregion Model

    }
}

