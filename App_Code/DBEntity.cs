using System;
using System.Collections.Generic;
//using System.Linq;
using System.Web;
using DocGen.DAL;
/// <summary> MR: Dec-2010
/// Summary description for DBEntity
/// </summary>
/// 
[Serializable]
public class Role
{
    private int? _iRoleID;
    private string _strRole;
    private string _strRoleType;
    private string _strRoleNotes;
    private DateTime? _dateAdded;
    private DateTime? _dateUpdated;
    public int? AccountID { get; set; }
    public bool? IsSystemRole { get; set; }
    public bool? IsActive { get; set; }

    public int? OwnerUserID { get; set; }

    public bool? AllowEditDashboard { get; set; }

    public int? DashboardDefaultFromUserID { get; set; }

    public bool? AllowEditView { get; set; }
    public int? ViewsDefaultFromUserID { get; set; }
    public Role(int? p_iRoleID, string p_strRole, string p_strRoleType,
        string p_strRoleNotes, DateTime? p_dateAdded, DateTime? p_dateUpdated)
	{
        _iRoleID = p_iRoleID;
        _strRole = p_strRole;
        _strRoleType = p_strRoleType;
        _strRoleNotes = p_strRoleNotes;
        _dateAdded = p_dateAdded;
        _dateUpdated = p_dateUpdated;
	}

    public int? RoleID { get { return _iRoleID; }
        set { _iRoleID = value; }
    }
    public string RoleName { get { return _strRole; }
        set { _strRole = value; }
    }
    public string RoleType { get { return _strRoleType; }
        set { _strRoleType = value; }
    }
    public string RoleNotes { get { return _strRoleNotes; }
        set { _strRoleNotes = value; }
    }
    public DateTime? DateAdded { get { return _dateAdded; }
        set { _dateAdded = value; }
    }
    public DateTime? DateUpdated { get { return _dateUpdated; }
        set { _dateUpdated = value; }
    }


}




[Serializable]
public class SystemOption
{
    private int? _iSystemOptionID;
    private string _strOptionKey;
    private string _strOptionValue;
    private string _strOptionNotes;
    private DateTime? _dateAdded;
    private DateTime? _dateUpdated;
    public int? AccountID { get; set; }
    public int? TableID { get; set; } 
    public SystemOption(int? p_iSystemOptionID, string p_strOptionKey, string p_strOptionValue,
        string p_strOptionNotes, DateTime? p_dateAdded, DateTime? p_dateUpdated)
    {
        _iSystemOptionID = p_iSystemOptionID;
        _strOptionKey = p_strOptionKey;
        _strOptionValue = p_strOptionValue;
        _strOptionNotes = p_strOptionNotes;
        _dateAdded = p_dateAdded;
        _dateUpdated = p_dateUpdated;
    }

    public int? SystemOptionID
    {
        get { return _iSystemOptionID; }
        set { _iSystemOptionID = value; }
    }
    public string OptionKey
    {
        get { return _strOptionKey; }
        set { _strOptionKey = value; }
    }
    public string OptionValue
    {
        get { return _strOptionValue; }
        set { _strOptionValue = value; }
    }
    public string OptionNotes
    {
        get { return _strOptionNotes; }
        set { _strOptionNotes = value; }
    }
    public DateTime? DateAdded
    {
        get { return _dateAdded; }
        set { _dateAdded = value; }
    }
    public DateTime? DateUpdated
    {
        get { return _dateUpdated; }
        set { _dateUpdated = value; }
    }


}


[Serializable]
public class ErrorLog
{
    private int? _iErrorLogID;
    private string _strModule;
    private string _strErrorMessage;
    private string _strErrorTrack;
    private DateTime? _dateErrorTime;
    private string _strPath;

    public ErrorLog(int? p_iErrorLogID, string p_strModule, string p_strErrorMessage,
        string p_strErrorTrack, DateTime? p_dateErrorTime, string p_strPath)
    {
        _iErrorLogID = p_iErrorLogID;
        _strModule = p_strModule;
        _strErrorMessage = p_strErrorMessage;
        _strErrorTrack = p_strErrorTrack;
        _dateErrorTime = p_dateErrorTime;
        _strPath = p_strPath;
    }

    public int? ErrorLogID
    {
        get { return _iErrorLogID; }
        set { _iErrorLogID = value; }
    }
    public string Module
    {
        get { return _strModule; }
        set { _strModule = value; }
    }
    public string ErrorMessage
    {
        get { return _strErrorMessage; }
        set { _strErrorMessage = value; }
    }
    public string ErrorTrack
    {
        get { return _strErrorTrack; }
        set { _strErrorTrack = value; }
    }
    public DateTime? ErrorTime
    {
        get { return _dateErrorTime; }
        set { _dateErrorTime = value; }
    }
    public string Path
    {
        get { return _strPath; }
        set { _strPath = value; }
    }


}






[Serializable]
public class User
{
    private int? _iUserID;
    private string _strFirstName;
    private string _strLastName;
    private string _strPhoneNumber;
    private string _strEmail;
    private string _strPassword;
    private bool? _bIsActive;
    private DateTime? _dateAdded;
    private DateTime? _dateUpdated;
    //private int? _iAccountID;
    
   
  

    
    //public int? RoleGroupID { get; set; }
    public User(int? p_iUserID, string p_strFirstName,
        string p_strLastName, string p_strPhoneNumber, string p_strEmail, string p_strPassword,
        bool? p_bIsActive, DateTime? p_dateAdded, DateTime? p_dateUpdated)
    {
        _iUserID = p_iUserID;
        _strFirstName = p_strFirstName;
        _strLastName = p_strLastName;
        _strPhoneNumber = p_strPhoneNumber;
        _strEmail = p_strEmail;
        _strPassword = p_strPassword;
        _bIsActive = p_bIsActive;
        _dateAdded = p_dateAdded;
        _dateUpdated = p_dateUpdated;
        //_strAccountName = p_strAccountName;
        //_bIsAccountHolder = p_bIsAccountHolder;
        //_bIsAdvancedSecurity = p_bIsAdvancedSecurity;
      
    }

  
    

    public int? UserID
    {
        get { return _iUserID; }
        set { _iUserID = value; }
    }
    public string FirstName
    {
        get { return _strFirstName; }
        set { _strFirstName = value; }
    }
    public string LastName
    {
        get { return _strLastName; }
        set { _strLastName = value; }
    }

    public string PhoneNumber
    {
        get { return _strPhoneNumber; }
        set { _strPhoneNumber = value; }
    }
    public string Email
    {
        get { return _strEmail; }
        set { _strEmail = value; }
    }
    public string Password
    {
        get { return _strPassword; }
        set { _strPassword = value; }
    }
    public bool? IsActive
    {
        get { return _bIsActive; }
        set { _bIsActive = value; }
    }
   
    public DateTime? DateAdded 
    {
        get { return _dateAdded; }
        set { _dateAdded = value; }
    }
    public DateTime? DateUpdated
    {
        get { return _dateUpdated; }
        set { _dateUpdated = value; }
    }
 
   
   
   

}



[Serializable]
public class UserRole
{
    private int? _iUserRoleID;
    private int? _iUserID;
    private int? _iRoleID;
    private DateTime? _dateAdded;
    private DateTime? _dateUpdated;
 
 
    public int? AccountID { get; set; }
    public bool? IsPrimaryAccount { get; set; }

   // private DateTime? _dateAlertSeen;
    public bool? IsAccountHolder { get; set; }
    public bool? IsAdvancedSecurity { get; set; }
    public DateTime? AlertSeen { get; set; }
    public bool? IsDocSecurityAdvanced { get; set; }
    public string DocSecurityType { get; set; }
    public int? DashBoardDocumentID { get; set; }
    public int? DataScopeColumnID { get; set; }
    public string DataScopeValue { get; set; }

    //public bool? AllowEditDashboard { get; set; }
    public bool? AllowDeleteTable { get; set; }
    public bool? AllowDeleteColumn { get; set; }
    public bool? AllowDeleteRecord { get; set; }

    public UserRole()
    {

    }

    public UserRole(int? p_iUserRoleID, int?  p_iUserID, int?  p_iRoleID,
        DateTime? p_dateAdded, DateTime? p_dateUpdated)
    {
        _iUserRoleID = p_iUserRoleID;
        _iUserID = p_iUserID;
        _iRoleID = p_iRoleID;
        _dateAdded = p_dateAdded;
        _dateUpdated = p_dateUpdated;
      
    }

    public int? UserRoleID { get { return _iUserRoleID; } 
        set { _iUserRoleID = value; } 
    }
    public int? UserID { get { return _iUserID; } 
        set { _iUserID = value; } 
    }
    public int? RoleID { get { return _iRoleID; } 
        set { _iRoleID = value; } 
    }

    public DateTime? DateAdded { get { return _dateAdded; } 
        set { _dateAdded = value; } 
    }
    public DateTime? DateUpdated { get { return _dateUpdated; } 
        set { _dateUpdated = value; } 
    }

   
}

[Serializable]
public class Account
{
    
    
    private int? _iAccountID;
    private string _strAccountName;   
    private DateTime? _dateAdded;
    private DateTime? _dateUpdated;
    private object _oLogo = null;
    private bool? _bIsActive;
  
    private int? _iAccountTypeID;
    private DateTime? _dateExpiryDate;
    private bool? _bUseDefaultLogo;
    private double? _dMapCentreLat;
    private double? _dMapCentreLong;
    private int? _iMapZoomLevel;
 
    private int? _iMapDefaultTableID;
    private DateTime? _dateExceedLastEmail;
    private int? _iOtherMapZoomLevel;
    private int? _iCountryID;
    private string _strPhoneNumber;
    private bool? _bCreatedByWizard;
    private int? _iExtensionPacks;
    private bool? _bAlerts;
    private bool? _bReportGen;
    private int? _iLayout;

    public bool? SystemAccount { get; set; }
    public string ConfirmationCode { get; set; }
    public int? NextBilledAccountTypeID { get; set; }
    public string ClientRef { get; set; }
    public bool? GSTApplicable { get; set; }
    public string OrganisationName { get; set; }
    public string BillingPhoneNumber { get; set; }
    public string BillingAddress { get; set; }
    public string BillingEmail { get; set; }
    public int? BillEveryXMonths { get; set; }
    public string PaymentMethod { get; set; }
    public string BillingFirstName { get; set; }
    public string BillingLastName { get; set; }
    public string Comment { get; set; }
    public int? DefaultGraphOptionID { get; set; }

    public string CopyRightInfo { get; set; }
    public int? LoginContentID { get; set; }
    public bool? HideMyAccount { get; set; }
    public bool? HideDashBoard { get; set; }  

    public string MasterPage { get; set; }

    public bool? UseDataScope { get; set; }
    public int? DisplayTableID { get; set; }
    public string HomeMenuCaption { get; set; }
    public bool? ShowOpenMenu { get; set; }


    public bool? IsReportTopMenu { get; set; }
    public bool? UploadAfterVerificaition { get; set; }

    public int? EmailCount { get; set; }
    public int? SMSCount { get; set; }
    public bool? LabelOnTop { get; set; }
    public string HomePageLink { get; set; }

    public string ReportServer { get; set; }

    public string ReportServerUrl { get; set; }

    public string ReportUser { get; set; }
    public string ReportPW { get; set; }
    public string SMTPEmail { get; set; }

    public string SMTPUserName { get; set; }
    public string SMTPPassword { get; set; }
    public string SMTPPort { get; set; }
    public string SMTPServer { get; set; }
    public string SMTPSSL { get; set; }
    public string SMTPReplyToEmail { get; set; }
    public string POP3Email { get; set; }
    public string POP3UserName { get; set; }
    public string POP3Password { get; set; }
    public string POP3Port { get; set; }
    public string POP3Server { get; set; }
    public string POP3SSL { get; set; }


    public string SPAfterLogin { get; set; }


    public Account(int? p_iAccountID, string p_strAccountName,
        DateTime? p_dateAdded, DateTime? p_dateUpdated, int? p_iAccountTypeID,
        DateTime? p_dateExpiryDate)
    {
        _iAccountID = p_iAccountID;
        _strAccountName = p_strAccountName;       
        _dateAdded = p_dateAdded;
        _dateUpdated = p_dateUpdated;
        _iAccountTypeID = p_iAccountTypeID;
        _dateExpiryDate = p_dateExpiryDate;
    }

    public int? Layout
    {
        get { return _iLayout; }
        set { _iLayout = value; }
    }
    

    public int? ExtensionPacks
    {
        get { return _iExtensionPacks; }
        set { _iExtensionPacks = value; }
    }



    public bool? Alerts
    {
        get { return _bAlerts; }
        set { _bAlerts = value; }
    }


    public bool? ReportGen
    {
        get { return _bReportGen; }
        set { _bReportGen = value; }
    }


    public bool? CreatedByWizard
    {
        get { return _bCreatedByWizard; }
        set { _bCreatedByWizard = value; }
    }

    public string PhoneNumber
    {
        get { return _strPhoneNumber; }
        set { _strPhoneNumber = value; }
    }

    public int? CountryID
    {
        get { return _iCountryID; }
        set { _iCountryID = value; }
    }



    public int? OtherMapZoomLevel
    {
        get { return _iOtherMapZoomLevel; }
        set { _iOtherMapZoomLevel = value; }
    }

    public DateTime? ExceedLastEmail
    {
        get { return _dateExceedLastEmail; }
        set { _dateExceedLastEmail = value; }
    }

    public int? MapDefaultTableID
    {
        get { return _iMapDefaultTableID; }
        set { _iMapDefaultTableID = value; }
    }

    public double? MapCentreLat
    {
        get { return _dMapCentreLat; }
        set { _dMapCentreLat = value; }
    }
    public double? MapCentreLong
    {
        get { return _dMapCentreLong; }
        set { _dMapCentreLong = value; }
    }
    public int? MapZoomLevel
    {
        get { return _iMapZoomLevel; }
        set { _iMapZoomLevel = value; }
    }
  

   

    public bool? UseDefaultLogo
    {
        get { return _bUseDefaultLogo; }
        set { _bUseDefaultLogo = value; }
    }

    public int? AccountTypeID
    {
        get { return _iAccountTypeID; }
        set { _iAccountTypeID = value; }
    }

    public DateTime? ExpiryDate
    {
        get { return _dateExpiryDate; }
        set { _dateExpiryDate = value; }
    }

    public int? AccountID { get { return _iAccountID; }
        set { _iAccountID = value; }
    }
    public string AccountName { get { return _strAccountName; }
        set { _strAccountName = value; }
    }
  
    public DateTime? DateAdded { get { return _dateAdded; }
        set { _dateAdded = value; }
    }
    public DateTime? DateUpdated { get { return _dateUpdated; }
        set { _dateUpdated = value; }
    }

   

    public object Logo
    {
        get { return _oLogo; }
        set { _oLogo = value; }
    }

    public bool? IsActive
    {
        get { return _bIsActive; }
        set { _bIsActive  = value; }
    }

  



}


[Serializable]
public class Table
{
    private int? _iTableID;
    private string _strTableName;
    private DateTime? _dateAdded;
    private DateTime? _dateUpdated;
   
    //private bool? _bIsImportPositional=false;
    private bool? _bIsActive;
    private int? _iAccountID;
   
    private string _strPinImage;
    private double? _dMaxTimeBetweenRecords;
    private string _strMaxTimeBetweenRecordsUnit;
    private int? _iLastUpdatedUserID;
    private int? _iLateDataDays;
    //private int? _iImportDataStartRow;

    private string _strDateFormat;

    public int? DisplayOrder { get; set; }

    //public bool? IsRecordDateUnique { get; set; }
    public int? UniqueColumnID { get; set; }
    public int? UniqueColumnID2 { get; set; }
    //public bool? IsDataUpdateAllowed { get; set; }

    public string DuplicateRecordAction { get; set; }
    public bool? FlashAlerts { get; set; }


    public int? FilterColumnID { get; set; }
    public string FilterDefaultValue { get; set; }

    public string ReasonChangeType { get; set; }
    public string ChangeHistoryType { get; set; }
    public bool? AddWithoutLogin { get; set; }
    public int? ParentTableID { get; set; }

   public bool? AddUserRecord  { get; set; }
	public int? AddUserUserColumnID  { get; set; }
	public int? AddUserPasswordColumnID  { get; set; }
    public bool? AddUserNotification { get; set; }

    //public int? ImportColumnHeaderRow { get; set; }

    //public int? TempImportColumnHeaderRow { get; set; }
    //public int? TempImportDataStartRow { get; set; }
    public string TempFullUniqueFileName { get; set; }

    public int? SortColumnID { get; set; }
    public string HeaderName { get; set; }
    public bool? HideAdvancedOption { get; set; }

    public int? ValidateColumnID1 { get; set; }
    public int? ValidateColumnID2 { get; set; }

    public string HeaderColor  { get; set; }
    public string JSONAttachmentPOP3 { get; set; }
    public string JSONAttachmentInfo { get; set; }

    public bool? ShowTabVertically { get; set; }

    public int? GraphXAxisColumnID { get; set; }
    public int? GraphSeriesColumnID { get; set; }
    public int? GraphDefaultPeriod { get; set; }


    public bool? CopyToChildrenAfterImport { get; set; }
    public string CustomUploadSheet { get; set; }
    public string FilterType { get; set; }

    public string TabColour { get; set; }
    public string TabTextColour { get; set; }
    public bool? BoxAroundField { get; set; }
    public string FilterTopColour { get; set; }
    public string FilterBottomColour { get; set; }

    public bool? ShowEditAfterAdd { get; set; }
    public bool? AddOpensForm { get; set; }
    public string AddRecordSP { get; set; }
    public string SPSaveRecord { get; set; }
    public bool? SaveAndAdd { get; set; }

    public bool? HideFilter { get; set; }
    public bool? NavigationArrows { get; set; }

    //public int? DataUpdateUniqueColumnID { get; set; }

    public bool? AllowCopyRecords { get; set; }

    public string SPSendEmail { get; set; }

    public string SPUpdateConfirm { get; set; }
    public bool? ShowSentEmails { get; set; }
    public bool? ShowReceivedEmails { get; set; }
    //public string SPAfterImport { get; set; }

    public int? PinDisplayOrder { get; set; }

    public string GraphOnStart { get; set; }

    public int? GraphDefaultYAxisColumnID { get; set; }
    public string SummaryPageContent { get; set; }

    public int? DefaultImportTemplateID { get; set; }

    public string SPSearchGo { get; set; }
    public string SPSearchReset { get; set; }
    public bool? ShowChildTabsOnAdd { get; set; }

    public Table(int? p_iTableID, string p_strTableName, 
        DateTime? p_dateAdded, DateTime? p_dateUpdated
        , bool? p_bIsActive)
    {
        _iTableID = p_iTableID;       
        _strTableName = p_strTableName;
        _dateAdded = p_dateAdded;
        _dateUpdated = p_dateUpdated;       
        _bIsActive = p_bIsActive;

    }
    //_strDateFormay
   

    public string DateFormat
    {
        get { return _strDateFormat; }
        set { _strDateFormat = value; }
    }

    //public int? ImportDataStartRow
    //{
    //    get { return _iImportDataStartRow; }
    //    set { _iImportDataStartRow = value; }
    //}

    public int? LateDataDays
    {
        get { return _iLateDataDays; }
        set { _iLateDataDays = value; }
    }

    public int? LastUpdatedUserID
    {
        get { return _iLastUpdatedUserID; }
        set { _iLastUpdatedUserID = value; }
    }

   

