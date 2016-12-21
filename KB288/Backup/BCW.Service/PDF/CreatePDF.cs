using System;
//using iTextSharp;
//using iTextSharp.text;
//using iTextSharp.text.pdf;
using System.IO;
using System.Text;
using System.Data;
using BCW.Common;

namespace BCW.Service
{
    /// <summary>
    /// 将DataTable转化为PDF文件的方法
    /// </summary>
    public class CreatePDF
    {
        public CreatePDF()
        {
        }

        public void CreateMyPDF(string filepath, string headertxt, string footertxt, string Title, string Author, string Content)
        {
            ////创建文档对象
            //Document document = new Document();
            //// 创建文档写入实例
            //PdfWriter writer = PdfWriter.getInstance(document, new FileStream(filepath, FileMode.Create));

            ////打开文档内容对象
            //document.Open();

            ////创建PDF文档中的字体
            ////防宋体
            //BaseFont baseFont = BaseFont.createFont(
            //    @"C:\WINDOWS\Fonts\SIMFANG.TTF",
            //    BaseFont.IDENTITY_H,
            //    BaseFont.NOT_EMBEDDED);

            ////根据字体路径和字体大小属性创建字体
            //Font font = new Font(baseFont, 14, Font.NORMAL, new Color(255, 255, 255));
            //Font font2 = new Font(baseFont, 12, Font.NORMAL, new Color(0, 0, 0));
            //Font font3 = new Font(baseFont, 8, Font.ITALIC, new Color(0x66, 0x66, 0x66));
            //// 添加页眉
            //HeaderFooter header = new HeaderFooter(new Phrase(headertxt, font2), false);
            //document.Header = header;
            //// 添加页脚
            //HeaderFooter footer = new HeaderFooter(new Phrase(footertxt, font3), true);
            //footer.Border = Rectangle.NO_BORDER;
            //document.Footer = footer;

            ////添加块
            //Chunk chunk = new Chunk("内容纲要", font);
            //Phrase ph = new Phrase();
            //ph.Add(chunk);

            ////加表格
            //iTextSharp.text.Table atable = new iTextSharp.text.Table(1, 1);
            //atable.WidthPercentage = 100;
            //atable.Spacing = 0;           //表格字距;行距
            //atable.Padding = 4;           //表格边距
            //atable.BorderWidth = 1;
            //atable.addCell(ph);
            //atable.BackgroundColor = new Color(0x77, 0x77, 0x77);
            //atable.BorderColor = new Color(255, 255, 255);
            //document.Add(atable);
            //document.Add(new Paragraph("  ", font2));
            //document.Add(new Paragraph("标题:" + Title + "", font2));
            //document.Add(new Paragraph("作者:" + Author + "", font2));
            //document.Add(new Paragraph("创建时间:" + DateTime.Now + "", font2));
            //// 重置页面数量
            //document.resetPageCount();

            ////根据分页生成目录
            //int pageIndex = 0;
            //int recordCount = 0;
            //int pageTotal = BasePage.CalcPageCount(Content, Content.Length, 800, ref pageIndex);
            //for (int i = 1; i <= pageTotal; i++)
            //{
            //    //新添加一个页
            //    document.newPage();
            //    //添加块
            //    Chunk chunk1 = new Chunk("第" + i + "页", font);
            //    Phrase ph1 = new Phrase();
            //    ph1.Add(chunk1);

            //    //加表格
            //    atable = new iTextSharp.text.Table(1, 1);
            //    atable.WidthPercentage = 100;
            //    atable.Spacing = 0;           //表格字距;行距
            //    atable.Padding = 4;           //表格边距
            //    atable.BorderWidth = 1;
            //    atable.addCell(ph1);
            //    atable.BackgroundColor = new Color(0x77, 0x77, 0x77);
            //    atable.BorderColor = new Color(255, 255, 255);
            //    document.Add(atable);
            //    document.Add(new Paragraph(BasePage.MultiContent(Content, i, 800, 0, out recordCount), font2));
            //}
            //// 重置页面数量
            //document.resetPageCount();
            ////关闭文档对象
            //document.Close();
        }

    }
}