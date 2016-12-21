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

    public class CUMDFile
    {
        private const int A_32K_BYTE = 0x8000;
        private const byte ACTUAL_WIDTH_S60_HORI = 0xcc;
        private const byte ACTUAL_WIDTH_S60_VERT = 0xac;
        private const byte ACTUAL_WIDTH_SP = 0xa6;
        private uint additionalCheck;
        private string author = string.Empty;
        private const uint BASE_REFN_CHAP_OFF = 0x3000;
        private const uint BASE_REFN_CHAP_STR = 0x4000;
        private const uint BASE_REFN_CONTENT = 0x2000;
        private const uint BASE_REFN_COVER = 0x1000;
        private const uint BASE_REFN_PAGE_OFFSET = 0x7000;
        private const string BEYOND_END_FLAG = "\0";
        private const int BYTE_LEN = 1;
        private int[] chapOff;
        private CChapterList chapters = new CChapterList();
        private int cid;
        private int contentLength;
        private Image cover;
        private const byte COVER_TYPE_BMP = 0;
        private const byte COVER_TYPE_GIF = 2;
        private const byte COVER_TYPE_JPG = 1;
        private Stream coverStream;
        private const int CURR_VERSION = 1;
        private string day;
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
        private Encoding encoding = Encoding.Unicode;
        private string File_Name;
        private const byte FIXED_LINE_PER_PAGE_S60 = 50;
        private const byte FIXED_LINE_PER_PAGE_SP = 0x19;
        private string gender = string.Empty;
        private const int INT_LEN = 4;
        private const string KEY_CODE_TAB = "\t";
        private string month;
        private short pgkSeed = 0;
        private DateTime publishDate = DateTime.Now;
        private string publisher = string.Empty;
        private uint[] refContent;
        private const byte S60_FONT_SIZE_BIG = 0x10;
        private const byte S60_FONT_SIZE_SMALL = 12;
        private const byte SEREIS60_FONTS_COUNT = 2;
        private const int SHORT_LEN = 2;
        private const byte SP_FONT_SIZE_10 = 10;
        private const byte SP_FONT_SIZE_MAX = 0x10;
        private const byte SP_FONT_SIZE_MIN = 6;
        private const string SYMBIAN_RETURN = "\u2029";
        private const string SYMBIAN_SPACE = "銆€";
        private string text;
        private string title = string.Empty;
        private const byte UMD_DCTD_HEAD_LEN = 9;
        private const byte UMD_DCTS_HEAD_LEN = 5;
        private const int UMD_FREE_CID_MIN = 0x5f5e100;
        private const int UMD_FREE_PGK_SEED_MIN = 0x400;
        private const int UMD_LICENSEKEY_LEN = 0x10;
        private const int UMD_MAX_BOOKMARK_STR_LEN = 40;
        private const byte UMD_PLATFORM_ID_NONE = 0;
        private const byte UMD_PLATFORM_ID_S60 = 1;
        private const byte UMD_PLATFORM_ID_SP = 5;
        private const int UMD_SEGMENT_LENGTH = 0x8000;
        private string vendor = string.Empty;
        private const byte VER_PKG_LEN = 3;
        private static ArrayList widthData_S60 = new ArrayList();
        private static ArrayList widthData_SP = new ArrayList();
        private const string WINDOWS_RETURN = "\r\n";
        private string year;
        private const int ZIP_LEVEL = 9;
        private ArrayList zippedSeg = new ArrayList();

        public void AppendChapter()
        {
            CChapter chapter = new CChapter("第 " + (this.chapters.Count + 1) + "章", "");
            this.Chapters.Add(chapter);
        }

        public bool CanSave()
        {
            return ((this.File_Name != null) && (this.File_Name.Length > 0));
        }

        private byte CharWidth_S60(string @char, byte fontSize)
        {
            ushort num = @char[0];
            foreach (SWidthData data in widthData_S60)
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

        private byte CharWidth_SP(string @char, byte fontSize)
        {
            ushort num = @char[0];
            foreach (SWidthData data in widthData_SP)
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

        public void Close()
        {
            this.File_Name = "";
            this.chapters.Clear();
        }

        public void Extract(string path)
        {
            string str = path.EndsWith(@"\") ? path : (path + '\\');
            if (!Directory.Exists(str))
            {
                Directory.CreateDirectory(str);
            }
            this.SaveCover(str);
            for (int i = 0; i < this.chapOff.Length; i++)
            {
                StreamWriter writer = new StreamWriter(str + this.chapters[i].Title + ".txt", false, Encoding.Unicode);
                writer.Write(this.chapters[i].Content);
                writer.Close();
            }
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
            while (((uint) pagesOff[pagesOff.Count - 1]) < this.text.Length)
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
            while (((uint) pagesOff[pagesOff.Count - 1]) < this.text.Length)
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
            if (widthData_S60.Count == 0)
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
                            widthData_S60.Add(data);
                        }
                        reader.Close();
                        input.Close();
                    }
                }
            }
        }

        private void GetWidthData_SP()
        {
            if (widthData_SP.Count == 0)
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
                            widthData_SP.Add(data);
                        }
                        reader.Close();
                        input.Close();
                    }
                }
            }
        }

        private void Initialize()
        {
            if (this.title.Length == 0)
            {
                throw new ArgumentException("title");
            }
            if (this.author.Length == 0)
            {
                throw new ArgumentException("author");
            }
            if (this.chapters.Count == 0)
            {
                throw new ApplicationException("no chapter");
            }
            foreach (CChapter chapter in this.chapters)
            {
                if (!chapter.Content.EndsWith("\u2029"))
                {
                    chapter.Content = chapter.Content + "\u2029";
                }
            }
            this.chapOff = new int[this.chapters.Count];
        }

        public void Make(string path)
        {
            this.Make(path, false);
        }

        public void Make(string path, bool overwrite)
        {
            uint[] numArray;
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            this.Initialize();
            string str = path;
            this.Prepare();
            FileStream output = new FileStream(str, FileMode.OpenOrCreate, FileAccess.Write);
            BinaryWriter writer = new BinaryWriter(output);
            writer.Write((uint) 0xde9a9b89);
            this.WriteSection(1, 0, 3, writer);
            this.WriteSection(2, 0, (byte) (this.title.Length * 2), writer);
            this.WriteSection(3, 0, (byte) (this.author.Length * 2), writer);
            this.WriteSection(4, 0, (byte) (this.year.Length * 2), writer);
            this.WriteSection(5, 0, (byte) (this.month.Length * 2), writer);
            this.WriteSection(6, 0, (byte) (this.day.Length * 2), writer);
            if (this.gender.Length != 0)
            {
                this.WriteSection(7, 0, (byte) (this.gender.Length * 2), writer);
            }
            if (this.publisher.Length != 0)
            {
                this.WriteSection(8, 0, (byte) (this.publisher.Length * 2), writer);
            }
            if (this.vendor.Length != 0)
            {
                this.WriteSection(9, 0, (byte) (this.vendor.Length * 2), writer);
            }
            this.WriteSection(11, 0, 4, writer);
            Random random = new Random();
            this.additionalCheck = (uint) (0x3000 + random.Next(0xfff));
            this.WriteSection(0x83, 0, 4, writer);
            this.WriteAdditional(0x83, this.additionalCheck, (uint) (this.chapOff.Length * 4), writer);
            int num = 0;
            foreach (CChapter chapter in this.chapters)
            {
                num += (chapter.Title.Length * 2) + 1;
            }
            this.additionalCheck = (uint) (0x4000 + random.Next(0xfff));
            this.WriteSection(0x84, 1, 4, writer);
            this.WriteAdditional(0x84, this.additionalCheck, (uint) num, writer);
            int num2 = 0;
            int num3 = 0;
            if (this.zippedSeg.Count > 1)
            {
                num2 = random.Next(0, this.zippedSeg.Count - 1);
                num3 = random.Next(0, this.zippedSeg.Count - 1);
            }
            this.refContent = new uint[this.zippedSeg.Count];
            for (int i = 0; i < this.zippedSeg.Count; i++)
            {
                byte[] buffer = (byte[]) this.zippedSeg[i];
                this.refContent[i] = (uint) (random.Next(1, 0xfffffff) * -1);
                this.WriteAdditional(0x84, this.refContent[i], (uint) buffer.Length, writer);
                if (i == num2)
                {
                    this.WriteSection(0xf1, 0, 0x10, writer);
                }
                if (i == num3)
                {
                    this.WriteSection(10, 0, 4, writer);
                }
            }
            this.additionalCheck = (uint) (0x2000 + random.Next(0xfff));
            this.WriteSection(0x81, 1, 4, writer);
            this.WriteAdditional(0x81, this.additionalCheck, (uint) (this.refContent.Length * 4), writer);
            if (this.cover != null)
            {
                this.coverStream = new MemoryStream();
                this.cover.Save(this.coverStream, ImageFormat.Gif);
                this.additionalCheck = (uint) (0x1000 + random.Next(0xfff));
                this.WriteSection(130, 1, 5, writer);
                this.WriteAdditional(130, this.additionalCheck, (uint) this.coverStream.Length, writer);
                this.coverStream.Close();
                this.coverStream = null;
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
            this.WriteSection(12, 1, 4, writer);
            writer.Close();
        }

        public void Open(string strFileName)
        {
            this.Read(strFileName);
            this.File_Name = strFileName;
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
                    if (num < this.text.Length)
                    {
                        str2 = this.text.Substring((int) num, 1);
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
                        if ((num < this.text.Length) && (num > ((uint) pagesOff[pagesOff.Count - 1])))
                        {
                            pagesOff.Add(((uint) num2) + ((uint) pagesOff[pagesOff.Count - 1]));
                        }
                        if (num >= this.text.Length)
                        {
                            pagesOff.Add((uint) this.text.Length);
                        }
                    }
                }
            }
        }

        private void Prepare()
        {
            this.year = this.publishDate.Year.ToString();
            this.month = this.publishDate.Month.ToString();
            this.day = this.publishDate.Day.ToString();
            Random random = new Random();
            this.pgkSeed = (short) (random.Next(0x401, 0x7fff) % 0xffff);
            this.cid = random.Next(0x5f5e101, 0x3b9aca00);
            int byteIndex = 0;
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < this.chapters.Count; i++)
            {
                CChapter chapter = this.chapters[i];
                chapter.Content = chapter.Content.Replace("\r\n", "\u2029");
                builder.Append(chapter.Content);
                this.chapOff[i] = byteIndex;
                byteIndex += chapter.Content.Length * 2;
            }
            this.contentLength = byteIndex;
            this.text = builder.ToString();
            byte[] bytes = new byte[this.contentLength];
            byteIndex = 0;
            for (int j = 0; j < this.chapters.Count; j++)
            {
                CChapter chapter2 = this.chapters[j];
                this.encoding.GetBytes(chapter2.Content, 0, chapter2.Content.Length, bytes, byteIndex);
                byteIndex += chapter2.Content.Length * 2;
            }
            int num4 = 0;
            if ((bytes.Length % 0x8000) == 0)
            {
                num4 = bytes.Length / 0x8000;
            }
            else
            {
                num4 = (bytes.Length / 0x8000) + 1;
            }
            byteIndex = 0;
            byte[] output = new byte[0x8000];
            for (int k = 0; k < num4; k++)
            {
                Deflater deflater = new Deflater(Deflater.BEST_COMPRESSION, false);
                deflater.SetInput(bytes, byteIndex, Math.Min(0x8000, bytes.Length - byteIndex));
                deflater.Finish();
                deflater.Deflate(output);
                byte[] destinationArray = new byte[deflater.TotalOut];
                Array.Copy(output, 0, destinationArray, 0, destinationArray.Length);
                this.zippedSeg.Add(destinationArray);
                byteIndex += output.Length;
            }
        }

        private void Read(BinaryReader reader)
        {
            this.chapters.Clear();
            if (reader.ReadUInt32() != 0xde9a9b89)
            {
                throw new ApplicationException("Wrong header");
            }
            char ch = (char) reader.PeekChar();
            while (ch == '#')
            {
                reader.ReadChar();
                short id = reader.ReadInt16();
                byte b = reader.ReadByte();
                byte length = (byte) (reader.ReadByte() - 5);
                this.ReadSection(id, b, length, reader);
                ch = (char) reader.PeekChar();
                switch (id)
                {
                    case 10:
                    case 0xf1:
                        id = 0x84;
                        break;
                }
                while (ch == '$')
                {
                    reader.ReadChar();
                    uint check = reader.ReadUInt32();
                    uint num6 = reader.ReadUInt32() - 9;
                    this.ReadAdditional(id, check, num6, reader);
                    ch = (char) reader.PeekChar();
                }
            }
            reader.Close();
            int destinationIndex = 0;
            byte[] destinationArray = new byte[this.contentLength];
            byte[] buf = new byte[0x8000];
            foreach (byte[] buffer3 in this.zippedSeg)
            {
                Inflater inflater = new Inflater();
                inflater.SetInput(buffer3);
                inflater.Inflate(buf);
                if (destinationIndex < destinationArray.Length)
                {
                    Array.Copy(buf, 0, destinationArray, destinationIndex, Math.Min(destinationArray.Length - destinationIndex, inflater.TotalOut));
                    destinationIndex += Convert.ToInt32(inflater.TotalOut);
                }
            }
            for (int i = 0; i < this.chapOff.Length; i++)
            {
                this.chapters[i].Content = this.encoding.GetString(destinationArray, this.chapOff[i], ((i < (this.chapOff.Length - 1)) ? this.chapOff[i + 1] : destinationArray.Length) - this.chapOff[i]).Replace("\u2029", "\r\n");
            }
            this.zippedSeg.Clear();
            this.publishDate = new DateTime(int.Parse(this.year), int.Parse(this.month), int.Parse(this.day));
        }

        private void Read(string file)
        {
            BinaryReader reader = new BinaryReader(new FileStream(file, FileMode.Open));
            this.Read(reader);
        }

        protected virtual void ReadAdditional(short id, uint check, uint length, BinaryReader reader)
        {
            switch (id)
            {
                case 0x81:
                    reader.ReadBytes((int) length);
                    return;

                case 130:
                    this.cover = Image.FromStream(new MemoryStream(reader.ReadBytes((int) length)));
                    return;

                case 0x83:
                    this.chapOff = new int[length / 4];
                    for (int i = 0; i < this.chapOff.Length; i++)
                    {
                        this.chapOff[i] = reader.ReadInt32();
                    }
                    return;

                case 0x84:
                    if (this.additionalCheck == check)
                    {
                        int index = 0;
                        byte[] bytes = reader.ReadBytes((int) length);
                        while (index < bytes.Length)
                        {
                            byte count = bytes[index];
                            this.chapters.Add(new CChapter(this.encoding.GetString(bytes, ++index, count), string.Empty));
                            index += count;
                        }
                        return;
                    }
                    this.zippedSeg.Add(reader.ReadBytes((int) length));
                    return;
            }
            reader.ReadBytes((int) length);
        }

        protected virtual void ReadSection(short id, byte b, byte length, BinaryReader reader)
        {
            switch (id)
            {
                case 1:
                    reader.ReadByte();
                    this.pgkSeed = reader.ReadInt16();
                    return;

                case 2:
                    this.title = this.encoding.GetString(reader.ReadBytes(length));
                    return;

                case 3:
                    this.author = this.encoding.GetString(reader.ReadBytes(length));
                    return;

                case 4:
                    this.year = this.encoding.GetString(reader.ReadBytes(length));
                    return;

                case 5:
                    this.month = this.encoding.GetString(reader.ReadBytes(length));
                    return;

                case 6:
                    this.day = this.encoding.GetString(reader.ReadBytes(length));
                    return;

                case 7:
                    this.gender = this.encoding.GetString(reader.ReadBytes(length));
                    return;

                case 8:
                    this.publisher = this.encoding.GetString(reader.ReadBytes(length));
                    return;

                case 9:
                    this.vendor = this.encoding.GetString(reader.ReadBytes(length));
                    return;

                case 10:
                    this.cid = reader.ReadInt32();
                    return;

                case 11:
                    this.contentLength = reader.ReadInt32();
                    return;

                case 12:
                    reader.ReadUInt32();
                    return;

                case 0x81:
                case 0x83:
                case 0x84:
                    this.additionalCheck = reader.ReadUInt32();
                    return;

                case 130:
                    reader.ReadByte();
                    this.additionalCheck = reader.ReadUInt32();
                    return;
            }
            reader.ReadBytes(length);
        }

        public void Save()
        {
            this.Save(this.File_Name);
        }

        public void Save(string strFileName)
        {
            widthData_S60.Clear();
            widthData_SP.Clear();
            this.pgkSeed = 0;
            this.zippedSeg.Clear();
            this.chapOff = null;
            this.Make(strFileName, true);
            this.File_Name = strFileName;
        }

        public void SaveAs(string strFileName)
        {
            this.Save(strFileName);
        }

        protected virtual void SaveCover(string path)
        {
            Guid guid = this.cover.RawFormat.Guid;
            string str = ".tmp";
            if (guid == ImageFormat.Bmp.Guid)
            {
                str = ".bmp";
            }
            else if (guid == ImageFormat.Gif.Guid)
            {
                str = ".gif";
            }
            else if (guid == ImageFormat.Jpeg.Guid)
            {
                str = ".jpg";
            }
            this.cover.Save(path + "cover" + str);
        }

        protected virtual void WriteAdditional(short id, uint check, uint length, BinaryWriter writer)
        {
            writer.Write('$');
            writer.Write(check);
            writer.Write((uint) (length + 9));
            switch (id)
            {
                case 0x81:
                    foreach (uint num in this.refContent)
                    {
                        writer.Write(num);
                    }
                    return;

                case 130:
                {
                    this.coverStream.Seek(0L, SeekOrigin.Begin);
                    byte[] buffer = new byte[this.coverStream.Length];
                    this.coverStream.Read(buffer, 0, buffer.Length);
                    writer.Write(buffer);
                    this.coverStream.Close();
                    return;
                }
                case 0x83:
                    foreach (int num2 in this.chapOff)
                    {
                        writer.Write(num2);
                    }
                    return;

                case 0x84:
                    if (this.additionalCheck != check)
                    {
                        int index = Array.IndexOf(this.refContent, check);
                        if (index != -1)
                        {
                            writer.Write((byte[]) this.zippedSeg[index]);
                        }
                        return;
                    }
                    foreach (CChapter chapter in this.chapters)
                    {
                        writer.Write((byte) (chapter.Title.Length * 2));
                        writer.Write(this.encoding.GetBytes(chapter.Title));
                    }
                    return;
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

        protected virtual void WriteSection(short id, byte b, byte length, BinaryWriter writer)
        {
            writer.Write('#');
            writer.Write(id);
            writer.Write(b);
            writer.Write((byte) (length + 5));
            switch (id)
            {
                case 1:
                    writer.Write((byte) 1);
                    writer.Write(this.pgkSeed);
                    return;

                case 2:
                    writer.Write(this.encoding.GetBytes(this.title));
                    return;

                case 3:
                    writer.Write(this.encoding.GetBytes(this.author));
                    return;

                case 4:
                    writer.Write(this.encoding.GetBytes(this.year));
                    return;

                case 5:
                    writer.Write(this.encoding.GetBytes(this.month));
                    return;

                case 6:
                    writer.Write(this.encoding.GetBytes(this.day));
                    return;

                case 7:
                    writer.Write(this.encoding.GetBytes(this.gender));
                    return;

                case 8:
                    writer.Write(this.encoding.GetBytes(this.publisher));
                    return;

                case 9:
                    writer.Write(this.encoding.GetBytes(this.vendor));
                    return;

                case 10:
                    writer.Write(this.cid);
                    return;

                case 11:
                    writer.Write(this.contentLength);
                    return;

                case 12:
                    writer.Write((uint) (((uint) writer.BaseStream.Position) + 4));
                    return;

                case 0x81:
                case 0x83:
                case 0x84:
                    writer.Write(this.additionalCheck);
                    return;

                case 130:
                    writer.Write((byte) 1);
                    writer.Write(this.additionalCheck);
                    return;

                case 0xf1:
                    writer.Write(Encoding.ASCII.GetBytes("\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0"));
                    return;
            }
        }

        public string Author
        {
            get
            {
                return this.author;
            }
            set
            {
                this.author = value;
            }
        }

        public CChapterList Chapters
        {
            get
            {
                return this.chapters;
            }
        }

        public Image Cover
        {
            get
            {
                return this.cover;
            }
            set
            {
                this.cover = value;
            }
        }

        public string Gender
        {
            get
            {
                return this.gender;
            }
            set
            {
                this.gender = value;
            }
        }

        public DateTime PublishDate
        {
            get
            {
                return this.publishDate;
            }
            set
            {
                this.publishDate = value;
            }
        }

        public string Publisher
        {
            get
            {
                return this.publisher;
            }
            set
            {
                this.publisher = value;
            }
        }

        public string Title
        {
            get
            {
                return this.title;
            }
            set
            {
                this.title = value;
            }
        }

        public string Vendor
        {
            get
            {
                return this.vendor;
            }
            set
            {
                this.vendor = value;
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
