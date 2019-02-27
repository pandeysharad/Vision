using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Globalization;

/// <summary>
/// Summary description for FillDropdown
/// </summary>
public class FillDropdown
{
    static DataClassesDataContext obj = new DataClassesDataContext();
    
	public FillDropdown()
	{
		
	}
    public static DataTable GetiNVESTIGATIONDETAILSBySlipId(int slipid)
    {
          DataTable dt = new DataTable();
        using (SqlConnection conn = new SqlConnection(Globals.connectionString))
        {

          
            using (SqlCommand cm = new SqlCommand("SP_GETINVESTIGATIONDETAILSBY_SLIPID", conn))
            {
                cm.Parameters.AddWithValue("@slipid", slipid);
                SqlDataAdapter sqlDa = new SqlDataAdapter(cm);
                cm.CommandType = CommandType.StoredProcedure;
                sqlDa.Fill(dt);
               
            }
        }
        return dt;
    }

   

    public static DataSet GetTestForHealthPlanBySlipId(int slipid)
    {
        DataSet dt = new DataSet();
        using (SqlConnection conn = new SqlConnection(Globals.connectionString))
        {


            using (SqlCommand cm = new SqlCommand("SP_GETHEALTHPLANTESTSBY_SLIPID", conn))
            {
                cm.Parameters.AddWithValue("@SLIPID", slipid);
                SqlDataAdapter sqlDa = new SqlDataAdapter(cm);
                cm.CommandType = CommandType.StoredProcedure;
                sqlDa.Fill(dt);

            }
        }
        return dt;
    }

    public static DataSet SP_GETDETAILFORCRTOCS(int slipid)
    {
        DataSet dt = new DataSet();
        using (SqlConnection conn = new SqlConnection(Globals.connectionString))
        {


            using (SqlCommand cm = new SqlCommand("SP_GETDETAILFORCRTOCS", conn))
            {
                SqlDataAdapter sqlDa = new SqlDataAdapter(cm);
                cm.Parameters.AddWithValue("@slipid", slipid);
                cm.CommandType = CommandType.StoredProcedure;
                sqlDa.Fill(dt);

            }
        }
        return dt;
    }

    public static DataTable SP_GETOPERATIONRESOURCEBYSLIPID(int slipid)
    {
          DataTable dt = new DataTable();
        using (SqlConnection conn = new SqlConnection(Globals.connectionString))
        {


            using (SqlCommand cm = new SqlCommand("SP_GETOPERATIONRESOURCEBYSLIPID", conn))
            {
                cm.Parameters.AddWithValue("@slipid", slipid);
                SqlDataAdapter sqlDa = new SqlDataAdapter(cm);
                cm.CommandType = CommandType.StoredProcedure;
                sqlDa.Fill(dt);
               
            }
        }
        return dt;
    }

    

    public static void FillTestHead(DropDownList ddl)
    {
        using (SqlConnection conn = new SqlConnection(Globals.connectionString))
        {
            DataTable dt = new DataTable();
            using (SqlCommand cm = new SqlCommand("select * from investigationheads where isdeleted=0 ", conn))
            {
                SqlDataAdapter sqlDa = new SqlDataAdapter(cm);
                cm.CommandType = CommandType.Text;
                sqlDa.Fill(dt);
                ddl.DataSource = dt;
                ddl.DataTextField = "InvestigationHead";
                ddl.DataValueField = "InvestigationheadID";
                ddl.DataBind();
                ddl.Items.Insert(0, new ListItem("--SELECT HEAD--", "0"));

            }
        }
    }
    public static void FillMonth(DropDownList ddl)
    {
        ddl.Items.Insert(0, new ListItem("--Select--", "0"));
        ddl.Items.Insert(1, new ListItem("April", "4"));
        ddl.Items.Insert(2, new ListItem("May", "5"));
        ddl.Items.Insert(3, new ListItem("June", "6"));
        ddl.Items.Insert(4, new ListItem("July", "7"));
        ddl.Items.Insert(5, new ListItem("August", "8"));
        ddl.Items.Insert(6, new ListItem("September", "9"));
        ddl.Items.Insert(7, new ListItem("October", "10"));
        ddl.Items.Insert(8, new ListItem("November", "11"));
        ddl.Items.Insert(9, new ListItem("December", "12"));
        ddl.Items.Insert(10, new ListItem("January", "1"));
        ddl.Items.Insert(11, new ListItem("February", "2"));
        ddl.Items.Insert(12, new ListItem("March", "3"));
    }
    //public static void FillMonth(DropDownList ddl )
    //{
    //    var months = CultureInfo.CurrentCulture.DateTimeFormat.MonthNames;

    //    ddl.Items.Insert(0, new ListItem("--Select--", "0"));

