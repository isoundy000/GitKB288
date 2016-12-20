using System;
using System.Xml;

namespace BCW.Update
{
    /// <summary>
    /// 简化 XmlReader 操作 (有缺点, 未处理. 读取时候要注意顺序)
    /// </summary>
    public class XmlReaderExtend : IDisposable
    {
        private XmlReader _Reader;
        private bool _Continue = true;

        #region 属性

        public XmlReader xmlReader
        {
            get { return _Reader; }
            set { _Reader = value; }
        }

        /// <summary>
        /// 是否继续执行
        /// </summary>
        public bool Continue
        {
            get { return _Continue; }
        }

        #endregion 属性

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="p_xml">XML 文档</param>
        public XmlReaderExtend(string p_xml)
        {
            XmlReaderSettings _ReaderSetting = new XmlReaderSettings();
            _ReaderSetting.ValidationType = ValidationType.None;
            _ReaderSetting.IgnoreComments = true;
            _ReaderSetting.IgnoreWhitespace = true;

            _Reader = XmlReader.Create(new XmlTextReader(new System.IO.StringReader(p_xml)), _ReaderSetting);
        }

        /// <summary>
        /// 一直读取，直到找到具有指定限定名的元素
        /// </summary>
        /// <param name="p_name">元素名称</param>
        /// <returns>如果找到匹配的元素，则为 true；否则为 false 且 XmlReader 位于文件的末尾</returns>
        public bool ReadToFollowing(string p_name)
        {
            return _Reader.ReadToFollowing(p_name);
        }

        /// <summary>
        /// 前进到下一个具有指定限定名的同级元素
        /// </summary>
        /// <param name="p_name">元素名称</param>
        /// <returns>如果找到匹配的元素，则为 true；否则为 false 且 XmlReader 会定位在父元素的结束标记</returns>
        public bool ReadToNextSibling(string p_name)
        {
            return _Reader.ReadToNextSibling(p_name);
        }

        /// <summary>
        /// 取得元素名称
        /// </summary>
        public string GetElementName()
        {
            string strVal = string.Empty;
            strVal = _Reader.Name;

            return strVal;
        }

        /// <summary>
        /// 取得元素值
        /// </summary>
        public string GetElementValue()
        {
            string strVal = string.Empty;
            strVal = _Reader.ReadString();

            return strVal;
        }

        /// <summary>
        /// 取得元素值
        /// </summary>
        /// <param name="p_elementName">元素名称</param>
        public string GetElementValue(string p_elementName)
        {
            string strVal = string.Empty;

            if (_Reader.ReadToFollowing(p_elementName))
                strVal = _Reader.ReadString();

            return strVal;
        }

        /// <summary>
        /// 取得属性值 (此方法不移动读取器)
        /// </summary>
        /// <param name="p_attributeName">属性名称</param>
        public string GetAttributeValue(string p_attributeName)
        {
            string strVal = string.Empty;
            strVal = _Reader.GetAttribute(p_attributeName);

            return strVal;
        }

        /// <summary>
        /// 取得属性值
        /// </summary>
        /// <param name="p_elementName">元素名称</param>
        /// <param name="p_attributeName">属性名称</param>
        public string GetAttributeValue(string p_elementName, string p_attributeName)
        {
            string strVal = string.Empty;

            if (_Reader.ReadToFollowing(p_elementName))
                strVal = _Reader.GetAttribute(p_attributeName);

            return strVal;
        }

        #region IDisposable

        public void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool disposing)
        {
            if (_Reader.ReadState != ReadState.Closed)
                _Reader.Close();
        }

        #endregion IDisposable
    }
}
