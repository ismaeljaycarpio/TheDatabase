﻿using System;
using System.Collections.Generic;
//using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Data;
using System.Web.Security;
using System.Configuration;
using System.Collections;
using InnovaStudio;

public partial class Extender_InlineContentEditor : SecurePage
{


    // Place your customizations in Section 1. Do not modify Section 2.
    #region "Section 1: Place your customizations here."
    // Editor's Script Location
    string sEditorScriptPath = "../Editor/scripts/";

    // Editor's CSS
    string sEditorCSS = "../Styles/Style.css";

    // Editor's Asset Manager
    string sEditorAssetManager = "";


    protected override void OnLoad(EventArgs e)
    {
        // secure this page
        bool bAuthorized = false;

        string sUsername = "";
        if (Session["LoginInfo"] != null)
        {
            sUsername = ((Hashtable)Session["LoginInfo"])["UserName"].ToString();
        }



        //if (sUsername.Length > 0)
        //{
        //    if (DBGurus.UserHasRole(sUsername,1))
        //    {
        //        bAuthorized = true;
        //    }
        //}

        //if (bAuthorized == false)
        //{
        //    // redirect to forbidden page
        //    Page.Response.Redirect("../Shared/Forbidden.aspx");
        //}

        base.OnLoad(e);
    }

    /// <summary>
    /// Use this method to initialize the editor
    /// </summary>
    private void LoadEditor()
    {
        /* 
         * Customize below codes when it's needed
         */
        string nContentKey = "";

        string sError = "";

        // Get Main Content
        oEditor.Text = DBGurus.GetContentWithValues(sContentID, null, out nContentKey, out sError);
        this.sContentKey = nContentKey.ToString();
    }

    /// <summary>
    /// Use this method to update the database with current editot's content
    /// </summary>
    private void SaveEditor()
    {
        /* 
        * Customize below codes when it's needed
        */

        // Save the changes to the database
        string sErrorInfo = "";
        string sSQL = string.Format("UPDATE Content SET Content='{0}' WHERE ContentID={1}", this.oEditor.Text.Replace("'", "''"), sContentID);

        if (DBGurus.ExecuteSQL(sSQL, out sErrorInfo) != 0)
        {
            //DBGurus.AddErrorLog(sErrorInfo);
        }
    }
    #endregion

    #region "Section 2: Do not modify this section."

    #region Local Variables
    private string sUniqueID = "";
    private string sTableName = "";
    private string sContentKey = "";
    private string sReturnURL = "";
    private string sContentID = "";
    private bool bAssetManager = false;
    private string sFontSize = "";

    private string VIEW_STATE_UNIQUE_ID = "ViewState_UniqueID";
    private string VIEW_STATE_TABLE_NAME = "ViewState_TableName";
    private string VIEW_STATE_CONTENT_KEY = "ViewState_ContentKey";
    private string VIEW_STATE_RETURN_URL = "ViewState_ReturnURL";
    private string VIEW_STATE_CONTENT_ID = "ViewState_ContentID";
    private string VIEW_STATE_ASSET_MANAGER = "ViewState_AssetManager";
    private string VIEW_STATE_FONT_SIZE = "ViewState_FontSize";
    #endregion