    public int? AccountID
    {
        get { return _iAccountID; }
        set { _iAccountID = value; }
    }


    public bool? IsActive
    {
        get { return _bIsActive; }
        set { _bIsActive = value; }
    }
   
    public int? TableID
    {
        get { return _iTableID; }
        set { _iTableID = value; }
    }
    public string TableName
    {
        get { return _strTableName; }
        set { _strTableName = value; }
    }
   
   public DateTime? DateAdded
    {
        get { return _dateAdded; }
        set { _dateAdded = value; }
    }
    public DateTime? DateUpdated
    {
        get { return _dateUpdated; }
        set { _dateUpdated = value; }
    }

    

  

  

    //public bool? IsImportPositional
    //{
    //    get { return _bIsImportPositional; }
    //    set { _bIsImportPositional = value; }
    //}

    public string PinImage
    {
        get { return _strPinImage; }
        set { _strPinImage = value; }
    }


    public double? MaxTimeBetweenRecords
    {
        get { return _dMaxTimeBetweenRecords; }
        set { _dMaxTimeBetweenRecords = value; }
    }

    public string MaxTimeBetweenRecordsUnit
    {
        get { return _strMaxTimeBetweenRecordsUnit; }
        set { _strMaxTimeBetweenRecordsUnit = value; }
    }



}

[Serializable]
public class Menu
{
    private int? _iMenuID;
    private string _strMenuP;
    private int? _iAccountID;
    private bool? _bShowOnMenu;
    //private string _strAccountName;
    private bool? _bIsActive;
    public int? DisplayOrder { get; set; }
    public int? ParentMenuID { get; set; }
    public int? TableID { get; set; }

    public int? DocumentID { get; set; }
    //public string CustomPageLink { get; set; }

    public string ExternalPageLink { get; set; }
    public bool? OpenInNewWindow { get; set; }
    public string TableName { get; set; }
        
    public int? DocumentTypeID { get; set; }
    public string MenuType { get; set; }
    public Menu(int? p_iMenuID, string p_strMenu,
        int? p_iAccountID, bool? p_bShowOnMenu,  bool? p_bIsActive)
    {
        _iMenuID = p_iMenuID;
        _strMenuP = p_strMenu;
        _iAccountID = p_iAccountID;
        _bShowOnMenu = p_bShowOnMenu;
        _bIsActive = p_bIsActive;
        
    }
    public bool? IsActive
    {
        get { return _bIsActive; }
        set { _bIsActive = value; }
    }
    public int? MenuID
    {
        get { return _iMenuID; }
        set { _iMenuID = value; }
    }
    public string MenuP
    {
        get { return _strMenuP; }
        set { _strMenuP = value; }
    }
    public int?  AccountID
    {
        get { return _iAccountID; }
        set { _iAccountID = value; }
    }
    public bool?  ShowOnMenu
    {
        get { return _bShowOnMenu; }
        set { _bShowOnMenu = value; }
    }
    //public string AccountName
    //{
    //    get { return _strAccountName; }
    //    set { _strAccountName = value; }
    //}
   
}



[Serializable]
public class Record
{
    private int? _iRecordID;
    private int? _iTableID;
    private DateTime? _dtDateTimeRecorded;
    private string _strNotes;
    private int? _iEnteredBy;
    private bool? _bIsActive;
    private DateTime? _dtDateAdded;
    private DateTime? _dtDateUpdated;
    
    private string _strValidationResults;
    private string _strWarningResults;

    private int? _iTempRecordID;
    private int? _iLastUpdatedUserID;

    private string _strTableName;
    private string _strEnteredByUserName;



    private string _strChangeReason;
    public int? OwnerUserID { get; set; }
    public int? BatchID { get; set; }
    public string DeleteReason { get; set; }
    public string V001 { get; set; }
    public string V002 { get; set; }
    public string V003 { get; set; }
    public string V004 { get; set; }
    public string V005 { get; set; }
    public string V006 { get; set; }
    public string V007 { get; set; }
    public string V008 { get; set; }
    public string V009 { get; set; }
    public string V010 { get; set; }
    public string V011 { get; set; }
    public string V012 { get; set; }
    public string V013 { get; set; }
    public string V014 { get; set; }
    public string V015 { get; set; }
    public string V016 { get; set; }
    public string V017 { get; set; }
    public string V018 { get; set; }
    public string V019 { get; set; }
    public string V020 { get; set; }
    public string V021 { get; set; }
    public string V022 { get; set; }
    public string V023 { get; set; }
    public string V024 { get; set; }
    public string V025 { get; set; }
    public string V026 { get; set; }
    public string V027 { get; set; }
    public string V028 { get; set; }
    public string V029 { get; set; }
    public string V030 { get; set; }
    public string V031 { get; set; }
    public string V032 { get; set; }
    public string V033 { get; set; }
    public string V034 { get; set; }
    public string V035 { get; set; }
    public string V036 { get; set; }
    public string V037 { get; set; }
    public string V038 { get; set; }
    public string V039 { get; set; }
    public string V040 { get; set; }
    public string V041 { get; set; }
    public string V042 { get; set; }
    public string V043 { get; set; }
    public string V044 { get; set; }
    public string V045 { get; set; }
    public string V046 { get; set; }
    public string V047 { get; set; }
    public string V048 { get; set; }
    public string V049 { get; set; }
    public string V050 { get; set; }


    public string V051 { get; set; }
    public string V052 { get; set; }
    public string V053 { get; set; }
    public string V054 { get; set; }
    public string V055 { get; set; }
    public string V056 { get; set; }
    public string V057 { get; set; }
    public string V058 { get; set; }
    public string V059 { get; set; }
    public string V060 { get; set; }
    public string V061 { get; set; }
    public string V062 { get; set; }
    public string V063 { get; set; }
    public string V064 { get; set; }
    public string V065 { get; set; }
    public string V066 { get; set; }
    public string V067 { get; set; }
    public string V068 { get; set; }
    public string V069 { get; set; }
    public string V070 { get; set; }
    public string V071 { get; set; }
    public string V072 { get; set; }
    public string V073 { get; set; }
    public string V074 { get; set; }
    public string V075 { get; set; }
    public string V076 { get; set; }
    public string V077 { get; set; }
    public string V078 { get; set; }
    public string V079 { get; set; }
    public string V080 { get; set; }
    public string V081 { get; set; }
    public string V082 { get; set; }
    public string V083 { get; set; }
    public string V084 { get; set; }
    public string V085 { get; set; }
    public string V086 { get; set; }
    public string V087 { get; set; }
    public string V088 { get; set; }
    public string V089 { get; set; }
    public string V090 { get; set; }
    public string V091 { get; set; }
    public string V092 { get; set; }
    public string V093 { get; set; }
    public string V094 { get; set; }
    public string V095 { get; set; }
    public string V096 { get; set; }
    public string V097 { get; set; }
    public string V098 { get; set; }
    public string V099 { get; set; }
    public string V100 { get; set; }


    public string V101 { get; set; }
    public string V102 { get; set; }
    public string V103 { get; set; }
    public string V104 { get; set; }
    public string V105 { get; set; }
    public string V106 { get; set; }
    public string V107 { get; set; }
    public string V108 { get; set; }
    public string V109 { get; set; }
    public string V110 { get; set; }
    public string V111 { get; set; }
    public string V112 { get; set; }
    public string V113 { get; set; }
    public string V114 { get; set; }
    public string V115 { get; set; }
    public string V116 { get; set; }
    public string V117 { get; set; }
    public string V118 { get; set; }
    public string V119 { get; set; }
    public string V120 { get; set; }
    public string V121 { get; set; }
    public string V122 { get; set; }
    public string V123 { get; set; }
    public string V124 { get; set; }
    public string V125 { get; set; }
    public string V126 { get; set; }
    public string V127 { get; set; }
    public string V128 { get; set; }
    public string V129 { get; set; }
    public string V130 { get; set; }
    public string V131 { get; set; }
    public string V132 { get; set; }
    public string V133 { get; set; }
    public string V134 { get; set; }
    public string V135 { get; set; }
    public string V136 { get; set; }
    public string V137 { get; set; }
    public string V138 { get; set; }
    public string V139 { get; set; }
    public string V140 { get; set; }
    public string V141 { get; set; }
    public string V142 { get; set; }
    public string V143 { get; set; }
    public string V144 { get; set; }
    public string V145 { get; set; }
    public string V146 { get; set; }
    public string V147 { get; set; }
    public string V148 { get; set; }
    public string V149 { get; set; }
    public string V150 { get; set; }


    public string V151 { get; set; }
    public string V152 { get; set; }
    public string V153 { get; set; }
    public string V154 { get; set; }
    public string V155 { get; set; }
    public string V156 { get; set; }
    public string V157 { get; set; }
    public string V158 { get; set; }
    public string V159 { get; set; }
    public string V160 { get; set; }
    public string V161 { get; set; }
    public string V162 { get; set; }
    public string V163 { get; set; }
    public string V164 { get; set; }
    public string V165 { get; set; }
    public string V166 { get; set; }
    public string V167 { get; set; }
    public string V168 { get; set; }
    public string V169 { get; set; }
    public string V170 { get; set; }
    public string V171 { get; set; }
    public string V172 { get; set; }
    public string V173 { get; set; }
    public string V174 { get; set; }
    public string V175 { get; set; }
    public string V176 { get; set; }
    public string V177 { get; set; }
    public string V178 { get; set; }
    public string V179 { get; set; }
    public string V180 { get; set; }
    public string V181 { get; set; }
    public string V182 { get; set; }
    public string V183 { get; set; }
    public string V184 { get; set; }
    public string V185 { get; set; }
    public string V186 { get; set; }
    public string V187 { get; set; }
    public string V188 { get; set; }
    public string V189 { get; set; }
    public string V190 { get; set; }
    public string V191 { get; set; }
    public string V192 { get; set; }
    public string V193 { get; set; }
    public string V194 { get; set; }
    public string V195 { get; set; }
    public string V196 { get; set; }
    public string V197 { get; set; }
    public string V198 { get; set; }
    public string V199 { get; set; }
    public string V200 { get; set; }


    public string V201 { get; set; }
    public string V202 { get; set; }
    public string V203 { get; set; }
    public string V204 { get; set; }
    public string V205 { get; set; }
    public string V206 { get; set; }
    public string V207 { get; set; }
    public string V208 { get; set; }
    public string V209 { get; set; }
    public string V210 { get; set; }
    public string V211 { get; set; }
    public string V212 { get; set; }
    public string V213 { get; set; }
    public string V214 { get; set; }
    public string V215 { get; set; }
    public string V216 { get; set; }
    public string V217 { get; set; }
    public string V218 { get; set; }
    public string V219 { get; set; }
    public string V220 { get; set; }
    public string V221 { get; set; }
    public string V222 { get; set; }
    public string V223 { get; set; }
    public string V224 { get; set; }
    public string V225 { get; set; }
    public string V226 { get; set; }
    public string V227 { get; set; }
    public string V228 { get; set; }
    public string V229 { get; set; }
    public string V230 { get; set; }
    public string V231 { get; set; }
    public string V232 { get; set; }
    public string V233 { get; set; }
    public string V234 { get; set; }
    public string V235 { get; set; }
    public string V236 { get; set; }
    public string V237 { get; set; }
    public string V238 { get; set; }
    public string V239 { get; set; }
    public string V240 { get; set; }
    public string V241 { get; set; }
    public string V242 { get; set; }
    public string V243 { get; set; }
    public string V244 { get; set; }
    public string V245 { get; set; }
    public string V246 { get; set; }
    public string V247 { get; set; }
    public string V248 { get; set; }
    public string V249 { get; set; }
    public string V250 { get; set; }


    public string V251 { get; set; }
    public string V252 { get; set; }
    public string V253 { get; set; }
    public string V254 { get; set; }
    public string V255 { get; set; }
    public string V256 { get; set; }
    public string V257 { get; set; }
    public string V258 { get; set; }
    public string V259 { get; set; }
    public string V260 { get; set; }
    public string V261 { get; set; }
    public string V262 { get; set; }
    public string V263 { get; set; }
    public string V264 { get; set; }
    public string V265 { get; set; }
    public string V266 { get; set; }
    public string V267 { get; set; }
    public string V268 { get; set; }
    public string V269 { get; set; }
    public string V270 { get; set; }
    public string V271 { get; set; }
    public string V272 { get; set; }
    public string V273 { get; set; }
    public string V274 { get; set; }
    public string V275 { get; set; }
    public string V276 { get; set; }
    public string V277 { get; set; }
    public string V278 { get; set; }
    public string V279 { get; set; }
    public string V280 { get; set; }
    public string V281 { get; set; }
    public string V282 { get; set; }
    public string V283 { get; set; }
    public string V284 { get; set; }
    public string V285 { get; set; }
    public string V286 { get; set; }
    public string V287 { get; set; }
    public string V288 { get; set; }
    public string V289 { get; set; }
    public string V290 { get; set; }
    public string V291 { get; set; }
    public string V292 { get; set; }
    public string V293 { get; set; }
    public string V294 { get; set; }
    public string V295 { get; set; }
    public string V296 { get; set; }
    public string V297 { get; set; }
    public string V298 { get; set; }
    public string V299 { get; set; }
    public string V300 { get; set; }

    public string V301 { get; set; }
    public string V302 { get; set; }
    public string V303 { get; set; }
    public string V304 { get; set; }
    public string V305 { get; set; }
    public string V306 { get; set; }
    public string V307 { get; set; }
    public string V308 { get; set; }
    public string V309 { get; set; }
    public string V310 { get; set; }
    public string V311 { get; set; }
    public string V312 { get; set; }
    public string V313 { get; set; }
    public string V314 { get; set; }
    public string V315 { get; set; }
    public string V316 { get; set; }
    public string V317 { get; set; }
    public string V318 { get; set; }
    public string V319 { get; set; }
    public string V320 { get; set; }
    public string V321 { get; set; }
    public string V322 { get; set; }
    public string V323 { get; set; }
    public string V324 { get; set; }
    public string V325 { get; set; }
    public string V326 { get; set; }
    public string V327 { get; set; }
    public string V328 { get; set; }
    public string V329 { get; set; }
    public string V330 { get; set; }
    public string V331 { get; set; }
    public string V332 { get; set; }
    public string V333 { get; set; }
    public string V334 { get; set; }
    public string V335 { get; set; }
    public string V336 { get; set; }
    public string V337 { get; set; }
    public string V338 { get; set; }
    public string V339 { get; set; }
    public string V340 { get; set; }
    public string V341 { get; set; }
    public string V342 { get; set; }
    public string V343 { get; set; }
    public string V344 { get; set; }
    public string V345 { get; set; }
    public string V346 { get; set; }
    public string V347 { get; set; }
    public string V348 { get; set; }
    public string V349 { get; set; }
    public string V350 { get; set; }


    public string V351 { get; set; }
    public string V352 { get; set; }
    public string V353 { get; set; }
    public string V354 { get; set; }
    public string V355 { get; set; }
    public string V356 { get; set; }
    public string V357 { get; set; }
    public string V358 { get; set; }
    public string V359 { get; set; }
    public string V360 { get; set; }
    public string V361 { get; set; }
    public string V362 { get; set; }
    public string V363 { get; set; }
    public string V364 { get; set; }
    public string V365 { get; set; }
    public string V366 { get; set; }
    public string V367 { get; set; }
    public string V368 { get; set; }
    public string V369 { get; set; }
    public string V370 { get; set; }
    public string V371 { get; set; }
    public string V372 { get; set; }
    public string V373 { get; set; }
    public string V374 { get; set; }
    public string V375 { get; set; }
    public string V376 { get; set; }
    public string V377 { get; set; }
    public string V378 { get; set; }
    public string V379 { get; set; }
    public string V380 { get; set; }
    public string V381 { get; set; }
    public string V382 { get; set; }
    public string V383 { get; set; }
    public string V384 { get; set; }
    public string V385 { get; set; }
    public string V386 { get; set; }
    public string V387 { get; set; }
    public string V388 { get; set; }
    public string V389 { get; set; }
    public string V390 { get; set; }
    public string V391 { get; set; }
    public string V392 { get; set; }
    public string V393 { get; set; }
    public string V394 { get; set; }
    public string V395 { get; set; }
    public string V396 { get; set; }
    public string V397 { get; set; }
    public string V398 { get; set; }
    public string V399 { get; set; }
    public string V400 { get; set; }


    public string V401 { get; set; }
    public string V402 { get; set; }
    public string V403 { get; set; }
    public string V404 { get; set; }
    public string V405 { get; set; }
    public string V406 { get; set; }
    public string V407 { get; set; }
    public string V408 { get; set; }
    public string V409 { get; set; }
    public string V410 { get; set; }
    public string V411 { get; set; }
    public string V412 { get; set; }
    public string V413 { get; set; }
    public string V414 { get; set; }
    public string V415 { get; set; }
    public string V416 { get; set; }
    public string V417 { get; set; }
    public string V418 { get; set; }
    public string V419 { get; set; }
    public string V420 { get; set; }
    public string V421 { get; set; }
    public string V422 { get; set; }
    public string V423 { get; set; }
    public string V424 { get; set; }
    public string V425 { get; set; }
    public string V426 { get; set; }
    public string V427 { get; set; }
    public string V428 { get; set; }
    public string V429 { get; set; }
    public string V430 { get; set; }
    public string V431 { get; set; }
    public string V432 { get; set; }
    public string V433 { get; set; }
    public string V434 { get; set; }
    public string V435 { get; set; }
    public string V436 { get; set; }
    public string V437 { get; set; }
    public string V438 { get; set; }
    public string V439 { get; set; }
    public string V440 { get; set; }
    public string V441 { get; set; }
    public string V442 { get; set; }
    public string V443 { get; set; }
    public string V444 { get; set; }
    public string V445 { get; set; }
    public string V446 { get; set; }
    public string V447 { get; set; }
    public string V448 { get; set; }
    public string V449 { get; set; }
    public string V450 { get; set; }


    public string V451 { get; set; }
    public string V452 { get; set; }
    public string V453 { get; set; }
    public string V454 { get; set; }
    public string V455 { get; set; }
    public string V456 { get; set; }
    public string V457 { get; set; }
    public string V458 { get; set; }
    public string V459 { get; set; }
    public string V460 { get; set; }
    public string V461 { get; set; }
    public string V462 { get; set; }
    public string V463 { get; set; }
    public string V464 { get; set; }
    public string V465 { get; set; }
    public string V466 { get; set; }
    public string V467 { get; set; }
    public string V468 { get; set; }
    public string V469 { get; set; }
    public string V470 { get; set; }
    public string V471 { get; set; }
    public string V472 { get; set; }
    public string V473 { get; set; }
    public string V474 { get; set; }
    public string V475 { get; set; }
    public string V476 { get; set; }
    public string V477 { get; set; }
    public string V478 { get; set; }
    public string V479 { get; set; }
    public string V480 { get; set; }
    public string V481 { get; set; }
    public string V482 { get; set; }
    public string V483 { get; set; }
    public string V484 { get; set; }
    public string V485 { get; set; }
    public string V486 { get; set; }
    public string V487 { get; set; }
    public string V488 { get; set; }
    public string V489 { get; set; }
    public string V490 { get; set; }
    public string V491 { get; set; }
    public string V492 { get; set; }
    public string V493 { get; set; }
    public string V494 { get; set; }
    public string V495 { get; set; }
    public string V496 { get; set; }
    public string V497 { get; set; }
    public string V498 { get; set; }
    public string V499 { get; set; }
    public string V500 { get; set; }



