using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using DocGen.Utility;
using System.Configuration;
using DocGen.DAL;
using System.Drawing;
using System.Data;
namespace DocGen.Document.ImageSection
{
    public partial class Edit : SecurePage
    {
        public int DocumentSectionID
        {
            get
            {
                int _DocumentSectionID = 0;
                if (Request.QueryString["DocumentSectionID"] != null)
                {
                    Int32.TryParse(Convert.ToString(Request.QueryString["DocumentSectionID"]), out _DocumentSectionID);
                }
                return _DocumentSectionID;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["dtShowWhen"] = null;
                lblAllowedExt.Text = String.Format(lblAllowedExt.Text, ConfigurationManager.AppSettings["AllowedUploadImageExt"].ToString().Replace(",", ",&nbsp;"));
                lblMaxFileSize.Text = String.Format(lblMaxFileSize.Text, ConfigurationManager.AppSettings["MaxUploadFileSizeKB"]);
                if (DocumentSectionID <= 0)
                {
                    hlShowWhen.NavigateUrl = "~/Pages/Record/ShowHide.aspx?Context=dashboard";
                    if (Request.QueryString["PrevID"].ToString() != "-1")
                    {
                        hfRemoveSection.Value = "1";
                    }
                }
                else
                {
                    hlShowWhen.NavigateUrl = "~/Pages/Record/ShowHide.aspx?Context=dashboard&DocumentSectionID=" + DocumentSectionID.ToString();

                    string strShowWhenID = Common.GetValueFromSQL("SELECT TOP 1 ShowWhenID FROM ShowWhen WHERE DocumentSectionID=" + DocumentSectionID.ToString());

                    if (strShowWhenID!="")
                    {
                        chkShowWhen.Checked = true;
                    }

                    using (DAL.DocGenDataContext ctx = new DAL.DocGenDataContext())
                    {
                        DAL.DocumentSection section = ctx.DocumentSections.SingleOrDefault<DAL.DocumentSection>(s => s.DocumentSectionID == DocumentSectionID);
                        if (section != null)
                        {
                            //CheckPermission(section.DocumentID);
                            //txtTitle.Text = section.SectionName;
                            //txtDescription.Text = section.Content;
                            ImageSectionStyle imageStyle = JSONField.GetTypedObject<ImageSectionStyle>(section.Details);
                            if (imageStyle != null)
                            {
                                switch (imageStyle.Position)
                                {
                                    case "center":
                                    case "right":
                                        ddlPosition.SelectedValue = imageStyle.Position;
                                        break;
                                }
                                if (imageStyle.Width > 0)
                                {
                                    txtSize.Text = imageStyle.Width.ToString();
                                }
                            }
                            imgImage.ImageUrl = String.Format("../../Uploaded/ImageSection/{0}.png", section.DocumentSectionID);
                            if ( string.IsNullOrEmpty(imageStyle.OpenLink)==false)
                            {
                                chkOpenLink.Checked = true;
                                txtOpenLink.Text = imageStyle.OpenLink;
                            }
                            //CancelButton.CommandArgument = section.DocumentID.ToString();
                        }
                        else
                        {
                            //Response.Redirect("../Summary.aspx", true);
                        }
                    }



                }
            }

            if (fuImage.HasFile)
            {
                PopulateImageControl();
            }



        }
        public int AccountID
        {
            get
            {
                int retVal = 0;
                if (Session["AccountID"] != null)
                    retVal = Convert.ToInt32(Session["AccountID"]);
                return retVal;
            }
        }

        protected void CheckPermission(int DocumentID)
        {
            using (DAL.DocGenDataContext ctx = new DAL.DocGenDataContext())
            {
                DAL.Document doc = ctx.Documents.SingleOrDefault<DAL.Document>(d => d.DocumentID == DocumentID && d.AccountID == this.AccountID);
                if (doc == null)
                {
                    Response.Redirect("~/Empty.aspx", false);
                }
            }
        }

