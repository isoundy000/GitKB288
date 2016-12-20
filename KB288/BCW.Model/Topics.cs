using System;
namespace BCW.Model
{
    /// <summary>
    /// 实体类Topics 。(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public class Topics
    {
        public Topics()
        { }
        #region Model
        private int _id;
        private int _leibie;
        private int _nodeid;
        private int _types;
        private string _title;
        private string _content;
        private int _isbr;
        private int _ispc;
        private int _cent;
        private int _selltypes;
        private string _inpwd;
        private string _payid;
        private int _bztype;
        private int _vipleven;
        private int _paixu;
        private int _hidden;
        /// <summary>
        /// 项目ID
        /// </summary>
        public int ID
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 节点ID
        /// </summary>
        public int NodeId
        {
            set { _nodeid = value; }
            get { return _nodeid; }
        }
        /// <summary>
        /// 大类
        /// </summary>
        public int Leibie
        {
            set { _leibie = value; }
            get { return _leibie; }
        }
        /// <summary>
        /// 项目类型
        /// </summary>
        public int Types
        {
            set { _types = value; }
            get { return _types; }
        }
        /// <summary>
        /// 名称
        /// </summary>
        public string Title
        {
            set { _title = value; }
            get { return _title; }
        }
        /// <summary>
        /// 内容
        /// </summary>
        public string Content
        {
            set { _content = value; }
            get { return _content; }
        }
        /// <summary>
        /// 是否换行
        /// </summary>
        public int IsBr
        {
            set { _isbr = value; }
            get { return _isbr; }
        }
        /// <summary>
        /// 访问限制
        /// </summary>
        public int IsPc
        {
            set { _ispc = value; }
            get { return _ispc; }
        }
        /// <summary>
        /// 访问收费多少
        /// </summary>
        public int Cent
        {
            set { _cent = value; }
            get { return _cent; }
        }
        /// <summary>
        /// 购买方式
        /// </summary>
        public int SellTypes
        {
            set { _selltypes = value; }
            get { return _selltypes; }
        }
        /// <summary>
        /// 访问密码
        /// </summary>
        public string InPwd
        {
            set { _inpwd = value; }
            get { return _inpwd; }
        }
        /// <summary>
        /// 按次计费时的购买ID
        /// </summary>
        public string PayId
        {
            set { _payid = value; }
            get { return _payid; }
        }
        /// <summary>
        /// 收费币种
        /// </summary>
        public int BzType
        {
            set { _bztype = value; }
            get { return _bztype; }
        }
        /// <summary>
        /// 进入VIP等级
        /// </summary>
        public int VipLeven
        {
            set { _vipleven = value; }
            get { return _vipleven; }
        }
        /// <summary>
        /// 排序
        /// </summary>
        public int Paixu
        {
            set { _paixu = value; }
            get { return _paixu; }
        }
        /// <summary>
        /// 0正常/1登录可见/2隐藏
        /// </summary>
        public int Hidden
        {
            set { _hidden = value; }
            get { return _hidden; }
        }
        #endregion Model

    }
}

