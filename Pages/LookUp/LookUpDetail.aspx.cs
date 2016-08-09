using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Pages_LookUp_LookUpDetail : SecurePage
{
    string _strActionMode = "view";
    int? _iLookupDataID;
    string _qsMode = "";
    User _ObjUser;

    int? _iLookupTypeID = null;
    LookupType _theLookupType;
    protected void Page_Load(object sender, EventArgs e)
    {

        _iLookupTypeID = int.Parse(Cryptography.Decrypt(Request.QueryString["LookupTypeID"].ToString()));

        _theLookupType = SystemData.LookUpType_Detail((int)_iLookupTypeID);

        lblDisplayText.Text = _theLookupType.LookupTypeName + " Display Text*";
        lblValue.Text = _theLookupType.LookupTypeName + " Value*";

        _ObjUser = (User)Session["User"];
               

        if (!IsPostBack)
        {
            if (!Common.HaveAccess(Session["roletype"].ToString(), "1" ))
            { Response.Redirect("~/Default.aspx", false); }

            if (Request.QueryString["SearchCriteria"] != null)
            {
                hlBack.NavigateUrl = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/LookUp/LookUp.aspx" + "?SearchCriteria=" + Request.QueryString["SearchCriteria"].ToString() + "&LookupTypeID=" + Request.QueryString["LookupTypeID"].ToString();
            }
            else
            {
                hlBack.NavigateUrl = "http://" + Request.Url.Authority + Request.ApplicationPath + "/Pages/LookUp/LookUp.aspx?LookupTypeID=" + Request.QueryString["LookupTypeID"].ToString();
            }
           
            
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


                if (Request.QueryString["LookupDataID"] != null)
                {

                    _iLookupDataID = int.Parse(Cryptography.Decrypt(Request.QueryString["LookupDataID"]));
                   
                }

            }
            else
            {
                Server.Transfer("~/Default.aspx");
            }


        }
       

      
        // checking permission
        string strTitle="LookUp Data";
        
        switch (_strActionMode.ToLower())
        {
            case "add":
                strTitle = "Add " + _theLookupType.LookupTypeName;

                break;

            case "view":

                strTitle = "View " + _theLookupType.LookupTypeName;


                PopulateTheRecord();

                EnableTheRecordControls(false);
                divSave.Visible = false;

                break;

            case "edit":

                strTitle = "Edit " + _theLookupType.LookupTypeName;

                if (!IsPostBack)
                {
                    PopulateTheRecord();
                }
                break;


            default:
                //?

                break;
        }


        lblTitle.Text = strTitle;
        Title = strTitle;



    }


    protected void PopulateTheRecord()
    {
        try
        {
            //int iTemp = 0;

            LookUpData theLookUpData = SystemData.LookUpData_Detail((int)_iLookupDataID);


            txtDisplayText.Text = theLookUpData.DisplayText;
            txtValue.Text = theLookUpData.Value;
          
            if (_strActionMode == "edit")
            {
                ViewState["theLookUpData"] = theLookUpData;
               
            }
           
        }
        catch (Exception ex)
        {
          
            lblMsg.Text = ex.Message;
        }

    }


    protected void EnableTheRecordControls(bool p_bEnable)
    {
        txtDisplayText.Enabled = p_bEnable;
        txtValue.Enabled = p_bEnable;


    }

    protected bool IsUserInputOK()
    {
        //this is the final server side vaidation before database action


        return true;
    }



    
    protected void lnkSave_Click(object sender, EventArgs e)
    {
        
        try
        {
            if (IsUserInputOK())
            {

                switch (_strActionMode.ToLower())
                {
                    case "add":



                        LookUpData newLookUpData = new LookUpData(null,_iLookupTypeID, txtDisplayText.Text,txtValue.Text, 
                            null, null);
                        SystemData.LookUpData_Insert(newLookUpData);

                        break;

                    case "view":


                        break;

                    case "edit":
                        LookUpData editLookUpData = (LookUpData)ViewState["theLookUpData"];

                        if ((int)editLookUpData.LookupDataID < 0)
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Problem", "alert('We can not update System Data.');", true);
                            return;
                        }


                        editLookUpData.DisplayText = txtDisplayText.Text;
                        editLookUpData.Value = txtValue.Text;

                        SystemData.LookUpData_Update(editLookUpData);

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

            Response.Redirect(hlBack.NavigateUrl, false);
        }
        catch (Exception ex)
        {
            
            lblMsg.Text = ex.Message;
            
        }
    }  
     

  
}
