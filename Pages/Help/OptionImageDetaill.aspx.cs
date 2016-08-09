using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using System.Drawing;
using DocGen.DAL;
public partial class Pages_Help_OptionImageDetaill : SecurePage
{

    string _strActionMode = "view";
    //int? _iOptionImageID;
    string _qsMode = "";
    string _qsOptionImageID = "-1";
    //int _iColumnID = -1;
    User _ObjUser;
    string _strFilesLocation = "";
    string _strFilesPhisicalPath = "";
    protected void Page_Init(object sender, EventArgs e)
    {
        _strFilesLocation = Session["FilesLocation"].ToString();
        _strFilesPhisicalPath = Session["FilesPhisicalPath"].ToString();

    }
    protected void Page_Load(object sender, EventArgs e)
    {
                
        _ObjUser = (User)Session["User"];
        

        //_iColumnID = int.Parse(Request.QueryString["ColumnID"].ToString());
        if (!IsPostBack)
        {
            lblAllowedExt.Text = String.Format(lblAllowedExt.Text, ConfigurationManager.AppSettings["AllowedUploadImageExt"].ToString().Replace(",", ",&nbsp;"));

        }
        if (Request.QueryString["mode"] == null)
        {
            Server.Transfer("~/Default.aspx");
        }
        else
        {
            _qsMode = Cryptography.Decrypt(Request.QueryString["mode"]);

            if (_qsMode == "add" ||
                _qsMode == "view" ||
                _qsMode == "edit")
            {
                _strActionMode = _qsMode;

                if (Request.QueryString["OptionImageID"] != null)
                {
                    _qsOptionImageID = Cryptography.Decrypt(Request.QueryString["OptionImageID"]);
                   
                }
                else
                {
                    //if (Session["OptionImage"] != null)
                    //{
                    //    DataTable dtTemp=(DataTable)Session["OptionImage"];
                    //    if (dtTemp.Rows.Count > 0)
                    //    {
                    //        _qsOptionImageID = (int.Parse(dtTemp.Rows[dtTemp.Rows.Count - 1]["OptionImageID"].ToString()) - 1).ToString();
                    //    }
                    //}
                }

                
            }
            else
            {
                Server.Transfer("~/Default.aspx");
            }

        }

        string strTitle = "Option Image Detail";        
        
        // checking permission

        switch (_strActionMode.ToLower())
        {
            case "add":
                strTitle = "Add Option Image";
               

                break;

            case "view":

                strTitle = "View Option Image";


                PopulateTheRecord();

                
                divSave.Visible = false;

                break;

            case "edit":

                strTitle = "Edit Option Image";
                if (!IsPostBack)
                {
                    PopulateTheRecord();
                }
                break;


            default:
                //?

                break;
        }


        Title = strTitle;
        lblTitle.Text = strTitle;


        if (fuImage.HasFile)
        {
            PopulateImageControl();
        }

    }


    protected void ImageValidator_ServerValidate(object source, ServerValidateEventArgs args)
    {
        if (fuImage.HasFile)
        {
            string fileType = fuImage.FileName;
            fileType = fileType.Substring(fileType.LastIndexOf(".") + 1);
            string[] allowedType = ConfigurationManager.AppSettings["AllowedUploadImageExt"].Split(',');
            if (allowedType.Contains(fileType.ToLower()))
            {
                args.IsValid = true;
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


            string strFolder = "\\UserFiles\\AppFiles";
            string strFileName = fuImage.FileName;
            string strUniqueName = Guid.NewGuid().ToString() + "_" + strFileName;
            string strPath = _strFilesPhisicalPath + strFolder + "\\" + strUniqueName;
            fuImage.SaveAs(strPath);

            imgImage.ImageUrl = _strFilesLocation + "/UserFiles/AppFiles/" + strUniqueName;

            OptionImage newOptionImage = new OptionImage();
            newOptionImage.OptionImageID = Guid.NewGuid().ToString();
            newOptionImage.Value = txtValue.Text;
            newOptionImage.FileName = strFileName;
            newOptionImage.UniqueFileName = strUniqueName;
            if (ViewState["theOptionImage"]!=null)
            {
                OptionImage theOptionImage = (OptionImage)ViewState["theOptionImage"];
                theOptionImage.Value = newOptionImage.Value;
                theOptionImage.FileName = newOptionImage.FileName;
                theOptionImage.UniqueFileName = newOptionImage.UniqueFileName;
                ViewState["theOptionImage"] = theOptionImage;
            }
            else
            {
                ViewState["newOptionImage"] = newOptionImage;
            }
            

        }
        catch
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Problem", "alert('Invalid File!');", true);

        }
    }


