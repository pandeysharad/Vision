using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using System.Net;
using System.Drawing;
using System.Data;

public partial class Pages_FeesRecieve : System.Web.UI.Page
{
    public static Double TransportFees = 0;
    public static string ChiledStatusDetails = "", payMonthClassFee = "", payMonthTransportFee = "";
    private static string classFessDetails = "", TransportFeesDetails = "";
    string msg;
    public int SrNo = 0, SrNoForAssetFine = 0;
    DataClassesDataContext obj = new DataClassesDataContext();
    int admId = 0;
    LoginRole role = new LoginRole();
    protected void Page_Load(object sender, EventArgs e)
    {
        ChiledStatusDetails = "";
        if (Session["UserId"] != null)
        {
            if (Session["UserType"].ToString() != "Admin")
            {
                IEnumerable<LoginRole> roles = from r in obj.LoginRoles
                                               where r.LoginId.Value == Convert.ToInt32(Session["UserId"]) && r.RoleId == 4
                                               select r;
                if (roles.Count<LoginRole>() > 0)
                {
                    role = roles.First<LoginRole>();
                }
                else
                {
                    role.Select = false;
                    role.Insert = false;
                    role.Delete = false;
                    role.Update = false;
                }
            }
            else
            {
                role.Select = true;
                role.Insert = true;
                role.Delete = true;
                role.Update = true;
            }
        }
        try
        {
            var DATA = from Cons in obj.Settings
                       where Cons.CompanyId == Convert.ToInt32(Session["CompanyId"])
                       select Cons;
            Session["SendMsg"] = DATA.First().SendMsg;
        }
        catch (Exception ex)
        { }
        if (!IsPostBack)
        {

            FillDropdown.FillClassList(ddlClassName);
            if (Request.QueryString["AdmissionNo"] != null)
            {
                GetAdmissionDetailsByAddmissionNo(Request.QueryString["AdmissionNo"]);
            }
            if (Session["CompanyId"] != null)
            {
                if (Session["SessionId"].ToString() != "1")
                {
                    Previous.Visible = false;
                }
                else
                {
                    Previous.Visible = true;
                }
                AutoCompleteExtender4.ContextKey = Session["SessionId"].ToString();
                dtReceivedDate.Text = System.DateTime.Now.ToString("dd/MM/yyyy");
                txtAdminNo.Focus();
                //Clear();
                if (Session["ReceiptAdmissionNo"] != null)
                {
                    txtAdminNo.Text = Session["ReceiptAdmissionNo"].ToString();
                    lblPaymentId.Text = "0";
                    if (txtAdminNo.Text != null)
                    {
                        GetStudentInfo();
                        txtPaidPreviousBalance.Enabled = false;
                    }
                }
            }
            else
            {
                Response.Redirect("../Default.aspx");
            }
            //for (int month = 1; month <= 12; month++)
            //{
            //    string monthName = DateTimeFormatInfo.CurrentInfo.GetMonthName(month);
            //    ddlMonths.Items.Add(new ListItem(monthName, month.ToString().PadLeft(2, '0')));
            //}

        }
    }
    protected void btnSearchDetails_Click(object sender, EventArgs e)
    {
        GetAdmissionDetailsByAddmissionNo(txtAdminNo.Text);
    }

    protected void GetAdmissionDetailsByAddmissionNo(string AdmissionNo)
    {
        txtAdminNo.Text = AdmissionNo.Trim().ToString();
        try
        {
            GetDataAssetFineByAddmisionNo();
            if (role.Select.Value)
            {
                if (txtAdminNo.Text != "")
                {
                    IEnumerable<Addmision> Addmisions1 = from id in obj.Addmisions
                                                         where id.AdmissionNo == txtAdminNo.Text
                                                         && id.CompanyId == Convert.ToInt32(Session["CompanyId"]) && id.SessionId == Convert.ToInt32(Session["SessionId"])
                                                         select id;
                    ChiledStatusDetails = "";
                    if (Addmisions1.First().Course1 != 0 && Addmisions1.First().Course1 != null)
                    {
                        ChiledStatusDetails += "<tr style='font-weight:bold'><td>Staus</td><td>Class</td><td>Name</td><td>Scholar No.</td></tr>";
                        IEnumerable<Course> courseName = from id in obj.Courses
                                                         where id.CourseId == Convert.ToInt32(Addmisions1.First().Course1)
                                                             && id.CompanyId == Convert.ToInt32(Session["CompanyId"]) && id.SessionId == Convert.ToInt32(Session["SessionId"])
                                                         select id;
                        IEnumerable<Addmision> AddmisionsChild1 = from id in obj.Addmisions
                                                                  where id.AdmissionId == Convert.ToInt32(Addmisions1.First().StudentName1)
                                                             && id.CompanyId == Convert.ToInt32(Session["CompanyId"]) && id.SessionId == Convert.ToInt32(Session["SessionId"])
                                                                  select id;
                        ChiledStatusDetails += "<tr><td>" + AddmisionsChild1.First().Nationality + "</td><td>" + courseName.First().CourseName + "</td><td>" + AddmisionsChild1.First().StudentName + "</td><td><a target='_blank' href='FeesRecieve.aspx?AdmissionNo=" + AddmisionsChild1.First().AdmissionNo + " '>" + AddmisionsChild1.First().AdmissionNo + "</a></td></tr>";
                    }
                    if (Addmisions1.First().Course2 != 0 && Addmisions1.First().Course2 != null)
                    {

                        IEnumerable<Course> courseName = from id in obj.Courses
                                                         where id.CourseId == Convert.ToInt32(Addmisions1.First().Course2)
                                                             && id.CompanyId == Convert.ToInt32(Session["CompanyId"]) && id.SessionId == Convert.ToInt32(Session["SessionId"])
                                                         select id;
                        IEnumerable<Addmision> AddmisionsChild1 = from id in obj.Addmisions
                                                                  where id.AdmissionId == Convert.ToInt32(Addmisions1.First().StudentName2)
                                                             && id.CompanyId == Convert.ToInt32(Session["CompanyId"]) && id.SessionId == Convert.ToInt32(Session["SessionId"])
                                                                  select id;
                        ChiledStatusDetails += "<tr><td>" + AddmisionsChild1.First().Nationality + "</td><td>" + courseName.First().CourseName + "</td><td>" + AddmisionsChild1.First().StudentName + "</td><td><a target='_blank' href='FeesRecieve.aspx?AdmissionNo=" + AddmisionsChild1.First().AdmissionNo + " '>" + AddmisionsChild1.First().AdmissionNo + "</a></td></tr>";
                    }
                    if (Addmisions1.First().CourseOther1 != 0 && Addmisions1.First().CourseOther1 != null)
                    {
                        ChiledStatusDetails += "<tr style='font-weight:bold'><td>Staus</td><td>Class</td><td>Name</td><td>Scholar No.</td></tr>";
                        IEnumerable<Course> courseName = from id in obj.Courses
                                                         where id.CourseId == Convert.ToInt32(Addmisions1.First().CourseOther1)
                                                             && id.CompanyId == Convert.ToInt32(Session["CompanyId"]) && id.SessionId == Convert.ToInt32(Session["SessionId"])
                                                         select id;
                        IEnumerable<Addmision> AddmisionsChild1 = from id in obj.Addmisions
                                                                  where id.AdmissionId == Convert.ToInt32(Addmisions1.First().StudentOther1)
                                                             && id.CompanyId == Convert.ToInt32(Session["CompanyId"]) && id.SessionId == Convert.ToInt32(Session["SessionId"])
                                                                  select id;
                        ChiledStatusDetails += "<tr><td>" + AddmisionsChild1.First().Nationality + "</td><td>" + courseName.First().CourseName + "</td><td>" + AddmisionsChild1.First().StudentName + "</td><td><a target='_blank' href='FeesRecieve.aspx?AdmissionNo=" + AddmisionsChild1.First().AdmissionNo + "  '>" + AddmisionsChild1.First().AdmissionNo + "</a></td></tr>";
                    }
                    string str = "", year = "";
                    str = Convert.ToDateTime(Addmisions1.First().AdmissionDate).ToString("yyyy/MM/dd");
                    year = str.Substring(0, 4);
                    string str1 = Convert.ToDateTime(DateTime.Now).Year.ToString();
                    if (Session["SessionId"].ToString() == "1")
                    {
                        if (year == str1)
                        {
                            Previous.Visible = false;
                        }
                        else
                        {
                            Previous.Visible = true;
                        }
                    }
                    if (Addmisions1.Count<Addmision>() > 0)
                    {
                        var D1 = from Cons in obj.Sessions
                                 where Cons.Sessionid == Convert.ToInt32(Session["SessionId"])
                                 select Cons;
                        str = D1.First().SessionName;
                        year = str.Substring(0, 4);
                        str1 = Convert.ToDateTime(DateTime.Now).Year.ToString();
                        if (year == str1)
                        {
                            IEnumerable<Addmision> Addmisions = from id in obj.Addmisions
                                                                where id.AdmissionNo == txtAdminNo.Text
                                                                && id.CompanyId == Convert.ToInt32(Session["CompanyId"]) && id.SessionId == Convert.ToInt32(Session["SessionId"])
                                                                select id;

                            if (Addmisions.First().R1 == "ACTIVE" || Addmisions.First().R1 == "ACTIVE RTE")
                            {
                                if (Addmisions.First().R1 == "ACTIVE" || Addmisions.First().R1 == "ACTIVE RTE")
                                {
                                    //Clear();
                                    chkPreviouse.Checked = false;
                                    txtPaidPreviousBalance.Text = "";
                                    GetStudentInfo();
                                }
                                //if (Addmisions.First().R1 == "ACTIVE RTE")
                                //{
                                //    if (Addmisions.First().From != "" && Addmisions.First().To != "")
                                //    {
                                //        Clear();
                                //        chkPreviouse.Checked = false;
                                //        txtPaidPreviousBalance.Text = "";
                                //        //GetStudentInfo();
                                //    }
                                //    else
                                //    {
                                //        Globals.Message(Page, "RTE student...");
                                //    }
                                //}
                            }
                            else
                            {
                                Globals.Message(Page, "This admission no. not active or TC Issued");
                            }
                        }
                        else
                        {
                            IEnumerable<Addmision> Addmisions = from id in obj.Addmisions
                                                                where id.AdmissionNo == txtAdminNo.Text
                                                                && id.CompanyId == Convert.ToInt32(Session["CompanyId"]) && id.SessionId == Convert.ToInt32(Session["SessionId"])
                                                                select id;

                            if (Addmisions.Count<Addmision>() > 0)
                            {
                                if (Addmisions.First().R1 == "ACTIVE" || Addmisions.First().R1 == "ACTIVE BPL" || Addmisions.First().R1 == "ACTIVE RTE")
                                {
                                    //Clear();
                                    chkPreviouse.Checked = false;
                                    txtPaidPreviousBalance.Text = "";
                                    GetStudentInfo();
                                }
                                else
                                {
                                    Globals.Message(Page, "This admission no. not active or TC Issued");
                                }
                            }
                            else
                            {
                                Globals.Message(Page, "This admission no. not active or TC Issued");
                            }
                        }
                    }
                    else
                    {
                        Globals.Message(Page, "This admission is not exist...");
                    }
                }
                else
                {
                    msg = "Please enter student admission number...";
                    ScriptManager.RegisterStartupScript(this, GetType(), "MessageShow", "MessageShow('" + msg + "');", true);
                }
            }
            else Globals.Message(Page, "Not Authorized ??");
        }
        catch (Exception ex)
        {
            msg = "Adimission no does not exist...";
            ScriptManager.RegisterStartupScript(this, GetType(), "MessageShow", "MessageShow('" + msg + "');", true);
        }
    }

    private void GetStudentInfo()
    {
        try
        {
            decimal TotalFees = 0;
            GridStudentFees.Columns[4].Visible = true;

            GridStudentFees.Columns[5].Visible = true;
            if (role.Select.Value)
            {

                var DATA = from Cons in obj.Addmisions
                           where Cons.AdmissionNo == txtAdminNo.Text && Cons.Remove == false && Cons.CompanyId == Convert.ToInt32(Session["CompanyId"]) && Cons.SessionId == Convert.ToInt32(Session["SessionId"])
                           select Cons;


                if (DATA.First().AdmissionNo != null)
                {
                    //Showing Transport Area
                    if (Convert.ToDouble(DATA.First().TransportFeesAfterDisc) > 0)
                    {
                        if (DATA.First().EnrollmentNo != null && DATA.First().EnrollmentNo != "")
                        {
                            DivlblTransportArea.Visible = true;
                            lblTransportArea.Text = DATA.First().EnrollmentNo;
                        }
                    }
                    else
                    {
                        DivlblTransportArea.Visible = false;
                    }
                    admId = DATA.First().AdmissionId;
                    Session["AdmissionCourseId"] = DATA.First().CourseId;
                    txtAdmissionNo.Text = DATA.First().AdmissionNo;
                    txtStudentName.Text = DATA.First().StudentName;
                    txtCourse.Text = DATA.First().CourseName + " / '" + DATA.First().Section + "'";
                    txtCourseFees.Text = DATA.First().CourseFeesAfterDisc.ToString();
                    TotalFees = (Convert.ToDecimal(DATA.First().TotalFees) + Convert.ToDecimal(DATA.First().TransportFeesAfterDisc));
                    txtTransportFees.Text = DATA.First().TransportFeesAfterDisc.ToString();
                    txtTotalPayable.Text = (Convert.ToDecimal(txtCourseFees.Text) + Convert.ToDecimal(txtTransportFees.Text)).ToString();
                    imgStudentImg.ImageUrl = DATA.First().StudentPhoto;
                    txtFatherName.Text = DATA.First().FatherName;
                    txtChildStatus.Text = DATA.First().Nationality;
                }
                else
                {
                    msg = "Adimission no does not exist...";
                    ScriptManager.RegisterStartupScript(this, GetType(), "MessageShow", "MessageShow('" + msg + "');", true);
                }
                IEnumerable<PreviousBalance> PreviousBalances = from Cons in obj.PreviousBalances
                                                                where Cons.AdmissionNo == txtAdminNo.Text && Cons.Remove == false && Cons.CompanyId == Convert.ToInt32(Session["CompanyId"]) && Cons.SessionId == Convert.ToInt32(Session["SessionId"])
                                                                select Cons;
                if (PreviousBalances.Count<PreviousBalance>() > 0)
                {
                    ddlSession.Text = PreviousBalances.First().PreviousSession;
                    txtOpening.Text = PreviousBalances.First().PreviousBalance1.ToString();
                    Session["PreviousBalance"] = PreviousBalances.First().PreviousBalance1.ToString();
                    txtOpening.Enabled = false;
                    chkPreviouse.Enabled = true;
                    btnOpeningSave.Visible = false;
                }
                else
                {
                    txtOpening.Text = "";
                    Session["PreviousBalance"] = "0";
                    txtOpening.Enabled = true;
                    txtPaidPreviousBalance.Text = "";
                    txtPaidPreviousBalance.Enabled = false;
                    chkPreviouse.Checked = false;
                    btnOpeningSave.Visible = true;
                }
                if (DATA.First().PaymentType == "Installment")
                {
                    IdMonthlyInstallments.Visible = false;
                    IdInstallments.Visible = true;
                    var DATA2 = from Cons in obj.Installments
                                where Cons.AdmissionId == admId && Cons.Remove == false && Cons.CompanyId == Convert.ToInt32(Session["CompanyId"]) && Cons.SessionId == Convert.ToInt32(Session["SessionId"])
                                select Cons;
                    int i = DATA2.Count();
                    GridInstallments.DataSource = DATA2;
                    GridInstallments.DataBind();
                }
                else if (DATA.First().PaymentType == "Monthly")
                {
                    IdMonthlyInstallments.Visible = true;
                    IdInstallments.Visible = false;
                    var DATA2 = from Cons in obj.MonthlyInstallments
                                where Cons.AdmissionId == admId && Cons.Remove == false && Cons.CompanyId == Convert.ToInt32(Session["CompanyId"]) && Cons.SessionId == Convert.ToInt32(Session["SessionId"])
                                select Cons;
                    GridMonthlyInstallments.DataSource = DATA2;
                    GridMonthlyInstallments.DataBind();
                }
                int aa = 1;
                var _session = obj.Sessions.Where(x => x.Sessionid == Convert.ToInt32(Session["SessionId"]));
                var studentSession = txtAdminNo.Text.Substring(0, 4);
                var sessionName = _session.First().SessionName.Substring(2, 2) + _session.First().SessionName.Substring(5);
                if (studentSession == sessionName)
                {
                    aa = 0;
                }
                chkOtherFees.Checked = true;
                DataSet ds = AllMethods.GetOtherFeesByCourse(Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["SessionId"]), Convert.ToInt32(DATA.First().CourseId), aa, Convert.ToInt32(DATA.First().AdmissionId));
                GridOtherFee.DataSource = ds;
                GridOtherFee.DataBind();
                // OtherFeesPaidHead showing code start
                lblOtherFeesPaidHead.Text = GetOtherFeesPaidHead(Convert.ToInt32(DATA.First().AdmissionId), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["SessionId"]));
                if (lblOtherFeesPaidHead.Text != "")
                {
                    divOtherFeesPaidHead.Visible = true; 
                }
                else
                {
                    divOtherFeesPaidHead.Visible = false;
                }
                // OtherFeesPaidHead showing code End
                string str = "", year = "";
                str = DATA.First().Session;
                year = str.Substring(0, 4);
                string str1 = Convert.ToDateTime(DATA.First().AdmissionDate).Year.ToString();
                if (year == str1)
                {
                    var Other_Fee = from o in obj.Other_Fees
                                    where o.CourseId == Convert.ToInt32(DATA.First().CourseId)
                                    && o.Remove == false && o.CompanyId == Convert.ToInt32(Session["CompanyId"]) && o.SessionId == Convert.ToInt32(Session["SessionId"])
                                    select o;

                    comOtherFeesType.DataSource = Other_Fee;
                    comOtherFeesType.DataTextField = "FeesType";
                    comOtherFeesType.DataValueField = "OtherFeesId";
                    comOtherFeesType.DataBind();
                    comOtherFeesType.Items.Insert(0, new ListItem());
                    string AllOthers = "";
                    if (comOtherFeesType.Items.Count >= 1)
                    {
                        if (comOtherFeesType.Items.Count <= 1)
                        {
                            for (int i = 1; i < comOtherFeesType.Items.Count; i++)
                            {
                                AllOthers = comOtherFeesType.Items[i].ToString();
                            }
                        }
                        else
                        {
                            for (int i = 1; i < comOtherFeesType.Items.Count; i++)
                            {
                                if (i == 1)
                                    AllOthers = AllOthers + comOtherFeesType.Items[i].ToString();
                                else
                                    AllOthers = AllOthers + " & " + comOtherFeesType.Items[i].ToString();
                            }
                        }
                    }
                    comOtherFeesType.Items.Insert(1, new ListItem(AllOthers, "0"));
                }
                else
                {
                    var Other_Fee = from o in obj.Other_Fees
                                    where o.CourseId == Convert.ToInt32(DATA.First().CourseId) && o.FeesType != "CAUTION MONEY"
                                    && o.Remove == false && o.CompanyId == Convert.ToInt32(Session["CompanyId"]) && o.SessionId == Convert.ToInt32(Session["SessionId"])
                                    select o;

                    comOtherFeesType.DataSource = Other_Fee;
                    comOtherFeesType.DataTextField = "FeesType";
                    comOtherFeesType.DataValueField = "OtherFeesId";
                    comOtherFeesType.DataBind();
                    comOtherFeesType.Items.Insert(0, new ListItem());
                    string AllOthers = "";
                    if (comOtherFeesType.Items.Count >= 1)
                    {
                        if (comOtherFeesType.Items.Count <= 1)
                        {
                            for (int i = 1; i < comOtherFeesType.Items.Count; i++)
                            {
                                AllOthers = comOtherFeesType.Items[i].ToString();
                            }
                        }
                        else
                        {
                            for (int i = 1; i < comOtherFeesType.Items.Count; i++)
                            {
                                if (i == 1)
                                    AllOthers = AllOthers + comOtherFeesType.Items[i].ToString();
                                else
                                    AllOthers = AllOthers + " & " + comOtherFeesType.Items[i].ToString();
                            }
                        }
                    }
                    comOtherFeesType.Items.Insert(1, new ListItem(AllOthers, "0"));
                }

                var DATA3 = from Cons in obj.Payments
                            where Cons.AdmissionId == admId && Cons.Remove == false && Cons.ComapanyId == Convert.ToInt32(Session["CompanyId"]) && Cons.SessionId == Convert.ToInt32(Session["SessionId"])
                            && Cons.PreviousPaid == 0
                            select Cons;

                GridStudentFees.DataSource = DATA3;
                GridStudentFees.DataBind();
                //GridPreviousPaid
                DataSet dsPreviousPaid = new DataSet();
                dsPreviousPaid = AllMethods.GetPreviousPaidFeesDetails(Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["SessionId"]), Convert.ToInt32(DATA.First().CourseId), admId);
                GridPreviousPaid.DataSource = dsPreviousPaid;
                GridPreviousPaid.DataBind();