    public Record()
    {
       
    }

    public string ChangeReason
    {
        get { return _strChangeReason; }
        set { _strChangeReason = value; }
    }


    public int? LastUpdatedUserID
    {
        get { return _iLastUpdatedUserID; }
        set { _iLastUpdatedUserID = value; }
    }

    public int? RecordID
    {
        get { return _iRecordID; }
        set { _iRecordID = value; }
    }
    public int? TableID
    {
        get { return _iTableID; }
        set { _iTableID = value; }
    }
    public DateTime? DateTimeRecorded
    {
        get { return _dtDateTimeRecorded; }
        set { _dtDateTimeRecorded = value; }
    }
    public string Notes
    {
        get { return _strNotes; }
        set { _strNotes = value; }
    }

    public int? EnteredBy
    {
        get { return _iEnteredBy; }
        set { _iEnteredBy = value; }
    }

    public bool? IsActive
    {
        get { return _bIsActive; }
        set { _bIsActive = value; }
    }
    public DateTime? DateAdded
    {
        get { return _dtDateAdded; }
        set { _dtDateAdded = value; }
    }
    public DateTime? DateUpdated
    {
        get { return _dtDateUpdated; }
        set { _dtDateUpdated = value; }
    }
   
    public string ValidationResults
    {
        get { return _strValidationResults; }
        set { _strValidationResults = value; }
    }

    public string WarningResults
    {
        get { return _strWarningResults; }
        set { _strWarningResults = value; }
    }

    public int? TempRecordID
    {
        get { return _iTempRecordID; }
        set { _iTempRecordID = value; }
    }


    public string TableName
    {
        get { return _strTableName; }
        set { _strTableName = value; }
    }
    public string EnteredByUserName
    {
        get { return _strEnteredByUserName; }
        set { _strEnteredByUserName = value; }
    }





}



[Serializable]
public class Column
{
    private int? _iColumnID;
    private int? _iTableID;
    private string _strSystemName;
    private int? _iDisplayOrder;
    private string _strDisplayTextSummary;
    private string _strDisplayTextDetail;
    //private string _strImportHeaderName;
    //private string _strNameOnExport;
    private int? _iGraphTypeID;
    private string _strValidationOnWarning;
    private string _strValidationOnEntry;
    private bool? _bIsStandard;
    private DateTime? _dateAdded;
    private DateTime? _dateUpdated;
    private string _strDisplayName;
    private string _strConstant;
    private string _strCalculation;
    //private int? _iSensorID;
 
    private bool? _bShowTotal;
    private bool? _bIgnoreSymbols;
    private string _strNotes;
    private bool? _bIsRound;
    private int? _iRoundNumber;
    private bool? _bCheckUnlikelyValue;

    private string _strGraphTypeName;
    private string _strTableName;

    private string _strGraphLabel;
    private int? _iLastUpdatedUserID;

    private string _strDropdownValues;
    //private bool? _bIsMandatory;

    private string _strAlignment;
    private int? _iNumberType;
 

    private string _strDefaultValue;

    private int? _iAvgColumnID;

    private int? _iAvgNumberOfRecords;

    private string _strMobileName;

    private double? _dShowGraphExceedance;
    private double? _dShowGraphWarning;

    private int? _iFlatLineNumber;

    private double? _dMaxValueAt;

    private int? _iDefaultGraphDefinitionID;

    public string SummaryCellBackColor { set; get; }

    public int? FormVerticalPosition { set; get; }
    public int? FormHorizontalPosition { set; get; }
    public int? ParentColumnID { set; get; }

    public string TextType { set; get; }
    public string RegEx { set; get; }
   
    public string DateCalculationType { set; get; }

    public int? OnlyForAdmin { set; get; }

    public bool IsSystemColumn { set; get; }
    public int? LinkedParentColumnID { set; get; }
    //public int? DataRetrieverID { set; get; }
    public bool? VerticalList { set; get; }
    public bool? SummarySearch { set; get; }
    public bool? QuickAddLink { set; get; }
    public int? TableTabID { set; get; }

    //public int? HideColumnID { set; get; }
    //public string HideColumnValue { set; get; }
    //public string HideOperator { set; get; }

    public bool? CalculationIsActive { set; get; }

    public string ViewName { set; get; }
    public string SPDefaultValue { set; get; }

    public string DefaultType { set; get; }
    public int? DefaultColumnID { set; get; }
    public string ShowViewLink { set; get; }

    public int? FilterParentColumnID { set; get; }
    public int? FilterOtherColumnID { set; get; }
    public string FilterValue { set; get; }
    public string FilterOperator { set; get; }
    public int? MapPinHoverColumnID { set; get; }

    public int? CompareColumnID { set; get; }
    public string CompareOperator { set; get; }
    public string MapPopup { set; get; }

    public int? TrafficLightColumnID { set; get; }
    public string TrafficLightValues { set; get; }

    public int? DefaultRelatedTableID { set; get; }
    public bool? DefaultUpdateValues { set; get; }

    public bool? ValidationCanIgnore { set; get; }
    public bool? ImageOnSummary { set; get; }

    public bool? AllowCopy { set; get; }
    public string ValidationOnExceedance { set; get; }
    public bool? ColourCells { set; get; }

    public string ButtonInfo { set; get; }

    public bool? IsReadOnly { set; get; }
    public string ControlValueChangeService { set; get; }

    public string SystemName_2nd { set; get; } //not in DB
    public string FileColumnName_Import { set; get; }//not in DB

    private string _strImportance="";
    public Column()
    {
    }
    public Column(int? p_iColumnID, int? p_iTableID, string p_strSystemName,
        int? p_iDisplayOrder, string p_strDisplayTextSummary,string p_strDisplayTextDetail,
       int? p_iGraphTypeID,
        string p_strValidationOnWarning, string p_strValidationOnEntry,
        DateTime? p_dateAdded, DateTime? p_dateUpdated,
        string p_strGraphTypeName, string p_strTableName, bool? p_bIsStandard,
        string p_strDisplayName, string p_strNotes,
        bool? p_bIsRound, int? p_iRoundNumber, bool? p__bCheckUnlikelyValue,
        string p_strGraphLabel, string p_strDropdownValues,
        string p_strImportance)
    {
        _iColumnID = p_iColumnID;
        _iTableID = p_iTableID;
        _iDisplayOrder = p_iDisplayOrder;
        _strSystemName = p_strSystemName;
        _strDisplayTextSummary = p_strDisplayTextSummary;
        _strDisplayTextDetail = p_strDisplayTextDetail;
        _iGraphTypeID = p_iGraphTypeID;
        _strValidationOnWarning = p_strValidationOnWarning;
        _strValidationOnEntry = p_strValidationOnEntry;
        _dateAdded = p_dateAdded;
        _dateUpdated = p_dateUpdated;
        _strGraphTypeName = p_strGraphTypeName;
        _strTableName = p_strTableName;
        _bIsStandard = p_bIsStandard;
        _strDisplayName = p_strDisplayName;
        _strNotes = p_strNotes;
        _bIsRound = p_bIsRound;
        _iRoundNumber = p_iRoundNumber;
        _bCheckUnlikelyValue = p__bCheckUnlikelyValue;
        _strGraphLabel = p_strGraphLabel;
        _strDropdownValues = p_strDropdownValues;
        _strImportance = p_strImportance;
    }


    public double? MaxValueAt
    {
        get { return _dMaxValueAt; }
        set { _dMaxValueAt = value; }
    }

    public int? FlatLineNumber
    {
        get { return _iFlatLineNumber; }
        set { _iFlatLineNumber = value; }
    }


    public double? ShowGraphExceedance
    {
        get { return _dShowGraphExceedance; }
        set { _dShowGraphExceedance = value; }
    }

    public double? ShowGraphWarning
    {
        get { return _dShowGraphWarning; }
        set { _dShowGraphWarning = value; }
    }

    public int? DefaultGraphDefinitionID
    {
        get { return _iDefaultGraphDefinitionID; }
        set { _iDefaultGraphDefinitionID = value; }
    }

    //public bool? IsDateSingleColumn
    //{
    //    get { return _bIsDateSingleColumn; }
    //    set { _bIsDateSingleColumn = value; }
    //}

    public string MobileName
    {
        get { return _strMobileName; }
        set { _strMobileName = value; }
    }

    public string Importance
    {
        get { return _strImportance; }
        set { _strImportance = value; }
    }


    public int? AvgNumberOfRecords
    {
        get { return _iAvgNumberOfRecords; }
        set { _iAvgNumberOfRecords = value; }
    }

    public int? AvgColumnID
    {
        get { return _iAvgColumnID; }
        set { _iAvgColumnID = value; }
    }

    public string DefaultValue
    {
        get { return _strDefaultValue; }
        set { _strDefaultValue = value; }
    }

    public int? NumberType
    {
        get { return _iNumberType; }
        set { _iNumberType = value; }
    }

  

    public string  Alignment
    {
        get { return _strAlignment; }
        set { _strAlignment = value; }
    }

    //public bool? IsMandatory
    //{
    //    get { return _bIsMandatory; }
    //    set { _bIsMandatory = value; }
    //}

    public string DropdownValues
    {
        get { return _strDropdownValues; }
        set { _strDropdownValues = value; }
    }

    public int? LastUpdatedUserID
    {
        get { return _iLastUpdatedUserID; }
        set { _iLastUpdatedUserID = value; }
    }

    public bool? CheckUnlikelyValue
    {
        get { return _bCheckUnlikelyValue; }
        set { _bCheckUnlikelyValue = value; }
    }


    public string GraphLabel
    {
        get { return _strGraphLabel; }
        set { _strGraphLabel = value; }
    }

    public string Notes
    {
        get { return _strNotes; }
        set { _strNotes = value; }
    }


    public bool? IsRound
    {
        get { return _bIsRound; }
        set { _bIsRound = value; }
    }



    public int? RoundNumber
    {
        get { return _iRoundNumber; }
        set { _iRoundNumber = value; }
    }

    //public int? PositionOnImport
    //{
    //    get { return _iPositionOnImport; }
    //    set { _iPositionOnImport = value; }
    //}


    public int? ColumnID
    {
        get { return _iColumnID; }
        set { _iColumnID = value; }
    }

    public int? TableID
    {
        get { return _iTableID; }
        set { _iTableID = value; }
    }
    public string DisplayName
    {
        get { return _strDisplayName; }
        set { _strDisplayName = value; }
    }
    public string SystemName
    {
        get { return _strSystemName; }
        set { _strSystemName = value; }
    }
    public int? DisplayOrder
    {
        get { return _iDisplayOrder; }
        set { _iDisplayOrder = value; }
    }
    public string DisplayTextSummary
    {
        get { return _strDisplayTextSummary; }
        set { _strDisplayTextSummary = value; }
    }
    public string DisplayTextDetail
    {
        get { return _strDisplayTextDetail; }
        set { _strDisplayTextDetail = value; }
    }
    //public string ImportHeaderName
    //{
    //    get { return _strImportHeaderName; }
    //    set { _strImportHeaderName = value; }
    //}
    //public string NameOnExport
    //{
    //    get { return _strNameOnExport; }
    //    set { _strNameOnExport = value; }
    //}
    public int? GraphTypeID
    {
        get { return _iGraphTypeID; }
        set { _iGraphTypeID = value; }
    }
    public string ValidationOnWarning
    {
        get { return _strValidationOnWarning; }
        set { _strValidationOnWarning = value; }
    }
    public string ValidationOnEntry
    {
        get { return _strValidationOnEntry; }
        set { _strValidationOnEntry = value; }
    }

    public bool? IsStandard
    {
        get { return _bIsStandard; }
        set { _bIsStandard = value; }

    }

    public DateTime? DateAdded
    {
        get { return _dateAdded; }
        set { _dateAdded = value; }
    }
    public DateTime? DateUpdated
    {
        get { return _dateUpdated; }
        set { _dateUpdated = value; }
    }

    public string GraphTypeName
    {
        get { return _strGraphTypeName; }
        set { _strGraphTypeName = value; }
    }

    public string TableName
    {
        get { return _strTableName; }
        set { _strTableName = value; }
    }

    public string Constant
    {
        get { return _strConstant; }
        set { _strConstant = value; }
    }

    public string Calculation
    {
        get { return _strCalculation; }
        set { _strCalculation = value; }
    }

    //public int? SensorID
    //{
    //    get { return _iSensorID; }
    //    set { _iSensorID = value; }
    //}

   
    public bool? ShowTotal
    {
        get { return _bShowTotal; }
        set { _bShowTotal = value; }
    }

    public bool? IgnoreSymbols
    {
        get { return _bIgnoreSymbols; }
        set { _bIgnoreSymbols = value; }
    }


    public int? TextWidth { get; set; }
    public int? TextHeight { get; set; }
    public string ColumnType { get; set; }
    public string DropDownType { get; set; }
    public int? TableTableID { get; set; }
    public string DisplayColumn { get; set; }
    public bool? DisplayRight { get; set; }   

}



[Serializable]
public class GraphType
{
    private int? _iGraphTypeID;
    private string _strGraphTypeName;


    public GraphType(int? p_iGraphTypeID, string p_strGraphTypeName)
    {
        _iGraphTypeID = p_iGraphTypeID;
        _strGraphTypeName = p_strGraphTypeName;
       
    }

    public int? GraphTypeID
    {
        get { return _iGraphTypeID; }
        set { _iGraphTypeID = value; }
    }

    public string GraphTypeName
    {
        get { return _strGraphTypeName; }
        set { _strGraphTypeName = value; }
    }

}



[Serializable]
public class SearchCriteria
{
    private int? _iSearchCriteriaID;
    private string _strSearchText;


    public SearchCriteria(int? p_iSearchCriteriaID, string p_strSearchText)
    {
        _iSearchCriteriaID = p_iSearchCriteriaID;
        _strSearchText = p_strSearchText;


    }

    public int? SearchCriteriaID
    {
        get { return _iSearchCriteriaID; }
        set { _iSearchCriteriaID = value; }
    }

    public string SearchText
    {
        get { return _strSearchText; }
        set { _strSearchText = value; }
    }   


}




[Serializable]
public class Batch
{
    private int? _iBatchID;
     private int? _iTableID;
    private string _strBatchDescription;
    private string _strUploadedFileName;
    private DateTime? _dateAdded;
    private Guid? _guidUniqueName;
    private int? _iUserIDUploaded;
    private int? _iAccountID;
    private bool? _bIsImported;
    //private bool? _bIsImportPositional = false;

    //public bool? AllowDataUpdate { get; set; }

    public int? ImportTemplateID { get; set; }
    public Batch(int? p_iBatchID, int? p_iTableID, string p_strBatchDescription,
        string p_strUploadedFileName, DateTime? p_dateAdded, Guid? p_guidUniqueName,
        int? p_iUserIDUploaded, int? p_iAccountID)
    {
        _iBatchID = p_iBatchID;
        _iTableID = p_iTableID;
        _strBatchDescription = p_strBatchDescription;
        _strUploadedFileName = p_strUploadedFileName;
        _dateAdded = p_dateAdded;
        _guidUniqueName = p_guidUniqueName;
        _iUserIDUploaded = p_iUserIDUploaded;
        _iAccountID = p_iAccountID;
        

    }
    //public bool? IsImportPositional
    //{
    //    get { return _bIsImportPositional; }
    //    set { _bIsImportPositional = value; }
    //}

    public int? BatchID
    {
        get { return _iBatchID; }
        set { _iBatchID = value; }
    }
    public int? TableID
    {
        get { return _iTableID; }
        set { _iTableID = value; }
    }
    public string BatchDescription
    {
        get { return _strBatchDescription; }
        set { _strBatchDescription = value; }
    }
    public string UploadedFileName
    {
        get { return _strUploadedFileName; }
        set { _strUploadedFileName = value; }
    }
 
    public DateTime? DateAdded
    {
        get { return _dateAdded; }
        set { _dateAdded = value; }
    }

    public Guid? UniqueName
    {
        get { return _guidUniqueName; }
        set { _guidUniqueName = value; }
    }

    public int? UserIDUploaded
    {
        get { return _iUserIDUploaded; }
        set { _iUserIDUploaded = value; }
    }

    public int? AccountID
    {
        get { return _iAccountID; }
        set { _iAccountID = value; }
    }
    public bool? IsImported
    {
        get { return _bIsImported; }
        set { _bIsImported = value; }
    }

}



[Serializable]
public class TempRecord
{
    private int? _iTempRecordID;
    private int? _iAccountID;
    private int? _iBatchID;
    private int? _iTableID;
    private DateTime? _dtDateTimeRecorded;
    private string _strNotes;
    private bool? _bIsActive;
    private bool? _bIsValidated;
    private string _strRejectReason;
    private string _strValidationResults;
  
    private string _strWarningReason;

    private string _strTableName;



    public string V001 { get; set; }
    public string V002 { get; set; }
    public string V003 { get; set; }
    public string V004 { get; set; }
    public string V005 { get; set; }
    public string V006 { get; set; }
    public string V007 { get; set; }
    public string V008 { get; set; }
    public string V009 { get; set; }
    public string V010 { get; set; }
    public string V011 { get; set; }
    public string V012 { get; set; }
    public string V013 { get; set; }
    public string V014 { get; set; }
    public string V015 { get; set; }
    public string V016 { get; set; }
    public string V017 { get; set; }
    public string V018 { get; set; }
    public string V019 { get; set; }
    public string V020 { get; set; }
    public string V021 { get; set; }
    public string V022 { get; set; }
    public string V023 { get; set; }
    public string V024 { get; set; }
    public string V025 { get; set; }
    public string V026 { get; set; }
    public string V027 { get; set; }
    public string V028 { get; set; }
    public string V029 { get; set; }
    public string V030 { get; set; }
    public string V031 { get; set; }
    public string V032 { get; set; }
    public string V033 { get; set; }
    public string V034 { get; set; }
    public string V035 { get; set; }
    public string V036 { get; set; }
    public string V037 { get; set; }
    public string V038 { get; set; }
    public string V039 { get; set; }
    public string V040 { get; set; }
    public string V041 { get; set; }
    public string V042 { get; set; }
    public string V043 { get; set; }
    public string V044 { get; set; }
    public string V045 { get; set; }
    public string V046 { get; set; }
    public string V047 { get; set; }
    public string V048 { get; set; }
    public string V049 { get; set; }
    public string V050 { get; set; }


    public string V051 { get; set; }
    public string V052 { get; set; }
    public string V053 { get; set; }
    public string V054 { get; set; }
    public string V055 { get; set; }
    public string V056 { get; set; }
    public string V057 { get; set; }
    public string V058 { get; set; }
    public string V059 { get; set; }
    public string V060 { get; set; }
    public string V061 { get; set; }
    public string V062 { get; set; }
    public string V063 { get; set; }
    public string V064 { get; set; }
    public string V065 { get; set; }
    public string V066 { get; set; }
    public string V067 { get; set; }
    public string V068 { get; set; }
    public string V069 { get; set; }
    public string V070 { get; set; }
    public string V071 { get; set; }
    public string V072 { get; set; }
    public string V073 { get; set; }
    public string V074 { get; set; }
    public string V075 { get; set; }
    public string V076 { get; set; }
    public string V077 { get; set; }
    public string V078 { get; set; }
    public string V079 { get; set; }
    public string V080 { get; set; }
    public string V081 { get; set; }
    public string V082 { get; set; }
    public string V083 { get; set; }
    public string V084 { get; set; }
    public string V085 { get; set; }
    public string V086 { get; set; }
    public string V087 { get; set; }
    public string V088 { get; set; }
    public string V089 { get; set; }
    public string V090 { get; set; }
    public string V091 { get; set; }
    public string V092 { get; set; }
    public string V093 { get; set; }
    public string V094 { get; set; }
    public string V095 { get; set; }
    public string V096 { get; set; }
    public string V097 { get; set; }
    public string V098 { get; set; }
    public string V099 { get; set; }
    public string V100 { get; set; }