    protected void PopulateTheRecord()
    {
        try
        {

            DataTable dtOptionImage = (DataTable)Session["OptionImage"];
            DataRow[] drRows = dtOptionImage.Select("OptionImageID='" + _qsOptionImageID + "'");
            OptionImage   theOptionImage=new OptionImage();

                foreach (DataRow dr in drRows)
                {
                    txtValue.Text = dr["Value"].ToString();
                    imgImage.ImageUrl = _strFilesLocation + "/UserFiles/AppFiles/" + dr["UniqueFileName"].ToString();

                    theOptionImage.OptionImageID = (string)dr["OptionImageID"];
                    theOptionImage.Value=(string) dr["Value"];
                    theOptionImage.FileName=(string) dr["FileName"];
                    theOptionImage.UniqueFileName=(string) dr["UniqueFileName"];
                    ViewState["theOptionImage"] = theOptionImage;

                    break;
                }           
            

           
           
        }
        catch (Exception ex)
        {
            ErrorLog theErrorLog = new ErrorLog(null, "Option Image Detail", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
            SystemData.ErrorLog_Insert(theErrorLog);
            lblMsg.Text = ex.Message;
        }

    }


   
    protected bool IsUserInputOK()
    {
        //this is the final server side vaidation before database action


        return true;
    }



    //protected void cmdSave_Click(object sender, ImageClickEventArgs e)
    protected void lnkSave_Click(object sender, EventArgs e)
    {
        try
        {
          
            if (IsUserInputOK())
            {
                

                switch (_strActionMode.ToLower())
                {
                    case "add":

                        if(ViewState["newOptionImage"]!=null)
                        {
                            OptionImage newOptionImage=(OptionImage)ViewState["newOptionImage"];
                            newOptionImage.Value=txtValue.Text;

                             DataTable dtOptionImage = (DataTable)Session["OptionImage"];

                            dtOptionImage.Rows.Add(newOptionImage.OptionImageID,newOptionImage.Value,
                                newOptionImage.FileName,newOptionImage.UniqueFileName);
                            dtOptionImage.AcceptChanges();
                             Session["OptionImage"] = dtOptionImage;
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "FileSelect", "alert('Please select an image file.');", true);
                            return;
                        }

                        

                        break;

                    case "view":


                        break;

                    case "edit":

                       
                            DataTable dtOptionImageU = (DataTable)Session["OptionImage"];

                            OptionImage theOptionImage = (OptionImage)ViewState["theOptionImage"];
                            for (int i = 0; i < dtOptionImageU.Rows.Count; i++)
                            {
                                if (dtOptionImageU.Rows[i]["OptionImageID"].ToString() == _qsOptionImageID)
                                {
                                    dtOptionImageU.Rows[i]["Value"] = txtValue.Text;
                                    dtOptionImageU.Rows[i]["FileName"] = theOptionImage.FileName;
                                    dtOptionImageU.Rows[i]["UniqueFileName"] = theOptionImage.UniqueFileName;
                                }
                            }

                            dtOptionImageU.AcceptChanges();

                            Session["OptionImage"] = dtOptionImageU;                      
                        
                        
                       
                        break;

                    default:
                        //?
                        break;
                }
            }
            else
            {
                //user input is not ok

            }


            ScriptManager.RegisterStartupScript(this, this.GetType(), "RefreshGrid", "CloseAndRefresh();", true);

        }
        catch (Exception ex)
        {
            ErrorLog theErrorLog = new ErrorLog(null, "Option Image Detail", ex.Message, ex.StackTrace, DateTime.Now, Request.Path);
            SystemData.ErrorLog_Insert(theErrorLog);
            lblMsg.Text = ex.Message;

            
        }



    }

}
