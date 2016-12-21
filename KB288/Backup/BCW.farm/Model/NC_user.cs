using System;
namespace BCW.farm.Model
{
    /// <summary>
    /// ʵ����NC_user ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
    /// </summary>
    [Serializable]
    public class NC_user
    {
        public NC_user()
        { }
        #region Model
        private int _id;
        private int _usid;
        private int _grade;
        private long _goid;
        private long _experience;
        private int _ischan;
        private int _iszhong;
        private int _iscao;
        private int _iswater;
        private int _isinsect;
        private int _isshou;
        private int _isshifei;
        private DateTime _signtime;
        private int _signtotal;
        private int _signkeep;
        private int _tuditpye;
        private int _big_bozhong;
        private int _big_bangmang;
        private int _big_shihuai;
        private DateTime _updatetime;
        private int _isbaitan;
        private int _shoutype;
        private int _big_shifei;
        private int _big_beicaozuo;
        private int _big_zfcishu;
        private int _big_cccishu;
        private int _big_zjcaozuo;
        private int _zengsongnum;
        private string _jiyu;
        /// <summary>
        /// 
        /// </summary>
        public int ID
        {
            set { _id = value; }
            get { return _id; }
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
        /// �ȼ�
        /// </summary>
        public int Grade
        {
            set { _grade = value; }
            get { return _grade; }
        }
        /// <summary>
        /// ���
        /// </summary>
        public long Goid
        {
            set { _goid = value; }
            get { return _goid; }
        }
        /// <summary>
        /// ����
        /// </summary>
        public long Experience
        {
            set { _experience = value; }
            get { return _experience; }
        }
        /// <summary>
        /// һ������
        /// </summary>
        public int ischan
        {
            set { _ischan = value; }
            get { return _ischan; }
        }
        /// <summary>
        /// һ����ֲ
        /// </summary>
        public int iszhong
        {
            set { _iszhong = value; }
            get { return _iszhong; }
        }
        /// <summary>
        /// һ������
        /// </summary>
        public int iscao
        {
            set { _iscao = value; }
            get { return _iscao; }
        }
        /// <summary>
        /// һ����ˮ
        /// </summary>
        public int iswater
        {
            set { _iswater = value; }
            get { return _iswater; }
        }
        /// <summary>
        /// һ������
        /// </summary>
        public int isinsect
        {
            set { _isinsect = value; }
            get { return _isinsect; }
        }
        /// <summary>
        /// һ���ջ�
        /// </summary>
        public int isshou
        {
            set { _isshou = value; }
            get { return _isshou; }
        }
        /// <summary>
        /// һ��ʩ��
        /// </summary>
        public int isshifei
        {
            set { _isshifei = value; }
            get { return _isshifei; }
        }
        /// <summary>
        /// ǩ��ʱ��
        /// </summary>
        public DateTime SignTime
        {
            set { _signtime = value; }
            get { return _signtime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int SignTotal
        {
            set { _signtotal = value; }
            get { return _signtotal; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int SignKeep
        {
            set { _signkeep = value; }
            get { return _signkeep; }
        }
        /// <summary>
        /// 1��ͨ2��3��4��
        /// </summary>
        public int tuditpye
        {
            set { _tuditpye = value; }
            get { return _tuditpye; }
        }
        /// <summary>
        /// ���������
        /// </summary>
        public int big_bozhong
        {
            set { _big_bozhong = value; }
            get { return _big_bozhong; }
        }
        /// <summary>
        /// ���ݳ��潽ˮ����
        /// </summary>
        public int big_bangmang
        {
            set { _big_bangmang = value; }
            get { return _big_bangmang; }
        }
        /// <summary>
        /// �ֲݷų澭��
        /// </summary>
        public int big_shihuai
        {
            set { _big_shihuai = value; }
            get { return _big_shihuai; }
        }
        /// <summary>
        /// ����ũ�������ʱ��
        /// </summary>
        public DateTime updatetime
        {
            set { _updatetime = value; }
            get { return _updatetime; }
        }
        /// <summary>
        /// 1���԰�̯0������
        /// </summary>
        public int isbaitan
        {
            set { _isbaitan = value; }
            get { return _isbaitan; }
        }
        /// <summary>
        /// �ջ��ж�
        /// </summary>
        public int shoutype
        {
            set { _shoutype = value; }
            get { return _shoutype; }
        }
        /// <summary>
        /// ʩ�ʴ���
        /// </summary>
        public int big_shifei
        {
            set { _big_shifei = value; }
            get { return _big_shifei; }
        }
        /// <summary>
        /// �Լ�ũ�����������
        /// </summary>
        public int big_beicaozuo
        {
            set { _big_beicaozuo = value; }
            get { return _big_beicaozuo; }
        }
        /// <summary>
        /// ����ֲݷų����
        /// </summary>
        public int big_zfcishu
        {
            set { _big_zfcishu = value; }
            get { return _big_zfcishu; }
        }
        /// <summary>
        /// �Լ������ݳ������
        /// </summary>
        public int big_cccishu
        {
            set { _big_cccishu = value; }
            get { return _big_cccishu; }
        }
        /// <summary>
        /// �Լ������ݳ������
        /// </summary>
        public int big_zjcaozuo
        {
            set { _big_zjcaozuo = value; }
            get { return _big_zjcaozuo; }
        }
        /// <summary>
        /// ÿ�����ʹ���
        /// </summary>
        public int zengsongnum
        {
            set { _zengsongnum = value; }
            get { return _zengsongnum; }
        }
        /// <summary>
        /// ũ������
        /// </summary>
        public string jiyu
        {
            set { _jiyu = value; }
            get { return _jiyu; }
        }
        #endregion Model

    }
}

