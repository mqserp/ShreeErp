﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;


public partial class Account_Transactions_ADD_RecieptEntry : System.Web.UI.Page
{
    #region General Declaration
    RecieptEntry_BL BL_RecieptEntry = null;
    double PedQty = 0;


    static int mlCode = 0;
    static string right = "";
    static string redirect = "~/Account/Transactions/VIEW/ViewRecieptEntry.aspx";
    static string ItemUpdateIndex = "-1";
    static DataTable dt2 = new DataTable();
    DataTable dtFilter = new DataTable();
    public static string str = "";
    public static int Index = 0;
    DataTable dt = new DataTable();
    DataTable dtPO = new DataTable();
    DataRow dr;
    DataTable dtInwardDetail = new DataTable();
    #endregion

    public string Msg = "";

    #region Events

    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty((string)Session["CompanyId"]) && string.IsNullOrEmpty((string)Session["Username"]))
        {
            Response.Redirect("~/Default.aspx", false);
        }
        else
        {
            if (!IsPostBack)
            {
                ViewState["mlCode"] = mlCode;
                ViewState["Index"] = Index;
                ViewState["ItemUpdateIndex"] = ItemUpdateIndex;
                ViewState["dt2"] = dt2;
                ((DataTable)ViewState["dt2"]).Rows.Clear();
                str = "";
                ViewState["str"] = str;

                DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='132'");
                right = dtRights.Rows.Count == 0 ? "00000000" : dtRights.Rows[0][0].ToString();
                try
                {
                    if (Request.QueryString[0].Equals("VIEW"))
                    {
                        BL_RecieptEntry = new RecieptEntry_BL();
                        ViewState["mlCode"] = Convert.ToInt32(Request.QueryString[1].ToString());

                        ViewRec("VIEW");
                        CtlDisable();
                    }
                    else if (Request.QueryString[0].Equals("MODIFY"))
                    {

                        BL_RecieptEntry = new RecieptEntry_BL();
                        ViewState["mlCode"] = Convert.ToInt32(Request.QueryString[1].ToString());
                        ViewRec("MOD");
                    }
                    else if (Request.QueryString[0].Equals("INSERT"))
                    {
                        //txtRCPDate.Text = System.DateTime.Now.ToString("dd MMM yyyy");
                        //txtChequeDate.Text = System.DateTime.Now.ToString("dd MMM yyyy");
                        txtRCPDate.Text = CommonClasses.GetCurrentTime().ToString("dd MMM yyyy");
                        txtChequeDate.Text = CommonClasses.GetCurrentTime().ToString("dd MMM yyyy");

                        BlankGrid();
                        LoadCombos();
                        //LoadCurr();
                        dtFilter.Rows.Clear();

                        //txtRCPDate.Attributes.Add("readonly", "readonly");
                        //txtChequeDate.Attributes.Add("readonly", "readonly");

                    }
                    ddlCustomer.Focus();
                }
                catch (Exception ex)
                {
                    CommonClasses.SendError("Reciept Entry", "PageLoad", ex.Message);
                }
            }

        }
    }

    private void BlankGrid()
    {
        dtFilter.Clear();
        if (dtFilter.Columns.Count == 0)
        {
            dgInwardMaster.Enabled = false;
            dtFilter.Columns.Add(new System.Data.DataColumn("RCPD_REF_CODE", typeof(string)));
            dtFilter.Columns.Add(new System.Data.DataColumn("REF_DESC", typeof(string)));
            dtFilter.Columns.Add(new System.Data.DataColumn("RCPD_INVOICE_CODE", typeof(string)));
            dtFilter.Columns.Add(new System.Data.DataColumn("RCPD_INVOICE_CODE_TEMP", typeof(string)));
            dtFilter.Columns.Add(new System.Data.DataColumn("INVOICE_NO", typeof(string)));

            dtFilter.Columns.Add(new System.Data.DataColumn("RCPD_AMOUNT", typeof(string)));
            dtFilter.Columns.Add(new System.Data.DataColumn("RCPD_ADJ_AMOUNT", typeof(string)));

            dtFilter.Columns.Add(new System.Data.DataColumn("RCPD_REMARK", typeof(string)));

            dtFilter.Columns.Add(new System.Data.DataColumn("RCPD_TYPE", typeof(string)));


            dtFilter.Rows.Add(dtFilter.NewRow());
            dgInwardMaster.DataSource = dtFilter;
            dgInwardMaster.DataBind();
            ((DataTable)ViewState["dt2"]).Clear();
        }
    }
    #endregion

    #region LoadCombos
    private void LoadCombos()
    {
        #region FillCustomer
        try
        {
            DataTable dtParty = new DataTable();
            //  dt = CommonClasses.FillCombo("PARTY_MASTER,INVOICE_MASTER", "P_NAME", "P_CODE", "INVOICE_MASTER.ES_DELETE=0 AND PARTY_MASTER.ES_DELETE=0 and P_CM_COMP_ID=" + (string)Session["CompanyId"] + " and P_TYPE='2' and P_ACTIVE_IND=1 and RCP_P_CODE=P_CODE  ORDER BY P_NAME", ddlCustomer);
            if (Convert.ToInt32(ViewState["mlCode"].ToString()) == 0)
            {
                dt = CommonClasses.Execute("SELECT DISTINCT P_CODE,P_NAME FROM PARTY_MASTER,INVOICE_MASTER WHERE PARTY_MASTER.ES_DELETE=0 AND INM_P_CODE=P_CODE  AND INVOICE_MASTER.ES_DELETE=0  AND  P_CM_COMP_ID='" + (string)Session["CompanyId"] + "'   and P_ACTIVE_IND=1 UNION SELECT DISTINCT P_CODE,P_NAME FROM PARTY_MASTER,CREDIT_NOTE_MASTER WHERE PARTY_MASTER.ES_DELETE=0 AND CNM_CUST_CODE=P_CODE  AND CREDIT_NOTE_MASTER.ES_DELETE=0    AND P_CM_COMP_ID='" + (string)Session["CompanyId"] + "'   and P_ACTIVE_IND=1 UNION SELECT DISTINCT P_CODE,P_NAME FROM PARTY_MASTER,DEBIT_NOTE_MASTER WHERE PARTY_MASTER.ES_DELETE=0 AND DNM_CUST_CODE=P_CODE  AND DEBIT_NOTE_MASTER.ES_DELETE=0 AND  P_CM_COMP_ID='" + (string)Session["CompanyId"] + "'     and P_ACTIVE_IND=1 ORDER BY P_NAME ");
            }
            else
            {
                dt = CommonClasses.Execute("SELECT DISTINCT P_CODE,P_NAME FROM PARTY_MASTER,INVOICE_MASTER WHERE PARTY_MASTER.ES_DELETE=0 AND INM_P_CODE=P_CODE AND INVOICE_MASTER.ES_DELETE=0 AND   P_CM_COMP_ID='" + (string)Session["CompanyId"] + "'  and P_ACTIVE_IND=1   UNION SELECT DISTINCT P_CODE,P_NAME FROM PARTY_MASTER,CREDIT_NOTE_MASTER WHERE PARTY_MASTER.ES_DELETE=0 AND CNM_CUST_CODE=P_CODE  AND CREDIT_NOTE_MASTER.ES_DELETE=0    AND P_CM_COMP_ID='" + (string)Session["CompanyId"] + "'   and P_ACTIVE_IND=1 UNION SELECT DISTINCT P_CODE,P_NAME FROM PARTY_MASTER,DEBIT_NOTE_MASTER WHERE PARTY_MASTER.ES_DELETE=0 AND DNM_CUST_CODE=P_CODE  AND DEBIT_NOTE_MASTER.ES_DELETE=0  AND P_CM_COMP_ID='" + (string)Session["CompanyId"] + "'     and P_ACTIVE_IND=1 ORDER BY P_NAME ");
            }
            ddlCustomer.DataSource = dt;
            ddlCustomer.DataTextField = "P_NAME";
            ddlCustomer.DataValueField = "P_CODE";
            ddlCustomer.DataBind();
            ddlCustomer.Items.Insert(0, new ListItem("Please Select Customer ", "0"));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Reciept Entry", "LoadCombos", Ex.Message);
        }
        #endregion

        #region ACCOUNTLEDGER
        try
        {

            dt = CommonClasses.FillCombo("ACCOUNT_LEDGER", "L_NAME", "L_CODE", "ACCOUNT_LEDGER.ES_DELETE=0  and L_CM_ID=" + (string)Session["CompanyId"] + "  ORDER BY L_NAME", ddlLedger);
            ddlLedger.Items.Insert(0, new ListItem("Please Select Ledger ", "0"));




        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Reciept Entry", "LoadCombos", Ex.Message);
        }
        #endregion





    }
    #endregion

    #region btnSubmit_Click
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(1, 1)), this, "For Save"))
            {
                if (((Convert.ToDateTime(Session["OpeningDate"]) <= (Convert.ToDateTime(txtRCPDate.Text))) && (Convert.ToDateTime(Session["ClosingDate"]) >= Convert.ToDateTime(txtRCPDate.Text))) && ((Convert.ToDateTime(Session["OpeningDate"]) <= (Convert.ToDateTime(txtChequeDate.Text))) && (Convert.ToDateTime(Session["ClosingDate"]) >= Convert.ToDateTime(txtChequeDate.Text))))
                {
                    if (dgInwardMaster.Rows.Count > 0 && dgInwardMaster.Enabled)
                    {
                        #region Validate_Amount_With_Total_Amt
                        double Amount = 0, Total_Amount = 0;
                        if (txtAmount.Text != null || txtAmount.Text.Trim() != "0")
                            Amount = Math.Round(Convert.ToDouble(txtAmount.Text), 2);
                        for (int i = 0; i < dgInwardMaster.Rows.Count; i++)
                        {
                            //Total_Amount = Total_Amount + Convert.ToDouble(((Label)dgInwardMaster.Rows[i].FindControl("lblRCPD_AMOUNT")).Text) + Convert.ToDouble(((Label)dgInwardMaster.Rows[i].FindControl("lblRCPD_ADJ_AMOUNT")).Text); ;
                            Total_Amount = Math.Round(Total_Amount, 2) + Math.Round(Convert.ToDouble(((Label)dgInwardMaster.Rows[i].FindControl("lblRCPD_AMOUNT")).Text), 2);
                        }
                        if (Math.Round(Amount, 2) != Math.Round(Total_Amount, 2))
                        {
                            PanelMsg.Visible = true;
                            lblmsg.Text = "Detail Total is Not Match with Total Amount";
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                            txtAmount.Focus();
                            return;
                        }
                        else
                        {
                            SaveRec();
                        }
                        #endregion Validate_Amount_With_Total_Amt
                    }
                    else
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Please Insert Invoice ";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        ddlInvoiceNo.Focus();
                        return;
                    }
                }
                else
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Please Select Current Year Date ";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    ddlInvoiceNo.Focus();
                    return;
                }
            }
        }
        catch (Exception Ex)
        {

            CommonClasses.SendError("Reciept Entry", "btnSubmit_Click", Ex.Message);
        }
    }
    #endregion


    #endregion

    #region ViewRec
    private void ViewRec(string str)
    {
        DataTable dt = new DataTable();
        try
        {
            txtChequeDate.Attributes.Add("readonly", "readonly");
            txtRCPDate.Attributes.Add("readonly", "readonly");


            LoadCombos();
            dt = CommonClasses.Execute(" SELECT RCP_CODE,RCP_NO,RCP_DATE,RCP_P_CODE,RCP_CHEQUE_NO,RCP_CHEQUE_DATE,RCP_AMOUNT,RCP_LEDGER_CODE,RCP_REMARK,RCP_CM_CODE FROM RECIEPT_MASTER WHERE RECIEPT_MASTER.ES_DELETE = 0 AND RCP_CM_CODE=" + Convert.ToInt32(Session["CompanyCode"]) + " and RCP_CODE=" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "");

            if (dt.Rows.Count > 0)
            {
                ViewState["mlCode"] = Convert.ToInt32(dt.Rows[0]["RCP_CODE"]);
                txtRCPno.Text = Convert.ToInt32(dt.Rows[0]["RCP_NO"]).ToString();
                txtRCPDate.Text = Convert.ToDateTime(dt.Rows[0]["RCP_DATE"]).ToString("dd MMM yyyy");
                ddlCustomer.SelectedValue = Convert.ToInt32(dt.Rows[0]["RCP_P_CODE"]).ToString();
                txtChequeNo.Text = (dt.Rows[0]["RCP_CHEQUE_NO"]).ToString();
                txtChequeDate.Text = Convert.ToDateTime(dt.Rows[0]["RCP_CHEQUE_DATE"]).ToString("dd MMM yyyy");

                txtAmount.Text = (dt.Rows[0]["RCP_AMOUNT"]).ToString();
                ddlLedger.SelectedValue = dt.Rows[0]["RCP_LEDGER_CODE"].ToString();

                ddlInvoiceNo.Items.Clear();
                int id = Convert.ToInt32(ddlCustomer.SelectedValue);

                ddlInvoiceNo.Items.Clear();
                ddlRefernce.Items.Clear();
                DataTable dtItem = new DataTable();
                dtItem = CommonClasses.Execute("declare @temp table(INM_CODE int IDENTITY (1,1),INVOICE_CODE int,INM_NO nvarchar(500),INM_TYPE int)insert into @temp SELECT INM_CODE,CONCAT(INM_NO,'-','INV') as INM_NO,1 AS INM_TYPE FROM INVOICE_MASTER WHERE INVOICE_MASTER.ES_DELETE=0 and INM_P_CODE='" + ddlCustomer.SelectedValue + "'  and INM_CM_CODE='" + (string)Session["CompanyCode"] + "'  UNION SELECT CNM_CODE,CONCAT(CNM_SERIAL_NO,'-','CN'),0 AS INM_TYPE FROM CREDIT_NOTE_MASTER WHERE CREDIT_NOTE_MASTER.ES_DELETE=0 and CNM_CUST_CODE='" + ddlCustomer.SelectedValue + "'   and CNM_CM_CODE='" + (string)Session["CompanyCode"] + "'  UNION SELECT DNM_CODE,CONCAT(DNM_SERIAL_NO,'-','DN'),2 AS INM_TYPE  FROM DEBIT_NOTE_MASTER WHERE DEBIT_NOTE_MASTER.ES_DELETE=0 and DNM_CUST_CODE='" + ddlCustomer.SelectedValue + "'   and DNM_CM_CODE='" + (string)Session["CompanyCode"] + "' select * from @temp ");
                ddlInvoiceNo.DataSource = dtItem;
                ddlInvoiceNo.DataTextField = "INM_NO";
                ddlInvoiceNo.DataValueField = "INM_CODE";
                ddlInvoiceNo.DataBind();
                ddlInvoiceNo.Items.Insert(0, new ListItem("Please Select Invoice No ", "0"));



                CommonClasses.FillCombo("REFERENCE_MASTER", "REF_DESC", "REF_CODE", " ES_DELETE=0 and REF_CM_ID=" + (string)Session["CompanyId"], ddlRefernce);
                // ddlInvoiceNo.Items.Insert(0, new ListItem("Please Select Invoice No ", "0"));
                ddlRefernce.Items.Insert(0, new ListItem("Reference Type ", "0"));


                dtInwardDetail = CommonClasses.Execute("SELECT identity(int,1,1) as RN,RECIEPT_DETAIL.RCPD_REF_CODE, REFERENCE_MASTER.REF_DESC, RECIEPT_DETAIL.RCPD_INVOICE_CODE, RECIEPT_DETAIL.RCPD_INVOICE_CODE_TEMP,case RCPD_TYPE when 0 then CONCAT(INM_NO,'-','CN') when 1 then CONCAT(INM_NO,'-','INV') when 2 then CONCAT(INM_NO,'-','DN') end as INVOICE_NO, RECIEPT_DETAIL.RCPD_AMOUNT,RECIEPT_DETAIL.RCPD_ADJ_AMOUNT, RECIEPT_DETAIL.RCPD_REMARK, RECIEPT_DETAIL.RCPD_TYPE into #Temp FROM     RECIEPT_MASTER INNER JOIN RECIEPT_DETAIL ON RECIEPT_MASTER.RCP_CODE = RECIEPT_DETAIL.RCPD_RCP_CODE INNER JOIN REFERENCE_MASTER ON RECIEPT_DETAIL.RCPD_REF_CODE = REFERENCE_MASTER.REF_CODE LEFT OUTER JOIN INVOICE_MASTER ON RECIEPT_DETAIL.RCPD_INVOICE_CODE = INVOICE_MASTER.INM_CODE where  RCP_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "' and RCPD_RCP_CODE='" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "' declare @temp table(id int identity(1,1),newrefkey int,Ttype int) declare @cnt int=0,@i int=1,@newreferncecode int=0,@newreferncename varchar(5000) insert into @temp select RCPD_INVOICE_CODE,RCPD_TYPE from #Temp where RCPD_TYPE=7  select @cnt=count(*) from @temp where Ttype=7   while(@cnt>=@i) begin   select @newreferncecode=newrefkey from @Temp where id=@i select @newreferncename=REF_NAME from NEWREFERENCE_MASTER where REF_CODE=@newreferncecode   update #Temp set INVOICE_NO=@newreferncename where RCPD_INVOICE_CODE=@newreferncecode and RCPD_TYPE=7   set @i=@i+1  end   select * from #Temp   drop table #Temp");

                if (dtInwardDetail.Rows.Count != 0)
                {
                    dgInwardMaster.DataSource = dtInwardDetail;
                    dgInwardMaster.DataBind();
                    ViewState["dt2"] = dtInwardDetail;
                    calcAmt();
                }
            }

            if (str == "VIEW")
            {
                ddlCustomer.Enabled = false;

                ddlInvoiceNo.Enabled = false;
                ddlRefernce.Enabled = false;

                txtRecievedAmount.Enabled = false;
                txtAdjsAmt.Enabled = false;
                txtRemark.Enabled = false;
                dgInwardMaster.Enabled = false;

            }

            if (str == "MOD")
            {
                ddlCustomer.Enabled = false;
                CommonClasses.SetModifyLock("RECIEPT_MASTER", "MODIFY", "RCP_CODE", Convert.ToInt32(ViewState["mlCode"].ToString()));
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Reciept Entry", "ViewRec", Ex.Message);
        }
    }
    #endregion

    //#region LoadCurr
    //private void LoadCurr()
    //{
    //    try
    //    {
    //        DataTable dt = CommonClasses.Execute("SELECT CURR_CODE,CURR_NAME FROM CURRANCY_MASTER WHERE ES_DELETE = 0 AND CURR_CM_COMP_ID = " + (string)Session["CompanyId"] + "  ORDER BY CURR_NAME");
    //        ddlLedger.DataSource = dt;
    //        ddlLedger.DataTextField = "CURR_NAME";
    //        ddlLedger.DataValueField = "CURR_CODE";
    //        ddlLedger.DataBind();
    //        ddlLedger.Items.Insert(0, new ListItem("Select Ledger", "0"));
    //        ddlLedger.SelectedIndex = -1;
    //    }
    //    catch (Exception Ex)
    //    {
    //        CommonClasses.SendError("Export PO", "LoadIName", Ex.Message);
    //    }
    //}
    //#endregion

    #region GetValues
    public bool GetValues(string str)
    {
        bool res = false;
        try
        {
            if (str == "VIEW")
            {
                ddlCustomer.SelectedValue = BL_RecieptEntry.RCP_P_CODE.ToString();
                txtRCPno.Text = BL_RecieptEntry.RCP_NO.ToString();
                txtChequeNo.Text = BL_RecieptEntry.RCP_CHEQUE_NO.ToString();

                txtRCPDate.Text = BL_RecieptEntry.RCP_DATE.ToString("dd MMM YYYY");
                txtChequeDate.Text = BL_RecieptEntry.RCP_CHEQUE_DATE.ToString("dd MMM YYYY");
                ddlLedger.SelectedValue = BL_RecieptEntry.RCP_LEDGER_CODE.ToString();
                txtAmount.Text = BL_RecieptEntry.RCP_AMOUNT.ToString();
            }
            else if (str == "VIEW")
            {
                ddlCustomer.Enabled = false;
                txtRCPno.Enabled = false;
                txtChequeNo.Enabled = false;

                txtChequeDate.Enabled = false;
                txtRCPDate.Enabled = false;
                BtnInsert.Visible = false;
                btnSubmit.Visible = false;
                ddlLedger.Enabled = false;
                txtAmount.Enabled = false;
            }
            res = true;
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Reciept Entry", "GetValues", ex.Message);
        }
        return res;
    }
    #endregion

    #region Setvalues
    public bool Setvalues()
    {
        bool res = false;
        try
        {
            BL_RecieptEntry.RCP_P_CODE = Convert.ToInt32(ddlCustomer.SelectedValue);
            //BL_RecieptEntry.RCP_SITE_CODE = Convert.ToInt32(ddlSiteName.SelectedValue);
            BL_RecieptEntry.RCP_NO = Convert.ToInt32(txtRCPno.Text);
            BL_RecieptEntry.RCP_CHEQUE_NO = txtChequeNo.Text.ToString();


            BL_RecieptEntry.RCP_CHEQUE_DATE = Convert.ToDateTime(txtChequeDate.Text);
            BL_RecieptEntry.RCP_DATE = Convert.ToDateTime(txtRCPDate.Text);
            BL_RecieptEntry.RCP_LEDGER_CODE = Convert.ToInt32(ddlLedger.SelectedValue);
            BL_RecieptEntry.RCP_AMOUNT = float.Parse(txtAmount.Text);
            BL_RecieptEntry.RCP_REMARK = txtRemark.Text;
            BL_RecieptEntry.RCP_CM_CODE = Convert.ToInt32(Session["CompanyCode"]);



            res = true;
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Reciept Entry", "Setvalues", ex.Message);
        }
        return res;
    }
    #endregion

    #region SaveRec
    bool SaveRec()
    {
        bool result = false;
        try
        {
            #region INSERT
            if (Request.QueryString[0].Equals("INSERT"))
            {
                BL_RecieptEntry = new RecieptEntry_BL();
                txtRCPno.Text = Numbering();
                if (Setvalues())
                {
                    if (BL_RecieptEntry.Save(dgInwardMaster))
                    {
                        string Code = CommonClasses.GetMaxId("Select Max(RCP_CODE) from RECIEPT_MASTER");
                        CommonClasses.WriteLog("Reciept Entry ", "Insert", "Reciept Entry", Code, Convert.ToInt32(Code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect(redirect, false);
                    }
                    else
                    {
                        if (BL_RecieptEntry.Msg != "")
                        {
                            ShowMessage("#Avisos", BL_RecieptEntry.Msg.ToString(), CommonClasses.MSG_Warning);
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                            BL_RecieptEntry.Msg = "";
                        }
                        ddlCustomer.Focus();
                    }
                }
            }
            #endregion

            #region MODIFY
            else if (Request.QueryString[0].Equals("MODIFY"))
            {
                BL_RecieptEntry = new RecieptEntry_BL(Convert.ToInt32(ViewState["mlCode"].ToString()));

                if (Setvalues())
                {
                    if (BL_RecieptEntry.Update(dgInwardMaster))
                    {
                        CommonClasses.RemoveModifyLock("RECIEPT_MASTER", "MODIFY", "RCP_CODE", Convert.ToInt32(ViewState["mlCode"].ToString()));
                        // CommonClasses.WriteLog("Reciept Entry ", "Update", "Reciept Entry",  Convert.ToInt32(ViewState["mlCode"].ToString()) , Convert.ToInt32(mlCode), Convert.ToInt32(Session["CompanyId"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        CommonClasses.WriteLog("Reciept Entry ", "Update", "Reciept Entry", Convert.ToInt32(ViewState["mlCode"].ToString()).ToString(), Convert.ToInt32(ViewState["mlCode"].ToString()), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        result = true;
                        Response.Redirect(redirect, false);
                    }
                    else
                    {
                        if (BL_RecieptEntry.Msg != "")
                        {
                            ShowMessage("#Avisos", BL_RecieptEntry.Msg.ToString(), CommonClasses.MSG_Warning);
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                            BL_RecieptEntry.Msg = "";
                        }
                        ddlCustomer.Focus();
                    }
                }
            }
            #endregion
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Reciept Entry", "SaveRec", ex.Message);
        }
        return result;
    }
    #endregion

    #region ShowMessage
    public bool ShowMessage(string DiveName, string Message, string MessageType)
    {
        try
        {
            if ((!ClientScript.IsStartupScriptRegistered("regMostrarMensagem")))
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "regMostrarMensagem", "MostrarMensagem('" + DiveName + "', '" + Message + "', '" + MessageType + "');", true);
            }
            return true;
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Reciept Entry", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region Numbering
    string Numbering()
    {

        int GenGINNO;
        DataTable dt = new DataTable();
        dt = CommonClasses.Execute("select max(RCP_NO) as RCP_NO from RECIEPT_MASTER where RCP_CM_CODE='" + Session["CompanyCode"] + "'  and ES_DELETE=0");
        if (dt.Rows[0]["RCP_NO"] == null || dt.Rows[0]["RCP_NO"].ToString() == "")
        {
            GenGINNO = 1;
        }
        else
        {
            GenGINNO = Convert.ToInt32(dt.Rows[0]["RCP_NO"]) + 1;
        }
        return GenGINNO.ToString();
    }
    #endregion

    protected void dgInwardMaster_Deleting(object sender, GridViewDeleteEventArgs e)
    {
    }
    #region dgInwardMaster_RowCommand
    protected void dgInwardMaster_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            ViewState["Index"] = Convert.ToInt32(e.CommandArgument.ToString());
            GridViewRow row = dgInwardMaster.Rows[Convert.ToInt32(ViewState["Index"].ToString())];


            if (e.CommandName == "Delete")
            {

                int rowindex = row.RowIndex;
                dgInwardMaster.DeleteRow(rowindex);
                ((DataTable)ViewState["dt2"]).Rows.RemoveAt(rowindex);
                dgInwardMaster.DataSource = ((DataTable)ViewState["dt2"]);
                dgInwardMaster.DataBind();
                if (dgInwardMaster.Rows.Count == 0)
                    BlankGrid();
                // clearDetail();
                calcAmt();
            }
            if (e.CommandName == "Select")
            {
                if (((Label)(row.FindControl("lblRCPD_REF_CODE"))).Text == "-2147483647")
                {
                    ddlInvoiceNo.Items.Clear();
                    txtRemaningamt.Text = "";
                    txtRecievedAmount.Text = "";
                    txtAdjsAmt.Text = "";
                    DataTable dtItem = new DataTable();
                    dtItem = CommonClasses.Execute("select * from NEWREFERENCE_MASTER where ES_DELETE=0   and REF_CM_COMP_ID='" + (string)Session["CompanyID"] + "'");
                    ddlInvoiceNo.DataSource = dtItem;
                    ddlInvoiceNo.DataTextField = "REF_NAME";
                    ddlInvoiceNo.DataValueField = "REF_CODE";
                    ddlInvoiceNo.DataBind();
                    ddlInvoiceNo.Items.Insert(0, new ListItem("Please Select Reference ", "0"));
                }
                else
                {
                    FillInvoiceNo();
                }


                LinkButton lnkDelete = (LinkButton)(row.FindControl("lnkDelete"));
                lnkDelete.Enabled = false;

                //dgInwardMaster.Columns[1].Visible = false;

                ViewState["str"] = "Modify";
                ViewState["ItemUpdateIndex"] = e.CommandArgument.ToString();
                //LoadICode();
                //LoadIName();
                string s = ((Label)(row.FindControl("lblRCPD_INVOICE_CODE"))).Text;
                ddlRefernce.SelectedValue = ((Label)(row.FindControl("lblRCPD_REF_CODE"))).Text;
                ddlInvoiceNo.SelectedValue = ((Label)(row.FindControl("lblRCPD_INVOICE_CODE_TEMP"))).Text;
                ddlRefernce_SelectedIndexChanged(null, null);

                txtAdjsAmt.Text = ((Label)(row.FindControl("lblRCPD_ADJ_AMOUNT"))).Text;
                if (txtAdjsAmt.Text == "")
                {
                    txtAdjsAmt.Text = "0";
                }

                //txtRate.Text = ((Label)(row.FindControl("lblSPOD_RATE"))).Text;
                txtRecievedAmount.Text = ((Label)(row.FindControl("lblRCPD_AMOUNT"))).Text;
                txtRemark.Text = ((Label)(row.FindControl("lblRCPD_REMARK"))).Text;
                txtRemaningamt.Text = (Convert.ToDouble(((Label)(row.FindControl("lblRCPD_AMOUNT"))).Text) + Convert.ToDouble(txtAdjsAmt.Text)).ToString();


            }

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Reciept Entry", "dgInwardMaster_RowCommand", Ex.Message);
        }
    }
    #endregion

    #region clearDetail
    private void clearDetail()
    {
        try
        {
            ddlInvoiceNo.SelectedValue = "0";
            ddlRefernce.SelectedValue = "0";
            ddlCustomer.Items.Insert(0, new ListItem("Please Select PO ", "0"));
            txtRecievedAmount.Text = "0.00";
            txtAdjsAmt.Text = "0.00";
            ViewState["str"] = "";
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError(" Reciept Entry ", "clearDetail", Ex.Message);
        }
    }
    #endregion

    #region btnInsert_Click
    protected void BtnInsert_Click(object sender, EventArgs e)
    {
        try
        {
            PanelMsg.Visible = false;

            if (Convert.ToInt32(ddlCustomer.SelectedValue) == 0)
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Select Customer Name";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                ddlRefernce.Focus();
                return;
            }

            if (txtChequeNo.Text.Trim() == "")
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Cheque No Is Required";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                ddlRefernce.Focus();
                return;
            }

            if (txtAmount.Text == "" || Convert.ToDecimal(txtAmount.Text) <= 0)
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Enter Cheque Amount";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtAmount.Focus();
                return;
            }
            if (Convert.ToInt32(ddlLedger.SelectedIndex) == 0)
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Select Ledger Head";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                ddlLedger.Focus();
                return;
            }
            if (Convert.ToInt32(ddlRefernce.SelectedIndex) == 0)
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Select Reference No";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                ddlRefernce.Focus();
                return;
            }
            if (Convert.ToInt32(ddlInvoiceNo.SelectedIndex) == 0)
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Select Invoice No";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                ddlInvoiceNo.Focus();
                return;
            }


            //if (txtRecievedAmount.Text.Trim() == "" || Convert.ToDecimal(txtRecievedAmount.Text) == 0)
            //{
            //    PanelMsg.Visible = true;
            //    lblmsg.Text = "Enter Recieved Amount";
            //    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            //    txtRecievedAmount.Focus();
            //    return;
            //}
            if (txtRecievedAmount.Text == "")
            {
                txtRecievedAmount.Text = "0.0";
            }
            double chq = Convert.ToDouble(txtRecievedAmount.Text);
            if (txtRemaningamt.Text == "")
            {
                txtRemaningamt.Text = "0.0";
            }
            double pdq = Convert.ToDouble(txtRemaningamt.Text);

            PanelMsg.Visible = false;
            if (txtAdjsAmt.Text == "")
            {
                txtAdjsAmt.Text = "0.0";
            }

            #region Validate_Data_In_Grid
            if (dgInwardMaster.Rows.Count > 0)
            {
                for (int i = 0; i < dgInwardMaster.Rows.Count; i++)
                {
                    string ITEM_CODE = ((Label)(dgInwardMaster.Rows[i].FindControl("lblINVOICE_NO"))).Text;
                    if (ViewState["ItemUpdateIndex"].ToString() == "-1")
                    {
                        if (ITEM_CODE == ddlInvoiceNo.SelectedItem.ToString())
                        {
                            PanelMsg.Visible = true;
                            lblmsg.Text = "Record Already Exist For This Invoice No";
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                            return;
                        }
                    }
                    else
                    {
                        if (ITEM_CODE == ddlInvoiceNo.SelectedItem.ToString() && ViewState["ItemUpdateIndex"].ToString() != i.ToString())
                        {
                            PanelMsg.Visible = true;
                            lblmsg.Text = "Record Already Exist For This Invoice No";
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                            return;
                        }
                    }
                }
            }
            #endregion

            #region datatable structure
            if (((DataTable)ViewState["dt2"]).Columns.Count == 0)
            {
                ((DataTable)ViewState["dt2"]).Columns.Add("RCPD_REF_CODE");

                ((DataTable)ViewState["dt2"]).Columns.Add("REF_DESC");
                ((DataTable)ViewState["dt2"]).Columns.Add("RCPD_INVOICE_CODE");
                ((DataTable)ViewState["dt2"]).Columns.Add("RCPD_INVOICE_CODE_TEMP");
                ((DataTable)ViewState["dt2"]).Columns.Add("INVOICE_NO", typeof(string));
                ((DataTable)ViewState["dt2"]).Columns.Add("RCPD_AMOUNT");
                ((DataTable)ViewState["dt2"]).Columns.Add("RCPD_ADJ_AMOUNT");
                ((DataTable)ViewState["dt2"]).Columns.Add("RCPD_REMARK");
                ((DataTable)ViewState["dt2"]).Columns.Add("RCPD_TYPE");

            }
            #endregion
            double adjamt = 0;
            if (txtAdjsAmt.Text != "")
            {
                adjamt = Convert.ToDouble(txtAdjsAmt.Text);
            }
            #region add control value to Dt
            dr = ((DataTable)ViewState["dt2"]).NewRow();
            dr["RCPD_REF_CODE"] = ddlRefernce.SelectedValue;
            dr["REF_DESC"] = ddlRefernce.SelectedItem;
            string strInvoicType = ddlInvoiceNo.SelectedItem.Text;
            DataTable dtInvoiceCode = new DataTable();
            string[] strArr = null;
            if (ddlRefernce.SelectedValue == "-2147483648")
            {

                strArr = strInvoicType.Split('-');





                if (strArr[1] == "CN")
                {
                    dtInvoiceCode = CommonClasses.Execute("select CNM_CODE  AS PK from CREDIT_NOTE_MASTER where CNM_SERIAL_NO='" + strArr[0] + "'");
                }


                if (strArr[1] == "INV")
                {
                    dtInvoiceCode = CommonClasses.Execute("select INM_CODE AS PK from INVOICE_MASTER where INM_NO='" + strArr[0] + "'");
                }


                if (strArr[1] == "DN")
                {
                    dtInvoiceCode = CommonClasses.Execute("select DNM_CODE AS PK from DEBIT_NOTE_MASTER where DNM_SERIAL_NO='" + strArr[0] + "'");
                }


                if (strArr[1] == "BPM")
                {
                    dtInvoiceCode = CommonClasses.Execute("select BPM_CODE AS PK from BILL_PASSING_MASTER where BPM_NO='" + strArr[0] + "'");
                }
            }
            if (dtInvoiceCode.Rows.Count > 0)
            {
                dr["RCPD_INVOICE_CODE"] = dtInvoiceCode.Rows[0]["PK"].ToString();
            }
            if (ddlRefernce.SelectedValue == "-2147483647")
            {
                dr["RCPD_INVOICE_CODE"] = ddlInvoiceNo.SelectedValue;
            }

            dr["RCPD_INVOICE_CODE_TEMP"] = ddlInvoiceNo.SelectedValue;

            //dr["RCPD_INVOICE_CODE"] = ddlInvoiceNo.SelectedValue;
            dr["INVOICE_NO"] = ddlInvoiceNo.SelectedItem.ToString();
            dr["RCPD_AMOUNT"] = string.Format("{0:0.00}", (Convert.ToDouble(txtRecievedAmount.Text)));
            dr["RCPD_ADJ_AMOUNT"] = string.Format("{0:0.00}", (adjamt));

            dr["RCPD_REMARK"] = txtRemark.Text;






            if (ddlRefernce.SelectedValue == "-2147483648")
            {

                if (strArr[1] == "CN")
                {
                    dr["RCPD_TYPE"] = 0;
                }


                if (strArr[1] == "INV")
                {
                    dr["RCPD_TYPE"] = 1;

                }


                if (strArr[1] == "DN")
                {
                    dr["RCPD_TYPE"] = 2;
                }


                if (strArr[1] == "BPM")
                {
                    dr["RCPD_TYPE"] = 3;
                }

            }
            if (ddlRefernce.SelectedValue == "-2147483647")
            {
                dr["RCPD_TYPE"] = 7;
            }
            #endregion

            #region check Data table,insert or Modify Data
            if (ViewState["str"].ToString() == "Modify")
            {
                if (((DataTable)ViewState["dt2"]).Rows.Count > 0)
                {
                    ((DataTable)ViewState["dt2"]).Rows.RemoveAt(Convert.ToInt32(ViewState["Index"].ToString()));
                    ((DataTable)ViewState["dt2"]).Rows.InsertAt(dr, Convert.ToInt32(ViewState["Index"].ToString()));
                    txtRecievedAmount.Text = "";
                    txtAdjsAmt.Text = "";
                    txtRemark.Text = "";
                    ddlInvoiceNo.SelectedIndex = 0;
                    ddlRefernce.SelectedIndex = 0;
                    txtRemaningamt.Text = "";
                }
            }
            else
            {
                ((DataTable)ViewState["dt2"]).Rows.Add(dr);
                txtRecievedAmount.Text = "";
                txtAdjsAmt.Text = "";
                txtRemark.Text = "";

                ddlInvoiceNo.SelectedIndex = 0;
                ddlRefernce.SelectedIndex = 0;
                txtRemaningamt.Text = "";
            }
            #endregion

            #region Binding data to Grid
            dgInwardMaster.Enabled = true;
            dgInwardMaster.Visible = true;
            dgInwardMaster.DataSource = ((DataTable)ViewState["dt2"]);
            dgInwardMaster.DataBind();
            ViewState["str"] = "";
            ViewState["ItemUpdateIndex"] = "-1";
            #endregion

            calcAmt();
            //clearDetail();
        }


        catch (Exception Ex)
        {
            CommonClasses.SendError("Reciept Entry", "btnInsert_Click", Ex.Message);
        }
        ddlRefernce.SelectedValue = "-2147483648";
        ddlInvoiceNo.Focus();
    }
    #endregion

    #region calcAmt
    public void calcAmt()
    {
        try
        {


            double amt = 0; double Adjamt = 0;
            for (int i = 0; i < dgInwardMaster.Rows.Count; i++)
            {
                amt = Math.Round(amt, 2) + Math.Round(Convert.ToDouble(((Label)(dgInwardMaster.Rows[i].FindControl("lblRCPD_AMOUNT"))).Text), 2);
                //Adjamt = Adjamt + Convert.ToDouble(((Label)(dgInwardMaster.Rows[i].FindControl("lblRCPD_ADJ_AMOUNT"))).Text);
            }
            /*Amount = SUM(RecivedAmt)*/
            txtToalAmt.Text = (Math.Round(amt, 2)).ToString();
            //txtDiffAmt.Text = (Convert.ToDouble(string.Format("{0:0.00}", txtAmount.Text)) - (amt + Adjamt)).ToString();
            txtDiffAmt.Text = Math.Round((Convert.ToDouble(string.Format("{0:0.00}", txtAmount.Text)) - ((Math.Round(amt, 2)))), 2).ToString();
        }
        catch (Exception)
        {


        }

    }
    #endregion calcAmt

    #region ddlRefernce_SelectedIndexChanged
    protected void ddlRefernce_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlRefernce.SelectedValue == "-2147483647")
        {
            ddlInvoiceNo.Items.Clear();
            txtRemaningamt.Text = "";
            txtRecievedAmount.Text = "";
            txtAdjsAmt.Text = "";
            DataTable dtItem = new DataTable();
            dtItem = CommonClasses.Execute("select * from NEWREFERENCE_MASTER where ES_DELETE=0   and REF_CM_COMP_ID='" + (string)Session["CompanyID"] + "'");
            ddlInvoiceNo.DataSource = dtItem;
            ddlInvoiceNo.DataTextField = "REF_NAME";
            ddlInvoiceNo.DataValueField = "REF_CODE";
            ddlInvoiceNo.DataBind();
            ddlInvoiceNo.Items.Insert(0, new ListItem("Please Select Reference ", "0"));
        }
        else
        {
            FillInvoiceNo();
        }



    }
    #endregion

    #region ddlInvoiceNo_SelectedIndexChanged
    protected void ddlInvoiceNo_SelectedIndexChanged(object sender, EventArgs e)
    {

        Remainingamt();
    }
    #endregion

    #region Remainingamt
    public void Remainingamt()
    {
        string strInvoicType = ddlInvoiceNo.SelectedItem.Text;

        string[] strArr = null;
        strArr = strInvoicType.Split('-');

        if (ddlRefernce.SelectedValue == "-2147483648")
        {

            DataTable dt1 = new DataTable();
            if (strArr[1] == "CN")
            {
                dt1 = CommonClasses.Execute("select CNM_NET_AMOUNT-ISNULL(CNM_RECIEVED_AMT,0) as INM_G_AMT,plusminus  from CREDIT_NOTE_MASTER where ES_DELETE=0 AND CNM_SERIAL_NO='" + strArr[0] + "'  AND CNM_CUST_CODE='" + ddlCustomer.SelectedValue + "'");
            }


            if (strArr[1] == "INV")
            {
                dt1 = CommonClasses.Execute("select INM_G_AMT-ISNULL(INM_RECIEVED_AMT,0) as INM_G_AMT ,'' as plusminus from INVOICE_MASTER where ES_DELETE=0 AND INM_NO='" + strArr[0] + "'  AND INM_P_CODE='" + ddlCustomer.SelectedValue + "'");
            }


            if (strArr[1] == "DN")
            {
                dt1 = CommonClasses.Execute("select DNM_NET_AMOUNT-ISNULL(DNM_PAID_AMT,0) as INM_G_AMT ,plusminus from DEBIT_NOTE_MASTER where ES_DELETE=0 AND DNM_SERIAL_NO='" + strArr[0] + "'  AND DNM_CUST_CODE='" + ddlCustomer.SelectedValue + "'");
            }
            if (strArr[1] == "BPM")
            {
                dt1 = CommonClasses.Execute("select BPM_G_AMT-ISNULL(BPM_PAID_AMT,0) as INM_G_AMT , '-' as plusminus from BILL_PASSING_MASTER where ES_DELETE=0 AND BPM_NO='" + strArr[0] + "'   AND BPM_P_CODE='" + ddlCustomer.SelectedValue + "'");
            }

            if (dt1.Rows.Count > 0)
            {
                txtRemaningamt.Text = dt1.Rows[0]["plusminus"] + dt1.Rows[0]["INM_G_AMT"].ToString();
                txtRecievedAmount.Text = dt1.Rows[0]["plusminus"] + dt1.Rows[0]["INM_G_AMT"].ToString();
            }


            //txtRecievedAmount.Text = "";
            txtAdjsAmt.Text = "";
        }
    }

    #endregion

    #region ddlCustomer_SelectedIndexChanged
    protected void ddlCustomer_SelectedIndexChanged(object sender, EventArgs e)
    {
        #region FillItems
        try
        {
            if (ddlCustomer.SelectedValue.ToString() == "0")
            {
                //LoadCombos();
            }
            else
            {


                int id = Convert.ToInt32(ddlCustomer.SelectedValue);

                ddlRefernce.Items.Clear();
                FillInvoiceNo();
                DataTable dt1 = CommonClasses.FillCombo("REFERENCE_MASTER", "REF_DESC", "REF_CODE", "ES_DELETE=0  and REF_CM_ID=" + (string)Session["CompanyId"] + "  ORDER BY REF_DESC", ddlRefernce);
                ddlRefernce.Items.Insert(0, new ListItem("Please Select Reference ", "0"));

                ddlRefernce.SelectedValue = "-2147483648";
                BlankGrid();



            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Reciept Entry", "ddlCustomer_SelectedIndexChanged", Ex.Message);
        }
        #endregion
    }
    #endregion

    #region FillInvoiceNo
    public void FillInvoiceNo()
    {
        ddlInvoiceNo.Items.Clear();

        DataTable dtItem = new DataTable();
        dtItem = CommonClasses.Execute("declare @temp table(INM_CODE int IDENTITY (1,1),INVOICE_CODE int,INM_NO nvarchar(500),INM_TYPE int)insert into @temp SELECT INM_CODE,CONCAT(INM_NO,'-','INV') as INM_NO,1 AS INM_TYPE FROM INVOICE_MASTER WHERE   INVOICE_MASTER.ES_DELETE=0 AND ((ISNULL(INM_RECIEVED_AMT,0)) < (INM_G_AMT))  and INM_P_CODE='" + ddlCustomer.SelectedValue + "'    UNION SELECT CNM_CODE,CONCAT(CNM_SERIAL_NO,'-','CN'),0 AS INM_TYPE FROM CREDIT_NOTE_MASTER WHERE CREDIT_NOTE_MASTER.ES_DELETE=0 AND ((ISNULL(CNM_RECIEVED_AMT,0)) < (CNM_NET_AMOUNT))   and CNM_CUST_CODE='" + ddlCustomer.SelectedValue + "'    UNION SELECT DNM_CODE,CONCAT(DNM_SERIAL_NO,'-','DN'),2 AS INM_TYPE  FROM DEBIT_NOTE_MASTER WHERE DEBIT_NOTE_MASTER.ES_DELETE=0 and DNM_CUST_CODE='" + ddlCustomer.SelectedValue + "'    UNION SELECT BPM_CODE,CONCAT(BPM_NO,'-','BPM'),3 AS INM_TYPE  FROM BILL_PASSING_MASTER WHERE BILL_PASSING_MASTER.ES_DELETE=0 and BPM_P_CODE='" + ddlCustomer.SelectedValue + "'   select * from @temp ");
        ddlInvoiceNo.DataSource = dtItem;
        ddlInvoiceNo.DataTextField = "INM_NO";
        ddlInvoiceNo.DataValueField = "INM_CODE";
        ddlInvoiceNo.DataBind();
        ddlInvoiceNo.Items.Insert(0, new ListItem("Please Select Invoice No ", "0"));
    }
    #endregion


    #region CtlDisable
    public void CtlDisable()
    {
        ddlCustomer.Enabled = false;
        txtRCPno.Enabled = false;
        txtChequeNo.Enabled = false;

        txtChequeDate.Enabled = false;
        txtRCPDate.Enabled = false;
        BtnInsert.Visible = false;
        btnSubmit.Visible = false;
    }
    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            if (Request.QueryString[0].Equals("VIEW"))
            {
                Response.Redirect(redirect, false);
            }
            else
            {
                if (CheckValid())
                {
                    ModalPopupPrintSelection.TargetControlID = "btnCancel";
                    ModalPopupPrintSelection.Show();
                    popUpPanel5.Visible = true;
                }
                else
                {
                    CancelRecord();
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Reciept Entry", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion

    #region btnOk_Click
    protected void btnOk_Click(object sender, EventArgs e)
    {
        try
        {
            CancelRecord();
            //SaveRec();
        }
        catch (Exception Ex)
        {

            CommonClasses.SendError("Reciept Entry", "btnOk_Click", Ex.Message);
        }
    }
    #endregion

    #region btnCancel1_Click
    protected void btnCancel1_Click(object sender, EventArgs e)
    {
        //CancelRecord();
    }
    #endregion

    #region CancelRecord
    public void CancelRecord()
    {
        try
        {
            if (Convert.ToInt32(ViewState["mlCode"].ToString()) != 0 && Convert.ToInt32(ViewState["mlCode"].ToString()) != null)
            {
                CommonClasses.RemoveModifyLock("RECIEPT_MASTER", "MODIFY", "RCP_CODE", Convert.ToInt32(ViewState["mlCode"].ToString()));
            }
            Response.Redirect(redirect, false);
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Reciept Entry", "CancelRecord", ex.Message.ToString());
        }
    }
    #endregion

    #region CheckValid
    bool CheckValid()
    {
        bool flag = false;
        try
        {
            if (ddlCustomer.Text == "")
            {
                flag = false;
            }
            else if (txtChequeNo.Text == "")
            {
                flag = false;
            }
            else if (txtChequeDate.Text == "")
            {
                flag = false;
            }
            else if (txtRCPDate.Text == "")
            {
                flag = false;
            }


            else
            {
                flag = true;
            }

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Reciept Entry", "CheckValid", Ex.Message);
        }

        return flag;

    }
    #endregion

    #region txtRecievedAmount_TextChanged
    protected void txtRecievedAmount_TextChanged(object sender, EventArgs e)
    {
        string totalStr = DecimalMasking(txtRecievedAmount.Text);
        txtRecievedAmount.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(totalStr), 3));
        if (txtRemaningamt.Text == "")
        {
            txtRemaningamt.Text = "0.0";
        }
        txtAdjsAmt.Text = (Convert.ToDouble(txtRemaningamt.Text) - Convert.ToDouble(txtRecievedAmount.Text)).ToString();
    }
    #endregion txtRecievedAmount_TextChanged

    #region txtAmount_TextChanged
    protected void txtAmount_TextChanged(object sender, EventArgs e)
    {
        calcAmt();
        ddlInvoiceNo.Focus();
    }
    #endregion txtRecievedAmount_TextChanged

    protected void txtAdjsAmt_TextChanged(object sender, EventArgs e)
    {
        string totalStr = DecimalMasking(txtAdjsAmt.Text);

        txtAdjsAmt.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(totalStr), 3));
    }

    #region DecimalMasking
    protected string DecimalMasking(string Text)
    {
        string result1 = "";
        string totalStr = "";
        string result2 = "";
        string s = Text;
        string result = s.Substring(0, (s.IndexOf(".") + 1));
        int no = s.Length - result.Length;
        if (no != 0)
        {
            //if ( no > 15)
            //{
            //     no = 15;
            //}
            // int n = no - 1;
            result1 = s.Substring((result.IndexOf(".") + 1), no);

            try
            {

                result2 = result1.Substring(0, result1.IndexOf("."));
            }
            catch
            {
            }
            string result3 = s.Substring((s.IndexOf(".") + 1), 1);
            if (result3 == ".")
            {
                result1 = "00";
            }
            if (result2 != "")
            {
                totalStr = result + result2;
            }
            else
            {
                totalStr = result + result1;
            }
        }
        else
        {
            result1 = "00";
            totalStr = result + result1;
        }
        return totalStr;
    }
    #endregion




}
    //#region LoadCombosBackup
    //private void LoadCombos()
    //{
    //    #region FillCustomer
    //    try
    //    {
    //        DataTable dtParty = new DataTable();
    //        //  dt = CommonClasses.FillCombo("PARTY_MASTER,INVOICE_MASTER", "P_NAME", "P_CODE", "INVOICE_MASTER.ES_DELETE=0 AND PARTY_MASTER.ES_DELETE=0 and P_CM_COMP_ID=" + (string)Session["CompanyId"] + " and P_TYPE='2' and P_ACTIVE_IND=1 and RCP_P_CODE=P_CODE  ORDER BY P_NAME", ddlCustomer);
    //        if (Convert.ToInt32(ViewState["mlCode"].ToString()) == 0)
    //        {
    //            dt = CommonClasses.Execute("SELECT DISTINCT P_CODE,P_NAME FROM PARTY_MASTER,INVOICE_MASTER WHERE PARTY_MASTER.ES_DELETE=0 AND INM_P_CODE=P_CODE  AND INVOICE_MASTER.ES_DELETE=0  AND INM_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "'  AND P_CM_COMP_ID='" + (string)Session["CompanyId"] + "'   and P_ACTIVE_IND=1 UNION SELECT DISTINCT P_CODE,P_NAME FROM PARTY_MASTER,CREDIT_NOTE_MASTER WHERE PARTY_MASTER.ES_DELETE=0 AND CNM_CUST_CODE=P_CODE  AND CREDIT_NOTE_MASTER.ES_DELETE=0   AND CNM_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "'  AND P_CM_COMP_ID='" + (string)Session["CompanyId"] + "'   and P_ACTIVE_IND=1 UNION SELECT DISTINCT P_CODE,P_NAME FROM PARTY_MASTER,DEBIT_NOTE_MASTER WHERE PARTY_MASTER.ES_DELETE=0 AND DNM_CUST_CODE=P_CODE  AND DEBIT_NOTE_MASTER.ES_DELETE=0 AND DNM_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "'  AND P_CM_COMP_ID='" + (string)Session["CompanyId"] + "'     and P_ACTIVE_IND=1 ORDER BY P_NAME ");
    //        }
    //        else
    //        {
    //            dt = CommonClasses.Execute("SELECT DISTINCT P_CODE,P_NAME FROM PARTY_MASTER,INVOICE_MASTER WHERE PARTY_MASTER.ES_DELETE=0 AND INM_P_CODE=P_CODE AND INVOICE_MASTER.ES_DELETE=0 AND  INM_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "'  AND P_CM_COMP_ID='" + (string)Session["CompanyId"] + "'  and P_ACTIVE_IND=1   UNION SELECT DISTINCT P_CODE,P_NAME FROM PARTY_MASTER,CREDIT_NOTE_MASTER WHERE PARTY_MASTER.ES_DELETE=0 AND CNM_CUST_CODE=P_CODE  AND CREDIT_NOTE_MASTER.ES_DELETE=0   AND CNM_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "'  AND P_CM_COMP_ID='" + (string)Session["CompanyId"] + "'   and P_ACTIVE_IND=1 UNION SELECT DISTINCT P_CODE,P_NAME FROM PARTY_MASTER,DEBIT_NOTE_MASTER WHERE PARTY_MASTER.ES_DELETE=0 AND DNM_CUST_CODE=P_CODE  AND DEBIT_NOTE_MASTER.ES_DELETE=0 AND DNM_CM_CODE='" + Convert.ToInt32(Session["CompanyCode"]) + "'  AND P_CM_COMP_ID='" + (string)Session["CompanyId"] + "'     and P_ACTIVE_IND=1 ORDER BY P_NAME ");
    //        }
    //        ddlCustomer.DataSource = dt;
    //        ddlCustomer.DataTextField = "P_NAME";
    //        ddlCustomer.DataValueField = "P_CODE";
    //        ddlCustomer.DataBind();
    //        ddlCustomer.Items.Insert(0, new ListItem("Please Select Customer ", "0"));
    //    }
    //    catch (Exception Ex)
    //    {
    //        CommonClasses.SendError("Reciept Entry", "LoadCombos", Ex.Message);
    //    }
    //    #endregion

    //    #region ACCOUNTLEDGER
    //    try
    //    {

    //        dt = CommonClasses.FillCombo("ACCOUNT_LEDGER", "L_NAME", "L_CODE", "ACCOUNT_LEDGER.ES_DELETE=0  and L_CM_ID=" + (string)Session["CompanyId"] + "  ORDER BY L_NAME", ddlLedger);
    //        ddlLedger.Items.Insert(0, new ListItem("Please Select Ledger ", "0"));




    //    }
    //    catch (Exception Ex)
    //    {
    //        CommonClasses.SendError("Reciept Entry", "LoadCombos", Ex.Message);
    //    }
    //    #endregion





    //}
    //#endregion

 //#region FillInvoiceNoBAckup
 //   public void FillInvoiceNo()
 //   {
 //       ddlInvoiceNo.Items.Clear();

 //       DataTable dtItem = new DataTable();
 //       dtItem = CommonClasses.Execute("declare @temp table(INM_CODE int IDENTITY (1,1),INVOICE_CODE int,INM_NO nvarchar(500),INM_TYPE int)insert into @temp SELECT INM_CODE,CONCAT(INM_NO,'-','INV') as INM_NO,1 AS INM_TYPE FROM INVOICE_MASTER WHERE   INVOICE_MASTER.ES_DELETE=0 AND ((ISNULL(INM_RECIEVED_AMT,0)) < (INM_G_AMT))  and INM_P_CODE='" + ddlCustomer.SelectedValue + "'  and INM_CM_CODE='" + (string)Session["CompanyCode"] + "'  UNION SELECT CNM_CODE,CONCAT(CNM_SERIAL_NO,'-','CN'),0 AS INM_TYPE FROM CREDIT_NOTE_MASTER WHERE CREDIT_NOTE_MASTER.ES_DELETE=0 AND ((ISNULL(CNM_RECIEVED_AMT,0)) < (CNM_NET_AMOUNT))   and CNM_CUST_CODE='" + ddlCustomer.SelectedValue + "'   and CNM_CM_CODE='" + (string)Session["CompanyCode"] + "'  UNION SELECT DNM_CODE,CONCAT(DNM_SERIAL_NO,'-','DN'),2 AS INM_TYPE  FROM DEBIT_NOTE_MASTER WHERE DEBIT_NOTE_MASTER.ES_DELETE=0 and DNM_CUST_CODE='" + ddlCustomer.SelectedValue + "'   and DNM_CM_CODE='" + (string)Session["CompanyCode"] + "' UNION SELECT BPM_CODE,CONCAT(BPM_NO,'-','BPM'),3 AS INM_TYPE  FROM BILL_PASSING_MASTER WHERE BILL_PASSING_MASTER.ES_DELETE=0 and BPM_P_CODE='" + ddlCustomer.SelectedValue + "'   and BPM_CM_CODE='" + (string)Session["CompanyCode"] + "' select * from @temp ");
 //       ddlInvoiceNo.DataSource = dtItem;
 //       ddlInvoiceNo.DataTextField = "INM_NO";
 //       ddlInvoiceNo.DataValueField = "INM_CODE";
 //       ddlInvoiceNo.DataBind();
 //       ddlInvoiceNo.Items.Insert(0, new ListItem("Please Select Invoice No ", "0"));
 //   }
 //   #endregion