using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

public partial class Test_DateAndTimePicker : System.Web.UI.UserControl
{
    private DateTime _currentDateTimeUtc;


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            DateFormatLiteral.Text = System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern;
            TimeFormatLiteral.Text = System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortTimePattern;

            // Populate TimeHour ListBox
            PopulateTimes();
            //writer.AddAttribute("onClick", "__timepicker_showpopup('" + listid + "')");
            DropDownImage.Attributes.Add("onClick", "__timepicker_showpopup('" + TimeHourHalfListBox.ClientID + "')");

            TimeHourHalfListBox.Attributes.Add("onfocusout", "__popup_losefocus(this);");
            TimeHourHalfListBox.Attributes.Add("onchange", "__timepicker_timechanged('" + TimeOfDayTextBox.ClientID + "',this);");

        }
    }


    public System.DateTime SelectedDateTime
    {
        get
        {
            return SelectedDate.Add(SelectedTime.TimeOfDay);
        }
        set
        {
            SelectedDate = value;
            SelectedTime = value;
        }
    }


    private System.DateTime SelectedDate
    {
        get
        {
            //EnsureChildControls();
            DateTime d = DateTime.UtcNow;
            try
            {
                d = DateTime.Parse(PostDateTextBox.Text.Trim());
                _currentDateTimeUtc = d;
            }
            catch
            {
                //Post Date must be formated as a date
                DateCompareValidator1.IsValid = false;
                return d;
            }
            return d;
        }
        set
        {
            //EnsureChildControls();
            PostDateTextBox.Text = value.ToShortDateString();
        }
    }


    private System.DateTime SelectedTime
    {
        get
        {
            //EnsureChildControls();
            System.DateTime d = DateTime.UtcNow;
            try
            {
                d = System.DateTime.Parse(TimeOfDayTextBox.Text);
            }
            catch
            {
                // Time must be formated as a time
                TimeFormatCustomValidator.IsValid = false;
                return d;
            }
            return d;
        }
        set
        {
            //EnsureChildControls();
            string s = value.ToString("h:mm tt");
            // Set the TextBox
            TimeOfDayTextBox.Text = s;
            ListItem item = TimeHourHalfListBox.Items.FindByText(s);
            if (item != null)
            {
                TimeHourHalfListBox.SelectedValue = s;
            }
        }
    }



    private void PopulateTimes()
    {
        if (TimeHourHalfListBox.Items.Count == 0)
        {
            TimeHourHalfListBox.Items.Add("12:00 AM");
            TimeHourHalfListBox.Items.Add("12:30 AM");
            for (int i = 1; i <= 11; i++)
            {
                TimeHourHalfListBox.Items.Add(i + ":00 AM");
                TimeHourHalfListBox.Items.Add(i + ":30 AM");
            }
            TimeHourHalfListBox.Items.Add("12:00 PM");
            TimeHourHalfListBox.Items.Add("12:30 PM");
            for (int i = 1; i <= 11; i++)
            {
                TimeHourHalfListBox.Items.Add(i + ":00 PM");
                TimeHourHalfListBox.Items.Add(i + ":30 PM");
            }

        }
    }



    protected void TimeFormatCustomValidator_ServerValidate(object source, ServerValidateEventArgs args)
    {

        System.DateTime d = DateTime.UtcNow;
        try
        {
            DateTime dateTime;
            if (System.DateTime.TryParse(TimeOfDayTextBox.Text, out dateTime) == false)
            {
                args.IsValid = false;
            }
        }
        catch
        {
            // Time must be formated as a time
            //TimeFormatCustomValidator.IsValid = false;
            args.IsValid = false;
        }

    }


    protected void Page_PreRender(object sender, EventArgs e)
    {

        commonScript.WritePopupRoutines(Page);

        StringBuilder sb = new StringBuilder();
        sb.AppendLine("function __timepicker_showpopup(name)");
        sb.AppendLine("{");
        sb.AppendLine(" if (__popup_panel != null)");
        sb.AppendLine(" {");
        sb.AppendLine(" document.getElementById(__popup_panel).style.display='none';");
        sb.AppendLine(" }");
        sb.AppendLine(" __popup_panel=name;");
        sb.AppendLine(" var panel=document.getElementById(__popup_panel);");
        sb.AppendLine(" panel.style.display='block';");
        sb.AppendLine(" panel.focus();");
        sb.AppendLine(" window.event.cancelBubble=true;");
        sb.AppendLine("}");

        sb.AppendLine("function __timepicker_timechanged(tbxid, selectid)");
        sb.AppendLine("{");
        sb.AppendLine("document.getElementById(tbxid).value=selectid.options[selectid.selectedIndex].text;");
        sb.AppendLine(" if (__popup_panel != null)");
        sb.AppendLine(" {");
        sb.AppendLine(" document.getElementById(__popup_panel).style.display='none';");
        sb.AppendLine(" __popup_panel=null;");
        sb.AppendLine(" }");
        sb.AppendLine("}");

        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "popup", sb.ToString(), true);
    }




    public static class commonScript
    {

        public static void WritePopupRoutines(System.Web.UI.Page Page)
        {
            StringBuilder sb = new StringBuilder();
            sb = new StringBuilder();
            sb.AppendLine("var __popup_panel;");

            sb.AppendLine("function __popup_clear() {");
            sb.AppendLine(" if (__popup_panel != null ) ");
            sb.AppendLine(" {");
            sb.AppendLine(" document.getElementById(__popup_panel).style.display='none';");
            sb.AppendLine(" __popup_panel=null;");
            sb.AppendLine(" }");
            sb.AppendLine("}");
            sb.AppendLine("function __popup_losefocus(panel)");
            sb.AppendLine("{");
            sb.AppendLine(" if (!panel.contains(document.activeElement))");
            sb.AppendLine(" {");
            sb.AppendLine(" panel.style.display='none';");
            sb.AppendLine(" }");
            sb.AppendLine("}");

            Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "PopupRoutines", sb.ToString(), true);
        }
    }
}
