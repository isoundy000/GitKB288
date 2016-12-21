using System;
using System.Text;
using System.Collections;
using System.Drawing;
using System.Drawing.Imaging;
using System.Reflection;
using System.IO;

namespace BCW.Graph
{
    /// <summary>
    /// EXIFextractor Class
    /// 
    /// </summary>
    public class EXIFextractor : IEnumerable
    {
        /// <summary>
        /// 显示图片EXIF信息
        /// </summary>
        /// <param name="ImgPath">图片地址</param>
        /// <returns></returns>
        public static string  GetExif(string ImgPath)
        {
            //取图片EXIF信息
            System.Text.StringBuilder builder = new System.Text.StringBuilder("");
            try
            {
                string strFile = System.Web.HttpContext.Current.Server.MapPath(ImgPath);
                string PicExt = ImgPath.Substring(ImgPath.LastIndexOf("."), ImgPath.Length - ImgPath.LastIndexOf("."));
                builder.Append("图片格式:" + PicExt.ToUpper().Replace(".", "") + "<br />");
                builder.Append("原图尺寸:" + new ImageHelper().GetPicxywh(strFile, 0) + "<br />");
                builder.Append("分辨率:" + new ImageHelper().GetPicxywh(strFile, 1) + "<br />");
                Bitmap bmp = new Bitmap(strFile);
                EXIFextractor er = new EXIFextractor(ref bmp, "\n");
                // builder.Append(er);
                foreach (BCW.Graph.Pair pr in er)
                {
                    if (pr.First == "相机制造商" || pr.First == "最后编辑" || pr.First == "相机型号" || pr.First == "拍摄于" || pr.First == "编辑软件")
                    {
                        builder.Append(pr.First + ":" + Substring(pr.Second, 0, pr.Second.Length - 1) + "<br />");
                    }
                    else
                    {
                        if (pr.First == "焦距")
                            builder.Append(pr.First + ":" + pr.Second + "mm<br />");
                        else if (pr.First == "快门速度" || pr.First == "曝光时间")
                            builder.Append(pr.First + ":" + pr.Second + "秒<br />");
                        else
                            builder.Append(pr.First + ":" + pr.Second + "<br />");
                    }
                }
                bmp.Dispose();
            }
            catch { }
            return builder.ToString();
        }

        /// <summary
        /// 截取字符串
        /// </summary>
        public static string Substring(string p_strVal, int p_startIndex, int p_length)
        {
            if (string.IsNullOrEmpty(p_strVal))
                return string.Empty;

            p_strVal = p_strVal.Substring(p_startIndex, p_strVal.Length - p_startIndex < p_length ? p_strVal.Length - p_startIndex : p_length);

            return p_strVal;
        }

        public object this[string index]
        {
            get
            {
                return properties[index];
            }
        }
        //
        private System.Drawing.Bitmap bmp;
        //
        private string data;
        //
        private translation myHash;
        //
        private Hashtable properties;
        //
        internal int Count
        {
            get
            {
                return this.properties.Count;
            }
        }
        //
        string sp;
 
        public void setTag(int id, string data)
        {
            Encoding ascii = Encoding.ASCII;
            this.setTag(id, data.Length, 0x2, ascii.GetBytes(data));
        }

        public void setTag(int id, int len, short type, byte[] data)
        {
            PropertyItem p = CreatePropertyItem(type, id, len, data);
            this.bmp.SetPropertyItem(p);
            buildDB(this.bmp.PropertyItems);
        }

        private static PropertyItem CreatePropertyItem(short type, int tag, int len, byte[] value)
        {
            PropertyItem item;

            // Loads a PropertyItem from a Jpeg image stored in the assembly as a resource.
            Assembly assembly = Assembly.GetExecutingAssembly();
            Stream emptyBitmapStream = assembly.GetManifestResourceStream("EXIFextractor.decoy.jpg");
            System.Drawing.Image empty = System.Drawing.Image.FromStream(emptyBitmapStream);

            item = empty.PropertyItems[0];

            // Copies the data to the property item.
            item.Type = type;
            item.Len = len;
            item.Id = tag;
            item.Value = new byte[value.Length];
            value.CopyTo(item.Value, 0);

            return item;
        }
 
