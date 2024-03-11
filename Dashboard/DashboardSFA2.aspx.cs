using DataAccess;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Dashboard_DashboardSFA2 : System.Web.UI.Page
{
    DataTable WidgetDt = new DataTable();
    Int32 userID = 0;
    public string strReferenceTypeId;

    protected void Page_Load(object sender, EventArgs e)
    { 
        if (Convert.ToString(BussinessLogic.PageBase.UserId) == "")
        {
            Response.Redirect(ConfigurationManager.AppSettings["siteurl"] + "/logout.aspx", false);
        }

        if (!IsPostBack)
        {
            if (Request.QueryString["userID"] != null && Request.QueryString["userID"] != string.Empty)
            {
                //userID = Convert.ToInt32(Cryptography.Crypto.Decrypt(Request.QueryString["userID"].ToString().Replace(" ", "+"), "testEncrpt").ToString());
                userID = Convert.ToInt32(Request.QueryString["userID"]);
            }
            else
            {
                userID = BussinessLogic.PageBase.UserId;
            }

            /*27-04-2023 Helper class changed start */
            GetDashBoardWidget();
            GetUserOrderListData();
            GetUserAttandanceDashboardList();
            GetUserLeaveDashboardList();
            GetUserTravelDistanceListData();
            GetUserTimeSpendInMarketListData();
            GetUserPropectListData();
            GetUserExpenseListData();
            //GetUserAverageTimeListData();
            GetTodayTeamAttandanceListData();
            GetUserSaleListData();
            GetUserTopSalesMenListData();
            GetUserBeatPlanListData();
            GetUserPaymentCollection();
            GetTopModelListData();
            GetTargetVsAchievementListData();
            GetRankingListData();
            GetTopDistributorListData(); 

            /*27-04-2023 Helper class changed end */
        }
        //SetAPiValues();

        DateTime dateTime = DateTime.UtcNow.Date;
        DateTime Yesterday = DateTime.UtcNow.Date.AddDays(-1);
        lblbusinessYesterday.Text = Yesterday.ToString("dd-MMM-yyyy");
        lblbusinessToday.Text = dateTime.ToString("dd-MMM-yyyy");

        var startDate = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);
        lblbusinessMTD.Text = startDate.ToString("dd-MMM-yyyy") + " to " + dateTime.ToString("dd-MMM-yyyy");

        lblattendanceYesterday.Text = Yesterday.ToString("dd-MMM-yyyy");
        lblattendanceToday.Text = dateTime.ToString("dd-MMM-yyyy");
        lblattendanceMTD.Text = startDate.ToString("dd-MMM-yyyy") + " to " + dateTime.ToString("dd-MMM-yyyy");

        lbltravelYesterday.Text = Yesterday.ToString("dd-MMM-yyyy");
        lbltravelToday.Text = dateTime.ToString("dd-MMM-yyyy");
        lbltravelMTD.Text = startDate.ToString("dd-MMM-yyyy") + " to " + dateTime.ToString("dd-MMM-yyyy");
    }

    private void GetDashBoardWidget()
    {
        ClsBusinessDashboard objdas = new ClsBusinessDashboard();
        objdas.UserID = userID;
        objdas.authKey = "";
        DataSet dsResult = objdas.GetDashBoardWidget();
        if (dsResult != null)
        {
            if (dsResult.Tables.Count > 0)
            {
                if (dsResult.Tables[0].Rows.Count > 0)
                {
                    WidgetDt = dsResult.Tables[1];
                    DataView view = new DataView(WidgetDt);
                    DataTable distinctValues = view.ToTable(true, "WidgetType", "Status");


                    //customtabsthreebusinesstab.Visible = false;
                    //customtabsthreebusiness.Visible = false;

                    DataRow[] businessRows = WidgetDt.Select("WidgetType = 'Business' AND VisibleStatus=1");
                    if (businessRows.Count() == 0)
                    {
                        //customtabsthreebusinesstab.Visible = false;
                        //customtabsthreebusiness.Visible = false;
                    }

                    DataRow[] attendanceRows = WidgetDt.Select("WidgetType = 'Attendance' AND VisibleStatus=1");
                    if (attendanceRows.Count() == 0)
                    {
                        //customtabsthreeattendancetab.Visible = false;
                        //customtabsthreeattendance.Visible = false;
                    }

                    DataRow[] travelRows = WidgetDt.Select("WidgetType = 'Travel' AND VisibleStatus=1");
                    if (travelRows.Count() == 0)
                    {
                        //customtabsthreetraveltab.Visible = false;
                        //customtabsthreetravel.Visible = false;
                    }
                }
            }
        }
    }

    private void GetUserOrderListData()
    {
        ClsBusinessDashboard objdas = new ClsBusinessDashboard();
        objdas.UserID = userID;
        objdas.authKey = "";
        
        DataSet dsResult = objdas.GetUserOrderListData();
        if (dsResult != null)
        {
            if (dsResult.Tables.Count > 0)
            {
                if (dsResult.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = dsResult.Tables[0];
                    lblYesterdayOrderQty.Text = (Math.Ceiling(Convert.ToDouble(dt.Rows[0]["YesterdayOrderQty"]))).ToString("#,##0");
                    lblYesterdayOrderAmount.Text = (Math.Ceiling(Convert.ToDouble(dt.Rows[0]["YesterdayOrderAmount"]))).ToString("#,##0");
                    lblMTDOrderQty.Text = (Math.Ceiling(Convert.ToDouble(dt.Rows[0]["MTDOrderQty"]))).ToString("#,##0");
                    lblMTDOrderAmount.Text = (Math.Ceiling(Convert.ToDouble(dt.Rows[0]["MTDOrderAmount"]))).ToString("#,##0");
                }
            }
        }

        bool VisibleStatus = getVisibleStatus("UserOrder");
        if (VisibleStatus == false)
        {
            YesterdayOrderwidget.Visible = false;
            MTDOrderwidget.Visible = false;
        }
    }

    private void GetUserAttandanceDashboardList()
    {
        ClsBusinessDashboard objdas = new ClsBusinessDashboard();
        objdas.UserID = userID;
        objdas.authKey = "";
        objdas.SelfOrTeam = 0;// self
        DataSet dsResult = objdas.GetUserAttandanceDashboardList();
        if (dsResult != null)
        {
            if (dsResult.Tables.Count > 0)
            {
                if (dsResult.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = dsResult.Tables[0];
                    lblPresentDays.Text = dt.Rows[0]["PresentDays"].ToString();
                    lblAbsentDays.Text = dt.Rows[0]["AbsentDays"].ToString();
                }
            }
        }

        objdas.SelfOrTeam = 1;// team
        DataSet dsResultTeam = objdas.GetUserAttandanceDashboardList();
        if (dsResultTeam != null)
        {
            if (dsResultTeam.Tables.Count > 0)
            {
                if (dsResultTeam.Tables[0].Rows.Count > 0)
                {
                    rptAttendanceDiv.Visible = true;
                    rptAttendanceNoData.Visible = false;

                    DataTable dt = dsResultTeam.Tables[0];
                    rptAttendance.DataSource = dt;
                    rptAttendance.DataBind();
                }
                else
                {
                    rptAttendanceDiv.Visible = false;
                    rptAttendanceNoData.Visible = true;
                }
            }
            else
            {
                rptAttendanceDiv.Visible = false;
                rptAttendanceNoData.Visible = true;
            }
        }

        bool VisibleStatus = getVisibleStatus("TodayAttendance");
        if (VisibleStatus == false)
        {
            AttendanceMTDwidget.Visible = false;
            rptAttendanceDiv.Visible = false;
            rptAttendanceNoData.Visible = false;
        }
    }

    private bool getVisibleStatus(string WidgetName)
    {
        if (WidgetDt.Rows.Count > 0)
        {
            DataRow[] businessRows = WidgetDt.Select("WidgetName = '" + WidgetName + "' AND VisibleStatus=1");
            if (businessRows.Count() == 0)
            {
                return false;
            }
            else
            {
                return true;
            }

        }
        else
        {
            return true;
        }
    }

    private void GetUserLeaveDashboardList()
    {
        ClsBusinessDashboard objdas = new ClsBusinessDashboard();
        objdas.UserID = userID;
        objdas.authKey = "";
        objdas.SelfOrTeam = 0;// self
        DataSet dsResult = objdas.GetUserLeaveDashboardList();
        if (dsResult != null)
        {
            if (dsResult.Tables.Count > 0)
            {
                if (dsResult.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = dsResult.Tables[0];
                   lblLeaveApproved.Text = dt.Rows[0]["Approved"].ToString();
                    lblLeavePending.Text = dt.Rows[0]["Pending"].ToString();
                    lblLeaveRejected.Text = dt.Rows[0]["Rejected"].ToString();
                }
            }
        }

        objdas.SelfOrTeam = 1;// team
        DataSet dsResultTeam = objdas.GetUserLeaveDashboardList();
        if (dsResultTeam != null)
        {
            if (dsResultTeam.Tables.Count > 0)
            {
                if (dsResultTeam.Tables[0].Rows.Count > 0)
                {
                    rptLeaveDiv.Visible = true;
                    rptLeaveNoData.Visible = false;

                    DataTable dt = dsResultTeam.Tables[0];
                    rptLeave.DataSource = dt;
                    rptLeave.DataBind();

                    GridView1.DataSource = dt;
                    GridView1.DataBind();
                }
                else
                {
                    rptLeaveDiv.Visible = false;
                    rptLeaveNoData.Visible = true;
                }
            }
            else
            {
                rptLeaveDiv.Visible = false;
                rptLeaveNoData.Visible = true;
            }
        }


        bool VisibleStatus = getVisibleStatus("MTDLeave");
        if (VisibleStatus == false)
        {
            leaveMTDwidget.Visible = false;
            rptLeaveDiv.Visible = false;
            rptLeaveNoData.Visible = false;
        }
    }

    private void GetUserTravelDistanceListData()
    {
        ClsBusinessDashboard objdas = new ClsBusinessDashboard();
        objdas.UserID = userID;
        objdas.authKey = "";
        objdas.SelfOrTeam = 0;// self
        DataSet dsResult = objdas.GetUserTravelDistanceListData();
        if (dsResult != null)
        {
            if (dsResult.Tables.Count > 0)
            {
                if (dsResult.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = dsResult.Tables[0];
                    lblYesterdayKm.Text = (Math.Round(Convert.ToDecimal(dt.Rows[0]["YesterdayKm"].ToString()))).ToString("#,##0");
                    lblMTDKM.Text = (Math.Round(Convert.ToDecimal(dt.Rows[0]["MTDKM"].ToString()))).ToString("#,##0");
                }
            }
        }

        objdas.SelfOrTeam = 1;// team
        DataSet dsResultTeam = objdas.GetUserTravelDistanceListData();
        if (dsResultTeam != null)
        {
            if (dsResultTeam.Tables.Count > 0)
            {
                if (dsResultTeam.Tables[0].Rows.Count > 0)
                {
                    rptDistanceTravelDiv.Visible = true;
                    rptDistanceTravelNoData.Visible = false;

                    DataTable dt = dsResultTeam.Tables[0];
                    rptDistanceTravel.DataSource = dt;
                    rptDistanceTravel.DataBind();
                }
                else
                {
                    rptDistanceTravelDiv.Visible = false;
                    rptDistanceTravelNoData.Visible = true;
                }
            }
            else
            {
                rptDistanceTravelDiv.Visible = false;
                rptDistanceTravelNoData.Visible = true;
            }
        }

        bool VisibleStatus = getVisibleStatus("DistanceTravelled");
        if (VisibleStatus == false)
        {
            DistanceTravelwidget.Visible = false;
            rptDistanceTravelDiv.Visible = false;
            rptDistanceTravelNoData.Visible = false;
        }
    }

    private void GetUserTimeSpendInMarketListData()
    {
        ClsBusinessDashboard objdas = new ClsBusinessDashboard();
        objdas.UserID = userID;
        objdas.authKey = "";
        objdas.SelfOrTeam = 0;// self
        DataSet dsResult = objdas.GetUserTimeSpendInMarketListData();
        if (dsResult != null)
        {
            if (dsResult.Tables.Count > 0)
            {
                if (dsResult.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = dsResult.Tables[0];
                    lblYesterdayTimeSpend.Text = (Math.Round(Convert.ToDecimal(dt.Rows[0]["YesterdayTimeSpend"].ToString()))).ToString();
                    lblMTDTimeSpend.Text = (Math.Round(Convert.ToDecimal(dt.Rows[0]["MTDTimeSpend"].ToString()))).ToString();
                }
                else
                {

                }
            }
            else
            {

            }
        }

        objdas.SelfOrTeam = 1;// team
        DataSet dsResultTeam = objdas.GetUserTimeSpendInMarketListData();
        if (dsResultTeam != null)
        {
            if (dsResultTeam.Tables.Count > 0)
            {
                if (dsResultTeam.Tables[0].Rows.Count > 0)
                {
                    rptTimeSpendDiv.Visible = true;
                    rptTimeSpendNoData.Visible = false;

                    DataTable dt = dsResultTeam.Tables[0];
                    rptTimeSpend.DataSource = dt;
                    rptTimeSpend.DataBind();
                }
                else
                {
                    rptTimeSpendDiv.Visible = false;
                    rptTimeSpendNoData.Visible = true;
                }
            }
            else
            {
                rptTimeSpendDiv.Visible = false;
                rptTimeSpendNoData.Visible = true;
            }
        }

        bool VisibleStatus = getVisibleStatus("TimeSpendInMarket");
        if (VisibleStatus == false)
        {
            TimespendMarketwidget.Visible = false;
            rptTimeSpendDiv.Visible = false;
            rptTimeSpendNoData.Visible = false;
        }
    }

    private void GetUserPropectListData()
    {
        ClsBusinessDashboard objdas = new ClsBusinessDashboard();
        objdas.UserID = userID;
        objdas.authKey = "";
        DataSet dsResult = objdas.GetUserPropectListData();
        if (dsResult != null)
        {
            if (dsResult.Tables.Count > 0)
            {
                if (dsResult.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = dsResult.Tables[0];
                    lblYesterdayProspect.Text = (Math.Round(Convert.ToDecimal(dt.Rows[0]["YesterdayProspect"].ToString()))).ToString();
                    lblMTDProspect.Text = (Math.Round(Convert.ToDecimal(dt.Rows[0]["MTDProspect"].ToString()))).ToString();
                }
            }
        }

        bool VisibleStatus = getVisibleStatus("UserProspect");
        if (VisibleStatus == false)
        {
            YesterdayProspectwidget.Visible = false;
            MTDProspectwidget.Visible = false;
        }
    }

    private void GetUserExpenseListData()
    {
        ClsBusinessDashboard objdas = new ClsBusinessDashboard();
        objdas.UserID = userID;
        objdas.authKey = "";
        // get self value
        objdas.SelfOrTeam = 0;
        DataSet dsResult = objdas.GetUserExpenseListData();
        if (dsResult != null)
        {
            if (dsResult.Tables.Count > 0)
            {
                if (dsResult.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = dsResult.Tables[0];
                    lblExpenseApproved.Text = dt.Rows[0]["Approved"].ToString();
                    lblExpensePending.Text = dt.Rows[0]["Pending"].ToString();
                    lblExpenseRejected.Text = dt.Rows[0]["Rejected"].ToString();
                }
            }
        }

        // get team value
        objdas.SelfOrTeam = 1;
        DataSet dsResulTeam = objdas.GetUserExpenseListData();
        if (dsResulTeam != null)
        {
            if (dsResulTeam.Tables.Count > 0)
            {
                if (dsResulTeam.Tables[0].Rows.Count > 0)
                {
                    rptExpenceDiv.Visible = true;
                    rptExpenceNoData.Visible = false;

                    DataTable dt = dsResulTeam.Tables[0];
                    rptExpence.DataSource = dt;
                    rptExpence.DataBind();
                }
                else
                {
                    rptExpenceDiv.Visible = false;
                    rptExpenceNoData.Visible = true;
                }
            }
            else
            {
                rptExpenceDiv.Visible = false;
                rptExpenceNoData.Visible = true;
            }
        }

        bool VisibleStatus = getVisibleStatus("Expense");
        if (VisibleStatus == false)
        {
            Expensewidget.Visible = false;
            rptExpenceDiv.Visible = false;
            rptExpenceNoData.Visible = false;
        }
    }

    //private void GetUserAverageTimeListData()
    //{
    //    ClsBusinessDashboard objdas = new ClsBusinessDashboard();
    //    objdas.UserID = userID;
    //    objdas.authKey = "";
    //    objdas.SelfOrTeam = 0;// self
    //    DataSet dsResult = objdas.GetUserAverageTimeListData();
    //    if (dsResult != null)
    //    {
    //        if (dsResult.Tables.Count > 0)
    //        {
    //            if (dsResult.Tables[0].Rows.Count > 0)
    //            {
    //                DataTable dt = dsResult.Tables[0];
    //                //lblYesterdayAverageTime.Text = (Math.Round(Convert.ToDecimal(dt.Rows[0]["YesterdayAverageTime"].ToString()))).ToString();
    //                //lblMTDAverageTime.Text = (Math.Round(Convert.ToDecimal(dt.Rows[0]["MTDAverageTime"].ToString()))).ToString();
    //            }
    //        }
    //    }

    //    objdas.SelfOrTeam = 1;// team
    //    DataSet dsResultTeam = objdas.GetUserAverageTimeListData();
    //    if (dsResultTeam != null)
    //    {
    //        if (dsResultTeam.Tables.Count > 0)
    //        {
    //            if (dsResultTeam.Tables[0].Rows.Count > 0)
    //            {
    //                rptAverageTimeDiv.Visible = true;
    //                rptAverageTimeNoData.Visible = false;

    //                DataTable dt = dsResultTeam.Tables[0];
    //                rptAverageTime.DataSource = dt;
    //                rptAverageTime.DataBind();
    //            }
    //            else
    //            {
    //                rptAverageTimeDiv.Visible = false;
    //                rptAverageTimeNoData.Visible = true;
    //            }
    //        }
    //        else
    //        {
    //            rptAverageTimeDiv.Visible = false;
    //            rptAverageTimeNoData.Visible = true;
    //        }
    //    }

    //    bool VisibleStatus = getVisibleStatus("AverageTimeInMarket");
    //    if (VisibleStatus == false)
    //    {
    //        //AverageTimeMarketwidget.Visible = false;
    //        rptAverageTimeDiv.Visible = false;
    //        rptAverageTimeNoData.Visible = false;
    //    }
    //}

    private void GetTodayTeamAttandanceListData()
    {
        ClsBusinessDashboard objdas = new ClsBusinessDashboard();
        objdas.UserID = userID;
        objdas.authKey = "";
        objdas.SelfOrTeam = 0;// self
        DataSet dsResult = objdas.GetTodayTeamAttandanceListData();
        if (dsResult != null)
        {
            if (dsResult.Tables.Count > 0)
            {
                if (dsResult.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = dsResult.Tables[0];
                    if (dt.Rows[0]["OnTime"].ToString() == "No")
                    {
                        lblOnTime.Visible = false;
                        ontime.Style.Add("display", "none");
                    }
                    else
                    {
                        lblOnTime.Text = "✓";
                    }
                    if (dt.Rows[0]["LateChecking"].ToString() == "No")
                    {
                        lblLateChecking.Visible = false;
                        late.Style.Add("display", "none");
                    }
                    else
                    {
                        lblLateChecking.Text = "✓";
                    }
                    if (dt.Rows[0]["Leave"].ToString() == "No")
                    {
                        lblLeave.Visible = false;
                        leave.Style.Add("display", "none");
                    }
                    else
                    {
                        lblLeave.Text = "✓";
                    }
                    if (dt.Rows[0]["NotChecking"].ToString() == "No")
                    {
                        lblNotChecking.Visible = false;
                        nocheck.Style.Add("display", "none");
                    }
                    else
                    {
                        lblNotChecking.Text = "✓";
                    }
                    //lblLateChecking.Text = dt.Rows[0]["LateChecking"].ToString();
                    //lblLeave.Text = dt.Rows[0]["Leave"].ToString();
                    //lblNotChecking.Text = dt.Rows[0]["NotChecking"].ToString();
                }
            }
        }


        objdas.SelfOrTeam = 1;// team
        DataSet dsResultTeam = objdas.GetTodayTeamAttandanceListData();
        if (dsResultTeam != null)
        {
            if (dsResultTeam.Tables.Count > 0)
            {
                if (dsResultTeam.Tables[0].Rows.Count > 0)
                {
                    rptTodayAttendanceDiv.Visible = true;
                    rptTodayAttendanceNoData.Visible = false;

                    DataTable dt = dsResultTeam.Tables[0];
                    rptTodayAttendance.DataSource = dt;
                    rptTodayAttendance.DataBind();
                }
                else
                {
                    rptTodayAttendanceDiv.Visible = false;
                    rptTodayAttendanceNoData.Visible = true;
                }
            }
            else
            {
                rptTodayAttendanceDiv.Visible = false;
                rptTodayAttendanceNoData.Visible = true;
            }
        }

        bool VisibleStatus = getVisibleStatus("MTDAttendance");
        if (VisibleStatus == false)
        {
            TodayAttendancewidget.Visible = false;
            rptTodayAttendanceDiv.Visible = false;
            rptTodayAttendanceNoData.Visible = false;
        }
    }

    private void GetUserSaleListData()
    {
        ClsBusinessDashboard objdas = new ClsBusinessDashboard();
        objdas.UserID = userID;
        objdas.authKey = "";
        DataSet dsResult = objdas.GetUserSaleListData();
        if (dsResult != null)
        {
            if (dsResult.Tables.Count > 0)
            {
                if (dsResult.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = dsResult.Tables[0];
                    lblSaleQty.Text = (Math.Ceiling(Convert.ToDouble(dt.Rows[0]["YTDSaleQuantity"]))).ToString("#,##0");
                    lblSaleValue.Text = (Math.Ceiling(Convert.ToDouble(dt.Rows[0]["YTDSaleValue"]))).ToString("#,##0");
                    lblMTDSaleQty.Text = (Math.Ceiling(Convert.ToDouble(dt.Rows[0]["MTDSaleQuantity"]))).ToString("#,##0");
                    lblMTDSaleValue.Text = (Math.Ceiling(Convert.ToDouble(dt.Rows[0]["MTDSaleValue"]))).ToString("#,##0");
                }
            }
        }

        bool VisibleStatus = getVisibleStatus("TopSalesMen");
        if (VisibleStatus == false)
        {
            YesterdaySalewidget.Visible = false;
            MTDSalewidget.Visible = false;
            YesterdayCollectionwidget.Visible = false;
            MTDCollectionwidget.Visible = false;
        }
    }

    // team data
    private void GetUserTopSalesMenListData()
    {
        ClsBusinessDashboard objdas = new ClsBusinessDashboard();
        objdas.UserID = userID;
        objdas.authKey = "";
        DataSet dsResult = objdas.GetUserTopSalesMenListData();
        if (dsResult != null)
        {
            if (dsResult.Tables.Count > 0)
            {
                if (dsResult.Tables[0].Rows.Count > 0)
                {
                    idTeamDiv.Visible = true;
                    idTeamNoData.Visible = false;

                    DataTable dt = dsResult.Tables[0];
                    rptTeams.DataSource = dt;
                    rptTeams.DataBind();
                }
                else
                {
                    idTeamDiv.Visible = false;
                    idTeamNoData.Visible = true;
                }
            }
            else
            {
                idTeamDiv.Visible = false;
                idTeamNoData.Visible = true;
            }
        }

        bool VisibleStatus = getVisibleStatus("TopSalesMen");
        if (VisibleStatus == false)
        {
            idTeamDiv.Visible = false;
            idTeamNoData.Visible = false;
        }
    }

    private void GetUserBeatPlanListData()
    {
        idBeatPlanDiv.Visible = true;
        idRankingDiv.Visible = false;

        ClsBusinessDashboard objdas = new ClsBusinessDashboard();
        objdas.UserID = userID;
        objdas.authKey = "";
        DataSet dsResult = objdas.GetUserBeatPlanListData();
        if (dsResult != null)
        {
            if (dsResult.Tables.Count > 0)
            {
                if (dsResult.Tables[0].Rows.Count > 0)
                {
                    idBeatPlan.Visible = true;
                    idBeatPlanNoData.Visible = false;

                    DataTable dt = dsResult.Tables[0];
                    rptBeatPlan.DataSource = dt;
                    rptBeatPlan.DataBind();
                }
                else
                {
                    idBeatPlan.Visible = false;
                    idBeatPlanNoData.Visible = true;
                }
            }
            else
            {
                idBeatPlan.Visible = false;
                idBeatPlanNoData.Visible = true;
            }
        }

        bool VisibleStatus = getVisibleStatus("BeatPlan");
        if (VisibleStatus == false)
        {
            rbtnBeatPlan.Visible = false;
            idBeatPlanDiv.Visible = false;
        }
    }

    private void GetUserPaymentCollection()
    {
        //idBeatPlanDiv.Visible = true;
        //idRankingDiv.Visible = false;

        ClsBusinessDashboard objdas = new ClsBusinessDashboard();
        objdas.UserID = userID;
        objdas.BaseEntityTypeID = 2;
        objdas.authKey = "";
        DataSet dsResult = objdas.GetPaymentCollectionListData();
        if (dsResult != null)
        {
            if (dsResult.Tables.Count > 0)
            {
                if (dsResult.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = dsResult.Tables[0];
                    lblytdcol.Text = (Math.Round(Convert.ToDecimal(dt.Rows[0]["YTDAmount"].ToString()))).ToString("#,##0");
                    lblmtdcol.Text = (Math.Round(Convert.ToDecimal(dt.Rows[0]["MTDAmount"].ToString()))).ToString("#,##0");
                    //lblExpenseRejected.Text = dt.Rows[0]["Rejected"].ToString();
                }
            }
        }
    }

    private void GetTopModelListData()
    {
        ClsBusinessDashboard objdas = new ClsBusinessDashboard();
        objdas.UserID = userID;
        objdas.authKey = "";
        //objdas.RoleId = BussinessLogic.PageBase.RoleID;
        DataSet dsResult = objdas.GetTopModelListData();
        if (dsResult != null)
        {
            if (dsResult.Tables.Count > 0)
            {
                if (dsResult.Tables[0].Rows.Count > 0)
                {
                    topmodel.Visible = true;
                    topmodelNodata.Visible = false;

                    rptTopModel.DataSource = dsResult.Tables[0];
                    rptTopModel.DataBind();
                }
                else
                {
                    topmodel.Visible = false;
                    topmodelNodata.Visible = true;
                }
            }
            else
            {
                topmodel.Visible = false;
                topmodelNodata.Visible = true;
            }
        }

        bool VisibleStatus = getVisibleStatus("TopModels");
        if (VisibleStatus == false)
        {
            topmodel.Visible = false;
            topmodelNodata.Visible = false;
        }
    }

    private void GetTargetVsAchievementListData()
    {
        ClsBusinessDashboard objdas = new ClsBusinessDashboard();
        objdas.UserID = userID;
        objdas.authKey = "";
        DataSet dsResult = objdas.GetTargetVsAchievementListData();
        if (dsResult != null)
        {
            if (dsResult.Tables.Count > 0)
            {
                if (dsResult.Tables[0].Rows.Count > 0)
                {

                }
            }
        }
    }

    private void GetRankingListData()
    {
        ClsBusinessDashboard objdas = new ClsBusinessDashboard();
        objdas.UserID = userID;
        objdas.authKey = "";
        DataSet dsResult = objdas.GetRankingListData();
        if (dsResult != null)
        {
            if (dsResult.Tables.Count > 0)
            {
                if (dsResult.Tables[0].Rows.Count > 0)
                {
                    idRanking.Visible = true;
                    idRankingNoData.Visible = false;

                    DataTable dt = dsResult.Tables[0];
                    rptRanking.DataSource = dt;
                    rptRanking.DataBind();
                }
                else
                {
                    idRanking.Visible = false;
                    idRankingNoData.Visible = true;
                }
            }
            else
            {
                idRanking.Visible = false;
                idRankingNoData.Visible = true;
            }
        }

        bool VisibleStatus = getVisibleStatus("Ranking");
        if (VisibleStatus == false)
        {
            rbtnRanking.Visible = false;
            idRankingDiv.Visible = false;
        }
    }

    private void GetTopDistributorListData()
    {
        //idRetailerDiv.Visible = true;
        //idDistributorsDiv.Visible = false;

        ClsBusinessDashboard objdas = new ClsBusinessDashboard();
        objdas.UserID = userID;
        objdas.authKey = "";
        //objdas.RoleId = 1;//BussinessLogic.PageBase.RoleID;
        objdas.TopType = 0; // 0=Top Retailer
        DataSet dsResult = objdas.GetTopDistributorListData();
        if (dsResult != null)
        {
            if (dsResult.Tables.Count > 0)
            {
                if (dsResult.Tables[0].Rows.Count > 0)
                {
                    idtopRetailerNoData.Visible = false;
                    idtopRetailer.Visible = true;

                    DataTable dt = dsResult.Tables[0];
                    rptopRetailer.DataSource = dt;
                    rptopRetailer.DataBind();
                }
                else
                {
                    idtopRetailer.Visible = false;
                    idtopRetailerNoData.Visible = true;
                }
            }
            else
            {
                idtopRetailer.Visible = false;
                idtopRetailerNoData.Visible = true;
            }
        }

        objdas.TopType = 1;
        //1=Top distributor
        DataSet dsResultdis = objdas.GetTopDistributorListData();
        if (dsResultdis != null)
        {
            if (dsResultdis.Tables.Count > 0)
            {
                if (dsResultdis.Tables[0].Rows.Count > 0)
                {
                    idtopDistributorsNoData.Visible = false;
                    //idtopDistributors.Visible = true;

                    DataTable dt = dsResult.Tables[0];
                    rptDistributors.DataSource = dt;
                    rptDistributors.DataBind();
                }
                else
                {
                    idtopDistributorsNoData.Visible = true;
                    //idtopDistributors.Visible = false;
                }
            }
            else
            {
                idtopDistributorsNoData.Visible = true;
               // idtopDistributors.Visible = false;
            }
        }

        bool VisibleStatus = getVisibleStatus("TopDistributor");
        //if (VisibleStatus == false)
        //{
        //    rbtnRetailer.Visible = false;
        //    rbtnDistributors.Visible = false;
        //    idRetailerDiv.Visible = false;
        //    idDistributorsDiv.Visible = false;
        //}
    }

    protected void rbtn_CheckedChanged(object sender, EventArgs e)
    {

        if (rbtnRetailer.Checked)
        {
            idRetailerDiv.Visible = true;
            idDistributorsDiv.Visible = false;
        }
        else
        {
            idRetailerDiv.Visible = false;
            idDistributorsDiv.Visible = true;
        }

    }

    protected void rbtnPlan_CheckedChanged(object sender, EventArgs e)
    {
        if (rbtnBeatPlan.Checked)
        {
            idBeatPlanDiv.Visible = true;
            idRankingDiv.Visible = false;
        }
        else
        {
            idBeatPlanDiv.Visible = false;
            idRankingDiv.Visible = true;
        }

    }

    protected void OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView1.PageIndex = e.NewPageIndex;
        this.GetUserLeaveDashboardList();
    }

    protected override void InitializeCulture()
    {
        CultureInfo ci = new CultureInfo("en-IN");
        ci.NumberFormat.CurrencySymbol = "₹";
        Thread.CurrentThread.CurrentCulture = ci;
        ci.NumberFormat.CurrencyDecimalDigits = 0;
        
        base.InitializeCulture();
    }

    protected void OnItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (IsPostBack==false)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                RepeaterItem item = e.Item;

                string lblEntityTypeID = (item.FindControl("Label3") as Label).Text;
                this.strReferenceTypeId = Cryptography.Crypto.Encrypt(lblEntityTypeID, "testEncrpt").ToString();

                //Session["encryptuserid"] = strReferenceTypeId;
                //hrefChieldId
               // HtmlAnchor a1 = (HtmlAnchor)e.Item.FindControl("hrefChieldId");
               // a1.HRef = "DashboardSFA2popup.aspx?userID=" + strReferenceTypeId;
                ////a1.Attributes.Add("href", "window.open('" + a1.HRef + "', 'newwindow','toolbar=yes,location=no,menubar=no,width=450,height=200,resizable=yes,scrollbars=yes,top=200,left=250');return false;");

                ////ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", "var Mleft = (screen.width/2)-(760/2);var Mtop = (screen.height/2)-(700/2);window.open( 'url', null, 'height=700,width=760,status=yes,toolbar=no,scrollbars=yes,menubar=no,location=no,top=\'+Mtop+\', left=\'+Mleft+\'' );", true);
                //string s = "window.open('" + url + "', 'popup_window', 'width=300,height=100,left=100,top=100,resizable=yes');";
                //ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
            }
        }
    }
}