    public string V101 { get; set; }
    public string V102 { get; set; }
    public string V103 { get; set; }
    public string V104 { get; set; }
    public string V105 { get; set; }
    public string V106 { get; set; }
    public string V107 { get; set; }
    public string V108 { get; set; }
    public string V109 { get; set; }
    public string V110 { get; set; }
    public string V111 { get; set; }
    public string V112 { get; set; }
    public string V113 { get; set; }
    public string V114 { get; set; }
    public string V115 { get; set; }
    public string V116 { get; set; }
    public string V117 { get; set; }
    public string V118 { get; set; }
    public string V119 { get; set; }
    public string V120 { get; set; }
    public string V121 { get; set; }
    public string V122 { get; set; }
    public string V123 { get; set; }
    public string V124 { get; set; }
    public string V125 { get; set; }
    public string V126 { get; set; }
    public string V127 { get; set; }
    public string V128 { get; set; }
    public string V129 { get; set; }
    public string V130 { get; set; }
    public string V131 { get; set; }
    public string V132 { get; set; }
    public string V133 { get; set; }
    public string V134 { get; set; }
    public string V135 { get; set; }
    public string V136 { get; set; }
    public string V137 { get; set; }
    public string V138 { get; set; }
    public string V139 { get; set; }
    public string V140 { get; set; }
    public string V141 { get; set; }
    public string V142 { get; set; }
    public string V143 { get; set; }
    public string V144 { get; set; }
    public string V145 { get; set; }
    public string V146 { get; set; }
    public string V147 { get; set; }
    public string V148 { get; set; }
    public string V149 { get; set; }
    public string V150 { get; set; }


    public string V151 { get; set; }
    public string V152 { get; set; }
    public string V153 { get; set; }
    public string V154 { get; set; }
    public string V155 { get; set; }
    public string V156 { get; set; }
    public string V157 { get; set; }
    public string V158 { get; set; }
    public string V159 { get; set; }
    public string V160 { get; set; }
    public string V161 { get; set; }
    public string V162 { get; set; }
    public string V163 { get; set; }
    public string V164 { get; set; }
    public string V165 { get; set; }
    public string V166 { get; set; }
    public string V167 { get; set; }
    public string V168 { get; set; }
    public string V169 { get; set; }
    public string V170 { get; set; }
    public string V171 { get; set; }
    public string V172 { get; set; }
    public string V173 { get; set; }
    public string V174 { get; set; }
    public string V175 { get; set; }
    public string V176 { get; set; }
    public string V177 { get; set; }
    public string V178 { get; set; }
    public string V179 { get; set; }
    public string V180 { get; set; }
    public string V181 { get; set; }
    public string V182 { get; set; }
    public string V183 { get; set; }
    public string V184 { get; set; }
    public string V185 { get; set; }
    public string V186 { get; set; }
    public string V187 { get; set; }
    public string V188 { get; set; }
    public string V189 { get; set; }
    public string V190 { get; set; }
    public string V191 { get; set; }
    public string V192 { get; set; }
    public string V193 { get; set; }
    public string V194 { get; set; }
    public string V195 { get; set; }
    public string V196 { get; set; }
    public string V197 { get; set; }
    public string V198 { get; set; }
    public string V199 { get; set; }
    public string V200 { get; set; }


    public string V201 { get; set; }
    public string V202 { get; set; }
    public string V203 { get; set; }
    public string V204 { get; set; }
    public string V205 { get; set; }
    public string V206 { get; set; }
    public string V207 { get; set; }
    public string V208 { get; set; }
    public string V209 { get; set; }
    public string V210 { get; set; }
    public string V211 { get; set; }
    public string V212 { get; set; }
    public string V213 { get; set; }
    public string V214 { get; set; }
    public string V215 { get; set; }
    public string V216 { get; set; }
    public string V217 { get; set; }
    public string V218 { get; set; }
    public string V219 { get; set; }
    public string V220 { get; set; }
    public string V221 { get; set; }
    public string V222 { get; set; }
    public string V223 { get; set; }
    public string V224 { get; set; }
    public string V225 { get; set; }
    public string V226 { get; set; }
    public string V227 { get; set; }
    public string V228 { get; set; }
    public string V229 { get; set; }
    public string V230 { get; set; }
    public string V231 { get; set; }
    public string V232 { get; set; }
    public string V233 { get; set; }
    public string V234 { get; set; }
    public string V235 { get; set; }
    public string V236 { get; set; }
    public string V237 { get; set; }
    public string V238 { get; set; }
    public string V239 { get; set; }
    public string V240 { get; set; }
    public string V241 { get; set; }
    public string V242 { get; set; }
    public string V243 { get; set; }
    public string V244 { get; set; }
    public string V245 { get; set; }
    public string V246 { get; set; }
    public string V247 { get; set; }
    public string V248 { get; set; }
    public string V249 { get; set; }
    public string V250 { get; set; }


    public string V251 { get; set; }
    public string V252 { get; set; }
    public string V253 { get; set; }
    public string V254 { get; set; }
    public string V255 { get; set; }
    public string V256 { get; set; }
    public string V257 { get; set; }
    public string V258 { get; set; }
    public string V259 { get; set; }
    public string V260 { get; set; }
    public string V261 { get; set; }
    public string V262 { get; set; }
    public string V263 { get; set; }
    public string V264 { get; set; }
    public string V265 { get; set; }
    public string V266 { get; set; }
    public string V267 { get; set; }
    public string V268 { get; set; }
    public string V269 { get; set; }
    public string V270 { get; set; }
    public string V271 { get; set; }
    public string V272 { get; set; }
    public string V273 { get; set; }
    public string V274 { get; set; }
    public string V275 { get; set; }
    public string V276 { get; set; }
    public string V277 { get; set; }
    public string V278 { get; set; }
    public string V279 { get; set; }
    public string V280 { get; set; }
    public string V281 { get; set; }
    public string V282 { get; set; }
    public string V283 { get; set; }
    public string V284 { get; set; }
    public string V285 { get; set; }
    public string V286 { get; set; }
    public string V287 { get; set; }
    public string V288 { get; set; }
    public string V289 { get; set; }
    public string V290 { get; set; }
    public string V291 { get; set; }
    public string V292 { get; set; }
    public string V293 { get; set; }
    public string V294 { get; set; }
    public string V295 { get; set; }
    public string V296 { get; set; }
    public string V297 { get; set; }
    public string V298 { get; set; }
    public string V299 { get; set; }
    public string V300 { get; set; }

    public string V301 { get; set; }
    public string V302 { get; set; }
    public string V303 { get; set; }
    public string V304 { get; set; }
    public string V305 { get; set; }
    public string V306 { get; set; }
    public string V307 { get; set; }
    public string V308 { get; set; }
    public string V309 { get; set; }
    public string V310 { get; set; }
    public string V311 { get; set; }
    public string V312 { get; set; }
    public string V313 { get; set; }
    public string V314 { get; set; }
    public string V315 { get; set; }
    public string V316 { get; set; }
    public string V317 { get; set; }
    public string V318 { get; set; }
    public string V319 { get; set; }
    public string V320 { get; set; }
    public string V321 { get; set; }
    public string V322 { get; set; }
    public string V323 { get; set; }
    public string V324 { get; set; }
    public string V325 { get; set; }
    public string V326 { get; set; }
    public string V327 { get; set; }
    public string V328 { get; set; }
    public string V329 { get; set; }
    public string V330 { get; set; }
    public string V331 { get; set; }
    public string V332 { get; set; }
    public string V333 { get; set; }
    public string V334 { get; set; }
    public string V335 { get; set; }
    public string V336 { get; set; }
    public string V337 { get; set; }
    public string V338 { get; set; }
    public string V339 { get; set; }
    public string V340 { get; set; }
    public string V341 { get; set; }
    public string V342 { get; set; }
    public string V343 { get; set; }
    public string V344 { get; set; }
    public string V345 { get; set; }
    public string V346 { get; set; }
    public string V347 { get; set; }
    public string V348 { get; set; }
    public string V349 { get; set; }
    public string V350 { get; set; }


    public string V351 { get; set; }
    public string V352 { get; set; }
    public string V353 { get; set; }
    public string V354 { get; set; }
    public string V355 { get; set; }
    public string V356 { get; set; }
    public string V357 { get; set; }
    public string V358 { get; set; }
    public string V359 { get; set; }
    public string V360 { get; set; }
    public string V361 { get; set; }
    public string V362 { get; set; }
    public string V363 { get; set; }
    public string V364 { get; set; }
    public string V365 { get; set; }
    public string V366 { get; set; }
    public string V367 { get; set; }
    public string V368 { get; set; }
    public string V369 { get; set; }
    public string V370 { get; set; }
    public string V371 { get; set; }
    public string V372 { get; set; }
    public string V373 { get; set; }
    public string V374 { get; set; }
    public string V375 { get; set; }
    public string V376 { get; set; }
    public string V377 { get; set; }
    public string V378 { get; set; }
    public string V379 { get; set; }
    public string V380 { get; set; }
    public string V381 { get; set; }
    public string V382 { get; set; }
    public string V383 { get; set; }
    public string V384 { get; set; }
    public string V385 { get; set; }
    public string V386 { get; set; }
    public string V387 { get; set; }
    public string V388 { get; set; }
    public string V389 { get; set; }
    public string V390 { get; set; }
    public string V391 { get; set; }
    public string V392 { get; set; }
    public string V393 { get; set; }
    public string V394 { get; set; }
    public string V395 { get; set; }
    public string V396 { get; set; }
    public string V397 { get; set; }
    public string V398 { get; set; }
    public string V399 { get; set; }
    public string V400 { get; set; }


    public string V401 { get; set; }
    public string V402 { get; set; }
    public string V403 { get; set; }
    public string V404 { get; set; }
    public string V405 { get; set; }
    public string V406 { get; set; }
    public string V407 { get; set; }
    public string V408 { get; set; }
    public string V409 { get; set; }
    public string V410 { get; set; }
    public string V411 { get; set; }
    public string V412 { get; set; }
    public string V413 { get; set; }
    public string V414 { get; set; }
    public string V415 { get; set; }
    public string V416 { get; set; }
    public string V417 { get; set; }
    public string V418 { get; set; }
    public string V419 { get; set; }
    public string V420 { get; set; }
    public string V421 { get; set; }
    public string V422 { get; set; }
    public string V423 { get; set; }
    public string V424 { get; set; }
    public string V425 { get; set; }
    public string V426 { get; set; }
    public string V427 { get; set; }
    public string V428 { get; set; }
    public string V429 { get; set; }
    public string V430 { get; set; }
    public string V431 { get; set; }
    public string V432 { get; set; }
    public string V433 { get; set; }
    public string V434 { get; set; }
    public string V435 { get; set; }
    public string V436 { get; set; }
    public string V437 { get; set; }
    public string V438 { get; set; }
    public string V439 { get; set; }
    public string V440 { get; set; }
    public string V441 { get; set; }
    public string V442 { get; set; }
    public string V443 { get; set; }
    public string V444 { get; set; }
    public string V445 { get; set; }
    public string V446 { get; set; }
    public string V447 { get; set; }
    public string V448 { get; set; }
    public string V449 { get; set; }
    public string V450 { get; set; }


    public string V451 { get; set; }
    public string V452 { get; set; }
    public string V453 { get; set; }
    public string V454 { get; set; }
    public string V455 { get; set; }
    public string V456 { get; set; }
    public string V457 { get; set; }
    public string V458 { get; set; }
    public string V459 { get; set; }
    public string V460 { get; set; }
    public string V461 { get; set; }
    public string V462 { get; set; }
    public string V463 { get; set; }
    public string V464 { get; set; }
    public string V465 { get; set; }
    public string V466 { get; set; }
    public string V467 { get; set; }
    public string V468 { get; set; }
    public string V469 { get; set; }
    public string V470 { get; set; }
    public string V471 { get; set; }
    public string V472 { get; set; }
    public string V473 { get; set; }
    public string V474 { get; set; }
    public string V475 { get; set; }
    public string V476 { get; set; }
    public string V477 { get; set; }
    public string V478 { get; set; }
    public string V479 { get; set; }
    public string V480 { get; set; }
    public string V481 { get; set; }
    public string V482 { get; set; }
    public string V483 { get; set; }
    public string V484 { get; set; }
    public string V485 { get; set; }
    public string V486 { get; set; }
    public string V487 { get; set; }
    public string V488 { get; set; }
    public string V489 { get; set; }
    public string V490 { get; set; }
    public string V491 { get; set; }
    public string V492 { get; set; }
    public string V493 { get; set; }
    public string V494 { get; set; }
    public string V495 { get; set; }
    public string V496 { get; set; }
    public string V497 { get; set; }
    public string V498 { get; set; }
    public string V499 { get; set; }
    public string V500 { get; set; }

    //public string ExceedanceReason { get; set; }

    private string _strDateFormat = "DD/MM/YYYY";

    public TempRecord()
    {

    }

    public string DateFormat
    {
        get { return _strDateFormat; }
        set { _strDateFormat = value; }
    }

    public int? TempRecordID
    {
        get { return _iTempRecordID; }
        set { _iTempRecordID = value; }
    }
    public int? AccountID
    {
        get { return _iAccountID; }
        set { _iAccountID = value; }
    }
    public int? BatchID
    {
        get { return _iBatchID; }
        set { _iBatchID = value; }
    }

    public int? TableID
    {
        get { return _iTableID; }
        set { _iTableID = value; }
    }
    public DateTime? DateTimeRecorded
    {
        get { return _dtDateTimeRecorded; }
        set { _dtDateTimeRecorded = value; }
    }
    public string Notes
    {
        get { return _strNotes; }
        set { _strNotes = value; }
    }

   
    public bool? IsActive
    {
        get { return _bIsActive; }
        set { _bIsActive = value; }
    }

    public bool? IsValidated
    {
        get { return _bIsValidated; }
        set { _bIsValidated = value; }
    }

    public string WarningReason
    {
        get { return _strWarningReason; }
        set { _strWarningReason = value; }
    }

    public string RejectReason
    {
        get { return _strRejectReason; }
        set { _strRejectReason = value; }
    }
    public string ValidationResults
    {
        get { return _strValidationResults; }
        set { _strValidationResults = value; }
    }


   
    public string TableName
    {
        get { return _strTableName; }
        set { _strTableName = value; }
    }






}


[Serializable]
public class Content
{
    private int? _iContentID;
    private string _strContentKey;
    private string _strHeading;
    private string _strContent;
    private string _strStoredProcedure;
    private DateTime? _dateAdded;
    private DateTime? _dateUpdated;
    private int? _iAccountID;
    private bool? _bForAllAccount;
    public int? ContentTypeID { get; set; }
    public Content(int? p_iContentID, string p_strContentKey, string p_strHeading,
        string p_strContent,  string p_strStoredProcedure,
        DateTime? p_dateAdded, DateTime? p_dateUpdated, int? p_iAccountID, 
        bool? p_bForAllAccount)
    {
        _iContentID = p_iContentID;
        _strContentKey = p_strContentKey;
        _strHeading = p_strHeading;
        _strContent = p_strContent;
        _strStoredProcedure = p_strStoredProcedure;
        _dateAdded = p_dateAdded;
        _dateUpdated = p_dateUpdated;
        _iAccountID = p_iAccountID;
        _bForAllAccount = p_bForAllAccount;
    }

    public int? ContentID
    {
        get { return _iContentID; }
        set { _iContentID = value; }
    }
    public int? AccountID
    {
        get { return _iAccountID; }
        set { _iAccountID = value; }
    }
  
    public bool? ForAllAccount
    {
        get { return _bForAllAccount; }
        set { _bForAllAccount = value; }
    }
    public string ContentKey
    {
        get { return _strContentKey; }
        set { _strContentKey = value; }
    }
    public string Heading
    {
        get { return _strHeading; }
        set { _strHeading = value; }
    }
    public string ContentP
    {
        get { return _strContent; }
        set { _strContent = value; }
    }
  
    public string StoredProcedure
    {
        get { return _strStoredProcedure; }
        set { _strStoredProcedure = value; }
    }
    public DateTime? DateAdded
    {
        get { return _dateAdded; }
        set { _dateAdded = value; }
    }
    public DateTime? DateUpdated
    {
        get { return _dateUpdated; }
        set { _dateUpdated = value; }
    }


}

[Serializable]
public class UserContent
{
    private int? _iUserContentID;
    private int? _iUserID;
    private int? _iContentID;
    private bool? _bIsDefaultShow;


    public UserContent(int? p_iUserContentID, int? p_iUserID, int? p_iContentID,
        bool? p_bIsDefaultShow)
    {
        _iUserContentID = p_iUserContentID;
        _iUserID = p_iUserID;
        _iContentID = p_iContentID;
        _bIsDefaultShow = p_bIsDefaultShow;
    }

    public int? UserContentID
    {
        get { return _iUserContentID; }
        set { _iUserContentID = value; }
    }
    public int? UserID
    {
        get { return _iUserID; }
        set { _iUserID = value; }
    }
    public int? ContentID
    {
        get { return _iContentID; }
        set { _iContentID = value; }
    }

    public bool? IsDefaultShow
    {
        get { return _bIsDefaultShow; }
        set { _bIsDefaultShow = value; }
    }
   

}

[Serializable]
public class TableUser
{
    private int? _iTableUserID;
    private int? _iTableID;
    private int? _iUserID;   
    private bool? _bLateWarningEmail;
    private bool? _bLateWarningSMS;
    private bool? _bUploadEmail;
    private bool? _bUploadSMS;
    private bool? _bUploadWarningEmail;
    private bool? _bUploadWarningSMS;
    private bool? _bAddDataEmail ;
    private bool? _bAddDataSMS;

    public bool? ExceedanceEmail { set; get; }
    public bool? ExceedanceSMS { set; get; }

    public TableUser(int? p_iTableUserID,  int? p_iTableID,
        int? p_iUserID, 
        bool? p_bLateWarningEmail, bool? p_bLateWarningSMS, bool? p_bUploadEmail, bool? p_bUploadSMS,
        bool? p_bUploadWarningEmail, bool? p_bUploadWarningSMS, bool? p_bAddDataEmail, bool? p_bAddDataSMS)
    {
        _iTableUserID = p_iTableUserID;
        _iTableID = p_iTableID;
        _iUserID = p_iUserID;     
        _bLateWarningEmail = p_bLateWarningEmail;
        _bLateWarningSMS = p_bLateWarningSMS;
        _bUploadEmail = p_bUploadEmail;
        _bUploadSMS = p_bUploadSMS;
        _bUploadWarningEmail = p_bUploadWarningEmail;
        _bUploadWarningSMS = p_bUploadWarningSMS;
        _bAddDataEmail = p_bAddDataEmail;
        _bAddDataSMS = p_bAddDataSMS;

    }

    public bool? AddDataEmail
    {
        get { return _bAddDataEmail; }
        set { _bAddDataEmail = value; }
    }

    public bool? AddDataSMS
    {
        get { return _bAddDataSMS; }
        set { _bAddDataSMS = value; }
    }