        protected void SaveButton_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
              
                ErrorMessage.Text = "";
                ImageSectionStyle imageStyle = new ImageSectionStyle();
                imageStyle.Position = ddlPosition.SelectedValue;
                imageStyle.Width = 0;

                if (chkOpenLink.Checked && txtOpenLink.Text.Trim() != "")
                {
                    imageStyle.OpenLink = txtOpenLink.Text.Trim();
                }
                else
                {
                    imageStyle.OpenLink = "";
                }

                if (txtSize.Text.Trim() != "")
                {
                    int imgW = 0;
                    Int32.TryParse(txtSize.Text, out imgW);
                    if (imgW > 0)
                    {
                        imageStyle.Width = imgW;
                    }
                    else
                    {
                        ErrorMessage.Text = "Image size is invalid";
                        return;
                    }
                }
               
                try
                {
                    int ID = 0;

                    if (Request.QueryString["PrevID"].ToString() != "-1")
                    {
                        //Guid newGuid = new Guid();
                        //newGuid = Guid.NewGuid();

                        //if (fuImage.HasFile)
                        //{                           

                        //    string FilePath = Server.MapPath(String.Format("~/Uploaded/ImageSection/{0}.png", newGuid.ToString()));
                        //    string Message = ImageUtil.SaveAsPNG(FilePath, fuImage.FileBytes);
                        //    if (Message != "")
                        //    {
                        //        ErrorMessage.Text = Message;
                        //        return;
                        //    }
                        //    File.Delete(FilePath);
                        //}




                        int DocumentID = 0;
                        Int32.TryParse(Convert.ToString(Request.QueryString["DocumentID"]), out DocumentID);

                        int iPosition = 1;

                        //if (Request.QueryString["Position"] != null)
                        //{
                        //    iPosition = int.Parse(Convert.ToString(Request.QueryString["Position"])) + 1;
                        //}

                        int NewSectionID = 0;

                        using (DAL.DocGenDataContext ctx = new DAL.DocGenDataContext())
                        {

                            if (Request.QueryString["PrevID"].ToString() != "0")
                            {

                                DAL.DocumentSection PreSection = ctx.DocumentSections.SingleOrDefault<DAL.DocumentSection>(s => s.DocumentSectionID == int.Parse(Request.QueryString["PrevID"].ToString()));

                                iPosition = PreSection.Position + 1;
                            }
                            else
                            {
                                iPosition = 1;
                            }


                            DAL.DocumentSection newSection = new DAL.DocumentSection();

                            //if (Request.QueryString["Position"] != null)
                            //{
                                ctx.ExecuteCommand("UPDATE DocumentSection SET Position=Position + 1 WHERE DocumentID={0}  AND Position>{1}", DocumentID.ToString(), (iPosition - 1).ToString());
                            //}

                            newSection.DocumentID = DocumentID;
                            //newSection.SectionName = txtTitle.Text;
                            newSection.DocumentSectionTypeID = 3; //Image

                            newSection.Details = imageStyle.GetJSONString();
                            newSection.Position = iPosition;
                            newSection.DateAdded = DateTime.Now;
                            newSection.DateUpdated = DateTime.Now;

                            
                            ctx.DocumentSections.InsertOnSubmit(newSection);

                            ctx.SubmitChanges();
                            NewSectionID = newSection.DocumentSectionID;
                            hfRemoveSection.Value = "0";
                            ID = NewSectionID;

                            if (chkShowWhen.Checked && Session["dtShowWhen"] != null)
                            {
                                //insert new show when
                                DataTable dtShowWhen = (DataTable)Session["dtShowWhen"];
                                int iDO = 1;
                                foreach (DataRow drSW in dtShowWhen.Rows)
                                {
                                    if (iDO == 1)
                                    {
                                        if (drSW["HideColumnID"].ToString() == "" || drSW["HideColumnValue"].ToString() == "")
                                        {
                                            continue;
                                        }
                                        ShowWhen theShowWhen1 = new ShowWhen();
                                        theShowWhen1.DocumentSectionID = NewSectionID;
                                        theShowWhen1.Context = "dashboard";
                                        theShowWhen1.HideColumnID = int.Parse(drSW["HideColumnID"].ToString());
                                        theShowWhen1.HideColumnValue = drSW["HideColumnValue"].ToString();
                                        theShowWhen1.HideOperator = drSW["HideOperator"].ToString();
                                        theShowWhen1.DisplayOrder = 1;
                                        theShowWhen1.JoinOperator = "";
                                        theShowWhen1.ShowWhenID = RecordManager.dbg_ShowWhen_Insert(theShowWhen1);

                                        iDO = iDO + 1;
                                        continue;
                                    }
                                    else
                                    {
                                        if (drSW["HideColumnID"].ToString() == "" || drSW["HideColumnValue"].ToString() == "" || drSW["JoinOperator"].ToString() == "")
                                        {
                                            continue;
                                        }


                                        ShowWhen theShowWhenJoin = new ShowWhen();
                                        theShowWhenJoin.DocumentSectionID = NewSectionID;
                                        theShowWhenJoin.Context = "dashboard";
                                        theShowWhenJoin.HideColumnID = null;
                                        theShowWhenJoin.HideColumnValue = "";
                                        theShowWhenJoin.HideOperator = "";
                                        theShowWhenJoin.DisplayOrder = iDO;
                                        theShowWhenJoin.JoinOperator = drSW["JoinOperator"].ToString();

                                        theShowWhenJoin.ShowWhenID = RecordManager.dbg_ShowWhen_Insert(theShowWhenJoin);
                                        iDO = iDO + 1;

                                        ShowWhen theShowWhen = new ShowWhen();
                                        theShowWhen.DocumentSectionID = NewSectionID;
                                        theShowWhen.Context = "dashboard";
                                        theShowWhen.HideColumnID = int.Parse(drSW["HideColumnID"].ToString());
                                        theShowWhen.HideColumnValue = drSW["HideColumnValue"].ToString();
                                        theShowWhen.HideOperator = drSW["HideOperator"].ToString();
                                        theShowWhen.DisplayOrder = iDO;
                                        theShowWhen.JoinOperator = "";

                                        theShowWhen.ShowWhenID = RecordManager.dbg_ShowWhen_Insert(theShowWhen);
                                        iDO = iDO + 1;

                                    }
                                }

                            }


                        }



                        //if (fuImage.HasFile)
                        //{                            
                        //    string FilePath = Server.MapPath(String.Format("~/Uploaded/ImageSection/{0}.png", NewSectionID));
                        //    string Message = ImageUtil.SaveAsPNG(FilePath, fuImage.FileBytes);
                        //    if (Message != "")
                        //    {
                        //        ErrorMessage.Text = Message;
                        //        return;
                        //    }
                        //}

                        if (ViewState["imagepath"] != null)
                        {
                            File.Copy(Server.MapPath(ViewState["imagepath"].ToString()), Server.MapPath( String.Format("~/Uploaded/ImageSection/{0}.png", NewSectionID)));
                        }


                    }
                    else
                    {
                        ID = DocumentSectionID;



                        //if (fuImage.HasFile)
                        //{
                        //    string FilePath = Server.MapPath(String.Format("~/Uploaded/ImageSection/{0}.png", ID));
                        //    string Message = ImageUtil.SaveAsPNG(FilePath, fuImage.FileBytes);
                        //    if (Message != "")
                        //    {
                        //        ErrorMessage.Text = Message;
                        //        return;
                        //    }
                        //}

                        if (ViewState["imagepath"] != null)
                        {
                            File.Copy( Server.MapPath( ViewState["imagepath"].ToString()), Server.MapPath(String.Format("~/Uploaded/ImageSection/{0}.png", ID)),true);
                        }


                        using (DAL.DocGenDataContext ctx = new DAL.DocGenDataContext())
                        {
                            DAL.DocumentSection section = ctx.DocumentSections.SingleOrDefault<DAL.DocumentSection>(s => s.DocumentSectionID == ID);
                            if (section != null)
                            {
                                //section.SectionName = txtTitle.Text;
                                //section.Content = txtDescription.Text;
                                section.Details = imageStyle.GetJSONString();
                            }
                            ctx.SubmitChanges();
                            hfRemoveSection.Value = "0";
                        }


                    }

                    ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "CloseScript", "window.parent.SectionUpdated(" + ID.ToString() + ");", true);
                }
                catch (Exception ex)
                {
                    ErrorMessage.Text = ex.Message;
                }

            }
        }

        //protected void CancelButton_Click(object sender, EventArgs e)
        //{
        //    Response.Redirect("../Edit.aspx?DocumentID=" + CancelButton.CommandArgument);
        //}

        protected void ImageValidator_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (fuImage.HasFile)
            {
                string fileType = fuImage.FileName;
                fileType = fileType.Substring(fileType.LastIndexOf(".") + 1);
                string[] allowedType = ConfigurationManager.AppSettings["AllowedUploadImageExt"].Split(',');
                if (allowedType.Contains(fileType.ToLower()))
                {
                    if (fuImage.PostedFile.ContentLength <= Convert.ToInt32(ConfigurationManager.AppSettings["MaxUploadFileSizeKB"]) * 1024)
                        args.IsValid = true;
                    else
                        args.IsValid = false;
                }
                else
                {
                    args.IsValid = false;
                }
            }
            else
            {
                args.IsValid = true;
            }
        }


        protected void PopulateImageControl()
        {
            try
            {
                BinaryReader br = new BinaryReader(fuImage.PostedFile.InputStream);
                byte[] data = null;
                data = br.ReadBytes((int)fuImage.PostedFile.ContentLength);

                System.Drawing.Image theImage = System.Drawing.Image.FromStream(new MemoryStream(data));


                Guid newGuid = new Guid();
                newGuid = Guid.NewGuid();

                string strFileNameTemp = newGuid.ToString();

                string strFilyType = fuImage.FileName.Substring(fuImage.FileName.LastIndexOf("."));

                strFileNameTemp = strFileNameTemp + strFilyType;

                txtSize.Text = theImage.Width.ToString();

                //ViewState["data"] = data;

                //if (theImage.Width > 400 || theImage.Height > 400)
                //{
                //    data = Common.ResizeImageFile(data, 400);
                //}
                //else
                //{
                //    //
                //}

                //ViewState["data"] = data;

                theImage = System.Drawing.Image.FromStream(new MemoryStream(data));
                Bitmap bmp = new Bitmap(theImage);
                //bmp.Save(Server.MapPath("Images/" + strFileNameTemp));
                string strFilteURL = Server.MapPath("~/Pages/Docgen/Images/" + strFileNameTemp);
                string Message = ImageUtil.SaveAsPNG(strFilteURL, fuImage.FileBytes);
                if (Message != "")
                {
                    ErrorMessage.Text = Message;
                    return;
                }

                imgImage.ImageUrl = "~/Pages/Docgen/Images/" + strFileNameTemp;

                ViewState["imagepath"] = "~/Pages/Docgen/Images/" + strFileNameTemp;


                //pnlPhoto.Height = theImage.Height;
                //lets delete old files.
                try
                {
                    DeleteOldFiles("Images");
                }
                catch
                {

                }
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Problem", "alert('Invalid File!');", true);

            }
        }

        protected void DeleteOldFiles(string strFolder)
        {
            DirectoryInfo di = new DirectoryInfo(Server.MapPath(strFolder));
            FileInfo[] rgFiles = di.GetFiles();
            foreach (FileInfo fi in rgFiles)
            {
                if (fi.CreationTime.AddHours(1) < DateTime.Now)
                {
                    fi.Delete();
                }
            }
        }




    }
}