using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.bydr.Model;
namespace BCW.bydr.BLL
{
	/// <summary>
	/// ҵ���߼���CmgDaoju ��ժҪ˵����
	/// </summary>
	public class CmgDaoju
	{
		private readonly BCW.bydr.DAL.CmgDaoju dal=new BCW.bydr.DAL.CmgDaoju();
		public CmgDaoju()
		{}
		#region  ��Ա����
        /// <summary>
        /// �õ����ID
        /// </summary>
        public int GetMaxId()
        {
            return dal.GetMaxId();
        }
		/// <summary>
		/// �Ƿ���ڸü�¼
		/// </summary>
		public bool Exists(int ID)
		{
			return dal.Exists(ID);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public int  Add(BCW.bydr.Model.CmgDaoju model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Update(BCW.bydr.Model.CmgDaoju model)
		{
			dal.Update(model);
		}

        
		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public void Delete(int ID)
		{
			
			dal.Delete(ID);
		}

		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public BCW.bydr.Model.CmgDaoju GetCmgDaoju(int ID)
		{
			
			return dal.GetCmgDaoju(ID);
		}
        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public string GetyuName(int ID)
        {
            return dal.GetyuName(ID);
        }
        ///// <summary>
        ///// �õ���������
        ///// </summary>
        //public int Getchangj2(int changj2,string Changjing)
        //{
        //    return dal.Getchangj2(changj2, Changjing);
        //}


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
		/// <returns>IList CmgDaoju</returns>
		public IList<BCW.bydr.Model.CmgDaoju> GetCmgDaojus(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetCmgDaojus(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

		#endregion  ��Ա����

      
    }
}

