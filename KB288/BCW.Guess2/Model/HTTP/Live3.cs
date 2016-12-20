using System;
using System.Collections.Generic;
using System.Text;

namespace TPR2.Model.Http
{
 	/// <summary>
	/// 实体类Live 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class Live3
	{
		public Live3()
		{}
		#region Model
        private string _txtlivellist;
        private string _txtlivetlist;
        private string _txtliveview;

        /// <summary>
        /// 篮球即时比分
        /// </summary>
        public string txtLivellist
        {
            set { _txtlivellist = value; }
            get { return _txtlivellist; }
        }
        /// <summary>
        /// 今天篮球全部赛事
        /// </summary>
        public string txtLivetlist
        {
            set { _txtlivetlist = value; }
            get { return _txtlivetlist; }
        }
        /// <summary>
        /// 详细记录
        /// </summary>
        public string txtLiveView
        {
            set { _txtliveview = value; }
            get { return _txtliveview; }
        }
		#endregion Model

	}
}