                decimal admin = 0, latefees = 0, CourseFees = 0, TransportFees = 0, OtherFees = 0, Total = 0, PreviousPaid = 0,
                    transportFeeDiscount = 0, classFeeDiscount = 0, otherFeeDiscount = 0;
                string DiscountType = "";
                //Check Previus and class fees and Transport fee and other fee check in previus fees recipt
                var TransportFeesDATA = from Cons in obj.Payments
                                        where Cons.AdmissionId == admId && Cons.Remove == false && Cons.ComapanyId == Convert.ToInt32(Session["CompanyId"]) && Cons.SessionId == Convert.ToInt32(Session["SessionId"])
                                        && Cons.PreviousPaid != 0
                                        select Cons;
                if (TransportFeesDATA.Count() > 0)
                {
                    TransportFees = Convert.ToDecimal(TransportFeesDATA.Sum(x => x.TransportFees));
                    CourseFees = Convert.ToDecimal(TransportFeesDATA.Sum(x => x.CourseFees));
                    OtherFees = Convert.ToDecimal(TransportFeesDATA.Sum(x => x.OtherFees));
                }
                //End Check Previus
                if (GridStudentFees.Rows.Count > 0)
                {
                    for (int i = 0; i < GridStudentFees.Rows.Count; i++)
                    {
                        GridViewRow row = GridStudentFees.Rows[i];
                        PreviousPaid = PreviousPaid + Convert.ToDecimal(row.Cells[5].Text);
                        admin = admin + Convert.ToDecimal(row.Cells[6].Text);
                        latefees = latefees + Convert.ToDecimal(row.Cells[7].Text);
                        OtherFees = OtherFees + Convert.ToDecimal(row.Cells[9].Text);
                        CourseFees = CourseFees + Convert.ToDecimal(row.Cells[10].Text);
                        TransportFees = TransportFees + Convert.ToDecimal(row.Cells[11].Text);
                        Total = Total + Convert.ToDecimal(row.Cells[13].Text);
                        DiscountType = row.Cells[15].Text.ToString();
                        if (DiscountType != string.Empty && DiscountType != null)
                        {
                            if (DiscountType == "Transport Fee")
                            {
                                transportFeeDiscount = transportFeeDiscount + Convert.ToDecimal(row.Cells[12].Text);
                            }
                            else if (DiscountType == "Class Fee")
                            {
                                classFeeDiscount = classFeeDiscount + Convert.ToDecimal(row.Cells[12].Text);
                            }
                            else if (DiscountType == "Other")
                            {
                                otherFeeDiscount = otherFeeDiscount + Convert.ToDecimal(row.Cells[12].Text);
                            }
                        }
                    }
                    GridStudentFees.FooterRow.Cells[3].Text = "TOTAL";
                    GridStudentFees.FooterRow.Cells[3].HorizontalAlign = HorizontalAlign.Right;
                    GridStudentFees.FooterRow.Cells[5].Text = PreviousPaid.ToString();
                    GridStudentFees.FooterRow.Cells[6].Text = admin.ToString();
                    GridStudentFees.FooterRow.Cells[7].Text = latefees.ToString();
                    GridStudentFees.FooterRow.Cells[9].Text = OtherFees.ToString();
                    GridStudentFees.FooterRow.Cells[10].Text = CourseFees.ToString();
                    GridStudentFees.FooterRow.Cells[11].Text = TransportFees.ToString();
                    GridStudentFees.FooterRow.Cells[13].Text = Total.ToString();
                }

                //TotalFees = (Convert.ToDecimal(DATA.First().TotalFees) + Convert.ToDecimal(DATA.First().TransportFeesAfterDisc));
                lbltotClassFee.Text = DATA.First().CourseFeesAfterDisc.ToString();
                lbltotTransportFee.Text = DATA.First().TransportFeesAfterDisc.ToString();
                lbltotOtherFee.Text = (Convert.ToDecimal(DATA.First().TotalFees) - (Convert.ToDecimal(DATA.First().CourseFeesAfterDisc))).ToString();

                lbltotClassFeeDis.Text = (classFeeDiscount).ToString();
                lbltotTransportFeeDis.Text = (transportFeeDiscount).ToString();
                lbltotOtherFeeDis.Text = (otherFeeDiscount).ToString();

                lbltotClassFeeRec.Text = (CourseFees - classFeeDiscount).ToString();
                lbltotTransportFeeRec.Text = (TransportFees - transportFeeDiscount).ToString();
                lbltotOtherFeeRec.Text = (OtherFees - otherFeeDiscount).ToString();

