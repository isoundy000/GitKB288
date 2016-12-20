using System;
using System.Security.Cryptography;
using System.IO;
using System.Text;

namespace BCW.BLL
{
    /// 
    /// Security 的摘要说明。 
    /// Security类实现.NET框架下的加密和解密。 
    /// CopyRight KangSoft@Hotmail.com@Hotmail.com@hotmail.com 
    /// 
    public class Security
    {
        string _QueryStringKey = "asdfdfgm"; //URL传输参数加密Key j
        string _PassWordKey = "hgfedcba"; //PassWord加密Key 
        //static int[] offset;
        public Security()
        {
            // 
            // TODO: 在此处添加构造函数逻辑 
            // 
        }

        /// 
        /// 加密URL传输的字符串 
        /// 
        /// 
        /// 
        public string EncryptQueryString( string pToEncrypt)
        {
            return Encrypt(pToEncrypt, _QueryStringKey);
        }

        /// 
        /// 解密URL传输的字符串 
        /// 
        /// 
        /// 
        public string DecryptQueryString(string pToDecrypt)
        {
            return Decrypt(pToDecrypt, _QueryStringKey);
        }

        /// 
        /// 加密帐号口令 
        /// 
        /// 
        /// 
        public string EncryptPassWord(string PassWord)
        {
            return Encrypt(PassWord, _PassWordKey);
        }

        /// 
        /// 解密帐号口令 
        /// 
        /// 
        /// 
        public string DecryptPassWord(string PassWord)
        {
            return Decrypt(PassWord, _PassWordKey);
        }

        /// 
        /// DEC 加密过程 
        /// 
        /// 
        /// 
        /// 
        public string Encrypt(string pToEncrypt, string sKey)//  第一个是加密的字符串，第二个是关键字
        {

            //Random ran = new Random(); 
            //offset[1] = ran.Next(100, 999); 

            DESCryptoServiceProvider des = new DESCryptoServiceProvider(); //把字符串放到byte数组中 

            byte[] inputByteArray = Encoding.Default.GetBytes(pToEncrypt);
            //byte[] inputByteArray=Encoding.Unicode.GetBytes(pToEncrypt); 

            des.Key = ASCIIEncoding.ASCII.GetBytes(sKey); //建立加密对象的密钥和偏移量 
            des.IV = ASCIIEncoding.ASCII.GetBytes(sKey); //原文使用ASCIIEncoding.ASCII方法的GetBytes方法 
            MemoryStream ms = new MemoryStream(); //使得输入密码必须输入英文文本 
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);

            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();

            StringBuilder ret = new StringBuilder();
            foreach (byte b in ms.ToArray())
            {
                ret.AppendFormat("{0:X2}", b);
            }
            ret.ToString();
            return ret.ToString();
        }

        /// 
        /// DEC 解密过程 
        /// 
        /// 
        /// 
        /// 
        public string Decrypt(string pToDecrypt, string sKey)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();

            byte[] inputByteArray = new byte[pToDecrypt.Length / 2];
            for (int x = 0; x < pToDecrypt.Length / 2; x++)
            {
                int i = (Convert.ToInt32(pToDecrypt.Substring(x * 2, 2), 16));
                inputByteArray[x] = (byte)i;
            }
            des.Key = ASCIIEncoding.ASCII.GetBytes(sKey); //建立加密对象的密钥和偏移量，此值重要，不能修改 
            des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();

            StringBuilder ret = new StringBuilder(); //建立StringBuild对象，CreateDecrypt使用的是流对象，必须把解密后的文本变成流对象 

            return System.Text.Encoding.Default.GetString(ms.ToArray());
        }
        public string Code()
        {
            string codeSerial = "0,1,2,3,4,5,6,7,8,9,a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z,A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z";
            string[] arr = codeSerial.Split(',');

            string code = "";

            int randValue = -1;

            Random rand = new Random(unchecked((int)DateTime.Now.Ticks));

            for (int i = 0; i < 4; i++)
            {
                randValue = rand.Next(0, arr.Length - 1);

                code += arr[randValue];
            }

            return code;
        }
        /// 
        /// 检查己加密的字符串是否与原文相同 
        /// 
        /// 
        /// 
        /// 
        /// 
    }
}