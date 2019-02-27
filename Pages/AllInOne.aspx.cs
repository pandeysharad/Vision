using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Pages_AllInOne : System.Web.UI.Page
{
    int LogId = 0;
    public int srno = 1;
    DataClassesDataContext obj = new DataClassesDataContext();
    LoginRole role = new LoginRole();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["UserId"] != null)
            {
                if (Session["UserType"].ToString() != "Admin")
                {
                    IEnumerable<LoginRole> roles = from r in obj.LoginRoles
                                                   where r.LoginId.Value == Convert.ToInt32(Session["UserId"]) && r.RoleId == 11
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
            if (!IsPostBack)
            {

                if (Session["CompanyId"] != null)
                {
                    getTableHeads();
                    //ddltablehead.ClearSelection();
                    //for (int i = 0; i < ddltablehead.Items.Count; i++)
                    //{
                    //    if (ddltablehead.Items[i].Text.Equals("BANK"))
                    //    {
                    //        ddltablehead.SelectedIndex = i; break;
                    //    }
                    //}
                    //ddltablehead.Enabled = false;
                    Getadata();
                }
                else
                {
                    Response.Redirect("../Default.aspx");
                }
            }
        }
        catch (Exception ex) { }

    }
    private void getTableHeads()
    {
        ddltablehead.DataSource = obj.SP_GET_SINGLEVALUETABLES(0);
        ddltablehead.DataValueField = "STID";
        ddltablehead.DataTextField = "TABLENAME";
        ddltablehead.DataBind();
        ddltablehead.Items.Insert(0, new ListItem("--Select--", "0"));
    }
    protected void ddltablehead_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (role.Select.Value)
        {
            Getadata();
        }
        else Globals.Message(Page, "Not Authorized ??");
    }
    protected void btnsaveem_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (role.Insert.Value)
            {
                if (ddltablehead.SelectedIndex > 0)
                {

                    ImageButton img = (ImageButton)sender;
                    GridViewRow grow = (GridViewRow)img.NamingContainer;
                    TextBox txtval = (TextBox)grow.FindControl("txteminsertvalue");
                    TextBox txtvalue = (TextBox)grow.FindControl("txtvalue");
                    if (string.IsNullOrEmpty(txtval.Text))
                    {
                        Globals.Message(this.Page, "Please enter head...");
                        txtval.Focus();
                    }
                    else
                    {

                        if (obj.SP_SINGLEVALUEDATA(1, 0, Convert.ToInt32(ddltablehead.SelectedItem.Value), txtval.Text.ToUpper().Trim(), string.Empty, false, Convert.ToInt32(Session["UserId"]), DateTime.Now, Convert.ToInt32(Session["UserId"]), DateTime.Now) == 0)
                        {
                            Globals.Message(this.Page, "Record Saved !!");
                            Getadata(); txtvalue.Focus();

                        }
                        else
                        {
                            Globals.Message(this.Page, "Record Not Saved !!");
                        }
                    }
                }
                else
                {
                    Globals.Message(this.Page, "Please select heading...");
                    ddltablehead.Focus();
                }
            }
            else Globals.Message(Page, "Not Authorized ??");
        }
        catch (Exception epp)
        {
            Globals.Message(this.Page, "Record Not Saved " + epp.Message);
        }

    }
    protected void btnsave_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (role.Insert.Value)
            {
                if (ddltablehead.SelectedIndex > 0)
                {
                    ImageButton img = (ImageButton)sender;
                    GridViewRow grow = (GridViewRow)img.NamingContainer;
                    TextBox txtval = (TextBox)grow.FindControl("txtinsertvalue");
                    if (string.IsNullOrEmpty(txtval.Text))
                    {
                        Globals.Message(this.Page, "Please enter head...");
                        txtval.Focus();
                    }
                    else
                    {

                        if (obj.SP_SINGLEVALUEDATA(1, 0, Convert.ToInt32(ddltablehead.SelectedItem.Value), txtval.Text.ToUpper().Trim(), string.Empty, false, Convert.ToInt32(Session["UserId"]), DateTime.Now, Convert.ToInt32(Session["UserId"]), DateTime.Now) == 0)
                        {
                            Globals.Message(this.Page, "Record Saved !!");
                            Getadata();
                            txtval.Focus();
                        }
                        else
                        {
                            Globals.Message(this.Page, "Record Not Saved !!");
                        }
                    }
                }
                else
                {
                    Globals.Message(this.Page, "Please select heading...");
                    ddltablehead.Focus();
                }
            }
            else Globals.Message(Page, "Not Authorized ??");
        }
        catch (Exception epp)
        {
            Globals.Message(this.Page, "Record Not Saved " + epp.Message);
        }

    }
    protected void btnedit_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (role.Update.Value)
            {
                if (ddltablehead.SelectedIndex > 0)
                {

                    ImageButton img = (ImageButton)sender;
                    GridViewRow grow = (GridViewRow)img.NamingContainer;
                    Label lblvalue = (Label)grow.FindControl("lblvalue");
                    TextBox txtvalue = (TextBox)grow.FindControl("txtvalue");
                    ImageButton btnupdate = (ImageButton)grow.FindControl("btnupdate");

                    lblvalue.Visible = false;
                    txtvalue.Visible = true;
                    btnupdate.Visible = true;
                }
                else
                {
                    Globals.Message(this.Page, "Please select heading...");
                    ddltablehead.Focus();
                }
            }
            else Globals.Message(Page, "Not Authorized ??");
        }
        catch (Exception epp)
        {
            Globals.Message(this.Page, "Record Not Deleted " + epp.Message);
        }

    }

    protected void btndelete_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (role.Delete.Value)
            {
                if (ddltablehead.SelectedIndex > 0)
                {
                    ImageButton img = (ImageButton)sender;
                    GridViewRow grow = (GridViewRow)img.NamingContainer;
                    string id = Grd.DataKeys[grow.RowIndex].Value.ToString();
                    TextBox txtvalue = (TextBox)grow.FindControl("txtvalue");
                    if (string.IsNullOrEmpty(id))
                    {
                        Globals.Message(this.Page, "Please enter head...");
                        txtvalue.Focus();
                    }
                    else
                    {

                        if (obj.SP_SINGLEVALUEDATA(3, Convert.ToInt32(id), Convert.ToInt32(ddltablehead.SelectedItem.Value), string.Empty, string.Empty, false, Convert.ToInt32(Session["UserId"]), DateTime.Now, Convert.ToInt32(Session["UserId"]), DateTime.Now) == 0)
                        {
                            Globals.Message(this.Page, "Record Deleted !!");
                            Getadata();
                            txtvalue.Focus();

                        }
                        else
                        {
                            Globals.Message(this.Page, "Record Not Deleted !!");
                        }
                    }
                }
                else
                {
                    Globals.Message(this.Page, "Please select heading...");
                    ddltablehead.Focus();
                }
            }
            else Globals.Message(Page, "Not Authorized ??");
        }
        catch (Exception epp)
        {
            Globals.Message(this.Page, "Record Not Deleted " + epp.Message);
        }

    }
    protected void btnupdate_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (role.Update.Value)
            {
                if (ddltablehead.SelectedIndex > 0)
                {
                    ImageButton img = (ImageButton)sender;
                    GridViewRow grow = (GridViewRow)img.NamingContainer;
                    string id = Grd.DataKeys[grow.RowIndex].Value.ToString();
                    TextBox txtvalue = (TextBox)grow.FindControl("txtvalue");
                    if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(txtvalue.Text))
                    {

                    }
                    else
                    {

                        if (obj.SP_SINGLEVALUEDATA(2, Convert.ToInt32(id), Convert.ToInt32(ddltablehead.SelectedItem.Value), txtvalue.Text.ToUpper().Trim(), string.Empty, false, Convert.ToInt32(Session["UserId"]), DateTime.Now, Convert.ToInt32(Session["UserId"]), DateTime.Now) == 0)
                        {
                            Globals.Message(this.Page, "Record Updated !!");
                            Getadata();
                            txtvalue.Focus();

                        }
                        else
                        {
                            Globals.Message(this.Page, "Record Not Updated !!");
                        }
                    }
                }
            }
            else Globals.Message(Page, "Not Authorized ??");

        }
        catch (Exception epp)
        {
            Globals.Message(this.Page, "Record Not Updated " + epp.Message);
        }

    }


    private void Getadata()
    {
        try
        {
            if (role.Select.Value)
            {
                Grd.DataSource = obj.SP_GET_SINGLEVALUEDATA(Convert.ToInt32(ddltablehead.SelectedValue));
                Grd.DataBind();
            }
        }
        catch
        {

        }
    }


}