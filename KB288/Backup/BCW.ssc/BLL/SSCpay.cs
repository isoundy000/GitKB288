using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
namespace BCW.ssc.BLL
{
	/// <summary>
	/// ҵ���߼���SSCpay ��ժҪ˵����
	/// </summary>
	public class SSCpay
	{
		private readonly BCW.ssc.DAL.SSCpay dal=new BCW.ssc.DAL.SSCpay();
		public SSCpay()
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
        /// �Ƿ���ڶҽ���¼
        /// </summary>
        public bool ExistsState(int ID, int UsID)
        {
            return dal.ExistsState(ID, UsID);
        }
        /// <summary>
        /// ���ڻ�����
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="WinUserID"></param>
        /// <returns></returns>
        public bool ExistsReBot(int ID, int UsID)
        {
            return dal.ExistsReBot(ID, UsID);
        }
		/// <summary>
		/// ����һ������
		/// </summary>
		public int  Add(BCW.ssc.Model.SSCpay model)
		{
			return dal.Add(model);
		}

        /// <summary>
        /// ����һ������
        /// </summary>
        public int Add2(BCW.ssc.Model.SSCpay model)
        {
            return dal.Add2(model);
        }

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Update(BCW.ssc.Model.SSCpay model)
		{
			dal.Update(model);
		}
               
        /// <summary>
        /// �����û��ҽ���ʶ
        /// </summary>
        public void UpdateState(int ID, int State)
        {
            dal.UpdateState(ID, State);
        }
        /// <summary>
        /// �����û��ҽ���ʶ
        /// </summary>
        public void UpdateState1(int ID, int State)
        {
            dal.UpdateState1(ID, State);
        }
        /// <summary>
        /// ����ӮǮ
        /// </summary>
        public void UpdateWincent(int ID, long  Wincent)
        {
            dal.UpdateWincent(ID, Wincent);
        }   
        /// <summary>
        /// ����ʱʱ�ʿ������
        /// </summary>
        public void UpdateResult(int SSCId, string Result)
        {
            dal.UpdateResult(SSCId, Result);
        }
        /// <summary>
        /// ����ʱʱ�ʿ������
        /// </summary>
        public void UpdateResult1(int SSCId, string Result)
        {
            dal.UpdateResult1(SSCId, Result);
        }
        /// <summary>
        /// ����ʱʱ�ʿ������
        /// </summary>
        public void UpdateWinNotes(int ID, string WinNotes)
        {
            dal.UpdateWinNotes(ID, WinNotes);
        }
                
        /// <summary>
        /// ������Ϸ�����ñ�
        /// </summary>
        public void UpdateWinCent(int ID, int SSCId, int UsID, string UsName,int Types, long WinCent, string WinNotes)
        {
               string GameName = ub.GetSub("SSCName", "/Controls/ssc.xml");
            if (new BCW.ssc.BLL.SSCpay().GetIsSpier(ID) != 1)//ub.GetSub("SSCGuestSet", "/Controls/ssc.xml") == "0" && 
            {
                new BCW.BLL.Guest().Add(1, UsID, UsName, "����[url=/bbs/game/ssc.aspx]" + GameName + "[/url]:" + SSCId + "��" + OutType(Types) + "�Ѿ������������" + WinCent + ub.Get("SiteBz") + "[url=/bbs/game/ssc.aspx?act=case]>>���϶ҽ�[/url]");//������ʾ��Ϣ,1��ʾ������Ϣ
            }

            dal.UpdateWinCent(ID, WinCent, WinNotes);
            //�������Զ��ҽ�
            if (new BCW.ssc.BLL.SSCpay().GetIsSpier(ID) == 1)
            {
                new BCW.ssc.BLL.SSCpay().UpdateState(ID, 2);
               BCW.ssc.Model.SSClist n=  new BCW.ssc.BLL.SSClist().GetSSClistbySSCId(SSCId);
                new BCW.BLL.User().UpdateiGold(UsID, UsName, WinCent, 11, "" + GameName + "-��[url=./game/ssc.aspx?act=view&amp;id=" + n.ID + "&amp;ptype=2]" + SSCId + "[/url]��-�ҽ�|��ʶID" + ID + "");
            }
        }
        #region ��ע���� OutType
        /// <summary>
        /// ��ע����
        /// </summary>
        /// <param name="Types"></param>
        /// <returns></returns>
        private string OutType(int Types)
        {
            string ptypey = string.Empty;
            string payname1 = string.Empty;
            string odds1 = string.Empty;
            string oddsc1 = string.Empty;
            string rule1 = string.Empty;
            for (int i = 1; i < 57; i++)
            {
                ptypey = ub.GetSub("ptype" + i + "", "/Controls/ssc.xml");
                string[] ptypef = ptypey.Split('#');
                payname1 += "#" + ptypef[0];
                odds1 += "#" + ptypef[1];
                oddsc1 += "#" + ptypef[2];
                rule1 += "#" + ptypef[3];
            }
            string[] payname2 = payname1.Split('#');
            string[] odds2 = odds1.Split('#');
            string[] oddsc2 = oddsc1.Split('#');
            string[] rule2 = rule1.Split('#');
            string pText = string.Empty;

            for (int i = 1; i < 57; i++)
            {
                if (Types == i)
                    pText = payname2[i];
            }

            return pText;
        }
        #endregion