                lbltotClassFeeBla.Text = (DATA.First().CourseFeesAfterDisc - CourseFees).ToString();
                lbltotTransportFeeBla.Text = (DATA.First().TransportFeesAfterDisc - TransportFees).ToString();
                lbltotOtherFeeBla.Text = (Convert.ToDecimal(DATA.First().TotalFees) - (Convert.ToDecimal(DATA.First().CourseFeesAfterDisc)) - OtherFees).ToString();
                lbltotFee.Text = TotalFees.ToString();
                lbltotRec.Text = ((CourseFees + TransportFees + OtherFees) - (classFeeDiscount + transportFeeDiscount + otherFeeDiscount)) + " (Disc.:" + (classFeeDiscount + transportFeeDiscount + otherFeeDiscount) + ")".ToString();
                lbltotbla.Text = (Convert.ToDecimal(TotalFees) - (CourseFees + TransportFees + OtherFees)).ToString();
                if (DATA.First().R1 == "ACTIVE RTE")
                {
                    Globals.Message(Page, "ACTIVE RTE STUDENT !!");
                    lbltotClassFee.Text = "0";
                    lbltotOtherFee.Text = "0";

                    lbltotClassFeeDis.Text = "0";
                    lbltotTransportFeeDis.Text = (transportFeeDiscount).ToString();
                    lbltotOtherFeeDis.Text = "0";

                    lbltotClassFeeRec.Text = "0";
                    lbltotTransportFeeRec.Text = (TransportFees - transportFeeDiscount).ToString();
                    lbltotOtherFeeRec.Text = "0";

                    lbltotClassFeeBla.Text = "0";
                    lbltotTransportFeeBla.Text = (DATA.First().TransportFeesAfterDisc - TransportFees).ToString();
                    lbltotOtherFeeBla.Text = "0";

                    lbltotFee.Text = DATA.First().TransportFeesAfterDisc.ToString();
                    lbltotRec.Text = ((TransportFees) - (transportFeeDiscount)) + " (Disc.:" + (transportFeeDiscount) + ")".ToString();
                    lbltotbla.Text = (Convert.ToDecimal(DATA.First().TransportFeesAfterDisc) - ((TransportFees) - (transportFeeDiscount))).ToString();
                }
                lbMessage.Text = "<b>Total Fees : " + TotalFees + " &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Received Fees : " + (CourseFees + TransportFees + OtherFees) + " &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Balance : " + (Convert.ToDecimal(TotalFees) - (CourseFees + TransportFees + OtherFees)) + "</b>";
                IEnumerable<PreviousBalance> PreviousBalances1 = from Cons in obj.PreviousBalances
                                                                 where Cons.AdmissionNo == txtAdminNo.Text && Cons.Remove == false && Cons.CompanyId == Convert.ToInt32(Session["CompanyId"]) && Cons.SessionId == Convert.ToInt32(Session["SessionId"])
                                                                 select Cons;
                if (PreviousBalances1.Count<PreviousBalance>() > 0)
                {
                    GridStudentFees.Columns[4].Visible = true;

                    GridStudentFees.Columns[5].Visible = true;
                }
                else
                {
                    GridStudentFees.Columns[4].Visible = false;

                    GridStudentFees.Columns[5].Visible = false;
                }
                GetPreviousBalance(txtAdminNo.Text.Trim());
            }
            else Globals.Message(Page, "Not Authorized ??");
        }
        catch (Exception ex)
        { }
    }
    public void GetPreviousBalance(string AddmissionNo)
    {
        decimal TotalClassFees = 0, TotalTransportFees = 0, TotalOtherFees = 0, Balance = 0;
        IEnumerable<Session> Sessions = from Cons in obj.Sessions

                                        select Cons;
        Label lbl = new Label();
        lbl.Text = "";
        foreach (Session Cd in Sessions)
        {
            if (Cd.Sessionid == Convert.ToInt32(Session["SessionId"]))
            {
                break;
            }
            else
            {
                PnlLabel.Visible = true;
                IEnumerable<PreviousBalance> PreviousBalances = from Cons in obj.PreviousBalances
                                                                where Cons.AdmissionNo == AddmissionNo && Cons.SessionId == Cd.Sessionid
                                                                select Cons;

                IEnumerable<Addmision> Addmisions = from Cons in obj.Addmisions
                                                    where Cons.AdmissionNo == AddmissionNo && Cons.SessionId == Cd.Sessionid
                                                    select Cons;

                IEnumerable<Payment> Payments = from Cons in obj.Payments
                                                where Cons.AdmissionNo == AddmissionNo && Cons.SessionId == Cd.Sessionid
                                                select Cons;

                TotalClassFees = Convert.ToDecimal(Payments.Sum<Payment>(s => s.CourseFees.Value));
                TotalTransportFees = Convert.ToDecimal(Payments.Sum<Payment>(s => s.TransportFees.Value));
                TotalOtherFees = Convert.ToDecimal(Payments.Sum<Payment>(s => s.OtherFees.Value));

                Balance = Convert.ToDecimal((Addmisions.First().TotalFees) - (TotalClassFees + TotalTransportFees + TotalOtherFees));

                if (Balance == 0 && PreviousBalances.First().PreviousBalance1 == 0)
                {
                    PnlLabel.Visible = false;
                }
                else
                {
                    PnlLabel.Visible = true;
                }

                if (PreviousBalances.Count<PreviousBalance>() > 0)
                {

                    lbl.Text += "2017-18-" + PreviousBalances.First().PreviousBalance1 + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
                    lbl.Font.Bold = true;
                }
                lbl.Text += Cd.SessionName + "-" + (Addmisions.First().TotalFees - (TotalClassFees + TotalTransportFees + TotalOtherFees)) + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
                lbl.Font.Bold = true;
                lbl.BackColor = System.Drawing.Color.Blue;
            }
            PnlLabel.Controls.Add(lbl);
        }
    }
    protected void ddlMonths_SelectedIndexChanged(object sender, EventArgs e)
    {
        //msg = ddlMonths.SelectedItem.Text + " " + Convert.ToInt32(ddlMonths.SelectedItem.Value);
        //ScriptManager.RegisterStartupScript(this, GetType(), "MessageShow", "MessageShow('" + msg + "');", true);
    }
    protected void comPaymentMode_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            for (int Index = 0; Index < GridInstallments.Rows.Count; Index++)
            {
                TextBox txtCourseFees = (TextBox)GridInstallments.Rows[Index].FindControl("txtCourseFees");
                TextBox txtTransportFees = (TextBox)GridInstallments.Rows[Index].FindControl("txtTransportFees");
                TextBox txtCoursePaid = (TextBox)GridInstallments.Rows[Index].FindControl("txtCoursePaid");
                TextBox txtTransportPaid = (TextBox)GridInstallments.Rows[Index].FindControl("txtTransportPaid");
                DropDownList ddlCountries = (DropDownList)GridInstallments.Rows[Index].FindControl("comPaymentMode");

                //TextBox txtMonths = (TextBox)GridInstallments.Rows[Index].FindControl("txtMonths");
                TextBox txtInstallmentDate = (TextBox)GridInstallments.Rows[Index].FindControl("txtInstallmentDate");
                TextBox txtCourseBalance = (TextBox)GridInstallments.Rows[Index].FindControl("txtCourseBalance");
                TextBox txtTransportBalance = (TextBox)GridInstallments.Rows[Index].FindControl("txtTransportBalance");
                TextBox txtPayClassFees = (TextBox)GridInstallments.Rows[Index].FindControl("txtPayClassFees");
                TextBox txtPayTransportFees = (TextBox)GridInstallments.Rows[Index].FindControl("txtPayTransportFees");
                int InstMonthlyId = Convert.ToInt32(((TextBox)GridInstallments.Rows[Index].FindControl("txtInstallmentId")).Text);
                var installments = from Cons in obj.Installments
                                   where Cons.InstallmentId == InstMonthlyId
                                   //&& Cons.Remove == false && Cons.CompanyId == Convert.ToInt32(Session["CompanyId"]) && Cons.SessionId == Convert.ToInt32(Session["SessionId"])
                                   select Cons;
                double AlreadyPaid = Convert.ToDouble(installments.First().CousePaid);
                double TransportAlreadyPaid = Convert.ToDouble(installments.First().TransportPaid);
                if (((DropDownList)GridInstallments.Rows[Index].FindControl("comPaymentMode")).SelectedItem.Text == "PAID")
                {
                    if (installments.First().PaymentStatus != "PAID")
                    {
                        ((CheckBox)GridInstallments.Rows[Index].FindControl("chk1")).Checked = true;
                        if (Convert.ToDouble(txtPayClassFees.Text) > 0 || Convert.ToDouble(txtPayTransportFees.Text) > 0)
                        {

                        }
                        else
                        {
                            ((TextBox)GridInstallments.Rows[Index].FindControl("txtPayClassFees")).Text = txtCourseFees.Text;
                            ((TextBox)GridInstallments.Rows[Index].FindControl("txtPayTransportFees")).Text = txtTransportFees.Text;

                            ((TextBox)GridInstallments.Rows[Index].FindControl("txtCourseBalance")).Text = "0.00";
                            ((TextBox)GridInstallments.Rows[Index].FindControl("txtTransportBalance")).Text = "0.00";
                        }
                        ddlCountries.BackColor = Color.Green;
                        txtCourseFees.BackColor = Color.Green;
                        txtTransportFees.BackColor = Color.Green;
                        txtInstallmentDate.BackColor = Color.Green;
                        txtCoursePaid.BackColor = Color.Green;
                        txtTransportPaid.BackColor = Color.Green;
                        txtCourseBalance.BackColor = Color.Green;
                        txtTransportBalance.BackColor = Color.Green;
                        txtPayClassFees.BackColor = Color.Green;
                        txtPayTransportFees.BackColor = Color.Green;
                        txtPayClassFees.ForeColor = Color.White;
                        txtPayTransportFees.ForeColor = Color.White;
                    }
                }
                else if (((DropDownList)GridInstallments.Rows[Index].FindControl("comPaymentMode")).SelectedItem.Text == "BALANCE")
                {
                    if (((TextBox)GridInstallments.Rows[Index].FindControl("txtCoursePaid")).Text == "0.00" || ((TextBox)GridInstallments.Rows[Index].FindControl("txtCoursePaid")).Text == txtCourseFees.Text)
                    {
                        //((TextBox)GridInstallments.Rows[Index].FindControl("txtCoursePaid")).Text = "0";
                        //((TextBox)GridInstallments.Rows[Index].FindControl("txtTransportPaid")).Text = "0";
                        //((TextBox)GridInstallments.Rows[Index].FindControl("txtPayClassFees")).Text = "0";
                        //((TextBox)GridInstallments.Rows[Index].FindControl("txtPayTransportFees")).Text ="0";
                        //((TextBox)GridInstallments.Rows[Index].FindControl("txtCourseBalance")).Text = txtCourseFees.Text;
                        //((TextBox)GridInstallments.Rows[Index].FindControl("txtTransportBalance")).Text = txtTransportFees.Text;
                        ddlCountries.BackColor = Color.White;
                        txtCourseFees.BackColor = Color.White;
                        txtTransportFees.BackColor = Color.White;
                        txtInstallmentDate.BackColor = Color.White;
                        txtCoursePaid.BackColor = Color.White;
                        txtTransportPaid.BackColor = Color.White;
                        txtCourseBalance.BackColor = Color.White;
                        txtTransportBalance.BackColor = Color.White;
                        ddlCountries.ForeColor = Color.Black;
                    }
                }
            }
            Calc();
        }
        catch (Exception ex)
        { }
    }
    protected void comPaymentMode_MOnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            for (int Index = 0; Index < GridMonthlyInstallments.Rows.Count; Index++)
            {

                DropDownList ddlCountries = (DropDownList)GridMonthlyInstallments.Rows[Index].FindControl("comPaymentMode");
                string s = ddlCountries.SelectedItem.Text;
                TextBox txtCourseFees = (TextBox)GridMonthlyInstallments.Rows[Index].FindControl("txtCourseFees");
                TextBox txtTransportFees = (TextBox)GridMonthlyInstallments.Rows[Index].FindControl("txtTransportFees");
                TextBox txtCoursePaid = (TextBox)GridMonthlyInstallments.Rows[Index].FindControl("txtPayClassFees");
                TextBox txtTransportPaid = (TextBox)GridMonthlyInstallments.Rows[Index].FindControl("txtPayTransportFees");
                TextBox txtMonths = (TextBox)GridMonthlyInstallments.Rows[Index].FindControl("txtMonths");
                TextBox txtInstallmentDate = (TextBox)GridMonthlyInstallments.Rows[Index].FindControl("txtInstallmentDate");
                TextBox txtCourseBalance = (TextBox)GridMonthlyInstallments.Rows[Index].FindControl("txtCourseBalance");
                TextBox txtTransportBalance = (TextBox)GridMonthlyInstallments.Rows[Index].FindControl("txtTransportBalance");
                TextBox txtPayClassFees = (TextBox)GridMonthlyInstallments.Rows[Index].FindControl("txtPayClassFees");
                TextBox txtPayTransportFees = (TextBox)GridMonthlyInstallments.Rows[Index].FindControl("txtPayTransportFees");
                int MonthlyInstId = Convert.ToInt32(((TextBox)GridMonthlyInstallments.Rows[Index].FindControl("txtMonthlyInstId")).Text);
                var monthlyInstallments = from Cons in obj.MonthlyInstallments
                                          where Cons.MonthlyInstId == MonthlyInstId
                                          //&& Cons.Remove == false && Cons.CompanyId == Convert.ToInt32(Session["CompanyId"]) && Cons.SessionId == Convert.ToInt32(Session["SessionId"])
                                          select Cons;
                if (((DropDownList)GridMonthlyInstallments.Rows[Index].FindControl("comPaymentMode")).SelectedItem.Text == "PAID")
                {
                    if (monthlyInstallments.First().PaymentStatus != "PAID")
                    {
                        ddlCountries.BackColor = Color.Green;
                        txtMonths.BackColor = Color.Green;
                        txtCourseFees.BackColor = Color.Green;
                        txtTransportFees.BackColor = Color.Green;
                        txtInstallmentDate.BackColor = Color.Green;
                        txtCoursePaid.BackColor = Color.Green;
                        txtTransportPaid.BackColor = Color.Green;
                        txtCourseBalance.BackColor = Color.Green;
                        txtTransportBalance.BackColor = Color.Green;
                        txtPayClassFees.BackColor = Color.Green;
                        txtPayTransportFees.BackColor = Color.Green;
                        txtPayClassFees.ForeColor = Color.Black;
                        txtPayTransportFees.ForeColor = Color.Black;
                    }
                }
                if (((DropDownList)GridMonthlyInstallments.Rows[Index].FindControl("comPaymentMode")).SelectedItem.Text == "PAID")
                {
                    if (monthlyInstallments.First().PaymentStatus != "PAID")
                    {
                        ((CheckBox)GridMonthlyInstallments.Rows[Index].FindControl("chk1")).Checked = true;
                        if (Convert.ToDouble(txtPayClassFees.Text) > 0 || Convert.ToDouble(txtPayTransportFees.Text) > 0)
                        {

                        }
                        else
                        {
                            ((TextBox)GridMonthlyInstallments.Rows[Index].FindControl("txtPayClassFees")).Text = txtCourseFees.Text;
                            ((TextBox)GridMonthlyInstallments.Rows[Index].FindControl("txtPayTransportFees")).Text = txtTransportFees.Text;

                            ((TextBox)GridMonthlyInstallments.Rows[Index].FindControl("txtCourseBalance")).Text = "0.00";
                            ((TextBox)GridMonthlyInstallments.Rows[Index].FindControl("txtTransportBalance")).Text = "0.00";
                        }

                    }
                }
                else if (((DropDownList)GridMonthlyInstallments.Rows[Index].FindControl("comPaymentMode")).SelectedItem.Text == "BALANCE")
                {
                    if (((TextBox)GridMonthlyInstallments.Rows[Index].FindControl("txtCoursePaid")).Text == "0.00" || ((TextBox)GridMonthlyInstallments.Rows[Index].FindControl("txtCoursePaid")).Text == txtCourseFees.Text)
                    {
                        //((TextBox)GridMonthlyInstallments.Rows[Index].FindControl("txtPayClassFees")).Text = "0";
                        //((TextBox)GridMonthlyInstallments.Rows[Index].FindControl("txtPayTransportFees")).Text = "0";
                        //((TextBox)GridMonthlyInstallments.Rows[Index].FindControl("txtCourseBalance")).Text = txtCourseFees.Text;
                        //((TextBox)GridMonthlyInstallments.Rows[Index].FindControl("txtTransportBalance")).Text = txtTransportFees.Text;
                        ddlCountries.BackColor = Color.White;
                        txtMonths.BackColor = Color.White;
                        txtCourseFees.BackColor = Color.White;
                        txtTransportFees.BackColor = Color.White;
                        txtInstallmentDate.BackColor = Color.White;
                        txtCoursePaid.BackColor = Color.White;
                        txtTransportPaid.BackColor = Color.White;
                        txtCourseBalance.BackColor = Color.White;
                        txtTransportBalance.BackColor = Color.White;
                        ddlCountries.ForeColor = Color.Black;
                        txtPayClassFees.BackColor = Color.Green;
                        txtPayTransportFees.BackColor = Color.Green;
                        txtPayClassFees.ForeColor = Color.White;
                        txtPayTransportFees.ForeColor = Color.White;
                    }
                }
            }
            MCalc();
            GetPreviousBalance(txtAdminNo.Text.Trim());
        }
        catch (Exception ex)
        { }
    }
    protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //Find the DropDownList in the Row
            DropDownList ddlCountries = (e.Row.FindControl("comPaymentMode") as DropDownList);
            var DATA2 = from Cons in obj.Installments
                        where Cons.AdmissionId == admId && Cons.InstallmentId == Convert.ToInt32(((TextBox)e.Row.FindControl("txtInstallmentId")).Text) && Cons.Remove == false && Cons.CompanyId == Convert.ToInt32(Session["CompanyId"]) && Cons.SessionId == Convert.ToInt32(Session["SessionId"])
                        select Cons;

            ddlCountries.Items.FindByValue(DATA2.First().PaymentStatus).Selected = true;
            if (DATA2.First().CousePaid > 0 || DATA2.First().TransportPaid > 0)
            {
                ddlCountries.Enabled = false;
            }
            else
            {
                ddlCountries.Enabled = true;
            }
            if (DATA2.First().PaymentStatus == "PAID")
            {
                TextBox txtMonths = (e.Row.FindControl("txtInstallmentDate") as TextBox);
                TextBox txtCourseFees = (e.Row.FindControl("txtCourseFees") as TextBox);
                TextBox txtTransportFees = (e.Row.FindControl("txtTransportFees") as TextBox);
                TextBox txtInstallmentDate = (e.Row.FindControl("txtInstallmentDate") as TextBox);
                TextBox txtCoursePaid = (e.Row.FindControl("txtCoursePaid") as TextBox);
                TextBox txtTransportPaid = (e.Row.FindControl("txtTransportPaid") as TextBox);
                TextBox txtCourseBalance = (e.Row.FindControl("txtCourseBalance") as TextBox);
                TextBox txtTransportBalance = (e.Row.FindControl("txtTransportBalance") as TextBox);
                TextBox txtPayClassFees = (e.Row.FindControl("txtPayClassFees") as TextBox);
                TextBox txtPayTransportFees = (e.Row.FindControl("txtPayTransportFees") as TextBox);
                e.Row.BackColor = Color.Yellow;
                ddlCountries.BackColor = Color.Yellow;
                txtMonths.BackColor = Color.Yellow;
                txtCourseFees.BackColor = Color.Yellow;
                txtTransportFees.BackColor = Color.Yellow;
                txtInstallmentDate.BackColor = Color.Yellow;
                txtCoursePaid.BackColor = Color.Yellow;
                txtTransportPaid.BackColor = Color.Yellow;
                txtCourseBalance.BackColor = Color.Yellow;
                txtTransportBalance.BackColor = Color.Yellow;
                txtPayClassFees.BackColor = Color.Yellow;
                txtPayTransportFees.BackColor = Color.Yellow;
                txtPayClassFees.ForeColor = Color.Black;
                txtPayTransportFees.ForeColor = Color.Black;
            }
            if (((TextBox)e.Row.FindControl("txtCoursePaid")).Text != "0.00" || ((TextBox)e.Row.FindControl("txtTransportPaid")).Text != "0.00")
            {
                ((CheckBox)e.Row.FindControl("chk1")).Checked = true;
                if (((TextBox)e.Row.FindControl("txtCoursePaid")).Text != "0.00" && ((DropDownList)e.Row.FindControl("comPaymentMode")).Text == "BALANCE")
                {
                    ((TextBox)e.Row.FindControl("txtCoursePaid")).Enabled = false;
                    ((TextBox)e.Row.FindControl("txtPayClassFees")).Enabled = true;
                    ((TextBox)e.Row.FindControl("txtPayTransportFees")).Enabled = true;
                    ((TextBox)e.Row.FindControl("txtPayClassFees")).Focus();
                }

            }
            else
            {

                ((CheckBox)e.Row.FindControl("chk1")).Checked = false;
            }
        }
    }
    protected void Monthly_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //Find the DropDownList in the Row
            DropDownList ddlCountries = (e.Row.FindControl("comPaymentMode") as DropDownList);
            //if (ddlCountries.SelectedItem.Text == "PAID")
            //{
            //    e.Row.BackColor = System.Drawing.Color.Yellow;
            //}
            var DATA2 = from Cons in obj.MonthlyInstallments
                        where Cons.AdmissionId == admId && Cons.MonthlyInstId == Convert.ToInt32(((TextBox)e.Row.FindControl("txtMonthlyInstId")).Text) && Cons.Remove == false && Cons.CompanyId == Convert.ToInt32(Session["CompanyId"]) && Cons.SessionId == Convert.ToInt32(Session["SessionId"])
                        select Cons;

            ddlCountries.Items.FindByValue(DATA2.First().PaymentStatus).Selected = true;
            if (DATA2.First().CousePaid > 0 || DATA2.First().TransportPaid > 0)
            {
                ddlCountries.Enabled = false;
            }
            else
            {
                ddlCountries.Enabled = true;
            }
            if (Convert.ToDouble(((TextBox)e.Row.FindControl("txtCoursePaid")).Text) > 0 || Convert.ToDouble(((TextBox)e.Row.FindControl("txtTransportPaid")).Text) > 0)
            {
                ((CheckBox)e.Row.FindControl("chk1")).Checked = true;
                if (Convert.ToDouble(((TextBox)e.Row.FindControl("txtCoursePaid")).Text) <= 0 && ((DropDownList)e.Row.FindControl("comPaymentMode")).Text == "BALANCE")
                {
                    ((TextBox)e.Row.FindControl("txtCoursePaid")).Enabled = false;
                    ((TextBox)e.Row.FindControl("txtPayClassFees")).Enabled = true;
                    ((TextBox)e.Row.FindControl("txtPayTransportFees")).Enabled = true;
                    ((TextBox)e.Row.FindControl("txtTransportPaid")).Enabled = false;
                    ((TextBox)e.Row.FindControl("txtPayClassFees")).Focus();
                }

                if (DATA2.First().PaymentStatus == "PAID")
                {
                    TextBox txtMonths = (e.Row.FindControl("txtMonths") as TextBox);
                    TextBox txtCourseFees = (e.Row.FindControl("txtCourseFees") as TextBox);
                    TextBox txtTransportFees = (e.Row.FindControl("txtTransportFees") as TextBox);
                    TextBox txtInstallmentDate = (e.Row.FindControl("txtInstallmentDate") as TextBox);
                    TextBox txtCoursePaid = (e.Row.FindControl("txtCoursePaid") as TextBox);
                    TextBox txtTransportPaid = (e.Row.FindControl("txtTransportPaid") as TextBox);
                    TextBox txtCourseBalance = (e.Row.FindControl("txtCourseBalance") as TextBox);
                    TextBox txtTransportBalance = (e.Row.FindControl("txtTransportBalance") as TextBox);
                    TextBox txtPayClassFees = (e.Row.FindControl("txtPayClassFees") as TextBox);
                    TextBox txtPayTransportFees = (e.Row.FindControl("txtPayTransportFees") as TextBox);
                    e.Row.BackColor = Color.Yellow;
                    ddlCountries.BackColor = Color.Yellow;
                    txtMonths.BackColor = Color.Yellow;
                    txtCourseFees.BackColor = Color.Yellow;
                    txtTransportFees.BackColor = Color.Yellow;
                    txtInstallmentDate.BackColor = Color.Yellow;
                    txtCoursePaid.BackColor = Color.Yellow;
                    txtTransportPaid.BackColor = Color.Yellow;
                    txtCourseBalance.BackColor = Color.Yellow;
                    txtTransportBalance.BackColor = Color.Yellow;
                    txtPayClassFees.BackColor = Color.Yellow;
                    txtPayTransportFees.BackColor = Color.Yellow;
                    txtPayClassFees.ForeColor = Color.Black;
                    txtPayTransportFees.ForeColor = Color.Black;
                }
            }
            else
            {

                ((CheckBox)e.Row.FindControl("chk1")).Checked = false;
            }
        }
    }
    protected void chk1_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {

            for (int Index = 0; Index < GridInstallments.Rows.Count; Index++)
            {
                if (((CheckBox)GridInstallments.Rows[Index].FindControl("chk1")).Checked)
                {

                    ((TextBox)GridInstallments.Rows[Index].FindControl("txtPayClassFees")).Enabled = true;
                    ((TextBox)GridInstallments.Rows[Index].FindControl("txtPayTransportFees")).Enabled = true;
                    ((TextBox)GridInstallments.Rows[Index].FindControl("txtPayClassFees")).Focus();
                }
                else
                {
                    //DropDownList ddlCountries = ((DropDownList)GridInstallments.Rows[Index].FindControl("comPaymentMode"));
                    //ddlCountries.Items.FindByValue("BALANCE").Selected = true;
                    // ((DropDownList)GridInstallments.Rows[Index].FindControl("comPaymentMode")).SelectedIndex = 1;
                    ((TextBox)GridInstallments.Rows[Index].FindControl("txtPayClassFees")).Enabled = false;
                    ((TextBox)GridInstallments.Rows[Index].FindControl("txtPayTransportFees")).Enabled = false;
                }
            }
            //Calc();
        }
        catch (Exception ex)
        { }
    }
    protected void chk1_MOnCheckedChanged(object sender, EventArgs e)
    {
        try
        {

            for (int Index = 0; Index < GridMonthlyInstallments.Rows.Count; Index++)
            {
                if (((CheckBox)GridMonthlyInstallments.Rows[Index].FindControl("chk1")).Checked)
                {
                    ((TextBox)GridMonthlyInstallments.Rows[Index].FindControl("txtPayClassFees")).Enabled = true;
                    ((TextBox)GridMonthlyInstallments.Rows[Index].FindControl("txtPayTransportFees")).Enabled = true;
                    ((TextBox)GridMonthlyInstallments.Rows[Index].FindControl("txtPayClassFees")).Focus();
                }
                else
                {
                    //DropDownList ddlCountries = ((DropDownList)GridInstallments.Rows[Index].FindControl("comPaymentMode"));
                    //ddlCountries.Items.FindByValue("BALANCE").Selected = true;
                    //((DropDownList)GridMonthlyInstallments.Rows[Index].FindControl("comPaymentMode")).SelectedIndex = 1;
                    ((TextBox)GridMonthlyInstallments.Rows[Index].FindControl("txtPayClassFees")).Enabled = false;
                    ((TextBox)GridMonthlyInstallments.Rows[Index].FindControl("txtPayTransportFees")).Enabled = false;
                }
            }
            //MCalc();
        }
        catch (Exception ex)
        { }
    }
    protected void txtCoursePaid_OnTextChanged(object sender, EventArgs e)
    {
        try
        {
            Calc();
        }
        catch (Exception ex)
        { }
    }
    protected void txtCoursePaid_MOnTextChanged(object sender, EventArgs e)
    {
        try
        {
            MCalc();
        }
        catch (Exception ex)
        { }
    }
    protected void txtCoursePaid_MOnTextChanged1(object sender, EventArgs e)
    {
        try
        {
            MCalc();
        }
        catch (Exception ex)
        { }
    }
    //private void Calc()
    //{
    //    string installmentName = "Installment";
    //    int rowCountGridForinstallmentName = 0;
    //    payMonthClassFee = "";
    //    payMonthTransportFee = "";
    //    classFessDetails = "";
    //    TransportFeesDetails = "";
    //    decimal CourseFees = 0, TransportFees = 0, CoursePaid = 0, TransportPaid = 0, CourseTotal = 0, TransportTotal = 0,
    //         AlreadyPaid = 0, PayClassFees = 0
    //        , CourseBalance = 0, TransportAlreadyPaid = 0, PayTransportFees = 0, TransportBalance = 0;
    //    for (int Index = 0; Index < GridInstallments.Rows.Count; Index++)
    //    {
    //        rowCountGridForinstallmentName++;
    //        int InstMonthlyId = Convert.ToInt32(((TextBox)GridInstallments.Rows[Index].FindControl("txtInstallmentId")).Text);
    //        var installments = from Cons in obj.Installments
    //                                  where Cons.InstallmentId == InstMonthlyId
    //                                  //&& Cons.Remove == false && Cons.CompanyId == Convert.ToInt32(Session["CompanyId"]) && Cons.SessionId == Convert.ToInt32(Session["SessionId"])
    //                                  select Cons;
    //        if (installments.Count() > 0)
    //        {
    //            AlreadyPaid = Convert.ToDecimal(installments.First().CousePaid);
    //            TransportAlreadyPaid = Convert.ToDecimal(installments.First().TransportPaid);
    //        }
    //        if (((CheckBox)GridInstallments.Rows[Index].FindControl("chk1")).Checked)
    //        {

    //            CourseFees = Convert.ToDecimal(((TextBox)GridInstallments.Rows[Index].FindControl("txtCourseFees")).Text);
    //            TransportFees = Convert.ToDecimal(((TextBox)GridInstallments.Rows[Index].FindControl("txtTransportFees")).Text);
    //            PayClassFees = Convert.ToDecimal(((TextBox)GridInstallments.Rows[Index].FindControl("txtPayClassFees")).Text);
    //            PayTransportFees = Convert.ToDecimal(((TextBox)GridInstallments.Rows[Index].FindControl("txtPayTransportFees")).Text);
    //            try
    //            {
    //                CoursePaid = Convert.ToDecimal(((TextBox)GridInstallments.Rows[Index].FindControl("txtPayClassFees")).Text);
    //            }
    //            catch (Exception ex)
    //            {
    //                ((TextBox)GridInstallments.Rows[Index].FindControl("txtPayClassFees")).Text = "0";
    //                CoursePaid = 0; }
    //            try
    //            {
    //                TransportPaid = Convert.ToDecimal(((TextBox)GridInstallments.Rows[Index].FindControl("txtPayTransportFees")).Text);
    //            }
    //            catch (Exception ex)
    //            {
    //                ((TextBox)GridInstallments.Rows[Index].FindControl("txtPayTransportFees")).Text = "0";
    //                TransportPaid = 0; }
    //            if (PayClassFees > 0)
    //            {
    //                if (payMonthClassFee == "")
    //                {
    //                    payMonthClassFee += installmentName + rowCountGridForinstallmentName;
    //                    classFessDetails += installmentName + rowCountGridForinstallmentName + "=" + PayClassFees;
    //                }
    //                else
    //                {
    //                    payMonthClassFee += ", " + installmentName + rowCountGridForinstallmentName;
    //                    classFessDetails += ", " + installmentName + rowCountGridForinstallmentName + "=" + PayClassFees;
    //                }
    //            }
    //            if (PayTransportFees > 0)
    //            {
    //                if (payMonthTransportFee == "")
    //                {
    //                    payMonthTransportFee += installmentName + rowCountGridForinstallmentName;
    //                    TransportFeesDetails += installmentName + rowCountGridForinstallmentName + "=" + PayTransportFees;
    //                }
    //                else
    //                {
    //                    payMonthTransportFee += ", " + installmentName + rowCountGridForinstallmentName;
    //                    TransportFeesDetails += ", " + installmentName + rowCountGridForinstallmentName + "=" + PayTransportFees;
    //                }
    //            }
    //            if (CoursePaid <= CourseFees)
    //            {
    //                decimal CourseBal = ((CourseFees - AlreadyPaid) - PayClassFees);
    //                if (CourseBal >= 0)
    //                {
    //                    ((TextBox)GridInstallments.Rows[Index].FindControl("txtCourseBalance")).Text = CourseBal.ToString();
    //                }
    //                else
    //                {
    //                    ((TextBox)GridInstallments.Rows[Index].FindControl("txtPayClassFees")).Text = "0";
    //                    CoursePaid = 0;
    //                }
    //                if (TransportPaid <= TransportFees)
    //                {
    //                    decimal TransportFeesBal = ((TransportFees - TransportAlreadyPaid) - TransportPaid);
    //                    if (TransportFeesBal >= 0)
    //                    {
    //                        ((TextBox)GridInstallments.Rows[Index].FindControl("txtTransportBalance")).Text = TransportFeesBal.ToString();
    //                        ((TextBox)GridInstallments.Rows[Index].FindControl("txtTransportBalance")).Focus();
    //                    }
    //                    else
    //                    {
    //                        ((TextBox)GridInstallments.Rows[Index].FindControl("txtPayTransportFees")).Text = "0";
    //                        TransportPaid = 0;
    //                    }
    //                    //((TextBox)GridInstallments.Rows[Index].FindControl("txtCourseBalance")).Text = (CourseFees - CoursePaid).ToString();
    //                    //((TextBox)GridInstallments.Rows[Index].FindControl("txtTransportBalance")).Text = (TransportFees - TransportPaid).ToString();
    //                    //((TextBox)GridInstallments.Rows[Index].FindControl("txtCourseBalance")).Focus();
    //                }
    //                else
    //                {
    //                    ((TextBox)GridInstallments.Rows[Index].FindControl("txtPayTransportFees")).Text = "0";
    //                    TransportPaid = 0;
    //                    msg = "Please enter transport paid fees less then or equal to transport fees";
    //                    ScriptManager.RegisterStartupScript(this, GetType(), "MessageShow", "MessageShow('" + msg + "');", true);
    //                    ((TextBox)GridInstallments.Rows[Index].FindControl("txtPayTransportFees")).Focus();
    //                }
    //            }
    //            else
    //            {
    //                ((TextBox)GridInstallments.Rows[Index].FindControl("txtPayClassFees")).Text = "0";
    //                CoursePaid = 0;
    //                msg = "Please enter course paid fees less then or equal to course fees";
    //                ScriptManager.RegisterStartupScript(this, GetType(), "MessageShow", "MessageShow('" + msg + "');", true);
    //                ((TextBox)GridInstallments.Rows[Index].FindControl("txtPayClassFees")).Focus();

    //            }

    //            CourseTotal = CourseTotal + CoursePaid;
    //            TransportTotal = TransportTotal + TransportPaid;

    //            var DATA = from Cons in obj.Addmisions
    //                       where Cons.AdmissionNo == txtAdminNo.Text && Cons.Remove == false && Cons.CompanyId == Convert.ToInt32(Session["CompanyId"]) && Cons.SessionId == Convert.ToInt32(Session["SessionId"])
    //                   select Cons;
    //                     admId = DATA.First().AdmissionId;

    //            if (lblPaymentId.Text != "0")
    //            {
    //                var CFees = (from Cons in obj.Payments
    //                             where Cons.AdmissionId == admId && Cons.PaymentId != Convert.ToInt32(lblPaymentId.Text) && Cons.Remove == false && Cons.ComapanyId == Convert.ToInt32(Session["CompanyId"]) && Cons.SessionId == Convert.ToInt32(Session["SessionId"])
    //                            select Cons.CourseFees).Sum();

    //                var TFees = (from Cons in obj.Payments
    //                             where Cons.AdmissionId == admId && Cons.PaymentId != Convert.ToInt32(lblPaymentId.Text) && Cons.Remove == false && Cons.ComapanyId == Convert.ToInt32(Session["CompanyId"]) && Cons.SessionId == Convert.ToInt32(Session["SessionId"])
    //                            select Cons.TransportFees).Sum();
    //                if (CFees == null) { CFees = 0; } if (TFees == null) { TFees = 0; }
    //                //txtInstallCourseFees.Text = (CourseTotal - Convert.ToDecimal(CFees)).ToString();
    //                //txtInstallTransportFees.Text = (TransportTotal - Convert.ToDecimal(TFees)).ToString();
    //                txtInstallCourseFees.Text = CourseTotal.ToString();
    //                txtInstallTransportFees.Text = TransportTotal.ToString();
    //                txtTotal.Text = ((CourseTotal + TransportTotal) - Convert.ToDecimal(CFees + TFees)).ToString();
    //            }
    //            else
    //            {
    //                var CFees = (from Cons in obj.Payments
    //                             where Cons.AdmissionId == admId && Cons.Remove == false && Cons.ComapanyId == Convert.ToInt32(Session["CompanyId"]) && Cons.SessionId == Convert.ToInt32(Session["SessionId"])
    //                            select Cons.CourseFees).Sum();

    //                var TFees = (from Cons in obj.Payments
    //                             where Cons.AdmissionId == admId && Cons.Remove == false && Cons.ComapanyId == Convert.ToInt32(Session["CompanyId"]) && Cons.SessionId == Convert.ToInt32(Session["SessionId"])
    //                            select Cons.TransportFees).Sum();
    //                if (CFees == null) { CFees = 0; } if (TFees == null) { TFees = 0; }
    //                //txtInstallCourseFees.Text = (CourseTotal - Convert.ToDecimal(CFees)).ToString();
    //                //txtInstallTransportFees.Text = (TransportTotal - Convert.ToDecimal(TFees)).ToString();
    //                txtInstallCourseFees.Text = CourseTotal.ToString();
    //                txtInstallTransportFees.Text = TransportTotal.ToString();
    //                txtTotal.Text = ((CourseTotal + TransportTotal) - Convert.ToDecimal(CFees + TFees)).ToString();
    //            }
    //        }
    //        else
    //        {
    //            CourseFees = Convert.ToDecimal(((TextBox)GridInstallments.Rows[Index].FindControl("txtCourseFees")).Text);
    //            TransportFees = Convert.ToDecimal(((TextBox)GridInstallments.Rows[Index].FindControl("txtTransportFees")).Text);
    //            ((TextBox)GridInstallments.Rows[Index].FindControl("txtCoursePaid")).Text = "0.00";
    //            ((TextBox)GridInstallments.Rows[Index].FindControl("txtTransportPaid")).Text = "0.00";
    //            CoursePaid = 0;
    //            TransportPaid = 0;

    //            if (CoursePaid <= CourseFees)
    //            {
    //                if (TransportPaid <= TransportFees)
    //                {
    //                    ((TextBox)GridInstallments.Rows[Index].FindControl("txtCourseBalance")).Text = (CourseFees - CoursePaid).ToString();
    //                    ((TextBox)GridInstallments.Rows[Index].FindControl("txtTransportBalance")).Text = (TransportFees - TransportPaid).ToString();
    //                }
    //                else
    //                {
    //                    msg = "Please enter transport paid fees less then or equal to transport fees";
    //                    ScriptManager.RegisterStartupScript(this, GetType(), "MessageShow", "MessageShow('" + msg + "');", true);
    //                    ((TextBox)GridInstallments.Rows[Index].FindControl("txtTransportPaid")).Focus();
    //                }
    //            }
    //            else
    //            {
    //                msg = "Please enter course paid fees less then or equal to course fees";
    //                ScriptManager.RegisterStartupScript(this, GetType(), "MessageShow", "MessageShow('" + msg + "');", true);
    //                ((TextBox)GridInstallments.Rows[Index].FindControl("txtCoursePaid")).Focus();

    //            }
    //            ((TextBox)GridInstallments.Rows[Index].FindControl("txtPayClassFees")).Enabled = false;
    //            ((TextBox)GridInstallments.Rows[Index].FindControl("txtTransportPaid")).Enabled = false;
    //        }
    //        CourseBalance = Convert.ToDecimal(((TextBox)GridInstallments.Rows[Index].FindControl("txtCourseBalance")).Text);
    //        TransportBalance = Convert.ToDecimal(((TextBox)GridInstallments.Rows[Index].FindControl("txtTransportBalance")).Text);
    //        if (CourseBalance == 0 && TransportBalance == 0)
    //        {
    //            ((DropDownList)GridInstallments.Rows[Index].FindControl("comPaymentMode")).SelectedIndex = 0;
    //        }
    //        else
    //        {
    //            ((DropDownList)GridInstallments.Rows[Index].FindControl("comPaymentMode")).SelectedIndex = 1;
    //        }
    //    }
    //    CalcOther();
    //}

    //private void MCalc()
    //{
    //    payMonthClassFee = "";
    //    payMonthTransportFee = "";
    //    classFessDetails = "";
    //    TransportFeesDetails = "";
    //    string FeeStatusForMonthName = "";
    //    decimal CourseFees = 0, TransportFees = 0, CoursePaid = 0, TransportPaid = 0, CourseTotal = 0, TransportTotal = 0, AlreadyPaid = 0, PayClassFees = 0
    //        , CourseBalance = 0, TransportAlreadyPaid = 0, PayTransportFees = 0, TransportBalance = 0;
    //    for (int Index = 0; Index < GridMonthlyInstallments.Rows.Count; Index++)
    //    {
    //        string MonthName = ((TextBox)GridMonthlyInstallments.Rows[Index].FindControl("txtMonths")).Text;
    //        PayClassFees = Convert.ToDecimal(((TextBox)GridMonthlyInstallments.Rows[Index].FindControl("txtPayClassFees")).Text);
    //        int InstMonthlyId = Convert.ToInt32(((TextBox)GridMonthlyInstallments.Rows[Index].FindControl("txtMonthlyInstId")).Text);
    //        var monthlyInstallments = from Cons in obj.MonthlyInstallments
    //                    where Cons.MonthlyInstId == InstMonthlyId 
    //                    //&& Cons.Remove == false && Cons.CompanyId == Convert.ToInt32(Session["CompanyId"]) && Cons.SessionId == Convert.ToInt32(Session["SessionId"])
    //                    select Cons;
    //        if (monthlyInstallments.Count() > 0)
    //        {
    //            FeeStatusForMonthName=monthlyInstallments.First().PaymentStatus;
    //            AlreadyPaid = Convert.ToDecimal(monthlyInstallments.First().CousePaid);
    //            TransportAlreadyPaid = Convert.ToDecimal(monthlyInstallments.First().TransportPaid);
    //        }
    //        if (((CheckBox)GridMonthlyInstallments.Rows[Index].FindControl("chk1")).Checked)
    //        {
    //            if (FeeStatusForMonthName != "PAID")
    //            {
    //                PayClassFees = 0;
    //                PayTransportFees = 0;
    //                CourseFees = Convert.ToDecimal(((TextBox)GridMonthlyInstallments.Rows[Index].FindControl("txtCourseFees")).Text);
    //                TransportFees = Convert.ToDecimal(((TextBox)GridMonthlyInstallments.Rows[Index].FindControl("txtTransportFees")).Text);
    //                PayClassFees = Convert.ToDecimal(((TextBox)GridMonthlyInstallments.Rows[Index].FindControl("txtPayClassFees")).Text);
    //                PayTransportFees = Convert.ToDecimal(((TextBox)GridMonthlyInstallments.Rows[Index].FindControl("txtPayTransportFees")).Text);
    //                try
    //                {
    //                    CoursePaid = Convert.ToDecimal(((TextBox)GridMonthlyInstallments.Rows[Index].FindControl("txtPayClassFees")).Text);
    //                }
    //                catch (Exception ex)
    //                {
    //                    ((TextBox)GridMonthlyInstallments.Rows[Index].FindControl("txtPayClassFees")).Text = "0";
    //                    CoursePaid = 0;
    //                }
    //                try
    //                {
    //                    TransportPaid = Convert.ToDecimal(((TextBox)GridMonthlyInstallments.Rows[Index].FindControl("txtPayTransportFees")).Text);
    //                }
    //                catch (Exception ex)
    //                {
    //                    ((TextBox)GridMonthlyInstallments.Rows[Index].FindControl("txtPayTransportFees")).Text = "0";
    //                    TransportPaid = 0;
    //                }
    //                if (PayClassFees > 0)
    //                {
    //                    if (payMonthClassFee == "")
    //                    {
    //                        payMonthClassFee += ((TextBox)GridMonthlyInstallments.Rows[Index].FindControl("txtMonths")).Text;
    //                        classFessDetails += ((TextBox)GridMonthlyInstallments.Rows[Index].FindControl("txtMonths")).Text + "=" + PayClassFees;
    //                    }
    //                    else
    //                    {
    //                        payMonthClassFee += ", " + ((TextBox)GridMonthlyInstallments.Rows[Index].FindControl("txtMonths")).Text;
    //                        classFessDetails += ", " + ((TextBox)GridMonthlyInstallments.Rows[Index].FindControl("txtMonths")).Text + "=" + PayClassFees;
    //                    }
    //                }
    //                if (PayTransportFees > 0)
    //                {
    //                    if (payMonthTransportFee == "")
    //                    {
    //                        payMonthTransportFee += ((TextBox)GridMonthlyInstallments.Rows[Index].FindControl("txtMonths")).Text;
    //                        TransportFeesDetails += ((TextBox)GridMonthlyInstallments.Rows[Index].FindControl("txtMonths")).Text + "=" + PayTransportFees;
    //                    }
    //                    else
    //                    {
    //                        payMonthTransportFee += ", " + ((TextBox)GridMonthlyInstallments.Rows[Index].FindControl("txtMonths")).Text;
    //                        TransportFeesDetails += ", " + ((TextBox)GridMonthlyInstallments.Rows[Index].FindControl("txtMonths")).Text + "=" + PayTransportFees;
    //                    }
    //                }
    //                if (CoursePaid <= CourseFees)
    //                {

    //                    decimal CourseBal = ((CourseFees - AlreadyPaid) - PayClassFees);
    //                    if (CourseBal >= 0)
    //                    {
    //                        ((TextBox)GridMonthlyInstallments.Rows[Index].FindControl("txtCourseBalance")).Text = CourseBal.ToString();
    //                    }
    //                    else
    //                    {
    //                        ((TextBox)GridMonthlyInstallments.Rows[Index].FindControl("txtPayClassFees")).Text = "0";
    //                        ((TextBox)GridMonthlyInstallments.Rows[Index].FindControl("txtCourseBalance")).Text = (CourseFees - AlreadyPaid).ToString();
    //                        CoursePaid = 0;
    //                    }
    //                    if (TransportPaid <= TransportFees)
    //                    {
    //                        decimal TransportFeesBal = ((TransportFees - TransportAlreadyPaid) - TransportPaid);
    //                        if (TransportFeesBal >= 0)
    //                        {
    //                            ((TextBox)GridMonthlyInstallments.Rows[Index].FindControl("txtTransportBalance")).Text = TransportFeesBal.ToString();
    //                            ((TextBox)GridMonthlyInstallments.Rows[Index].FindControl("txtTransportBalance")).Focus();
    //                        }
    //                        else
    //                        {
    //                            ((TextBox)GridMonthlyInstallments.Rows[Index].FindControl("txtTransportBalance")).Text = (TransportFees - TransportAlreadyPaid).ToString();
    //                            ((TextBox)GridMonthlyInstallments.Rows[Index].FindControl("txtPayTransportFees")).Text = "0";
    //                            TransportPaid = 0;
    //                        }


    //                        //((TextBox)GridMonthlyInstallments.Rows[Index].FindControl("txtTransportBalance")).Text = (TransportFees - TransportPaid).ToString();
    //                        //((TextBox)GridMonthlyInstallments.Rows[Index].FindControl("txtTransportBalance")).Focus();
    //                    }
    //                    else
    //                    {
    //                        ((TextBox)GridMonthlyInstallments.Rows[Index].FindControl("txtPayTransportFees")).Text = "0";
    //                        TransportPaid = 0;
    //                        msg = "Please enter transport paid fees less then or equal to transport fees";
    //                        ScriptManager.RegisterStartupScript(this, GetType(), "MessageShow", "MessageShow('" + msg + "');", true);
    //                        ((TextBox)GridMonthlyInstallments.Rows[Index].FindControl("txtPayTransportFees")).Focus();
    //                    }
    //                }
    //                else
    //                {
    //                    ((TextBox)GridMonthlyInstallments.Rows[Index].FindControl("txtPayClassFees")).Text = "0";
    //                    CoursePaid = 0;
    //                    msg = "Please enter course paid fees less then or equal to course fees";
    //                    ScriptManager.RegisterStartupScript(this, GetType(), "MessageShow", "MessageShow('" + msg + "');", true);
    //                    ((TextBox)GridMonthlyInstallments.Rows[Index].FindControl("txtPayClassFees")).Focus();

    //                }
    //                CourseTotal = CourseTotal + CoursePaid;
    //                TransportTotal = TransportTotal + TransportPaid;
    //                var DATA = from Cons in obj.Addmisions
    //                           where Cons.AdmissionNo == txtAdminNo.Text && Cons.Remove == false && Cons.CompanyId == Convert.ToInt32(Session["CompanyId"]) && Cons.SessionId == Convert.ToInt32(Session["SessionId"])
    //                           select Cons;
    //                admId = DATA.First().AdmissionId;
    //                if (lblPaymentId.Text != "0")
    //                {
    //                    var CFees = (from Cons in obj.Payments
    //                                 where Cons.AdmissionId == admId && Cons.PaymentId != Convert.ToInt32(lblPaymentId.Text) && Cons.Remove == false && Cons.ComapanyId == Convert.ToInt32(Session["CompanyId"]) && Cons.SessionId == Convert.ToInt32(Session["SessionId"])
    //                                 select Cons.CourseFees).Sum();

    //                    var TFees = (from Cons in obj.Payments
    //                                 where Cons.AdmissionId == admId && Cons.PaymentId != Convert.ToInt32(lblPaymentId.Text) && Cons.Remove == false && Cons.ComapanyId == Convert.ToInt32(Session["CompanyId"]) && Cons.SessionId == Convert.ToInt32(Session["SessionId"])
    //                                 select Cons.TransportFees).Sum();
    //                    if (CFees == null) { CFees = 0; } if (TFees == null) { TFees = 0; }
    //                    //txtInstallCourseFees.Text = (CourseTotal - Convert.ToDecimal(CFees)).ToString();
    //                    txtInstallCourseFees.Text = CourseTotal.ToString();
    //                    //txtInstallTransportFees.Text = (TransportTotal - Convert.ToDecimal(TFees)).ToString();
    //                    txtInstallTransportFees.Text = TransportTotal.ToString();
    //                    txtTotal.Text = ((CourseTotal + TransportTotal) - Convert.ToDecimal(CFees + TFees)).ToString();
    //                }
    //                else
    //                {
    //                    var CFees = (from Cons in obj.Payments
    //                                 where Cons.AdmissionId == admId && Cons.Remove == false && Cons.ComapanyId == Convert.ToInt32(Session["CompanyId"]) && Cons.SessionId == Convert.ToInt32(Session["SessionId"])
    //                                 select Cons.CourseFees).Sum();

    //                    var TFees = (from Cons in obj.Payments
    //                                 where Cons.AdmissionId == admId && Cons.Remove == false && Cons.ComapanyId == Convert.ToInt32(Session["CompanyId"]) && Cons.SessionId == Convert.ToInt32(Session["SessionId"])
    //                                 select Cons.TransportFees).Sum();
    //                    if (CFees == null) { CFees = 0; } if (TFees == null) { TFees = 0; }
    //                    //txtInstallCourseFees.Text = (CourseTotal - Convert.ToDecimal(CFees)).ToString();
    //                    txtInstallCourseFees.Text = CourseTotal.ToString();
    //                    //txtInstallTransportFees.Text = (TransportTotal - Convert.ToDecimal(TFees)).ToString();
    //                    txtInstallTransportFees.Text = TransportTotal.ToString();
    //                    txtTotal.Text = ((CourseTotal + TransportTotal) - Convert.ToDecimal(CFees + TFees)).ToString();
    //                }
    //            }
    //        }
    //        //not check
    //        else
    //        {
    //            CourseFees = Convert.ToDecimal(((TextBox)GridMonthlyInstallments.Rows[Index].FindControl("txtCourseFees")).Text);
    //            TransportFees = Convert.ToDecimal(((TextBox)GridMonthlyInstallments.Rows[Index].FindControl("txtTransportFees")).Text);
    //            ((TextBox)GridMonthlyInstallments.Rows[Index].FindControl("txtPayClassFees")).Text = "0.00";
    //            ((TextBox)GridMonthlyInstallments.Rows[Index].FindControl("txtTransportPaid")).Text = "0.00";
    //            CoursePaid = 0;
    //            TransportPaid = 0;

    //            if (CoursePaid <= CourseFees)
    //            {
    //                ((TextBox)GridMonthlyInstallments.Rows[Index].FindControl("txtCourseBalance")).Text = (CourseFees - CoursePaid).ToString();
    //                if (TransportPaid <= TransportFees)
    //                {

    //                    ((TextBox)GridMonthlyInstallments.Rows[Index].FindControl("txtTransportBalance")).Text = (TransportFees - TransportPaid).ToString();
    //                }
    //                else
    //                {
    //                    msg = "Please enter transport paid fees less then or equal to transport fees";
    //                    ScriptManager.RegisterStartupScript(this, GetType(), "MessageShow", "MessageShow('" + msg + "');", true);
    //                    ((TextBox)GridMonthlyInstallments.Rows[Index].FindControl("txtTransportPaid")).Focus();
    //                }
    //            }
    //            else
    //            {
    //                msg = "Please enter course paid fees less then or equal to course fees";
    //                ScriptManager.RegisterStartupScript(this, GetType(), "MessageShow", "MessageShow('" + msg + "');", true);
    //                ((TextBox)GridMonthlyInstallments.Rows[Index].FindControl("txtPayClassFees")).Focus();

    //            }
    //            ((TextBox)GridMonthlyInstallments.Rows[Index].FindControl("txtPayClassFees")).Enabled = false;
    //            ((TextBox)GridMonthlyInstallments.Rows[Index].FindControl("txtTransportPaid")).Enabled = false;
    //        }
    //        CourseBalance = Convert.ToDecimal(((TextBox)GridMonthlyInstallments.Rows[Index].FindControl("txtCourseBalance")).Text);
    //        TransportBalance = Convert.ToDecimal(((TextBox)GridMonthlyInstallments.Rows[Index].FindControl("txtTransportBalance")).Text);
    //        if (CourseBalance == 0 && TransportBalance == 0)
    //        {
    //            ((DropDownList)GridMonthlyInstallments.Rows[Index].FindControl("comPaymentMode")).SelectedIndex = 0;
    //        }
    //        else
    //        {
    //            ((DropDownList)GridMonthlyInstallments.Rows[Index].FindControl("comPaymentMode")).SelectedIndex = 1;
    //        }
    //    }
    //    CalcOther();
    //}
    protected void txtTransportPaid_OnTextChanged(object sender, EventArgs e)
    {
        try
        {
            Calc();
        }
        catch (Exception ex)
        { }
    }
    protected void txtTransportPaid_MOnTextChanged(object sender, EventArgs e)
    {
        try
        {
            MCalc();
        }
        catch (Exception ex)
        { }
    }
    protected void chkOtherFees_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (chkOtherFees.Checked)
            {
                GridOtherFee.Visible = true;
                comOtherFeesType.Enabled = false;
                comOtherFeesType.SelectedIndex = 0;
                txtOtherFees.Text = string.Empty;
                CalcOther();
            }
            else
            {
                GridOtherFee.Visible = false;
                txtOtherFees.Text = string.Empty;
                comOtherFeesType.SelectedIndex = 0;
                comOtherFeesType.Enabled = false;
                CalcOther();
            }
        }
        catch (Exception ex)
        { }
    }
    protected void comOtherFeesType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (comOtherFeesType.SelectedIndex != 0)
            {
                if (comOtherFeesType.SelectedIndex == 1)
                {
                    var D1 = from Cons in obj.Addmisions
                             where Cons.AdmissionNo == txtAdminNo.Text
                             select Cons;
                    string str = "", year = "";
                    str = Convert.ToDateTime(D1.First().AdmissionDate).ToString("yyyy/MM/dd");
                    year = str.Substring(0, 4);
                    string str1 = Convert.ToDateTime(DateTime.Now).Year.ToString();
                    if (year != str1)
                    {
                        IEnumerable<Other_Fee> Other_Fees = from Cons in obj.Other_Fees
                                                            where Cons.CourseId == Convert.ToInt32(Session["AdmissionCourseId"]) && Cons.FeesType != "CAUTION MONEY" && Cons.Remove == false && Cons.CompanyId == Convert.ToInt32(Session["CompanyId"]) && Cons.SessionId == Convert.ToInt32(Session["SessionId"])
                                                            select Cons;
                        txtOtherFees.Text = Convert.ToDouble(Other_Fees.Sum(s => s.Fees.Value)).ToString();
                        CalcOther();
                    }
                    else
                    {
                        IEnumerable<Other_Fee> Other_Fees = from Cons in obj.Other_Fees
                                                            where Cons.CourseId == Convert.ToInt32(Session["AdmissionCourseId"]) && Cons.Remove == false && Cons.CompanyId == Convert.ToInt32(Session["CompanyId"]) && Cons.SessionId == Convert.ToInt32(Session["SessionId"])
                                                            select Cons;
                        txtOtherFees.Text = Convert.ToDouble(Other_Fees.Sum(s => s.Fees.Value)).ToString();
                        CalcOther();
                    }
                }
                else
                {
                    var Other_Fee = from Cons in obj.Other_Fees
                                    where Cons.OtherFeesId == Convert.ToInt32(comOtherFeesType.SelectedValue) && Cons.Remove == false && Cons.CompanyId == Convert.ToInt32(Session["CompanyId"]) && Cons.SessionId == Convert.ToInt32(Session["SessionId"])
                                    select Cons;
                    txtOtherFees.Text = Other_Fee.First().Fees.ToString();
                    CalcOther();
                }
            }
            else
            {
                txtOtherFees.Text = "";
                CalcOther();
            }
        }
        catch (Exception ex)
        { }
    }
    protected void txtAdmiFees_TextChanged(object sender, EventArgs e)
    {
        CalcOther();
    }
    private void CalcOther()
    {
        try
        {
            decimal AdmFees = 0, LateFees = 0, CourseFees = 0, TransportFees = 0, OtherFees = 0, Discount = 0, PreviousPaid = 0;
            try
            {
                AdmFees = Convert.ToDecimal(txtAdmiFees.Text);
            }
            catch (Exception ex)
            { AdmFees = 0; }
            try
            {
                PreviousPaid = Convert.ToDecimal(txtPaidPreviousBalance.Text);
            }
            catch (Exception ex)
            { PreviousPaid = 0; }
            try
            {
                Discount = Convert.ToDecimal(txtDiscount.Text);
            }
            catch (Exception ex)
            { Discount = 0; }
            try
            {
                LateFees = Convert.ToDecimal(txtLateFees.Text);
            }
            catch (Exception ex)
            { LateFees = 0; }
            try
            {
                CourseFees = Convert.ToDecimal(txtInstallCourseFees.Text);
            }
            catch (Exception ex)
            { CourseFees = 0; }
            try
            {
                TransportFees = Convert.ToDecimal(txtInstallTransportFees.Text);
            }
            catch (Exception ex)
            { TransportFees = 0; }
            try
            {
                OtherFees = Convert.ToDecimal(txtOtherFees.Text);
            }
            catch (Exception ex)
            { OtherFees = 0; }
            txtTotal.Text = ((AdmFees + LateFees + CourseFees + TransportFees + OtherFees + PreviousPaid) - Discount).ToString();
        }
        catch (Exception ex)
        { }
    }
    protected void txtLateFees_TextChanged(object sender, EventArgs e)
    {
        CalcOther();
    }
    protected void txtDiscount_TextChanged(object sender, EventArgs e)
    {
        CalcOther();
    }
    protected void txtInstallCourseFees_TextChanged(object sender, EventArgs e)
    {
        CalcOther();
    }
    protected void txtInstallTransportFees_TextChanged(object sender, EventArgs e)
    {
        CalcOther();
    }
    private void Calc()
    {
        string installmentName = "Installment";
        int rowCountGridForinstallmentName = 0;
        payMonthClassFee = "";
        payMonthTransportFee = "";
        classFessDetails = "";
        TransportFeesDetails = "";
        txtotherFessDetails.Text = "";
        decimal CourseFees = 0, TransportFees = 0, CoursePaid = 0, TransportPaid = 0, CourseTotal = 0, TransportTotal = 0,
             AlreadyPaid = 0, PayClassFees = 0
            , CourseBalance = 0, TransportAlreadyPaid = 0, PayTransportFees = 0, TransportBalance = 0;
        for (int Index = 0; Index < GridInstallments.Rows.Count; Index++)
        {
            rowCountGridForinstallmentName++;
            int InstMonthlyId = Convert.ToInt32(((TextBox)GridInstallments.Rows[Index].FindControl("txtInstallmentId")).Text);
            var installments = from Cons in obj.Installments
                               where Cons.InstallmentId == InstMonthlyId
                               && Cons.CompanyId == Convert.ToInt32(Session["CompanyId"])
                               //&& Cons.Remove == false && Cons.CompanyId == Convert.ToInt32(Session["CompanyId"]) && Cons.SessionId == Convert.ToInt32(Session["SessionId"])
                               select Cons;
            if (installments.Count() > 0)
            {
                AlreadyPaid = Convert.ToDecimal(installments.First().CousePaid);
                TransportAlreadyPaid = Convert.ToDecimal(installments.First().TransportPaid);
            }
            if (((CheckBox)GridInstallments.Rows[Index].FindControl("chk1")).Checked)
            {

                CourseFees = Convert.ToDecimal(((TextBox)GridInstallments.Rows[Index].FindControl("txtCourseFees")).Text);
                TransportFees = Convert.ToDecimal(((TextBox)GridInstallments.Rows[Index].FindControl("txtTransportFees")).Text);
                PayClassFees = Convert.ToDecimal(((TextBox)GridInstallments.Rows[Index].FindControl("txtPayClassFees")).Text);
                PayTransportFees = Convert.ToDecimal(((TextBox)GridInstallments.Rows[Index].FindControl("txtPayTransportFees")).Text);
                try
                {
                    CoursePaid = Convert.ToDecimal(((TextBox)GridInstallments.Rows[Index].FindControl("txtPayClassFees")).Text);
                }
                catch (Exception ex)
                {
                    ((TextBox)GridInstallments.Rows[Index].FindControl("txtPayClassFees")).Text = "0";
                    CoursePaid = 0;
                }
                try
                {
                    TransportPaid = Convert.ToDecimal(((TextBox)GridInstallments.Rows[Index].FindControl("txtPayTransportFees")).Text);
                }
                catch (Exception ex)
                {
                    ((TextBox)GridInstallments.Rows[Index].FindControl("txtPayTransportFees")).Text = "0";
                    TransportPaid = 0;
                }
                if (PayClassFees > 0)
                {
                    if (payMonthClassFee == "")
                    {
                        payMonthClassFee += installmentName + rowCountGridForinstallmentName;
                        classFessDetails += installmentName + rowCountGridForinstallmentName + "=" + PayClassFees;
                    }
                    else
                    {
                        payMonthClassFee += ", " + installmentName + rowCountGridForinstallmentName;
                        classFessDetails += ", " + installmentName + rowCountGridForinstallmentName + "=" + PayClassFees;
                    }
                    txtpayMonthClassFee.Text = payMonthClassFee;
                    txtclassFessDetails.Text = classFessDetails;
                }
                if (PayTransportFees > 0)
                {
                    if (payMonthTransportFee == "")
                    {
                        payMonthTransportFee += installmentName + rowCountGridForinstallmentName;
                        TransportFeesDetails += installmentName + rowCountGridForinstallmentName + "=" + PayTransportFees;
                    }
                    else
                    {
                        payMonthTransportFee += ", " + installmentName + rowCountGridForinstallmentName;
                        TransportFeesDetails += ", " + installmentName + rowCountGridForinstallmentName + "=" + PayTransportFees;
                    }
                    txtpayMonthTransportFee.Text = payMonthTransportFee;
                    txtTransportFeesDetails.Text = TransportFeesDetails;
                }
                if (CoursePaid <= CourseFees)
                {
                    decimal CourseBal = ((CourseFees - AlreadyPaid) - PayClassFees);
                    if (CourseBal >= 0)
                    {
                        ((TextBox)GridInstallments.Rows[Index].FindControl("txtCourseBalance")).Text = CourseBal.ToString();
                    }
                    else
                    {
                        ((TextBox)GridInstallments.Rows[Index].FindControl("txtPayClassFees")).Text = "0";
                        CoursePaid = 0;
                    }
                    if (TransportPaid <= TransportFees)
                    {
                        decimal TransportFeesBal = ((TransportFees - TransportAlreadyPaid) - TransportPaid);
                        if (TransportFeesBal >= 0)
                        {
                            ((TextBox)GridInstallments.Rows[Index].FindControl("txtTransportBalance")).Text = TransportFeesBal.ToString();
                            ((TextBox)GridInstallments.Rows[Index].FindControl("txtTransportBalance")).Focus();
                        }
                        else
                        {
                            ((TextBox)GridInstallments.Rows[Index].FindControl("txtPayTransportFees")).Text = "0";
                            TransportPaid = 0;
                        }
                        //((TextBox)GridInstallments.Rows[Index].FindControl("txtCourseBalance")).Text = (CourseFees - CoursePaid).ToString();
                        //((TextBox)GridInstallments.Rows[Index].FindControl("txtTransportBalance")).Text = (TransportFees - TransportPaid).ToString();
                        //((TextBox)GridInstallments.Rows[Index].FindControl("txtCourseBalance")).Focus();
                    }
                    else
                    {
                        ((TextBox)GridInstallments.Rows[Index].FindControl("txtPayTransportFees")).Text = "0";
                        TransportPaid = 0;
                        msg = "Please enter transport paid fees less then or equal to transport fees";
                        ScriptManager.RegisterStartupScript(this, GetType(), "MessageShow", "MessageShow('" + msg + "');", true);
                        ((TextBox)GridInstallments.Rows[Index].FindControl("txtPayTransportFees")).Focus();
                    }
                }
                else
                {
                    ((TextBox)GridInstallments.Rows[Index].FindControl("txtPayClassFees")).Text = "0";
                    CoursePaid = 0;
                    msg = "Please enter course paid fees less then or equal to course fees";
                    ScriptManager.RegisterStartupScript(this, GetType(), "MessageShow", "MessageShow('" + msg + "');", true);
                    ((TextBox)GridInstallments.Rows[Index].FindControl("txtPayClassFees")).Focus();

                }

                CourseTotal = CourseTotal + CoursePaid;
                TransportTotal = TransportTotal + TransportPaid;

                var DATA = from Cons in obj.Addmisions
                           where Cons.AdmissionNo == txtAdminNo.Text && Cons.Remove == false && Cons.CompanyId == Convert.ToInt32(Session["CompanyId"]) && Cons.SessionId == Convert.ToInt32(Session["SessionId"])
                           select Cons;
                admId = DATA.First().AdmissionId;

                if (lblPaymentId.Text != "0")
                {
                    var CFees = (from Cons in obj.Payments
                                 where Cons.AdmissionId == admId && Cons.PaymentId != Convert.ToInt32(lblPaymentId.Text) && Cons.Remove == false && Cons.ComapanyId == Convert.ToInt32(Session["CompanyId"]) && Cons.SessionId == Convert.ToInt32(Session["SessionId"])
                                 select Cons.CourseFees).Sum();

                    var TFees = (from Cons in obj.Payments
                                 where Cons.AdmissionId == admId && Cons.PaymentId != Convert.ToInt32(lblPaymentId.Text) && Cons.Remove == false && Cons.ComapanyId == Convert.ToInt32(Session["CompanyId"]) && Cons.SessionId == Convert.ToInt32(Session["SessionId"])
                                 select Cons.TransportFees).Sum();
                    if (CFees == null) { CFees = 0; } if (TFees == null) { TFees = 0; }
                    //txtInstallCourseFees.Text = (CourseTotal - Convert.ToDecimal(CFees)).ToString();
                    //txtInstallTransportFees.Text = (TransportTotal - Convert.ToDecimal(TFees)).ToString();
                    txtInstallCourseFees.Text = CourseTotal.ToString();
                    txtInstallTransportFees.Text = TransportTotal.ToString();
                    txtTotal.Text = ((CourseTotal + TransportTotal) - Convert.ToDecimal(CFees + TFees)).ToString();
                }
                else
                {
                    var CFees = (from Cons in obj.Payments
                                 where Cons.AdmissionId == admId && Cons.Remove == false && Cons.ComapanyId == Convert.ToInt32(Session["CompanyId"]) && Cons.SessionId == Convert.ToInt32(Session["SessionId"])
                                 select Cons.CourseFees).Sum();

                    var TFees = (from Cons in obj.Payments
                                 where Cons.AdmissionId == admId && Cons.Remove == false && Cons.ComapanyId == Convert.ToInt32(Session["CompanyId"]) && Cons.SessionId == Convert.ToInt32(Session["SessionId"])
                                 select Cons.TransportFees).Sum();
                    if (CFees == null) { CFees = 0; } if (TFees == null) { TFees = 0; }
                    //txtInstallCourseFees.Text = (CourseTotal - Convert.ToDecimal(CFees)).ToString();
                    //txtInstallTransportFees.Text = (TransportTotal - Convert.ToDecimal(TFees)).ToString();
                    txtInstallCourseFees.Text = CourseTotal.ToString();
                    txtInstallTransportFees.Text = TransportTotal.ToString();
                    txtTotal.Text = ((CourseTotal + TransportTotal) - Convert.ToDecimal(CFees + TFees)).ToString();
                }
            }
            else
            {
                CourseFees = Convert.ToDecimal(((TextBox)GridInstallments.Rows[Index].FindControl("txtCourseFees")).Text);
                TransportFees = Convert.ToDecimal(((TextBox)GridInstallments.Rows[Index].FindControl("txtTransportFees")).Text);
                ((TextBox)GridInstallments.Rows[Index].FindControl("txtCoursePaid")).Text = "0.00";
                ((TextBox)GridInstallments.Rows[Index].FindControl("txtTransportPaid")).Text = "0.00";
                CoursePaid = 0;
                TransportPaid = 0;

                if (CoursePaid <= CourseFees)
                {
                    if (TransportPaid <= TransportFees)
                    {
                        ((TextBox)GridInstallments.Rows[Index].FindControl("txtCourseBalance")).Text = (CourseFees - CoursePaid).ToString();
                        ((TextBox)GridInstallments.Rows[Index].FindControl("txtTransportBalance")).Text = (TransportFees - TransportPaid).ToString();
                    }
                    else
                    {
                        msg = "Please enter transport paid fees less then or equal to transport fees";
                        ScriptManager.RegisterStartupScript(this, GetType(), "MessageShow", "MessageShow('" + msg + "');", true);
                        ((TextBox)GridInstallments.Rows[Index].FindControl("txtTransportPaid")).Focus();
                    }
                }
                else
                {
                    msg = "Please enter course paid fees less then or equal to course fees";
                    ScriptManager.RegisterStartupScript(this, GetType(), "MessageShow", "MessageShow('" + msg + "');", true);
                    ((TextBox)GridInstallments.Rows[Index].FindControl("txtCoursePaid")).Focus();

                }
                ((TextBox)GridInstallments.Rows[Index].FindControl("txtPayClassFees")).Enabled = false;
                ((TextBox)GridInstallments.Rows[Index].FindControl("txtTransportPaid")).Enabled = false;
            }
            CourseBalance = Convert.ToDecimal(((TextBox)GridInstallments.Rows[Index].FindControl("txtCourseBalance")).Text);
            TransportBalance = Convert.ToDecimal(((TextBox)GridInstallments.Rows[Index].FindControl("txtTransportBalance")).Text);
            if (CourseBalance == 0 && TransportBalance == 0)
            {
                ((DropDownList)GridInstallments.Rows[Index].FindControl("comPaymentMode")).SelectedIndex = 0;
            }
            else
            {
                ((DropDownList)GridInstallments.Rows[Index].FindControl("comPaymentMode")).SelectedIndex = 1;
            }
        }
        CalcOther();
    }
    private void MCalc()
    {
        payMonthClassFee = "";
        payMonthTransportFee = "";
        classFessDetails = "";
        TransportFeesDetails = "";
        string FeeStatusForMonthName = "";
        decimal CourseFees = 0, TransportFees = 0, CoursePaid = 0, TransportPaid = 0, CourseTotal = 0, TransportTotal = 0, AlreadyPaid = 0, PayClassFees = 0
            , CourseBalance = 0, TransportAlreadyPaid = 0, PayTransportFees = 0, TransportBalance = 0;
        for (int Index = 0; Index < GridMonthlyInstallments.Rows.Count; Index++)
        {
            string MonthName = ((TextBox)GridMonthlyInstallments.Rows[Index].FindControl("txtMonths")).Text;
            PayClassFees = Convert.ToDecimal(((TextBox)GridMonthlyInstallments.Rows[Index].FindControl("txtPayClassFees")).Text);
            int InstMonthlyId = Convert.ToInt32(((TextBox)GridMonthlyInstallments.Rows[Index].FindControl("txtMonthlyInstId")).Text);
            var monthlyInstallments = from Cons in obj.MonthlyInstallments
                                      where Cons.MonthlyInstId == InstMonthlyId
                                      && Cons.CompanyId == Convert.ToInt32(Session["CompanyId"])
                                      //&& Cons.Remove == false && Cons.CompanyId == Convert.ToInt32(Session["CompanyId"]) && Cons.SessionId == Convert.ToInt32(Session["SessionId"])
                                      select Cons;
            if (monthlyInstallments.Count() > 0)
            {
                FeeStatusForMonthName = monthlyInstallments.First().PaymentStatus;
                AlreadyPaid = Convert.ToDecimal(monthlyInstallments.First().CousePaid);
                TransportAlreadyPaid = Convert.ToDecimal(monthlyInstallments.First().TransportPaid);
            }
            if (((CheckBox)GridMonthlyInstallments.Rows[Index].FindControl("chk1")).Checked)
            {
                if (FeeStatusForMonthName != "PAID")
                {
                    PayClassFees = 0;
                    PayTransportFees = 0;
                    CourseFees = Convert.ToDecimal(((TextBox)GridMonthlyInstallments.Rows[Index].FindControl("txtCourseFees")).Text);
                    TransportFees = Convert.ToDecimal(((TextBox)GridMonthlyInstallments.Rows[Index].FindControl("txtTransportFees")).Text);
                    PayClassFees = Convert.ToDecimal(((TextBox)GridMonthlyInstallments.Rows[Index].FindControl("txtPayClassFees")).Text);
                    PayTransportFees = Convert.ToDecimal(((TextBox)GridMonthlyInstallments.Rows[Index].FindControl("txtPayTransportFees")).Text);
                    try
                    {
                        CoursePaid = Convert.ToDecimal(((TextBox)GridMonthlyInstallments.Rows[Index].FindControl("txtPayClassFees")).Text);
                    }
                    catch (Exception ex)
                    {
                        ((TextBox)GridMonthlyInstallments.Rows[Index].FindControl("txtPayClassFees")).Text = "0";
                        CoursePaid = 0;
                    }
                    try
                    {
                        TransportPaid = Convert.ToDecimal(((TextBox)GridMonthlyInstallments.Rows[Index].FindControl("txtPayTransportFees")).Text);
                    }
                    catch (Exception ex)
                    {
                        ((TextBox)GridMonthlyInstallments.Rows[Index].FindControl("txtPayTransportFees")).Text = "0";
                        TransportPaid = 0;
                    }
                    if (PayClassFees > 0)
                    {
                        if (payMonthClassFee == "")
                        {
                            payMonthClassFee += ((TextBox)GridMonthlyInstallments.Rows[Index].FindControl("txtMonths")).Text;
                            classFessDetails += ((TextBox)GridMonthlyInstallments.Rows[Index].FindControl("txtMonths")).Text + "=" + PayClassFees;
                        }
                        else
                        {
                            payMonthClassFee += ", " + ((TextBox)GridMonthlyInstallments.Rows[Index].FindControl("txtMonths")).Text;
                            classFessDetails += ", " + ((TextBox)GridMonthlyInstallments.Rows[Index].FindControl("txtMonths")).Text + "=" + PayClassFees;
                        }
                        txtpayMonthClassFee.Text = payMonthClassFee;
                        txtclassFessDetails.Text = classFessDetails;
                    }
                    if (PayTransportFees > 0)
                    {
                        if (payMonthTransportFee == "")
                        {
                            payMonthTransportFee += ((TextBox)GridMonthlyInstallments.Rows[Index].FindControl("txtMonths")).Text;
                            TransportFeesDetails += ((TextBox)GridMonthlyInstallments.Rows[Index].FindControl("txtMonths")).Text + "=" + PayTransportFees;
                        }
                        else
                        {
                            payMonthTransportFee += ", " + ((TextBox)GridMonthlyInstallments.Rows[Index].FindControl("txtMonths")).Text;
                            TransportFeesDetails += ", " + ((TextBox)GridMonthlyInstallments.Rows[Index].FindControl("txtMonths")).Text + "=" + PayTransportFees;
                        }
                        txtpayMonthTransportFee.Text = payMonthTransportFee;
                        txtTransportFeesDetails.Text = TransportFeesDetails;
                    }
                    if (CoursePaid <= CourseFees)
                    {

                        decimal CourseBal = ((CourseFees - AlreadyPaid) - PayClassFees);
                        if (CourseBal >= 0)
                        {
                            ((TextBox)GridMonthlyInstallments.Rows[Index].FindControl("txtCourseBalance")).Text = CourseBal.ToString();
                        }
                        else
                        {
                            ((TextBox)GridMonthlyInstallments.Rows[Index].FindControl("txtPayClassFees")).Text = "0";
                            ((TextBox)GridMonthlyInstallments.Rows[Index].FindControl("txtCourseBalance")).Text = (CourseFees - AlreadyPaid).ToString();
                            CoursePaid = 0;
                        }
                        if (TransportPaid <= TransportFees)
                        {
                            decimal TransportFeesBal = ((TransportFees - TransportAlreadyPaid) - TransportPaid);
                            if (TransportFeesBal >= 0)
                            {
                                ((TextBox)GridMonthlyInstallments.Rows[Index].FindControl("txtTransportBalance")).Text = TransportFeesBal.ToString();
                                ((TextBox)GridMonthlyInstallments.Rows[Index].FindControl("txtTransportBalance")).Focus();
                            }
                            else
                            {
                                ((TextBox)GridMonthlyInstallments.Rows[Index].FindControl("txtTransportBalance")).Text = (TransportFees - TransportAlreadyPaid).ToString();
                                ((TextBox)GridMonthlyInstallments.Rows[Index].FindControl("txtPayTransportFees")).Text = "0";
                                TransportPaid = 0;
                            }


                            //((TextBox)GridMonthlyInstallments.Rows[Index].FindControl("txtTransportBalance")).Text = (TransportFees - TransportPaid).ToString();
                            //((TextBox)GridMonthlyInstallments.Rows[Index].FindControl("txtTransportBalance")).Focus();
                        }
                        else
                        {
                            ((TextBox)GridMonthlyInstallments.Rows[Index].FindControl("txtPayTransportFees")).Text = "0";
                            TransportPaid = 0;
                            msg = "Please enter transport paid fees less then or equal to transport fees";
                            ScriptManager.RegisterStartupScript(this, GetType(), "MessageShow", "MessageShow('" + msg + "');", true);
                            ((TextBox)GridMonthlyInstallments.Rows[Index].FindControl("txtPayTransportFees")).Focus();
                        }
                    }
                    else
                    {
                        ((TextBox)GridMonthlyInstallments.Rows[Index].FindControl("txtPayClassFees")).Text = "0";
                        CoursePaid = 0;
                        msg = "Please enter course paid fees less then or equal to course fees";
                        ScriptManager.RegisterStartupScript(this, GetType(), "MessageShow", "MessageShow('" + msg + "');", true);
                        ((TextBox)GridMonthlyInstallments.Rows[Index].FindControl("txtPayClassFees")).Focus();

                    }
                    CourseTotal = CourseTotal + CoursePaid;
                    TransportTotal = TransportTotal + TransportPaid;
                    var DATA = from Cons in obj.Addmisions
                               where Cons.AdmissionNo == txtAdminNo.Text && Cons.Remove == false && Cons.CompanyId == Convert.ToInt32(Session["CompanyId"]) && Cons.SessionId == Convert.ToInt32(Session["SessionId"])
                               select Cons;
                    admId = DATA.First().AdmissionId;
                    if (lblPaymentId.Text != "0")
                    {
                        var CFees = (from Cons in obj.Payments
                                     where Cons.AdmissionId == admId && Cons.PaymentId != Convert.ToInt32(lblPaymentId.Text) && Cons.Remove == false && Cons.ComapanyId == Convert.ToInt32(Session["CompanyId"]) && Cons.SessionId == Convert.ToInt32(Session["SessionId"])
                                     select Cons.CourseFees).Sum();

                        var TFees = (from Cons in obj.Payments
                                     where Cons.AdmissionId == admId && Cons.PaymentId != Convert.ToInt32(lblPaymentId.Text) && Cons.Remove == false && Cons.ComapanyId == Convert.ToInt32(Session["CompanyId"]) && Cons.SessionId == Convert.ToInt32(Session["SessionId"])
                                     select Cons.TransportFees).Sum();
                        if (CFees == null) { CFees = 0; } if (TFees == null) { TFees = 0; }
                        //txtInstallCourseFees.Text = (CourseTotal - Convert.ToDecimal(CFees)).ToString();
                        txtInstallCourseFees.Text = CourseTotal.ToString();
                        //txtInstallTransportFees.Text = (TransportTotal - Convert.ToDecimal(TFees)).ToString();
                        txtInstallTransportFees.Text = TransportTotal.ToString();
                        txtTotal.Text = ((CourseTotal + TransportTotal) - Convert.ToDecimal(CFees + TFees)).ToString();
                    }
                    else
                    {
                        var CFees = (from Cons in obj.Payments
                                     where Cons.AdmissionId == admId && Cons.Remove == false && Cons.ComapanyId == Convert.ToInt32(Session["CompanyId"]) && Cons.SessionId == Convert.ToInt32(Session["SessionId"])
                                     select Cons.CourseFees).Sum();

                        var TFees = (from Cons in obj.Payments
                                     where Cons.AdmissionId == admId && Cons.Remove == false && Cons.ComapanyId == Convert.ToInt32(Session["CompanyId"]) && Cons.SessionId == Convert.ToInt32(Session["SessionId"])
                                     select Cons.TransportFees).Sum();
                        if (CFees == null) { CFees = 0; } if (TFees == null) { TFees = 0; }
                        //txtInstallCourseFees.Text = (CourseTotal - Convert.ToDecimal(CFees)).ToString();
                        txtInstallCourseFees.Text = CourseTotal.ToString();
                        //txtInstallTransportFees.Text = (TransportTotal - Convert.ToDecimal(TFees)).ToString();
                        txtInstallTransportFees.Text = TransportTotal.ToString();
                        txtTotal.Text = ((CourseTotal + TransportTotal) - Convert.ToDecimal(CFees + TFees)).ToString();
                    }
                }
            }
            //not check
            else
            {
                CourseFees = Convert.ToDecimal(((TextBox)GridMonthlyInstallments.Rows[Index].FindControl("txtCourseFees")).Text);
                TransportFees = Convert.ToDecimal(((TextBox)GridMonthlyInstallments.Rows[Index].FindControl("txtTransportFees")).Text);
                ((TextBox)GridMonthlyInstallments.Rows[Index].FindControl("txtPayClassFees")).Text = "0.00";
                ((TextBox)GridMonthlyInstallments.Rows[Index].FindControl("txtTransportPaid")).Text = "0.00";
                CoursePaid = 0;
                TransportPaid = 0;

                if (CoursePaid <= CourseFees)
                {
                    ((TextBox)GridMonthlyInstallments.Rows[Index].FindControl("txtCourseBalance")).Text = (CourseFees - CoursePaid).ToString();
                    if (TransportPaid <= TransportFees)
                    {

                        ((TextBox)GridMonthlyInstallments.Rows[Index].FindControl("txtTransportBalance")).Text = (TransportFees - TransportPaid).ToString();
                    }
                    else
                    {
                        msg = "Please enter transport paid fees less then or equal to transport fees";
                        ScriptManager.RegisterStartupScript(this, GetType(), "MessageShow", "MessageShow('" + msg + "');", true);
                        ((TextBox)GridMonthlyInstallments.Rows[Index].FindControl("txtTransportPaid")).Focus();
                    }
                }
                else
                {
                    msg = "Please enter course paid fees less then or equal to course fees";
                    ScriptManager.RegisterStartupScript(this, GetType(), "MessageShow", "MessageShow('" + msg + "');", true);
                    ((TextBox)GridMonthlyInstallments.Rows[Index].FindControl("txtPayClassFees")).Focus();

                }
                ((TextBox)GridMonthlyInstallments.Rows[Index].FindControl("txtPayClassFees")).Enabled = false;
                ((TextBox)GridMonthlyInstallments.Rows[Index].FindControl("txtTransportPaid")).Enabled = false;
            }
            CourseBalance = Convert.ToDecimal(((TextBox)GridMonthlyInstallments.Rows[Index].FindControl("txtCourseBalance")).Text);
            TransportBalance = Convert.ToDecimal(((TextBox)GridMonthlyInstallments.Rows[Index].FindControl("txtTransportBalance")).Text);
            if (CourseBalance == 0 && TransportBalance == 0)
            {
                ((DropDownList)GridMonthlyInstallments.Rows[Index].FindControl("comPaymentMode")).SelectedIndex = 0;
            }
            else
            {
                ((DropDownList)GridMonthlyInstallments.Rows[Index].FindControl("comPaymentMode")).SelectedIndex = 1;
            }
        }
        CalcOther();
    }
    protected void btnFeesReceipt_Click(object sender, EventArgs e)
    {
        System.Threading.Thread.Yield();
        txtReceiptNo.Text = ((from Cons in obj.Payments
                              where Cons.ComapanyId == Convert.ToInt32(Session["CompanyId"]) && Cons.SessionId == Convert.ToInt32(Session["SessionId"])
                              select Cons.ReceiptNo).Max() + 1).ToString();
        try
        {
            if (chkPreviouse.Checked)
            {
                payMonthClassFee = ""; classFessDetails = ""; TransportFeesDetails = ""; txtotherFessDetails.Text = "";
            }
            if (txtAdmissionNo.Text != "")
            {
                var DATA = from Cons in obj.Addmisions
                           where Cons.AdmissionNo == txtAdminNo.Text && Cons.Remove == false && Cons.CompanyId == Convert.ToInt32(Session["CompanyId"]) && Cons.SessionId == Convert.ToInt32(Session["SessionId"])
                           select Cons;

                admId = DATA.First().AdmissionId;
                if (IdInstallments.Visible == true || IdMonthlyInstallments.Visible == true || chkPreviouse.Checked)
                {
                    if (txtTotal.Text != "" && txtTotal.Text != "0" && txtTotal.Text != "0.00")
                    {
                        if (Convert.ToDecimal(txtTotal.Text) > 0)
                        {
                            if (ddlPaymentMode.SelectedItem.Text != "")
                            {
                                if (lblPaymentId.Text == "0")
                                {
                                    IEnumerable<Payment> payment = from id in obj.Payments
                                                                   where id.ReceiptNo == Convert.ToInt32(txtReceiptNo.Text)
                                                                   && id.ComapanyId == Convert.ToInt32(Session["CompanyId"]) && id.SessionId == Convert.ToInt32(Session["SessionId"])
                                                                   select id;

                                    if (payment.Count<Payment>() <= 0)
                                    {
                                        if (role.Insert.Value)
                                        {
                                            if (obj.Sp_Payment(1, 0, admId, Convert.ToInt32(Session["AdmissionCourseId"]), txtAdminNo.Text.Trim(), Convert.ToInt32(txtReceiptNo.Text), Convert.ToDateTime(dtReceivedDate.Text), ddlPaymentMode.Text, txtInstallCourseFees.Text != "" ? Convert.ToDecimal(txtInstallCourseFees.Text) : 0,
                                                txtInstallTransportFees.Text != "" ? Convert.ToDecimal(txtInstallTransportFees.Text) : 0, txtAdmiFees.Text != "" ? Convert.ToDecimal(txtAdmiFees.Text) : 0, txtLateFees.Text != "" ? Convert.ToDecimal(txtLateFees.Text) : 0,
                                                txtOtherFeeType.Text, txtOtherFees.Text != "" ? Convert.ToDecimal(txtOtherFees.Text) : 0, Convert.ToDecimal(txtTotal.Text), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["UserId"]), Convert.ToInt32(Session["SessionId"]),
                                                txtBankName.Text, txtchequeNo.Text, txtChequeDate.Text != "" ? Convert.ToDateTime(txtChequeDate.Text) : System.DateTime.Now, txtDiscount.Text != "" ? Convert.ToDecimal(txtDiscount.Text) : 0, txtRemarks.Text, "", ddlSession.SelectedItem.Text, chkPreviouse.Checked == true ? Convert.ToDecimal(txtPaidPreviousBalance.Text) : 0,
                                                txtpayMonthClassFee.Text,
                                            txtclassFessDetails.Text,
                                            txtTransportFeesDetails.Text, ddlDiscountType.SelectedItem.Text, txtotherFessDetails.Text, Convert.ToInt32(ddlLateFeesType.SelectedValue)) == 0)
                                            {
                                                InstallmentUpdate(DATA);
                                                try
                                                {
                                                    obj.Sp_PreviousBalance(2, 0, admId, txtAdminNo.Text.Trim(), "", txtOpening.Text != "" ? Convert.ToDecimal(txtOpening.Text) : 0, 0, 0, 0);
                                                }
                                                catch (Exception ex) { }
                                                var DATA1 = (from Cons in obj.Payments
                                                             where Cons.Remove == false && Cons.ComapanyId == Convert.ToInt32(Session["CompanyId"]) && Cons.SessionId == Convert.ToInt32(Session["SessionId"])
                                                             select Cons.PaymentId).Max();
                                                try
                                                {
                                                    Session["StudentId"] = admId;
                                                    Session["PaymentId"] = DATA1;
                                                    string URL = "javascript:window.open('ReceiptPrint.aspx')";
                                                    ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "", URL, true);
                                                }
                                                catch (Exception ex) { }
                                                GetAdmissionDetailsByAddmissionNo(txtAdminNo.Text);
                                                if (Convert.ToBoolean(Session["SendMsg"]) == true)
                                                {
                                                    try
                                                    {

                                                        var DATA2 = from Cons in obj.Payments
                                                                    where Cons.PaymentId == Convert.ToInt32(DATA1) && Cons.Remove == false && Cons.ComapanyId == Convert.ToInt32(Session["CompanyId"]) && Cons.SessionId == Convert.ToInt32(Session["SessionId"])
                                                                    select Cons;
                                                        string msg = "Thank you " + DATA.First().StudentName + ",\nYour payment received  " + Convert.ToDecimal(DATA2.First().TotalFees).ToString("##,##,##,####.00") + " Rupees,Date " + Convert.ToDateTime(DATA2.First().PaymentDate).ToString("dd/MM/yyyy")
                                                         + ",Receipt No. is " + DATA2.First().ReceiptNo;
                                                        string Mobile = DATA.First().ContactNo;
                                                        SendSMS.sendsmsapi(Mobile, msg, "English");
                                                        //string api = Session["MsgAPI"].ToString();
                                                        //string api1 = (api.Replace("mobile", Mobile));
                                                        //string api2 = (api1.Replace("msg", msg));
                                                        //try
                                                        //{
                                                        //    WebClient client = new WebClient();
                                                        //    string baseURL = api2;
                                                        //    client.OpenRead(baseURL);
                                                        //}
                                                        //catch (Exception ex)
                                                        //{ }
                                                    }
                                                    catch (Exception ex)
                                                    { }
                                                }
                                                Clear();

                                                txtPaidPreviousBalance.Text = "";
                                                txtPaidPreviousBalance.Enabled = false;
                                                //msg = "Record insterted successfully...";
                                                //ScriptManager.RegisterStartupScript(this, GetType(), "MessageShow", "MessageShow('" + msg + "');", true);
                                            }
                                            else
                                            {
                                                msg = "Record not instert successfully...";
                                                ScriptManager.RegisterStartupScript(this, GetType(), "MessageShow", "MessageShow('" + msg + "');", true);
                                            }
                                        }
                                        else Globals.Message(Page, "Not Authorized ??");
                                    }
                                    else Globals.Message(Page, "Receipt no. alraedy exist...");
                                }
                                else if (lblPaymentId.Text != "0")
                                {
                                    if (role.Update.Value)
                                    {
                                        InstallmentUpdate(DATA);
                                        if (obj.Sp_Payment(2, 0, admId, Convert.ToInt32(Session["AdmissionCourseId"]), txtAdminNo.Text, Convert.ToInt32(txtReceiptNo.Text), Convert.ToDateTime(dtReceivedDate.Text), ddlPaymentMode.Text, txtInstallCourseFees.Text != "" ? Convert.ToDecimal(txtInstallCourseFees.Text) : 0,
                                            txtInstallTransportFees.Text != "" ? Convert.ToDecimal(txtInstallTransportFees.Text) : 0, txtAdmiFees.Text != "" ? Convert.ToDecimal(txtAdmiFees.Text) : 0, txtLateFees.Text != "" ? Convert.ToDecimal(txtLateFees.Text) : 0,
                                           txtOtherFeeType.Text, txtOtherFees.Text != "" ? Convert.ToDecimal(txtOtherFees.Text) : 0, Convert.ToDecimal(txtTotal.Text), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["UserId"]), Convert.ToInt32(Session["SessionId"]), txtBankName.Text, txtchequeNo.Text, txtChequeDate.Text != "" ? Convert.ToDateTime(txtChequeDate.Text) : System.DateTime.Now, txtDiscount.Text != "" ? Convert.ToDecimal(txtDiscount.Text) : 0, txtRemarks.Text, "", ddlSession.SelectedItem.Text, chkPreviouse.Checked == true ? Convert.ToDecimal(txtPaidPreviousBalance.Text) : 0,
                                           txtpayMonthClassFee.Text,
                                            txtclassFessDetails.Text,
                                            txtTransportFeesDetails.Text, ddlDiscountType.SelectedItem.Text, txtotherFessDetails.Text, Convert.ToInt32(ddlLateFeesType.SelectedValue)) == 0)
                                        {

                                            try
                                            {
                                                obj.Sp_PreviousBalance(2, 0, admId, txtAdminNo.Text.Trim(), "", txtOpening.Text != "" ? Convert.ToDecimal(txtOpening.Text) : 0, 0, 0, 0);
                                            }
                                            catch (Exception ex) { }
                                            try
                                            {
                                                GetAdmissionDetailsByAddmissionNo(txtAdminNo.Text);

                                                Session["StudentId"] = admId;
                                                Session["PaymentId"] = lblPaymentId.Text;
                                                string URL = "javascript:window.open('ReceiptPrint.aspx')";
                                                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "", URL, true);
                                                Clear();
                                            }
                                            catch (Exception ex) { }

                                            lblPaymentId.Text = "0";
                                            txtPaidPreviousBalance.Text = "";
                                            txtPaidPreviousBalance.Enabled = false;
                                            //msg = "Record insterted successfully...";
                                            //ScriptManager.RegisterStartupScrip t(this, GetType(), "MessageShow", "MessageShow('" + msg + "');", true);
                                        }
                                        else
                                        {
                                            msg = "Record not instert successfully...";
                                            ScriptManager.RegisterStartupScript(this, GetType(), "MessageShow", "MessageShow('" + msg + "');", true);
                                        }
                                    }
                                    else Globals.Message(Page, "Not Authorized ??");
                                }
                            }
                            else
                            {
                                msg = "Please select payment mode type...";
                                ScriptManager.RegisterStartupScript(this, GetType(), "MessageShow", "MessageShow('" + msg + "');", true);
                                ddlPaymentMode.Focus();
                            }
                        }
                        else
                        {
                            msg = "Please firstly give valid payment...";
                            ScriptManager.RegisterStartupScript(this, GetType(), "MessageShow", "MessageShow('" + msg + "');", true);
                        }
                    }
                    else
                    {
                        msg = "Please firstly give any payment...";
                        ScriptManager.RegisterStartupScript(this, GetType(), "MessageShow", "MessageShow('" + msg + "');", true);
                    }
                }
                else
                {
                    msg = "Please make the installment first...";
                    ScriptManager.RegisterStartupScript(this, GetType(), "MessageShow", "MessageShow('" + msg + "');", true);
                }
            }
            else
            {
                msg = "Please search the student by admission no...";
                ScriptManager.RegisterStartupScript(this, GetType(), "MessageShow", "MessageShow('" + msg + "');", true);
                txtAdminNo.Focus();
            }
        }
        catch (Exception ex)
        { }
    }
    private void Clear()
    {
        try
        {
            // Response.Redirect(Request.RawUrl + "?AdmissionNo=" + txtAdminNo.Text);
            txtotherFessDetails.Text = "";
            classFessDetails = "";
            TransportFeesDetails = "";
            txtpayMonthClassFee.Text = "";
            txtclassFessDetails.Text = "";
            txtTransportFeesDetails.Text = "";
            txtpayMonthTransportFee.Text = "";
            txtOtherFeeType.Text = ""; ddlLateFeesType.ClearSelection();
            ddlPaymentMode.SelectedIndex = 0;
            txtAdmiFees.Text = "";
            txtLateFees.Text = "";
            chkOtherFees.Checked = false;
            comOtherFeesType.Enabled = false;
            DivCheque.Visible = false;
            txtBankName.Text = string.Empty;
            txtchequeNo.Text = string.Empty;
            txtChequeDate.Text = string.Empty;
            txtOtherFees.Text = "";
            txtInstallCourseFees.Text = "";
            txtInstallTransportFees.Text = "";
            txtTotal.Text = "";
            chkPreviouse.Checked = false;
            txtDiscount.Text = "";
            txtRemarks.Text = string.Empty;
            ddlDiscountType.ClearSelection();
            var DATA1 = (from Cons in obj.Payments
                         where Cons.ComapanyId == Convert.ToInt32(Session["CompanyId"]) && Cons.SessionId == Convert.ToInt32(Session["SessionId"])
                         select Cons.ReceiptNo).Max() + 1;

            if (DATA1 == null)
            {
                txtReceiptNo.Text = "1";
            }
            else
            {
                txtReceiptNo.Text = DATA1.ToString();
            }
            try
            {
                if (role.Select.Value)
                {
                    //GetStudentInfo();
                }
            }
            catch (Exception ex)
            { }
            //Response.Redirect(Request.RawUrl + "?AdmissionNo="+txtAdminNo.Text);
        }
        catch (Exception ex)
        { }
    }
    private void InstallmentUpdate(IQueryable<Addmision> DATA)
    {
        try
        {
            if (DATA.First().PaymentType == "Installment")
            {
                for (int Index = 0; Index < GridInstallments.Rows.Count; Index++)
                {
                    decimal AlreadyPaid = 0, AlreadyPaidTransportFees = 0;
                    int InstallmentId = Convert.ToInt32(((TextBox)GridInstallments.Rows[Index].FindControl("txtInstallmentId")).Text);
                    var installments = from Cons in obj.Installments
                                       where Cons.InstallmentId == InstallmentId
                                       //&& Cons.Remove == false && Cons.CompanyId == Convert.ToInt32(Session["CompanyId"]) && Cons.SessionId == Convert.ToInt32(Session["SessionId"])
                                       select Cons;
                    if (installments.Count() > 0)
                    {
                        AlreadyPaid = Convert.ToDecimal(installments.First().CousePaid);
                        AlreadyPaidTransportFees = Convert.ToDecimal(installments.First().TransportPaid);
                    }
                    decimal InstallCourseFees = 0;
                    decimal InstallTransportFees = 0;
                    if (lblPaymentId.Text != "0")
                    {
                        var paymentDetails = from Cons in obj.Payments
                                             where Cons.PaymentId == Convert.ToInt32(lblPaymentId.Text) && Cons.Remove == false && Cons.ComapanyId == Convert.ToInt32(Session["CompanyId"]) && Cons.SessionId == Convert.ToInt32(Session["SessionId"])
                                             select Cons;

                        InstallCourseFees = Convert.ToDecimal(paymentDetails.First().CourseFees);
                        InstallTransportFees = Convert.ToDecimal(paymentDetails.First().TransportFees);
                    }
                    else
                    {
                        InstallCourseFees = 0;
                        InstallTransportFees = 0;
                    }
                    decimal CourseFees = Convert.ToDecimal(((TextBox)GridInstallments.Rows[Index].FindControl("txtCourseFees")).Text);
                    decimal TransportFees = Convert.ToDecimal(((TextBox)GridInstallments.Rows[Index].FindControl("txtTransportFees")).Text);
                    DateTime date = Convert.ToDateTime(((TextBox)GridInstallments.Rows[Index].FindControl("txtInstallmentDate")).Text);
                    decimal CoursePaid = (Convert.ToDecimal(((TextBox)GridInstallments.Rows[Index].FindControl("txtPayClassFees")).Text) + AlreadyPaid);
                    decimal TransportPaid = (Convert.ToDecimal(((TextBox)GridInstallments.Rows[Index].FindControl("txtPayTransportFees")).Text) + AlreadyPaidTransportFees);
                    //decimal CourseBalance = Convert.ToDecimal(((TextBox)GridInstallments.Rows[Index].FindControl("txtCourseBalance")).Text);
                    //decimal TransportBalance = Convert.ToDecimal(((TextBox)GridInstallments.Rows[Index].FindControl("txtTransportBalance")).Text);
                    decimal CourseBalance = CourseFees - CoursePaid;
                    decimal TransportBalance = TransportFees - TransportPaid;
                    string PaymentMode = ((DropDownList)GridInstallments.Rows[Index].FindControl("comPaymentMode")).Text;

                    obj.Sp_Installments(4, InstallmentId, admId, CourseFees, TransportFees, date, CoursePaid, TransportPaid, CourseBalance, TransportBalance, PaymentMode, Convert.ToInt32(Session["UserId"]), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["SessionId"]));
                    var Paymentid = obj.Payments.Select(m => m.PaymentId).Max();
                    obj.SP_RecieptHistory(1, 0, Convert.ToInt32(Paymentid), InstallmentId, admId, Convert.ToDouble(CoursePaid - AlreadyPaid), Convert.ToDouble(TransportPaid - AlreadyPaidTransportFees), DATA.First().PaymentType, "Edit", "", "");
                }
            }
            else if (DATA.First().PaymentType == "Monthly")
            {
                for (int Index = 0; Index < GridMonthlyInstallments.Rows.Count; Index++)
                {
                    decimal AlreadyPaid = 0, AlreadyPaidTransportFees = 0;
                    int InstMonthlyId = Convert.ToInt32(((TextBox)GridMonthlyInstallments.Rows[Index].FindControl("txtMonthlyInstId")).Text);
                    var monthlyInstallments = from Cons in obj.MonthlyInstallments
                                              where Cons.MonthlyInstId == InstMonthlyId
                                              //&& Cons.Remove == false && Cons.CompanyId == Convert.ToInt32(Session["CompanyId"]) && Cons.SessionId == Convert.ToInt32(Session["SessionId"])
                                              select Cons;
                    if (monthlyInstallments.Count() > 0)
                    {
                        AlreadyPaid = Convert.ToDecimal(monthlyInstallments.First().CousePaid);
                        AlreadyPaidTransportFees = Convert.ToDecimal(monthlyInstallments.First().TransportPaid);
                    }
                    decimal InstallCourseFees = 0;
                    decimal InstallTransportFees = 0;
                    if (lblPaymentId.Text != "0")
                    {
                        if (Convert.ToDecimal(((TextBox)GridMonthlyInstallments.Rows[Index].FindControl("txtPayClassFees")).Text) > 0)
                        {
                            var paymentDetails = from Cons in obj.Payments
                                                 where Cons.PaymentId == Convert.ToInt32(lblPaymentId.Text) && Cons.Remove == false && Cons.ComapanyId == Convert.ToInt32(Session["CompanyId"]) && Cons.SessionId == Convert.ToInt32(Session["SessionId"])
                                                 select Cons;

                            InstallCourseFees = Convert.ToDecimal(paymentDetails.First().CourseFees);
                            InstallTransportFees = Convert.ToDecimal(paymentDetails.First().TransportFees);
                            AlreadyPaid = (AlreadyPaid - InstallCourseFees);
                            AlreadyPaidTransportFees = (AlreadyPaidTransportFees - InstallTransportFees);
                            ((TextBox)GridMonthlyInstallments.Rows[Index].FindControl("txtCourseBalance")).Text = (AlreadyPaid + (Convert.ToDecimal(((TextBox)GridMonthlyInstallments.Rows[Index].FindControl("txtPayClassFees")).Text))).ToString();
                            ((TextBox)GridMonthlyInstallments.Rows[Index].FindControl("txtTransportBalance")).Text = (AlreadyPaidTransportFees + (Convert.ToDecimal(((TextBox)GridMonthlyInstallments.Rows[Index].FindControl("txtTransportBalance")).Text))).ToString();
                            Session["StudentId"] = admId;
                            Session["PaymentId"] = lblPaymentId.Text;
                            //lblPaymentId.Text = "0";
                        }
                    }
                    else
                    {
                        InstallCourseFees = 0;
                        InstallTransportFees = 0;
                    }
                    string Months = ((TextBox)GridMonthlyInstallments.Rows[Index].FindControl("txtMonths")).Text;
                    decimal CourseFees = Convert.ToDecimal(((TextBox)GridMonthlyInstallments.Rows[Index].FindControl("txtCourseFees")).Text);
                    decimal TransportFees = Convert.ToDecimal(((TextBox)GridMonthlyInstallments.Rows[Index].FindControl("txtTransportFees")).Text);
                    DateTime date = Convert.ToDateTime(((TextBox)GridMonthlyInstallments.Rows[Index].FindControl("txtInstallmentDate")).Text);
                    decimal CoursePaid = (Convert.ToDecimal(((TextBox)GridMonthlyInstallments.Rows[Index].FindControl("txtPayClassFees")).Text) + AlreadyPaid);
                    decimal TransportPaid = (Convert.ToDecimal(((TextBox)GridMonthlyInstallments.Rows[Index].FindControl("txtPayTransportFees")).Text) + AlreadyPaidTransportFees);
                    decimal CourseBalance = CourseFees - CoursePaid;
                    decimal TransportBalance = TransportFees - TransportPaid;
                    string PaymentMode = ((DropDownList)GridMonthlyInstallments.Rows[Index].FindControl("comPaymentMode")).Text;

                    obj.Sp_MonthlyInstallments(4, InstMonthlyId, admId, Months, CourseFees, TransportFees, date, CoursePaid, TransportPaid, CourseBalance, TransportBalance, PaymentMode, Convert.ToInt32(Session["UserId"]), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["SessionId"]));
                    var Paymentid = obj.Payments.Select(m => m.PaymentId).Max();
                    obj.SP_RecieptHistory(1, 0, Convert.ToInt32(Paymentid), InstMonthlyId, admId, Convert.ToDouble(CoursePaid - AlreadyPaid), Convert.ToDouble(TransportPaid - AlreadyPaidTransportFees), DATA.First().PaymentType, "Edit", "", "");
                }

            }
        }
        catch (Exception ex)
        {

            throw ex;
        }
    }
    protected void Print_Click(object sender, EventArgs e)
    {
        try
        {
            lblPaymentId.Text = (sender as LinkButton).CommandArgument;
            var DATA = from Cons in obj.Payments
                       where Cons.PaymentId == Convert.ToInt32(lblPaymentId.Text) && Cons.Remove == false && Cons.ComapanyId == Convert.ToInt32(Session["CompanyId"]) && Cons.SessionId == Convert.ToInt32(Session["SessionId"])
                       select Cons;
            var DATA1 = from Cons in obj.Addmisions
                        where Cons.AdmissionNo == txtAdminNo.Text && Cons.Remove == false && Cons.CompanyId == Convert.ToInt32(Session["CompanyId"]) && Cons.SessionId == Convert.ToInt32(Session["SessionId"])
                        select Cons;

            admId = DATA1.First().AdmissionId;
            Session["StudentId"] = admId;
            Session["PaymentId"] = lblPaymentId.Text;
            string URL = "javascript:window.open('ReceiptPrint.aspx')";
            ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "", URL, true);
            lblPaymentId.Text = "0";
        }
        catch (Exception ex)
        { }
    }
    protected void Edit_Click(object sender, EventArgs e)
    {
        try
        {
            if (role.Update.Value)
            {
                lblPaymentId.Text = (sender as LinkButton).CommandArgument;
                var DATA = from Cons in obj.Payments
                           where Cons.PaymentId == Convert.ToInt32(lblPaymentId.Text) && Cons.Remove == false && Cons.ComapanyId == Convert.ToInt32(Session["CompanyId"]) && Cons.SessionId == Convert.ToInt32(Session["SessionId"])
                           select Cons;
                if (DATA.First().PreviousPaid > 0)
                {
                    txtPaidPreviousBalance.Text = DATA.First().PreviousPaid.ToString();
                    IdInstallments.Visible = false;
                    IdMonthlyInstallments.Visible = false;
                    txtPaidPreviousBalance.Focus();
                    chkPreviouse.Checked = true;
                    txtPaidPreviousBalance.Enabled = true;
                }
                else
                {
                    chkPreviouse.Checked = false;
                    txtPaidPreviousBalance.Text = "";

                }

                dtReceivedDate.Text = Convert.ToDateTime(DATA.First().PaymentDate).ToString("dd/MM/yyyy");
                txtReceiptNo.Text = DATA.First().ReceiptNo.ToString();
                ddlPaymentMode.Text = DATA.First().PaymentType;
                txtAdmiFees.Text = DATA.First().AdmissionFees.ToString();
                txtLateFees.Text = DATA.First().LateFees.ToString();
                try
                {
                    if (DATA.First().OtherFeesType != "")
                    {
                        chkOtherFees.Checked = true;
                        comOtherFeesType.Enabled = true;
                        comOtherFeesType.ClearSelection();
                        foreach (ListItem li in comOtherFeesType.Items)
                        {
                            if (li.Text == DATA.First().OtherFeesType.ToString())
                            {
                                li.Selected = true;
                                break;
                            }
                        }
                    }
                    else
                    {
                        comOtherFeesType.SelectedIndex = 0;
                        chkOtherFees.Checked = false;
                        comOtherFeesType.Enabled = false;
                    }
                }
                catch (Exception ex)
                { }
                txtOtherFees.Text = DATA.First().OtherFees.ToString();
                txtInstallCourseFees.Text = DATA.First().CourseFees.ToString();
                txtInstallTransportFees.Text = DATA.First().TransportFees.ToString();
                txtTotal.Text = DATA.First().TotalFees.ToString();
                if (DATA.First().PaymentType == "CHEQUE")
                {
                    DivCheque.Visible = true;
                    txtBankName.Text = DATA.First().BankName;
                    txtchequeNo.Text = DATA.First().ChequeNo;
                    txtChequeDate.Text = Convert.ToDateTime(DATA.First().ChequeDate).ToString("dd/MM/yyyy");
                }
                else
                {
                    DivCheque.Visible = false;
                    txtBankName.Text = string.Empty;
                    txtchequeNo.Text = string.Empty;
                    txtChequeDate.Text = string.Empty;
                }
                txtDiscount.Text = DATA.First().Discount.ToString();
                txtRemarks.Text = DATA.First().Remarks;
            }
            else Globals.Message(Page, "Not Authorized ??");
        }
        catch (Exception ex)
        { }
    }
    protected void Delete_Click(object sender, EventArgs e)
    {
        try
        {
            var MaxPaymentId = obj.Payments.Max(x => x.PaymentId);
            lblDelete.Text = (sender as LinkButton).CommandArgument;
            int paymentId = Convert.ToInt32(lblDelete.Text);
            var _PaymentsData = from p in obj.Payments where p.PaymentId == Convert.ToInt32(lblDelete.Text) select p;
            if (Convert.ToInt32(paymentId) == Convert.ToInt32(MaxPaymentId))
            {
                if (Convert.ToInt32(lblDelete.Text) > 0)
                {
                    int admissionId = Convert.ToInt32(_PaymentsData.First().AdmissionId);
                    var _AddmisionsData = from p in obj.Addmisions where p.AdmissionId == admissionId select p;
                    var _RecieptHistories = from p in obj.RecieptHistories where p.isdel == false && p.PaymentId == paymentId select p;
                    if (_RecieptHistories.Count() > 0)
                    {
                        foreach (var item in _RecieptHistories)
                        {
                            int respectTo = Convert.ToInt32(item.RespectTo);
                            decimal classPaidAmt = Convert.ToDecimal(item.CousePaid);
                            decimal transportPaidAmt = Convert.ToDecimal(item.TransportPaid);

                            obj.SP_RecieptHistory(3, 0, Convert.ToInt32(lblDelete.Text), respectTo, admissionId, Convert.ToDouble(classPaidAmt), Convert.ToDouble(transportPaidAmt), "", "", "", "");
                        }
                        if (obj.SP_RecieptHistory(2, 0, Convert.ToInt32(lblDelete.Text), 0, 0, Convert.ToDouble(0), Convert.ToDouble(0), "", Session["UserId"].ToString(), "", "") == 0)
                        {
                            GetAdmissionDetailsByAddmissionNo(txtAdminNo.Text);
                            Globals.Message(Page, "Record Cancelled");
                        }
                    }
                    else
                    {
                        Globals.Message(Page, "That is old reciept!!!");
                    }
                }
            }
            else if (Convert.ToInt32(paymentId) != Convert.ToInt32(MaxPaymentId))
            {
                if (Convert.ToInt32(lblDelete.Text) > 0)
                {
                    int admissionId = Convert.ToInt32(_PaymentsData.First().AdmissionId);
                    var _AddmisionsData = from p in obj.Addmisions where p.AdmissionId == admissionId select p;
                    var _RecieptHistories = from p in obj.RecieptHistories where p.isdel == false && p.PaymentId == paymentId select p;
                    if (_RecieptHistories.Count() > 0)
                    {
                        if (obj.SP_RecieptHistory(4, 0, Convert.ToInt32(lblDelete.Text), 0, 0, Convert.ToDouble(0), Convert.ToDouble(0), "", Session["UserId"].ToString(), "", "") == 0)
                        {
                            GetAdmissionDetailsByAddmissionNo(txtAdminNo.Text);
                            Globals.Message(Page, "Approvel request sent !!");
                        }
                    }
                    else
                    {
                        Globals.Message(Page, "That is old reciept!!!");
                    }
                }
            }
            else
            {
                Globals.Message(Page, "That permission only for Admin users !!");
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void btnAllFeesRefresh_Click(object sender, EventArgs e)
    {
        try
        {
            Session["StudentId"] = null;
            Session["PaymentId"] = null;
            Session["ReceiptAdmissionNo"] = null;
            lblPaymentId.Text = "0";
            Response.Redirect(Request.RawUrl);
        }
        catch (Exception ex)
        { }
    }
    protected void btnFeesRefresh_Click(object sender, EventArgs e)
    {
        try
        {
            Clear();
        }
        catch (Exception ex)
        { }
    }
    protected void ddlPaymentMode_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlPaymentMode.SelectedItem.Text == "CHEQUE")
            {
                DivCheque.Visible = true;
                txtBankName.Text = string.Empty;
                txtchequeNo.Text = string.Empty;
                txtChequeDate.Text = string.Empty;
                txtBankName.Focus();
            }
            else
            {
                DivCheque.Visible = false;
                txtBankName.Text = string.Empty;
                txtchequeNo.Text = string.Empty;
                txtChequeDate.Text = string.Empty;
                txtAdmiFees.Focus();
            }

        }
        catch (Exception ex)
        { }
    }
    protected void chkPreviouse_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (chkPreviouse.Checked)
            {
                GetStudentInfo();
                txtPaidPreviousBalance.Enabled = true;
                IdInstallments.Visible = false;
                IdMonthlyInstallments.Visible = false;
                txtPaidPreviousBalance.Focus();
            }
            else
            {
                IdInstallments.Visible = true;
                IdMonthlyInstallments.Visible = true;
                txtPaidPreviousBalance.Text = "";
                decimal PreviousBalance = 0, PreviousPaid = 0;
                CalcOther();
                try
                {
                    PreviousBalance = Convert.ToDecimal(txtOpening.Text);
                }
                catch (Exception ex)
                { PreviousBalance = 0; }
                try
                {
                    PreviousPaid = Convert.ToDecimal(txtPaidPreviousBalance.Text);
                }
                catch (Exception ex)
                { PreviousPaid = 0; }

                txtOpening.Text = (Convert.ToDecimal(Session["PreviousBalance"]) - PreviousPaid).ToString();
                txtPaidPreviousBalance.Enabled = false;
            }
        }
        catch (Exception ex) { }
    }
    protected void btnOpeningSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtOpening.Text == "0" || txtOpening.Text == string.Empty || Convert.ToDouble(txtOpening.Text) <= 0)
            {
                Globals.Message(Page, "Please enter valid amount!!");
            }
            else
            {
                if (role.Insert.Value)
                {
                    if (txtAdminNo.Text != "" && txtStudentName.Text != "")
                    {
                        var DATA = from Cons in obj.Addmisions
                                   where Cons.AdmissionNo == txtAdminNo.Text && Cons.Remove == false && Cons.CompanyId == Convert.ToInt32(Session["CompanyId"]) && Cons.SessionId == Convert.ToInt32(Session["SessionId"])
                                   select Cons;
                        if (DATA.First().AdmissionNo != null)
                        {
                            if (obj.Sp_PreviousBalance(1, 0, DATA.First().AdmissionId, DATA.First().AdmissionNo, ddlSession.SelectedItem.Text, txtOpening.Text != "" ? Convert.ToDecimal(txtOpening.Text) : 0, Convert.ToInt32(Session["UserId"]), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["SessionId"])) == 0)
                            {
                                Globals.Message(Page, "Previous balance added successfully...");
                                btnOpeningSave.Visible = false;
                                txtOpening.Enabled = false;
                                IEnumerable<PreviousBalance> PreviousBalances = from Cons in obj.PreviousBalances
                                                                                where Cons.AdmissionNo == txtAdminNo.Text && Cons.Remove == false && Cons.CompanyId == Convert.ToInt32(Session["CompanyId"]) && Cons.SessionId == Convert.ToInt32(Session["SessionId"])
                                                                                select Cons;
                                if (PreviousBalances.Count<PreviousBalance>() > 0)
                                {
                                    ddlSession.Text = PreviousBalances.First().PreviousSession;
                                    txtOpening.Text = PreviousBalances.First().PreviousBalance1.ToString();
                                    Session["PreviousBalance"] = PreviousBalances.First().PreviousBalance1.ToString();
                                    txtOpening.Enabled = false;
                                    chkPreviouse.Enabled = true;
                                    btnOpeningSave.Visible = false;
                                }
                                else
                                {
                                    txtOpening.Text = "";
                                    Session["PreviousBalance"] = "0";
                                    txtOpening.Enabled = true;
                                    txtPaidPreviousBalance.Text = "";
                                    txtPaidPreviousBalance.Enabled = false;
                                    chkPreviouse.Checked = false;
                                    btnOpeningSave.Visible = true;
                                }
                            }
                        }
                    }
                    else
                    {
                        Globals.Message(Page, "Please enter student admission number...");
                        txtAdminNo.Focus();
                    }
                }
            }
        }
        catch (Exception ex) { }
    }
    protected void txtPaidPreviousBalance_TextChanged(object sender, EventArgs e)
    {
        if (Convert.ToDecimal(txtPaidPreviousBalance.Text) > 0)
        {
            decimal PreviousBalance = 0, PreviousPaid = 0;
            CalcOther();
            try
            {
                PreviousBalance = Convert.ToDecimal(txtOpening.Text);
            }
            catch (Exception ex)
            { PreviousBalance = 0; }
            try
            {
                PreviousPaid = Convert.ToDecimal(txtPaidPreviousBalance.Text);
            }
            catch (Exception ex)
            { PreviousPaid = 0; }

            txtOpening.Text = (Convert.ToDecimal(Session["PreviousBalance"]) - PreviousPaid).ToString();
            Double open = 0, paid = 0;
            open = Convert.ToDouble(string.IsNullOrWhiteSpace(txtOpening.Text) ? "0" : txtOpening.Text);
            //paid = Convert.ToDouble(string.IsNullOrWhiteSpace(txtPaidPreviousBalance.Text) ? "0" : txtPaidPreviousBalance.Text);
            if (open <= -1)
            {
                txtOpening.Text = (Convert.ToDecimal(Session["PreviousBalance"]).ToString());
                txtPaidPreviousBalance.Text = "0";
                Globals.Message(Page, "Please enter valid amount");
            }
        }
        else
        {
            txtOpening.Text = (Convert.ToDecimal(Session["PreviousBalance"]).ToString());
            txtPaidPreviousBalance.Text = "0";
            Globals.Message(Page, "Please enter valid amount");
        }
    }
    protected void ddlTransportStaus_SelectedIndexChanged(object sender, EventArgs e)
    {

        if (ddlTransportStaus.SelectedIndex > 0)
        {
            if (ddlTransportStaus.SelectedItem.Text == "START")
            {
                ddlTo.Visible = true;
                lblToArea.Visible = true;
                var B = from Cons in obj.BusTransports
                        where Cons.Remove == false
                        select Cons;
                ddlTo.DataSource = B;
                ddlTo.DataTextField = "To";
                ddlTo.DataValueField = "TransportId";
                ddlTo.DataBind();
                ddlTo.Items.Insert(0, new ListItem());
            }
            else
            {
                ddlTo.Visible = false;
                lblToArea.Visible = false;
            }
            tblTransportStatusMonth.Visible = true;
            btnTransportStatus.Text = ddlTransportStaus.SelectedItem.Text;
        }
        else
            tblTransportStatusMonth.Visible = false;
    }
    protected void btnTransportStatus_Click(object sender, EventArgs e)
    {
        if (IdMonthlyInstallments.Visible == true)
        {
            if (btnTransportStatus.Text == "START")
            {
                if (ddlTo.SelectedIndex > 0)
                {
                    for (int Index = 0; Index < GridMonthlyInstallments.Rows.Count; Index++)
                    {
                        if (((CheckBox)GridMonthlyInstallments.Rows[Index].FindControl("chkTransport")).Checked)
                        {
                            int InstMonthlyId = Convert.ToInt32(((TextBox)GridMonthlyInstallments.Rows[Index].FindControl("txtMonthlyInstId")).Text);
                            var monthlyInstallments = from Cons in obj.MonthlyInstallments
                                                      where Cons.MonthlyInstId == InstMonthlyId && Cons.Remove == false
                                                      select Cons;
                            var addmisions = from Cons in obj.Addmisions
                                             where Cons.AdmissionId == Convert.ToInt32(monthlyInstallments.First().AdmissionId) && Cons.Remove == false
                                             select Cons;
                            if (Convert.ToDecimal(monthlyInstallments.First().TransportFees) <= 0)
                            {
                                if (Convert.ToDecimal(monthlyInstallments.First().TransportPaid) <= 0)
                                {
                                    obj.SP_UpdateTransportStatus(1, InstMonthlyId, false, Convert.ToDecimal(TransportFees), Convert.ToInt32(monthlyInstallments.First().AdmissionId), ddlTo.SelectedItem.Text);
                                    msg = "Transport service start...";
                                    ScriptManager.RegisterStartupScript(this, GetType(), "MessageShow", "MessageShow('" + msg + "');", true);
                                }
                                else
                                {
                                    msg = "Already paid Transport fees selected month ...";
                                    ScriptManager.RegisterStartupScript(this, GetType(), "MessageShow", "MessageShow('" + msg + "');", true);
                                }
                            }
                            else
                            {
                                msg = "Already set Transport fees selected month ...";
                                ScriptManager.RegisterStartupScript(this, GetType(), "MessageShow", "MessageShow('" + msg + "');", true);
                            }
                        }
                    }
                }
                else
                {
                    msg = "Please select To Area...";
                    ScriptManager.RegisterStartupScript(this, GetType(), "MessageShow", "MessageShow('" + msg + "');", true);
                }
            }
            else if (btnTransportStatus.Text == "STOP")
            {
                for (int Index = 0; Index < GridMonthlyInstallments.Rows.Count; Index++)
                {
                    if (((CheckBox)GridMonthlyInstallments.Rows[Index].FindControl("chkTransport")).Checked)
                    {
                        int InstMonthlyId = Convert.ToInt32(((TextBox)GridMonthlyInstallments.Rows[Index].FindControl("txtMonthlyInstId")).Text);
                        var monthlyInstallments = from Cons in obj.MonthlyInstallments
                                                  where Cons.MonthlyInstId == InstMonthlyId && Cons.Remove == false
                                                  select Cons;
                        var addmisions = from Cons in obj.Addmisions
                                         where Cons.AdmissionId == Convert.ToInt32(monthlyInstallments.First().AdmissionId) && Cons.Remove == false
                                         select Cons;
                        if (Convert.ToDecimal(monthlyInstallments.First().TransportPaid) <= 0)
                        {
                            obj.SP_UpdateTransportStatus(1, InstMonthlyId, true, Convert.ToDecimal(monthlyInstallments.First().TransportFees), Convert.ToInt32(monthlyInstallments.First().AdmissionId), "");
                            msg = "Transport service stop...";
                            ScriptManager.RegisterStartupScript(this, GetType(), "MessageShow", "MessageShow('" + msg + "');", true);
                        }
                        else
                        {
                            msg = "Already paid Transport fees selected month ...";
                            ScriptManager.RegisterStartupScript(this, GetType(), "MessageShow", "MessageShow('" + msg + "');", true);
                        }
                    }
                }
            }
        }
        else if (IdInstallments.Visible == true)
        {
            if (btnTransportStatus.Text == "START")
            {
                if (ddlTo.SelectedIndex > 0)
                {
                    for (int Index = 0; Index < GridInstallments.Rows.Count; Index++)
                    {
                        if (((CheckBox)GridInstallments.Rows[Index].FindControl("chkTransport")).Checked)
                        {
                            int InstallmentId = Convert.ToInt32(((TextBox)GridInstallments.Rows[Index].FindControl("txtInstallmentId")).Text);
                            var monthlyInstallments = from Cons in obj.Installments
                                                      where Cons.InstallmentId == InstallmentId && Cons.Remove == false
                                                      select Cons;
                            var addmisions = from Cons in obj.Addmisions
                                             where Cons.AdmissionId == Convert.ToInt32(monthlyInstallments.First().AdmissionId) && Cons.Remove == false
                                             select Cons;
                            if (Convert.ToDecimal(monthlyInstallments.First().TransportFees) <= 0)
                            {
                                if (Convert.ToDecimal(monthlyInstallments.First().TransportPaid) <= 0)
                                {
                                    obj.SP_UpdateTransportStatus(2, InstallmentId, false, Convert.ToDecimal(TransportFees), Convert.ToInt32(monthlyInstallments.First().AdmissionId), "");
                                    msg = "Transport service start...";
                                    ScriptManager.RegisterStartupScript(this, GetType(), "MessageShow", "MessageShow('" + msg + "');", true);
                                }
                                else
                                {
                                    msg = "Already paid Transport fees selected month ...";
                                    ScriptManager.RegisterStartupScript(this, GetType(), "MessageShow", "MessageShow('" + msg + "');", true);
                                }
                            }
                            else
                            {
                                msg = "Already set Transport fees selected month ...";
                                ScriptManager.RegisterStartupScript(this, GetType(), "MessageShow", "MessageShow('" + msg + "');", true);
                            }
                        }
                    }
                }
                else
                {
                    msg = "Please select To Area...";
                    ScriptManager.RegisterStartupScript(this, GetType(), "MessageShow", "MessageShow('" + msg + "');", true);
                }
            }
            else if (btnTransportStatus.Text == "STOP")
            {
                for (int Index = 0; Index < GridInstallments.Rows.Count; Index++)
                {
                    if (((CheckBox)GridInstallments.Rows[Index].FindControl("chkTransport")).Checked)
                    {
                        int InstallmentId = Convert.ToInt32(((TextBox)GridInstallments.Rows[Index].FindControl("txtInstallmentId")).Text);
                        var installments = from Cons in obj.Installments
                                           where Cons.InstallmentId == InstallmentId && Cons.Remove == false
                                           select Cons;
                        var addmisions = from Cons in obj.Addmisions
                                         where Cons.AdmissionId == Convert.ToInt32(installments.First().AdmissionId) && Cons.Remove == false
                                         select Cons;
                        if (Convert.ToDecimal(installments.First().TransportPaid) <= 0)
                        {
                            obj.SP_UpdateTransportStatus(2, InstallmentId, true, Convert.ToDecimal(installments.First().TransportFees), Convert.ToInt32(installments.First().AdmissionId), "");
                            msg = "Transport service stop...";
                            ScriptManager.RegisterStartupScript(this, GetType(), "MessageShow", "MessageShow('" + msg + "');", true);
                        }
                        else
                        {
                            msg = "Already paid Transport fees selected month ...";
                            ScriptManager.RegisterStartupScript(this, GetType(), "MessageShow", "MessageShow('" + msg + "');", true);
                        }
                    }
                }
            }
        }

        GetAdmissionDetailsByAddmissionNo(txtAdminNo.Text);
        ddlTo.ClearSelection();
        TransportFees = 0;
    }
    protected void ddlTo_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlTo.SelectedIndex != 0)
            {
                if (IdMonthlyInstallments.Visible == true)
                {
                    TransportFees = 0;
                    var D1 = from Cons in obj.BusTransports
                             where Cons.TransportId == Convert.ToInt32(ddlTo.SelectedValue) && Cons.Remove == false
                             select Cons;
                    TransportFees = Convert.ToDouble(D1.First().TotalFees);
                    TransportFees = TransportFees / 10;
                }
                else if (IdInstallments.Visible == true)
                {
                    int numberOfInstallment = Convert.ToInt32(AllMethods.GetNumberOfInstallment(txtAdminNo.Text.Trim()));
                    TransportFees = 0;
                    var D1 = from Cons in obj.BusTransports
                             where Cons.TransportId == Convert.ToInt32(ddlTo.SelectedValue) && Cons.Remove == false
                             select Cons;
                    TransportFees = Convert.ToDouble(D1.First().TotalFees);
                    TransportFees = TransportFees / numberOfInstallment;
                }
            }
        }
        catch (Exception ex)
        { }
    }
    protected void ddlDiscountType_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtDiscount.Text = string.Empty;
        if (ddlDiscountType.SelectedIndex > 0)
        {
            txtDiscount.Enabled = true;
            txtDiscount.Focus();
        }
        else
        {
            txtDiscount.Enabled = false;
        }
        CalcOther();
    }
    protected void chkOtherFeen_CheckedChanged(object sender, EventArgs e)
    {
        CalcOtherFee();
    }
    protected void CalcOtherFee()
    {
        try
        {
            decimal OtherFee = 0;
            txtOtherFeeType.Text = "";
            txtotherFessDetails.Text = "";
            for (int Index = 0; Index < GridOtherFee.Rows.Count; Index++)
            {
                if (((CheckBox)GridOtherFee.Rows[Index].FindControl("chkOtherFee")).Checked)
                {
                    OtherFee += Convert.ToDecimal(((TextBox)GridOtherFee.Rows[Index].FindControl("txtFees")).Text);
                    if (txtOtherFeeType.Text == "")
                    {
                        txtOtherFeeType.Text = ((TextBox)GridOtherFee.Rows[Index].FindControl("txtFeesType")).Text;
                        txtotherFessDetails.Text = txtOtherFeeType.Text + "=" + Convert.ToDecimal(((TextBox)GridOtherFee.Rows[Index].FindControl("txtFees")).Text);
                    }
                    else
                    {
                        txtOtherFeeType.Text += " & " + ((TextBox)GridOtherFee.Rows[Index].FindControl("txtFeesType")).Text;
                        txtotherFessDetails.Text += ", " + ((TextBox)GridOtherFee.Rows[Index].FindControl("txtFeesType")).Text + "=" + Convert.ToDecimal(((TextBox)GridOtherFee.Rows[Index].FindControl("txtFees")).Text);
                    }
                }
                txtOtherFees.Text = OtherFee.ToString();
                CalcOther();
            }
        }
        catch (Exception)
        {

            throw;
        }
    }
    protected void ddlSession_SelectedIndexChanged(object sender, EventArgs e)
    {
        string[] aa = ddlSession.SelectedItem.Text.Split('-');
        string SelectSeesion = aa.FirstOrDefault();
        var sessions = from Cons in obj.Sessions
                       where Cons.Remove == false && Cons.Sessionid == Convert.ToInt32(Session["SessionId"])
                       select Cons;
        string SessionforStudentArray = sessions.First().SessionName.Split('-').FirstOrDefault();
        //string SessionforStudent = SessionforStudentArray.FirstOrDefault();

        if (Convert.ToInt32(SelectSeesion) < Convert.ToInt32(SessionforStudentArray))
        {
            txtOpening.Enabled = true;
        }
        else
        {
            txtOpening.Enabled = false;
            Globals.Message(Page, "Select Properly Session - This is not valid session for this student!!");
        }

    }
    protected void cbAdvance_CheckedChanged(object sender, EventArgs e)
    {
        if (cbAdvance.Checked)
        {
            AdvanceSearch.Visible = true;
        }
        else
        {
            AdvanceSearch.Visible = false;
        }
    }
    protected void ddlClassName_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillDropdown.FillSectionByClass(ddlSectionName, Convert.ToInt32(ddlClassName.SelectedValue));
    }
    protected void ddlSectionName_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillDropdown.FillStudentByClassAndBySection(ddlStudentName, Convert.ToInt32(ddlClassName.SelectedValue), ddlSectionName.SelectedItem.Text);
    }
    protected void ddlStudentName_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtAdminNo.Text = ddlStudentName.SelectedValue;
        GetAdmissionDetailsByAddmissionNo(txtAdminNo.Text);
    }
    protected void txtAdminNo_TextChanged(object sender, EventArgs e)
    {
        if (txtAdminNo.Text.Length == 8)
        {
            GetAdmissionDetailsByAddmissionNo(txtAdminNo.Text);
        }
        else
        {
            Globals.Message(Page, "Please enter correct scholler number");
            txtAdminNo.Focus();
        }

    }
    public void GetDataAssetFineByAddmisionNo()
    {
        try
        {
            if (txtAdminNo.Text != string.Empty)
            {
                var _assetFine = obj.AssetFines.Where(x => x.AddmisionNo == txtAdminNo.Text && x.F1 == "DUE");
                if (_assetFine.Count() > 0)
                {
                    tableAssetFine.Visible = true;
                    GridAssetFine.DataSource = _assetFine;
                    GridAssetFine.DataBind();
                }
                else
                {
                    tableAssetFine.Visible = false;
                }
            }
        }
        catch (Exception)
        {
            throw;
        }
    }
    protected void AssetFine_Click(object sender, EventArgs e)
    {
        try
        {
            int id = Convert.ToInt32((sender as LinkButton).CommandArgument);
            if (id > 0)
            {
                Response.Redirect("AssetFine.aspx?id=" + id);
            }
            else
            { Globals.Message(Page, "Sorry for that's !!"); }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected static string GetOtherFeesPaidHead(int AdmissionId, int CompanyId, int SessionId)
    {
        try
        {
            DataTable dt = AllMethods.GetOtherFeesByAdmissionId(CompanyId, SessionId, AdmissionId);
            string values = "";
            foreach (DataRow dr in dt.Rows)
            {
                if (values=="")
                {
                    values = dr[0].ToString();                    
                }
                else
                {
                    values += ", " + dr[0].ToString();                    
                }
            }
            return values;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}