using System;
namespace BCW.farm.Model
{
    /// <summary>
    /// ʵ����NC_hecheng ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
    /// </summary>
    [Serializable]
    public class NC_hecheng
    {
        public NC_hecheng()
        { }
        #region Model
        private int _id;
        private string _title;
        private int _giftid;
        private string _prevpic;
        private int _usid;
        private int _num;
        private DateTime _addtime;
        private int _all_num;
        /// <summary>
        /// 
        /// </summary>
        public int ID
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// �ϳ�����
        /// </summary>
        public string Title
        {
            set { _title = value; }
            get { return _title; }
        }
        /// <summary>
        /// �ϳ�ǰID
        /// </summary>
        public int GiftId
        {
            set { _giftid = value; }
            get { return _giftid; }
        }
        /// <summary>
        /// ͼƬ��ַ
        /// </summary>
        public string PrevPic
        {
            set { _prevpic = value; }
            get { return _prevpic; }
        }
        /// <summary>
        /// �û���
        /// </summary>
        public int UsID
        {
            set { _usid = value; }
            get { return _usid; }
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
        /// �ϳ�ʱ��
        /// </summary>
        public DateTime AddTime
        {
            set { _addtime = value; }
            get { return _addtime; }
        }
        /// <summary>
        /// ������
        /// </summary>
        public int all_num
        {
            set { _all_num = value; }
            get { return _all_num; }
        }
        #endregion Model

    }
}

