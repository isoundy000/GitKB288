namespace BCW.Service
{
    using System;

    public class UMDFactory
    {
        private UMDFactory()
        {
        }

        public static CUMDBook CreateNewUMDBook()
        {
            return new CUMDBook();
        }

        public static CUMDBook ReadUMDBook(string filepath)
        {
            UMDBookReader reader = new UMDBookReader();
            return reader.Read(filepath);
        }

        public static string WriteUMDBook(CUMDBook book)
        {
            UMDBookWriter writer = new UMDBookWriter(book);
            return writer.Write();
        }
    }
}
