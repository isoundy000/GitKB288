using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
	/// <summary>
	/// ҵ���߼���File ��ժҪ˵����
	/// </summary>
	public class File
	{
		private readonly BCW.DAL.File dal=new BCW.DAL.File();
		public File()
		{}
		#region  ��Ա����
		/// <summary>
		/// �Ƿ���ڸü�¼
		/// </summary>
		public bool Exists(int ID)
		{
			return dal.Exists(ID);
		}
                
        /// <summary>
        /// ����ĳ������ļ���
        /// </summary>
        public int GetCount(int NodeId)
        {
            return dal.GetCount(NodeId);
        }

		/// <summary>
		/// ����һ������
		/// </summary>
		public int  Add(BCW.Model.File model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Update(BCW.Model.File model)
		{
			dal.Update(model);
		}
                
        /// <summary>
        /// �������ش���
        /// </summary>
        public void UpdateDownNum(int ID, int DownNum)
        {
            dal.UpdateDownNum(ID, DownNum);
        }

		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public void Delete(int ID)
		{
			
			dal.Delete(ID);
		}
                
        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public void Delete2(int NodeId)
        {
            dal.Delete2(NodeId);
        }

		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public BCW.Model.File GetFile(int ID)
		{
			
			return dal.GetFile(ID);
		}
      
        /// <summary>
        /// �õ�һ��Files
        /// </summary>
        public string GetFiles(int ID)
        {
            return dal.GetFiles(ID);
        }

		/// <summary>
		/// �����ֶ�ȡ�����б�
		/// </summary>
		public DataSet GetList(string strField, string strWhere)
		{
			return dal.GetList(strField, strWhere);
		}

		/// <summary>
		/// ȡ��ÿҳ��¼
		/// </summary>
		/// <param name="p_pageIndex">��ǰҳ</param>
		/// <param name="p_pageSize">��ҳ��С</param>
		/// <param name="p_recordCount">�����ܼ�¼��</param>
		/// <param name="strWhere">��ѯ����</param>
		/// <returns>IList File</returns>
		public IList<BCW.Model.File> GetFiles(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetFiles(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

		#endregion  ��Ա����
	}
}