    //    for (int i = 0; i < months.Length - 1; i++)
    //    {
    //        ddl.Items.Add(new ListItem(months[i], (i + 1).ToString()));
    //    }

    //}
    public static void FillSessionList(DropDownList ddl)
    {
        var DATA = from Cons in obj.Sessions
                   where Cons.Remove == false
                   select Cons;
        ddl.DataSource = DATA;
        ddl.DataTextField = "SessionName";
        ddl.DataValueField = "Sessionid";
        ddl.DataBind();
        ddl.Items.Insert(0, "Select Session");
        ddl.Focus();
    }
    public static void FillStudentNameByClass(DropDownList ddl, int Courseid)
    {
        using (SqlConnection conn = new SqlConnection(Globals.connectionString))
        {
            DataTable dt = new DataTable();
            using (SqlCommand cm = new SqlCommand("select AdmissionId,StudentName+'/'+AdmissionNo as StudentName from Addmision where Courseid='" + Courseid + "' order by StudentName asc", conn))
            {
                SqlDataAdapter sqlDa = new SqlDataAdapter(cm);
                cm.CommandType = CommandType.Text;
                sqlDa.Fill(dt);
                ddl.DataSource = dt;
                ddl.DataTextField = "StudentName";
                ddl.DataValueField = "AdmissionId";
                ddl.DataBind();
                ddl.Items.Insert(0, new ListItem("--SELECT STUDENT NAME--", "0"));

            }
        }
    }
    public static void FillStudentByClassAndBySection(DropDownList ddl, int Courseid, string section)
    {
        using (SqlConnection conn = new SqlConnection(Globals.connectionString))
        {
            DataTable dt = new DataTable();
            using (SqlCommand cm = new SqlCommand("select AdmissionNo,StudentName+' S/O '+FatherName as StudentName from Addmision where R1 in ('ACTIVE', 'ACTIVE RTE') and courseid='" + Courseid + "' and Section='" + section + "' order by StudentName asc", conn))
            {
                SqlDataAdapter sqlDa = new SqlDataAdapter(cm);
                cm.CommandType = CommandType.Text;
                sqlDa.Fill(dt);
                ddl.DataSource = dt;
                ddl.DataTextField = "StudentName";
                ddl.DataValueField = "AdmissionNo";
                ddl.DataBind();
                ddl.Items.Insert(0, new ListItem("--STUDENT NAME--", "0"));

            }
        }
    }
    public static void FillSectionByClass(DropDownList ddl, int Courseid)
    {
        using (SqlConnection conn = new SqlConnection(Globals.connectionString))
        {
            DataTable dt = new DataTable();
            using (SqlCommand cm = new SqlCommand("select distinct section from Addmision where courseid='" + Courseid + "'  order by  section asc", conn))
            {
                SqlDataAdapter sqlDa = new SqlDataAdapter(cm);
                cm.CommandType = CommandType.Text;
                sqlDa.Fill(dt);
                ddl.DataSource = dt;
                ddl.DataTextField = "section";
                ddl.DataValueField = "section";
                ddl.DataBind();
                ddl.Items.Insert(0, new ListItem("--SECTION--", "0"));

            }
        }
    }
    public static void FillListLedgersbysort(DropDownList ddl)
    {
        using (SqlConnection conn = new SqlConnection(Globals.connectionString))
        {
            DataTable dt = new DataTable();
            using (SqlCommand cm = new SqlCommand("select distinct Ltype from Ledgers where isdeleted=0", conn))
            {
                SqlDataAdapter sqlDa = new SqlDataAdapter(cm);
                cm.CommandType = CommandType.Text;
                sqlDa.Fill(dt);
                ddl.DataSource = dt;
                ddl.DataTextField = "Ltype";
                ddl.DataValueField = "Ltype";
                ddl.DataBind();
                ddl.Items.Insert(0, new ListItem("--filter by--", "0"));

            }
        }
    }

    public static void FillClientQuotationName(DropDownList ddl)
    {
        using (SqlConnection conn = new SqlConnection(Globals.connectionString))
        {
            DataTable dt = new DataTable();
            using (SqlCommand cm = new SqlCommand("select * from SOL_QuotationClient where isdel=0", conn))
            {
                SqlDataAdapter sqlDa = new SqlDataAdapter(cm);
                cm.CommandType = CommandType.Text;
                sqlDa.Fill(dt);
                ddl.DataSource = dt;
                ddl.DataTextField = "Name";
                ddl.DataValueField = "id";
                ddl.DataBind();
                ddl.Items.Insert(0, new ListItem("--SELECT COMPANY--", "0"));

            }
        }
    }