        public EXIFextractor(ref System.Drawing.Bitmap bmp, string sp)
        {
            properties = new Hashtable();
            //
            this.bmp = bmp;
            this.sp = sp;
            //
            myHash = new translation();
            buildDB(this.bmp.PropertyItems);
        }
        string msp = "";
        public EXIFextractor(ref System.Drawing.Bitmap bmp, string sp, string msp)
        {
            properties = new Hashtable();
            this.sp = sp;
            this.msp = msp;
            this.bmp = bmp;
            //    
            myHash = new translation();
            this.buildDB(bmp.PropertyItems);

        }
        public static PropertyItem[] GetExifProperties(string fileName)
        {
            FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            System.Drawing.Image image = System.Drawing.Image.FromStream(stream,
                /* useEmbeddedColorManagement = */ true,
                /* validateImageData = */ false);
            return image.PropertyItems;
        }
        public EXIFextractor(string file, string sp, string msp)
        {
            properties = new Hashtable();
            this.sp = sp;
            this.msp = msp;

            myHash = new translation();
            //    
            this.buildDB(GetExifProperties(file));

        }

        /// <summary>
        /// 
        /// </summary>
        private void buildDB(System.Drawing.Imaging.PropertyItem[] parr)
            {
                properties.Clear();
                //
                data = "";
                //
                Encoding ascii = Encoding.ASCII;
                //
                foreach (System.Drawing.Imaging.PropertyItem p in parr)
                {
                    string v = "";
                    string name = (string)myHash[p.Id];
                    // tag not found. skip it
                    if (name == null) continue;
                    //
                    data += name + ": ";
                    //
                    //1 = BYTE An 8-bit unsigned integer.,
                    if (p.Type == 0x1)
                    {
                        v = p.Value[0].ToString();
                    }
                    //2 = ASCII An 8-bit byte containing one 7-bit ASCII code. The final byte is terminated with NULL.,
                    else if (p.Type == 0x2)
                    {
                        // string     
                        v = ascii.GetString(p.Value);
                    }
                    //3 = SHORT A 16-bit (2 -byte) unsigned integer,
                    else if (p.Type == 0x3)
                    {
                        // orientation // lookup table     
                        switch (p.Id)
                        {
                            case 0x8827: // ISO
                                v = "ISO-" + convertToInt16U(p.Value).ToString();
                                break;
                            case 0xA217: // sensing method
                                {
                                    switch (convertToInt16U(p.Value))
                                    {
                                        case 1: v = "Not defined"; break;
                                        case 2: v = "One-chip color area sensor"; break;
                                        case 3: v = "Two-chip color area sensor"; break;
                                        case 4: v = "Three-chip color area sensor"; break;
                                        case 5: v = "Color sequential area sensor"; break;
                                        case 7: v = "Trilinear sensor"; break;
                                        case 8: v = "Color sequential linear sensor"; break;
                                        default: v = " reserved"; break;
                                    }
                                }
                                break;
                            case 0x8822: // aperture 
                                switch (convertToInt16U(p.Value))
                                {
                                    case 0: v = "Not defined"; break;
                                    case 1: v = "Manual"; break;
                                    case 2: v = "Normal program"; break;
                                    case 3: v = "Aperture priority"; break;
                                    case 4: v = "Shutter priority"; break;
                                    case 5: v = "Creative program (biased toward depth of field)"; break;
                                    case 6: v = "Action program (biased toward fast shutter speed)"; break;
                                    case 7: v = "Portrait mode (for closeup photos with the background out of focus)"; break;
                                    case 8: v = "Landscape mode (for landscape photos with the background in focus)"; break;
                                    default: v = "reserved"; break;
                                }
                                break;
                            case 0x9207: // metering mode
                                switch (convertToInt16U(p.Value))
                                {
                                    case 0: v = "unknown"; break;
                                    case 1: v = "Average"; break;
                                    case 2: v = "CenterWeightedAverage"; break;
                                    case 3: v = "Spot"; break;
                                    case 4: v = "MultiSpot"; break;
                                    case 5: v = "Pattern"; break;
                                    case 6: v = "Partial"; break;
                                    case 255: v = "Other"; break;
                                    default: v = "reserved"; break;
                                }
                                break;
                            case 0x9208: // light source
                                {
                                    switch (convertToInt16U(p.Value))
                                    {
                                        case 0: v = "unknown"; break;
                                        case 1: v = "Daylight"; break;
                                        case 2: v = "Fluorescent"; break;
                                        case 3: v = "Tungsten"; break;
                                        case 17: v = "Standard light A"; break;
                                        case 18: v = "Standard light B"; break;
                                        case 19: v = "Standard light C"; break;
                                        case 20: v = "D55"; break;
                                        case 21: v = "D65"; break;
                                        case 22: v = "D75"; break;
                                        case 255: v = "other"; break;
                                        default: v = "reserved"; break;
                                    }
                                }
                                break;
                            case 0x9209:
                                {
                                    switch (convertToInt16U(p.Value))
                                    {
                                        case 0: v = "Flash did not fire"; break;
                                        case 1: v = "Flash fired"; break;
                                        case 5: v = "Strobe return light not detected"; break;
                                        case 7: v = "Strobe return light detected"; break;
                                        default: v = "reserved"; break;
                                    }
                                }
                                break;
                            default:
                                v = convertToInt16U(p.Value).ToString();
                                break;
                        }
                    }
                    //4 = LONG A 32-bit (4 -byte) unsigned integer,
                    else if (p.Type == 0x4)
                    {
                        // orientation // lookup table     
                        v = convertToInt32U(p.Value).ToString();
                    }
                    //5 = RATIONAL Two LONGs. The first LONG is the numerator and the second LONG expresses the//denominator.,
                    else if (p.Type == 0x5)
                    {
                        // rational
                        byte[] n = new byte[p.Len / 2];
                        byte[] d = new byte[p.Len / 2];
                        Array.Copy(p.Value, 0, n, 0, p.Len / 2);
                        Array.Copy(p.Value, p.Len / 2, d, 0, p.Len / 2);
                        uint a = convertToInt32U(n);
                        uint b = convertToInt32U(d);
                        Rational r = new Rational(a, b);
                        //
                        //convert here
                        //

                        switch (p.Id)
                        {
                            case 0x9202: // aperture
                                v = "F/" + Math.Round(Math.Pow(Math.Sqrt(2), r.ToDouble()), 2).ToString();
                                break;
                            case 0x920A:
                                v = r.ToDouble().ToString();
                                break;
                            case 0x829A:
                                v = r.ToDouble().ToString();
                                break;
                            case 0x829D: // F-number
                                v = "F/" + r.ToDouble().ToString();
                                break;
                            case 0x502d:
                                 v = r.ToDouble().ToString();
                                 break;
                            case 0x502e:
                                 v = r.ToDouble().ToString();
                                break;

                            default:
                                v = r.ToString("/");
                                break;
                        }

                    }
                    //7 = UNDEFINED An 8-bit byte that can take any value depending on the field definition,
                    else if (p.Type == 0x7)
                    {
                        switch (p.Id)
                        {
                            case 0xA300:
                                {
                                    if (p.Value[0] == 3)
                                    {
                                        v = "DSC";
                                    }
                                    else
                                    {
                                        v = "reserved";
                                    }
                                    break;
                                }
                            case 0xA301:
                                if (p.Value[0] == 1)
                                    v = "A directly photographed image";
                                else
                                    v = "Not a directly photographed image";
                                break;
                            default:
                                v = "-";
                                break;
                        }
                    }
                    //9 = SLONG A 32-bit (4 -byte) signed integer (2s complement notation),
                    else if (p.Type == 0x9)
                    {
                        v = convertToInt32(p.Value).ToString();
                    }
                    //10 = SRATIONAL Two SLONGs. The first SLONG is the numerator and the second SLONG is the
                    //denominator.
                    else if (p.Type == 0xA)
                    {

                        // rational
                        byte[] n = new byte[p.Len / 2];
                        byte[] d = new byte[p.Len / 2];
                        Array.Copy(p.Value, 0, n, 0, p.Len / 2);
                        Array.Copy(p.Value, p.Len / 2, d, 0, p.Len / 2);
                        int a = convertToInt32(n);
                        int b = convertToInt32(d);
                        Rational r = new Rational(a, b);
                        //
                        // convert here
                        //
                        switch (p.Id)
                        {
                            case 0x9201: // shutter speed
                                v = "1/" + Math.Round(Math.Pow(2, r.ToDouble()), 2).ToString();
                                break;
                            case 0x9203:
                                v = Math.Round(r.ToDouble(), 4).ToString();
                                break;
                            default:
                                v = r.ToString("/");
                                break;
                        }
                    }
         

                    // add it to the list
                    if (properties[name] == null)
                        properties.Add(name, v);
                    // cat it too
                    data += v;
                    data += this.sp;
                }

            }


