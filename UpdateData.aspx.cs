using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Text;
using System.Text.RegularExpressions;
using System.Net.Mail;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.Caching;
using System.Web.UI.HtmlControls;

public partial class UpdateData : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["Email"] != null)
                {
                    txtLogInEmail.Text = Request.QueryString["Email"].ToString();
                }
                if (Request.QueryString["RecordID"] != null)
                {
                    hfRecordID.Value = Request.QueryString["RecordID"].ToString();
                }
                //if (Request.QueryString["Field1"] != null)
                //{
                //    hfField1.Value = Request.QueryString["Field1"].ToString();
                //}
                //if (Request.QueryString["Value1"] != null)
                //{
                //    hfValue1.Value = Request.QueryString["Value1"].ToString();
                //}


                //if (Request.QueryString["Field2"] != null)
                //{
                //    hfField2.Value = Request.QueryString["Field2"].ToString();
                //}
                //if (Request.QueryString["Value2"] != null)
                //{
                //    hfValue2.Value = Request.QueryString["Value2"].ToString();
                //}



                //if (Request.QueryString["Field3"] != null)
                //{
                //    hfField3.Value = Request.QueryString["Field3"].ToString();
                //}
                //if (Request.QueryString["Value3"] != null)
                //{
                //    hfValue3.Value = Request.QueryString["Value3"].ToString();
                //}


                string strFilesLocation = SystemData.SystemOption_ValueByKey_Account("FilesLocation",null,null);
                string strFilesPhisicalPath = SystemData.SystemOption_ValueByKey_Account("FilesPhisicalPath",null,null);

                if (strFilesLocation != "")
                {
                    Session["FilesLocation"] = strFilesLocation;
                }
                else
                {
                    Session["FilesLocation"] = Request.Url.Scheme +"://" + Request.Url.Authority + Request.ApplicationPath;
                }

                if (strFilesPhisicalPath != "")
                {
                    Session["FilesPhisicalPath"] = strFilesPhisicalPath;
                }
                else
                {
                    Session["FilesPhisicalPath"] = Server.MapPath("~");
                }

                Session["RunSpeedLog"] = null;

                if (Request.Browser.IsMobileDevice || Request.UserAgent.Contains("Android"))
                {
                    Session["IsMobile"] = "yes";
                }
                else
                {

                    if (Request.Cookies["IsMobile"] == null)
                    {
                        string u = Request.ServerVariables["HTTP_USER_AGENT"];
                        Regex b = new Regex(@"(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows (ce|phone)|xda|xiino", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        Regex v = new Regex(@"1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        if ((b.IsMatch(u) || v.IsMatch(u.Substring(0, 4))))
                        {
                            Session["IsMobile"] = "yes";
                        }
                    }

                }

                Title = "Please Confirm";


                if (Request.Cookies["UserInformation"] != null)
                {

                    string oUseInfor = Request.Cookies["UserInformation"].Value;
                    string[] aUserInfor = oUseInfor.Split('/');
                    txtLogInEmail.Text = aUserInfor.GetValue(0).ToString();
                    txtLogInPassword.Text = aUserInfor.GetValue(1).ToString();
                    chkRememberMe.Checked = true;

                }
                else
                {
                    txtLogInEmail.Text = "";
                    txtLogInPassword.Text = "";
                    chkRememberMe.Checked = false;
                    txtLogInEmail.Focus();
                    HttpCookie oUseInfor = new HttpCookie("UserInformation", "nothing");
                    oUseInfor.Expires = DateTime.Now.AddDays(-3d);
                    Response.Cookies.Add(oUseInfor);
                }


                if (Request.QueryString["Email"] != null)
                {
                    txtLogInEmail.Text = Request.QueryString["Email"].ToString();
                }

            }


            txtLogInPassword.Attributes.Add("Value", txtLogInPassword.Text);

        }
        catch (Exception ex)
        {
            //
        }




    }

    protected void lnkLogIn_Click(object sender, EventArgs e)
    {

        try
        {

            StringBuilder sb = new StringBuilder();
            string chkValid = "";
            //Username
            if (string.IsNullOrEmpty(txtLogInEmail.Text.Trim()))
            {
                chkValid += "1";
            }
            if (string.IsNullOrEmpty(txtLogInPassword.Text.Trim()))
            {
                chkValid += "2";
            }
            if (!string.IsNullOrEmpty(chkValid))
            {
                sb.AppendLine("Please correct the following errors:");
                if (chkValid.Equals("1"))
                {
                    txtLogInEmail.Focus();
                    sb.AppendLine(Resources.Login.UserReq);
                }
                else
                    if (chkValid.Equals("2"))
                    {
                        txtLogInPassword.Focus();
                        sb.AppendLine(Resources.Login.PassReq);
                    }
                    else
                    {
                        txtLogInEmail.Focus();
                        sb.AppendLine(Resources.Login.UserReq);
                        sb.AppendLine(Resources.Login.PassReq);
                    }
                string messerr = sb.ToString().Replace("\r\n", "");
                //Alert.Show(messerr);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "LoginErr", "alert('" + messerr + "');", true);
            }
            else
            {

                string strLogInEmail = txtLogInEmail.Text;
                string pass = txtLogInPassword.Text;
                User etUser = SecurityManager.User_LoginByEmail(strLogInEmail, pass);


                if (etUser != null)
                {
                    //check if the user has used code or not

                    SecurityManager.User_LoginCount_Increment((int)etUser.UserID);
                    int? iAccountID = SecurityManager.GetPrimaryAccountID((int)etUser.UserID);

                    Account theAccount = SecurityManager.Account_Details((int)iAccountID);

                    //User Actived
                    if ((bool)etUser.IsActive && theAccount != null)
                    {
                        //check if the account is active

                        if (theAccount.IsActive == false)
                        {
                            string strContactUs = SystemData.SystemOption_ValueByKey_Account("ContactUsPage",null,null);
                            if (strContactUs == "")
                            {
                                strContactUs = "www.dbgurus.com.au";
                            }
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "InActiveAccount", "alert('Your account (" + theAccount.AccountName + ") is not active – please contact us via our Contact Us page at " + strContactUs + "');", true);
                            return;
                        }


                        Session["LoginAccount"] = null;

                        if (theAccount.HideMyAccount != null && (bool)theAccount.HideMyAccount)
                        {
                            Session["LoginAccount"] = "AccountID=" + theAccount.AccountID.ToString();

                        }

                        Session["GridPageSize"] = SystemData.SystemOption_ValueByKey_Account("GridPageSize", iAccountID, null);

                        Session["User"] = etUser;
                        Session["AccountID"] = iAccountID;


                        //update session id

                        SecurityManager.User_SessionID_Update((int)etUser.UserID, Session.SessionID.ToString(), (int)iAccountID);

                        ///

                        string strUserInfor = string.Format("{0};{1};{2};{3};{4}", etUser.Email, etUser.Password, etUser.FirstName, etUser.UserID, iAccountID);

                        FormsAuthentication.SetAuthCookie(strUserInfor, false);
                        if (chkRememberMe.Checked)
                        {
                            HttpCookie oUseInfor = new HttpCookie("UserInformation", etUser.Email + "/" + etUser.Password + "/" + etUser.FirstName);
                            oUseInfor.Expires = DateTime.Now.AddDays(300d);
                            Response.Cookies.Add(oUseInfor);
                        }
                        else
                        {
                            HttpCookie oUseInfor = new HttpCookie("UserInformation", "");
                            oUseInfor.Expires = DateTime.Now.AddDays(-3d);
                            Response.Cookies.Add(oUseInfor);
                        }

                        string roletype = "";
                        roletype = SecurityManager.GetUserRoleTypeID((int)etUser.UserID, (int)iAccountID);



                        Session["roletype"] = roletype;

                        UserRole theUserRole = SecurityManager.GetUserRole((int)etUser.UserID, (int)iAccountID);

                        Session["UserRole"] = theUserRole;

                        if ((bool)theUserRole.IsAdvancedSecurity)
                        {
                            if (Session["roletype"].ToString() != Common.UserRoleType.OwnData)
                            {
                                Session["roletype"] = Common.UserRoleType.ReadOnly;
                            }
                        }

                        //Insert Usage
                        try
                        {
                            Usage theUsage = new Usage(null, iAccountID, DateTime.Now, 1, 0);

                            SecurityManager.Usage_Insert(theUsage);
                        }
                        catch
                        {
                            //
                        }



                        if (roletype.Length > 0)
                        {
                            FormsAuthentication.RedirectFromLoginPage(strLogInEmail, false);

                            if (SecurityManager.IsRecordsExceeded(int.Parse(Session["AccountID"].ToString())))
                            {
                                Session["DoNotAllow"] = "true";
                            }

                            // Here are the confirmation

                            if(hfRecordID.Value!="")
                            {
                                string UpdateValues = Request.Url.Query;
                                UpdateValues = Server.UrlDecode(UpdateValues);
                                //if (hfField1.Value != "" && hfValue1.Value != "")
                                //{
                                //    Common.ExecuteText("UPDATE [Record] SET " + hfField1.Value + "='" + hfValue1.Value.Replace("'", "''") + "' WHERE RecordID=" + hfRecordID.Value);

                                //}

                                //if (hfField2.Value != "" && hfValue2.Value != "")
                                //{
                                //    Common.ExecuteText("UPDATE [Record] SET " + hfField2.Value + "='" + hfValue2.Value.Replace("'", "''") + "' WHERE RecordID=" + hfRecordID.Value);

                                //}

                                //if (hfField3.Value != "" && hfValue3.Value != "")
                                //{
                                //    Common.ExecuteText("UPDATE [Record] SET " + hfField3.Value + "='" + hfValue3.Value.Replace("'", "''") + "' WHERE RecordID=" + hfRecordID.Value);

                                //}

                                Record theRecord = RecordManager.ets_Record_Detail_Full(int.Parse(hfRecordID.Value));
                                Session["SPUpdateConfirmMessage"] = null;
                                if(theRecord!=null)
                                {
                                    Table theTable = RecordManager.ets_Table_Details((int)theRecord.TableID);

                                    if (theTable.SPUpdateConfirm != null && theTable.SPUpdateConfirm != "")
                                    {
                                        string strHTML = RecordManager.Table_SPUpdateConfirm(theTable.SPUpdateConfirm, theRecord.RecordID, etUser.UserID, UpdateValues);
                                        if (strHTML != "")
                                        {
                                            Session["SPUpdateConfirmMessage"] = strHTML;
                                        }
                                    }
                                }
                               

                                Response.Redirect("~/UpdateConfirm.aspx?RecordID=" +  Cryptography.Encrypt(hfRecordID.Value), false);
                                return;
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "NotEnoughValue", "alert('The link is not correct');", true);
                                return;
                            }

                        }
                        else
                        {
                            //user has no role
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "NoRole", "alert('This user has no role');", true);
                            return;
                        }
                    }
                    else
                    {
                        txtLogInEmail.Focus();
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "InActiveUser", "alert('" + Resources.Login.LoginActive + "');", true);
                    }
                }
                else
                {
                    txtLogInEmail.Focus();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "LoginFail", "alert('" + Resources.Login.LoginErr + "');", true);
                }
            }


        }
        catch (Exception ex)
        {
            //
        }

    }
}