        ///<summary>
        ///ĳ��ĳ��Ͷע��ʽͶ�˶��ٱ�
        /// </summary>
        public long GetSumPricebyTypes(int Types, int SSCId)
        {
            return dal.GetSumPricebyTypes(Types, SSCId);
        }
        ///<summary>
        ///ĳ����ţͶ�˶��ٱ�
        /// </summary>
        public long GetSumPriceby23(int Types, int SSCId,int X)
        {
            return dal.GetSumPriceby23(Types, SSCId,X);
        }
        ///<summary>
        ///ĳ������Ͷ�˶��ٱ�
        /// </summary>
        public long GetSumPriceby27(int Types, int SSCId,int X)
        {
            return dal.GetSumPriceby27(Types, SSCId,X);
        }

        ///<summary>
        ///ĳ�ڴ�СͶ�˶��ٱ�
        /// </summary>
        public long GetSumPricebyDX(int Types, int SSCId, int X)
        {
            return dal.GetSumPricebyDX(Types, SSCId, X);
        }

        ///<summary>
        ///ĳ�ڵ�˫Ͷ�˶��ٱ�
        /// </summary>
        public long GetSumPricebyDS(int Types, int SSCId, int X)
        {
            return dal.GetSumPricebyDS(Types, SSCId, X);
        }

        ///<summary>
        ///ĳ���ܺʹ�СͶ�˶��ٱ�
        /// </summary>
        public long GetSumPricebyHD(int Types, int SSCId, int X)
        {
            return dal.GetSumPricebyHD(Types, SSCId, X);
        }

        ///<summary>
        ///ĳ�ں͵�˫Ͷ�˶��ٱ�
        /// </summary>
        public long GetSumPricebyHDx(int Types, int SSCId, int X)
        {
            return dal.GetSumPricebyHDx(Types, SSCId, X);
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
        public void Delete(string strWhere)
        {

            dal.Delete(strWhere);
        }
                
        /// <summary>
        /// ĳ��ĳID��Ͷ�˶��ٱ�
        /// </summary>
        public long GetSumPrices(int UsID, int SSCId)
        {
            return dal.GetSumPrices(UsID, SSCId);
        }

        /// <summary>
        /// ĳ��ĳͶע��ʽĳID��Ͷ�˶��ٱ�
        /// </summary>
        public long GetSumPrices(int UsID, int SSCId,int ptype)
        {
            return dal.GetSumPrices(UsID, SSCId,ptype);
        }

        /// <summary>
        /// �õ�һ��WinCent
        /// </summary>
        public long GetWinCent(int ID)
        {
            return dal.GetWinCent(ID);
        }
        /// <summary>
        /// �õ�һ��State
        /// </summary>
        public long GetState(int ID)
        {
            return dal.GetState(ID);
        }

        /// <summary>
        /// �õ�һ��IsSpier
        /// </summary>
        public int GetIsSpier(int ID)
        {
            return dal.GetIsSpier(ID);
        }

        /// <summary>
        /// ������������ұ���ֵ
        /// </summary>
        public long GetSumPrices(string strWhere)
        {
            return dal.GetSumPrices(strWhere);
        }

        /// <summary>
        /// �����������㷵��ֵ
        /// </summary>
        public long GetSumWinCent(string strWhere)
        {
            return dal.GetSumWinCent(strWhere);
        }    
        
        /// <summary>
        /// �õ�һ��WinCentNotes
        /// </summary>
        public string GetWinNotes(int ID)
        {

            return dal.GetWinNotes(ID);
        }

		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public BCW.ssc.Model.SSCpay GetSSCpay(int ID)
		{
			
			return dal.GetSSCpay(ID);
		}

        public int GetRecordCount(string strWhere)
        {
            return dal.GetRecordCount(strWhere);
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
		/// <returns>IList SSCpay</returns>
		public IList<BCW.ssc.Model.SSCpay> GetSSCpays(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetSSCpays(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}


        /// <summary>
        /// ȡ�����м�¼
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">��ҳ��С</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <returns>IList SSCpay</returns>
        public IList<BCW.ssc.Model.SSCpay> GetSSCpaysTop(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetSSCpaysTop(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }
		#endregion  ��Ա����
	}
}

