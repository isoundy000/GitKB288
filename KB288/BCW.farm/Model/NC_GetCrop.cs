using System;
namespace BCW.farm.Model
{
    /// <summary>
    /// ʵ����NC_GetCrop ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
    /// </summary>
    [Serializable]
    public class NC_GetCrop
    {
        public NC_GetCrop()
        { }
        #region Model
        private long _id;
        private string _name;
        private int _name_id;
        private int _num;
        private long _price_out;
        private int _usid;
        private int _suoding;
        private int _aa;
        private int _tou_nums;
        private int _get_nums;
        /// <summary>
        /// 
        /// </summary>
        public long id
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
        public int name_id
        {
            set { _name_id = value; }
            get { return _name_id; }
        }
        /// <summary>
        /// ����
        /// </summary>
        public int num
        {
            set { _num = value; }
            get { return _num; }
        }
        /// <summary>
        /// �����ļ�Ǯ
        /// </summary>
        public long price_out
        {
            set { _price_out = value; }
            get { return _price_out; }
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
        /// 0������1����
        /// </summary>
        public int suoding
        {
            set { _suoding = value; }
            get { return _suoding; }
        }
        /// <summary>
        /// ͵�˵Ĺ�ʵ����
        /// </summary>
        public int tou_nums
        {
            set { _tou_nums = value; }
            get { return _tou_nums; }
        }
        /// <summary>
        /// �ջ�Ĺ�ʵ����
        /// </summary>
        public int get_nums
        {
            set { _get_nums = value; }
            get { return _get_nums; }
        }
        #endregion Model

    }
}