    public bool? UploadWarningEmail
    {
        get { return _bUploadWarningEmail; }
        set { _bUploadWarningEmail = value; }
    }

    public bool? UploadWarningSMS
    {
        get { return _bUploadWarningSMS; }
        set { _bUploadWarningSMS = value; }
    }


    public bool? UploadEmail
    {
        get { return _bUploadEmail; }
        set { _bUploadEmail = value; }
    }
    public bool? UploadSMS
    {
        get { return _bUploadSMS; }
        set { _bUploadSMS = value; }
    }

    public bool? LateWarningEmail
    {
        get { return _bLateWarningEmail; }
        set { _bLateWarningEmail = value; }
    }
    public bool? LateWarningSMS
    {
        get { return _bLateWarningSMS; }
        set { _bLateWarningSMS = value; }
    }

    public int? TableUserID
    {
        get { return _iTableUserID; }
        set { _iTableUserID = value; }
    }

    public int? UserID
    {
        get { return _iUserID; }
        set { _iUserID = value; }
    }
    public int? TableID
    {
        get { return _iTableID; }
        set { _iTableID = value; }
    }

  


}




[Serializable]
public class Document
{
    private int? _iDocumentID;
    private int? _iAccountID;
    private string _strDocumentText;
    private int? _iDocumentTypeID;
     private string _strFileUniqename;
     private string _strFileTitle;
     private DateTime? _dateDocumentDate;
    private DateTime? _dateAdded;
    private DateTime? _dateUpdated;
    private int? _iUserID;
    private int? _iTableID;
    private string _strReportHTML;
    private bool? _bIsReportPublic;
    private string _strDocumentDescription;
    private DateTime? _dateDocumentEndDate;

    public bool? ForDashBoard { get; set;}
    public int? FolderID { get; set; }
    public double? Size { get; set; }
    public string ReportType { get; set; }

    public Document(int? p_iDocumentID, int? p_iAccountID, string p_strDocumentText,
        int? p_iDocumentTypeID, string p_strFileUniqename, string p_strFileTitle,
        DateTime? p_dateDocumentDate,DateTime? p_dateAdded, DateTime? p_dateUpdated
        , int? p_iUserID, int? p_iTableID)
    {
        _iDocumentID = p_iDocumentID;
        _iAccountID = p_iAccountID;
        _strDocumentText = p_strDocumentText;
        _iDocumentTypeID = p_iDocumentTypeID;
        _strFileUniqename = p_strFileUniqename;
        _strFileTitle = p_strFileTitle;
        _dateDocumentDate = p_dateDocumentDate;
        _dateAdded = p_dateAdded;
        _dateUpdated = p_dateUpdated;
        _iUserID = p_iUserID;
        _iTableID = p_iTableID;

    }


    public string DocumentDescription
    {
        get { return _strDocumentDescription; }
        set { _strDocumentDescription = value; }
    }


    public DateTime? DocumentEndDate
    {
        get { return _dateDocumentEndDate; }
        set { _dateDocumentEndDate = value; }
    }




    public string ReportHTML
    {
        get { return _strReportHTML; }
        set { _strReportHTML = value; }
    }
    public bool? IsReportPublic
    {
        get { return _bIsReportPublic; }
        set { _bIsReportPublic = value; }
    }




    public int? TableID
    {
        get { return _iTableID; }
        set { _iTableID = value; }
    }

    public int? DocumentID
    {
        get { return _iDocumentID; }
        set { _iDocumentID = value; }
    }

    public int? AccountID
    {
        get { return _iAccountID; }
        set { _iAccountID = value; }
    }
    public string DocumentText
    {
        get { return _strDocumentText; }
        set { _strDocumentText = value; }
    }
    public int? DocumentTypeID
    {
        get { return _iDocumentTypeID; }
        set { _iDocumentTypeID = value; }
    }
    public string FileUniqename
    {
        get { return _strFileUniqename; }
        set { _strFileUniqename = value; }
    }
    public string FileTitle
    {
        get { return _strFileTitle; }
        set { _strFileTitle = value; }
    }

    public DateTime? DocumentDate
    {
        get { return _dateDocumentDate; }
        set { _dateDocumentDate = value; }
    }


    public DateTime? DateAdded
    {
        get { return _dateAdded; }
        set { _dateAdded = value; }
    }
    public DateTime? DateUpdated
    {
        get { return _dateUpdated; }
        set { _dateUpdated = value; }
    }



    public int? UserID
    {
        get { return _iUserID; }
        set { _iUserID = value; }
    }

   
}




[Serializable]
public class InComingEmail
{
    private int? _iInComingEmailID;
    private string _strEmailSubject;
    private string _strEmailFrom;
    private string _strEmailTo;
    private string _strCC;
    private string _strBCC;
    private string _strAttachments;
    private DateTime? _dtEmailDate;
    private string _strMessageID;
    private string _strRawMessage;
    private string _strTextMessage;
    private string _strHTMLTextMessage;
    private string _strMIMEVersion;
    private DateTime? _dtDateCreated;
    private int? _iParentEmailID;
    private string _strBatchIDs;

    public string POPServer { get; set; }

    public InComingEmail()
    {
    }

    public InComingEmail(  int? p_iInComingEmailID,
     string p_strEmailSubject,
     string p_strEmailFrom,
     string p_strEmailTo,
     string p_strCC,
     string p_strBCC,
     string p_strAttachments,
     DateTime? p_dtEmailDate,
     string p_strMessageID,
     string p_strRawMessage,
     string p_strTextMessage,
     string p_strHTMLTextMessage,
     string p_strMIMEVersion,
     DateTime? p_dtDateCreated,
     int? p_iParentEmailID,
     string p_strBatchIDs)
    {
        _iInComingEmailID = p_iInComingEmailID;
        _strEmailSubject=p_strEmailSubject;
        _strEmailFrom=p_strEmailFrom;
        _strEmailTo=p_strEmailTo;
        _strCC=p_strCC;
        _strBCC=p_strBCC;
        _strAttachments=p_strAttachments;
        _dtEmailDate=p_dtEmailDate;
        _strMessageID =p_strMessageID;
        _strRawMessage =p_strRawMessage;
        _strTextMessage =p_strTextMessage;
        _strHTMLTextMessage =p_strHTMLTextMessage;
        _strMIMEVersion =p_strMIMEVersion;
        _dtDateCreated =p_dtDateCreated;
        _iParentEmailID =p_iParentEmailID;
        _strBatchIDs =p_strBatchIDs;

    }

    public int? InComingEmailID
    {
        get { return _iInComingEmailID; }
        set { _iInComingEmailID = value; }
    }

    public string EmailSubject
    {
        get { return _strEmailSubject; }
        set { _strEmailSubject = value; }
    }
    public string EmailFrom
    {
        get { return _strEmailFrom; }
        set { _strEmailFrom = value; }
    }
    public string EmailTo
    {
        get { return _strEmailTo; }
        set { _strEmailTo = value; }
    }
    public string CC
    {
        get { return _strCC; }
        set { _strCC = value; }
    }
    public string BCC
    {
        get { return _strBCC; }
        set { _strBCC = value; }
    }

    public string Attachments
    {
        get { return _strAttachments; }
        set { _strAttachments = value; }
    }


    public DateTime? EmailDate
    {
        get { return _dtEmailDate; }
        set { _dtEmailDate = value; }
    }
    public string MessageID
    {
        get { return _strMessageID; }
        set { _strMessageID = value; }
    }



    public string RawMessage
    {
        get { return _strRawMessage; }
        set { _strRawMessage = value; }
    }

    public string TextMessage
    {
        get { return _strTextMessage; }
        set { _strTextMessage = value; }
    }
    public string HTMLTextMessage
    {
        get { return _strHTMLTextMessage; }
        set { _strHTMLTextMessage = value; }
    }
    public string MIMEVersion
    {
        get { return _strMIMEVersion; }
        set { _strMIMEVersion = value; }
    }
    public DateTime? DateCreated
    {
        get { return _dtDateCreated; }
        set { _dtDateCreated = value; }
    }
    public int? ParentEmailID
    {
        get { return _iParentEmailID; }
        set { _iParentEmailID = value; }
    }
    public string BatchIDs
    {
        get { return _strBatchIDs; }
        set { _strBatchIDs = value; }
    }


}




[Serializable]
public class dbgFile
{
    private int? _iFileID;
    private string _strFileTitle;
    private string _strFileType;
    private string _strFileName;
    private string _strUniqueName;
    private DateTime? _dateAdded;
    private DateTime? _dateUpdated;
    private int? _iAccountID;
    private bool? _bIsTemp;
     private bool? _bIsIncomingEmail;


     public dbgFile(int? p_iFileID,
     string p_strFileTitle,
     string p_strFileType,
     string p_strFileName,
     string p_strUniqueName,
     DateTime? p_dateAdded,
     DateTime? p_dateUpdated,
     int? p_iAccountID,
     bool? p_bIsTemp,
      bool? p_bIsIncomingEmail  )
    {
         _iFileID=p_iFileID;
      _strFileTitle=p_strFileTitle;
      _strFileType=p_strFileType;
      _strFileName=p_strFileName;
      _strUniqueName=p_strUniqueName;
      _dateAdded=p_dateAdded;
      _dateUpdated=p_dateUpdated;
     _iAccountID= p_iAccountID;
      _bIsTemp=p_bIsTemp;
      _bIsIncomingEmail = p_bIsIncomingEmail;

    }

    public int? FileID
    {
        get { return _iFileID; }
        set { _iFileID = value; }
    }

    public string FileTitle
    {
        get { return _strFileTitle; }
        set { _strFileTitle = value; }
    }
    public string FileType
    {
        get { return _strFileType; }
        set { _strFileType = value; }
    }
    public string FileName
    {
        get { return _strFileName; }
        set { _strFileName = value; }
    }
    public string UniqueName
    {
        get { return _strUniqueName; }
        set { _strUniqueName = value; }
    }
    public DateTime? DateAdded
    {
        get { return _dateAdded; }
        set { _dateAdded = value; }
    }

    public DateTime? DateUpdated
    {
        get { return _dateUpdated; }
        set { _dateUpdated = value; }
    }


    public int? AccountID
    {
        get { return _iAccountID; }
        set { _iAccountID = value; }
    }
    public bool? IsTemp
    {
        get { return _bIsTemp; }
        set { _bIsTemp = value; }
    }



    public bool? IsIncomingEmail
    {
        get { return _bIsIncomingEmail; }
        set { _bIsIncomingEmail = value; }
    }


}


[Serializable]
public class DocumentType
{
    private int? _iDocumentTypeID;
    private int? _iAccountID;
    private string _strDocumentTypeName;
    private DateTime? _dateAdded;
    private DateTime? _dateUpdated;

    public DocumentType(int? p_iDocumentTypeID, int? p_iAccountID, string p_strDocumentTypeName,
         DateTime? p_dateAdded, DateTime? p_dateUpdated)
	{
        _iDocumentTypeID = p_iDocumentTypeID;
        _iAccountID = p_iAccountID;
        _strDocumentTypeName = p_strDocumentTypeName;
        _dateAdded = p_dateAdded;
        _dateUpdated = p_dateUpdated;
	}

    public int? DocumentTypeID
    {
        get { return _iDocumentTypeID; }
        set { _iDocumentTypeID = value; }
    }
    public int? AccountID
    {
        get { return _iAccountID; }
        set { _iAccountID = value; }
    }
    public string DocumentTypeName
    {
        get { return _strDocumentTypeName; }
        set { _strDocumentTypeName = value; }
    }   
    
    public DateTime? DateAdded { get { return _dateAdded; }
        set { _dateAdded = value; }
    }
    public DateTime? DateUpdated { get { return _dateUpdated; }
        set { _dateUpdated = value; }
    }


}


[Serializable]
public class RoleTable
{
    private int? _iRoleTableID;
    private int? _iTableID;
  
    private int? _iRoleType;
    private DateTime? _dateAdded;
    private DateTime? _dateUpdated;
    public bool? CanExport { get; set; }
    public int? RoleID { get; set; }

    public bool? AllowEditView { get; set; }
    public int? ViewsDefaultFromUserID { get; set; }
    public bool? ShowMenu { get; set; }


    public RoleTable(int? p_iRoleTableID, int? p_iTableID,
        int? p_iRecordRightID,  DateTime? p_dateAdded, DateTime? p_dateUpdated)
    {
        _iRoleTableID = p_iRoleTableID;
        _iTableID = p_iTableID;    
        _iRoleType = p_iRecordRightID;
        _dateAdded = p_dateAdded;
        _dateUpdated = p_dateUpdated;
    }

    public int? RoleTableID
    {
        get { return _iRoleTableID; }
        set { _iRoleTableID = value; }
    }

   
    public int? TableID
    {
        get { return _iTableID; }
        set { _iTableID = value; }
    }

    public int? RoleType
    {
        get { return _iRoleType; }
        set { _iRoleType = value; }
    }
   
    public DateTime? DateAdded
    {
        get { return _dateAdded; }
        set { _dateAdded = value; }
    }
    public DateTime? DateUpdated
    {
        get { return _dateUpdated; }
        set { _dateUpdated = value; }
    }



}


//[Serializable]
//public class Sensor
//{
//    private int? _iSensorID;
//    private int? _iAccountID;
//    private int? _iSensorTypeID;
//    private string _strReferenceNumber;
//    private string _strMake;
//    private string _strModel;
//    private double? _dMinimumDetectableUnit;
//    private string _strNotes;
//    private DateTime? _dateAdded;
//    private DateTime? _dateUpdated;
//    private bool? _bIsActive;
//    private int? _iUserAdded;
//    private int? _iUserUpdated;

//    public Sensor(int? p_iSensorID, int? p_iAccountID, int? p_iSensorTypeID, string p_strReferenceNumber, string p_strMake,
//        string p_strModel, double? p_dMinimumDetectableUnit, string p_strNotes, DateTime? p_dateAdded, DateTime? p_dateUpdated)
//    {
//        _iSensorID = p_iSensorID;
//        _iAccountID = p_iAccountID;
//        _iSensorTypeID = p_iSensorTypeID;
//        _strReferenceNumber = p_strReferenceNumber;
//        _strMake = p_strMake;
//        _strModel = p_strModel;
//        _dMinimumDetectableUnit = p_dMinimumDetectableUnit;
//        _strNotes = p_strNotes;
//        _dateAdded = p_dateAdded;
//        _dateUpdated = p_dateUpdated;
       
//    }

//    public int? UserAdded
//    {
//        get { return _iUserAdded; }
//        set { _iUserAdded = value; }
//    }

//    public int? UserUpdated
//    {
//        get { return _iUserUpdated; }
//        set { _iUserUpdated = value; }
//    }


//    public bool? IsActive
//    {
//        get { return _bIsActive; }
//        set { _bIsActive = value; }
//    }

//    public int? SensorID
//    {
//        get { return _iSensorID; }
//        set { _iSensorID = value; }
//    }
//    public int? AccountID
//    {
//        get { return _iAccountID; }
//        set { _iAccountID = value; }
//    }
//    public int? SensorTypeID
//    {
//        get { return _iSensorTypeID; }
//        set { _iSensorTypeID = value; }
//    }

//    public string ReferenceNumber
//    {
//        get { return _strReferenceNumber; }
//        set { _strReferenceNumber = value; }
//    }
//    public string Make
//    {
//        get { return _strMake; }
//        set { _strMake = value; }
//    }
//    public string Model
//    {
//        get { return _strModel; }
//        set { _strModel = value; }
//    }

//    public double? MinimumDetectableLimit
//    {
//        get { return _dMinimumDetectableUnit; }
//        set { _dMinimumDetectableUnit = value; }
//    }

//    public string Notes
//    {
//        get { return _strNotes; }
//        set { _strNotes = value; }
//    }

//    public DateTime? DateAdded
//    {
//        get { return _dateAdded; }
//        set { _dateAdded = value; }
//    }
//    public DateTime? DateUpdated
//    {
//        get { return _dateUpdated; }
//        set { _dateUpdated = value; }
//    }


//}


//[Serializable]
//public class SensorType
//{
//    private int? _iSensorTypeID;
//      private int? _iAccountID;
//    private string _strSensorType;

//    public SensorType(int? p_iSensorTypeID, int? p_iAccountID, string p_strSensorType)
//    {
//        _iSensorTypeID = p_iSensorTypeID;
//        _iAccountID = p_iAccountID;
//        _strSensorType = p_strSensorType;      
//    }

//    public int? SensorTypeID
//    {
//        get { return _iSensorTypeID; }
//        set { _iSensorTypeID = value; }
//    }

//    public int? AccountID
//    {
//        get { return _iAccountID; }
//        set { _iAccountID = value; }
//    }
//    public string SensorTypeName
//    {
//        get { return _strSensorType; }
//        set { _strSensorType = value; }
//    }  
//}


//[Serializable]
//public class Calibration
//{
//    private int? _iCalibrationID;
//    private int? _iSensorID;
//    private string _strFileDisplayName;
//    private string _strFileInternalName;
//    private DateTime? _dateCalibrationValidForm;
//    private DateTime? _dateCalibrationValidTo;
//    private string _strNotes;
//    private DateTime? _dateAdded;
//    private DateTime? _dateUpdated;
//    private bool? _bIsActive;

//    public Calibration(int? p_iCalibrationID, int? p_iSensorID, string p_strFileDisplayName, string p_strFileInternalName,
//       DateTime? p_dateCalibrationValidForm, DateTime? p_dateCalibrationValidTo,
//        string p_strNotes, DateTime? p_dateAdded, DateTime? p_dateUpdated)
//    {
//        _iCalibrationID = p_iCalibrationID;
//        _iSensorID = p_iSensorID;
//        _strFileDisplayName = p_strFileDisplayName;
//        _strFileInternalName = p_strFileInternalName;
//        _dateCalibrationValidForm = p_dateCalibrationValidForm;
//        _dateCalibrationValidTo = p_dateCalibrationValidTo;
//        _strNotes = p_strNotes;
//        _dateAdded = p_dateAdded;
//        _dateUpdated = p_dateUpdated;
       
//    }

//    public bool? IsActive
//    {
//        get { return _bIsActive; }
//        set { _bIsActive = value; }
//    }

//    public int? CalibrationID
//    {
//        get { return _iCalibrationID; }
//        set { _iCalibrationID = value; }
//    }

//    public int? SensorID
//    {
//        get { return _iSensorID; }
//        set { _iSensorID = value; }
//    }

//    public string FileDisplayName
//    {
//        get { return _strFileDisplayName; }
//        set { _strFileDisplayName = value; }
//    }

//    public string FileInternalName
//    {
//        get { return _strFileInternalName; }
//        set { _strFileInternalName = value; }
//    }
//    public DateTime? CalibrationValidForm
//    {
//        get { return _dateCalibrationValidForm; }
//        set { _dateCalibrationValidForm = value; }
//    }
//    public DateTime? CalibrationValidTo
//    {
//        get { return _dateCalibrationValidTo; }
//        set { _dateCalibrationValidTo = value; }
//    }

//    public string Notes
//    {
//        get { return _strNotes; }
//        set { _strNotes = value; }
//    }

//    public DateTime? DateAdded
//    {
//        get { return _dateAdded; }
//        set { _dateAdded = value; }
//    }
//    public DateTime? DateUpdated
//    {
//        get { return _dateUpdated; }
//        set { _dateUpdated = value; }
//    }


//}






//[Serializable]
//public class SensorNotificationUser
//{
//    private int? _iSensorNotificationUserID;
//    private int? _iSensorID;
//    private int? _iUserID;
//    private bool? _bWarningEmail;
//    private bool? _bWarningSMS;

