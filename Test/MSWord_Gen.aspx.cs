using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;


public partial class Test_MSWord_Gen : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            string error = String.Empty;
            Generate(Server.MapPath("1.docx"), out error);
        }
        catch (Exception ex)
        {
            ErrorLog theErrorLog = new ErrorLog(null, "MSWord Gen", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
            SystemData.ErrorLog_Insert(theErrorLog);
        }
    }

    public void Generate(string documentFileName, out string error)
    {
        error = String.Empty;

        using (WordprocessingDocument wordprocessingDocument = WordprocessingDocument.Open(documentFileName, true))
        {
            string path = System.IO.Path.GetDirectoryName(documentFileName);
            MainDocumentPart mainPart = wordprocessingDocument.MainDocumentPart;
            DocumentFormat.OpenXml.Wordprocessing.Document document = mainPart.Document;
            IEnumerable<Text> texts = document.Body.Descendants<Text>();
            string strPreValue = "";
            foreach (Text text in texts)
            {
                //Console.WriteLine(text.Text);
                if (text.Text == "[Date of incident]")
                    text.Text = DateTime.Today.ToString("d") + "ok22";

                if (text.Text == "[Details of incident]")
                    text.Text = DateTime.Today.ToString("d") + " test22";


                if (strPreValue == "[")
                {
                    if ("[" + text.Text + "]" == "[Details of incident]")
                        text.Text = DateTime.Today.ToString("d") + " test";
                }

                strPreValue = text.Text;

                if (text.Text == "[")
                    text.Text = "";

                if (text.Text == "]")
                    text.Text = "";

            }
        }
        //Console.ReadLine();
    }

}