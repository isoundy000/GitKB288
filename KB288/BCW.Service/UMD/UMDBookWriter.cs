namespace BCW.Service
{
    using ICSharpCode.SharpZipLib.Zip.Compression;
    using System;
    using System.Collections;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Text;

    public class UMDBookWriter
    {
        private CUMDBook _Book = null;
        private int[] _ChaptersOff = null;
        private Random _Random = new Random();
        private string _TotalContent = string.Empty;
        private int _TotalContentLen = 0;
        private ArrayList _TotalImageList = new ArrayList();
        private const int A_32K_BYTE = 0x8000;
        private const byte ACTUAL_WIDTH_S60_HORI = 0xcc;
        private const byte ACTUAL_WIDTH_S60_VERT = 0xac;
        private const byte ACTUAL_WIDTH_SP = 0xa6;
        private const uint BASE_REFN_CHAP_OFF = 0x3000;
        private const uint BASE_REFN_CHAP_STR = 0x4000;
        private const uint BASE_REFN_CONTENT = 0x2000;
        private const uint BASE_REFN_COVER = 0x1000;
        private const uint BASE_REFN_PAGE_OFFSET = 0x7000;
        private const string BEYOND_END_FLAG = "\0";
        private const int BYTE_LEN = 1;
        private const byte COVER_TYPE_BMP = 0;
        private const byte COVER_TYPE_GIF = 2;
        private const byte COVER_TYPE_JPG = 1;
        private const int CURR_VERSION = 1;
        private const short DCTS_CMD_ID_AUTHOR = 3;
        private const short DCTS_CMD_ID_CDS_KEY = 240;
        private const short DCTS_CMD_ID_CHAP_OFF = 0x83;
        private const short DCTS_CMD_ID_CHAP_STR = 0x84;
        private const short DCTS_CMD_ID_CONTENT_ID = 10;
        private const short DCTS_CMD_ID_COVER_PAGE = 130;
        private const short DCTS_CMD_ID_DAY = 6;
        private const short DCTS_CMD_ID_FILE_LENGTH = 11;
        private const short DCTS_CMD_ID_FIXED_LEN = 12;
        private const short DCTS_CMD_ID_GENDER = 7;
        private const short DCTS_CMD_ID_LICENSE_KEY = 0xf1;
        private const short DCTS_CMD_ID_MONTH = 5;
        private const short DCTS_CMD_ID_PAGE_OFFSET = 0x87;
        private const short DCTS_CMD_ID_PUBLISHER = 8;
        private const short DCTS_CMD_ID_REF_CONTENT = 0x81;
        private const short DCTS_CMD_ID_TITLE = 2;
        private const short DCTS_CMD_ID_VENDOR = 9;
        private const short DCTS_CMD_ID_VERSION = 1;
        private const short DCTS_CMD_ID_YEAR = 4;
        private const byte FIXED_LINE_PER_PAGE_S60 = 50;
        private const byte FIXED_LINE_PER_PAGE_SP = 0x19;
        private ArrayList widthData_S60 = new ArrayList();
        private ArrayList widthData_SP = new ArrayList();

        public UMDBookWriter(CUMDBook book)
        {
            this._Book = book;
        }

        private byte CharWidth_S60(string @char, byte fontSize)
        {
            ushort num = @char[0];
            foreach (SWidthData data in this.widthData_S60)
            {
                if (((data.FontSize == fontSize) && (num >= data.rngFrom)) && (num <= data.rngTo))
                {
                    if (data.vCount == 1)
                    {
                        return data.Value[0];
                    }
                    return data.Value[num - data.rngFrom];
                }
            }
            return fontSize;
        }

        private void ClearTempVariables()
        {
            this._TotalContent = string.Empty;
            this._TotalContentLen = 0;
            this._ChaptersOff = null;
            if (this.widthData_S60 == null)
            {
                this.widthData_S60 = new ArrayList();
            }
            else
            {
                this.widthData_S60.Clear();
            }
            if (this.widthData_SP == null)
            {
                this.widthData_SP = new ArrayList();
            }
            else
            {
                this.widthData_SP.Clear();
            }
        }

        private byte[][] CompressTxtContent(Encoding encoding, CChapterList chapters)
        {
            string s = string.Empty;
            int num = 0;
            ArrayList list = new ArrayList(chapters.Count);
            int[] numArray = new int[chapters.Count];
            for (int i = 0; i < chapters.Count; i++)
            {
                string str2 = chapters[i].Content.Replace("\r\n", "\u2029") + "\u2029";
                list.Add(str2);
                s = s + str2;
                numArray[i] = num;
                num += str2.Length * 2;
            }
            byte[] bytes = new byte[num];
            bytes = Encoding.Unicode.GetBytes(s);
            int num3 = 0;
            if ((num % 0x8000) == 0)
            {
                num3 = num / 0x8000;
            }
            else
            {
                num3 = (num / 0x8000) + 1;
            }
            byte[][] bufferArray = new byte[num3][];
            int index = 0;
            byte[] input = new byte[0x8000];
            int num5 = 0;
            for (int j = 0; j < bytes.Length; j++)
            {
                input[num5] = bytes[j];
                num5++;
                if ((num5 == 0x8000) || (j == (bytes.Length - 1)))
                {
                    byte[] output = new byte[0x8000];
                    Deflater deflater = new Deflater(Deflater.BEST_COMPRESSION, false);
                    if (deflater.IsNeedingInput)
                    {
                        deflater.SetInput(input, 0, input.Length);
                    }
                    deflater.Finish();
                    deflater.Deflate(output);
                    bufferArray[index] = new byte[deflater.TotalOut];
                    Deflater deflater2 = new Deflater(Deflater.BEST_COMPRESSION, false);
                    if (deflater2.IsNeedingInput)
                    {
                        deflater2.SetInput(input, 0, input.Length);
                    }
                    deflater2.Finish();
                    deflater2.Deflate(bufferArray[index]);
                    index++;
                    input = null;
                    input = new byte[0x8000];
                    num5 = 0;
                }
            }
            return bufferArray;
        }

        private bool GetPageOffsetS60(byte size, uint actualWidth, out uint[] result)
        {
            if ((size != 0x10) && (size != 12))
            {
                result = new uint[0];
                return false;
            }
            this.GetWidthData_S60();
            ArrayList pagesOff = new ArrayList();
            pagesOff.Add(0);
            while (((uint) pagesOff[pagesOff.Count - 1]) < this._TotalContent.Length)
            {
                this.ParseOnePage((uint) (pagesOff.Count - 1), size, actualWidth, ref pagesOff, 1);
            }
            result = new uint[pagesOff.Count];
            for (int i = 0; i < pagesOff.Count; i++)
            {
                result[i] = ((uint) pagesOff[i]) * 2;
            }
            return true;
        }

        private bool GetPageOffsetSP(byte size, uint actualWidth, out uint[] result)
        {
            if ((size < 6) || (size > 0x10))
            {
                result = new uint[0];
                return false;
            }
            ArrayList pagesOff = new ArrayList();
            pagesOff.Add(0);
            while (((uint) pagesOff[pagesOff.Count - 1]) < this._TotalContent.Length)
            {
                this.ParseOnePage((uint) (pagesOff.Count - 1), size, actualWidth, ref pagesOff, 5);
            }
            result = new uint[pagesOff.Count];
            for (int i = 0; i < pagesOff.Count; i++)
            {
                result[i] = ((uint) pagesOff[i]) * 2;
            }
            return true;
        }

        private void GetWidthData_S60()
        {
            if (this.widthData_S60.Count == 0)
            {
                for (int i = 0; i < 2; i++)
                {
                    string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\FontWidthData\S60CHS." + ((i == 0) ? "S16" : "S12") + ".wdt";
                    if (File.Exists(path))
                    {
                        FileStream input = new FileStream(path, FileMode.Open, FileAccess.Read);
                        BinaryReader reader = new BinaryReader(input);
                        while (reader.BaseStream.Position < reader.BaseStream.Length)
                        {
                            SWidthData data = new SWidthData();
                            data.FontSize = (i == 0) ? ((byte) 0x10) : ((byte) 12);
                            data.rngFrom = reader.ReadUInt16();
                            data.rngTo = reader.ReadUInt16();
                            data.vCount = reader.ReadUInt16();
                            data.Value = reader.ReadBytes((int) data.vCount);
                            this.widthData_S60.Add(data);
                        }
                        reader.Close();
                        input.Close();
                    }
                }
            }
        }

        private void GetWidthData_SP()
        {
            if (this.widthData_SP.Count == 0)
            {
                for (int i = 6; i < 0x10; i++)
                {
                    string path = string.Concat(new object[] { Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"\FontWidthData\sunfon.s", i, ".wdt" });
                    if (File.Exists(path))
                    {
                        FileStream input = new FileStream(path, FileMode.Open, FileAccess.Read);
                        BinaryReader reader = new BinaryReader(input);
                        while (reader.BaseStream.Position < reader.BaseStream.Length)
                        {
                            SWidthData data = new SWidthData();
                            data.FontSize = (byte) i;
                            data.rngFrom = reader.ReadUInt16();
                            data.rngTo = reader.ReadUInt16();
                            data.vCount = reader.ReadUInt16();
                            data.Value = reader.ReadBytes((int) data.vCount);
                            this.widthData_SP.Add(data);
                        }
                        reader.Close();
                        input.Close();
                    }
                }
            }
        }

        public string MakeImageBook(BinaryWriter writer)
        {
            this.WriteChaptersOff(writer);
            this.WriteChapterTitles(writer);
            if (this._Book.BookType[1] == 2)
            {
                this.WriteChapterImageContent(writer, 14);
            }
            else if (this._Book.BookType[1] == 3)
            {
                this.WriteChapterImageContent(writer, 15);
            }
            if (this._Book.Cover != null)
            {
                this.WriteBookCover(writer);
            }
            writer.Write('#');
            writer.Write((short) 0xf1);
            writer.Write((byte) 0);
            writer.Write((byte) 0x15);
            writer.Write(Encoding.ASCII.GetBytes("\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0"));
            return null;
        }

        public string MakeTxtBook(BinaryWriter writer)
        {
            uint[] numArray;
            Encoding bookEncoding = this._Book.BookEncoding;
            writer.Write('#');
            writer.Write((short) 11);
            writer.Write((byte) 0);
            writer.Write((byte) 9);
            writer.Write(this._TotalContentLen);
            this.WriteChaptersOff(writer);
            this.WriteChapterTitles(writer);
            if (this._Book.BookType[0] == 1)
            {
                this.WriteChapterTxtContents(writer);
            }
            if (this._Book.Cover != null)
            {
                this.WriteBookCover(writer);
            }
            this.GetPageOffsetS60(0x10, 0xcc, out numArray);
            this.WritePageOffset(0x10, 0xd0, ref numArray, writer, 1);
            this.GetPageOffsetS60(0x10, 0xac, out numArray);
            this.WritePageOffset(0x10, 0xb0, ref numArray, writer, 1);
            this.GetPageOffsetS60(12, 0xcc, out numArray);
            this.WritePageOffset(12, 0xd0, ref numArray, writer, 1);
            this.GetPageOffsetS60(12, 0xac, out numArray);
            this.WritePageOffset(12, 0xb0, ref numArray, writer, 1);
            this.GetPageOffsetSP(10, 0xa6, out numArray);
            this.WritePageOffset(10, 0xa6, ref numArray, writer, 5);
            return null;
        }

        private void ParseOnePage(uint pageNumber, byte fontSize, uint screenWidth, ref ArrayList pagesOff, int PID)
        {
            if (pageNumber < pagesOff.Count)
            {
                uint num = (uint) pagesOff[(int) pageNumber];
                int num2 = 0;
                string str = string.Empty;
                ArrayList list = new ArrayList();
                byte num3 = (PID == 1) ? ((byte) 50) : ((byte) 0x19);
                for (byte i = 0; i < num3; i = (byte) (i + 1))
                {
                    str = string.Empty;
                    string str2 = string.Empty;
                    byte num5 = 0;
                Label_0053:
                    if (num < this._TotalContent.Length)
                    {
                        str2 = this._TotalContent.Substring((int) num, 1);
                    }
                    else
                    {
                        str2 = "\0";
                    }
                    switch (str2)
                    {
                        case "\t":
                        case "\0":
                            str2 = "銆€";
                            break;
                    }
                    byte num6 = this.CharWidth_S60(str2, fontSize);
                    if (str2 == "\u2029")
                    {
                        num6 = 0;
                    }
                    if ((num6 + num5) <= screenWidth)
                    {
                        num5 = (byte) (num5 + num6);
                        num++;
                        if (str2 != "\u2029")
                        {
                            str = str + str2;
                            goto Label_0053;
                        }
                    }
                    if (str2 != "\u2029")
                    {
                        list.Add(str.Length);
                    }
                    else
                    {
                        list.Add(str.Length + 1);
                    }
                    num2 += (int) list[i];
                    if (i == ((byte) (num3 - 1)))
                    {
                        if ((num < this._TotalContent.Length) && (num > ((uint) pagesOff[pagesOff.Count - 1])))
                        {
                            pagesOff.Add(((uint) num2) + ((uint) pagesOff[pagesOff.Count - 1]));
                        }
                        if (num >= this._TotalContent.Length)
                        {
                            pagesOff.Add((uint) this._TotalContent.Length);
                        }
                    }
                }
            }
        }

        private string Precheck()
        {
            if ((this._Book.BookTitle == null) || (this._Book.BookTitle.Length < 1))
            {
                return "标题不能为空！";
            }
            if ((this._Book.Author == null) || (this._Book.Author.Length < 1))
            {
                return "作者不能为空！";
            }
            if ((this._Book.Chapters == null) || (this._Book.Chapters.Count < 1))
            {
                return "内容数量不能小于0！";
            }
            if ((this._Book.BookPath == null) || (this._Book.BookPath.Length < 1))
            {
                return "保存文件的路径不不正确！";
            }
            if (File.Exists(this._Book.BookPath))
            {
                File.Delete(this._Book.BookPath);
            }
            return null;
        }

        private string Prepare()
        {
            string str = string.Empty;
            int[] numArray = new int[this._Book.Chapters.Count];
            int num = 0;
            CChapter chapter = null;
            string str2 = null;
            for (int i = 0; i < this._Book.Chapters.Count; i++)
            {
                chapter = this._Book.Chapters[i];
                numArray[i] = num;/*http://www.joymo.cn,角摩手机乐园*/
                if (this._Book.BookType[1] == 2)
                {
                    this._TotalImageList.AddRange(chapter.ImageList);
                    num += chapter.ImageList.Count;
                }
                else if (this._Book.BookType[1] == 1)
                {
                    str2 = chapter.Content.Replace("\r\n", "\u2029") + "\u2029";
                    str = str + str2;
                    num += str2.Length * 2;
                }
            }
            this._TotalContent = str;
            this._TotalContentLen = num;
            this._ChaptersOff = numArray;
            return null;
        }

        public string Write()
        {
            string message = this.Precheck();
            if (message == null)
            {
                this.ClearTempVariables();
                message = this.Prepare();
                if (message != null)
                {
                    return message;
                }
                BinaryWriter writer = null;
                FileStream output = new FileStream(this._Book.BookPath, FileMode.OpenOrCreate, FileAccess.Write);
                try
                {
                    writer = new BinaryWriter(output);
                    short num = (short) (this._Random.Next(0x401, 0x7fff) % 0xffff);
                    writer.Write((uint) 0xde9a9b89);
                    writer.Write('#');
                    writer.Write((short) 1);
                    writer.Write((byte) 0);
                    writer.Write((byte) 8);
                    writer.Write(this._Book.BookType[1]);
                    writer.Write(num);
                    this.WriteBookProperties(writer);
                    if (this._Book.BookType[1] == 1)
                    {
                        this.MakeTxtBook(writer);
                    }
                    else if (this._Book.BookType[1] == 2)
                    {
                        this.MakeImageBook(writer);
                    }
                    writer.Write('#');
                    writer.Write((short) 12);
                    writer.Write((byte) 1);
                    writer.Write((byte) 9);
                    writer.Write((uint) (((uint) writer.BaseStream.Position) + 4));
                }
                catch (Exception exception)
                {
                    message = exception.Message;
                }
                finally
                {
                    if (writer != null)
                    {
                        writer.Close();
                        writer = null;
                    }
                    output.Close();
                    output = null;
                }
            }
            return message;
        }

        private void WriteBookCover(BinaryWriter Writer)
        {
            uint num = (uint) (0x1000 + this._Random.Next(0xfff));
            byte[] buffer = null;
            MemoryStream stream = new MemoryStream();
            this._Book.Cover.Save(stream, ImageFormat.Gif);
            stream.Seek(0L, SeekOrigin.Begin);
            buffer = new byte[stream.Length];
            stream.Read(buffer, 0, buffer.Length);
            stream.Close();
            stream = null;
            Writer.Write('#');
            Writer.Write((short) 130);
            Writer.Write((byte) 1);
            Writer.Write((byte) 10);
            Writer.Write((byte) 1);
            Writer.Write(num);
            Writer.Write('$');
            Writer.Write(num);
            Writer.Write((uint) (9 + buffer.Length));
            Writer.Write(buffer);
            buffer = null;
        }

        private void WriteBookProperties(BinaryWriter Writer)
        {
            Encoding bookEncoding = this._Book.BookEncoding;
            string propertyValue = this._Book.PublishDate.Year.ToString();
            string str2 = this._Book.PublishDate.Month.ToString();
            string str3 = this._Book.PublishDate.Day.ToString();
            this.WriteBookProperty(Writer, this._Book.BookTitle, 2);
            this.WriteBookProperty(Writer, this._Book.Author, 3);
            this.WriteBookProperty(Writer, propertyValue, 4);
            this.WriteBookProperty(Writer, str2, 5);
            this.WriteBookProperty(Writer, str3, 6);
            if ((this._Book.BookKind != null) && (this._Book.BookKind.Length > 0))
            {
                this.WriteBookProperty(Writer, this._Book.BookKind, 7);
            }
            if ((this._Book.Publisher != null) && (this._Book.Publisher.Length > 0))
            {
                this.WriteBookProperty(Writer, this._Book.Publisher, 8);
            }
            if ((this._Book.Vendor != null) && (this._Book.Vendor.Length > 0))
            {
                this.WriteBookProperty(Writer, this._Book.Vendor, 9);
            }
        }

        private void WriteBookProperty(BinaryWriter Writer, string PropertyValue, int Type)
        {
            Writer.Write('#');
            Writer.Write((short) Type);
            Writer.Write((byte) 0);
            Writer.Write((byte) (5 + (PropertyValue.Length * 2)));
            Writer.Write(this._Book.BookEncoding.GetBytes(PropertyValue));
        }

        private void WriteChapterImageContent(BinaryWriter writer, int SegType)
        {
            int num = this._Random.Next(0x5f5e101, 0x3b9aca00);
            uint[] numArray = new uint[this._TotalImageList.Count];
            uint num2 = 0;
            if (this._TotalImageList.Count > 1)
            {
                num2 = (uint) this._Random.Next(0, this._TotalImageList.Count - 1);
            }
            writer.Write('#');
            writer.Write((short) SegType);
            writer.Write((byte) 0);
            writer.Write((byte) 6);
            writer.Write((byte) 1);
            byte[] buffer = null;
            for (int i = 0; i < this._TotalImageList.Count; i++)
            {
                Image image = (Image) this._TotalImageList[i];
                MemoryStream stream = new MemoryStream();
                image.Save(stream, ImageFormat.Jpeg);
                stream.Seek(0L, SeekOrigin.Begin);
                buffer = new byte[stream.Length];
                stream.Read(buffer, 0, buffer.Length);
                stream.Close();
                stream = null;
                numArray[i] = (uint) (this._Random.Next(1, 0xfffffff) * -1);
                writer.Write('$');
                writer.Write(numArray[i]);
                writer.Write((uint) (9 + buffer.Length));
                writer.Write(buffer);
                if (num2 == i)
                {
                    writer.Write('#');
                    writer.Write((short) 10);
                    writer.Write((byte) 0);
                    writer.Write((byte) 9);
                    writer.Write(num);
                }
            }
            num2 = (uint) (0x2000 + this._Random.Next(0xfff));
            writer.Write('#');
            writer.Write((short) 0x81);
            writer.Write((byte) 1);
            writer.Write((byte) 9);
            writer.Write(num2);
            writer.Write('$');
            writer.Write(num2);
            writer.Write((uint) (9 + (numArray.Length * 4)));
            for (int j = 0; j < numArray.Length; j++)
            {
                writer.Write(numArray[j]);
            }
        }

        private void WriteChaptersOff(BinaryWriter writer)
        {
            uint num = (uint) (0x3000 + this._Random.Next(0xfff));
            writer.Write('#');
            writer.Write((short) 0x83);
            writer.Write((byte) 1);
            writer.Write((byte) 9);
            writer.Write(num);
            writer.Write('$');
            writer.Write(num);
            writer.Write((uint) (9 + (this._ChaptersOff.Length * 4)));
            for (int i = 0; i < this._ChaptersOff.Length; i++)
            {
                writer.Write(this._ChaptersOff[i]);
            }
        }

        private void WriteChapterTitles(BinaryWriter Writer)
        {
            uint num = (uint) (0x4000 + this._Random.Next(0xfff));
            int num2 = 0;
            foreach (CChapter chapter in this._Book.Chapters)
            {
                num2 += (chapter.Title.Length * 2) + 1;
            }
            Writer.Write('#');
            Writer.Write((short) 0x84);
            Writer.Write((byte) 1);
            Writer.Write((byte) 9);
            Writer.Write(num);
            Writer.Write('$');
            Writer.Write(num);
            Writer.Write((uint) (9 + num2));
            foreach (CChapter chapter2 in this._Book.Chapters)
            {
                Writer.Write((byte) (chapter2.Title.Length * 2));
                Writer.Write(this._Book.BookEncoding.GetBytes(chapter2.Title));
            }
        }

        private void WriteChapterTxtContents(BinaryWriter Writer)
        {
            Random random = new Random();
            int num2 = random.Next(0x5f5e101, 0x3b9aca00);
            byte[][] bufferArray = this.CompressTxtContent(this._Book.BookEncoding, this._Book.Chapters);
            int num3 = 0;
            int num4 = 0;
            if (bufferArray.Length > 1)
            {
                num3 = random.Next(0, bufferArray.Length - 1);
                num4 = random.Next(0, bufferArray.Length - 1);
            }
            uint[] numArray = new uint[bufferArray.Length];
            for (int i = 0; i < bufferArray.Length; i++)
            {
                numArray[i] = (uint) (random.Next(1, 0xfffffff) * -1);
                Writer.Write('$');
                Writer.Write(numArray[i]);
                Writer.Write((uint) (9 + bufferArray[i].Length));
                Writer.Write(bufferArray[i]);
                if (i == num3)
                {
                    Writer.Write('#');
                    Writer.Write((short) 0xf1);
                    Writer.Write((byte) 0);
                    Writer.Write((byte) 0x15);
                    Writer.Write(Encoding.ASCII.GetBytes("\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0"));
                }
                if (i == num4)
                {
                    Writer.Write('#');
                    Writer.Write((short) 10);
                    Writer.Write((byte) 0);
                    Writer.Write((byte) 9);
                    Writer.Write(num2);
                }
            }
            uint num = (uint) (0x2000 + random.Next(0xfff));
            Writer.Write('#');
            Writer.Write((short) 0x81);
            Writer.Write((byte) 1);
            Writer.Write((byte) 9);
            Writer.Write(num);
            Writer.Write('$');
            Writer.Write(num);
            Writer.Write((uint) (9 + (numArray.Length * 4)));
            for (int j = 0; j < numArray.Length; j++)
            {
                Writer.Write(numArray[j]);
            }
        }

        private void WritePageOffset(byte fontSize, byte screenWidth, ref uint[] data, BinaryWriter writer, byte PID)
        {
            Random random = new Random();
            uint num = (uint) (0x7000 + random.Next(0xfff));
            writer.Write('#');
            writer.Write((short) 0x87);
            writer.Write(PID);
            writer.Write((byte) 11);
            writer.Write(fontSize);
            writer.Write(screenWidth);
            writer.Write(num);
            writer.Write('$');
            writer.Write(num);
            writer.Write((uint) (9 + (data.Length * 4)));
            foreach (uint num2 in data)
            {
                writer.Write(num2);
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct SWidthData
        {
            public byte FontSize;
            public ushort rngFrom;
            public ushort rngTo;
            public uint vCount;
            public byte[] Value;
        }
    }
}
