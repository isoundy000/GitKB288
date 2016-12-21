using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
	/// <summary>
	/// ҵ���߼���tb_QuestionController ��ժҪ˵����
	/// </summary>
	public class tb_QuestionController
	{
		private readonly BCW.DAL.tb_QuestionController dal=new BCW.DAL.tb_QuestionController();
		public tb_QuestionController()
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
		public int  Add(BCW.Model.tb_QuestionController model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Update(BCW.Model.tb_QuestionController model)
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
        //�õ���󽱽�
        public int GetMaxAwardForID(int ID)
        {
          return  dal.GetMaxAwardForID(ID);
        }
        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public BCW.Model.tb_QuestionController Gettb_QuestionController(int ID)
		{
			
			return dal.Gettb_QuestionController(ID);
		}
        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public BCW.Model.tb_QuestionController Gettb_QuestionController(int uid,int contrID)
        {

            return dal.Gettb_QuestionController(uid, contrID);
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
		/// <returns>IList tb_QuestionController</returns>
		public IList<BCW.Model.tb_QuestionController> Gettb_QuestionControllers(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.Gettb_QuestionControllers(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

		#endregion  ��Ա����
	}
}

