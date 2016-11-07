using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Globalization;
using ChartDirector;
using System.Net.Mail;
using System.CodeDom.Compiler;
//using DocGen.DAL;
using System.IO;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using DocGen.DAL;
/// <summary>
/// Summary description for CustomMethod
/// </summary>
public class CustomMethod
{
	public CustomMethod()
	{
		//
		// TODO: Add constructor logic here
		//
	}


    public static List<object> DotNetMethod(string sMethodName, List<object> objList)
    {
        List<object> roList = new List<object>();
        switch (sMethodName.ToLower())
        {
            case "cs_test_method_join":

                roList = cs_test_method_join(objList);
                return roList;
            case "cs_test_method_EMPTY":

                roList = cs_test_method_EMPTY(objList);
                return roList;
        }

        return roList;
    }


    public static List<object> cs_test_method_join(List<object> objList)
    {
        List<object> roList = new List<object>();

        foreach(object obj in objList)
        {
            if(obj.GetType().Name=="Record")
            {
                Record theRecord = (Record)obj;
                if(theRecord!=null)
                {
                    theRecord.V002 = theRecord.V001 + " " + theRecord.V002;
                    
                    string xmlRecord=TheDatabase.RecordToXML(theRecord, null);
                    DataTable dtD = TheDatabase.XMLtoDataTableDetail(xmlRecord, theRecord.TableID);
                    roList.Add(dtD);
                    roList.Add("From C# cs_test_method_join METHOD.".ToString());
                    return roList;
                }
            }
        }

        return roList;
    }
    public static List<object> cs_test_method_EMPTY(List<object> objList)
    {
        List<object> roList = new List<object>();

        foreach (object obj in objList)
        {
            if (obj.GetType().Name == "Record")
            {
                Record theRecord = (Record)obj;
                if (theRecord != null)
                {
                    theRecord.V001 = "";
                    theRecord.V002 = "";
                    string xmlRecord = TheDatabase.RecordToXML(theRecord, null);
                    DataTable dtD = TheDatabase.XMLtoDataTableDetail(xmlRecord, theRecord.TableID);
                    roList.Add(dtD);
                    roList.Add("From C# cs_test_method_EMPTY METHOD.".ToString());
                    return roList;
                }
            }
        }

        return roList;
    }
}