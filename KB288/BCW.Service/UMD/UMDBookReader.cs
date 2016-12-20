namespace BCW.Service
{
    using ICSharpCode.SharpZipLib.Zip.Compression;
    using System;
    using System.Collections;
    using System.Drawing;
    using System.IO;

    public class UMDBookReader
    {
        private uint _AdditionalCheckNumber;
        private CUMDBook _Book = null;
        private int[] _ChaptersOff = null;
        private string _Publish_Day = string.Empty;
        private string _Publish_Month = string.Empty;
        private string _Publish_Year = string.Empty;
        private int _TotalContentLen = 0;
        private ArrayList _TotalImageList = new ArrayList();
        private ArrayList _ZippedContentList = new ArrayList();

        private CChapter GetChapter(int index)
        {
            if (index != this._Book.Chapters.Count)
            {
                throw new Exception("堆栈溢出！");
            }
            this._Book.Chapters.Add(new CChapter());
            return this._Book.Chapters[index];
        }

        private void ParseChapterImages()
        {
            int count = 0;
            for (int i = 0; i < this._Book.Chapters.Count; i++)
            {
                if (i < (this._Book.Chapters.Count - 1))
                {
                    count = this._ChaptersOff[i + 1];
                }
                else
                {
                    count = this._TotalImageList.Count;
                }
                int num1 = this._ChaptersOff[i];
                for (int j = this._ChaptersOff[i]; j < count; j++)
                {
                    this._Book.Chapters[i].AppendImage((Image) this._TotalImageList[j]);
                }
            }
            if (count < this._TotalImageList.Count)
            {
                CChapter chapter = new CChapter("未知", string.Empty);
                for (int k = count; k < this._TotalImageList.Count; k++)
                {
                    chapter.AppendImage((Image) this._TotalImageList[k]);
                }
                this._Book.AppendChapter(chapter);
            }
            this._TotalImageList.Clear();
        }

        private void ParseChapterTxtContents()
        {
            int destinationIndex = 0;
            byte[] destinationArray = new byte[this._TotalContentLen];
            byte[] buf = new byte[0x8000];
            foreach (byte[] buffer3 in this._ZippedContentList)
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
            for (int i = 0; i < this._ChaptersOff.Length; i++)
            {
                int index = this._ChaptersOff[i];
                int count = 0;
                if (i < (this._ChaptersOff.Length - 1))
                {
                    count = this._ChaptersOff[i + 1] - index;
                }
                else
                {
                    count = destinationArray.Length - index;
                }
                this._Book.Chapters[i].Content = this._Book.BookEncoding.GetString(destinationArray, index, count).Replace("\u2029", "\r\n");
            }
            this._ZippedContentList.Clear();
        }

        private void Read(BinaryReader reader)
        {
            if (reader.ReadUInt32() != 0xde9a9b89)
            {
                throw new ApplicationException("Wrong header");
            }
            short num2 = -1;
            char ch = (char) reader.PeekChar();
            while (ch == '#')
            {
                reader.ReadChar();
                short segType = reader.ReadInt16();
                byte segFlag = reader.ReadByte();
                byte length = (byte) (reader.ReadByte() - 5);
                this.ReadSection(segType, segFlag, length, reader);
                switch (segType)
                {
                    case 0xf1:
                    case 10:
                        segType = num2;
                        break;
                }
                num2 = segType;
                ch = (char) reader.PeekChar();
                while (ch == '$')
                {
                    reader.ReadChar();
                    uint additionalCheckNumber = reader.ReadUInt32();
                    uint num7 = reader.ReadUInt32() - 9;
                    this.ReadAdditionalSection(segType, additionalCheckNumber, num7, reader);
                    ch = (char) reader.PeekChar();
                }
                Console.WriteLine("BEGIN");
                Console.WriteLine((int) segType);
                Console.WriteLine(ch);
                Console.WriteLine("END");
            }
        }

        public CUMDBook Read(string filepath)
        {
            if (!File.Exists(filepath))
            {
                throw new Exception("找不到" + filepath);
            }
            this._Book = new CUMDBook();
            FileStream input = new FileStream(filepath, FileMode.Open, FileAccess.Read);
            if (input.CanRead)
            {
                BinaryReader reader = new BinaryReader(input);
                try
                {
                    this.Read(reader);
                    input.Close();
                    input = null;
                    try
                    {
                        this._Book.PublishDate = new DateTime(int.Parse(this._Publish_Year), int.Parse(this._Publish_Month), int.Parse(this._Publish_Day));
                    }
                    catch (Exception)
                    {
                        this._Book.PublishDate = DateTime.Now;
                    }
                    if (this._Book.BookType[1] == 1)
                    {
                        this.ParseChapterTxtContents();
                    }
                    else if (this._Book.BookType[1] == 2)
                    {
                        this.ParseChapterImages();
                    }
                    this._Book.BookPath = filepath;
                    return this._Book;
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception.Message);
                    Console.WriteLine(exception.StackTrace);
                    this._Book = null;
                    throw exception;
                }
                finally
                {
                    reader.Close();
                    reader = null;
                }
            }
            throw new Exception("文件正在被使用，无法读取！");
        }

        protected virtual void ReadAdditionalSection(short segType, uint additionalCheckNumber, uint length, BinaryReader reader)
        {
            switch (segType)
            {
                case 14:
                {
                    Image image = Image.FromStream(new MemoryStream(reader.ReadBytes((int) length)));
                    this._TotalImageList.Add(image);
                    return;
                }
                case 15:
                {
                    Image image2 = Image.FromStream(new MemoryStream(reader.ReadBytes((int) length)));
                    this._TotalImageList.Add(image2);
                    return;
                }
                case 0x81:
                    reader.ReadBytes((int) length);
                    return;

                case 130:
                    this._Book.Cover = Image.FromStream(new MemoryStream(reader.ReadBytes((int) length)));
                    return;

                case 0x83:
                    this._ChaptersOff = null;
                    this._ChaptersOff = new int[length / 4];
                    for (int i = 0; i < this._ChaptersOff.Length; i++)
                    {
                        this._ChaptersOff[i] = reader.ReadInt32();
                    }
                    return;

                case 0x84:
                    if (this._AdditionalCheckNumber == additionalCheckNumber)
                    {
                        int index = 0;
                        byte[] bytes = reader.ReadBytes((int) length);
                        while (index < bytes.Length)
                        {
                            byte count = bytes[index];
                            index++;
                            this._Book.Chapters.Add(new CChapter(this._Book.BookEncoding.GetString(bytes, index, count), string.Empty));
                            index += count;
                        }
                        return;
                    }
                    this._ZippedContentList.Add(reader.ReadBytes((int) length));
                    return;
            }
            Console.WriteLine("未知内容");
            Console.WriteLine("Seg Type = " + segType);
            Console.WriteLine("Seg Len = " + length);
            Console.WriteLine("content = " + reader.ReadBytes((int) length));
        }

        protected void ReadSection(short segType, byte segFlag, byte length, BinaryReader reader)
        {
            switch (segType)
            {
                case 1:
                    this._Book.BookType[0] = reader.ReadByte();
                    this._Book.BookType[1] = this._Book.BookType[0];
                    reader.ReadInt16();
                    return;

                case 2:
                    this._Book.BookTitle = this._Book.BookEncoding.GetString(reader.ReadBytes(length));
                    return;

                case 3:
                    this._Book.Author = this._Book.BookEncoding.GetString(reader.ReadBytes(length));
                    return;

                case 4:
                    this._Publish_Year = this._Book.BookEncoding.GetString(reader.ReadBytes(length));
                    return;

                case 5:
                    this._Publish_Month = this._Book.BookEncoding.GetString(reader.ReadBytes(length));
                    return;

                case 6:
                    this._Publish_Day = this._Book.BookEncoding.GetString(reader.ReadBytes(length));
                    return;

                case 7:
                    this._Book.BookKind = this._Book.BookEncoding.GetString(reader.ReadBytes(length));
                    return;

                case 8:
                    this._Book.Publisher = this._Book.BookEncoding.GetString(reader.ReadBytes(length));
                    return;

                case 9:
                    this._Book.Vendor = this._Book.BookEncoding.GetString(reader.ReadBytes(length));
                    return;

                case 10:
                    reader.ReadInt32();
                    return;

                case 11:
                    this._TotalContentLen = reader.ReadInt32();
                    return;

                case 12:
                    reader.ReadUInt32();
                    return;

                case 13:
                    Console.WriteLine("Seq type = " + 13);
                    Console.WriteLine(reader.ReadUInt32());
                    return;

                case 14:
                    reader.ReadByte();
                    return;

                case 15:
                    reader.ReadBytes(length);
                    this._Book.BookType[0] = 3;
                    return;

                case 0x81:
                case 0x83:
                case 0x84:
                    this._AdditionalCheckNumber = reader.ReadUInt32();
                    return;

                case 130:
                    reader.ReadByte();
                    this._AdditionalCheckNumber = reader.ReadUInt32();
                    return;
            }
            byte[] buffer = reader.ReadBytes(length);
            Console.WriteLine("未知编码");
            Console.WriteLine("Seg Type = " + segType);
            Console.WriteLine("Seg Flag = " + segFlag);
            Console.WriteLine("Seg Len = " + length);
            Console.WriteLine("Seg content = " + buffer.ToString());
        }
    }
}
