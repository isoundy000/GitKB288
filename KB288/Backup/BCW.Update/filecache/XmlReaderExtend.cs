using System;
using System.Xml;

namespace BCW.Update
{
    /// <summary>
    /// �� XmlReader ���� (��ȱ��, δ����. ��ȡʱ��Ҫע��˳��)
    /// </summary>
    public class XmlReaderExtend : IDisposable
    {
        private XmlReader _Reader;
        private bool _Continue = true;

        #region ����

        public XmlReader xmlReader
        {
            get { return _Reader; }
            set { _Reader = value; }
        }

        /// <summary>
        /// �Ƿ����ִ��
        /// </summary>
        public bool Continue
        {
            get { return _Continue; }
        }

        #endregion ����

        /// <summary>
        /// ���췽��
        /// </summary>
        /// <param name="p_xml">XML �ĵ�</param>
        public XmlReaderExtend(string p_xml)
        {
            XmlReaderSettings _ReaderSetting = new XmlReaderSettings();
            _ReaderSetting.ValidationType = ValidationType.None;
            _ReaderSetting.IgnoreComments = true;
            _ReaderSetting.IgnoreWhitespace = true;

            _Reader = XmlReader.Create(new XmlTextReader(new System.IO.StringReader(p_xml)), _ReaderSetting);
        }

        /// <summary>
        /// һֱ��ȡ��ֱ���ҵ�����ָ���޶�����Ԫ��
        /// </summary>
        /// <param name="p_name">Ԫ������</param>
        /// <returns>����ҵ�ƥ���Ԫ�أ���Ϊ true������Ϊ false �� XmlReader λ���ļ���ĩβ</returns>
        public bool ReadToFollowing(string p_name)
        {
            return _Reader.ReadToFollowing(p_name);
        }

        /// <summary>
        /// ǰ������һ������ָ���޶�����ͬ��Ԫ��
        /// </summary>
        /// <param name="p_name">Ԫ������</param>
        /// <returns>����ҵ�ƥ���Ԫ�أ���Ϊ true������Ϊ false �� XmlReader �ᶨλ�ڸ�Ԫ�صĽ������</returns>
        public bool ReadToNextSibling(string p_name)
        {
            return _Reader.ReadToNextSibling(p_name);
        }

        /// <summary>
        /// ȡ��Ԫ������
        /// </summary>
        public string GetElementName()
        {
            string strVal = string.Empty;
            strVal = _Reader.Name;

            return strVal;
        }

        /// <summary>
        /// ȡ��Ԫ��ֵ
        /// </summary>
        public string GetElementValue()
        {
            string strVal = string.Empty;
            strVal = _Reader.ReadString();

            return strVal;
        }

        /// <summary>
        /// ȡ��Ԫ��ֵ
        /// </summary>
        /// <param name="p_elementName">Ԫ������</param>
        public string GetElementValue(string p_elementName)
        {
            string strVal = string.Empty;

            if (_Reader.ReadToFollowing(p_elementName))
                strVal = _Reader.ReadString();

            return strVal;
        }

        /// <summary>
        /// ȡ������ֵ (�˷������ƶ���ȡ��)
        /// </summary>
        /// <param name="p_attributeName">��������</param>
        public string GetAttributeValue(string p_attributeName)
        {
            string strVal = string.Empty;
            strVal = _Reader.GetAttribute(p_attributeName);

            return strVal;
        }

        /// <summary>
        /// ȡ������ֵ
        /// </summary>
        /// <param name="p_elementName">Ԫ������</param>
        /// <param name="p_attributeName">��������</param>
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
