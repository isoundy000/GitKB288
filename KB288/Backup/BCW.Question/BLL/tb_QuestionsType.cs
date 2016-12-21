using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
	/// <summary>
	/// ҵ���߼���tb_QuestionsType ��ժҪ˵����
	/// </summary>
	public class tb_QuestionsType
	{
		private readonly BCW.DAL.tb_QuestionsType dal=new BCW.DAL.tb_QuestionsType();
		public tb_QuestionsType()
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
		/// ����һ������
		/// </summary>
		public int  Add(BCW.Model.tb_QuestionsType model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Update(BCW.Model.tb_QuestionsType model)
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
        /// �õ�Name
        /// </summary>
        public string GetName(int ID)
        {
          return  dal.GetName(ID);
        }
        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public BCW.Model.tb_QuestionsType Gettb_QuestionsType(int ID)
		{
			
			return dal.Gettb_QuestionsType(ID);
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
		/// <returns>IList tb_QuestionsType</returns>
		public IList<BCW.Model.tb_QuestionsType> Gettb_QuestionsTypes(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.Gettb_QuestionsTypes(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

		#endregion  ��Ա����
	}
}