//    public SensorNotificationUser(int? p_iSensorNotificationUserID, int? p_iSensorID,
//        int? p_iUserID, bool? p_bWarningEmail, bool? p_bWarningSMS)
//    {
//        _iSensorNotificationUserID = p_iSensorNotificationUserID;
//        _iSensorID = p_iSensorID;
//        _iUserID = p_iUserID;
//        _bWarningEmail = p_bWarningEmail;
//        _bWarningSMS = p_bWarningSMS;

//    }

//    public int? SensorNotificationUserID
//    {
//        get { return _iSensorNotificationUserID; }
//        set { _iSensorNotificationUserID = value; }
//    }

//    public int? UserID
//    {
//        get { return _iUserID; }
//        set { _iUserID = value; }
//    }
//    public int? SensorID
//    {
//        get { return _iSensorID; }
//        set { _iSensorID = value; }
//    }

//    public bool? WarningEmail
//    {
//        get { return _bWarningEmail; }
//        set { _bWarningEmail = value; }
//    }
//    public bool? WarningSMS
//    {
//        get { return _bWarningSMS; }
//        set { _bWarningSMS = value; }
//    }


//}






//[Serializable]
//public class MonitorSchedule
//{
//    private int? _iMonitorScheduleID;
//    private int?  _iAccountID;
//    private int? _iTableID;
//    private DateTime? _dateScheduleDateTime;
//    private string _strDescription;
//    private bool? _bHasAlarm;
//    private DateTime? _dateAlarmDateTime;
//    private DateTime? _dateAdded;
//    private DateTime? _dateUpdated;
//    private int? _iUserAdded;
//    private int? _iUserUpdated;
//    private int? _iInitialScheduleID;

//    public MonitorSchedule(int? p_iMonitorScheduleID, int? p__iAccountID, int? p_iTableID,
//        DateTime? p_dateScheduleDateTime,string p_strDescription,bool? p_bHasAlarm,
//        DateTime? p_dateAlarmDateTime, DateTime? p_dateAdded, DateTime? p_dateUpdated,
//        int? p_iUserAdded, int? p_iUserUpdated, int? p_iInitialScheduleID)
//    {
//        _iMonitorScheduleID = p_iMonitorScheduleID;
//        _iAccountID = p__iAccountID;
//        _iTableID = p_iTableID;
//        _dateScheduleDateTime = p_dateScheduleDateTime;
//        _strDescription = p_strDescription;
//        _bHasAlarm = p_bHasAlarm;
//        _dateAlarmDateTime = p_dateAlarmDateTime;
//        _dateAdded = p_dateAdded;
//        _dateUpdated = p_dateUpdated;
//        _iUserAdded = p_iUserAdded;
//        _iUserUpdated = p_iUserUpdated;
//        _iInitialScheduleID = p_iInitialScheduleID;
        
//    }

//    public int? InitialScheduleID
//    {
//        get { return _iInitialScheduleID; }
//        set { _iInitialScheduleID = value; }
//    }

//    public int? MonitorScheduleID
//    {
//        get { return _iMonitorScheduleID; }
//        set { _iMonitorScheduleID = value; }
//    }
//    public int? AccountID
//    {
//        get { return _iAccountID; }
//        set { _iAccountID = value; }
//    }
//    public int? TableID
//    {
//        get { return _iTableID; }
//        set { _iTableID = value; }
//    }
//    public DateTime? ScheduleDateTime
//    {
//        get { return _dateScheduleDateTime; }
//        set { _dateScheduleDateTime = value; }
//    }
//    public string Description
//    {
//        get { return _strDescription; }
//        set { _strDescription = value; }
//    }

//    public bool? HasAlarm
//    {
//        get { return _bHasAlarm; }
//        set { _bHasAlarm = value; }
//    }
//    public DateTime? AlarmDateTime
//    {
//        get { return _dateAlarmDateTime; }
//        set { _dateAlarmDateTime = value; }
//    }
//    public DateTime? DateAdded
//    {
//        get { return _dateAdded; }
//        set { _dateAdded = value; }
//    }
//    public DateTime? DateUpdated
//    {
//        get { return _dateUpdated; }
//        set { _dateUpdated = value; }
//    }
//    public int? UserAdded
//    {
//        get { return _iUserAdded; }
//        set { _iUserAdded = value; }
//    }
//    public int? UserUpdated
//    {
//        get { return _iUserUpdated; }
//        set { _iUserUpdated = value; }
//    }  


//}


[Serializable]
public class Contact
{
    private int? _iContactID;
    private string _strEmail;
    private DateTime? _dateSubscriptionDate;
    private string _strName;
    private string _strPhone;
    private string _strMessage;
    private DateTime? _dateAdded;
    private DateTime? _dateUpdated;
    private int? _iContactTypeID;

    public Contact(int? p_iContactID, string p_strEmail,
        DateTime? p_dateSubscriptionDate, string p_strName, string p_strPhone,
        string p_strMessage, int? p_iContactTypeID)
    {
        _iContactID = p_iContactID;
        _strEmail = p_strEmail;
        _dateSubscriptionDate = p_dateSubscriptionDate;
        _strName = p_strName;
        _strPhone = p_strPhone;
        _strMessage = p_strMessage;
        _iContactTypeID = p_iContactTypeID;
    }

    public int? ContactTypeID
    {
        get { return _iContactTypeID; }
        set { _iContactTypeID = value; }
    }

    public string Name
    {
        get { return _strName; }
        set { _strName = value; }
    }
    public string Phone
    {
        get { return _strPhone; }
        set { _strPhone = value; }
    }
    public string Message
    {
        get { return _strMessage; }
        set { _strMessage = value; }
    }
    public int? ContactID
    {
        get { return _iContactID; }
        set { _iContactID = value; }
    }
    public string Email
    {
        get { return _strEmail; }
        set { _strEmail = value; }
    }

    public DateTime? SubscriptionDate
    {
        get { return _dateSubscriptionDate; }
        set { _dateSubscriptionDate = value; }
    }

    public DateTime? DateAdded
    {
        get { return _dateAdded; }
        set { _dateAdded = value; }
    }
    public DateTime? DateUpdated
    {
        get { return _dateUpdated; }
        set { _dateUpdated = value; }
    }


}



[Serializable]
public class AccountType
{
    private int? _iAccountTypeID;
    private string _strAccountTypeName;
    //private int? _iMaxRecordsPerMonth;
    private int? _iMaxTotalRecords;
    private int? _iMaxUsers;

    public int? DiskSpaceMB { set; get; }
    public double? CostPerMonth { set; get; }

    public int? MaxEmailsPerMonth { set; get; }
    public int? MaxSMSPerMonth { set; get; }

    public AccountType(int? p_iAccountTypeID, string p_strAccountTypeName,
        int? p_iMaxTotalRecords,int? p_iMaxUsers)
    {
        _iAccountTypeID = p_iAccountTypeID;
        _strAccountTypeName = p_strAccountTypeName;
        _iMaxTotalRecords = p_iMaxTotalRecords;
        _iMaxUsers = p_iMaxUsers;
        
    }

    public int? MaxUsers
    {
        get { return _iMaxUsers; }
        set { _iMaxUsers = value; }
    }

    public int? AccountTypeID
    {
        get { return _iAccountTypeID; }
        set { _iAccountTypeID = value; }
    }

    public string AccountTypeName
    {
        get { return _strAccountTypeName; }
        set { _strAccountTypeName = value; }
    }

    //public int? MaxRecordsPerMonth
    //{
    //    get { return _iMaxRecordsPerMonth; }
    //    set { _iMaxRecordsPerMonth = value; }
    //}

    public int? MaxTotalRecords
    {
        get { return _iMaxTotalRecords; }
        set { _iMaxTotalRecords = value; }
    }   


}



[Serializable]
public class VisitorLog
{
    private int? _iVisitorLogID;
    private int? _iUserID;
    private string _strIPAddress;
    private string _strBrowser;
    private string _strPageURL;
   
    private DateTime? _dateAdded;
    private string _strRefSite;

    public VisitorLog(int? p_iVisitorLogID, int? p_iUserID, string p_strIPAddress, string p_strBrowser,
        string p_strPageURL, DateTime? p_dateAdded, string p_strRefSite)
    {
        _iVisitorLogID = p_iVisitorLogID;
        _iUserID = p_iUserID;
        _strIPAddress = p_strIPAddress;
        _strBrowser = p_strBrowser;
        _strPageURL = p_strPageURL;
        
        _dateAdded = p_dateAdded;
        _strRefSite = p_strRefSite;
       
    }

    public string RefSite
    {
        get { return _strRefSite; }
        set { _strRefSite = value; }
    }

    public int? VisitorLogID
    {
        get { return _iVisitorLogID; }
        set { _iVisitorLogID = value; }
    }

    public int? UserID
    {
        get { return _iUserID; }
        set { _iUserID = value; }
    }

    public string IPAddress
    {
        get { return _strIPAddress; }
        set { _strIPAddress = value; }
    }
    public string Browser
    {
        get { return _strBrowser; }
        set { _strBrowser = value; }
    }
    public string PageURL
    {
        get { return _strPageURL; }
        set { _strPageURL = value; }
    }

 

    public DateTime? DateAdded
    {
        get { return _dateAdded; }
        set { _dateAdded = value; }
    }
   


}




//[Serializable]
//public class Payment
//{
//    private int? _iPaymentID;
//    private int? _iMonthsPaid;
//    private string _strPaymentType;
//    private int? _iPaypalID;


//    private int? _iUserID;
//    private int? _iAccountID;
//    private bool? _bIsManual;
//    private double? _dPaymentAmount;

//    private bool? _bIsPaid;
//    private DateTime? _datePaymentReceiveDate;
//    private string _strCustomerInfo;  


//    private DateTime? _dateAdded;
//    private DateTime? _dateUpdated;
//    private int? _iLastUpdatedUserID;
//    private string _strPrimaryPhone;
//    private string _strBackUpEmail;
//    private string _strBillingAddress;


//    public Payment(int? p_iPaymentID, int? p_iMonthsPaid, string p_strPaymentType,
//        int? p_iPaypalID, int? p_iUserID, int? p_iAccountID, bool? p_bIsManual,
//        double? p_dPaymentAmount, bool? p_bIsPaid, DateTime? p_datePaymentReceiveDate, string p_strCustomerInfo,
//        DateTime? p_dateAdded, DateTime? p_dateUpdated)
//    {
//        _iPaymentID = p_iPaymentID;
//        _iMonthsPaid = p_iMonthsPaid;
//        _strPaymentType = p_strPaymentType;
//        _iPaypalID = p_iPaypalID;
//        _iUserID = p_iUserID;
//        _iAccountID = p_iAccountID;
//        _bIsManual=p_bIsManual;
//        _dPaymentAmount = p_dPaymentAmount;
//        _bIsPaid = p_bIsPaid;
//        _datePaymentReceiveDate = p_datePaymentReceiveDate;
//        _strCustomerInfo = p_strCustomerInfo;
//        _dateAdded = p_dateAdded;
//        _dateUpdated = p_dateUpdated;
//    }

//    public string BillingAddress
//    {
//        get { return _strBillingAddress; }
//        set { _strBillingAddress = value; }
//    }
//    public string BackUpEmail
//    {
//        get { return _strBackUpEmail; }
//        set { _strBackUpEmail = value; }
//    }
//    public string PrimaryPhone
//    {
//        get { return _strPrimaryPhone; }
//        set { _strPrimaryPhone = value; }
//    }

//    public int? LastUpdatedUserID
//    {
//        get { return _iLastUpdatedUserID; }
//        set { _iLastUpdatedUserID = value; }
//    }

//    public int? PaymentID
//    {
//        get { return _iPaymentID; }
//        set { _iPaymentID = value; }
//    }
//    public int? MonthsPaid
//    {
//        get { return _iMonthsPaid; }
//        set { _iMonthsPaid = value; }
//    }
//    public string PaymentType
//    {
//        get { return _strPaymentType; }
//        set { _strPaymentType = value; }
//    }
//    public int? PaypalID
//    {
//        get { return _iPaypalID; }
//        set { _iPaypalID = value; }
//    }


//    public int? UserID
//    {
//        get { return _iUserID; }
//        set { _iUserID = value; }
//    }
//    public int? AccountID
//    {
//        get { return _iAccountID; }
//        set { _iAccountID = value; }
//    }
//    public bool? IsManual
//    {
//        get { return _bIsManual; }
//        set { _bIsManual = value; }
//    }
//    public Double? PaymentAmount
//    {
//        get { return _dPaymentAmount; }
//        set { _dPaymentAmount = value; }
//    }


//    public bool? IsPaid
//    {
//        get { return _bIsPaid; }
//        set { _bIsPaid = value; }
//    }
//    public DateTime? PaymentReceiveDate
//    {
//        get { return _datePaymentReceiveDate; }
//        set { _datePaymentReceiveDate = value; }
//    }
//    public string CustomerInfo
//    {
//        get { return _strCustomerInfo; }
//        set { _strCustomerInfo = value; }
//    }
   
//    public DateTime? DateAdded
//    {
//        get { return _dateAdded; }
//        set { _dateAdded = value; }
//    }
//    public DateTime? DateUpdated
//    {
//        get { return _dateUpdated; }
//        set { _dateUpdated = value; }
//    }


//}




[Serializable]
public class Paypal
{
    private int? _iPaypalID;
    private string _strtxn_id;
    private string _strpayment_status;
    private string _strpending_reason;

    private string _strpayer_email;
    private string _strreceiver_email;
    private double? _dmc_gross;
    private string _strtxn_type;


    private DateTime? _dateAdded;
   

    public Paypal(int? p_iPaypalID, string p_strtxn_id, string p_strpayment_status,
        string p_strpending_reason, string p_strpayer_email,string p_strreceiver_email,
        double? p_dmc_gross,string p_strtxn_type,
         DateTime? p_dateAdded)
    {
        _iPaypalID = p_iPaypalID;
        _strtxn_id = p_strtxn_id;
        _strpayment_status = p_strpayment_status;
        _strpending_reason = p_strpending_reason;
        _strpayer_email = p_strpayer_email;
        _strreceiver_email = p_strreceiver_email;
        _strtxn_type = p_strtxn_type;

        _dmc_gross = p_dmc_gross;
        _dateAdded = p_dateAdded;
      
    }

    public int? PaypalID
    {
        get { return _iPaypalID; }
        set { _iPaypalID = value; }
    }
    public string txn_id
    {
        get { return _strtxn_id; }
        set { _strtxn_id = value; }
    }
    public string payment_status
    {
        get { return _strpayment_status; }
        set { _strpayment_status = value; }
    }
    public string pending_reason
    {
        get { return _strpending_reason; }
        set { _strpending_reason = value; }
    }



    public string payer_email
    {
        get { return _strpayer_email; }
        set { _strpayer_email = value; }
    }
    public string receiver_email
    {
        get { return _strreceiver_email; }
        set { _strreceiver_email = value; }
    }
    public double? mc_gross
    {
        get { return _dmc_gross; }
        set { _dmc_gross = value; }
    }
    public string txn_type
    {
        get { return _strtxn_type; }
        set { _strtxn_type = value; }
    }




    public DateTime? DateAdded
    {
        get { return _dateAdded; }
        set { _dateAdded = value; }
    }
   


}






[Serializable]
public class Usage
{
    private int? _iUsageID;
    private int? _iAccountID;
    private DateTime? _dateDate;
    private int? _iSignedInCount;
    private int? _iUploadedCount;

    public Usage(int? p_iUsageID, int? p_iAccountID, DateTime? p_dateDate,
        int? p_iSignedInCount, int? p_iUploadedCount)
    {
        _iUsageID = p_iUsageID;
        _iAccountID = p_iAccountID;
        _dateDate = p_dateDate;
        _iSignedInCount = p_iSignedInCount;
        _iUploadedCount = p_iUploadedCount;
        
    }

    public int? UsageID
    {
        get { return _iUsageID; }
        set { _iUsageID = value; }
    }

    public int? AccountID
    {
        get { return _iAccountID; }
        set { _iAccountID = value; }
    }

    public DateTime? Date
    {
        get { return _dateDate; }
        set { _dateDate = value; }
    }

    public int? SignedInCount
    {
        get { return _iSignedInCount; }
        set { _iSignedInCount = value; }
    }

    public int? UploadedCount
    {
        get { return _iUploadedCount; }
        set { _iUploadedCount = value; }
    }   


}




[Serializable]
public class LookUpData
{
    private int? _iLookupDataID;
    private int? _iLookupTypeID;
    private string _strDisplayText;
    private DateTime? _dateAdded;
    private DateTime? _dateUpdated;
    private string _strValue;

    public LookUpData(int? p_iLookupDataID, int? p_iLookupTypeID, string p_strDisplayText, string p_strValue,
         DateTime? p_dateAdded, DateTime? p_dateUpdated)
    {
        _iLookupDataID = p_iLookupDataID;
        _iLookupTypeID = p_iLookupTypeID;
        _strDisplayText = p_strDisplayText;
        _dateAdded = p_dateAdded;
        _dateUpdated = p_dateUpdated;
        _strValue = p_strValue;
    }

    public string Value
    {
        get { return _strValue; }
        set { _strValue = value; }
    }

    public int? LookupDataID
    {
        get { return _iLookupDataID; }
        set { _iLookupDataID = value; }
    }
    public int? LookupTypeID
    {
        get { return _iLookupTypeID; }
        set { _iLookupTypeID = value; }
    }
    public string DisplayText
    {
        get { return _strDisplayText; }
        set { _strDisplayText = value; }
    }

    public DateTime? DateAdded
    {
        get { return _dateAdded; }
        set { _dateAdded = value; }
    }
    public DateTime? DateUpdated
    {
        get { return _dateUpdated; }
        set { _dateUpdated = value; }
    }


}




[Serializable]
public class LookupType
{
    private int? _iLookupTypeID;
    private string _strLookupTypeName;
    private bool? _bLockedValues;
    private DateTime? _dateAdded;
    private DateTime? _dateUpdated;

    public LookupType(int? p_iLookupTypeID, string p_strLookupTypeName, bool? p_bLockedValues,
         DateTime? p_dateAdded, DateTime? p_dateUpdated)
    {
        _iLookupTypeID = p_iLookupTypeID;
        _strLookupTypeName = p_strLookupTypeName;
        _bLockedValues = p_bLockedValues;
        _dateAdded = p_dateAdded;
        _dateUpdated = p_dateUpdated;
    }

    public int? LookupTypeID
    {
        get { return _iLookupTypeID; }
        set { _iLookupTypeID = value; }
    }
    public string LookupTypeName
    {
        get { return _strLookupTypeName; }
        set { _strLookupTypeName = value; }
    }
    public bool? _LockedValues
    {
        get { return _bLockedValues; }
        set { _bLockedValues = value; }
    }

    public DateTime? DateAdded
    {
        get { return _dateAdded; }
        set { _dateAdded = value; }
    }
    public DateTime? DateUpdated
    {
        get { return _dateUpdated; }
        set { _dateUpdated = value; }
    }


}









//[Serializable]
//public class MonitorScheduleUser
//{
//    private int? _iMonitorScheduleUserID;
//    private int? _iMonitorScheduleID;
//    private int? _iUserID;
//    public string EmailSMS { get; set; }