    #region Event Handlers
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.RawUrl.IndexOf("SDisplayContent.aspx") > -1)
        {
            oEditor.Width = 1375;
            oEditor.Height = 610;
        }

        LoadVariables();
        InitializeEditor();
    }

    protected void SaveButton_Click(object sender, EventArgs e)
    {
        // save changes
        SaveEditor();

        Page.ClientScript.RegisterStartupScript(
            typeof(Page), "Editor_Close", string.Format("parent.js_{0} = 1; parent.contenteditorwindow.hide();", sUniqueID.Replace("-", "_")), true);
    }

    protected override object SaveViewState()
    {
        // save local variables to the view state
        this.ViewState.Add(VIEW_STATE_UNIQUE_ID, this.sUniqueID);
        this.ViewState.Add(VIEW_STATE_TABLE_NAME, this.sTableName);
        this.ViewState.Add(VIEW_STATE_CONTENT_KEY, this.sContentKey);
        this.ViewState.Add(VIEW_STATE_RETURN_URL, this.sReturnURL);
        this.ViewState.Add(VIEW_STATE_CONTENT_ID, this.sContentID);
        this.ViewState.Add(VIEW_STATE_ASSET_MANAGER, this.bAssetManager);
        this.ViewState.Add(VIEW_STATE_FONT_SIZE, this.sFontSize);

        return base.SaveViewState();
    }

    protected override void LoadViewState(object savedState)
    {
        base.LoadViewState(savedState);

        // load view states to the local variables
        if (this.ViewState[VIEW_STATE_UNIQUE_ID] != null)
            this.sUniqueID = this.ViewState[VIEW_STATE_UNIQUE_ID].ToString();
        if (this.ViewState[VIEW_STATE_TABLE_NAME] != null)
            this.sTableName = this.ViewState[VIEW_STATE_TABLE_NAME].ToString();
        if (this.ViewState[VIEW_STATE_CONTENT_KEY] != null)
            this.sContentKey = this.ViewState[VIEW_STATE_CONTENT_KEY].ToString();
        if (this.ViewState[VIEW_STATE_RETURN_URL] != null)
            this.sReturnURL = this.ViewState[VIEW_STATE_RETURN_URL].ToString();
        if (this.ViewState[VIEW_STATE_CONTENT_ID] != null)
            this.sContentID = this.ViewState[VIEW_STATE_CONTENT_ID].ToString();
        if (this.ViewState[VIEW_STATE_ASSET_MANAGER] != null)
            this.bAssetManager = Convert.ToBoolean(this.ViewState[VIEW_STATE_ASSET_MANAGER].ToString());
        if (this.ViewState[VIEW_STATE_FONT_SIZE] != null)
            this.sFontSize = this.ViewState[VIEW_STATE_FONT_SIZE].ToString();
    }
    #endregion

    #region Methods
    private void LoadVariables()
    {
        if (!Page.IsPostBack)
        {
            // load from query string
            this.sUniqueID = Request.QueryString["UID"].ToString();
            this.sTableName = Request.QueryString["TableName"].ToString();
            this.sContentID = Request.QueryString["ContentID"].ToString();
            this.sReturnURL = Request.QueryString["ReturnURL"].ToString();
            if (Request.QueryString["AssetManager"] != null)
                this.bAssetManager = Convert.ToBoolean(Request.QueryString["AssetManager"].ToString());
            if (Request.QueryString["FontSize"] != null)
                this.sFontSize = Request.QueryString["FontSize"].ToString();
        }
    }

    /// <summary>
    /// Editor Initialization
    /// </summary>
    private void InitializeEditor()
    {
        // initialize default script path
        if (this.sEditorScriptPath == "")
            oEditor.scriptPath = "../Editor/scripts/";
        else
            oEditor.scriptPath = sEditorScriptPath;

        // initialize default CSS
        if (this.sEditorCSS == "")
            oEditor.Css = "../Styles/Style.css";
        else
            oEditor.Css = sEditorCSS;

        // initialize Asset Manager
        if (bAssetManager)
        {
            if (this.sEditorAssetManager == "")
            {
                // get browser id
                bool bMozilla = this.Request.Browser.Id.ToLower().Contains("mozilla");
                if (bMozilla)
                {
                    oEditor.AssetManager = System.Configuration.ConfigurationManager.AppSettings["AssetManagerMozilla"];
                }
                else
                {
                    oEditor.AssetManager = System.Configuration.ConfigurationManager.AppSettings["AssetManager"];
                }
            }
            else
                oEditor.AssetManager = sEditorAssetManager;
        }
        else
        {
            oEditor.AssetManager = "";
        }

        if (!Page.IsPostBack)
        {
            // call customizable codes to fill the content editor
            LoadEditor();
        }

        if (sFontSize != "")
            //oEditor.StyleList = new string[,]{
            //    {"body",false.ToString(),"",string.Format("font-size:{0};", sFontSize)},
            //    {"td",false.ToString(),"",string.Format("font-size:{0};", sFontSize)}
            //};

            oEditor.EditingStyles.Add(new EditingStyle("body", false, "", string.Format("font-size:{0};", "10px")));
        oEditor.EditingStyles.Add(new EditingStyle("td", false, "", string.Format("font-size:{0};", "10px")));

    }
    #endregion

    #endregion
}