        public override string ToString()
        {
            return data;
        }

        int convertToInt32(byte[] arr)
        {
            if (arr.Length != 4)
                return 0;
            else
                return arr[3] << 24 | arr[2] << 16 | arr[1] << 8 | arr[0];
        }

        int convertToInt16(byte[] arr)
        {
            if (arr.Length != 2)
                return 0;
            else
                return arr[1] << 8 | arr[0];
        }

        uint convertToInt32U(byte[] arr)
        {
            if (arr.Length != 4)
                return 0;
            else
                return Convert.ToUInt32(arr[3] << 24 | arr[2] << 16 | arr[1] << 8 | arr[0]);
        }
 
        uint convertToInt16U(byte[] arr)
        {
            if (arr.Length != 2)
                return 0;
            else
                return Convert.ToUInt16(arr[1] << 8 | arr[0]);
        }
        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {

            return (new EXIFextractorEnumerator(this.properties));
        }

        #endregion
    }

    class EXIFextractorEnumerator : IEnumerator
    {
        Hashtable exifTable;
        IDictionaryEnumerator index;

        internal EXIFextractorEnumerator(Hashtable exif)
        {
            this.exifTable = exif;
            this.Reset();
            index = exif.GetEnumerator();
        }

        #region IEnumerator Members

        public void Reset()
        {
            this.index = null;
        }

