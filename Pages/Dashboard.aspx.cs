using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Pages_Dashboard : System.Web.UI.Page
{
    public int srnoTop20Defaulter = 0;
    DataClassesDataContext obj = new DataClassesDataContext();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["CompanyId"] != null)
            {
                Session["CompanyId"] = Session["CompanyId"].ToString();
                Session["UserName"] = Session["UserName"].ToString();
                if (Session["UserType"].ToString() == "Admin")
                {
                    GetData();
                }
                else {
                    UpdatePanel6.Visible = false;
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
    protected void GetData()
    {
        DataSet ds = AllMethods.GetDashboard(1, Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["SessionId"]));
        GridData.DataSource = ds;
        GridData.DataBind();
        //Today Collection
        DataSet dsTodayCollection = AllMethods.GetDashboard(2, Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["SessionId"]));
        foreach (DataRow dr in dsTodayCollection.Tables[0].Rows)
        {
            lblTodayCollection.Text = dr[0].ToString();
        }
        //Student Staus
        DataSet dsStudentStatus = AllMethods.GetDashboard(3, Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["SessionId"]));
        GridStudentStaus.DataSource = dsStudentStatus;
        GridStudentStaus.DataBind();
        //Approval Students List
        var _GridUpdateApprovals = from p in obj.GridUpdateApprovals
                                   where p.UpdateStatus == "Request" && p.isdel == false
                                   select new { AdmissionId=p.AdmissionId,
                                                AdmissionNo = p.AdmissionNo,
                                                R2=p.R2
                                   };
        if (_GridUpdateApprovals.Count() > 0)
        {
            SectionGridUpdateApprovals.Visible = true;
        }
        else
        {
            SectionGridUpdateApprovals.Visible = false;
        }
        //var _GridUpdateApprovals = obj.GridUpdateApprovals.ToList().Distinct().Where(m => m.isdel == false && m.UpdateStatus == "Request");
        GridGridUpdateApprovals.DataSource = _GridUpdateApprovals.Distinct();
        GridGridUpdateApprovals.DataBind();
        //Top20 Due List
        DataSet dsTop20DueList = AllMethods.GetDashboard(4, Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["SessionId"]));
        GridTop20Defaulter.DataSource = dsTop20DueList;
        GridTop20Defaulter.DataBind();
        //Get Receipt Approvel List
        try
        {
            DataSet GetReceiptApprovelList = AllMethods.GetDashboard(5, Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["SessionId"]));
            GridReceiptApprovel.DataSource = GetReceiptApprovelList;
            GridReceiptApprovel.DataBind();
        }
        catch (Exception)
        {
            
            throw;
        }
    }
    protected void btnApproved_Click(object sender, EventArgs e)
    {
        LinkButton btn = sender as LinkButton;
        GridViewRow row = btn.NamingContainer as GridViewRow;
        int AdmissionId = Convert.ToInt32(GridGridUpdateApprovals.DataKeys[row.RowIndex].Values[0].ToString());
        var _GridUpdateApprovals = obj.GridUpdateApprovals.ToList().Where(m => m.isdel == false && m.UpdateStatus == "Request" && m.AdmissionId==AdmissionId);
        foreach (var item in _GridUpdateApprovals)
        {
            if (obj.SP_UpdateFeeGrid(1,item.id, item.AdmissionNo, Convert.ToInt32(item.R1), item.ClassFees, item.TransportFees, item.Date, item.ClassPaid, item.TransportPaid, item.ClassBalance, item.TransportBalance, item.PaymentStatus) == 0)
            {
            
            }
        }
        GetData();
        Globals.Message(Page, "Grid successfully update !!!");
    }
    protected void btnView_Click(object sender, EventArgs e)
    {
        LinkButton btn = sender as LinkButton;
        GridViewRow row = btn.NamingContainer as GridViewRow;
        int AdmissionId = Convert.ToInt32(GridGridUpdateApprovals.DataKeys[row.RowIndex].Values[0].ToString());
        ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "", "window.open('../Reports/CompareGrid.aspx?AdmissionId=" + AdmissionId + "','');", true);
        
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        LinkButton btn = sender as LinkButton;
        GridViewRow row = btn.NamingContainer as GridViewRow;
        int AdmissionId = Convert.ToInt32(GridGridUpdateApprovals.DataKeys[row.RowIndex].Values[0].ToString());
        var _GridUpdateApprovals = obj.GridUpdateApprovals.ToList().Where(m => m.isdel == false && m.UpdateStatus == "Request" && m.AdmissionId == AdmissionId);
        foreach (var item in _GridUpdateApprovals)
        {
            if (obj.SP_UpdateFeeGrid(2,item.id, item.AdmissionNo, Convert.ToInt32(item.R1), item.ClassFees, item.TransportFees, item.Date, item.ClassPaid, item.TransportPaid, item.ClassBalance, item.TransportBalance, item.PaymentStatus) == 0)
            {

            }
        }
        GetData();
        Globals.Message(Page, "Grid successfully Canceled !!!");
    }
    protected void btnReceiptApprovel_Click(object sender, EventArgs e)
    {
        LinkButton btn = sender as LinkButton;
        GridViewRow row = btn.NamingContainer as GridViewRow;
        int paymentid = Convert.ToInt32(GridReceiptApprovel.DataKeys[row.RowIndex].Values[0].ToString());
        var _RecieptHistories = from p in obj.RecieptHistories where p.isdel == false && p.PaymentId == paymentid select p;
                    
        foreach (var item in _RecieptHistories)
        {
            int respectTo = Convert.ToInt32(item.RespectTo);
            decimal classPaidAmt = Convert.ToDecimal(item.CousePaid);
            decimal transportPaidAmt = Convert.ToDecimal(item.TransportPaid);
            int admissionId = Convert.ToInt16(item.AdmissionId);
            obj.SP_RecieptHistory(3, 0, paymentid, respectTo, admissionId, Convert.ToDouble(classPaidAmt), Convert.ToDouble(transportPaidAmt), "", "", "", "");
        }
        if (obj.SP_RecieptHistory(2, 0, paymentid, 0, 0, Convert.ToDouble(0), Convert.ToDouble(0), "", Session["UserId"].ToString(), "", "") == 0)
        {
            Globals.Message(Page, "Record Cancelled");
        }
    }
    protected void btnReceiptApprovelCancel_Click(object sender, EventArgs e)
    {
        LinkButton btn = sender as LinkButton;
        GridViewRow row = btn.NamingContainer as GridViewRow;
        int paymentid = Convert.ToInt32(GridReceiptApprovel.DataKeys[row.RowIndex].Values[0].ToString());
        if (obj.SP_RecieptHistory(5, 0, paymentid, 0, 0, Convert.ToDouble(0), Convert.ToDouble(0), "", Session["UserId"].ToString(), "", "") == 0)
        {
            Globals.Message(Page, "Record not approv");
        }
    }
}