    public static void FillTestHeadForCGHH(DropDownList ddl,int typ)
    {
        using (SqlConnection conn = new SqlConnection(Globals.connectionString))
        {
            DataTable dt = new DataTable();
            using (SqlCommand cm = new SqlCommand("select * from investigationheads where isdeleted=0 ", conn))
            {
                SqlDataAdapter sqlDa = new SqlDataAdapter(cm);
                cm.CommandType = CommandType.Text;
                sqlDa.Fill(dt);
                ddl.DataSource = dt;
                ddl.DataTextField = "InvestigationHead";
                ddl.DataValueField = "InvestigationheadID";
                ddl.DataBind();
                if (typ == 1)
                {
                    foreach (ListItem item in ddl.Items)
                    {
                        if (item.Text.Equals("CARDIAC") || item.Text.Equals("PATHOLOGY") || item.Text.Equals("RADIOLOGY"))
                        {

                        }
                        else
                        {
                            item.Enabled = false;
                        }
                    }
                }
                else
                {
                    foreach (ListItem item in ddl.Items)
                    {
                        if (item.Text.Equals("CARDIAC") || item.Text.Equals("PATHOLOGY") || item.Text.Equals("RADIOLOGY"))
                        {
                            item.Enabled = false;
                        }
                    }
                }
                ddl.Items.Insert(0, new ListItem("--SELECT HEAD--", "0"));

            }
        }
    }
    public static void FillStaffType(DropDownList ddl)
    {
        ddl.DataSource = obj.SP_GETSINGLEVALUES("STAFF TYPE");
        ddl.DataValueField = "SVID";
        ddl.DataTextField = "VALUE";
        ddl.DataBind();
        ddl.Items.Insert(0, new ListItem("--SELECT--", "0"));
    }
    
    public static void FillSTREAM(DropDownList ddl)
    {
        ddl.DataSource = obj.SP_GETSINGLEVALUES("STREAM");
        ddl.DataValueField = "SVID";
        ddl.DataTextField = "VALUE";
        ddl.DataBind();
        ddl.Items.Insert(0, new ListItem("NA", "0"));
    }
    public static void Fill12thStream(DropDownList ddl)
    {
        ddl.DataSource = obj.SP_GETSINGLEVALUES("12TH STREAM");
        ddl.DataValueField = "SVID";
        ddl.DataTextField = "VALUE";
        ddl.DataBind();
        ddl.Items.Insert(0, new ListItem("--SELECT--", "0"));
    }
    public static void FillAdmissionStatus(DropDownList ddl)
    {
        ddl.DataSource = obj.SP_GETSINGLEVALUES("STATUS");
        ddl.DataValueField = "SVID";
        ddl.DataTextField = "VALUE";
        ddl.DataBind();
        ddl.Items.Insert(0, new ListItem("--SELECT--", "0"));
    }
    public static void FillGraduationStream(DropDownList ddl)
    {
        ddl.DataSource = obj.SP_GETSINGLEVALUES("GRADUATION STREAM");
        ddl.DataValueField = "SVID";
        ddl.DataTextField = "VALUE";
        ddl.DataBind();
        ddl.Items.Insert(0, new ListItem("--SELECT--", "0"));
    }
    public static void FillPostGraduationStream(DropDownList ddl)
    {
        ddl.DataSource = obj.SP_GETSINGLEVALUES("POST GRADUATION STREAM");
        ddl.DataValueField = "SVID";
        ddl.DataTextField = "VALUE";
        ddl.DataBind();
        ddl.Items.Insert(0, new ListItem("--SELECT--", "0"));
    }
    public static void FillCast(DropDownList ddl)
    {
        ddl.DataSource = obj.SP_GETSINGLEVALUES("CAST");
        ddl.DataValueField = "SVID";
        ddl.DataTextField = "VALUE";
        ddl.DataBind();
        ddl.Items.Insert(0, new ListItem("--SELECT--", "0"));
    }
    public static void FillReligion(DropDownList ddl)
    {
        ddl.DataSource = obj.SP_GETSINGLEVALUES("RELIGION");
        ddl.DataValueField = "SVID";
        ddl.DataTextField = "VALUE";
        ddl.DataBind();
        ddl.Items.Insert(0, new ListItem("--SELECT--", "0"));
    }
 public static void FillRegMode(DropDownList ddl)
    {
        ddl.DataSource = obj.SP_GETSINGLEVALUES("REGMODES");
        ddl.DataValueField = "SVID";
        ddl.DataTextField = "VALUE";
        ddl.DataBind();
        ddl.Items.Insert(0, new ListItem("--SELECT--", "0"));
    }


    public static void FillBanks(DropDownList ddl)
    {
        ddl.DataSource = obj.SP_GETSINGLEVALUES("BANKS");
        ddl.DataValueField = "SVID";
        ddl.DataTextField = "VALUE";
        ddl.DataBind();
        ddl.Items.Insert(0, new ListItem("--SELECT--", "0"));
    }


