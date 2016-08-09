using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DocGen.DAL
{
    public class ImageSectionStyle : JSONField
    {
        public string Position { get; set; }
        public int Width { get; set; }
        public string OpenLink { get; set; }
    }

    public class CalendarSectionDetail : JSONField
    {
        public string CalendarTitle { get; set; }
        public int? TableID { get; set; }
        public int? DateFieldColumnID { get; set; }
        public string FieldDisplay { get; set; }
        public string FilterTextSearch { get; set; }
        public string FilterControlInfo { get; set; } //xml
        public string TextColourInfo { get; set; }//xml
        public bool? ShowAddRecordIcon { get; set; }
        public string CalendarDefaultView { get; set; }

        public int? Height { get; set; }
        public int? Width { get; set; }
    }

    public class MapSectionDetail : JSONField
    {
        public string Address { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public int? MapScale { get; set; }
        public int? ShowLocation { get; set; }
        public int? Height { get; set; }
        public int? Width { get; set; }
        public string MapTypeId { get; set; }
    }
    public class DialSectionDetail : JSONField
    {
        public int TableID{ get; set; }
        public int ColumnID { get; set; }
        public string Dial{ get; set; }
        public string  Label{ get; set; }
        public int? Scale { get; set; }
        public string Heading { get; set; }
        public int? Height { get; set; }
        public int? Width { get; set; }

    }

    public class RecordTableSectionDetail : JSONField
    {
        public int TableID { get; set; }
        //public int SearchCriteriaID { get; set; } 
        public int ViewID { get; set; }
    }

    [Serializable]
    public class OptionImage : JSONField
    {
        public string OptionImageID { get; set; }
        public string Value { get; set; }
        public string  FileName { get; set; }
        public string UniqueFileName { get; set; }
    }

    [Serializable]
     public class OptionImageList : JSONField
     {
         public List<OptionImage> ImageList { get; set; }

     }



    [Serializable]
    public class EachPinInfo : JSONField
    {
        public string lat { get; set; }
        public string lon { get; set; }
        public string title { get; set; }
        public string pin { get; set; }

        public string ssid { get; set; }
        public string url { get; set; }
        public string mappopup { get; set; }

    }
    [Serializable]
    public class ListPinInfo : JSONField
    {
        public List<EachPinInfo> PinList { get; set; }

    }

    [Serializable]
    public class ColumnButtonInfo : JSONField
    {
        public string SPToRun { get; set; }
        public string ImageFullPath { get; set; }
        public string WarningMessage { get; set; }

        public string OpenLink { get; set; }

    }
    [Serializable]
    public class ChartDashBoard : JSONField
    {
        public int RecentNumber { get; set; }
        public string RecentPeriod { get; set; }


    }


    [Serializable]
    public class OfflineTaskParameters : JSONField
    {
        public string ReturnSQL { get; set; }
        public string ReturnHeaderSQL { get; set; }
        public string UniqueFileName { get; set; }
        public string FileFriendlyName { get; set; }
        public int? TableID { get; set; }
        public string TableName { get; set; }
        public int TotalNumberOfRecords { get; set; }
    }
}