//    public MonitorScheduleUser(int? p_iMonitorScheduleUserID, int? p_iMonitorScheduleID, int? p_iUserID)
//    {
//        _iMonitorScheduleUserID = p_iMonitorScheduleUserID;
//        _iMonitorScheduleID = p_iMonitorScheduleID;
//        _iUserID = p_iUserID;
       

//    }



//    public int? MonitorScheduleUserID
//    {
//        get { return _iMonitorScheduleUserID; }
//        set { _iMonitorScheduleUserID = value; }
//    }
//    public int? MonitorScheduleID
//    {
//        get { return _iMonitorScheduleID; }
//        set { _iMonitorScheduleID = value; }
//    }

//    public int? UserID
//    {
//        get { return _iUserID; }
//        set { _iUserID = value; }
//    }




//}


[Serializable]
public class StackRoom
{
    public string ID { get; set; }
    public string Text { get; set; }
    public string TableTabID { get; set; }
    public StackRoom()
    {

    }
    public StackRoom(string p_ID, string p_Text)
    {
        ID = p_ID;
        Text = p_Text;
    }  
}

[Serializable]
public class IDnText
{
    //private byte[] _bytes;
    public string ID { get; set; }
    public string Text { get; set; }
    public IDnText()
    {

    }
    public IDnText(string p_ID, string p_Text)
    {
        ID = p_ID;
        Text = p_Text;
    }
    // change explicit to implicit depending on what you need
    //public static explicit operator byte[](IDnText m)
    //{
    //    return m._bytes;
    //}

}

[Serializable]
public class TableChild
{

    public int? TableChildID { get; set; }
    public int? ParentTableID { get; set; }
    public int? ChildTableID { get; set; }
    public string Description { get; set; }
    public string DetailPageType { get; set; }

    public bool? ShowAddButton { get; set; }
    public bool? ShowEditButton { get; set; }

    public int? HideColumnID { get; set; }
    public string HideColumnValue { get; set; }
    public string HideOperator { get; set; }

    public TableChild(int? iTableChildID, int? iParentTableID, int? iChildTableID,
        string strDescription, string strDetailPageType)
    {
        TableChildID = iTableChildID;
        ParentTableID = iParentTableID;
        ChildTableID = iChildTableID;
        Description=strDescription;
        DetailPageType = strDetailPageType;

    }

}


[Serializable]
public class ScheduleReport
{
    private int? _iScheduleReportID;
    private int? _iMainDocumentID;
    private string _strFrequency;
    private string _strFrequencyWhen;
    private DateTime? _dateAdded;
    private DateTime? _dateUpdated;

    private int? _iReportPeriod;
    private string _strReportPeriodUnit;
    private string _strEmails;

    public ScheduleReport(int? p_iScheduleReportID, int? p_iMainDocumentID, string p_strFrequency,
        string p_strFrequencyWhen, DateTime? p_dateAdded, DateTime? p_dateUpdated)
    {
        _iScheduleReportID = p_iScheduleReportID;
        _iMainDocumentID = p_iMainDocumentID;
        _strFrequency = p_strFrequency;
        _strFrequencyWhen = p_strFrequencyWhen;
        _dateAdded = p_dateAdded;
        _dateUpdated = p_dateUpdated;
    }

    public int? ReportPeriod
    {
        get { return _iReportPeriod; }
        set { _iReportPeriod = value; }
    }
    public string ReportPeriodUnit
    {
        get { return _strReportPeriodUnit; }
        set { _strReportPeriodUnit = value; }
    }
    public string Emails
    {
        get { return _strEmails; }
        set { _strEmails = value; }
    }

    public int? ScheduleReportID
    {
        get { return _iScheduleReportID; }
        set { _iScheduleReportID = value; }
    }
    public int? MainDocumentID
    {
        get { return _iMainDocumentID; }
        set { _iMainDocumentID = value; }
    }
    public string Frequency
    {
        get { return _strFrequency; }
        set { _strFrequency = value; }
    }
    public string FrequencyWhen
    {
        get { return _strFrequencyWhen; }
        set { _strFrequencyWhen = value; }
    }
    public DateTime? DateAdded
    {
        get { return _dateAdded; }
        set { _dateAdded = value; }
    }
    public DateTime? DateUpdated
    {
        get { return _dateUpdated; }
        set { _dateUpdated = value; }
    }


}



[Serializable]
public class Terminology
{

    public int? TerminologyID { get; set; }
    public int? AccountID { get; set; }
    public string PageName { get; set; }
    public string InputText { get; set; }
    public string OutputText { get; set; }

    public Terminology(int? iTerminologyID, int? iAccountID, string strPageName,
        string strInputText, string strOutputText)
    {
        TerminologyID = iTerminologyID;
        AccountID = iAccountID;
        PageName = strPageName;
        InputText = strInputText;
        OutputText = strOutputText;

    }

}






[Serializable]
public class Upload
{
    public int? UploadID {get; set;}

    public int? TableID { get; set; }
    public string UploadName { get; set; }
    public string EmailFrom { get; set; }
    public string Filename { get; set; }
    public bool UseMapping { get; set; }

    public Upload(int? p_UploadID, int? p_TableID,
        string p_UploadName, string p_EmailFrom, string p_strFilename, bool p_UseMapping)
    {
        UploadID = p_UploadID;
        TableID = p_TableID;
        UploadName = p_UploadName;
        EmailFrom = p_EmailFrom;
        Filename = p_strFilename;
        UseMapping = p_UseMapping;
    }

  


}





[Serializable]
public class SubDomainInfo
{
    public int? SubDomainInfoID { get; set; }

   
    public string RootURL { get; set; }
    public string LogoFileName { get; set; }
    public string Footer { get; set; }

    public string Notes { get; set; }

    public SubDomainInfo(int? p_SubDomainInfoID,
        string p_RootURL, string p_LogoFileName, string p_strFooter, string p_strNotes)
    {
        SubDomainInfoID = p_SubDomainInfoID;    
        RootURL = p_RootURL;
        LogoFileName = p_LogoFileName;
        Footer = p_strFooter;
        Notes = p_strNotes;
    }




}




[Serializable]
public class DataReminder
{
    public int? DataReminderID { get; set; }
    public int? ColumnID { get; set; }
    public int? NumberOfDays { get; set; }
    public string ReminderHeader { get; set; }
    public string ReminderContent { get; set; }

    public DataReminder(int? p_DataReminderID,
        int? p_ColumnID, int? p_NumberOfDays, string p_strReminderHeader, string p_strReminderContent)
    {
        DataReminderID = p_DataReminderID;
        ColumnID = p_ColumnID;
        NumberOfDays = p_NumberOfDays;
        ReminderHeader = p_strReminderHeader;
        ReminderContent = p_strReminderContent;
    }




}




[Serializable]
public class DataReminderUser
{
    public int? DataReminderUserID { get; set; }
    public int? DataReminderID { get; set; }
    public int? UserID { get; set; }
    public int? ReminderColumnID { get; set; }

    public DataReminderUser(int? p_DataReminderUserID,
        int? p_DataReminderID, int? p_UserID)
    {
        DataReminderUserID = p_DataReminderUserID;
        DataReminderID = p_DataReminderID;
        UserID = p_UserID;       
    }
    
}



[Serializable]
public class Folder
{
    public int? FolderID { get; set; }
    public int? AccountID { get; set; }
    public int? ParentFolderID { get; set; }
    public string FolderName { get; set; }

    public Folder(int? p_FolderID,
        int? p_AccountID, int? p_ParentFolderID, string p_strFolderName)
    {
        FolderID = p_FolderID;
        AccountID = p_AccountID;
        ParentFolderID = p_ParentFolderID;
        FolderName = p_strFolderName;
    }

}




[Serializable]
public class WorkFlow
{
    public int? WorkFlowID { get; set; }
    public int? AccountID { get; set; }
    public string WorkFlowName { get; set; }

    public WorkFlow(int? p_WorkFlowID,
        int? p_AccountID, string p_strWorkFlowName)
    {
        WorkFlowID = p_WorkFlowID;
        AccountID = p_AccountID;
        WorkFlowName = p_strWorkFlowName;
    }

}




[Serializable]
public class UserFolder
{
    public int? UserFolderID { get; set; }
    public int? FolderID { get; set; }
    public int? UserID { get; set; }
    public string RightType { get; set; }

    public UserFolder(int? p_UserFolderID,
        int? p_FolderID, int? p_UserID, string p_RightType)
    {
        UserFolderID = p_UserFolderID;
        FolderID = p_FolderID;
        UserID = p_UserID;
        RightType = p_RightType;
    }

}


[Serializable]
public class WorkFlowSectionType
{
    public int? WorkFlowSectionTypeID { get; set; }
    public string WorkFlowSectionTypeName { get; set; }

    public WorkFlowSectionType(int? p_WorkFlowSectionTypeID,
       string p_WorkFlowSectionTypeName)
    {
        WorkFlowSectionTypeID = p_WorkFlowSectionTypeID;
        WorkFlowSectionTypeName = p_WorkFlowSectionTypeName;
       
    }

}


[Serializable]
public class WorkFlowSection
{
    public int? WorkFlowSectionID { get; set; }
    public int? WorkFlowID { get; set; }
    public int? WorkFlowSectionTypeID { get; set; }
    public string SectionName { get; set; }
    public int? Position { get; set; }
    public string JSONContent { get; set; }
    public int? ParentSectionID { get; set; }
    public int? ColumnIndex { get; set; }

    public WorkFlowSection()
    {

    }
    public WorkFlowSection(int? p_WorkFlowSectionID,
        int? p_WorkFlowID, int? p_WorkFlowSectionTypeID, string p_SectionName,
        int? p_Position, string p_JSONContent, int? p_ParentSectionID, int? p_ColumnIndex)
    {
        WorkFlowSectionID = p_WorkFlowSectionID;
        WorkFlowID = p_WorkFlowID;
        WorkFlowSectionTypeID = p_WorkFlowSectionTypeID;
        SectionName = p_SectionName;
        Position = p_Position;
        JSONContent = p_JSONContent;
        ParentSectionID = p_ParentSectionID;
        ColumnIndex = p_ColumnIndex;
    }

}




//[Serializable]
//public class EmailLog
//{
//    public int? EmailLogID { get; set; }
//    public int? AccountID { get; set; }
//    public string EmailSubject { get; set; }
//    public string EmailTo { get; set; }
//    public DateTime? EmailDate { get; set; }
//    public int? TableID { get; set; }
//    public int? RecordID { get; set; }
//    public string EmailType { get; set; }
//    public string RawMessage { get; set; }
//    public string EmailUID { get; set; }

//    public EmailLog()
//    {

//    }
//    public EmailLog(int? p_EmailLogID,
//        int? p_AccountID, string p_EmailSubject, string p_EmailTo,
//        DateTime? p_EmailDate, int? p_TableID, int? p_RecordID, string p_EmailType, string p_RawMessage)
//    {
//        EmailLogID = p_EmailLogID;
//        AccountID = p_AccountID;
//        EmailSubject = p_EmailSubject;
//        EmailTo = p_EmailTo;
//        EmailDate = p_EmailDate;
//        TableID = p_TableID;
//        RecordID = p_RecordID;
//        EmailType = p_EmailType;
//        RawMessage = p_RawMessage;
//    }

//}



[Serializable]
public class ScheduledTask
{
    public int? ScheduledTaskID { get; set; }
    public int? AccountID { get; set; }
    public int? TableID { get; set; }
    public string Frequency { get; set; }
    public string FrequencyWhen { get; set; }
    public string ScheduleType { get; set; }
    public int? MessageID { get; set; }
    public DateTime? RecordDateAdded { get; set; }
    public DateTime? LastEmailSentDate { get; set; }
  
    public ScheduledTask()
    {

    }
    public ScheduledTask(int? p_ScheduledTaskID,
        int? p_AccountID, int? p_TableID, string p_Frequency, string p_FrequencyWhen,
        string p_ScheduleType, int? p_EmailLogID)
    {
        ScheduledTaskID = p_ScheduledTaskID;
        AccountID = p_AccountID;
        TableID = p_TableID;
        Frequency = p_Frequency;
        FrequencyWhen = p_FrequencyWhen;
        ScheduleType = p_ScheduleType;
        MessageID = p_EmailLogID;
    }

}



[Serializable]
public class DataRetriever
{
    public int? DataRetrieverID { get; set; }
    public int? TableID { get; set; }
    public string SPName { get; set; }
    public string CodeSnippet { get; set; }
    public string DataRetrieverName { get; set; }
    //
    public DataRetriever()
    {

    }
    public DataRetriever(int? p_DataRetrieverID,
        int? p_TableID, string p_SPName, string p_DataRetrieverName)
    {
        DataRetrieverID = p_DataRetrieverID;
        TableID = p_TableID;
        SPName = p_SPName;
        DataRetrieverName = p_DataRetrieverName;
    }

}


[Serializable]
public class DocTemplate
{
    public int? DocTemplateID { get; set; }
    public string FileUniqueName { get; set; }
    public string FileName { get; set; }
    public int? DataRetrieverID { get; set; }
  
    public DocTemplate()
    {

    }
    public DocTemplate(int? p_DocTemplateID,
        string p_FileUniqueName, string p_FileName, int? p_DataRetrieverID)
    {
        DocTemplateID = p_DocTemplateID;
        FileUniqueName = p_FileUniqueName;
        FileName = p_FileName;
        DataRetrieverID = p_DataRetrieverID;
       
    }

}



[Serializable]
public class Form
{
    public int? FormID { get; set; }
    public int? TableID { get; set; }
    public string FormName { get; set; }
    public string PermissionType { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public string StyleSheet { get; set; }

    public Form()
    {

    }
    public Form(int? p_FormID,
        int? p_TableID, string p_FormName, string p_PermissionType,
        DateTime? p_ExpiryDate, string p_StyleSheet)
    {
        FormID = p_FormID;
        TableID = p_TableID;
        FormName = p_FormName;
        PermissionType = p_PermissionType;
        ExpiryDate = p_ExpiryDate;
        StyleSheet = p_StyleSheet;
    }

}




[Serializable]
public class Layout
{
    //JSONContent

    public int? LayoutID { get; set; }
    public int? FormID { get; set; }
    public int? ColumnID { get; set; }
    public int? DisplayOrder { get; set; }
    public int? DisplayColumn { get; set; }
    public string LayoutHTML { get; set; }
    public int? ParentLayoutID { get; set; }
    public string JSONContent { get; set; }
    public Layout()
    {

    }
    public Layout(int? p_LayoutID,
        int? p_FormID, int? p_ColumnID, int? p_DisplayOrder,
        int? p_DisplayColumn, string p_LayoutHTML, int? p_ParentLayoutID)
    {
        LayoutID = p_LayoutID;
        FormID = p_FormID;
        ColumnID = p_ColumnID;
        DisplayOrder = p_DisplayOrder;
        DisplayColumn = p_DisplayColumn;
        LayoutHTML = p_LayoutHTML;
        ParentLayoutID = p_ParentLayoutID;

    }

}



[Serializable]
public class FormSet
{
    public int? FormSetID { get; set; }
    public int? FormSetGroupID { get; set; }
    public int? RowPosition { get; set; }
    public string FormSetName { get; set; }
    public bool? ShowOnAdd { get; set; }

    public FormSet()
    {

    }
    public FormSet(int? p_FormSetID,
        int? p_FormSetGroupID, int? p_RowPosition, string p_FormSetName)
    {
        FormSetID = p_FormSetID;
        FormSetGroupID = p_FormSetGroupID;
        RowPosition = p_RowPosition;
        FormSetName = p_FormSetName;     
    }

}




[Serializable]
public class FormSetForm
{
     public int? FormSetFormID { get; set; }
    public int? FormSetID { get; set; }
    public int? TableID { get; set; }
    public int? DisplayOrder { get; set; }
    public int? UpdateColumnID { get; set; }
    public string UpdateColumnValue { get; set; }

    public string IncompleteImage { get; set; }

    public FormSetForm()
    {

    }
    public FormSetForm(int? p_FormSetFormID, int? p_FormSetID,int? p_TableID,
        int? p_DisplayOrder, int? p_UpdateColumnID, string p_UpdateColumnValue)
    {
        FormSetFormID = p_FormSetFormID;
        FormSetID = p_FormSetID;
        TableID = p_TableID;
        DisplayOrder = p_DisplayOrder;
        UpdateColumnID = p_UpdateColumnID;
        UpdateColumnValue = p_UpdateColumnValue;      
    }

}




[Serializable]
public class FormSetGroup
{
    public int? FormSetGroupID { get; set; }
    public string FormSetGroupName { get; set; }
    public int? ColumnPosition { get; set; }
    public int? ParentTableID { get; set; }
    public bool? Sequential { get; set; }

    public int? HideColumnID { get; set; }
    public string HideColumnValue { get; set; }
    public string HideOperator { get; set; }

    public FormSetGroup()
    {

    }
    public FormSetGroup(int? p_FormSetGroupID,
        string p_FormSetGroupName, int? p_ColumnPosition, int? p_ParentTableID, bool? p_Sequential)
    {
        FormSetGroupID = p_FormSetGroupID;
        FormSetGroupName = p_FormSetGroupName;
        ColumnPosition = p_ColumnPosition;
        ParentTableID = p_ParentTableID;
        Sequential = p_Sequential;      
    }

}



[Serializable]
public class FormSetProgress
{
    public int? FormSetProgressID { get; set; }
    public int? RecordID { get; set; }
    public int? FormSetID { get; set; }
    public int? FormSetFormID { get; set; }
    public bool? Completed { get; set; }

    public FormSetProgress()
    {

    }
    public FormSetProgress(int? p_FormSetProgressID,
        int? p_RecordID, int? p_FormSetID, int? p_FormSetFormID, bool? p_Completed)
    {
        FormSetProgressID = p_FormSetProgressID;
        RecordID = p_RecordID;
        FormSetID = p_FormSetID;
        FormSetFormID = p_FormSetFormID;
        Completed = p_Completed;
    }

}




[Serializable]
public class ShowWhen
{
    public int? ShowWhenID { get; set; }
    public int? ColumnID { get; set; }
    public int? DocumentSectionID { get; set; }
    public int? TableTabID { get; set; }
    public string Context { get; set; }
    public int? HideColumnID { get; set; }
    public string HideColumnValue { get; set; }
    public string HideOperator { get; set; }
    public int? DisplayOrder { get; set; }
    public string JoinOperator { get; set; }
    
    public ShowWhen()
    {

    }
    public ShowWhen(int? p_ShowWhenID, int? p_ColumnID, int? p_HideColumnID, string p_HideColumnValue,
        string p_HideOperator, int? p_DisplayOrder, string p_JoinOperator)
    {
        ShowWhenID = p_ShowWhenID;
        ColumnID = p_ColumnID;
        HideColumnID = p_HideColumnID;
        HideColumnValue = p_HideColumnValue;
        HideOperator = p_HideOperator;
        DisplayOrder = p_DisplayOrder;
        JoinOperator = p_JoinOperator;

    }

}





[Serializable]
public class TableTab
{
    public int? TableTabID { get; set; }
    public int? TableID { get; set; }
    public string TabName { get; set; }
    public int? DisplayOrder { get; set; }

    public TableTab()
    {

    }
    public TableTab(int? p_TableTabID, int? p_TableID, string p_TabName,
         int? p_DisplayOrder)
    {
        TableTabID = p_TableTabID;
        TableID = p_TableID;
        TabName = p_TabName;
        DisplayOrder = p_DisplayOrder;
    }

}



[Serializable]
public class View
{

    public int? ViewID { get; set; }
    public int? TableID { get; set; }
    public string  ViewName { get; set; }
    public int? UserID { get; set; }
    public int? RowsPerPage { get; set; }