    public static void FillDepartments(DropDownList ddl)
    {
        ddl.DataSource = obj.SP_GETSINGLEVALUES("DEPARTMENTS");
        ddl.DataValueField = "SVID";
        ddl.DataTextField = "VALUE";
        ddl.DataBind();
        ddl.Items.Insert(0, new ListItem("--SELECT--", "0"));
    }

   


    public static void FillDesignations(DropDownList ddl)
    {
        ddl.DataSource = obj.SP_GETSINGLEVALUES("DESIGNATIONS");
        ddl.DataValueField = "SVID";
        ddl.DataTextField = "VALUE";
        ddl.DataBind();
        ddl.Items.Insert(0, new ListItem("--SELECT--", "0"));
    }

    public static void FillWardCategories(DropDownList ddl)
    {
        ddl.DataSource = obj.SP_GETSINGLEVALUES("WARD CATEGORIES");
        ddl.DataValueField = "SVID";
        ddl.DataTextField = "VALUE";
        ddl.DataBind();
        ddl.Items.Insert(0, new ListItem("--SELECT--", "0"));
    }

   

    public static void FillFloors(DropDownList ddl)
    {
        ddl.DataSource = obj.SP_GETSINGLEVALUES("FLOORS");
        ddl.DataValueField = "SVID";
        ddl.DataTextField = "VALUE";
        ddl.DataBind();
        ddl.Items.Insert(0, new ListItem("--SELECT--", "0"));
    }





    public static void FillQualifications(DropDownList ddl)
    {
        ddl.DataSource = obj.SP_GETSINGLEVALUES("QUALIFICATIONS");
        ddl.DataValueField = "SVID";
        ddl.DataTextField = "VALUE";
        ddl.DataBind();
        ddl.Items.Insert(0, new ListItem("--SELECT--", "0"));
    }


    public static void FillOrganisations(DropDownList ddl)
    {
        ddl.DataSource = obj.SP_GETSINGLEVALUES("ORGANISATIONS");
        ddl.DataValueField = "SVID";
        ddl.DataTextField = "VALUE";
        ddl.DataBind();
        ddl.Items.Insert(0, new ListItem("--SELECT--", "0"));
    }
  public static void FillAreas(DropDownList ddl)
    {
        ddl.DataSource = obj.SP_GETSINGLEVALUES("AREAS");
        ddl.DataValueField = "SVID";
        ddl.DataTextField = "VALUE";
        ddl.DataBind();
        ddl.Items.Insert(0, new ListItem("--SELECT AREA--", "0"));
    }



    public static void FillDistrict(DropDownList ddl)
    {
        ddl.DataSource = obj.SP_GETSINGLEVALUES("DISTRICT");
        ddl.DataValueField = "SVID";
        ddl.DataTextField = "VALUE";
        ddl.DataBind();
        ddl.Items.Insert(0, new ListItem("--SELECT DISTRICT--", "0"));
    }
   

    public static DataTable GetOPDsDETAILS(int uhid, string opdsrno, string pateientname, int cond,int actionid)
    {
        DataTable deptset = new DataTable();
        SqlConnection cnn = new SqlConnection(Globals.connectionString);
        SqlCommand cmd = new SqlCommand(); 
        cmd = new SqlCommand("sp_OPDsearch", cnn);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@ACTIONID", actionid);
        cmd.Parameters.AddWithValue("@SERIALNO", opdsrno.Trim());
        cmd.Parameters.AddWithValue("@UHID", uhid);
        cmd.Parameters.AddWithValue("@NAME", pateientname.Trim().ToUpper());
        cmd.Parameters.AddWithValue("@CAREOF", pateientname.Trim().ToUpper());
        SqlDataAdapter dAdapter = new SqlDataAdapter(cmd);
        dAdapter.Fill(deptset);
        return deptset;
    }

    public static void GetHeadSubHeads(DropDownList ddl)
    {
        DataTable deptset = new DataTable();
        SqlConnection cnn = new SqlConnection(Globals.connectionString);
        SqlCommand cmd = new SqlCommand();
        cmd = new SqlCommand("SP_GETHEADS_SUBHEADS_FORREPORT", cnn);
        cmd.CommandType = CommandType.StoredProcedure;
        SqlDataAdapter dAdapter = new SqlDataAdapter(cmd);
        dAdapter.Fill(deptset);
        ddl.DataSource = deptset;
        ddl.DataValueField = "ID";
        ddl.DataTextField = "NAME";
        ddl.DataBind();
        ddl.Items.Insert(0, new ListItem("-ALL--", "0"));

    }

