using System;
namespace BCW.Baccarat.Model
{
    /// <summary>
    /// ʵ����BJL_user ��(����˵���Զ���ȡ���ݿ��ֶε�������Ϣ)
    /// </summary>
    [Serializable]
    public class BJL_user
    {
        public BJL_user()
        { }
        #region Model
        private int _id;
        private int _usid;
        private int _setshow;
        private int _kainum;
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
        /// ��ʾ��Ч��0����1ͼƬ
        /// </summary>
        public int setshow
        {
            set { _setshow = value; }
            get { return _setshow; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int kainum
        {
            set { _kainum = value; }
            get { return _kainum; }
        }
        #endregion Model

    }
}

