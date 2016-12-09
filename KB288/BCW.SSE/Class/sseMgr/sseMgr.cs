using System;
using System.Collections.Generic;
using System.Text;
using BCW.SSE.Class.sseModel;

/// <summary>
/// 枚举：版本类型
/// </summary>
public enum ESseVersionType
{
    sseKB=0,              //酷币
    sseJB                 //金币
}

namespace BCW.SSE.Class.sseMgr
{
      

    public abstract class sseMgrBase
    {
       protected int mVersionId;
       protected string mPageName;        
       protected AccountBase mAccount;

       public sseMgrBase()
       {
           this.Init();
       }

       public abstract void Init();


       public AccountBase account
       {
           get
           {
               return this.mAccount;
           }
       }

        public string pageName
       {
           get
           {
               return this.mPageName;
           }
       }

        public int versionId
        {
            get
            {
                return this.mVersionId;
            }
        }
    }

    public class sseKbMgr : sseMgrBase
    {
        public override void Init()
        {
            this.mAccount = new AccountKB();
            this.mPageName = "SSE.aspx?sseVe=0&amp;";
            this.mVersionId = 0;
        }
    }

    public class sseJbMgr : sseMgrBase
    {
        public override void Init()
        {
            this.mAccount = new AccountJB();
            this.mPageName = "SSE.aspx?sseVe=1&amp;";
            this.mVersionId = 1;
        }
    }

}
