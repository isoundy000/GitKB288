using System;
using System.Data;
using System.Collections.Generic;
using BCW.Model;
using BCW.Common;
namespace BCW.BLL
{
    /// <summary>
    /// ҵ���߼���Health ��ժҪ˵����
    /// </summary>
    public class Health
    {
        private readonly BCW.DAL.Health dal = new BCW.DAL.Health();
        public Health()
        { }
        #region  ��Ա����

        /// <summary>
        /// ɾ������
        /// </summary>
        public void Delete()
        {

            dal.Delete();
        }
        /// <summary>
        /// ȡ��ÿҳ��¼
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">��ҳ��С</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <returns>IList Topics</returns>
        public IList<BCW.Model.Health> GetHealths(int p_pageIndex, int p_pageSize, out int p_recordCount)
        {
            return dal.GetHealths(p_pageIndex, p_pageSize, out p_recordCount);
        }

        #endregion  ��Ա����
    }
}