    public static void SP_GET_DISTIBUTION_CONSULTANTID(DropDownList ddl, DateTime from, DateTime to )
    {
        DataTable deptset = new DataTable();
        SqlConnection cnn = new SqlConnection(Globals.connectionString);
        SqlCommand cmd = new SqlCommand();
        cmd = new SqlCommand("SP_GET_DISTIBUTION_CONSULTANTID", cnn);
        cmd.CommandType = CommandType.StoredProcedure;
        SqlDataAdapter dAdapter = new SqlDataAdapter(cmd);
        dAdapter.SelectCommand.Parameters.AddWithValue("@datefrom", Convert.ToDateTime(from));
        dAdapter.SelectCommand.Parameters.AddWithValue("@dateto", Convert.ToDateTime(to ));
        dAdapter.Fill(deptset);
        ddl.DataSource = deptset;
        ddl.DataValueField = "doneby";
        ddl.DataBind();
        ddl.Items.Insert(0, new ListItem("-ALL--", "0"));

    }

    public static void SP_GET_MASTERS_FILL(DropDownList ddl, int id)
    {
        DataTable deptset = new DataTable();
        SqlConnection cnn = new SqlConnection(Globals.connectionString);
        SqlCommand cmd = new SqlCommand();
        cmd = new SqlCommand("SP_GET_MASTERS_FILL", cnn);
        cmd.CommandType = CommandType.StoredProcedure;
        SqlDataAdapter dAdapter = new SqlDataAdapter(cmd);
        dAdapter.SelectCommand.Parameters.AddWithValue("@id", id);
        dAdapter.Fill(deptset);
        ddl.DataSource = deptset;
        ddl.DataValueField = "STID";
        ddl.DataTextField = "TABLENAME";
        ddl.DataBind();
        ddl.Items.Insert(0, new ListItem("-ALL--", "0"));

    }



    public static DataTable SP_GET_MONITORCONSULTANTS()
    {
        DataTable deptset = new DataTable();
        SqlConnection cnn = new SqlConnection(Globals.connectionString);
        SqlCommand cmd = new SqlCommand();
        cmd = new SqlCommand("SP_GET_MONITORCONSULTANTS", cnn);
        cmd.CommandType = CommandType.StoredProcedure;
        SqlDataAdapter dAdapter = new SqlDataAdapter(cmd);
        dAdapter.Fill(deptset);
        return deptset;
    }

    public static DataTable GetIPDsDETAILS(int uhid, string opdsrno, string pateientname, int cond, int actionid)
    {
        DataTable deptset = new DataTable();
        SqlConnection cnn = new SqlConnection(Globals.connectionString);
        SqlCommand cmd = new SqlCommand();
        cmd = new SqlCommand("sp_IPDsearch", cnn);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@ACTIONID", actionid);
        cmd.Parameters.AddWithValue("@SERIALNO", opdsrno.Trim());
        cmd.Parameters.AddWithValue("@UHID", uhid);
        cmd.Parameters.AddWithValue("@NAME", pateientname.Trim().ToUpper());
        cmd.Parameters.AddWithValue("@CAREOF", pateientname.Trim().ToUpper());
        SqlDataAdapter dAdapter = new SqlDataAdapter(cmd);
        dAdapter.Fill(deptset);
        return deptset;
    }


    public static DataTable GetDCTDETAILS(int uhid, string opdsrno, string pateientname, int cond, int actionid)
    {
        DataTable deptset = new DataTable();
        SqlConnection cnn = new SqlConnection(Globals.connectionString);
        SqlCommand cmd = new SqlCommand(); ;
        cmd = new SqlCommand("sp_DCTSEARCH", cnn);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@ACTIONID", actionid);
        cmd.Parameters.AddWithValue("@SERIALNO", opdsrno.Trim());
        cmd.Parameters.AddWithValue("@UHID", uhid);
        cmd.Parameters.AddWithValue("@NAME", pateientname.Trim().ToUpper());
        cmd.Parameters.AddWithValue("@CAREOF", pateientname.Trim().ToUpper());
        SqlDataAdapter dAdapter = new SqlDataAdapter(cmd);
        dAdapter.Fill(deptset);
        return deptset;
    }

    public static DataTable SP_GETREGISTRATIONBYSRNO_OPDIPDDCT(string opdsrno, string pateientname)
    {
        DataTable deptset = new DataTable();
        SqlConnection cnn = new SqlConnection(Globals.connectionString);
        SqlCommand cmd = new SqlCommand(); ;
        cmd = new SqlCommand("SP_GETREGISTRATIONBYSRNO_OPDIPDDCT", cnn);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@SERIALNO", opdsrno.Trim());
        cmd.Parameters.AddWithValue("@patienttype", pateientname);
        SqlDataAdapter dAdapter = new SqlDataAdapter(cmd);
        dAdapter.Fill(deptset);
        return deptset;
    }


