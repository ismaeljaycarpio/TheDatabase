using System;
using System.Collections.Generic;
//using System.Linq;
using System.Web;
//using System.Data.Linq;

using System.Text;
using System.Web.UI.WebControls;
using System.ComponentModel;

/// <summary>
/// Summary description for SqlData_User
/// </summary>
public class SqlData_User
{
	public SqlData_User()
	{
	}
    //private static ASPTemplateDataContext srcETS = new ASPTemplateDataContext();
    //public static User UserByUserPass(string username, string password)
    //{
    //    return SecurityManager.GetUserByUserPass(username, password);
    //    //return (from u in srcETS.GetTable<User>()
    //    //        where ( u.UserName == username && u.Password == password && u.IsActive==true)
    //    //        select u).SingleOrDefault<User>();
    //}

    //public static User UserByUserID(int uid)
    //{
    //    return (from u in srcETS.GetTable<User>()
    //            where (u.UserID==uid)
    //            select u).SingleOrDefault<User>();
    //}

    //public static List<User> UserByEmail(string email)
    //{
    //    Func<ASPTemplateDataContext, IQueryable<User>> compiledQuery =
    //       CompiledQuery.Compile((ASPTemplateDataContext db) => from u in db.Users
    //                                                    where u.Email == email
    //                                                    select u);

    //    var metaNew = new ASPTemplateDataContext();
    //    IOrderedQueryable<User> query =
    //        (IOrderedQueryable<User>)
    //        compiledQuery(metaNew);
    //    List<User> newList = query.ToList();
    //    return newList;
    //}

    //public static List<UserRole> RolesByUserID(int uid)
    //{
    //    var q = from ur in srcETS.UserRoles
    //            where ur.UserID == uid
    //            select ur;

    //    return q.ToList();
    //}
}



namespace DBGWebUI
{

    //public class dbgGridView : GridView
    //{
    //    private const string _virtualCountItem = "bg_vitemCount";
    //    private const string _sortColumn = "bg_sortColumn";
    //    private const string _sortDirection = "bg_sortDirection";
    //    private const string _currentPageIndex = "bg_pageIndex";

    //    public dbgGridView()
    //        : base()
    //    {
    //    }

    //    #region Custom Properties
    //    [Browsable(true), Category("NewDynamic")]
    //    [Description("Set the virtual item count for this grid")]
    //    public int VirtualItemCount
    //    {
    //        get
    //        {
    //            if (ViewState[_virtualCountItem] == null)
    //                ViewState[_virtualCountItem] = -1;
    //            return Convert.ToInt32(ViewState[_virtualCountItem]);
    //        }
    //        set
    //        {
    //            ViewState[_virtualCountItem] = value;
    //        }
    //    }

    //    public string GridViewSortColumn
    //    {
    //        get
    //        {
    //            if (ViewState[_sortColumn] == null)
    //                ViewState[_sortColumn] = string.Empty;
    //            return ViewState[_sortColumn].ToString();
    //        }
    //        set
    //        {
    //            if (ViewState[_sortColumn] == null || !ViewState[_sortColumn].Equals(value))
    //                GridViewSortDirection = SortDirection.Ascending;
    //            ViewState[_sortColumn] = value;
    //        }
    //    }

    //    public SortDirection GridViewSortDirection
    //    {
    //        get
    //        {
    //            if (ViewState[_sortDirection] == null)
    //                ViewState[_sortDirection] = SortDirection.Ascending;
    //            return (SortDirection)ViewState[_sortDirection];
    //        }
    //        set
    //        {
    //            ViewState[_sortDirection] = value;
    //        }
    //    }

    //    private int CurrentPageIndex
    //    {
    //        get
    //        {
    //            if (ViewState[_currentPageIndex] == null)
    //                ViewState[_currentPageIndex] = 0;
    //            return Convert.ToInt32(ViewState[_currentPageIndex]);
    //        }
    //        set
    //        {
    //            ViewState[_currentPageIndex] = value;
    //        }
    //    }

    //    private bool CustomPaging
    //    {
    //        get { return (VirtualItemCount != -1); }
    //    }
    //    #endregion

    //    #region Overriding the parent methods
    //    public override object DataSource
    //    {
    //        get
    //        {
    //            return base.DataSource;
    //        }
    //        set
    //        {
    //            base.DataSource = value;
    //            // store the page index so we don't lose it in the databind event
    //            CurrentPageIndex = PageIndex;
    //        }
    //    }

    //    protected override void OnSorting(GridViewSortEventArgs e)
    //    {
    //        //Store the direction to find out if next sort should be asc or desc
    //        SortDirection direction = SortDirection.Ascending;
    //        if (ViewState[_sortColumn] != null && (SortDirection)ViewState[_sortDirection] == SortDirection.Ascending)
    //        {
    //            direction = SortDirection.Descending;
    //        }
    //        GridViewSortDirection = direction;
    //        GridViewSortColumn = e.SortExpression;
    //        base.OnSorting(e);
    //    }

    //    protected override void InitializePager(GridViewRow row, int columnSpan, PagedDataSource pagedDataSource)
    //    {
    //        // This method is called to initialise the pager on the grid. We intercepted this and override
    //        // the values of pagedDataSource to achieve the custom paging using the default pager supplied
    //        if (CustomPaging)
    //        {
    //            pagedDataSource.AllowCustomPaging = true;
    //            pagedDataSource.VirtualCount = VirtualItemCount;
    //            pagedDataSource.CurrentPageIndex = CurrentPageIndex;
    //        }
    //        base.InitializePager(row, columnSpan, pagedDataSource);
    //    }

    //    protected override object SaveViewState()
    //    {
    //        //object[] state = new object[3];
    //        //state[0] = base.SaveViewState();
    //        //state[1] = this.dirtyRows;
    //        //state[2] = this.newRows;
    //        //return state;

    //        return base.SaveViewState();
    //    }

    //    protected override void LoadViewState(object savedState)
    //    {

    //        //object[] state = null;

    //        //if (savedState != null)
    //        //{
    //        //    state = (object[])savedState;
    //        //    base.LoadViewState(state[0]);
    //        //    this.dirtyRows = (List<int>)state[1];
    //        //    this.newRows = (List<int>)state[2];
    //        //}

    //        base.LoadViewState(savedState);
    //    }
    //    #endregion

    //    public override string[] DataKeyNames
    //    {
    //        get
    //        {
    //            return base.DataKeyNames;
    //        }
    //        set
    //        {
    //            base.DataKeyNames = value;
    //        }
    //    }

    //    public override DataKeyArray DataKeys
    //    {
    //        get
    //        {
    //            return base.DataKeys;
    //        }
    //    }

    //    public override DataKey SelectedDataKey
    //    {
    //        get
    //        {
    //            return base.SelectedDataKey;
    //        }
    //    }
    //}
}