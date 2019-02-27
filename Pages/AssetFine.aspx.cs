﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Pages_AssetFine : System.Web.UI.Page
{
    public static string StudentName = "", FatherName = "", ClassSection = "";
    DataClassesDataContext obj = new DataClassesDataContext();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            FillDropdown.FillSINGLEVALUEDATA(ddlFineType, "ASSET TYPE");
            FillDropdown.FillSINGLEVALUEDATA(ddlFineTpePaid, "ASSET TYPE");
            FillDropdown.FillClassList(ddlClassName);
            ShowOrHideSectionReciveAndAdd();
        }
    }
    public void GetSudentInfo(string AddmisionNo)
    {
        StudentName = ""; FatherName = ""; ClassSection = "";
        if (txtAdminNo.Text != string.Empty)
        {
            var _studentInfo = obj.Addmisions.Where(x => x.AdmissionNo == AddmisionNo.Trim());
            StudentName = _studentInfo.First().StudentName;
            FatherName = _studentInfo.First().FatherName;
            ClassSection = _studentInfo.First().CourseName + "-" + _studentInfo.First().Section;
        }
        ShowHideStudentDetails();
    }
    protected void btnSearchDetails_Click(object sender, EventArgs e)
    {
        GetSudentInfo(txtAdminNo.Text);
        //GetAdmissionDetailsByAddmissionNo(txtAdminNo.Text);
    }
    protected void btnAllFeesRefresh_Click(object sender, EventArgs e)
    {
        try
        {   
            Response.Redirect(Request.RawUrl);
        }
        catch (Exception ex)
        { }
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
        GetSudentInfo(txtAdminNo.Text);
    }
    protected void txtAdminNo_TextChanged(object sender, EventArgs e)
    {
        if (txtAdminNo.Text.Length == 8)
        {
            GetSudentInfo(txtAdminNo.Text);
        }
        else
        {
            Globals.Message(Page, "Please enter correct scholler number");
            txtAdminNo.Focus();
        }
    }
    public void ShowHideStudentDetails()
    {
        if (txtAdminNo.Text != null || txtAdminNo.Text != string.Empty)
        {
            tableStudentDetails.Visible = true;
        }
        else
        {
            tableStudentDetails.Visible = false;
        }
    }
    public bool Validate()
    { 
        bool flag=true;
        if (txtAdminNo.Text == string.Empty)
        {
            flag = false;
            txtAdminNo.Focus();
        }
        if (txtFineAmount.Text == string.Empty)
        {
            flag = false;
            txtFineAmount.Focus();
        }
        if (ddlFineType.SelectedIndex == 0)
        {
            flag = false;
            ddlFineType.Focus();
        }
        return flag;
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (Validate())
        {
            var DATA = from p in obj.Addmisions
                       where p.AdmissionNo == txtAdminNo.Text.Trim()
                       select p;
            int AdmissionId = Convert.ToInt32(DATA.First().AdmissionId);
            if (obj.SP_AssetFine(1, 0, AdmissionId, txtAdminNo.Text.Trim(), Convert.ToInt32(ddlFineType.SelectedValue), Convert.ToDouble(string.IsNullOrEmpty(txtFineAmount.Text) ? "0" : txtFineAmount.Text), txtRemarks.Text.Trim(), "DUE", "", "", "", Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["SessionId"]), 0, Convert.ToInt32(Session["UserId"])) == 0)
            {
                Clear();
                Globals.Message(Page, "Records Save !");
            }
        }
        {
            Globals.Message(Page, "Please full/fill mandatory field !");
        }
    }
    public void Clear()
    {
        txtFineAmount.Text = string.Empty;
        txtRemarks.Text = string.Empty;
        ddlFineType.ClearSelection();
    }
    public void ShowOrHideSectionReciveAndAdd()
    {
        try
        {
            if (Convert.ToInt32(Request.QueryString["id"]) > 0)
            {
                tablePaidFine.Visible = true;
                tableAddAssetFine.Visible = false;
                GetAssetFineDataById();
                GetRecieptNo();
            }
            else
            {
                tablePaidFine.Visible = false;
                tableAddAssetFine.Visible = true;
            }
        }
        catch (Exception)
        {
            
            throw;
        }
    }
    public void GetAssetFineDataById()
    {
        try
        {
            var _assetFine = obj.AssetFines.Where(x => x.id == Convert.ToInt32(Request.QueryString["id"]));
            ddlFineTpePaid.SelectedValue = _assetFine.First().AssetType.ToString();
            txtFineAmountPaid.Text = _assetFine.First().Fine.ToString();
            txtRemarksPaid.Text = _assetFine.First().Remark.ToString();
            txtAdminNo.Text = _assetFine.First().AddmisionNo.ToString();
            dtReceivedDate.Text = System.DateTime.Now.ToShortDateString();
        }
        catch (Exception)
        {

            throw;
        }
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
            }
        }
        catch (Exception ex)
        { }
    }
    public bool ValidatePaid()
    {
        bool flag = true;
        if (txtAdminNo.Text == string.Empty)
        {
            flag = false;
            txtAdminNo.Focus();
        }
        if (txtFineAmountPaid.Text == string.Empty)
        {
            flag = false;
            txtFineAmountPaid.Focus();
        }
        if (ddlFineTpePaid.SelectedIndex == 0)
        {
            flag = false;
            ddlFineTpePaid.Focus();
        }
        if (dtReceivedDate.Text == string.Empty)
        {
            flag = false;
            ddlFineType.Focus();
        }
        return flag;
    }
    public void InsertRecord()
    {
        GetRecieptNo();
        var _assetFine = obj.AssetFines.Where(x => x.id == Convert.ToInt32(Request.QueryString["id"]));
        var _addmisions = obj.Addmisions.Where(x => x.AdmissionId == Convert.ToInt32(_assetFine.First().Addmisionid));
        if (obj.Sp_AssetFinePayment(1, 0, Convert.ToInt32(_assetFine.First().Addmisionid), Convert.ToInt32(_addmisions.First().CourseId), _addmisions.First().AdmissionNo, Convert.ToInt32(txtReceiptNo.Text), Convert.ToDateTime(dtReceivedDate.Text), ddlPaymentMode.Text, 0,
                                                    0, 0, 0,
                                                    "", 0, Convert.ToDecimal(txtFineAmountPaid.Text), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["UserId"]), Convert.ToInt32(Session["SessionId"]),
                                                    txtBankName.Text, txtchequeNo.Text, txtChequeDate.Text != "" ? Convert.ToDateTime(txtChequeDate.Text) : System.DateTime.Now, 0, txtRemarksPaid.Text, "", "", 0,
                                                    "", "", "", "", "", 0, Convert.ToDouble(txtFineAmountPaid.Text), Convert.ToInt32(Request.QueryString["id"])) == 0)
        {
            Response.Redirect("FeesRecieve.aspx?AdmissionNo=" + txtAdminNo.Text.Trim());
        }
    }
    public void GetRecieptNo()
    {
        txtReceiptNo.Text = ((from Cons in obj.Payments
                              where Cons.ComapanyId == Convert.ToInt32(Session["CompanyId"]) && Cons.SessionId == Convert.ToInt32(Session["SessionId"])
                              select Cons.ReceiptNo).Max() + 1).ToString();
    }
    protected void btnSavePaid_Click(object sender, EventArgs e)
    {
        if (ValidatePaid())
        {
            InsertRecord();
        }
        else
        {
            Globals.Message(Page, "please fill in all required fields !");
        }
    }
}