    public static DataSet GetDeshBoardDetails(DateTime dt)
    {
        DataSet deptset = new DataSet();
        SqlConnection cnn = new SqlConnection(Globals.connectionString);
        SqlCommand cmd = new SqlCommand();
        cmd = new SqlCommand("SP_GET_DASHBORD_DATA", cnn);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@DATE", dt);
        SqlDataAdapter dAdapter = new SqlDataAdapter(cmd);
        dAdapter.Fill(deptset);
        return deptset;
    }


    public static DataTable SP_RPT_INVESTIGATIONDETAILPATIENTWISE(string serialno)
    {
        DataTable deptset = new DataTable();
        SqlConnection cnn = new SqlConnection(Globals.connectionString);
        SqlCommand cmd = new SqlCommand(); ;
        cmd = new SqlCommand("[SP_RPT_INVESTIGATIONDETAILPATIENTWISE]", cnn);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@SERIALNO", serialno);
        SqlDataAdapter dAdapter = new SqlDataAdapter(cmd);
        dAdapter.Fill(deptset);
        return deptset;
    }

    public static DataTable sp_rpt_billreport( DateTime FROM, DateTime TO , int  CONSULTANTID, int  REFARALID)
    {
        DataTable deptset = new DataTable();
        SqlConnection cnn = new SqlConnection(Globals.connectionString);
        SqlCommand cmd = new SqlCommand(); 
        cmd = new SqlCommand("[sp_rpt_billreport]", cnn);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@datefrom", FROM );
        cmd.Parameters.AddWithValue("@dateto", TO);
        cmd.Parameters.AddWithValue("@CONSULTANTID", CONSULTANTID);
        cmd.Parameters.AddWithValue("@REFARALID", REFARALID);
        SqlDataAdapter dAdapter = new SqlDataAdapter(cmd);
        cmd.CommandTimeout = 500000;
        dAdapter.Fill(deptset);
        return deptset;
    }

    public static DataTable SP_DISTIBUTION_ALL(DateTime FROM, DateTime TO, int CONSULTANTID)
    {
        DataTable deptset = new DataTable();
        SqlConnection cnn = new SqlConnection(Globals.connectionString);
        SqlCommand cmd = new SqlCommand();
        cmd = new SqlCommand("[SP_DISTIBUTION_ALL]", cnn);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@fromdate", FROM);
        cmd.Parameters.AddWithValue("@todate", TO);
        cmd.Parameters.AddWithValue("@cid", CONSULTANTID);
        SqlDataAdapter dAdapter = new SqlDataAdapter(cmd);
        cmd.CommandTimeout = 500000;
        dAdapter.Fill(deptset);
        return deptset;
    }

    public static DataTable SP_GET_JOBCARD(int VEHICLENAME)
    {
        DataTable deptset = new DataTable();
        SqlConnection cnn = new SqlConnection(Globals.connectionString);
        SqlCommand cmd = new SqlCommand();
        cmd = new SqlCommand("SP_GET_JOBCARD", cnn);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@VEHICLENAME", VEHICLENAME);
        SqlDataAdapter dAdapter = new SqlDataAdapter(cmd);
        dAdapter.Fill(deptset);
        return deptset;
    }

    public static DataTable SP_GET_JOBCARDCLIENT(int ACID, int clientId, string R3, int Unit_id)
    {
        DataTable deptset = new DataTable();
        SqlConnection cnn = new SqlConnection(Globals.connectionString);
        SqlCommand cmd = new SqlCommand();
        cmd = new SqlCommand("SP_GET_JOBCARDCLIENT", cnn);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@ACID", ACID);
        cmd.Parameters.AddWithValue("@clientId", clientId);
        cmd.Parameters.AddWithValue("@R3", R3);
        cmd.Parameters.AddWithValue("@Unit_id", Unit_id);
        SqlDataAdapter dAdapter = new SqlDataAdapter(cmd);
        dAdapter.Fill(deptset);
        return deptset;
    }

    public static DataTable SP_RPT_INVESTIGATIONDETAILTESTWISE(int testid,DateTime dtfm,DateTime dtto)
    {
        DataTable deptset = new DataTable();
        SqlConnection cnn = new SqlConnection(Globals.connectionString);
        SqlCommand cmd = new SqlCommand(); ;
        cmd = new SqlCommand("[SP_RPT_INVESTIGATIONDETAILTESTWISE]", cnn);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@TESTID", testid);
        cmd.Parameters.AddWithValue("@DATEFROM", dtfm);
        cmd.Parameters.AddWithValue("@DATETO", dtto);
        SqlDataAdapter dAdapter = new SqlDataAdapter(cmd);
        dAdapter.Fill(deptset);
        return deptset;
    }


