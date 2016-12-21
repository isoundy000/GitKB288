using System;
using System.Collections.Generic;
using System.Text;

namespace BCW.HP3.Model
{
    /// <summary>
    /// SWB:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class SWB
    {
        public SWB()
        { }
        #region Model
        private int _userid;
        private long _hp3money;
        private DateTime _hp3isget;
        /// <summary>
        /// 
        /// </summary>
        public int UserID
        {
            set { _userid = value; }
            get { return _userid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public long HP3Money
        {
            set { _hp3money = value; }
            get { return _hp3money; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime HP3IsGet
        {
            set { _hp3isget = value; }
            get { return _hp3isget; }
        }
        #endregion Model

    }
}
