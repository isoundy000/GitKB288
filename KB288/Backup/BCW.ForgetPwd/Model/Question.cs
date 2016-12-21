using System;
namespace BCW.Model
{
    /// <summary>
    /// 实体类tb_Question 。(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public class tb_Question
    {
        public tb_Question()
        { }
        #region Model
        private string _question;
        private string _answer;
        private int _state;
        private string _mobile;
        private int? _id;
        private int _lastchange;
        private int _changecount;
        private string _code;
        /// <summary>
        /// 
        /// </summary>
        public string Question
        {
            set { _question = value; }
            get { return _question; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Answer
        {
            set { _answer = value; }
            get { return _answer; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int state
        {
            set { _state = value; }
            get { return _state; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Mobile
        {
            set { _mobile = value; }
            get { return _mobile; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? ID
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int lastchange
        {
            set { _lastchange = value; }
            get { return _lastchange; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int changecount
        {
            set { _changecount = value; }
            get { return _changecount; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string code
        {
            set { _code = value; }
            get { return _code; }
        }
        #endregion Model

    }
}