    public static DataTable sp_rpt_gas_charges( DateTime dtfm, DateTime dtto)
    {
        DataTable deptset = new DataTable();
        SqlConnection cnn = new SqlConnection(Globals.connectionString);
        SqlCommand cmd = new SqlCommand(); ;
        cmd = new SqlCommand("[sp_rpt_gas_charges]", cnn);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@datefrom", dtfm);
        cmd.Parameters.AddWithValue("@dateto", dtto);
        SqlDataAdapter dAdapter = new SqlDataAdapter(cmd);
        dAdapter.Fill(deptset);
        return deptset;
    }
    public static DataTable SP_SOLShortByTop(int actionid, int unitid)
    {
        DataTable deptset = new DataTable();
        SqlConnection cnn = new SqlConnection(Globals.connectionString);
        SqlCommand cmd = new SqlCommand();
        cmd = new SqlCommand("SP_SOL_ShortByTop", cnn);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@ACTIONID", actionid);
        cmd.Parameters.AddWithValue("@Unit_id", unitid);
        SqlDataAdapter dAdapter = new SqlDataAdapter(cmd);
        dAdapter.Fill(deptset);
        return deptset;
    }
    public static DataSet SP_FillEmployeeList(int acid, int empid)
    {
        DataSet dt = new DataSet();
        using (SqlConnection cnn = new SqlConnection(Globals.connectionString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd = new SqlCommand("SP_FillEmployeeList", cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@acid", acid);
            cmd.Parameters.AddWithValue("@empid",empid);
            SqlDataAdapter dAdapter = new SqlDataAdapter(cmd);
            dAdapter.Fill(dt);
            
        }
        return dt;
    }

    public static DataSet SP_EmployeeSalesplot(int acid, int empid, string type)
    {
        DataSet dt = new DataSet();
        using (SqlConnection cnn = new SqlConnection(Globals.connectionString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd = new SqlCommand("SP_EmployeeSalesplot", cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@acid", acid);
            cmd.Parameters.AddWithValue("@empid", empid);
            cmd.Parameters.AddWithValue("@type", type);
            SqlDataAdapter dAdapter = new SqlDataAdapter(cmd);
            dAdapter.Fill(dt);

        }
        return dt;
    }
    public static string OnlineEmployeeStatus(int Refid)
    {
        string roleName = string.Empty;

        SqlConnection cnn = new SqlConnection(Globals.connectionString);

        StringBuilder qry = new StringBuilder();

        qry.Append("select  top 1 isnull((select F1 from ulog where loginid=l.loginid and  DATEPART(day , chkIn)=DATEPART(day ,getdate()) and DATEPART(MONTH , chkIn)=DATEPART(MONTH ,getdate())and DATEPART(YEAR , chkIn)=DATEPART(YEAR  ,getdate())),'NotLogin')Ulog from Logins l  where l.LoginId=(select LoginId from Logins where Refid=@Refid)");

        SqlCommand cmd = new SqlCommand(qry.ToString(), cnn);

        cmd.Parameters.Add(new SqlParameter("@Refid", SqlDbType.Int));
        cmd.Parameters["@Refid"].Value = Refid;

        cmd.CommandType = CommandType.Text;

        cnn.Open();

        SqlDataReader dReader = cmd.ExecuteReader();

        using (dReader)
        {
            if (dReader.HasRows)
            {
                dReader.Read();
                roleName = dReader[0].ToString().Trim();
            }
        }

        cmd.Dispose();
        cnn.Close();

        return roleName.Trim();
    }

    public static string EnqWalkin()
    {
        string count = "0";

        SqlConnection cnn = new SqlConnection(Globals.connectionString);

        StringBuilder qry = new StringBuilder();

        qry.Append("select count(*) from Enquiries where   DATEPART(MONTH , enquirydate)=DATEPART(MONTH ,getdate())and DATEPART(YEAR , enquirydate)=DATEPART(YEAR  ,getdate())and LastName='Walkin'");

        SqlCommand cmd = new SqlCommand(qry.ToString(), cnn);

        cmd.CommandType = CommandType.Text;

        cnn.Open();

        SqlDataReader dReader = cmd.ExecuteReader();

        using (dReader)
        {
            if (dReader.HasRows)
            {
                dReader.Read();
                count = dReader[0].ToString().Trim();
            }
        }

        cmd.Dispose();
        cnn.Close();

        return count.Trim();
    }
    public static string EnqTelephonic()
    {
        string count = "0";

        SqlConnection cnn = new SqlConnection(Globals.connectionString);

        StringBuilder qry = new StringBuilder();

        qry.Append("select count(*) from Enquiries where   DATEPART(MONTH , enquirydate)=DATEPART(MONTH ,getdate())and DATEPART(YEAR , enquirydate)=DATEPART(YEAR  ,getdate())and LastName='Telephonic'");

        SqlCommand cmd = new SqlCommand(qry.ToString(), cnn);

        cmd.CommandType = CommandType.Text;

        cnn.Open();

        SqlDataReader dReader = cmd.ExecuteReader();

        using (dReader)
        {
            if (dReader.HasRows)
            {
                dReader.Read();
                count = dReader[0].ToString().Trim();
            }
        }

        cmd.Dispose();
        cnn.Close();

        return count.Trim();
    }
    public static string EnqAdvertisement()
    {
        string count = "0";

        SqlConnection cnn = new SqlConnection(Globals.connectionString);

        StringBuilder qry = new StringBuilder();

        qry.Append("select count(*) from Enquiries where   DATEPART(MONTH , enquirydate)=DATEPART(MONTH ,getdate())and DATEPART(YEAR , enquirydate)=DATEPART(YEAR  ,getdate())and LastName='Advertisement'");

        SqlCommand cmd = new SqlCommand(qry.ToString(), cnn);

        cmd.CommandType = CommandType.Text;

        cnn.Open();

        SqlDataReader dReader = cmd.ExecuteReader();

        using (dReader)
        {
            if (dReader.HasRows)
            {
                dReader.Read();
                count = dReader[0].ToString().Trim();
            }
        }

        cmd.Dispose();
        cnn.Close();

        return count.Trim();
    }
    public static string EnqWeb()
    {
        string count = "0";

        SqlConnection cnn = new SqlConnection(Globals.connectionString);

        StringBuilder qry = new StringBuilder();

        qry.Append("select count(*) from Enquiries where   DATEPART(MONTH , enquirydate)=DATEPART(MONTH ,getdate())and DATEPART(YEAR , enquirydate)=DATEPART(YEAR  ,getdate())and LastName='Web'");

        SqlCommand cmd = new SqlCommand(qry.ToString(), cnn);

        cmd.CommandType = CommandType.Text;

        cnn.Open();

        SqlDataReader dReader = cmd.ExecuteReader();

        using (dReader)
        {
            if (dReader.HasRows)
            {
                dReader.Read();
                count = dReader[0].ToString().Trim();
            }
        }

        cmd.Dispose();
        cnn.Close();

        return count.Trim();
    }
    public static string EnqCanopy()
    {
        string count = "0";

        SqlConnection cnn = new SqlConnection(Globals.connectionString);

        StringBuilder qry = new StringBuilder();

        qry.Append("select count(*) from Enquiries where   DATEPART(MONTH , enquirydate)=DATEPART(MONTH ,getdate())and DATEPART(YEAR , enquirydate)=DATEPART(YEAR  ,getdate())and LastName='Canopy promotion'");

        SqlCommand cmd = new SqlCommand(qry.ToString(), cnn);

        cmd.CommandType = CommandType.Text;

        cnn.Open();

        SqlDataReader dReader = cmd.ExecuteReader();

        using (dReader)
        {
            if (dReader.HasRows)
            {
                dReader.Read();
                count = dReader[0].ToString().Trim();
            }
        }

        cmd.Dispose();
        cnn.Close();

        return count.Trim();
    }
     
    public static string EmpDesgination( int id)
    {
        string count = " ";

        SqlConnection cnn = new SqlConnection(Globals.connectionString);

        StringBuilder qry = new StringBuilder();

        qry.Append("select Designation from Designations where DesignId='"+id+"'");

        SqlCommand cmd = new SqlCommand(qry.ToString(), cnn);

        cmd.CommandType = CommandType.Text;

        cnn.Open();

        SqlDataReader dReader = cmd.ExecuteReader();

        using (dReader)
        {
            if (dReader.HasRows)
            {
                dReader.Read();
                count = dReader[0].ToString().Trim();
            }
        }

        cmd.Dispose();
        cnn.Close();

        return count.Trim();
    }
    public static void FillClassList(DropDownList ddl)
    {
        var DATA = from Cons in obj.Courses 
                   where Cons.Remove == false 
                   select Cons;
        ddl.DataSource = DATA;
        ddl.DataTextField = "CourseName";
        ddl.DataValueField = "CourseId";
        ddl.DataBind();
        ddl.Items.Insert(0, "Select Class");
        ddl.Focus();
 
   }
    public static void FillSINGLEVALUEDATA(DropDownList ddl, string Head)
    {
        ddl.DataSource = obj.SP_GETSINGLEVALUES(Head);
        ddl.DataValueField = "SVID";
        ddl.DataTextField = "VALUE";
        ddl.DataBind();
        ddl.Items.Insert(0, new ListItem("--SELECT--", "0"));
    }
    public static void FillClassList11th12th(DropDownList ddl)
    {
        var DATA = from Cons in obj.Courses
                   where Cons.Remove == false && Cons.CourseName == "11TH" || Cons.CourseName == "12TH"
                   select Cons;
        ddl.DataSource = DATA;
        ddl.DataTextField = "CourseName";
        ddl.DataValueField = "CourseId";
        ddl.DataBind();
        ddl.Items.Insert(0, "Select Class");
        ddl.Focus();

    }
}