using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

using System.IO;
using System.Text;
using System.Web;


/// <summary>
/// Summary description for webconfig
/// </summary>
public class Settings
{
    public Settings()
	{

	}

    // ------------------------------------------------------------------------------
    private static string sSystemPrefix = "";
    private static string sSystemName = "CnString";
    private static string sFileFolder = "files";
    // ------------------------------------------------------------------------------

    public static string SystemPrefix()
    {
        return sSystemPrefix;
    }

    public static string TableName(string sTableName)
    {
        return String.Concat(sSystemPrefix, sTableName);
    }

    public static string FileFolder()
    {
        return sFileFolder;
    }

    /// <summary>
    /// Convenient way to get the real stored procedure name. Pass the table (without system prefix) and the action 
    /// and the proper stored procedure name will be returned (with prefix)
    /// </summary>
    /// <param name="sTable">Table name (without system prefix) e.g. SystemOption</param>
    /// <param name="sAction">Acion: "Select", "Insert", "Update" or "Delete"</param>
    /// <returns></returns>
    public static string SPName(string sTable, string sAction)
    {
        return string.Concat(sSystemPrefix, sTable, sAction);
    }
    
    /// <summary>
    /// Convenient way to get the real stored procedure name. Pass the stored procedure name without the system prefix
    /// and the proper stored procedure name will be returned (with prefix)
    /// </summary>
    /// <param name="sSPName">Stored Procedure name (without system prefix)</param>
    /// <returns></returns>
    public static string SPName(string sRawSPName)
    {
        return string.Concat(sSystemPrefix, sRawSPName);
    }

    public static string SystemName()
    {
        return sSystemName;
    }

    public static string GetConnectionString()
    {
        //string sConnectionString = System.Configuration.ConfigurationSettings.AppSettings[sSystemName];
        string sConnectionString = DBGurus.strGlobalConnectionString;
        

        return sConnectionString;
    }

}