        public object Current
        {
            get
            {
                return (new Pair(this.index.Key, this.index.Value));
            }
        }

        public bool MoveNext()
        {
            if (index != null && index.MoveNext())
                return true;
            else
                return false;
        }

        #endregion

    }
    public class Pair
    {
        public string First;
        public string Second;
        public Pair(object key, object value)
        {
            this.First = key.ToString();
            this.Second = value.ToString();
        }
    }


    public class translation : Hashtable
    {

        public translation()
        {
            this.Add(0x10f, "相机制造商");
            this.Add(0x110, "相机型号");
            this.Add(0x829a, "曝光时间");
            this.Add(0x8827, "感光度");
            this.Add(0x9003, "拍摄于");
            this.Add(0x9201, "快门速度");
            this.Add(0x9202, "光圈");
            this.Add(0x829d, "F-Number");
            this.Add(0x9204, "曝光补偿");
            this.Add(0x920a, "焦距");
            this.Add(0x131, "编辑软件");
            this.Add(0x132, "最后编辑");

        }
    }

    /// <summary>
    /// private class
    /// </summary>
    internal class Rational
    {
        private int n;
        private int d;
        public Rational(int n, int d)
        {
            this.n = n;
            this.d = d;
            simplify(ref this.n, ref this.d);
        }
        public Rational(uint n, uint d)
        {
            this.n = Convert.ToInt32(n);
            this.d = Convert.ToInt32(d);

            simplify(ref this.n, ref this.d);
        }
        public Rational()
        {
            this.n = this.d = 0;
        }
        public string ToString(string sp)
        {
            if (sp == null) sp = "/";
            return n.ToString() + sp + d.ToString();
        }
        public double ToDouble()
        {
            if (d == 0)
                return 0.0;

            return Math.Round(Convert.ToDouble(n) / Convert.ToDouble(d), 2);
        }
        private void simplify(ref int a, ref int b)
        {
            if (a == 0 || b == 0)
                return;

            int gcd = euclid(a, b);
            a /= gcd;
            b /= gcd;
        }
        private int euclid(int a, int b)
        {
            if (b == 0)
                return a;
            else
                return euclid(b, a % b);
        }
    }
}