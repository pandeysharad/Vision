using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Reports_MonthlyReceivedAndBalanceSheet : System.Web.UI.Page
{

    DataClassesDataContext obj = new DataClassesDataContext();
    public int SrNo = 1;
    public string SchoolName = "";
    public DateTime From = DateTime.Now, To = DateTime.Now;
    int CourseId;
    double ClassBalTotal = 0;
    double TransportBalTotal = 0.0;
    double ClassFeesTotal = 0;
    double TransportFeesTotal = 0.0;
    double ClassPaidTotal = 0;
    double TransportPaidTotal = 0.0;
    double OtherFeesReceivedTotal = 0.0;
    double OtherFeesTotal = 0.0;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["CompanyId"] != null)
            {

                var DATA = from Cons in obj.Settings
                           select Cons;
                SchoolName = DATA.First().SchoolName;
                if (!string.IsNullOrEmpty(Request.QueryString["From"]))
                {
                    From = Convert.ToDateTime(Request.QueryString["From"]);
                }
                if (!string.IsNullOrEmpty(Request.QueryString["To"]))
                {
                    To = Convert.ToDateTime(Request.QueryString["To"]);
                }
                if (!string.IsNullOrEmpty(Request.QueryString["CourseId"]))
                {
                    CourseId = Convert.ToInt32(Request.QueryString["CourseId"]);
                }
                if (!IsPostBack)
                {
                    FillDataPaymentSheet();
                }
            }
            else
            {
                Response.Redirect("../Default.aspx");
            }
        }
        catch (Exception ex)
        { }
    }
    private void FillDataPaymentSheet()
    {
        try
        {
            if (CourseId == 0)
            {
                var inv = from i in obj.MonthlyInstallments
                          from a in obj.Addmisions
                          where i.InstallmentDate >= @From
                           && i.InstallmentDate <= @To
                          && a.AdmissionId == i.AdmissionId
                          && (a.R1=="ACTIVE" || a.R1=="ACTIVE RTE")
                          && a.Remove == false && i.CompanyId == Convert.ToInt32(Session["CompanyId"]) && i.SessionId == Convert.ToInt32(Session["SessionId"])
                          orderby i.InstallmentDate.Value ascending
                          select new
                          {
                              StudentName = a.StudentName,
                              ContactNumber = a.ContactNo,
                              TotalCourseFees = a.CourseFeesAfterDisc,
                              TotalTransportFees = a.TransportFeesAfterDisc,
                              AdmiId = a.AdmissionId,
                              ClassName = a.CourseName,
                              Section = a.Section,
                              RollNo = a.AdmissionNo,
                              R1=a.R1
                              //Customer = (e.Title + " " + e.Name + " " + e.LastName),
                              //ContactNumber = e.ContactNumber,
                              //EnquiryId = e.EnquiryID,
                              //InvId = i.InvId,
                              //InvoiceDate = i.InvoiceDate.Value,
                              //Amount = (i.TotalAmount.Value + i.AdditionalCharges.Value),
                              //Broker = b.BrokerName,
                              //BrokerNo = b.ContactNo,
                              //BookingAmount = i.BookingAmount.Value
                          };
                Grid.DataSource = inv.Distinct();
                Grid.DataBind();
            }
            else if (CourseId != 0)
            {
                var inv = from i in obj.MonthlyInstallments
                          from a in obj.Addmisions
                          where i.InstallmentDate >= @From
                          && i.InstallmentDate <= @To
                          && a.AdmissionId == i.AdmissionId
                          && a.CourseId == CourseId
                          && (a.R1 == "ACTIVE" || a.R1 == "ACTIVE RTE")
                          && a.Remove == false && i.CompanyId == Convert.ToInt32(Session["CompanyId"]) && i.SessionId == Convert.ToInt32(Session["SessionId"])
                          orderby i.InstallmentDate.Value ascending
                          select new
                          {
                              StudentName = a.StudentName,
                              ContactNumber = a.ContactNo,
                              TotalCourseFees = a.CourseFeesAfterDisc,
                              TotalTransportFees = a.TransportFeesAfterDisc,
                              AdmiId = a.AdmissionId,
                              ClassName = a.CourseName,
                              Section = a.Section,
                              RollNo = a.AdmissionNo,
                              R1 = a.R1
                          };
                Grid.DataSource = inv.Distinct();
                Grid.DataBind();
            }
        }
        catch (Exception ex)
        { }
    }
    protected void Grid_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            double IClassBalTotal = 0;
            double ITransportBalTotal = 0.0;
            double IClassFeesTotal = 0;
            double ITransportFeesTotal = 0.0;
            double IClassPaidTotal = 0;
            double ITransportPaidTotal = 0.0;
            double bookinAmt = 0, TotalPrice = 0;
            double TotalOtherFeesReceived = 0;
            double TotalOtherFees = 0;
            int CId = 0;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int AdmisId = Convert.ToInt32(Grid.DataKeys[e.Row.RowIndex].Value.ToString());
                IEnumerable<MonthlyInstallment> install = from id in obj.MonthlyInstallments
                                                   where id.AdmissionId == AdmisId
                                                   && id.InstallmentDate >= From
                                                   && id.InstallmentDate <= To
                                                   && id.CompanyId == Convert.ToInt32(Session["CompanyId"]) && id.SessionId == Convert.ToInt32(Session["SessionId"])
                                                   select id;
                var GetCourseId = from id in obj.Addmisions
                                                    where id.AdmissionId == AdmisId
                                                    && id.CompanyId == Convert.ToInt32(Session["CompanyId"]) && id.SessionId == Convert.ToInt32(Session["SessionId"])
                                                    select id;
                CId = Convert.ToInt32(GetCourseId.First().CourseId);

                IEnumerable<Payment> pay = from id in obj.Payments
                                                          where id.AdmissionId == AdmisId
                                                          && id.ComapanyId == Convert.ToInt32(Session["CompanyId"]) && id.SessionId == Convert.ToInt32(Session["SessionId"])
                                                          select id;

                if (pay.Count<Payment>() > 0)
                {
                    TotalOtherFeesReceived = Convert.ToDouble(pay.Sum(s => s.AdmissionFees.Value)) + Convert.ToDouble(pay.Sum(s => s.OtherFees.Value));
                    foreach (Payment id in pay)
                    {
                        OtherFeesReceivedTotal += Convert.ToDouble(id.AdmissionFees.Value) + Convert.ToDouble(id.OtherFees.Value);
                    }
                }
                IEnumerable<Other_Fee> OtherFee = from id in obj.Other_Fees
                                                  where id.CourseId == CId && id.Remove==false
                                                 && id.CompanyId == Convert.ToInt32(Session["CompanyId"]) && id.SessionId == Convert.ToInt32(Session["SessionId"])
                                                  select id;
                if (OtherFee.Count<Other_Fee>() > 0)
                {
                    TotalOtherFees = Convert.ToDouble(OtherFee.Sum(s => s.Fees.Value));
                    foreach (Other_Fee id in OtherFee)
                    {
                    OtherFeesTotal += Convert.ToDouble(id.Fees.Value);
                    }
                }
                if (install.Count<MonthlyInstallment>() > 0)
                {
                    string InstallString = "<table cellspacing='0'>";
                    foreach (MonthlyInstallment id in install)
                    {
                        ClassFeesTotal += Convert.ToDouble(id.CourseFees.Value);
                        TransportFeesTotal += Convert.ToDouble(id.TransportFees.Value);
                        ClassPaidTotal += Convert.ToDouble(id.CousePaid.Value);
                        TransportPaidTotal += Convert.ToDouble(id.TransportPaid.Value);
                        ClassBalTotal += Convert.ToDouble(id.CourseBalance.Value);
                        TransportBalTotal += Convert.ToDouble(id.TransportBalance.Value);
                        InstallString += "<tr><td class='installr'>" + id.Months + "</td><td class='installr'>" + id.InstallmentDate.Value.ToString("dd/MM/yy") + "</td><td class='installr'>" + (id.CourseFees.Value).ToString("##,##,##,####.00") + "</td><td class='installr'>" + (id.TransportFees.Value).ToString("##,##,##,####.00") + "</td><td class='installr'>" + (id.CousePaid.Value).ToString("##,##,##,####.00") + "</td><td class='installr'>" + (id.TransportPaid.Value).ToString("##,##,##,####.00") + "</td><td class='installr'>" + (id.CourseBalance.Value).ToString("##,##,##,####.00") + "</td><td class='installr'>" + (id.TransportBalance.Value).ToString("##,##,##,####.00") + "</td><td class='installr'></td><td class='installr'></td><td class='installr'></td></tr>";
                    }
                    if (install.Count<MonthlyInstallment>() > 0)
                    {
                        IClassFeesTotal = Convert.ToDouble(install.Sum(s => s.CourseFees.Value));
                        ITransportFeesTotal = Convert.ToDouble(install.Sum(s => s.TransportFees.Value));
                        IClassPaidTotal = Convert.ToDouble(install.Sum(s => s.CousePaid.Value));
                        ITransportPaidTotal = Convert.ToDouble(install.Sum(s => s.TransportPaid.Value));
                        IClassBalTotal = Convert.ToDouble(install.Sum(s => s.CourseBalance.Value));
                        ITransportBalTotal = Convert.ToDouble(install.Sum(s => s.TransportBalance.Value));
                    }
                    InstallString += "<tr style='font-weight:bold;'><td class='installr'></td><td class='installr'>Total</td><td class='installr'>" + (IClassFeesTotal).ToString("##,##,##,####.00") + "</td><td class='installr'>" + (ITransportFeesTotal).ToString("##,##,##,####.00") + "</td><td class='installr'>" + (IClassPaidTotal).ToString("##,##,##,####.00") + "</td><td class='installr'>" + (ITransportPaidTotal).ToString("##,##,##,####.00") + "</td><td class='installr'>" + (IClassBalTotal).ToString("##,##,##,####.00") + "</td><td class='installr'>" + (ITransportBalTotal).ToString("##,##,##,####.00") + "</td><td class='installr'>" + (TotalOtherFees).ToString("##,##,##,####.00") + "</td><td class='installr'>" + (TotalOtherFeesReceived).ToString("##,##,##,####.00") + "</td><td class='installr'>" + (TotalOtherFees - TotalOtherFeesReceived).ToString("##,##,##,####.00") + "</td></tr>";
                    InstallString += "</table>";
                    e.Row.Cells[5].Text = InstallString;
                }


            }
            if (e.Row.RowType == DataControlRowType.Header)
            {

                e.Row.Cells[5].Text = "<table><tr><td class='installr'>MONTHS</td><td class='installr'>DATE</td><td class='installr'>SCHOOL FEE</td><td class='installr'>TRANSPORT FEE</td><td class='installr'>SCHOOL FEE RECEIVED</td><td class='installr'>TRANSPORT FEE RECEIVED</td><td class='installr'>SCHOOL FEE BAL.</td><td class='installr'>TRANSPORT FEE BAL.</td><td class='installr'>TOTAL OTHER FEE</td><td class='installr'>TOTAL OTHER FEE RECEIVED</td><td class='installr'>TOTAL OTHER FEE BAL.</td></tr></table>";
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {

                foreach (DataKey d in Grid.DataKeys)
                {
                    int InID = Convert.ToInt32(d.Value.ToString());


                    IEnumerable<Addmision> inv = from i in obj.Addmisions
                                                 where i.AdmissionId == InID
                                                 && i.CompanyId == Convert.ToInt32(Session["CompanyId"]) && i.SessionId == Convert.ToInt32(Session["SessionId"])
                                                 select i;
                    if (inv.Count<Addmision>() > 0)
                    {
                        bookinAmt += Convert.ToDouble(inv.Sum<Addmision>(s => s.CourseFeesAfterDisc.Value));
                        TotalPrice += Convert.ToDouble(inv.Sum<Addmision>(s => s.TransportFeesAfterDisc.Value));
                    }
                    if (CourseId == 0)
                    {
                        IEnumerable<MonthlyInstallment> install = from id in obj.MonthlyInstallments
                                                                  where id.InstallmentDate >= From
                                                                  && id.InstallmentDate <= To
                                                                  && id.CompanyId == Convert.ToInt32(Session["CompanyId"]) && id.SessionId == Convert.ToInt32(Session["SessionId"])
                                                                  select id;

                        if (install.Count<MonthlyInstallment>() > 0)
                        {
                            foreach (MonthlyInstallment id in install)
                            {
                                if (install.Count<MonthlyInstallment>() > 0)
                                {
                                    ClassFeesTotal = Convert.ToDouble(install.Sum(s => s.CourseFees.Value));
                                    TransportFeesTotal = Convert.ToDouble(install.Sum(s => s.TransportFees.Value));
                                    ClassPaidTotal = Convert.ToDouble(install.Sum(s => s.CousePaid.Value));
                                    TransportPaidTotal = Convert.ToDouble(install.Sum(s => s.TransportPaid.Value));
                                    ClassBalTotal = Convert.ToDouble(install.Sum(s => s.CourseBalance.Value));
                                    TransportBalTotal = Convert.ToDouble(install.Sum(s => s.TransportBalance.Value));
                                }

                            }
                        }
                        string footer = "";
                        footer += "<table>";
                        footer += "<tr><td class='installr'></td><td class='installr'></td><td class='installr'></td><td class='installr'>" + (ClassFeesTotal).ToString("##,##,##,####.00") + "</td><td class='installr'>" + (TransportFeesTotal).ToString("##,##,##,####.00") + "</td><td class='installr'>" + (ClassPaidTotal).ToString("##,##,##,####.00") + "</td><td class='installr'>" + (TransportPaidTotal).ToString("##,##,##,####.00") + "</td><td class='installr'>" + (ClassBalTotal).ToString("##,##,##,####.00") + "</td><td class='installr'>" + (TransportBalTotal).ToString("##,##,##,####.00") + "</td><td class='installr'>" + (OtherFeesTotal).ToString("##,##,##,####.00") + "</td><td class='installr'>" + (OtherFeesReceivedTotal).ToString("##,##,##,####.00") + "</td><td class='installr'>" + (OtherFeesTotal - OtherFeesReceivedTotal).ToString("##,##,##,####.00") + "</td></tr>";
                        footer += "</table>";

                        e.Row.Cells[5].Text = footer;
                        footer = "<table>";
                        footer += "<tr><td class='installr'>TOTAL</td></tr>";
                        footer += "</table>";
                        e.Row.Cells[2].Text = footer;
                        e.Row.Cells[3].Text = bookinAmt.ToString("##,##,##,####.00");
                        e.Row.Cells[4].Text = TotalPrice.ToString("##,##,##,####.00");
                    }
                    else if (CourseId != 0)
                    {
                        string footer = "";
                        footer += "<table>";
                        footer += "<tr><td class='installr'></td><td class='installr'></td><td class='installr'>" + (ClassFeesTotal).ToString("##,##,##,####.00") + "</td><td class='installr'>" + (TransportFeesTotal).ToString("##,##,##,####.00") + "</td><td class='installr'>" + (ClassPaidTotal).ToString("##,##,##,####.00") + "</td><td class='installr'>" + (TransportPaidTotal).ToString("##,##,##,####.00") + "</td><td class='installr'>" + (ClassBalTotal).ToString("##,##,##,####.00") + "</td><td class='installr'>" + (TransportBalTotal).ToString("##,##,##,####.00") + "</td><td class='installr'>" + (OtherFeesTotal).ToString("##,##,##,####.00") + "</td><td class='installr'>" + (OtherFeesReceivedTotal).ToString("##,##,##,####.00") + "</td><td class='installr'>" + (OtherFeesTotal - OtherFeesReceivedTotal).ToString("##,##,##,####.00") + "</td></tr>";
                        footer += "</table>";

                        e.Row.Cells[5].Text = footer;
                        footer = "<table>";
                        footer += "<tr><td class='installr'>TOTAL</td></tr>";
                        footer += "</table>";
                        e.Row.Cells[2].Text = footer;
                        e.Row.Cells[3].Text = bookinAmt.ToString("##,##,##,####.00");
                        e.Row.Cells[4].Text = TotalPrice.ToString("##,##,##,####.00");
                    }
                }
            }
        }
        catch (Exception ex)
        { }
    }
    protected void ddlWidth_SelectedIndexChanged(object sender, EventArgs e)
    {
        Grid.Width = new Unit(double.Parse(ddlWidth.SelectedValue));
    }
}