    public string SortOrder { get; set; }
    public string Filter { get; set; }

    public bool? ShowSearchFields { get; set; }
    public bool? ShowAddIcon { get; set; }
    public bool? ShowEditIcon { get; set; }
    public bool? ShowDeleteIcon { get; set; }
    public bool? ShowViewIcon { get; set; }
    public bool? ShowBulkUpdateIcon { get; set; }
    public string FilterControlsInfo { get; set; }
    public string ViewPageType { get; set; }
    public string FixedFilter { get; set; }
    public bool? ShowFixedHeader { get; set; }
    public int? ParentTableID { get; set; }
    public View()
    {

    }
    public View(int? iViewID, int? iTableID, string strViewName,
        int? iUserID, int? iRowsPerPage, string strSortOrder, string strFilter, bool? bShowSearchFields,
        bool? bShowAddIcon, bool? bShowEditIcon, bool? bShowDeleteIcon, bool? bShowViewIcon, bool? bShowBulkUpdateIcon)
    {
        ViewID = iViewID;
        TableID = iTableID;
        ViewName = strViewName;
        UserID = iUserID;
        RowsPerPage = iRowsPerPage;
        SortOrder = strSortOrder;
        Filter = strFilter;
        ShowSearchFields = bShowSearchFields;
        ShowAddIcon = bShowAddIcon;
        ShowEditIcon = bShowEditIcon;
        ShowDeleteIcon = bShowDeleteIcon;
        ShowViewIcon = bShowViewIcon;
        ShowBulkUpdateIcon = bShowBulkUpdateIcon;
    }

}






[Serializable]
public class ViewItem
{

    public int? ViewItemID { get; set; }
    public int? ViewID { get; set; }
    public int? ColumnID { get; set; }
    //public string Heading { get; set; }
    public bool? SearchField { get; set; }

    public bool? FilterField { get; set; }
    public string Alignment { get; set; }

    public int? Width { get; set; }
    public bool? ShowTotal { get; set; }
    public string SummaryCellBackColor { get; set; }
    public int? ColumnIndex { get; set; }

    public ViewItem(int? iViewItemID, int? iViewID, int? iColumnID, 
        bool? bSearchField, bool? bFilterField, string strAlignment, int? iWidth,bool? bShowTotal, string strSummaryCellBackColor)
    {
        ViewItemID = iViewItemID;
        ViewID = iViewID;
        ColumnID = iColumnID;
        //Heading = strHeading;
        SearchField = bSearchField;
        FilterField = bFilterField;
        Alignment = strAlignment;
        Width = iWidth;
        ShowTotal = bShowTotal;
        SummaryCellBackColor = strSummaryCellBackColor;        
    }

}






[Serializable]
public class ExportTemplate
{
    public int? ExportTemplateID { get; set; }
    public int? TableID { get; set; }
    public string ExportTemplateName { get; set; }

    public ExportTemplate()
    {

    }
    public ExportTemplate(int? p_ExportTemplateID, int? p_TableID, string p_ExportTemplateName)
    {
        ExportTemplateID = p_ExportTemplateID;
        TableID = p_TableID;
        ExportTemplateName = p_ExportTemplateName;
    }

}

[Serializable]
public class ExportTemplateItem
{
    public int? ExportTemplateItemID { get; set; }   
    public int? ExportTemplateID { get; set; }
    public int? ColumnID { get; set; }
    public string ExportHeaderName { get; set; }
    public int? ColumnIndex { get; set; }

    public ExportTemplateItem()
    {

    }
    public ExportTemplateItem(int? p_ExportTemplateItemID, int? p_ExportTemplateID, int? p_ColumnID,
        string p_ExportHeaderName, int? p_ColumnIndex)
    {
        ExportTemplateItemID = p_ExportTemplateItemID;
        ExportTemplateID = p_ExportTemplateID;
        ColumnID = p_ColumnID;
        ExportHeaderName = p_ExportHeaderName;
        ColumnIndex = p_ColumnIndex;
    }

}





[Serializable]
public class SpeedLog
{

    public int? SpeedLogID { get; set; }
    public int? UserID { get; set; }
    public string PageUrl { get; set; }
    public string FunctionName { get; set; }
    public int? FunctionLineNumber { get; set; }

    public DateTime? Runtime { get; set; }
    public string SessionID { get; set; }

    public SpeedLog()
    {

    }

    public SpeedLog(int? iSpeedLogID, int? iUserID, string strPageUrl,
         string strFunctionName, int? iFunctionLineNumber, DateTime? dtRuntime,
        string strSessionID)
    {
        SpeedLogID = iSpeedLogID;
        UserID = iUserID;
        PageUrl = strPageUrl;
        UserID = iUserID;
        FunctionName = strFunctionName;
        FunctionLineNumber = iFunctionLineNumber;
        Runtime = dtRuntime;
        SessionID = strSessionID;
        
    }

}




[Serializable]
public class ExportClass
{
   
    public string strRecords { get; set; }
    public string strExportFiletype { get; set; }

    public List<IDnText> objCheckBoxList { get; set; }
    public ExportClass()
    {

    }


}


[Serializable]
public class XMLData
{
    public int? XMLDataID { get; set; }
    public string XMLText { get; set; }
    public int? SearchCriteriaID { get; set; }

    public XMLData()
    {

    }

    public XMLData(int? iXMLDataID, string strXMLText, int? iSearchCriteriaID)
    {
        XMLDataID = iXMLDataID;
        XMLText = strXMLText;
        SearchCriteriaID = iSearchCriteriaID;
    }

}

[Serializable]
public class UserSearch
{
    public int? UserSearchID { get; set; }
    public int? UserID { get; set; }
    public int? ViewID { get; set; }
    public string SearchXML { get; set; }
    public DateTime? DateUpdated { get; set; }

    public UserSearch()
    {

    }

    public UserSearch(int? iUserSearchID, int? iUserID, int? iViewID, string sSearchXML, DateTime? dDateUpdated)
    {
        UserSearchID = iUserSearchID;
        UserID = iUserID;
        ViewID = iViewID;
        SearchXML = sSearchXML;
        DateUpdated = dDateUpdated;
    }

}


[Serializable]
public class ContentType
{
    public int? ContentTypeID { get; set; }
    public string ContentTypeKey { get; set; }
    public string ContentTypeName { get; set; }

    public ContentType()
    {

    }

    public ContentType(int? iContentTypeID, string strContentTypeKey, string strContentTypeName)
    {
        ContentTypeID = iContentTypeID;
        ContentTypeKey = strContentTypeKey;
        ContentTypeName = strContentTypeName;
    }

}



[Serializable]
public class ImportTemplate
{
    public int? ImportTemplateID { get; set; }
    public int? TableID { get; set; }
    public string ImportTemplateName { get; set; }
    public string HelpText { get; set; }
    public string TemplateUniqueFileName { get; set; }
    public string FileFormat { get; set; }
    public string SPName { get; set; }
    public string Notes { get; set; }

    public int? ImportDataStartRow { get; set; }
    public int? ImportColumnHeaderRow { get; set; }
    public int? TempImportColumnHeaderRow { get; set; }
    public int? TempImportDataStartRow { get; set; }
    public bool? IsImportPositional { get; set; }
    public string SPAfterImport { get; set; }

    public ImportTemplate()
    {

    }
    public ImportTemplate(int? p_ImportTemplateID, int? p_TableID, string p_ImportTemplateName, string p_HelpText,
        string p_TemplateUniqueFileName, string p_FileFormat, string p_SPName, string p_Notes)
    {
        ImportTemplateID = p_ImportTemplateID;
        TableID = p_TableID;
        ImportTemplateName = p_ImportTemplateName;
        HelpText = p_HelpText;
        TemplateUniqueFileName = p_TemplateUniqueFileName;
        FileFormat = p_FileFormat;
        SPName = p_SPName;
        Notes = p_Notes;
    }

}

[Serializable]
public class ImportTemplateItem
{
    public int? ImportTemplateItemID { get; set; }
    public int? ImportTemplateID { get; set; }
    public int? ColumnID { get; set; }
    public string ImportHeaderName { get; set; }
    public int? ColumnIndex { get; set; }
    public int? ParentImportColumnID { get; set; }
    public string PositionOnImport { get; set; }
    //public bool? IsDateSingleColumn { get; set; }
    public ImportTemplateItem()
    {

    }
    public ImportTemplateItem(int? p_ImportTemplateItemID, int? p_ImportTemplateID, int? p_ColumnID,
        string p_ImportHeaderName, int? p_ColumnIndex)
    {
        ImportTemplateItemID = p_ImportTemplateItemID;
        ImportTemplateID = p_ImportTemplateID;
        ColumnID = p_ColumnID;
        ImportHeaderName = p_ImportHeaderName;
        ColumnIndex = p_ColumnIndex;
    }

}




[Serializable]
public class ColumnColour
{
    public int? ColumnColourID { get; set; }
    public int? ID { get; set; }
    public int? ControllingColumnID { get; set; }
    public string Operator { get; set; }
    public string Value { get; set; }
    public string Colour { get; set; }
    public string Context { get; set; }
    public ColumnColour()
    {

    }
    public ColumnColour(int? p_ColumnColourID,string p_Context,  int? p_ID,int p_ControllingColumnID,
        string p_Operator, string p_Value, string p_Colour)
    {
        ColumnColourID = p_ColumnColourID;
        Context = p_Context;
        ID = p_ID;
        ControllingColumnID = p_ControllingColumnID;
        Operator = p_Operator;
        Value = p_Value;
        Colour = p_Colour;
    }

}


[Serializable]
public class Message
{
    public int? MessageID { get; set; }
    public int? RecordID { get; set; }
    public int? TableID { get; set; }
    public int? AccountID { get; set; }
    public DateTime? DateTimeP { get; set; }
    public string MessageType { get; set; }
    public string DeliveryMethod { get; set; }
    public bool? IsIncoming { get; set; }
    public string OtherParty { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
    public string Link { get; set; }
    public string ExternalMessageKey { get; set; }
    public Message()
    {

    }
    public Message(int? p_MessageID, int? p_RecordID, int? p_TableID, int? p_AccountID, DateTime? p_DateTimeP,
        string p_MessageType, string p_DeliveryMethod, bool? p_IsIncoming, string p_OtherParty, string p_Subject,
        string p_Body, string p_Link, string p_ExternalMessageKey)
    {
        MessageID = p_MessageID;
        RecordID = p_RecordID;
        TableID = p_TableID;
        AccountID = p_AccountID;
        DateTimeP = p_DateTimeP;
        MessageType = p_MessageType;
        DeliveryMethod = p_DeliveryMethod;
        IsIncoming = p_IsIncoming;
        OtherParty = p_OtherParty;
        Subject = p_Subject;
        Body = p_Body;
        Link = p_Link;
        ExternalMessageKey = p_ExternalMessageKey;       
    }

}




[Serializable]
public class OfflineTask
{
    public int? OfflineTaskID { get; set; }
    public int? AccountID { get; set; }

    public int? AddedByUserID { get; set; }
    public byte? Priority { get; set; }
    public string Processtorun { get; set; }
    public string Parameters { get; set; }
    public int? RepeatMins { get; set; }
    public DateTime? ScheduledToRun { get; set; }
    public DateTime? ActuallyRun { get; set; }
    public DateTime? DateAdded { get; set; }
    
    public OfflineTask()
    {

    }
    public OfflineTask(int? p_OfflineTaskID, int? p_AccountID, int? p_AddedByUserID,
        byte? p_Priority, string p_Processtorun,
        string p_Parameters, int? p_RepeatMins, DateTime? p_ScheduledToRun,
        DateTime? p_ActuallyRun, DateTime? p_DateAdded)
    {
        OfflineTaskID = p_OfflineTaskID;
        AccountID = p_AccountID;
        AddedByUserID = p_AddedByUserID;
        Priority = p_Priority;
        Processtorun = p_Processtorun;
        Parameters = p_Parameters;
        RepeatMins = p_RepeatMins;
        ScheduledToRun = p_ScheduledToRun;
        ActuallyRun = p_ActuallyRun;
        DateAdded = p_DateAdded;
    }

}


[Serializable]
public class OfflineTaskLog
{
    public int? OfflineTaskLogID { get; set; }
    public int? OfflineTaskID { get; set; }
    public string Result { get; set; }
    public DateTime? DateAdded { get; set; }
    public OfflineTaskLog()
    {

    }
    public OfflineTaskLog(int? p_OfflineTaskLogID, int? p_OfflineTaskID,
        string p_Result, DateTime? p_DateAdded)
    {
        OfflineTaskLogID = p_OfflineTaskLogID;
        OfflineTaskID = p_OfflineTaskID;
        Result = p_Result;
        DateAdded = p_DateAdded;
    }

}



[Serializable]
public class Condition
{
    public int? ConditionID { get; set; }
    public int? ColumnID { get; set; }
    public string ConditionType { get; set; }
    public int? CheckColumnID { get; set; }
 
    public string CheckFormula { get; set; }
    public string CheckValue { get; set; }

    public Condition()
    {

    }
    public Condition(int? p_ConditionID,  int? p_ColumnID,
        string p_ConditionType, int? p_CheckColumnID, string p_CheckFormula, string p_CheckValue)
    {
        ConditionID = p_ConditionID;
        ColumnID = p_ColumnID;
        ConditionType = p_ConditionType;
        CheckColumnID = p_CheckColumnID;
        CheckFormula = p_CheckFormula;
        CheckValue = p_CheckValue;
    }

}

//[Serializable]
//public class RoleGroup
//{

//    public int? RoleGroupID { get; set; }
//    public string RoleGroupName { get; set; }
//    public int? AccountID { get; set; }

//    public int? OwnerUserID { get; set; }

//    public RoleGroup()
//    {

//    }

//    public RoleGroup(int? iRoleGroupID, string strRoleGroupName,
//         int? iAccountID, int? iOwnerUserID)
//    {
//        RoleGroupID = iRoleGroupID;
//        RoleGroupName = strRoleGroupName;
//        AccountID = iAccountID;
//        OwnerUserID = iOwnerUserID;
       

//    }

//}




//[Serializable]
//public class RoleGroupTable
//{

//    public int? RoleGroupTableID { get; set; }
//    public int? RoleGroupID { get; set; }
//    public int? RoleType { get; set; }
//    public int? TableID { get; set; }
//    public bool? CanExport { get; set; }

//    public RoleGroupTable()
//    {

//    }

//    public RoleGroupTable(int? iRoleGroupTableID, int? iRoleGroupID, int? iRecordRightID, int? iTableID,
//        bool? bCanExport)
//    {
//        RoleGroupTableID = iRoleGroupTableID;
//        RoleGroupID = iRoleGroupID;
//        RoleType = iRecordRightID;
//        TableID = iTableID;
//        CanExport = bCanExport;

//    }

//}



public class LocationColumn : JSONField
{
    public string Address { get; set; }
    public string Latitude { get; set; }
    public string Longitude { get; set; }
    public int? ZoomLevel { get; set; }
}


public class SliderField : JSONField
{
    public int? Min { get; set; }
    public int? Max { get; set; }
}



public class AttachmentSetting : JSONField
{
    public bool? AttachOutgoingEmails { get; set; }
    public int? OutSavetoTableID { get; set; }
    public int? OutSaveRecipientColumnID { get; set; }
    public int? OutSaveSubjectColumnID { get; set; }
    public int? OutSaveBodyColumnID { get; set; }
    public bool? AttachIncomingEmails { get; set; }
    public int? InIdentifierColumnID { get; set; }
    public int? InSavetoTableID { get; set; }   
    public int? InSaveSubjectColumnID { get; set; }
    public int? InSaveEmailColumnID { get; set; }
    public int? InSaveSenderColumnID { get; set; }
    public int? InSaveAttachmentColumnID { get; set; }

}



public class AttachmentPOP3 : JSONField
{
    public string Email { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public bool? SSL { get; set; }
    public int? Port { get; set; }
    public string POP3Server { get; set; }
}

public static class TextTypeRegEx
{
   
    public static string text = @"^[a-zA-Z]*";
    public static string email=@"^([a-zA-Z0-9_\-\.])+@(([0-2]?[0-5]?[0-5]\.[0-2]?[0-5]?[0-5]\.[0-2]?[0-5]?[0-5]\.[0-2]?[0-5]?[0-5])|((([a-zA-Z0-9\-])+\.)+([a-zA-Z\-])+))$";
    public static string link = @"^((ht|f)tp(s?)\:\/\/)?(([a-zA-Z0-9\-\._]+(\.[a-zA-Z0-9\-\._]+)+)|localhost)(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&amp;%\$#_]*)?([\d\w\.\/\%\+\-\=\&amp;\?\:\\\&quot;\'\,\|\~\;]*)$";
        
    //@"^(ht|f)tp(s?)\:\/\/(([a-zA-Z0-9\-\._]+(\.[a-zA-Z0-9\-\._]+)+)|localhost)(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&amp;%\$#_]*)?([\d\w\.\/\%\+\-\=\&amp;\?\:\\\&quot;\'\,\|\~\;]*)$";
    //@"^http(s?)\:\/\/[0-9a-zA-Z]([.\w][0-9a-zA-Z])(:(0-9))(\/?)([a-zA-Z0-9\\.\?\,\'\/\\\+&amp;%\$#_])?$";
    
    public static string isbn = @"ISBN(-1(?:(0)|3))?:?\x20+(?(1)(?(2)(?:(?=.{13}$)\d{1,5}([ -])\d{1,7}\3\d{1,6}\3(?:\d|x)$)|(?:(?=.{17}$)97(?:8|9)([ -])\d{1,5}\4\d{1,7}\4\d{1,6}\4\d$))|(?(.{13}$)(?:\d{1,5}([ -])\d{1,7}\5\d{1,6}\5(?:\d|x)$)|(?:(?=.{17}$)97(?:8|9)([ -])\d{1,5}\6\d{1,7}\6\d{1,6}\6\d$)))";
    //public static string own;


}

[Serializable]
public class DynamasterConfig
{ 
    public int? TableID { get; set; }
    public int? Record_ID { get; set; }
    public string EMD { get; set; }
    public string EcotechSiteID { get; set; }
    public string NumberEventsToFetch { get; set; }
    public string ImportData { get; set; }
    public string OnlyImportValidShots { get; set; }
    public string LastCheckedOn { get; set; }
    public string LastDataImportedOn { get; set; }
    public string Password { get; set; }
    public string Username { get; set; }
    public System.DateTime? DateAdded { get; set; }
    public int? SampleTableID { get; set; }
    public DynamasterConfig()
    {

    }
    public DynamasterConfig(int? p_TableID, int? p_Record_ID, string p_EMD, string p_EcotechSiteID, 
        string p_NumberEventsToFetch, string p_ImportData, string p_OnlyImportValidShots, string p_LastCheckedOn, string p_LastDataImportedOn, string p_Password, string p_Username, System.DateTime? p_DateAdded)
    {
        TableID = p_TableID;
        Record_ID = p_Record_ID;
        EMD = p_EMD;
        EcotechSiteID = p_EcotechSiteID;
        NumberEventsToFetch = p_NumberEventsToFetch;
        ImportData = p_ImportData;
        OnlyImportValidShots = p_OnlyImportValidShots;
        LastCheckedOn = p_LastCheckedOn;
        LastDataImportedOn = p_LastDataImportedOn;
        Password = p_Password;
        Username = p_Username;
        DateAdded = p_DateAdded;
    }

}




