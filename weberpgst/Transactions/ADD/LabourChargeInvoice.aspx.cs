﻿using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Data.SqlClient;

public partial class Transactions_ADD_LabourChargeInvoice : System.Web.UI.Page
{
    #region Variable

    DataTable dt = new DataTable();
    DataTable dtInvoiceDetail = new DataTable();
    static int mlCode = 0;
    static int RowCount = 0;
    static double TrayQty = 0;
    DataRow dr;
    static DataTable dt2 = new DataTable();
    //static double EBasicPer = 0;
    //static double EEduCessPer = 0;
    //static double EHEduCessPer = 0;
    public static int Index = 0;
    public static string str = "";
    static string ItemUpdateIndex = "-1";
    DataTable dtFilter = new DataTable();

    #endregion

    #region ddlInvoiceType_SelectedIndexChanged
    protected void ddlInvoiceType_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlCustomer_SelectedIndexChanged(null, null);
    }
    #endregion

    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrEmpty((string)Session["CompanyId"]) && string.IsNullOrEmpty((string)Session["Username"]))
            {
                Response.Redirect("~/Default.aspx", false);
            }
            else
            {
                if (!IsPostBack)
                {
                    try
                    {
                        //EBasicPer = 0;
                        //EEduCessPer = 0;
                        //EHEduCessPer = 0;
                        ViewState["mlCode"] = "";
                        ViewState["RowCount"] = "";
                        ViewState["Index"] = "";

                        ViewState["mlCode"] = mlCode;
                        ViewState["RowCount"] = RowCount;
                        ViewState["Index"] = Index;
                        ViewState["ItemUpdateIndex"] = ItemUpdateIndex;
                        dt2.Clear();
                        ViewState["dt2"] = dt2;
                        ((DataTable)ViewState["dt2"]).Clear();
                        LoadCustomer();
                        //LoadICode();
                        //LoadIName();
                        Loadtax();
                        LoadTray();
                        TrayQty = 0;
                        LoadITariff();
                        if (Request.QueryString[0].Equals("VIEW"))
                        {

                            ViewState["mlCode"] = Convert.ToInt32(Request.QueryString[1].ToString());
                            ViewRec("VIEW");
                        }
                        else if (Request.QueryString[0].Equals("MODIFY"))
                        {
                            ViewState["mlCode"] = Convert.ToInt32(Request.QueryString[1].ToString());
                            ViewRec("MOD");
                        }
                        else if (Request.QueryString[0].Equals("INSERT"))
                        {
                            LoadFilter();

                            dgInvoiceAddDetail.Enabled = false;
                            //txtDate.Text = System.DateTime.Now.Date.ToString("dd MMM yyyy");
                            //txtIssuedate.Text = System.DateTime.Now.ToString("dd MMM yyyy");
                            //txtremovaldate.Text = System.DateTime.Now.ToString("dd MMM yyyy");
                            //txtLRDate.Text = System.DateTime.Now.Date.ToString("dd MMM yyyy");
                            //txtIssuetime.Text = DateTime.Now.ToString("HH:mm ");


                            txtDate.Text = CommonClasses.GetCurrentTime().Date.ToString("dd MMM yyyy");
                            txtIssuedate.Text = CommonClasses.GetCurrentTime().ToString("dd MMM yyyy");
                            txtremovaldate.Text = CommonClasses.GetCurrentTime().ToString("dd MMM yyyy");
                            txtLRDate.Text = CommonClasses.GetCurrentTime().Date.ToString("dd MMM yyyy");
                            txtIssuetime.Text = CommonClasses.GetCurrentTime().ToString("HH:mm ");



                            DateTime dtremovalDate = new DateTime();
                            dtremovalDate = Convert.ToDateTime(txtIssuetime.Text).AddMinutes(10);
                            txtRemoveltime.Text = dtremovalDate.ToString("HH:mm");

                            txtDate.Attributes.Add("readonly", "readonly");
                            txtIssuedate.Attributes.Add("readonly", "readonly");
                            txtremovaldate.Attributes.Add("readonly", "readonly");
                            txtLRDate.Attributes.Add("readonly", "readonly");

                        }
                        txtInvoiceNo.Focus();
                        dt.Rows.Clear();
                    }
                    catch (Exception ex)
                    {
                        CommonClasses.SendError("Tax Invoice", "Page_Load", ex.Message.ToString());
                    }
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Tax Invoice", "Page_Load", ex.Message.ToString());
        }
    }
    #endregion Page_Load

    #region btnSubmit_Click
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        //Page.ClientScript.RegisterStartupScript(typeof(Page), "Test", "<script type='text/javascript'>VerificaCamposObrigatorios();</script>");

        //if (ddlCustomer.SelectedIndex == 0)
        //{
        //    ShowMessage("#Avisos", "Select Customer", CommonClasses.MSG_Warning);
        //    PanelMsg.Visible = true;
        //    lblmsg.Text = "Select Customer Name";
        //    ddlCustomer.Focus();
        //    return;

        //}
        //if (txtDate.Text == "")
        //{
        //    //ShowMessage("#Avisos", "Select Item Code", CommonClasses.MSG_Warning);
        //    PanelMsg.Visible = true;
        //    lblmsg.Text = "Select Invoice Date";
        //    txtDate.Focus();
        //    return;
        //}
        if (dgInvoiceAddDetail.Enabled == false)
        {

            ShowMessage("#Avisos", "Record Not Found In Table", CommonClasses.MSG_Warning);
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
            //PanelMsg.Visible = true;
            //lblmsg.Text = "Record Not Exist";
            return;
        }
        //if (ddlTaxName.SelectedIndex == 0)
        //{
        //    ShowMessage("#Avisos", "Select Tax Name", CommonClasses.MSG_Warning);
        //    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
        //    //PanelMsg.Visible = true;
        //    //lblmsg.Text = "Select Tax Name";
        //    ddlTaxName.Focus();
        //    return;
        //}

        if (dgInvoiceAddDetail.Rows.Count > 0)
        {
            SaveRec();
        }
        else
        {
            ShowMessage("#Avisos", "Record Not Found In Table", CommonClasses.MSG_Warning);
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
            //PanelMsg.Visible = true;
            //lblmsg.Text = "Record Not Exist";
        }

    }

    #endregion btnSubmit_Click

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            if (Request.QueryString[0].Equals("VIEW"))
            {
                CancelRecord();
            }
            else
            {
                if (CheckValid())
                {
                    popUpPanel5.Visible = true;
                    //ModalPopupPrintSelection.TargetControlID = "btnCancel";
                    ModalPopupPrintSelection.Show();

                }
                else
                {
                    CancelRecord();
                }
            }

        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Tax Invoice", "btnCancel_Click", ex.Message.ToString());
        }
    }
    #endregion

    #region btnOk_Click
    protected void btnOk_Click(object sender, EventArgs e)
    {
        try
        {
            //SaveRec();
            CancelRecord();
        }
        catch (Exception Ex)
        {

            CommonClasses.SendError("Tax Invocie", "btnOk_Click", Ex.Message);
        }
    }
    #endregion

    #region btnCancel1_Click
    protected void btnCancel1_Click(object sender, EventArgs e)
    {
        // CancelRecord();
    }
    #endregion

    #region CancelRecord
    public void CancelRecord()
    {
        try
        {
            if (Convert.ToInt32(ViewState["mlCode"]) != 0 && Convert.ToInt32(ViewState["mlCode"]) != null)
            {
                CommonClasses.RemoveModifyLock("INVOICE_MASTER", "MODIFY", "INM_CODE", Convert.ToInt32(ViewState["mlCode"]));
            }

            ((DataTable)ViewState["dt2"]).Rows.Clear();
            Response.Redirect("~/Transactions/VIEW/ViewLabourChargeInvoice.aspx", false);
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Tax Invoice", "CancelRecord", ex.Message.ToString());
        }
    }
    #endregion

    #region CheckValid
    bool CheckValid()
    {
        bool flag = false;
        try
        {
            if (ddlCustomer.SelectedIndex == 0)
            {
                flag = false;
            }
            else if (txtDate.Text == "")
            {
                flag = false;
            }
            else if (ddlTaxName.SelectedIndex == 0)
            {
                flag = false;
            }
            //else if (dgInvoiceAddDetail.Enabled)
            //{
            //    flag = true;
            //}
            else
            {
                flag = true;
            }

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Tax Invoice", "CheckValid", Ex.Message);
        }

        return flag;

    }
    #endregion

    #region Loadtax
    private void Loadtax()
    {
        try
        {
            dt = CommonClasses.Execute("select ST_CODE,ST_TAX_NAME from SALES_TAX_MASTER where ES_DELETE=0 and ST_CM_COMP_ID=" + (string)Session["CompanyId"] + "  ORDER BY ST_TAX_NAME");
            ddlTaxName.DataSource = dt;
            ddlTaxName.DataTextField = "ST_TAX_NAME";
            ddlTaxName.DataValueField = "ST_CODE";
            ddlTaxName.DataBind();
            ddlTaxName.Items.Insert(0, new ListItem("Select Tax Name", "0"));
            ddlTaxName.SelectedIndex = -1;
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Tax Invoice", "LoadICode", Ex.Message);
        }

    }
    #endregion

    #region LoadCustomer
    private void LoadCustomer()
    {
        try
        {
            dt = CommonClasses.Execute("select distinct(P_CODE),P_NAME from PARTY_MASTER,CUSTPO_MASTER where CPOM_P_CODE=P_CODE and CUSTPO_MASTER.ES_DELETE=0 and P_CM_COMP_ID=" + (string)Session["CompanyId"] + " and P_TYPE='1' and P_ACTIVE_IND=1 AND CPOM_TYPE=-2147483648  ORDER BY P_NAME ASC-- AND CPOM_TYPE=-2147483648  ORDER BY P_NAME ASC");
            ddlCustomer.DataSource = dt;
            ddlCustomer.DataTextField = "P_NAME";
            ddlCustomer.DataValueField = "P_CODE";
            ddlCustomer.DataBind();
            ddlCustomer.Items.Insert(0, new ListItem("Select Customer", "0"));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Tax Invoice", "LoadCustomer", Ex.Message);
        }

    }
    #endregion

    #region LoadICode
    private void LoadICode()
    {
        try
        {
            DataTable dtItem = new DataTable();
            if (ddlInvoiceType.SelectedIndex.ToString() == "0")
            {
                //dtItem = CommonClasses.Execute("SELECT DISTINCT I_CODE,I_CODENO,I_NAME FROM CUSTPO_MASTER,CUSTPO_DETAIL,ITEM_MASTER,BOM_MASTER,BOM_DETAIL where CPOM_CODE=CPOD_CPOM_CODE  AND BD_I_CODE=I_CODE AND BM_CODE=BD_BM_CODE AND CPOD_I_CODE=BM_I_CODE AND CPOM_P_CODE=" + ddlCustomer.SelectedValue + "    order by  I_CODENO ASC ");
                dtItem = CommonClasses.Execute("SELECT DISTINCT I_CODE,I_CODENO,I_NAME FROM CUSTPO_MASTER,CUSTPO_DETAIL,ITEM_MASTER,BOM_MASTER,BOM_DETAIL where CPOM_CODE=CPOD_CPOM_CODE  AND BM_I_CODE=I_CODE AND BM_CODE=BD_BM_CODE AND CPOD_I_CODE=BM_I_CODE AND CPOM_P_CODE=" + ddlCustomer.SelectedValue + "    AND  I_CM_COMP_ID='" + (string)Session["CompanyId"] + "' ORDER BY I_NAME");
            }
            else
            {
                //dtItem = CommonClasses.Execute("select distinct I_CODE,I_CODENO from ITEM_MASTER,CUSTPO_DETAIL,CUSTPO_MASTER where CUSTPO_MASTER.ES_DELETE=0 and  ((CPOD_ORD_QTY-CPOD_DISPACH)>0 or CPOM_CODE=CPOD_CPOM_CODE) and CPOD_I_CODE=I_CODE and  CPOM_CODE=CPOD_CPOM_CODE and CPOM_P_CODE=" + ddlCustomer.SelectedValue + " and CPOM_IS_VERBAL=0 AND CPOM_TYPE=-2147483648 order by I_CODENO ASC");
                dtItem = CommonClasses.Execute("SELECT DISTINCT I_CODE,I_CODENO FROM ITEM_MASTER,CUSTPO_DETAIL,CUSTPO_MASTER WHERE CUSTPO_MASTER.ES_DELETE=0 AND  (( ISNULL(CPOD_ORD_QTY,0)-ISNULL(CPOD_DISPACH,0)) > 0 or CPOM_CODE=CPOD_CPOM_CODE) and CPOD_I_CODE=I_CODE and  CPOM_CODE=CPOD_CPOM_CODE and CPOM_P_CODE=" + ddlCustomer.SelectedValue + " order by I_CODENO ASC");
            }
            ddlItemCode.DataSource = dtItem;
            ddlItemCode.DataTextField = "I_CODENO";
            ddlItemCode.DataValueField = "I_CODE";
            ddlItemCode.DataBind();
            ddlItemCode.Items.Insert(0, new ListItem("Select Item Code", "0"));
            ddlItemCode.SelectedIndex = -1;
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Tax Invoice", "LoadICode", Ex.Message);
        }

    }
    #endregion

    #region LoadPO
    private void LoadPO()
    {
        try
        {
            DataTable dtPO = new DataTable();
            if (Request.QueryString[0].Equals("MODIFY"))
            {
                dtPO = CommonClasses.Execute("SELECT DISTINCT(CPOM_CODE),CPOM_PONO FROM CUSTPO_MASTER,CUSTPO_DETAIL,INVOICE_DETAIL WHERE CPOM_CODE=CPOD_CPOM_CODE and  CPOM_P_CODE='" + ddlCustomer.SelectedValue + "' AND CUSTPO_MASTER.ES_DELETE=0 and ((CPOD_ORD_QTY-CPOD_DISPACH)>0 or IND_CPOM_CODE=CPOM_CODE)   ");
            }
            else
            {
                dtPO = CommonClasses.Execute("SELECT DISTINCT(CPOM_CODE),CPOM_PONO FROM CUSTPO_MASTER,CUSTPO_DETAIL WHERE CPOM_CODE=CPOD_CPOM_CODE and CPOM_P_CODE='" + ddlCustomer.SelectedValue + "' AND ES_DELETE=0 and (CPOD_ORD_QTY-CPOD_DISPACH)>0  ");
            }

            ddlPONo.DataSource = dtPO;
            ddlPONo.DataTextField = "CPOM_PONO";
            ddlPONo.DataValueField = "CPOM_CODE";
            ddlPONo.DataBind();
            ddlPONo.Items.Insert(0, new ListItem("Select PO", "0"));

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Tax Invoice", "LoadPO", Ex.Message);
        }

    }
    #endregion

    #region LoadIName
    private void LoadIName()
    {
        try
        {
            DataTable dtItem = new DataTable();
            if (ddlInvoiceType.SelectedIndex.ToString() == "0")
            {
                dtItem = CommonClasses.Execute("SELECT DISTINCT I_CODE,I_CODENO,I_NAME FROM CUSTPO_MASTER,CUSTPO_DETAIL,ITEM_MASTER,BOM_MASTER,BOM_DETAIL where CPOM_CODE=CPOD_CPOM_CODE  AND BD_I_CODE=I_CODE AND BM_CODE=BD_BM_CODE AND CPOD_I_CODE=BM_I_CODE AND CPOM_P_CODE=" + ddlCustomer.SelectedValue + "    order by  I_CODENO ASC ");
                dtItem = CommonClasses.Execute("SELECT DISTINCT I_CODE,I_CODENO,I_NAME FROM CUSTPO_MASTER,CUSTPO_DETAIL,ITEM_MASTER,BOM_MASTER,BOM_DETAIL where CPOM_CODE=CPOD_CPOM_CODE  AND BM_I_CODE=I_CODE AND BM_CODE=BD_BM_CODE AND CPOD_I_CODE=BM_I_CODE AND CPOM_P_CODE=" + ddlCustomer.SelectedValue + "    AND  I_CM_COMP_ID='" + (string)Session["CompanyId"] + "' ORDER BY I_NAME");
            }
            else
            {
                //dtItem = CommonClasses.Execute("select distinct I_CODE,I_NAME from ITEM_MASTER,CUSTPO_DETAIL,CUSTPO_MASTER where CUSTPO_MASTER.ES_DELETE=0 and  ((CPOD_ORD_QTY-CPOD_DISPACH)>0 or CPOM_CODE=CPOD_CPOM_CODE) and  CPOD_I_CODE=I_CODE   and CPOM_CODE=CPOD_CPOM_CODE  and CPOM_P_CODE=" + ddlCustomer.SelectedValue + " and CPOM_IS_VERBAL=0 AND CPOM_TYPE=-2147483648 order by I_NAME ASC");
                dtItem = CommonClasses.Execute("SELECT DISTINCT I_CODE,I_NAME,I_CODENO FROM ITEM_MASTER,CUSTPO_DETAIL,CUSTPO_MASTER WHERE CUSTPO_MASTER.ES_DELETE=0 AND  (( ISNULL(CPOD_ORD_QTY,0)-ISNULL(CPOD_DISPACH,0)) > 0 or CPOM_CODE=CPOD_CPOM_CODE) and CPOD_I_CODE=I_CODE and  CPOM_CODE=CPOD_CPOM_CODE and CPOM_P_CODE=" + ddlCustomer.SelectedValue + " order by I_CODENO ASC");
            }

            ddlItemName.DataSource = dtItem;
            ddlItemName.DataTextField = "I_NAME";
            ddlItemName.DataValueField = "I_CODE";
            ddlItemName.DataBind();
            ddlItemName.Items.Insert(0, new ListItem("Select Item Name", "0"));
            ddlItemName.SelectedIndex = -1;
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Order ", "LoadIName", Ex.Message);
        }

    }
    #endregion

    #region LoadFilter
    public void LoadFilter()
    {
        if (dgInvoiceAddDetail.Rows.Count == 0)
        {
            dtFilter.Clear();

            if (dtFilter.Columns.Count == 0)
            {
                dtFilter.Columns.Add(new System.Data.DataColumn("IND_I_CODE", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("IND_I_CODENO", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("IND_I_NAME", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("UOM", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("PO_CODE", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("PO_NO", typeof(String)));

                dtFilter.Columns.Add(new System.Data.DataColumn("STOCK_QTY", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("PEND_QTY", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("INV_QTY", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("ACT_WGHT", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("RATE", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("AMT", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("AmortRate", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("AmortAmount", typeof(String)));

                dtFilter.Columns.Add(new System.Data.DataColumn("IND_SUBHEADING", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("IND_BACHNO", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("IND_NO_PACK", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("IND_PAK_QTY", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("IND_EX_AMT", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("IND_E_CESS_AMT", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("IND_SH_CESS_AMT", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("IND_PACK_DESC", typeof(String)));

                dtFilter.Columns.Add(new System.Data.DataColumn("IND_E_CODE", typeof(String)));
                dtFilter.Columns.Add(new System.Data.DataColumn("IND_E_TARIFF_NO", typeof(String)));

                dtFilter.Rows.Add(dtFilter.NewRow());
                dgInvoiceAddDetail.DataSource = dtFilter;
                dgInvoiceAddDetail.DataBind();
                dgInvoiceAddDetail.Enabled = false;
            }
        }
    }
    #endregion

    #region LoadFilter1
    public void LoadFilter1()
    {

        dtFilter.Clear();

        if (dtFilter.Columns.Count == 0)
        {
            dtFilter.Columns.Add(new System.Data.DataColumn("IND_I_CODE", typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("IND_I_CODENO", typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("IND_I_NAME", typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("UOM", typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("PO_CODE", typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("PO_NO", typeof(String)));

            dtFilter.Columns.Add(new System.Data.DataColumn("STOCK_QTY", typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("PEND_QTY", typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("INV_QTY", typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("ACT_WGHT", typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("RATE", typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("AMT", typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("AmortRate", typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("AmortAmount", typeof(String)));

            dtFilter.Columns.Add(new System.Data.DataColumn("IND_SUBHEADING", typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("IND_BACHNO", typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("IND_NO_PACK", typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("IND_PAK_QTY", typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("IND_EX_AMT", typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("IND_E_CESS_AMT", typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("IND_SH_CESS_AMT", typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("IND_PACK_DESC", typeof(String)));

            dtFilter.Rows.Add(dtFilter.NewRow());
            dgInvoiceAddDetail.DataSource = dtFilter;
            dgInvoiceAddDetail.DataBind();
            dgInvoiceAddDetail.Enabled = false;
        }

    }
    #endregion

    #region LoadFilter1
    public void LoadFilterBlank()
    {

        dtFilter.Clear();

        if (dtFilter.Columns.Count == 0)
        {
            dtFilter.Columns.Add(new System.Data.DataColumn("IND_I_CODE", typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("IND_I_CODENO", typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("IND_I_NAME", typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("UOM", typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("PO_CODE", typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("PO_NO", typeof(String)));

            dtFilter.Columns.Add(new System.Data.DataColumn("STOCK_QTY", typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("PEND_QTY", typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("INV_QTY", typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("ACT_WGHT", typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("RATE", typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("AMT", typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("AmortRate", typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("AmortAmount", typeof(String)));

            dtFilter.Columns.Add(new System.Data.DataColumn("IND_SUBHEADING", typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("IND_BACHNO", typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("IND_NO_PACK", typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("IND_PAK_QTY", typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("IND_EX_AMT", typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("IND_E_CESS_AMT", typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("IND_SH_CESS_AMT", typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("IND_PACK_DESC", typeof(String)));

            dtFilter.Columns.Add(new System.Data.DataColumn("IND_E_CODE", typeof(String)));
            dtFilter.Columns.Add(new System.Data.DataColumn("IND_E_TARIFF_NO", typeof(String)));

            dtFilter.Rows.Add(dtFilter.NewRow());
            dgInvoiceAddDetail.DataSource = dtFilter;
            dgInvoiceAddDetail.DataBind();
            dgInvoiceAddDetail.Enabled = false;
        }

    }
    #endregion

    #region LoadTray
    private void LoadTray()
    {
        try
        {
            dt = CommonClasses.Execute("select I_CODE,I_CODENO from ITEM_MASTER where es_delete ='0' and I_CAT_CODE ='-2147483633' order by I_CODENO ASC");
            ddlTray.DataSource = dt;
            ddlTray.DataTextField = "I_CODENO";
            ddlTray.DataValueField = "I_CODE";
            ddlTray.DataBind();
            ddlTray.Items.Insert(0, new ListItem("Select Tray", "0"));
            ddlTray.SelectedIndex = -1;
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Tax Invoice", "LoadTray", Ex.Message);
        }

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
            CommonClasses.SendError("Tax Invoice", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region ddlItemCode_SelectedIndexChanged
    protected void ddlItemCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlItemCode.SelectedIndex != 0)
            {
                ddlItemName.SelectedValue = ddlItemCode.SelectedValue;
                DataTable dt1 = CommonClasses.Execute("Select ITEM_UNIT_MASTER.I_UOM_CODE,I_UOM_NAME,I_UWEIGHT,I_CURRENT_BAL,I_SCAT_CODE from ITEM_UNIT_MASTER,ITEM_MASTER where ITEM_MASTER.I_UOM_CODE=ITEM_UNIT_MASTER.I_UOM_CODE and ITEM_MASTER.ES_DELETE=0 and I_CODE='" + ddlItemCode.SelectedValue + "'");
                DataTable dtPO = new DataTable();



                if (ddlInvoiceType.SelectedIndex == 0)
                {
                    if (Request.QueryString[0].Equals("MODIFY"))
                    {
                        //dtPO = CommonClasses.Execute("SELECT DISTINCT(CPOM_CODE),CPOM_PONO FROM CUSTPO_MASTER,CUSTPO_DETAIL,INVOICE_DETAIL,BOM_MASTER,BOM_DETAIL WHERE CPOM_CODE=CPOD_CPOM_CODE AND CPOD_I_CODE=BM_I_CODE AND BM_CODE=BD_BM_CODE and BD_I_CODE='" + ddlItemCode.SelectedValue + "' AND CPOM_P_CODE='" + ddlCustomer.SelectedValue + "' AND CUSTPO_MASTER.ES_DELETE=0  and IND_INM_CODE=" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "--and (( ISNULL(CPOD_ORDER_QTY,0)-ISNULL(CPOD_INW_QTY,0))>=0 or IND_CPOM_CODE=CPOM_CODE)  ");
                        dtPO = CommonClasses.Execute("SELECT CPOM_CODE,Convert(varchar,CPOM_PONO) as  CPOM_PONO FROM CUSTPO_MASTER,CUSTPO_DETAIL,ITEM_MASTER,PROJECT_CODE_MASTER WHERE  CPOM_CODE=CPOD_CPOM_CODE AND CPOM_P_CODE='" + ddlCustomer.SelectedValue + "' AND CPOD_I_CODE=I_CODE AND CPOD_I_CODE='" + ddlItemName.SelectedValue + "' AND CPOM_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' AND  CUSTPO_MASTER.ES_DELETE=0 and CPOM_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' AND  PROCM_CODE=CPOM_PROJECT_CODE  and CPOM_IS_VERBAL=0  order by CPOM_CODE");
                    }
                    else
                    {
                        //dtPO = CommonClasses.Execute("SELECT DISTINCT(CPOM_CODE),CPOM_PONO FROM  CUSTPO_MASTER,CUSTPO_DETAIL,BOM_MASTER,BOM_DETAIL WHERE CPOM_CODE=CPOD_CPOM_CODE AND CPOD_I_CODE=BM_I_CODE AND BM_CODE=BD_BM_CODE and BD_I_CODE='" + ddlItemCode.SelectedValue + "' AND CPOM_P_CODE='" + ddlCustomer.SelectedValue + "' AND CUSTPO_MASTER.ES_DELETE=0 --and ( ISNULL(CPOD_ORD_QTY,0)-ISNULL(CPOD_INW_QTY,0))>=0   ");
                        dtPO = CommonClasses.Execute("select CPOM_CODE,Convert(varchar,CPOM_PONO)  as  CPOM_PONO FROM CUSTPO_MASTER,CUSTPO_DETAIL,ITEM_MASTER ,PROJECT_CODE_MASTER WHERE  CPOM_CODE=CPOD_CPOM_CODE AND PROCM_CODE=CPOM_PROJECT_CODE  and  CPOM_P_CODE='" + ddlCustomer.SelectedValue + "' AND CPOD_I_CODE=I_CODE AND CPOD_I_CODE='" + ddlItemName.SelectedValue + "' AND CPOM_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' and CPOM_IS_VERBAL=0  order by CPOM_CODE");
                    }
                }
                else
                {
                    if (Request.QueryString[0].Equals("MODIFY"))
                    {
                        dtPO = CommonClasses.Execute("SELECT DISTINCT(CPOM_CODE),CPOM_PONO FROM CUSTPO_MASTER,CUSTPO_DETAIL,INVOICE_DETAIL WHERE CPOM_CODE=CPOD_CPOM_CODE and CPOD_I_CODE='" + ddlItemCode.SelectedValue + "' AND CPOM_P_CODE='" + ddlCustomer.SelectedValue + "' AND CUSTPO_MASTER.ES_DELETE=0   and IND_INM_CODE=" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "   and CPOM_IS_VERBAL=0");
                    }
                    else
                    {
                        dtPO = CommonClasses.Execute("SELECT DISTINCT(CPOM_CODE),CPOM_PONO FROM CUSTPO_MASTER,CUSTPO_DETAIL WHERE CPOM_CODE=CPOD_CPOM_CODE and CPOD_I_CODE='" + ddlItemCode.SelectedValue + "' AND CPOM_P_CODE='" + ddlCustomer.SelectedValue + "' AND CUSTPO_MASTER.ES_DELETE=0 and ES_DELETE=0    and CPOM_IS_VERBAL=0");
                    }
                }


                ddlPONo.DataSource = dtPO;
                ddlPONo.DataTextField = "CPOM_PONO";
                ddlPONo.DataValueField = "CPOM_CODE";
                ddlPONo.DataBind();
                //ddlPONo.Items.Insert(0, new ListItem("Select PO", "0"));


                ddlPONo_SelectedIndexChanged(null, null);


                if (dt1.Rows.Count > 0)
                {
                    txtUOM.Text = dt1.Rows[0]["I_UOM_NAME"].ToString();
                    txtActWght.Text = dt1.Rows[0]["I_UWEIGHT"].ToString();
                    txtStockQty.Text = dt1.Rows[0]["I_CURRENT_BAL"].ToString();
                    ddlTariff.SelectedValue = dt1.Rows[0]["I_SCAT_CODE"].ToString();
                    txtVQty.Text = "0";
                }
                else
                {
                    txtUOM.Text = "0";
                    txtVQty.Text = "0";
                }

            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Tax Invoice", "ddlItemCode_SelectedIndexChanged", Ex.Message);

        }
    }
    #endregion

    #region ddlPONo_SelectedIndexChanged
    protected void ddlPONo_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlItemCode.SelectedIndex != 0)
            {
                double inmQTy = 0;

                DataTable dt1 = new DataTable();
                DataTable dtQty = new DataTable();
                DataTable dtInvQty = new DataTable();
                if (ddlInvoiceType.SelectedIndex == 0)
                {
                    dt1 = CommonClasses.Execute("select CPOD_RATE,ISNULL(CPOD_AMORTRATE,0)  AS CPOD_AMORTRATE from CUSTPO_MASTER,CUSTPO_DETAIL where CUSTPO_MASTER.ES_DELETE=0 AND CPOM_CODE=CPOD_CPOM_CODE AND CPOM_P_CODE='" + ddlCustomer.SelectedValue + "' AND  CPOM_CM_COMP_ID='" + Convert.ToInt32(Session["CompanyId"]) + "' AND CPOD_I_CODE='" + ddlItemName.SelectedValue + "' and CPOM_CODE='" + ddlPONo.SelectedValue + "'");

                    // dtQty = CommonClasses.Execute("SELECT ISNULL(SUM(ISNULL(CHALLAN_STOCK_LEDGER.CL_CQTY,0) - ISNULL(CHALLAN_STOCK_LEDGER.CL_CON_QTY,0)), 0) / (ISNULL(BOM_DETAIL.BD_VQTY, 0) + ISNULL(BOM_DETAIL.BD_SCRAPQTY, 0)) AS PENDING_QTY FROM BOM_DETAIL INNER JOIN BOM_MASTER ON BOM_DETAIL.BD_BM_CODE = BOM_MASTER.BM_CODE INNER JOIN CHALLAN_STOCK_LEDGER ON BOM_DETAIL.BD_I_CODE = CHALLAN_STOCK_LEDGER.CL_I_CODE LEFT OUTER JOIN CUSTPO_DETAIL ON BOM_MASTER.BM_I_CODE = CUSTPO_DETAIL.CPOD_I_CODE WHERE (CUSTPO_DETAIL.CPOD_CPOM_CODE = '" + ddlPONo.SelectedValue + "') AND (BOM_MASTER.BM_I_CODE = '" + ddlItemCode.SelectedValue + "') AND (CHALLAN_STOCK_LEDGER.CL_P_CODE = '" + ddlCustomer.SelectedValue + "') AND (CHALLAN_STOCK_LEDGER.CL_DOC_TYPE = 'IWIFP') GROUP BY ISNULL(BOM_DETAIL.BD_VQTY, 0) + ISNULL(BOM_DETAIL.BD_SCRAPQTY, 0), CHALLAN_STOCK_LEDGER.CL_I_CODE ORDER BY SUM(CHALLAN_STOCK_LEDGER.CL_CQTY - CHALLAN_STOCK_LEDGER.CL_CON_QTY)");

                    dtQty = CommonClasses.Execute("SELECT ISNULL(SUM(ISNULL(CHALLAN_STOCK_LEDGER.CL_CQTY,0) - ISNULL(CHALLAN_STOCK_LEDGER.CL_CON_QTY,0)), 0) /(ISNULL(BOM_DETAIL.BD_VQTY, 0) + ISNULL(BOM_DETAIL.BD_SCRAPQTY, 0)) AS PENDING_QTY FROM CUSTPO_DETAIL INNER JOIN CUSTPO_MASTER ON CUSTPO_DETAIL.CPOD_CPOM_CODE = CUSTPO_MASTER.CPOM_CODE INNER JOIN BOM_MASTER INNER JOIN BOM_DETAIL ON BOM_MASTER.BM_CODE = BOM_DETAIL.BD_BM_CODE ON CUSTPO_DETAIL.CPOD_I_CODE = BOM_MASTER.BM_I_CODE LEFT OUTER JOIN CHALLAN_STOCK_LEDGER ON CUSTPO_MASTER.CPOM_P_CODE = CHALLAN_STOCK_LEDGER.CL_P_CODE AND  BOM_DETAIL.BD_I_CODE = CHALLAN_STOCK_LEDGER.CL_I_CODE WHERE  (CUSTPO_DETAIL.CPOD_CPOM_CODE = '" + ddlPONo.SelectedValue + "') AND  (BOM_MASTER.BM_I_CODE =   '" + ddlItemCode.SelectedValue + "') AND (CUSTPO_MASTER.CPOM_P_CODE = '" + ddlCustomer.SelectedValue + "')   GROUP BY (ISNULL(BOM_DETAIL.BD_VQTY, 0) + ISNULL(BOM_DETAIL.BD_SCRAPQTY, 0)),BD_I_CODE ORDER BY SUM(CHALLAN_STOCK_LEDGER.CL_CQTY - CHALLAN_STOCK_LEDGER.CL_CON_QTY)");
                    dtInvQty = CommonClasses.Execute("  SELECT   ISNULL(SUM(IND_INQTY),0) AS  IND_INQTY  FROM INVOICE_MASTER,INVOICE_DETAIL WHERE INM_CODE=IND_INM_CODE AND  INVOICE_MASTER.ES_DELETE=0 AND INM_TYPE='OutJWINM' AND IND_I_CODE=" + ddlItemCode.SelectedValue + " and IND_CPOM_CODE=" + ddlPONo.SelectedValue + " ");
                    if (dtInvQty.Rows.Count > 0)
                    {
                        inmQTy = Convert.ToDouble(dtInvQty.Rows[0]["IND_INQTY"].ToString());
                    }
                }
                else
                {
                    dt1 = CommonClasses.Execute("select CPOD_RATE,ISNULL(CPOD_MODNO,0) AS ModNo,ISNULL(CPOD_MODDATE,GetDate()) AS ModDate,ISNULL(CPOD_AMORTRATE,0)  AS CPOD_AMORTRATE  from CUSTPO_DETAIL where CPOD_I_CODE='" + ddlItemCode.SelectedValue + "' and CPOD_CPOM_CODE=" + ddlPONo.SelectedValue + " ");
                    //dtQty = CommonClasses.Execute("SELECT (CPOD_ORD_QTY-CPOD_DISPACH) as Qty FROM CUSTPO_DETAIL where CPOD_I_CODE=" + ddlItemCode.SelectedValue + " and CPOD_CPOM_CODE=" + ddlPONo.SelectedValue + "  ");
                    //dtInvQty = CommonClasses.Execute("SELECT ISNULL(SUM(IND_INQTY),0) AS IND_INQTY  FROM INVOICE_MASTER,INVOICE_DETAIL WHERE INM_CODE=IND_INM_CODE AND  INVOICE_MASTER.ES_DELETE=0 AND INM_TYPE='OutJWINM' AND IND_I_CODE=" + ddlItemCode.SelectedValue + " and IND_CPOM_CODE=" + ddlPONo.SelectedValue + " ");
                    //dtQty = CommonClasses.Execute("SELECT ISNULL(SUM(CL_CQTY-CL_CON_QTY),0) as PENDING_QTY   FROM CUSTPO_DETAIL,CHALLAN_STOCK_LEDGER where CPOD_CPOM_CODE='" + ddlPONo.SelectedValue + "' AND CPOD_I_CODE='" + ddlItemCode.SelectedValue + "' AND CPOD_I_CODE=CL_I_CODE AND CL_P_CODE='" + ddlCustomer.SelectedValue + "'  AND  CL_DOC_TYPE='IWIFP' ");

                    dtQty = CommonClasses.Execute(" SELECT ISNULL(SUM(CL_CQTY-CL_CON_QTY),0) as PENDING_QTY     FROM CUSTPO_DETAIL,CHALLAN_STOCK_LEDGER,CUSTPO_MASTER,INWARD_MASTER,INWARD_DETAIL  where CPOD_CPOM_CODE='" + ddlPONo.SelectedValue + "' AND CPOD_I_CODE='" + ddlItemCode.SelectedValue + "' AND CPOD_I_CODE=CL_I_CODE AND CL_P_CODE='" + ddlCustomer.SelectedValue + "'  AND  CL_DOC_TYPE='IWIFP'   AND CPOD_CPOM_CODE=CPOM_CODE AND CL_P_CODE=CPOM_P_CODE AND IWM_CODE=CL_DOC_ID  AND IWM_CODE=IWD_IWM_CODE AND IWD_CPOM_CODE=CPOD_CPOM_CODE    AND IWM_INWARD_TYPE=1 ");
                    dtInvQty = CommonClasses.Execute("  SELECT   ISNULL(SUM(IND_INQTY),0) AS  IND_INQTY  FROM INVOICE_MASTER,INVOICE_DETAIL WHERE INM_CODE=IND_INM_CODE AND  INVOICE_MASTER.ES_DELETE=0 AND INM_TYPE='OutJWINM' AND IND_I_CODE=" + ddlItemCode.SelectedValue + " and IND_CPOM_CODE=" + ddlPONo.SelectedValue + " ");
                    if (dtInvQty.Rows.Count > 0)
                    {
                        inmQTy = Convert.ToDouble(dtInvQty.Rows[0]["IND_INQTY"].ToString());
                    }
                }
                if (dt1.Rows.Count > 0)
                {
                    txtRate.Text = string.Format("{0:0.00}", dt1.Rows[0]["CPOD_RATE"]);
                    txtAmortrate.Text = string.Format("{0:0.00}", dt1.Rows[0]["CPOD_AMORTRATE"]);
                }
                else
                {
                    txtRate.Text = "0.00";
                }
                if (dtQty.Rows.Count > 0)
                {


                    txtPendingQty.Text = string.Format("{0:0.000}", Convert.ToDouble(dtQty.Rows[0]["PENDING_QTY"]));


                    txtVQty.Text = "0";
                }
                else
                {
                    txtPendingQty.Text = "0.000";
                    txtVQty.Text = "0";
                }

            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Tax Invoice", "ddlPONo_SelectedIndexChanged", Ex.Message);

        }
    }
    #endregion

    #region ddlCustomer_SelectedIndexChanged
    protected void ddlCustomer_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //LoadICode();
            //LoadIName();
            try
            {
                if (ddlInvoiceType.SelectedIndex.ToString() == "0")
                {
                    LoadICode();
                    LoadIName();
                }
                else
                {
                    LoadICode();
                    LoadIName();
                }

            }
            catch (Exception)
            {
            }

        }
        catch (Exception)
        {
        }
    }
    #endregion

    #region ddlItemName_SelectedIndexChanged
    protected void ddlItemName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlItemName.SelectedIndex != 0)
            {
                ddlItemCode.SelectedValue = ddlItemName.SelectedValue;
                DataTable dt1 = CommonClasses.Execute("Select ITEM_UNIT_MASTER.I_UOM_CODE,I_UOM_NAME,I_UWEIGHT,I_CURRENT_BAL,I_SCAT_CODE from ITEM_UNIT_MASTER,ITEM_MASTER where ITEM_MASTER.I_UOM_CODE=ITEM_UNIT_MASTER.I_UOM_CODE and ITEM_MASTER.ES_DELETE=0 and I_CODE='" + ddlItemCode.SelectedValue + "'");
                //DataTable dtPO = CommonClasses.Execute("select distinct(CPOM_CODE),CPOM_PONO from CUSTPO_MASTER,CUSTPO_DETAIL where CPOD_I_CODE='" + ddlItemCode.SelectedValue + "' and CPOM_P_CODE='" + ddlCustomer.SelectedValue + "' and ES_DELETE=0 and CPOM_TYPE=-2147483648");
                DataTable dtPO = new DataTable();
                if (Request.QueryString[0].Equals("MODIFY"))
                {
                    dtPO = CommonClasses.Execute("SELECT DISTINCT(CPOM_CODE),CPOM_PONO FROM CUSTPO_MASTER,CUSTPO_DETAIL,INVOICE_DETAIL WHERE CPOM_CODE=CPOD_CPOM_CODE and CPOD_I_CODE='" + ddlItemCode.SelectedValue + "' AND CPOM_P_CODE='" + ddlCustomer.SelectedValue + "' AND CUSTPO_MASTER.ES_DELETE=0   and IND_INM_CODE=" + Convert.ToInt32(ViewState["mlCode"].ToString()) + " --and CPOM_TYPE=-2147483648");
                }
                else
                {
                    dtPO = CommonClasses.Execute("SELECT DISTINCT(CPOM_CODE),CPOM_PONO FROM CUSTPO_MASTER,CUSTPO_DETAIL WHERE CPOM_CODE=CPOD_CPOM_CODE and CPOD_I_CODE='" + ddlItemCode.SelectedValue + "' AND CPOM_P_CODE='" + ddlCustomer.SelectedValue + "' AND ES_DELETE=0 and   ((CPOD_ORD_QTY-CPOD_DISPACH)>0 OR CPOD_ORD_QTY=0 ) --and CPOM_TYPE=-2147483648");
                }
                //ddlPONo.Items.Clear();
                //ddlPONo.SelectedIndex = -1;
                //ddlPONo.SelectedValue = null;
                //ddlPONo.ClearSelection();

                ddlPONo.DataSource = dtPO;
                ddlPONo.DataTextField = "CPOM_PONO";
                ddlPONo.DataValueField = "CPOM_CODE";
                ddlPONo.DataBind();
                //ddlPONo.Items.Insert(0, new ListItem("Select PO", "0"));
                ddlPONo_SelectedIndexChanged(null, null);
                //ddlPONo.SelectedIndexChanged(null, null);

                if (dt1.Rows.Count > 0)
                {
                    txtUOM.Text = dt1.Rows[0]["I_UOM_NAME"].ToString();
                    txtActWght.Text = dt1.Rows[0]["I_UWEIGHT"].ToString();
                    txtStockQty.Text = dt1.Rows[0]["I_CURRENT_BAL"].ToString();
                    ddlTariff.SelectedValue = dt1.Rows[0]["I_SCAT_CODE"].ToString();
                    txtVQty.Text = "0.00";
                }
                else
                {
                    txtUOM.Text = "";
                    txtVQty.Text = "0.00";
                }
                DataTable DtRate = new DataTable();


            }
            //else
            //{
            //    ddlItemCode.SelectedValue = "0";

            //}
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Tax Invoice", "ddlItemName_SelectedIndexChanged", Ex.Message);
        }
    }
    #endregion

    #region ddlTaxName_SelectedIndexChanged
    protected void ddlTaxName_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlTaxName.SelectedIndex != 0)
        {
            DataTable dtSTaxPer = CommonClasses.Execute("SELECT cast(ISNULL(ST_SALES_TAX,0) as numeric(20,2)) as ST_SALES_TAX FROM SALES_TAX_MASTER WHERE ST_CODE=" + ddlTaxName.SelectedValue + "");
            //if (dtSTaxPer.Rows.Count > 0)
            //{
            //    txtSalesTaxPer.Text = dtSTaxPer.Rows[0]["ST_SALES_TAX"].ToString();
            //    // txtSalesTaxAmount.Text = string.Format("{0:0.00}", Math.Round((Convert.ToDouble(txtNetAmount.Text) * Convert.ToDouble(txtSalesTaxPer.Text) / 100), 0));
            //    //txtGrandAmt.Text = string.Format("{0:0.00}", Convert.ToDouble(txtGrandAmt.Text) + Convert.ToDouble(txtSalesTaxAmount.Text));
            //    GetTotal();
            //}
            txtSalesTaxPer.Text = "0.00";
            txtSalesTaxAmount.Text = "0.00";
        }
        else
        {
            txtSalesTaxPer.Text = "0.00";
            txtSalesTaxAmount.Text = "0.00";
        }
    }
    #endregion

    #region ddlTray_SelectedIndexChanged
    protected void ddlTray_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            DataTable dtTrayStock = CommonClasses.Execute("Select ITEM_UNIT_MASTER.I_UOM_CODE,I_UOM_NAME,I_UWEIGHT,I_CURRENT_BAL from ITEM_UNIT_MASTER,ITEM_MASTER where ITEM_MASTER.I_UOM_CODE=ITEM_UNIT_MASTER.I_UOM_CODE and ITEM_MASTER.ES_DELETE=0 and I_CODE='" + ddlTray.SelectedValue + "'");
            if (dtTrayStock.Rows.Count > 0)
            {
                txtTrayStock.Text = dtTrayStock.Rows[0]["I_CURRENT_BAL"].ToString();
            }
        }
        catch (Exception)
        {
        }
    }
    #endregion

    #region ViewRec
    private void ViewRec(string str)
    {
        try
        {
            txtDate.Attributes.Add("readonly", "readonly");
            txtIssuedate.Attributes.Add("readonly", "readonly");
            txtremovaldate.Attributes.Add("readonly", "readonly");
            txtLRDate.Attributes.Add("readonly", "readonly");
            dtInvoiceDetail.Clear();
            DataTable dtMast = CommonClasses.Execute("Select INM_INVOICE_TYPE, INM_G_AMT,INM_CODE,INM_NO,INM_DATE,INM_P_CODE,cast(isnull(INM_NET_AMT,0) as numeric(20,2)) as INM_NET_AMT,INM_CM_CODE,MODIFY,cast(isnull(INM_DISC,0) as numeric(20,2)) as INM_DISC,cast(isnull(INM_DISC_AMT,0) as numeric(20,2)) as INM_DISC_AMT,cast(isnull(INM_BEXCISE,0) as numeric(20,2)) as INM_BEXCISE,cast(isnull(INM_BE_AMT,0) as numeric(20,2)) as INM_BE_AMT,cast(isnull(INM_EDUC_CESS,0) as numeric(20,2)) as INM_EDUC_CESS,cast(isnull(INM_EDUC_AMT,0) as numeric(20,2)) as INM_EDUC_AMT,cast(isnull(INM_H_EDUC_CESS,0) as numeric(20,2)) as INM_H_EDUC_CESS,cast(isnull(INM_H_EDUC_AMT,0) as numeric(20,2)) as INM_H_EDUC_AMT,cast(isnull(INM_FREIGHT,0) as numeric(20,2)) as INM_FREIGHT,cast(isnull(INM_S_TAX,0) as numeric(20,2)) as INM_S_TAX,cast(isnull(INM_S_TAX_AMT,0) as numeric(20,2)) as INM_S_TAX_AMT,cast(isnull(INM_TAX_TCS,0) as numeric(20,2)) as INM_TAX_TCS,cast(isnull(INM_TAX_TCS_AMT,0) as numeric(20,2)) as INM_TAX_TCS_AMT,cast(isnull(INM_PACK_AMT,0) as numeric(20,2)) as INM_PACK_AMT,cast(isnull(INM_OTHER_AMT,0) as numeric(20,2)) as INM_OTHER_AMT,INM_T_CODE,INM_STO_LOC,INM_VEH_NO,INM_TRANSPORT,INM_ISSUE_DATE,INM_REMOVAL_DATE,INM_REMARK,INM_LR_NO,INM_LR_DATE,cast(isnull(INM_ACCESSIBLE_AMT,0) as numeric(20,2)) as INM_ACCESSIBLE_AMT,cast(isnull(INM_TAXABLE_AMT,0) as numeric(20,2)) as INM_TAXABLE_AMT,cast(isnull(INM_ROUNDING_AMT,0) as numeric(20,2)) as INM_ROUNDING_AMT,cast(isnull(INM_OTHER_AMT,0) as numeric(20,2)) as INM_OTHER_AMT,cast(isnull(INM_FREIGHT,0) as numeric(20,2)) as INM_FREIGHT,cast(isnull(INM_INSURANCE,0) as numeric(20,2)) as INM_INSURANCE,cast(isnull(INM_TRANS_AMT,0) as numeric(20,2)) as INM_TRANS_AMT,cast(isnull(INM_OCTRI_AMT,0)  as numeric(20,2)) as INM_OCTRI_AMT,isnull(INM_IS_SUPPLIMENT,0) as INM_IS_SUPPLIMENT,INM_ISSU_TIME,INM_REMOVEL_TIME,INM_TRAY_CODE,INM_TRAY_QTY,INM_ADDRESS_SELECTED  from INVOICE_MASTER where ES_DELETE=0 and INM_CM_CODE=" + (string)Session["CompanyCode"] + " and INM_CODE=" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "");

            if (dt.Rows.Count > 0)
            {
                ViewState["mlCode"] = Convert.ToInt32(dtMast.Rows[0]["INM_CODE"]); ;

                txtInvoiceNo.Text = dtMast.Rows[0]["INM_NO"].ToString();
                txtDate.Text = Convert.ToDateTime(dtMast.Rows[0]["INM_DATE"]).ToString("dd MMM yyyy");
                ddlCustomer.SelectedValue = dtMast.Rows[0]["INM_P_CODE"].ToString();
                ddlCustomer_SelectedIndexChanged(null, null);
                ddlInvoiceType.SelectedValue = dtMast.Rows[0]["INM_INVOICE_TYPE"].ToString();
                txtNetAmount.Text = string.Format("{0:0.00}", Convert.ToDouble(dtMast.Rows[0]["INM_NET_AMT"].ToString()));

                txtstroreloc.Text = string.Format("{0:0.00}", Convert.ToDouble(dtMast.Rows[0]["INM_STO_LOC"].ToString()));
                //dtMast.Rows[0]["INM_STO_LOC"].ToString();
                txtDiscPer.Text = string.Format("{0:0.00}", Convert.ToDouble(dtMast.Rows[0]["INM_DISC"].ToString()));
                txtDiscAmt.Text = string.Format("{0:0.00}", Convert.ToDouble(dtMast.Rows[0]["INM_DISC_AMT"].ToString()));

                txtPackAmt.Text = string.Format("{0:0.00}", Convert.ToDouble(dtMast.Rows[0]["INM_PACK_AMT"].ToString()));

                txtAccessableAmt.Text = string.Format("{0:0.00}", Convert.ToDouble(dtMast.Rows[0]["INM_ACCESSIBLE_AMT"].ToString()));

                txtBasicExcPer.Text = dtMast.Rows[0]["INM_BEXCISE"].ToString();
                txtBasicExcAmt.Text = string.Format("{0:0.00}", Convert.ToDouble(dtMast.Rows[0]["INM_BE_AMT"].ToString()));

                txtducexcper.Text = dtMast.Rows[0]["INM_EDUC_CESS"].ToString();
                txtEdueceAmt.Text = string.Format("{0:0.00}", Convert.ToDouble(dtMast.Rows[0]["INM_EDUC_AMT"].ToString()));

                txtSHEExcPer.Text = dtMast.Rows[0]["INM_H_EDUC_CESS"].ToString();
                txtSHEExcAmt.Text = string.Format("{0:0.00}", Convert.ToDouble(dtMast.Rows[0]["INM_H_EDUC_AMT"].ToString()));

                txtTaxableAmt.Text = string.Format("{0:0.00}", Convert.ToDouble(dtMast.Rows[0]["INM_TAXABLE_AMT"].ToString()));

                txtSalesTaxPer.Text = string.Format("{0:0.00}", Convert.ToDouble(dtMast.Rows[0]["INM_S_TAX"].ToString()));
                txtSalesTaxAmount.Text = string.Format("{0:0.00}", Convert.ToDouble(dtMast.Rows[0]["INM_S_TAX_AMT"].ToString()));

                txtOtherCharges.Text = dtMast.Rows[0]["INM_OTHER_AMT"].ToString();

                txtFreight.Text = dtMast.Rows[0]["INM_FREIGHT"].ToString();

                txtIncurance.Text = string.Format("{0:0.00}", Convert.ToDouble(dtMast.Rows[0]["INM_INSURANCE"].ToString()));

                txtTransportAmt.Text = string.Format("{0:0.00}", Convert.ToDouble(dtMast.Rows[0]["INM_TRANS_AMT"].ToString()));

                txtOctri.Text = string.Format("{0:0.00}", Convert.ToDouble(dtMast.Rows[0]["INM_OCTRI_AMT"].ToString()));

                txtTCSAmt.Text = string.Format("{0:0.00}", Convert.ToDouble(dtMast.Rows[0]["INM_TAX_TCS_AMT"].ToString()));

                txtRoundingAmt.Text = string.Format("{0:0.00}", Convert.ToDouble(dtMast.Rows[0]["INM_ROUNDING_AMT"].ToString()));

                txtGrandAmt.Text = string.Format("{0:0.00}", Convert.ToDouble(dtMast.Rows[0]["INM_G_AMT"].ToString()));

                ddlTaxName.SelectedValue = dtMast.Rows[0]["INM_T_CODE"].ToString();

                txtVechicleNo.Text = dtMast.Rows[0]["INM_VEH_NO"].ToString();
                txtTransport.Text = dtMast.Rows[0]["INM_TRANSPORT"].ToString();
                txtIssuedate.Text = Convert.ToDateTime(dtMast.Rows[0]["INM_ISSUE_DATE"]).ToString("dd MMM yyyy");
                txtremovaldate.Text = Convert.ToDateTime(dtMast.Rows[0]["INM_REMOVAL_DATE"]).ToString("dd MMM yyyy");
                chkIsSuppliement.Checked = Convert.ToBoolean(dtMast.Rows[0]["INM_IS_SUPPLIMENT"]);
                if (Convert.ToBoolean(dtMast.Rows[0]["INM_ADDRESS_SELECTED"]))
                {
                    rbtAdd2.Checked = true;
                }
                else
                {
                    rbtAdd1.Checked = true;
                }
                txtRemark.Text = dtMast.Rows[0]["INM_REMARK"].ToString();
                txtLRNo.Text = dtMast.Rows[0]["INM_LR_NO"].ToString();
                if (dtMast.Rows[0]["INM_LR_DATE"] != DBNull.Value)
                {
                    txtLRDate.Text = Convert.ToDateTime(dtMast.Rows[0]["INM_LR_DATE"]).ToString("dd MMM yyyy");
                }
                txtIssuetime.Text = dtMast.Rows[0]["INM_ISSU_TIME"].ToString();
                txtRemoveltime.Text = dtMast.Rows[0]["INM_REMOVEL_TIME"].ToString();
                ddlTray.SelectedValue = dtMast.Rows[0]["INM_TRAY_CODE"].ToString();
                txtTrayQty.Text = dtMast.Rows[0]["INM_TRAY_QTY"].ToString();
                TrayQty = Convert.ToDouble(dtMast.Rows[0]["INM_TRAY_QTY"].ToString());

                //DataTable dtTrayStock = CommonClasses.Execute("Select ITEM_UNIT_MASTER.I_UOM_CODE,I_UOM_NAME,I_UWEIGHT,I_CURRENT_BAL from ITEM_UNIT_MASTER,ITEM_MASTER where ITEM_MASTER.I_UOM_CODE=ITEM_UNIT_MASTER.I_UOM_CODE and ITEM_MASTER.ES_DELETE=0 and I_CODE='" + ddlTray.SelectedValue + "'");
                //if (dtTrayStock.Rows.Count >= 0)
                //{
                //    txtTrayStock.Text = (TrayQty + Convert.ToDouble(dtTrayStock.Rows[0]["I_CURRENT_BAL"].ToString())).ToString();
                //}
                txtTrayStock.Text = "0";


                btnSubmit.Visible = true;
                btnInsert.Visible = true;

                //dtInvoiceDetail = CommonClasses.Execute("select CPOD_I_CODE as ItemCode,I_CODENO as ShortName,cast((CPOD_ORD_QTY*CPOD_RATE) as numeric(20,2)) as Amount,CPOD_AMT,I_NAME as ItemName,ITEM_UNIT_MASTER.I_UOM_CODE as UnitCode,I_UOM_NAME as Unit,cast(CPOD_ORD_QTY as numeric(20,2))  as OrderQty,cast(CPOD_RATE as  numeric(10,2)) as Rate,CPOD_DESC as Description FROM CUSTPO_DETAIL,CUSTPO_MASTER,ITEM_MASTER,ITEM_UNIT_MASTER WHERE CPOD_I_CODE=I_CODE AND CPOM_CODE=CPOD_CPOM_CODE AND CUSTPO_MASTER.ES_DELETE=0 and ITEM_MASTER.I_UOM_CODE=ITEM_UNIT_MASTER.I_UOM_CODE  AND ITEM_MASTER.ES_DELETE=0  and CPOM_CM_COMP_ID=" + (string)Session["CompanyId"] + " and CPOM_CODE='" + mlCode + "' order by CPOD_I_CODE");
                //dtInvoiceDetail = CommonClasses.Execute("select DISTINCT IND_I_CODE,I_CODENO as IND_I_CODENO,I_NAME as IND_I_NAME,I_UOM_NAME as UOM,IND_CPOM_CODE as PO_CODE,CPOM_PONO as PO_NO,'0.000' as STOCK_QTY,cast((CPOD_ORD_QTY-CPOD_DISPACH) as numeric(10,3)) as PEND_QTY,cast(IND_INQTY as  numeric(10,3)) as INV_QTY,cast(I_UWEIGHT as  numeric(10,2)) as ACT_WGHT,cast(IND_RATE as numeric(20,2)) as RATE,cast(IND_INQTY*IND_RATE as numeric(20,2)) as AMT,IND_SUBHEADING,IND_BACHNO,IND_NO_PACK,IND_PAK_QTY,IND_EX_AMT as IND_EX_AMT,IND_E_CESS_AMT as IND_E_CESS_AMT,IND_SH_CESS_AMT as IND_SH_CESS_AMT from INVOICE_DETAIL,INVOICE_MASTER,ITEM_MASTER,ITEM_UNIT_MASTER,CUSTPO_DETAIL,CUSTPO_MASTER  where IND_INM_CODE=INM_CODE and IND_CPOM_CODE=CPOM_CODE AND IND_CPOM_CODE=CPOD_CPOM_CODE AND CUSTPO_MASTER.ES_DELETE=0 AND CPOD_DISPACH > 0 and ITEM_MASTER.I_UOM_CODE=ITEM_UNIT_MASTER.I_UOM_CODE and INVOICE_MASTER.ES_DELETE=0 and IND_I_CODE=I_CODE and INM_CODE='" + mlCode + "' ");
                //dtInvoiceDetail = CommonClasses.Execute("select DISTINCT IND_I_CODE,I_CODENO as IND_I_CODENO,I_NAME as IND_I_NAME,I_UOM_NAME as UOM,IND_CPOM_CODE as PO_CODE,CPOM_PONO as PO_NO,I_CURRENT_BAL as STOCK_QTY,cast((CPOD_ORD_QTY-CPOD_DISPACH) as numeric(10,3)) as PEND_QTY,cast(IND_INQTY as  numeric(10,3)) as INV_QTY,cast(I_UWEIGHT as  numeric(10,2)) as ACT_WGHT,cast(IND_RATE as numeric(20,2)) as RATE,cast(IND_INQTY*IND_RATE as numeric(20,2)) as AMT,IND_SUBHEADING,IND_BACHNO,IND_NO_PACK,IND_PAK_QTY,IND_EX_AMT as IND_EX_AMT,IND_E_CESS_AMT as IND_E_CESS_AMT,IND_SH_CESS_AMT as IND_SH_CESS_AMT,IND_PACK_DESC ,ISNULL(IND_AMORTRATE,0) AS AmortRate,ISNULL(IND_AMORTAMT,0) AS AmortAmount,0 AS IND_E_CODE,IND_E_TARIFF_NO      from INVOICE_DETAIL,INVOICE_MASTER,ITEM_MASTER,ITEM_UNIT_MASTER,CUSTPO_DETAIL,CUSTPO_MASTER  where INM_CODE=IND_INM_CODE and IND_CPOM_CODE=CPOM_CODE AND  CPOM_CODE=CPOD_CPOM_CODE    and CPOD_I_CODE=I_CODE AND  ITEM_MASTER.I_UOM_CODE=ITEM_UNIT_MASTER.I_UOM_CODE  and  INVOICE_MASTER.ES_DELETE=0 and IND_I_CODE=I_CODE and INM_CODE='" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "' ");

                dtInvoiceDetail = CommonClasses.Execute("select DISTINCT IND_I_CODE,I_CODENO as IND_I_CODENO,I_NAME as IND_I_NAME,I_UOM_NAME as UOM,IND_CPOM_CODE as PO_CODE,CPOM_PONO as PO_NO,I_CURRENT_BAL as STOCK_QTY,cast((CPOD_ORD_QTY-CPOD_DISPACH) as numeric(10,3)) as PEND_QTY,cast(IND_INQTY as  numeric(10,3)) as INV_QTY,cast(I_UWEIGHT as  numeric(10,2)) as ACT_WGHT,cast(IND_RATE as numeric(20,2)) as RATE,cast(IND_INQTY*IND_RATE as numeric(20,2)) as AMT,IND_SUBHEADING,IND_BACHNO,IND_NO_PACK,IND_PAK_QTY,IND_EX_AMT as IND_EX_AMT,IND_E_CESS_AMT as IND_E_CESS_AMT,IND_SH_CESS_AMT as IND_SH_CESS_AMT,IND_PACK_DESC ,ISNULL(IND_AMORTRATE,0) AS AmortRate,ISNULL(IND_AMORTAMT,0) AS AmortAmount,0 AS IND_E_CODE,IND_HSN_CODE AS IND_E_TARIFF_NO      from INVOICE_DETAIL,INVOICE_MASTER,ITEM_MASTER,ITEM_UNIT_MASTER,CUSTPO_DETAIL,CUSTPO_MASTER  where INM_CODE=IND_INM_CODE and IND_CPOM_CODE=CPOM_CODE AND  CPOM_CODE=CPOD_CPOM_CODE    and CPOD_I_CODE=I_CODE AND  ITEM_MASTER.I_UOM_CODE=ITEM_UNIT_MASTER.I_UOM_CODE  and  INVOICE_MASTER.ES_DELETE=0 and IND_I_CODE=I_CODE and INM_CODE='" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "' ");
                LoadICode();
                LoadIName();
                LoadPO();
                if (dtInvoiceDetail.Rows.Count != 0)
                {
                    dgInvoiceAddDetail.DataSource = dtInvoiceDetail;
                    dgInvoiceAddDetail.DataBind();
                    ViewState["dt2"] = dtInvoiceDetail;
                    dgInvoiceAddDetail.Enabled = true;
                    //txtTotalAmt.Text = string.Format("{0:0.00}", sum1);
                    GetTotal();
                }

            }

            if (str == "VIEW")
            {
                //ddlPOType.Enabled = false;
                txtInvoiceNo.Enabled = false;
                txtDate.Enabled = false;
                ddlCustomer.Enabled = false;
                ddlItemCode.Enabled = false;
                ddlItemName.Enabled = false;
                txtAmount.Enabled = false;
                ddlPONo.Enabled = false;
                // txtDescription.Enabled=false;
                // txtEDUPer.Enabled=false;
                //  txtHEDUPer.Enabled=false;
                txtVQty.Enabled = false;
                txtRate.Enabled = false;
                txtVechicleNo.Enabled = false;
                txtTransport.Enabled = false;
                txtIssuedate.Enabled = false;
                txtremovaldate.Enabled = false;
                txtRemark.Enabled = false;
                txtLRNo.Enabled = false;
                txtLRDate.Enabled = false;

                txtDiscPer.Enabled = false;
                txtDiscAmt.Enabled = false;

                txtPackAmt.Enabled = false;

                ddlTaxName.Enabled = false;

                txtAccessableAmt.Enabled = false;
                txtTaxableAmt.Enabled = false;
                txtBasicExcPer.Enabled = false;
                txtBasicExcAmt.Enabled = false;

                // txtTaxName.Enabled = false;
                txtducexcper.Enabled = false;
                txtEdueceAmt.Enabled = false;

                txtSHEExcAmt.Enabled = false;
                txtSHEExcPer.Enabled = false;
                dgInvoiceAddDetail.Enabled = false;

                txtFreight.Enabled = false;
                txtOtherCharges.Enabled = false;
                txtIncurance.Enabled = false;
                txtTransportAmt.Enabled = false;
                txtOctri.Enabled = false;

                txtTCSAmt.Enabled = false;
                txtRoundingAmt.Enabled = false;

                btnSubmit.Visible = false;
                btnInsert.Visible = false;
                dgInvoiceAddDetail.Enabled = false;
                chkIsSuppliement.Enabled = false;
                txtPackDesc.Enabled = false;
                txtIssuetime.Enabled = false;
                txtRemoveltime.Enabled = false;
            }
            if (str == "MOD")
            {
                ddlCustomer.Enabled = false;
                chkIsSuppliement.Enabled = false;
                ddlInvoiceType.Enabled = false;
                rbtAdd1.Enabled = false;
                rbtAdd2.Enabled = false;
                CommonClasses.SetModifyLock("INVOICE_MASTER", "MODIFY", "INM_CODE", Convert.ToInt32(mlCode));
            }
            rbtAdd1.Enabled = false;
            rbtAdd2.Enabled = false;
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Tax Invoice", "ViewRec", Ex.Message);
        }
    }
    #endregion

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
            if ( no > 15)
            {
                 no = 15;
            }
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

    #region txtVQty_OnTextChanged
    protected void txtVQty_OnTextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txtVQty.Text == "")
            {
                txtVQty.Text = "0.000";
            }
            else
            {
                string totalStr = DecimalMasking(txtVQty.Text);

                txtVQty.Text = string.Format("{0:0.000}", Math.Round(Convert.ToDouble(totalStr), 3));
            }
            if (txtRate.Text == "")
            {
                txtRate.Text = "0.00";
            }
            else
            {
                string totalStr = DecimalMasking(txtRate.Text);

                txtRate.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(totalStr), 2));

                //txtRate.Text = string.Format("{0:0.00}", Convert.ToDouble(txtRate.Text));
            }

            if (txtAmortrate.Text == "")
            {
                txtAmortrate.Text = "0.00";
            }
            else
            {
                string totalStr = DecimalMasking(txtAmortrate.Text);

                txtAmortrate.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(totalStr), 2));

                //txtRate.Text = string.Format("{0:0.00}", Convert.ToDouble(txtRate.Text));
            }
            if (chkIsSuppliement.Checked == false)
            {
                if (txtPendingQty.Text.Trim() != "")
                {
                    //if (Convert.ToDouble(txtPendingQty.Text.Trim()) != 0)
                    //{
                    if (Convert.ToDouble(txtPendingQty.Text) < Convert.ToDouble(txtVQty.Text))
                    {
                        ////ShowMessage("#Avisos", "Invoice Qty Is Not Greater Than Pending Qty", CommonClasses.MSG_Warning);
                        //PanelMsg.Visible = true;
                        //lblmsg.Text = "Please Enter Correct Invoice Qty";
                        //ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        //txtVQty.Text = "0.00";
                        //txtVQty.Focus();
                        //return;
                        if (ddlInvoiceType.SelectedIndex == 0)
                        {
                            //DataTable dtCheckOpen = CommonClasses.Execute("SELECT CPOD_ORD_QTY FROM CUSTPO_DETAIL,BOM_MASTER,BOM_DETAIL  WHERE CPOD_I_CODE=BM_I_CODE AND BM_CODE=BD_BM_CODE and BD_I_CODE='" + ddlItemCode.SelectedValue + "' AND CPOD_CPOM_CODE='" + ddlPONo.SelectedValue + "' and CPOD_ORD_QTY>0");
                            //if (dtCheckOpen.Rows.Count > 0)
                            //{

                            //ShowMessage("#Avisos", "Select Customer", CommonClasses.MSG_Warning);
                            PanelMsg.Visible = true;
                            lblmsg.Text = "Quantity Should not Greater Than Pending Quantity...";
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                            txtVQty.Focus();
                            txtVQty.Text = "0";
                            return;
                            //}
                        }
                        else
                        {
                            //DataTable dtCheckOpen = CommonClasses.Execute("SELECT CPOD_ORD_QTY FROM CUSTPO_DETAIL WHERE CPOD_CPOM_CODE='" + ddlPONo.SelectedValue + "' AND CPOD_I_CODE='" + ddlItemCode.SelectedValue + "' and CPOD_ORD_QTY>0");
                            //if (dtCheckOpen.Rows.Count > 0)
                            //{

                            //ShowMessage("#Avisos", "Select Customer", CommonClasses.MSG_Warning);
                            PanelMsg.Visible = true;
                            lblmsg.Text = "Quantity Should not Greater Than Pending Quantity...";
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                            txtVQty.Focus();
                            txtVQty.Text = "0";
                            return;
                            //  }
                        }
                    }
                    //}
                    if (Convert.ToDouble(txtStockQty.Text) < Convert.ToDouble(txtVQty.Text))
                    {
                        //ShowMessage("#Avisos", "Invoice Qty Is Not Greater Than Pending Qty", CommonClasses.MSG_Warning);
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Please Enter Invoice Qty Less than stock";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        txtVQty.Text = "0.00";
                        txtVQty.Focus();
                        return;
                    }
                }
            }



            txtAmount.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(txtVQty.Text.ToString()) * Convert.ToDouble(txtRate.Text.ToString())), 2);

            txtAmortAmount.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(txtVQty.Text.ToString()) * Convert.ToDouble(txtAmortrate.Text.ToString())), 2);
            if (txtNoPackaeg.Text == "")
            {
                txtNoPackaeg.Text = "0";
            }
            if (Convert.ToDouble(txtNoPackaeg.Text) > 0)
            {
                double QtyPerPack = Math.Round(Convert.ToDouble(txtVQty.Text) / Convert.ToDouble(txtNoPackaeg.Text), 3);
                txtQtyPerPack.Text = string.Format("{0:0.000}", (QtyPerPack));
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Tax Incvoice", "txtVQty_OnTextChanged", Ex.Message);
        }
    }
    #endregion

    #region txtRate_TextChanged
    protected void txtRate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txtVQty.Text == "")
            {
                txtVQty.Text = "0.000";
            }
            else
            {
                string totalStr = DecimalMasking(txtVQty.Text);

                txtVQty.Text = string.Format("{0:0.000}", Math.Round(Convert.ToDouble(totalStr), 3));
                //txtVQty.Text = string.Format("{0:0.00}",Convert.ToDouble(txtVQty.Text));
            }
            if (txtRate.Text == "")
            {
                txtRate.Text = "0.000";
            }
            else
            {
                string totalStr = DecimalMasking(txtRate.Text);

                txtRate.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(totalStr), 2));
                // txtRate.Text = string.Format("{0:0.00}", Convert.ToDouble(txtRate.Text));
            }
            txtAmount.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(txtVQty.Text.ToString()) * Convert.ToDouble(txtRate.Text.ToString())), 2);
            //      txtAmount.Text = amount.ToString();
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Tax Invoice", "txtRate_TextChanged", Ex.Message.ToString());
        }
    }
    #endregion

    #region txtNoPackaeg_TextChanged
    protected void txtNoPackaeg_TextChanged(object sender, EventArgs e)
    {
        if (txtNoPackaeg.Text == "")
        {
            txtNoPackaeg.Text = "0";
        }
        // string totalStr = DecimalMasking(txtNoPackaeg.Text);

        //txtNoPackaeg.Text = string.Format("{0:0.000}", Math.Round(Convert.ToDouble(totalStr), 3));
        if (Convert.ToDouble(txtNoPackaeg.Text) > 0)
        {
            if (txtVQty.Text != "")
            {
                double QtyPerPack = Math.Round(Convert.ToDouble(txtVQty.Text) / Convert.ToDouble(txtNoPackaeg.Text), 3);
                txtQtyPerPack.Text = string.Format("{0:0.000}", (QtyPerPack));
            }
        }
    }
    #endregion

    #region btnInsert_Click
    protected void btnInsert_Click(object sender, EventArgs e)
    {
        try
        {

            Page.ClientScript.RegisterStartupScript(typeof(Page), "Test", "<script type='text/javascript'>VerificaCamposObrigatorios();</script>");
            //if (Convert.ToInt32(ViewState["RowCount"].ToString()) != 0)
            //{
            //    PanelMsg.Visible = true;
            //    lblmsg.Text = "Only One Item added in invoice";
            //    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            //    ddlCustomer.Focus();
            //    return;
            //}

            if (ddlCustomer.SelectedIndex == 0)
            {
                //ShowMessage("#Avisos", "Select Customer", CommonClasses.MSG_Warning);
                PanelMsg.Visible = true;
                lblmsg.Text = "Select Customer Name";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                ddlCustomer.Focus();
                return;

            }
            if (ddlItemCode.SelectedIndex == 0)
            {
                //ShowMessage("#Avisos", "Select Item Code", CommonClasses.MSG_Warning);
                PanelMsg.Visible = true;
                lblmsg.Text = "Select Item Code";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                ddlItemCode.Focus();
                return;
            }
            if (ddlItemName.SelectedIndex == 0)
            {
                //ShowMessage("#Avisos", "Select Item Name", CommonClasses.MSG_Warning);
                PanelMsg.Visible = true;
                lblmsg.Text = "Select Item Name";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                ddlItemName.Focus();
                return;
            }
            //if (ddlPONo.SelectedIndex == 0)
            //{
            //    // ShowMessage("#Avisos", "Select PO Number", CommonClasses.MSG_Warning);
            //    PanelMsg.Visible = true;
            //    lblmsg.Text = "Select PO Number";
            //    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            //    ddlPONo.Focus();
            //    return;
            //}
            if (chkIsSuppliement.Checked == false)
            {
                if (txtVQty.Text == "" || Convert.ToDouble(txtVQty.Text) == 0)
                {
                    //ShowMessage("#Avisos", "The Field 'Invoice Qty' Is Required", CommonClasses.MSG_Warning);                 
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Enter Invoice Qty";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    txtVQty.Focus();
                    return;
                }
                if (txtStockQty.Text == "" || txtStockQty.Text == "0.000")
                {
                    //ShowMessage("#Avisos", "The Field 'Pack Qty' Is Required", CommonClasses.MSG_Warning);
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Please check Stock  Qty";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    txtStockQty.Focus();
                    return;
                }

                if (txtPendingQty.Text == "" || Convert.ToDouble(txtPendingQty.Text) == 0)
                {
                    //ShowMessage("#Avisos", "The Field 'Pack Qty' Is Required", CommonClasses.MSG_Warning);
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Please check Stock  Qty";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    txtStockQty.Focus();
                    return;
                }
            }
            if (!chkIsSuppliement.Checked)
            {

            }

            if (dgInvoiceAddDetail.Enabled)
            {
                for (int i = 0; i < dgInvoiceAddDetail.Rows.Count; i++)
                {
                    string ITEM_CODE = ((Label)(dgInvoiceAddDetail.Rows[i].FindControl("lblIND_I_CODE"))).Text;
                    string PO_CODE = ((Label)(dgInvoiceAddDetail.Rows[i].FindControl("lblPO_CODE"))).Text;

                    if (ViewState["ItemUpdateIndex"].ToString() == "-1")
                    {

                        if (ITEM_CODE == ddlItemCode.SelectedValue.ToString() && PO_CODE == ddlPONo.SelectedValue.ToString())
                        {
                            //ShowMessage("#Avisos", "Record Already Exist For This Item In Table", CommonClasses.MSG_Info);

                            PanelMsg.Visible = true;
                            lblmsg.Text = "Record Already Exist For This Item In Table";
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);
                            return;
                        }
                    }
                    else
                    {
                        if (ITEM_CODE == ddlItemCode.SelectedValue.ToString() && ViewState["ItemUpdateIndex"].ToString() != i.ToString() && PO_CODE == ddlPONo.SelectedValue.ToString())
                        {

                            //ShowMessage("#Avisos", "Record Already Exist For This Item In Table", CommonClasses.MSG_Info);

                            PanelMsg.Visible = true;
                            lblmsg.Text = "Record Already Exist For This Item In Table";
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);

                            return;
                        }
                    }
                }
            }

            #region Add Data table coloums
            if (((DataTable)ViewState["dt2"]).Columns.Count == 0)
            {
                ((DataTable)ViewState["dt2"]).Columns.Add("IND_I_CODE");
                ((DataTable)ViewState["dt2"]).Columns.Add("IND_I_CODENO");
                ((DataTable)ViewState["dt2"]).Columns.Add("IND_I_NAME");
                ((DataTable)ViewState["dt2"]).Columns.Add("UOM");
                ((DataTable)ViewState["dt2"]).Columns.Add("PO_CODE");
                ((DataTable)ViewState["dt2"]).Columns.Add("PO_NO");
                ((DataTable)ViewState["dt2"]).Columns.Add("STOCK_QTY");
                ((DataTable)ViewState["dt2"]).Columns.Add("PEND_QTY");
                ((DataTable)ViewState["dt2"]).Columns.Add("INV_QTY");
                ((DataTable)ViewState["dt2"]).Columns.Add("ACT_WGHT");
                ((DataTable)ViewState["dt2"]).Columns.Add("RATE");
                ((DataTable)ViewState["dt2"]).Columns.Add("AMT");
                ((DataTable)ViewState["dt2"]).Columns.Add("AmortRate");
                ((DataTable)ViewState["dt2"]).Columns.Add("AmortAmount");

                ((DataTable)ViewState["dt2"]).Columns.Add("IND_SUBHEADING");
                ((DataTable)ViewState["dt2"]).Columns.Add("IND_BACHNO");
                ((DataTable)ViewState["dt2"]).Columns.Add("IND_NO_PACK");
                ((DataTable)ViewState["dt2"]).Columns.Add("IND_PAK_QTY");

                ((DataTable)ViewState["dt2"]).Columns.Add("IND_EX_AMT");
                ((DataTable)ViewState["dt2"]).Columns.Add("IND_E_CESS_AMT");
                ((DataTable)ViewState["dt2"]).Columns.Add("IND_SH_CESS_AMT");
                ((DataTable)ViewState["dt2"]).Columns.Add("IND_PACK_DESC");

                ((DataTable)ViewState["dt2"]).Columns.Add("IND_E_CODE");
                ((DataTable)ViewState["dt2"]).Columns.Add("IND_E_TARIFF_NO");
            }
            #endregion

            #region add control value to Dt
            dr = ((DataTable)ViewState["dt2"]).NewRow();
            dr["IND_I_CODE"] = ddlItemName.SelectedValue;
            dr["IND_I_CODENO"] = ddlItemCode.SelectedItem;
            dr["IND_I_NAME"] = ddlItemName.SelectedItem;
            dr["UOM"] = txtUOM.Text;
            dr["PO_CODE"] = ddlPONo.SelectedValue;
            dr["PO_NO"] = ddlPONo.SelectedItem;
            dr["STOCK_QTY"] = string.Format("{0:0.000}", (Convert.ToDouble(txtStockQty.Text == "" ? "0.00" : txtStockQty.Text) - Convert.ToDouble(txtVQty.Text)));
            //dr["PEND_QTY"] = string.Format("{0:0.000}", (Convert.ToDouble(txtPendingQty.Text) - Convert.ToDouble(txtVQty.Text)));
            dr["PEND_QTY"] = string.Format("{0:0.000}", ((Convert.ToDouble(txtPendingQty.Text == "" ? "0.00" : txtPendingQty.Text)) - Convert.ToDouble(txtVQty.Text)));
            dr["INV_QTY"] = string.Format("{0:0.000}", (Convert.ToDouble(txtVQty.Text == "" ? "0.00" : txtVQty.Text)));
            dr["ACT_WGHT"] = string.Format("{0:0.00}", (Convert.ToDouble(txtActWght.Text == "" ? "0.00" : txtActWght.Text)));
            dr["RATE"] = string.Format("{0:0.00}", (Convert.ToDouble(txtRate.Text == "" ? "0.00" : txtRate.Text)));
            dr["AMT"] = string.Format("{0:0.00}", (Convert.ToDouble(txtRate.Text) * Convert.ToDouble(txtVQty.Text)));
            dr["AmortRate"] = string.Format("{0:0.00}", (Convert.ToDouble(txtAmortrate.Text == "" ? "0.00" : txtAmortrate.Text)));
            dr["AmortAmount"] = string.Format("{0:0.00}", (Convert.ToDouble(txtAmortAmount.Text == "" ? "0.00" : txtAmortAmount.Text)));

            dr["IND_SUBHEADING"] = txtSubHeading.Text;
            dr["IND_BACHNO"] = txtBatchNo.Text;
            dr["IND_NO_PACK"] = txtNoPackaeg.Text;
            dr["IND_PAK_QTY"] = txtQtyPerPack.Text;
            dr["IND_PACK_DESC"] = txtPackDesc.Text;

            #region ExciseCalculation
            double EBasicPer = 0;
            double EEduCessPer = 0;
            double EHEduCessPer = 0;
            double EBasic = 0;
            double EEduCess = 0;
            double EHEduCess = 0;
            DataTable dtExcisePer = CommonClasses.Execute("SELECT E_BASIC,E_EDU_CESS,E_H_EDU FROM ITEM_MASTER,EXCISE_TARIFF_MASTER WHERE I_SCAT_CODE = E_CODE AND I_CODE=" + ddlItemCode.SelectedValue + "");
            if (dtExcisePer.Rows.Count > 0)
            {
                EBasicPer = Convert.ToDouble(dtExcisePer.Rows[0]["E_BASIC"].ToString());
                EEduCessPer = Convert.ToDouble(dtExcisePer.Rows[0]["E_EDU_CESS"].ToString());
                EHEduCessPer = Convert.ToDouble(dtExcisePer.Rows[0]["E_H_EDU"].ToString());
            }
            txtBasicExcPer.Text = string.Format("{0:0.00}", Convert.ToDouble(EBasicPer));
            txtducexcper.Text = string.Format("{0:0.00}", Convert.ToDouble(EEduCessPer));
            txtSHEExcPer.Text = string.Format("{0:0.00}", Convert.ToDouble(EHEduCessPer));


            DataTable dtCompState = CommonClasses.Execute("SELECT CM_STATE FROM COMPANY_MASTER WHERE CM_ID=" + (string)Session["CompanyId"] + "");
            DataTable dtPartyStae = CommonClasses.Execute("SELECT P_SM_CODE FROM PARTY_MASTER WHERE P_CODE='" + ddlCustomer.SelectedValue + "'");

            //if (chkIsSuppliement.Checked == false && Convert.ToDouble(txtBasicExcAmt.Text) == 0)
            //{
            if (dtCompState.Rows[0][0].ToString() == dtPartyStae.Rows[0][0].ToString())
            {
                EBasic = Math.Round((((Convert.ToDouble(txtRate.Text) * Convert.ToDouble(txtVQty.Text)) * EBasicPer) / 100), 2);
                EEduCess = Math.Round((Convert.ToDouble(txtRate.Text) * Convert.ToDouble(txtVQty.Text)) * EEduCessPer / 100, 2);
                EHEduCess = 0.00;
                txtBasicExcPer.Text = string.Format("{0:0.00}", Convert.ToDouble(EBasicPer));
                txtducexcper.Text = string.Format("{0:0.00}", Convert.ToDouble(EEduCessPer));
                txtSHEExcPer.Text = "0.00";
            }
            else
            {
                EBasic = 0.00;
                EEduCess = 0.00;
                EHEduCess = Math.Round((Convert.ToDouble(txtRate.Text) * Convert.ToDouble(txtVQty.Text)) * EHEduCessPer / 100, 2);
                txtSHEExcPer.Text = string.Format("{0:0.00}", Convert.ToDouble(EHEduCessPer));
                txtBasicExcPer.Text = "0.00";
                txtducexcper.Text = "0.00";
            }
            dr["IND_EX_AMT"] = EBasic;
            dr["IND_E_CESS_AMT"] = EEduCess;
            dr["IND_SH_CESS_AMT"] = EHEduCess;

            txtBasicExcAmt.Text = "0";

            dr["IND_E_CODE"] = ddlTariff.SelectedValue;
            dr["IND_E_TARIFF_NO"] = ddlTariff.SelectedItem;
            #endregion


            //dr["EBASIC"] = string.Format("{0:0.00}", (Convert.ToDouble(0)));
            //dr["EEDUCESS"] = string.Format("{0:0.00}", (Convert.ToDouble(0)));
            //dr["EHEDUCESS"] = string.Format("{0:0.00}", Convert.ToDouble(0));

            #endregion

            #region check Data table,insert or Modify Data
            if (str == "Modify")
            {
                if (((DataTable)ViewState["dt2"]).Rows.Count > 0)
                {
                    ((DataTable)ViewState["dt2"]).Rows.RemoveAt(Convert.ToInt32(ViewState["Index"].ToString()));
                    ((DataTable)ViewState["dt2"]).Rows.InsertAt(dr, Convert.ToInt32(ViewState["Index"].ToString()));
                    dgInvoiceAddDetail.Enabled = true;
                }
            }
            else
            {
                ((DataTable)ViewState["dt2"]).Rows.Add(dr);
                dgInvoiceAddDetail.Enabled = true;
            }
            #endregion


            #region Binding data to Grid
            dgInvoiceAddDetail.Visible = true;
            dgInvoiceAddDetail.Enabled = true;


            dgInvoiceAddDetail.DataSource = ((DataTable)ViewState["dt2"]);
            dgInvoiceAddDetail.DataBind();


            #endregion
            #region Tax
            //if (((DataTable)ViewState["dt2"]).Rows.Count == 1)
            //{
            //    DataTable dtTax = CommonClasses.Execute("select CPOD_ST_CODE from CUSTPO_DETAIL,CUSTPO_MASTER where CPOM_CODE='" + ddlPONo.SelectedValue + "' and CPOD_I_CODE='" + ddlItemName.SelectedValue + "' and CPOM_CODE=CPOD_CPOM_CODE");
            //    if (dtTax.Rows.Count > 0)
            //    {
            //        ddlTaxName.SelectedValue = dtTax.Rows[0]["CPOD_ST_CODE"].ToString();
            //        ddlTaxName_SelectedIndexChanged(null, null);
            //    }
            //}
            #endregion
            #region Clear Controles

            ClearControles();
            #endregion

            GetTotal();
            ViewState["RowCount"] = 1;
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Tax Invoice", "btnInsert_Click", Ex.Message);
        }
    }
    #endregion

    #region ClearControles
    void ClearControles()
    {
        ddlItemCode.SelectedIndex = 0;
        ddlItemName.SelectedIndex = 0;
        ddlPONo.SelectedIndex = 0;
        txtUOM.Text = "";
        txtStockQty.Text = "0.000";
        txtPendingQty.Text = "0.000";
        txtVQty.Text = "0.000";
        txtActWght.Text = "0.00";
        txtRate.Text = "0.00";
        txtAmount.Text = "0.00";
        txtSubHeading.Text = "";
        txtBatchNo.Text = "";
        txtNoPackaeg.Text = "0";
        txtQtyPerPack.Text = "0.00";
        txtPackDesc.Text = "";
        txtAmortAmount.Text = "0.000";
        txtAmortrate.Text = "0.000";
        str = "";
        ViewState["RowCount"] = 0;
    }
    #endregion

    #region GetTotal
    private void GetTotal()
    {
        double decTotal = 0;
        double ExcBasic = 0;
        double Excedu = 0;
        double ExcSH = 0;
        double AmortAmount = 0;
        try
        {

            if (dgInvoiceAddDetail.Enabled)
            {
                for (int i = 0; i < dgInvoiceAddDetail.Rows.Count; i++)
                {
                    string QED_AMT = ((Label)(dgInvoiceAddDetail.Rows[i].FindControl("lblAMT"))).Text;
                    string Amort = ((Label)(dgInvoiceAddDetail.Rows[i].FindControl("lblAmortAmount"))).Text;
                    string Basic = ((Label)(dgInvoiceAddDetail.Rows[i].FindControl("lblEBasic"))).Text;
                    string EduCess = ((Label)(dgInvoiceAddDetail.Rows[i].FindControl("lblEEcess"))).Text;
                    string SH = ((Label)(dgInvoiceAddDetail.Rows[i].FindControl("lblEHEcess"))).Text;

                    double Amount = Convert.ToDouble(QED_AMT);
                    decTotal = decTotal + Amount;
                    ExcBasic = ExcBasic + Convert.ToDouble(Basic);
                    Excedu = Excedu + Convert.ToDouble(EduCess);
                    ExcSH = ExcSH + Convert.ToDouble(SH);
                    AmortAmount = AmortAmount + Convert.ToDouble(Amort);
                }

            }
            else
            {
                ddlTaxName.SelectedIndex = 0;
            }
            txtNetAmount.Text = string.Format("{0:0.00}", Math.Round(decTotal, 2));

            //userd for amort
            txtstroreloc.Text = string.Format("{0:0.00}", Math.Round(AmortAmount, 2));
            if (dgInvoiceAddDetail.Enabled)
            {
                //string ICode = ((Label)(dgInvoiceAddDetail.Rows[0].FindControl("lblIND_I_CODE"))).Text;
                //DataTable dtExcisePer = CommonClasses.Execute("SELECT E_BASIC,E_EDU_CESS,E_H_EDU FROM ITEM_MASTER,EXCISE_TARIFF_MASTER WHERE I_E_CODE = E_CODE AND I_CODE=" + ICode + "");
                //if (dtExcisePer.Rows.Count > 0)
                //{
                //    EBasicPer = Convert.ToDouble(dtExcisePer.Rows[0]["E_BASIC"].ToString());
                //    EEduCessPer = Convert.ToDouble(dtExcisePer.Rows[0]["E_EDU_CESS"].ToString());
                //    EHEduCessPer = Convert.ToDouble(dtExcisePer.Rows[0]["E_H_EDU"].ToString());
                //}
            }
            else
            {
                txtNetAmount.Text = "0.00";
                txtDiscAmt.Text = "0.00";
                txtBasicExcAmt.Text = "0.00";
                txtEdueceAmt.Text = "0.00";
                txtSHEExcAmt.Text = "0.00";

            }
            if (txtNetAmount.Text == "")
            {
                txtNetAmount.Text = "0.00";
            }
            if (txtDiscAmt.Text == "")
            {
                txtDiscAmt.Text = "0.00";
            }
            if (txtDiscPer.Text == "")
            {
                txtDiscPer.Text = "0.00";
            }
            if (txtBasicExcAmt.Text == "")
            {
                txtBasicExcAmt.Text = "0.00";
            }
            if (txtBasicExcPer.Text == "")
            {
                txtBasicExcPer.Text = "0.00";
            }

            if (txtEdueceAmt.Text == "")
            {
                txtEdueceAmt.Text = "0.00";
            }
            if (txtSHEExcPer.Text == "")
            {
                txtSHEExcPer.Text = "0.00";
            }
            if (ddlTaxName.SelectedIndex == 0)
            {
                txtSalesTaxPer.Text = "0.00";
                txtSalesTaxAmount.Text = "0.00";
            }
            if (txtSalesTaxPer.Text == "")
            {
                txtSalesTaxPer.Text = "0.00";
            }

            if (txtSalesTaxAmount.Text == "")
            {
                txtSalesTaxAmount.Text = "0.00";
            }
            if (txtPackAmt.Text == "")
            {
                txtPackAmt.Text = "0.00";
            }

            if (txtGrandAmt.Text == "")
            {
                txtGrandAmt.Text = "0.00";
            }
            //txtBasicExcAmt.Text =string.Format("{0:0.00}", Math.Round(ExcBasic,2));
            //Discu Amount
            txtDiscAmt.Text = string.Format("{0:0.00}", Math.Round((Convert.ToDouble(txtNetAmount.Text) * Convert.ToDouble(txtDiscPer.Text) / 100), 2));
            //Packing Amount
            txtPackAmt.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(txtPackAmt.Text), 2));
            //Accessable Amount
            double AccessableValue = Convert.ToDouble(txtNetAmount.Text) + Convert.ToDouble(txtstroreloc.Text) - Convert.ToDouble(txtDiscAmt.Text) + Convert.ToDouble(txtPackAmt.Text);
            txtAccessableAmt.Text = AccessableValue.ToString();
            txtAccessableAmt.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(txtAccessableAmt.Text), 2));

            //Basic Excise Amt
            double ExcAmt = (Convert.ToDouble(txtAccessableAmt.Text) * (Convert.ToDouble(txtBasicExcPer.Text) / 100));
            txtBasicExcAmt.Text = ExcAmt.ToString();
            txtBasicExcAmt.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(txtBasicExcAmt.Text), 2));

            //Educational Excise Amt
            double EduExcAmt = (Convert.ToDouble(txtAccessableAmt.Text) * (Convert.ToDouble(txtducexcper.Text) / 100));
            txtEdueceAmt.Text = EduExcAmt.ToString();
            txtEdueceAmt.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(txtEdueceAmt.Text), 2));

            //HigherEducational Excise Amt
            double HEduExcAmt = (Convert.ToDouble(txtAccessableAmt.Text) * (Convert.ToDouble(txtSHEExcPer.Text) / 100));
            txtSHEExcAmt.Text = HEduExcAmt.ToString();
            txtSHEExcAmt.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(txtSHEExcAmt.Text), 2));

            //Taxable Amt
            double TaxableAmt = Convert.ToDouble(txtAccessableAmt.Text) - Convert.ToDouble(txtstroreloc.Text) + Convert.ToDouble(txtBasicExcAmt.Text) + Convert.ToDouble(txtEdueceAmt.Text) + Convert.ToDouble(txtSHEExcAmt.Text);
            txtTaxableAmt.Text = TaxableAmt.ToString();
            txtTaxableAmt.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(txtTaxableAmt.Text), 2));

            //Tax Amt
            double TaxAmt = (Convert.ToDouble(txtTaxableAmt.Text) * (Convert.ToDouble(txtSalesTaxPer.Text) / 100));
            txtSalesTaxAmount.Text = TaxAmt.ToString();
            txtSalesTaxAmount.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(txtSalesTaxAmount.Text), 2));

            //GrandAmount
            txtGrandAmt.Text = string.Format("{0:0.00}", Math.Round(((Convert.ToDouble(txtTaxableAmt.Text) + Convert.ToDouble(txtSalesTaxAmount.Text) + Convert.ToDouble(txtstroreloc.Text)) + Convert.ToDouble(txtOtherCharges.Text) + Convert.ToDouble(txtFreight.Text) + Convert.ToDouble(txtIncurance.Text) + Convert.ToDouble(txtTransportAmt.Text) + Convert.ToDouble(txtOctri.Text) + Convert.ToDouble(txtTCSAmt.Text) + Convert.ToDouble(txtRoundingAmt.Text)), 2));



            //txtEdueceAmt.Text=string.Format("{0:0.00}", Math.Round(Excedu,2));
            //txtSHEExcAmt.Text = string.Format("{0:0.00}", Math.Round(ExcSH,2));

            //double saletaxamt = ((Convert.ToDouble(txtNetAmount.Text) - Convert.ToDouble(txtDiscAmt.Text) + Convert.ToDouble(txtPackAmt.Text) + Convert.ToDouble(txtBasicExcAmt.Text) + Convert.ToDouble(txtEdueceAmt.Text) + Convert.ToDouble(txtSHEExcAmt.Text)) * (Convert.ToDouble(txtSalesTaxPer.Text) / 100));
            //txtSalesTaxAmount.Text = string.Format("{0:0.00}", Math.Round(saletaxamt, 2));

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Tax Incvoice", "GetTotal", Ex.Message);
        }
    }
    #endregion

    #region txtDiscPer_TextChanged
    protected void txtDiscPer_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txtDiscPer.Text == "")
            {
                txtDiscPer.Text = "0.00";
            }
            string totalStr = DecimalMasking(txtDiscPer.Text);
            txtDiscPer.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(totalStr), 2));

            txtDiscAmt.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(txtNetAmount.Text) * Convert.ToDouble(txtDiscPer.Text) / 100), 2);
            // CalcExise();
            GetTotal();
        }
        catch (Exception)
        {
        }
    }
    #endregion

    #region txtFreight_TextChanged
    protected void txtFreight_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txtFreight.Text == "")
            {
                txtFreight.Text = "0.00";
            }
            // CalcExise();
            string totalStr = DecimalMasking(txtFreight.Text);
            txtFreight.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(totalStr), 2));

            //txtFreight.Text = string.Format("{0:0.00}", Convert.ToDouble(txtFreight.Text));
            GetTotal();
        }
        catch (Exception)
        {
        }
    }
    #endregion

    #region txtIncurance_TextChanged
    protected void txtIncurance_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txtIncurance.Text == "")
            {
                txtIncurance.Text = "0.00";
            }
            // CalcExise();
            string totalStr = DecimalMasking(txtIncurance.Text);
            txtIncurance.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(totalStr), 2));

            // txtIncurance.Text = string.Format("{0:0.00}", Convert.ToDouble(txtIncurance.Text));
            GetTotal();
        }
        catch (Exception)
        {
        }
    }
    #endregion

    #region txtTransportAmt_TextChanged
    protected void txtTransportAmt_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txtTransportAmt.Text == "")
            {
                txtTransportAmt.Text = "0.00";
            }
            // CalcExise();
            string totalStr = DecimalMasking(txtTransportAmt.Text);
            txtTransportAmt.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(totalStr), 2));

            // txtTransportAmt.Text = string.Format("{0:0.00}", Convert.ToDouble(txtTransportAmt.Text));
            GetTotal();
        }
        catch (Exception)
        {
        }
    }
    #endregion

    #region txtOctri_TextChanged
    protected void txtOctri_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txtOctri.Text == "")
            {
                txtOctri.Text = "0.00";
            }
            // CalcExise();
            string totalStr = DecimalMasking(txtOctri.Text);
            txtOctri.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(totalStr), 2));
            //txtOctri.Text = string.Format("{0:0.00}", Convert.ToDouble(txtOctri.Text));
            GetTotal();
        }
        catch (Exception)
        {
        }
    }
    #endregion

    #region txtTaxableAmt_TextChanged
    protected void txtTaxableAmt_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txtTaxableAmt.Text == "")
            {
                txtTaxableAmt.Text = "0.00";
            }
            // CalcExise();
            string totalStr = DecimalMasking(txtTaxableAmt.Text);
            txtTaxableAmt.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(totalStr), 2));
            //txtOctri.Text = string.Format("{0:0.00}", Convert.ToDouble(txtOctri.Text));
            GetTotal();
        }
        catch (Exception)
        {
        }
    }
    #endregion

    #region txtTrayQty_OnTextChanged
    protected void txtTrayQty_OnTextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txtTrayQty.Text == "")
            {
                txtTrayQty.Text = "0.000";
            }
            else
            {
                string totalStr = DecimalMasking(txtTrayQty.Text);

                txtTrayQty.Text = string.Format("{0:0.000}", Math.Round(Convert.ToDouble(totalStr), 3));
            }
            if (txtTrayStock.Text != "")
            {
                if (Convert.ToDouble(txtTrayStock.Text) < Convert.ToDouble(txtTrayQty.Text))
                {
                    //ShowMessage("#Avisos", "Tray Qty Is Not Greater Than Tray Stock Qty", CommonClasses.MSG_Warning);
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Please Enter Correct Tray Qty";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    txtVQty.Text = "0.00";
                    txtVQty.Focus();
                    return;
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Tax Invoice", "txtTrayQty_OnTextChanged", Ex.Message);
        }
    }
    #endregion

    #region txtTCSAmt_TextChanged
    protected void txtTCSAmt_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txtTCSAmt.Text == "")
            {
                txtTCSAmt.Text = "0.00";
            }
            // CalcExise();
            string totalStr = DecimalMasking(txtTCSAmt.Text);
            txtTCSAmt.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(totalStr), 2));
            // txtTCSAmt.Text = string.Format("{0:0.00}", Convert.ToDouble(txtTCSAmt.Text));
            GetTotal();
        }
        catch (Exception)
        {
        }
    }
    #endregion

    #region txtRoundingAmt_TextChanged
    protected void txtRoundingAmt_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txtRoundingAmt.Text == "")
            {
                txtRoundingAmt.Text = "0.00";
            }
            // CalcExise();
            string totalStr = DecimalMasking(txtRoundingAmt.Text);
            txtRoundingAmt.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(totalStr), 2));
            GetTotal();
        }
        catch (Exception)
        {
        }
    }
    #endregion

    #region txtDiscAmt_TextChanged
    protected void txtDiscAmt_TextChanged(object sender, EventArgs e)
    {
        // CalcExise();
    }
    #endregion

    #region txtBasicExcPer_TextChanged
    protected void txtBasicExcPer_TextChanged(object sender, EventArgs e)
    {
        string totalStr = DecimalMasking(txtBasicExcPer.Text);
        txtBasicExcPer.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(totalStr), 2));
        // txtBasicExcPer.Text = string.Format("{0:0.00}", Convert.ToDouble(txtBasicExcPer.Text));
        GetTotal();
    }
    #endregion

    #region txtducexcper_TextChanged
    protected void txtducexcper_TextChanged(object sender, EventArgs e)
    {
        string totalStr = DecimalMasking(txtducexcper.Text);
        txtducexcper.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(totalStr), 2));
        // txtducexcper.Text = string.Format("{0:0.00}", Convert.ToDouble(txtducexcper.Text));
        GetTotal();
    }
    #endregion

    #region txtSHEExcPer_TextChanged
    protected void txtSHEExcPer_TextChanged(object sender, EventArgs e)
    {

        string totalStr = DecimalMasking(txtSHEExcPer.Text);
        txtSHEExcPer.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(totalStr), 2));
        //txtSHEExcPer.Text = string.Format("{0:0.00}", Convert.ToDouble(txtSHEExcPer.Text));
        GetTotal();
    }
    #endregion

    #region txtSalesTaxPer_TextChanged
    protected void txtSalesTaxPer_TextChanged(object sender, EventArgs e)
    {
        GetTotal();
    }
    #endregion

    #region txtPackAmt_TextChanged
    protected void txtPackAmt_TextChanged(object sender, EventArgs e)
    {
        string totalStr = DecimalMasking(txtPackAmt.Text);
        txtPackAmt.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(totalStr), 2));
        GetTotal();
    }
    #endregion

    #region txtOtherCharges_TextChanged
    protected void txtOtherCharges_TextChanged(object sender, EventArgs e)
    {
        string totalStr = DecimalMasking(txtOtherCharges.Text);
        txtOtherCharges.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(totalStr), 2));

        // txtOtherCharges.Text = string.Format("{0:0.00}",Convert.ToDouble(txtOtherCharges.Text));
        GetTotal();
    }
    #endregion

    #region dgInvoiceAddDetail_RowCommand
    protected void dgInvoiceAddDetail_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            ViewState["Index"] = Convert.ToInt32(e.CommandArgument.ToString());
            GridViewRow row = dgInvoiceAddDetail.Rows[Convert.ToInt32(ViewState["Index"].ToString())];

            if (e.CommandName == "Delete")
            {
                ViewState["RowCount"] = 0;
                int rowindex = row.RowIndex;
                dgInvoiceAddDetail.DeleteRow(rowindex);
                ((DataTable)ViewState["dt2"]).Rows.RemoveAt(rowindex);
                if (((DataTable)ViewState["dt2"]).Rows.Count > 0)
                {
                    dgInvoiceAddDetail.DataSource = ((DataTable)ViewState["dt2"]);
                    dgInvoiceAddDetail.DataBind();
                }
                else
                {
                    LoadFilterBlank();
                }
                if (dgInvoiceAddDetail.Rows.Count == 0)
                {
                    dgInvoiceAddDetail.Enabled = false;
                    GetTotal();
                    LoadFilter();
                }
                else
                {
                    GetTotal();
                }
            }
            if (e.CommandName == "Modify")
            {
                str = "Modify";
                ViewState["RowCount"] = 0;
                ViewState["ItemUpdateIndex"] = e.CommandArgument.ToString();
                //LoadICode();
                //LoadIName();
                ddlItemCode.SelectedValue = ((Label)(row.FindControl("lblIND_I_CODE"))).Text;
                ddlItemName.SelectedValue = ((Label)(row.FindControl("lblIND_I_CODE"))).Text;
                ddlItemCode_SelectedIndexChanged(null, null);
                //ddlItemCode_SelectedIndexChanged(null, null);
                txtUOM.Text = ((Label)(row.FindControl("lblUOM"))).Text;
                ddlPONo.SelectedValue = ((Label)(row.FindControl("lblPO_CODE"))).Text;
                txtStockQty.Text = string.Format("{0:0.000}", Math.Round((Convert.ToDouble(((Label)(row.FindControl("lblSTOCK_QTY"))).Text) + Convert.ToDouble(((Label)(row.FindControl("lblINV_QTY"))).Text)), 3));
                double pendQty = Convert.ToDouble(((Label)(row.FindControl("lblPEND_QTY"))).Text) + Convert.ToDouble(((Label)(row.FindControl("lblINV_QTY"))).Text);


                if (Request.QueryString[0].Equals("INSERT"))
                {
                    txtPendingQty.Text = string.Format("{0:0.000}", Math.Round((pendQty), 3));
                }
                else
                {
                    txtPendingQty.Text = string.Format("{0:0.000}", Math.Round(Convert.ToDouble(txtPendingQty.Text) + Convert.ToDouble(((Label)(row.FindControl("lblINV_QTY"))).Text), 3));
                }


                //DataTable dtQty = CommonClasses.Execute("SELECT (CPOD_ORD_QTY-CPOD_DISPACH) as Qty FROM CUSTPO_DETAIL where CPOD_I_CODE=" + ddlItemCode.SelectedValue + " and CPOD_CPOM_CODE=" + ddlPONo.SelectedValue + "");
                //if (dtQty.Rows.Count > 0)
                //{
                //   // txtRate.Text = string.Format("{0:0.00}", dt1.Rows[0]["CPOD_RATE"]);
                //    txtPendingQty.Text = string.Format("{0:0.000}", Math.Round(Convert.ToDouble(dtQty.Rows[0]["Qty"]), 3));
                //}
                txtVQty.Text = string.Format("{0:0.000}", Math.Round(Convert.ToDouble(((Label)(row.FindControl("lblINV_QTY"))).Text), 3));
                txtActWght.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(((Label)(row.FindControl("lblACT_WGHT"))).Text), 2));
                txtRate.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(((Label)(row.FindControl("lblRATE"))).Text), 2));
                txtAmount.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(((Label)(row.FindControl("lblAMT"))).Text), 2));
                txtSubHeading.Text = ((Label)(row.FindControl("lblSubHeading"))).Text;
                txtBatchNo.Text = ((Label)(row.FindControl("lblBatch"))).Text;
                txtNoPackaeg.Text = (Convert.ToInt32(((Label)(row.FindControl("lblNoPack"))).Text)).ToString();
                txtQtyPerPack.Text = string.Format("{0:0.00}", Math.Round(Convert.ToDouble(((Label)(row.FindControl("lblPakQty"))).Text), 2));
                txtPackDesc.Text = ((Label)(row.FindControl("lblIND_PACK_DESC"))).Text;

                foreach (GridViewRow gvr in dgInvoiceAddDetail.Rows)
                {
                    LinkButton lnkDelete = (LinkButton)(gvr.FindControl("lnkDelete"));
                    lnkDelete.Enabled = false;
                }
            }

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Tax Invoice", "dgMainPO_RowCommand", Ex.Message);
        }
    }
    #endregion

    #region dgInvoiceAddDetail_Deleting
    protected void dgInvoiceAddDetail_Deleting(object sender, GridViewDeleteEventArgs e)
    {

    }
    #endregion

    #region SaveRec
    bool SaveRec()
    {
        bool result = false;
        try
        {
            int IsSuppliement = 0, SeletedAddress = 0;
            if (chkIsSuppliement.Checked)
            {
                IsSuppliement = 1;
            }
            else
            {
                IsSuppliement = 0;
            }

            if (rbtAdd2.Checked)
            {
                SeletedAddress = 1;
            }
            else
            {
                SeletedAddress = 0;
            }
            if (Request.QueryString[0].Equals("INSERT"))
            {
                int Inv_No = 0;
                DataTable dt = new DataTable();
                string Invoice_No = "";
                if (rbtAdd2.Checked)
                {
                    dt = CommonClasses.Execute("Select isnull(max(INM_NO),0) as INM_NO FROM INVOICE_MASTER WHERE INM_CM_CODE = " + (string)Session["CompanyCode"] + "   AND INM_TYPE='OutJWINM' and ES_DELETE=0 and INM_ADDRESS_SELECTED=1");
                    if (dt.Rows.Count > 0)
                    {
                        Inv_No = Convert.ToInt32(dt.Rows[0]["INM_NO"]);
                        Inv_No = Inv_No + 1;
                        Invoice_No = CommonClasses.GenBillNo(Inv_No);
                        Invoice_No = "SUNB" + Invoice_No;
                    }
                }
                else
                {
                    dt = CommonClasses.Execute("Select isnull(max(INM_NO),0) as INM_NO FROM INVOICE_MASTER WHERE INM_CM_CODE = " + (string)Session["CompanyCode"] + "   AND INM_TYPE='OutJWINM' and ES_DELETE=0 and INM_ADDRESS_SELECTED=0");
                    if (dt.Rows.Count > 0)
                    {
                        Inv_No = Convert.ToInt32(dt.Rows[0]["INM_NO"]);
                        Inv_No = Inv_No + 1;
                        Invoice_No = CommonClasses.GenBillNo(Inv_No);
                        Invoice_No = "SUNLC" + Invoice_No;
                    }
                }
                if (CommonClasses.Execute1("INSERT INTO INVOICE_MASTER (INM_CM_CODE,INM_NO,INM_DATE,INM_INVOICE_TYPE,INM_P_CODE,INM_NET_AMT,INM_DISC,INM_DISC_AMT,INM_BEXCISE,INM_BE_AMT,INM_EDUC_CESS,INM_EDUC_AMT,INM_H_EDUC_CESS,INM_H_EDUC_AMT,INM_S_TAX,INM_S_TAX_AMT,INM_TAX_TCS,INM_TAX_TCS_AMT,INM_PACK_AMT,INM_G_AMT,INM_T_CODE,INM_STO_LOC,INM_VEH_NO,INM_TRANSPORT,INM_ISSUE_DATE,INM_REMOVAL_DATE,INM_REMARK,INM_LR_NO,INM_LR_DATE,INM_ACCESSIBLE_AMT,INM_TAXABLE_AMT,INM_ROUNDING_AMT,INM_OTHER_AMT,INM_FREIGHT,INM_INSURANCE,INM_TRANS_AMT,INM_OCTRI_AMT,INM_IS_SUPPLIMENT,INM_ISSU_TIME,INM_REMOVEL_TIME,INM_TRAY_CODE,INM_TRAY_QTY,INM_TYPE,INM_TNO,INM_ADDRESS_SELECTED)VALUES('" + Convert.ToInt32(Session["CompanyCode"]) + "','" + Inv_No + "','" + Convert.ToDateTime(txtDate.Text).ToString("dd/MMM/yyyy") + "','" + ddlInvoiceType.SelectedValue + "','" + ddlCustomer.SelectedValue + "','" + txtNetAmount.Text + "','" + txtDiscPer.Text + "','" + txtDiscAmt.Text + "','" + txtBasicExcPer.Text + "','" + txtBasicExcAmt.Text + "','" + txtducexcper.Text + "','" + txtEdueceAmt.Text + "','" + txtSHEExcPer.Text + "','" + txtSHEExcAmt.Text + "','" + txtSalesTaxPer.Text + "','" + txtSalesTaxAmount.Text + "','0','" + txtTCSAmt.Text + "','" + txtPackAmt.Text + "','" + txtGrandAmt.Text + "','" + ddlTaxName.SelectedValue + "','" + txtstroreloc.Text + "','" + txtVechicleNo.Text + "','" + txtTransport.Text + "','" + Convert.ToDateTime(txtIssuedate.Text).ToString("dd/MMM/yyyy") + "','" + Convert.ToDateTime(txtremovaldate.Text).ToString("dd/MMM/yyyy") + "','" + txtRemark.Text + "','" + txtLRNo.Text + "','" + Convert.ToDateTime(txtLRDate.Text).ToString("dd/MMM/yyyy") + "','" + txtAccessableAmt.Text + "','" + txtTaxableAmt.Text + "','" + txtRoundingAmt.Text + "','" + txtOtherCharges.Text + "','" + txtFreight.Text + "','" + txtIncurance.Text + "','" + txtTransportAmt.Text + "','" + txtOctri.Text + "','" + IsSuppliement + "','" + txtIssuetime.Text + "','" + txtRemoveltime.Text + "','" + ddlTray.SelectedValue + "','" + txtTrayQty.Text + "','OutJWINM','" + Invoice_No + "'," + SeletedAddress + ")"))
                {

                    string Code = CommonClasses.GetMaxId("Select Max(INM_CODE) from INVOICE_MASTER");

                    //Tray Stock Entry
                    CommonClasses.Execute1("INSERT INTO STOCK_LEDGER (STL_I_CODE,STL_DOC_NO,STL_DOC_NUMBER,STL_DOC_TYPE,STL_DOC_DATE,STL_DOC_QTY) VALUES ('" + ddlTray.SelectedValue + "','" + Code + "','" + Inv_No + "','OutJWINM','" + Convert.ToDateTime(txtDate.Text).ToString("dd/MMM/yyyy") + "','-" + txtTrayQty.Text + "')");
                    CommonClasses.Execute1("UPDATE ITEM_MASTER SET I_CURRENT_BAL=I_CURRENT_BAL-" + txtTrayQty.Text + ",I_ISSUE_DATE='" + Convert.ToDateTime(txtDate.Text).ToString("dd/MMM/yyyy") + "' WHERE I_CODE='" + ddlTray.SelectedValue + "'");


                    for (int i = 0; i < ((DataTable)ViewState["dt2"]).Rows.Count; i++)
                    {
                        result = CommonClasses.Execute1("INSERT INTO INVOICE_DETAIL(IND_INM_CODE,IND_I_CODE,IND_CPOM_CODE,IND_INQTY,IND_RATE,IND_AMT,IND_SUBHEADING,IND_BACHNO,IND_NO_PACK,IND_PAK_QTY,IND_EX_AMT,IND_E_CESS_AMT,IND_SH_CESS_AMT,IND_REFUNDABLE_QTY,IND_PACK_DESC,IND_AMORTRATE,IND_AMORTAMT,IND_HSN_CODE) values ('" + Code + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["IND_I_CODE"] + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["PO_CODE"] + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["INV_QTY"] + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["RATE"] + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["AMT"] + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["IND_SUBHEADING"] + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["IND_BACHNO"] + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["IND_NO_PACK"] + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["IND_PAK_QTY"] + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["IND_EX_AMT"] + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["IND_E_CESS_AMT"] + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["IND_SH_CESS_AMT"] + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["INV_QTY"] + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["IND_PACK_DESC"] + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["AmortRate"] + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["AmortAmount"] + "' ,'" + ((DataTable)ViewState["dt2"]).Rows[i]["IND_E_TARIFF_NO"] + "')");
                        //CommonClasses.Execute("update CUSTPO_DETAIL set CPOD_DISPACH = CPOD_DISPACH + " +  ((DataTable)ViewState["dt2"]).Rows[i]["INV_QTY"] + " where CPOD_CPOM_CODE='" +  ((DataTable)ViewState["dt2"]).Rows[i]["PO_CODE"] + "' and CPOD_I_CODE='" +  ((DataTable)ViewState["dt2"]).Rows[i]["IND_I_CODE"] + "'");

                        if (!chkIsSuppliement.Checked)
                        {
                            if (result == true)
                            {
                                // result = CommonClasses.Execute1("UPDATE CUSTPO_DETAIL SET CPOD_DISPACH = CPOD_DISPACH + " + ((DataTable)ViewState["dt2"]).Rows[i]["INV_QTY"] + " WHERE CPOD_CPOM_CODE='" + ((DataTable)ViewState["dt2"]).Rows[i]["PO_CODE"] + "' and CPOD_I_CODE='" + ((DataTable)ViewState["dt2"]).Rows[i]["IND_I_CODE"] + "'");
                            }
                            //Entry In Stock Ledger
                            if (result == true)
                            {
                                result = CommonClasses.Execute1("INSERT INTO STOCK_LEDGER (STL_I_CODE,STL_DOC_NO,STL_DOC_NUMBER,STL_DOC_TYPE,STL_DOC_DATE,STL_DOC_QTY) VALUES ('" + ((DataTable)ViewState["dt2"]).Rows[i]["IND_I_CODE"] + "','" + Code + "','" + Inv_No + "','OutJWINM','" + Convert.ToDateTime(txtDate.Text).ToString("dd/MMM/yyyy") + "','-" + ((DataTable)ViewState["dt2"]).Rows[i]["INV_QTY"] + "')");
                            }

                            #region Challan Entry
                            DataTable rowmateril = new DataTable();
                            DataTable dtchallan = new DataTable();
                            if (ddlInvoiceType.SelectedValue == "0")
                            {
                                dtchallan = CommonClasses.Execute("  SELECT ISNULL(SUM(CL_CQTY-CL_CON_QTY),0) as PENDING_QTY,CL_CH_NO,CL_DATE,CL_CODE,BD_VQTY+  BD_SCRAPQTY  AS BD_QTY,CL_I_CODE FROM CUSTPO_DETAIL,BOM_MASTER,BOM_DETAIL,CHALLAN_STOCK_LEDGER where CPOD_CPOM_CODE='" + ((DataTable)ViewState["dt2"]).Rows[i]["PO_CODE"] + "' AND BM_I_CODE='" + ((DataTable)ViewState["dt2"]).Rows[i]["IND_I_CODE"] + "' AND CPOD_I_CODE=BM_I_CODE AND BM_CODE=BD_BM_CODE AND BD_I_CODE=CL_I_CODE AND CL_P_CODE='" + ddlCustomer.SelectedValue + "'  AND CL_DOC_TYPE='IWIFP'  GROUP BY CL_CH_NO,CL_DATE,CL_CODE ,BD_VQTY, BD_SCRAPQTY ,CL_I_CODE  HAVING  SUM(CL_CQTY-CL_CON_QTY)>0 ");
                                rowmateril = CommonClasses.Execute(" SELECT BD_SCRAPQTY+BD_VQTY AS BD_QTY,BD_I_CODE FROM BOM_MASTER,BOM_DETAIL WHERE BM_CODE=BD_BM_CODE AND BM_I_CODE='" + ((DataTable)ViewState["dt2"]).Rows[i]["IND_I_CODE"] + "' AND BOM_MASTER.ES_DELETE=0");


                                //IWD_REV_QTY = Math.Round(IWD_REV_QTY * Convert.ToDouble(rowmateril.Rows[0]["BD_QTY"].ToString()));
                                //IWD_I_CODE = Convert.ToInt32(rowmateril.Rows[0]["CL_I_CODE"].ToString());
                            }
                            else
                            {
                                dtchallan = CommonClasses.Execute("SELECT  SUM(CL_CQTY-CL_CON_QTY) as PENDING_QTY,CL_CH_NO,CL_DATE,CL_CODE FROM CHALLAN_STOCK_LEDGER  where CL_P_CODE='" + ddlCustomer.SelectedValue + "' AND CL_I_CODE='" + ((DataTable)ViewState["dt2"]).Rows[i]["IND_I_CODE"] + "'  AND CL_DOC_TYPE='IWIFP'   GROUP BY CL_CH_NO,CL_DATE,CL_CODE   HAVING  SUM(CL_CQTY-CL_CON_QTY)>0 ");

                            }
                            double IWD_REV_QTY = Convert.ToDouble(((DataTable)ViewState["dt2"]).Rows[i]["INV_QTY"].ToString());
                            int IWD_I_CODE = Convert.ToInt32(((DataTable)ViewState["dt2"]).Rows[i]["IND_I_CODE"].ToString());
                            if (dtchallan.Rows.Count > 0)
                            {
                                if (ddlInvoiceType.SelectedValue == "0")
                                {
                                    double qty = Convert.ToDouble(((DataTable)ViewState["dt2"]).Rows[i]["INV_QTY"].ToString());

                                    for (int s = 0; s < rowmateril.Rows.Count; s++)
                                    {
                                        IWD_REV_QTY = Math.Round(IWD_REV_QTY * Convert.ToDouble(rowmateril.Rows[s]["BD_QTY"].ToString()));
                                        IWD_I_CODE = Convert.ToInt32(rowmateril.Rows[s]["BD_I_CODE"].ToString());
                                        double Bal_Qty = IWD_REV_QTY;
                                        for (int z = 0; z < dtchallan.Rows.Count; z++)
                                        {
                                            if (Bal_Qty > 0 && dtchallan.Rows[z]["CL_I_CODE"].ToString() == IWD_I_CODE.ToString())
                                            {
                                                if (Convert.ToDouble(dtchallan.Rows[z]["PENDING_QTY"].ToString()) >= Bal_Qty)
                                                {
                                                    CommonClasses.Execute("INSERT INTO CHALLAN_STOCK_LEDGER (CL_CH_NO,CL_DATE,CL_EX_NO,CL_P_CODE,CL_ASSY_CODE,CL_I_CODE,CL_PROCESS_CODE,CL_CQTY,CL_CON_QTY,CL_QTY_TEMP,CL_DOC_ID,CL_DOC_NO,CL_DOC_DATE,CL_DOC_TYPE,CL_REWORK_FLAG)VALUES('" + dtchallan.Rows[z]["CL_CH_NO"].ToString() + "','" + Convert.ToDateTime(dtchallan.Rows[z]["CL_DATE"].ToString()).ToString("dd/MMM/yyyy") + "','" + dtchallan.Rows[z]["CL_CH_NO"].ToString() + "','" + ddlCustomer.SelectedValue + "','" + IWD_I_CODE + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["IND_I_CODE"] + "',' 0 ','" + -(Bal_Qty) + "',0,0,'" + Code + "','" + Inv_No + "','" + Convert.ToDateTime(txtDate.Text).ToString("dd/MMM/yyyy") + "','OutJWINM',0)");
                                                    // CommonClasses.Execute(" UPDATE CHALLAN_STOCK_LEDGER SET CL_CON_QTY =CL_CON_QTY+'" + Bal_Qty + "' where CL_CH_NO='" + dtchallan.Rows[z]["CL_CH_NO"].ToString() + "' AND CL_DATE='" + Convert.ToDateTime(dtchallan.Rows[z]["CL_DATE"].ToString()).ToString("dd/MMM/yyyy") + "' AND CL_P_CODE='" + IWM_P_CODE + "' AND CL_I_CODE='" + IWD_I_CODE + "' AND CL_DOC_TYPE='OutSUBINM'");
                                                    CommonClasses.Execute(" UPDATE CHALLAN_STOCK_LEDGER SET CL_CON_QTY =CL_CON_QTY+'" + Bal_Qty + "' where  CL_CODE='" + dtchallan.Rows[z]["CL_CODE"].ToString() + "'");
                                                    Bal_Qty = 0;
                                                    break;
                                                }
                                                else
                                                {
                                                    Bal_Qty = IWD_REV_QTY - Convert.ToDouble(dtchallan.Rows[z]["PENDING_QTY"].ToString());
                                                    CommonClasses.Execute("INSERT INTO CHALLAN_STOCK_LEDGER (CL_CH_NO,CL_DATE,CL_EX_NO,CL_P_CODE,CL_ASSY_CODE,CL_I_CODE,CL_PROCESS_CODE,CL_CQTY,CL_CON_QTY,CL_QTY_TEMP,CL_DOC_ID,CL_DOC_NO,CL_DOC_DATE,CL_DOC_TYPE,CL_REWORK_FLAG)VALUES('" + dtchallan.Rows[z]["CL_CH_NO"].ToString() + "','" + Convert.ToDateTime(dtchallan.Rows[z]["CL_DATE"].ToString()).ToString("dd/MMM/yyyy") + "','" + dtchallan.Rows[z]["CL_CH_NO"].ToString() + "','" + ddlCustomer.SelectedValue + "','" + IWD_I_CODE + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["IND_I_CODE"] + "',' 0 ','" + -(Convert.ToDouble(dtchallan.Rows[z]["PENDING_QTY"].ToString())) + "',0,0,'" + Code + "','" + Inv_No + "','" + Convert.ToDateTime(txtDate.Text).ToString("dd/MMM/yyyy") + "','OutJWINM',0)");
                                                    //CommonClasses.Execute(" UPDATE CHALLAN_STOCK_LEDGER SET CL_CON_QTY =CL_CON_QTY+'" +  Convert.ToDouble(dtchallan.Rows[z]["PENDING_QTY"].ToString()) + "' where CL_CH_NO='" + dtchallan.Rows[z]["CL_CH_NO"].ToString() + "' AND CL_DATE='" + Convert.ToDateTime(dtchallan.Rows[z]["CL_DATE"].ToString()).ToString("dd/MMM/yyyy") + "' AND CL_P_CODE='" + IWM_P_CODE + "' AND CL_I_CODE='" + IWD_I_CODE + "' AND CL_DOC_TYPE='OutSUBINM'");
                                                    CommonClasses.Execute(" UPDATE CHALLAN_STOCK_LEDGER SET CL_CON_QTY =CL_CON_QTY+'" + Convert.ToDouble(dtchallan.Rows[z]["PENDING_QTY"].ToString()) + "' where  CL_CODE='" + dtchallan.Rows[z]["CL_CODE"].ToString() + "'");
                                                    IWD_REV_QTY = IWD_REV_QTY - Convert.ToDouble(dtchallan.Rows[z]["PENDING_QTY"].ToString());
                                                }
                                            }
                                        }
                                        IWD_REV_QTY = qty;
                                    }
                                }
                                else
                                {

                                    double Bal_Qty = IWD_REV_QTY;
                                    for (int z = 0; z < dtchallan.Rows.Count; z++)
                                    {
                                        if (Bal_Qty > 0)
                                        {
                                            if (Convert.ToDouble(dtchallan.Rows[z]["PENDING_QTY"].ToString()) >= Bal_Qty)
                                            {
                                                CommonClasses.Execute("INSERT INTO CHALLAN_STOCK_LEDGER (CL_CH_NO,CL_DATE,CL_EX_NO,CL_P_CODE,CL_ASSY_CODE,CL_I_CODE,CL_PROCESS_CODE,CL_CQTY,CL_CON_QTY,CL_QTY_TEMP,CL_DOC_ID,CL_DOC_NO,CL_DOC_DATE,CL_DOC_TYPE,CL_REWORK_FLAG)VALUES('" + dtchallan.Rows[z]["CL_CH_NO"].ToString() + "','" + Convert.ToDateTime(dtchallan.Rows[z]["CL_DATE"].ToString()).ToString("dd/MMM/yyyy") + "','" + dtchallan.Rows[z]["CL_CH_NO"].ToString() + "','" + ddlCustomer.SelectedValue + "','" + IWD_I_CODE + "','" + IWD_I_CODE + "',' 0 ','" + -(Bal_Qty) + "',0,0,'" + Code + "','" + Inv_No + "','" + Convert.ToDateTime(txtDate.Text).ToString("dd/MMM/yyyy") + "','OutJWINM',0)");
                                                // CommonClasses.Execute(" UPDATE CHALLAN_STOCK_LEDGER SET CL_CON_QTY =CL_CON_QTY+'" + Bal_Qty + "' where CL_CH_NO='" + dtchallan.Rows[z]["CL_CH_NO"].ToString() + "' AND CL_DATE='" + Convert.ToDateTime(dtchallan.Rows[z]["CL_DATE"].ToString()).ToString("dd/MMM/yyyy") + "' AND CL_P_CODE='" + IWM_P_CODE + "' AND CL_I_CODE='" + IWD_I_CODE + "' AND CL_DOC_TYPE='OutSUBINM'");
                                                CommonClasses.Execute(" UPDATE CHALLAN_STOCK_LEDGER SET CL_CON_QTY =CL_CON_QTY+'" + Bal_Qty + "' where  CL_CODE='" + dtchallan.Rows[z]["CL_CODE"].ToString() + "'");
                                                Bal_Qty = 0;
                                                break;
                                            }
                                            else
                                            {
                                                Bal_Qty = IWD_REV_QTY - Convert.ToDouble(dtchallan.Rows[z]["PENDING_QTY"].ToString());
                                                CommonClasses.Execute("INSERT INTO CHALLAN_STOCK_LEDGER (CL_CH_NO,CL_DATE,CL_EX_NO,CL_P_CODE,CL_ASSY_CODE,CL_I_CODE,CL_PROCESS_CODE,CL_CQTY,CL_CON_QTY,CL_QTY_TEMP,CL_DOC_ID,CL_DOC_NO,CL_DOC_DATE,CL_DOC_TYPE,CL_REWORK_FLAG)VALUES('" + dtchallan.Rows[z]["CL_CH_NO"].ToString() + "','" + Convert.ToDateTime(dtchallan.Rows[z]["CL_DATE"].ToString()).ToString("dd/MMM/yyyy") + "','" + dtchallan.Rows[z]["CL_CH_NO"].ToString() + "','" + ddlCustomer.SelectedValue + "','" + IWD_I_CODE + "','" + IWD_I_CODE + "',' 0 ','" + -(Convert.ToDouble(dtchallan.Rows[z]["PENDING_QTY"].ToString())) + "',0,0,'" + Code + "','" + Inv_No + "','" + Convert.ToDateTime(txtDate.Text).ToString("dd/MMM/yyyy") + "','OutJWINM',0)");
                                                //CommonClasses.Execute(" UPDATE CHALLAN_STOCK_LEDGER SET CL_CON_QTY =CL_CON_QTY+'" +  Convert.ToDouble(dtchallan.Rows[z]["PENDING_QTY"].ToString()) + "' where CL_CH_NO='" + dtchallan.Rows[z]["CL_CH_NO"].ToString() + "' AND CL_DATE='" + Convert.ToDateTime(dtchallan.Rows[z]["CL_DATE"].ToString()).ToString("dd/MMM/yyyy") + "' AND CL_P_CODE='" + IWM_P_CODE + "' AND CL_I_CODE='" + IWD_I_CODE + "' AND CL_DOC_TYPE='OutSUBINM'");
                                                CommonClasses.Execute(" UPDATE CHALLAN_STOCK_LEDGER SET CL_CON_QTY =CL_CON_QTY+'" + Convert.ToDouble(dtchallan.Rows[z]["PENDING_QTY"].ToString()) + "' where  CL_CODE='" + dtchallan.Rows[z]["CL_CODE"].ToString() + "'");
                                                IWD_REV_QTY = IWD_REV_QTY - Convert.ToDouble(dtchallan.Rows[z]["PENDING_QTY"].ToString());
                                            }
                                        }
                                    }

                                }
                            }
                            #endregion

                            //
                            //Removing Stock
                            if (result == true)
                            {
                                result = CommonClasses.Execute1("UPDATE ITEM_MASTER SET I_CURRENT_BAL=I_CURRENT_BAL-" + ((DataTable)ViewState["dt2"]).Rows[i]["INV_QTY"] + ",I_ISSUE_DATE='" + Convert.ToDateTime(txtDate.Text).ToString("dd/MMM/yyyy") + "' WHERE I_CODE='" + ((DataTable)ViewState["dt2"]).Rows[i]["IND_I_CODE"] + "'");
                            }
                        }


                    }

                    CommonClasses.WriteLog("Tax Invoice", "Save", "Tax Invoice", Convert.ToString(Inv_No), Convert.ToInt32(Code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                    result = true;
                    ((DataTable)ViewState["dt2"]).Rows.Clear();
                    Response.Redirect("~/Transactions/VIEW/ViewLabourChargeInvoice.aspx", false);

                }
                else
                {

                    ShowMessage("#Avisos", "Could not saved", CommonClasses.MSG_Warning);
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);

                    txtInvoiceNo.Focus();
                }

            }
            else if (Request.QueryString[0].Equals("MODIFY"))
            {
                if (CommonClasses.Execute1("UPDATE INVOICE_MASTER SET INM_CM_CODE=" + Session["CompanyCode"] + ",INM_NO='" + txtInvoiceNo.Text + "', INM_DATE='" + Convert.ToDateTime(txtDate.Text).ToString("dd/MMM/yyyy") + "',INM_P_CODE='" + ddlCustomer.SelectedValue + "',INM_NET_AMT='" + txtNetAmount.Text + "',INM_DISC='" + txtDiscPer.Text + "',INM_DISC_AMT='" + txtDiscAmt.Text + "',INM_BEXCISE='" + txtBasicExcPer.Text + "',INM_BE_AMT='" + txtBasicExcAmt.Text + "',INM_EDUC_CESS='" + txtducexcper.Text + "',INM_EDUC_AMT='" + txtEdueceAmt.Text + "',INM_H_EDUC_CESS='" + txtSHEExcPer.Text + "',INM_H_EDUC_AMT = '" + txtSHEExcAmt.Text + "',INM_S_TAX='" + txtSalesTaxPer.Text + "',INM_S_TAX_AMT='" + txtSalesTaxAmount.Text + "',INM_PACK_AMT='" + txtPackAmt.Text + "',INM_G_AMT='" + txtGrandAmt.Text + "',INM_T_CODE='" + ddlTaxName.SelectedValue + "',INM_STO_LOC='" + txtstroreloc.Text + "',INM_VEH_NO='" + txtVechicleNo.Text + "',INM_TRANSPORT='" + txtTransport.Text + "',INM_ISSUE_DATE='" + Convert.ToDateTime(txtIssuedate.Text).ToString("dd/MMM/yyyy") + "',INM_REMOVAL_DATE='" + Convert.ToDateTime(txtremovaldate.Text).ToString("dd/MMM/yyyy") + "',INM_REMARK='" + txtRemark.Text + "',INM_LR_NO='" + txtLRNo.Text + "',INM_LR_DATE='" + Convert.ToDateTime(txtLRDate.Text).ToString("dd/MMM/yyyy") + "',INM_ACCESSIBLE_AMT='" + txtAccessableAmt.Text + "',INM_TAXABLE_AMT='" + txtTaxableAmt.Text + "',INM_ROUNDING_AMT='" + txtRoundingAmt.Text + "',INM_OTHER_AMT='" + txtOtherCharges.Text + "',INM_FREIGHT='" + txtFreight.Text + "',INM_INSURANCE='" + txtIncurance.Text + "',INM_TRANS_AMT='" + txtTransportAmt.Text + "',INM_OCTRI_AMT='" + txtOctri.Text + "',INM_IS_SUPPLIMENT='" + IsSuppliement + "',INM_ISSU_TIME='" + txtIssuetime.Text + "',INM_REMOVEL_TIME='" + txtRemoveltime.Text + "' where INM_CODE='" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "'"))
                {
                    DataTable dtq = CommonClasses.Execute("SELECT IND_INQTY,IND_I_CODE,IND_CPOM_CODE FROM INVOICE_DETAIL where IND_INM_CODE=" + Convert.ToInt32(ViewState["mlCode"].ToString()) + " ");

                    if (!chkIsSuppliement.Checked)
                    {

                        for (int i = 0; i < dtq.Rows.Count; i++)
                        {
                            CommonClasses.Execute("UPDATE ITEM_MASTER SET I_CURRENT_BAL=I_CURRENT_BAL+" + dtq.Rows[i]["IND_INQTY"] + " where I_CODE='" + dtq.Rows[i]["IND_I_CODE"] + "'");
                            DataTable stockLedger = new DataTable();
                            if (ddlInvoiceType.SelectedValue == "0")
                            {
                                stockLedger = CommonClasses.Execute("SELECT *  FROM CHALLAN_STOCK_LEDGER where CL_DOC_ID='" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "'  AND  CL_DOC_TYPE='OutJWINM' AND  CL_ASSY_CODE IN (SELECT BD_I_CODE FROM BOM_MASTER,BOM_DETAIL WHERE BOM_MASTER.ES_DELETE=0 AND BM_CODE=BD_BM_CODE AND BM_I_CODE='" + dtq.Rows[i]["IND_I_CODE"] + "') ");
                                for (int z = 0; z < stockLedger.Rows.Count; z++)
                                {
                                    CommonClasses.Execute(" UPDATE CHALLAN_STOCK_LEDGER SET CL_CON_QTY =CL_CON_QTY-'" + Math.Abs(Convert.ToDouble(stockLedger.Rows[z]["CL_CQTY"].ToString())) + "' where CL_CH_NO='" + stockLedger.Rows[z]["CL_CH_NO"].ToString() + "' AND CL_DATE='" + Convert.ToDateTime(stockLedger.Rows[z]["CL_DATE"].ToString()).ToString("dd/MMM/yyyy") + "' AND CL_P_CODE='" + ddlCustomer.SelectedValue + "' AND CL_I_CODE='" + stockLedger.Rows[z]["CL_ASSY_CODE"].ToString() + "' AND CL_DOC_TYPE='IWIFP'");
                                    CommonClasses.Execute("DELETE FROM CHALLAN_STOCK_LEDGER where CL_DOC_ID='" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "' and CL_DOC_TYPE='OutJWINM' AND CL_I_CODE='" + stockLedger.Rows[z]["CL_I_CODE"].ToString() + "'  AND CL_CODE='" + stockLedger.Rows[z]["CL_CODE"].ToString() + "' ");
                                }
                            }
                            else
                            {
                                stockLedger = CommonClasses.Execute("SELECT *  FROM CHALLAN_STOCK_LEDGER where CL_DOC_ID='" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "' and CL_DOC_TYPE='OutJWINM' AND  CL_I_CODE='" + dtq.Rows[i]["IND_I_CODE"] + "' ");
                                for (int z = 0; z < stockLedger.Rows.Count; z++)
                                {
                                    CommonClasses.Execute(" UPDATE CHALLAN_STOCK_LEDGER SET CL_CON_QTY =CL_CON_QTY-'" + Math.Abs(Convert.ToDouble(stockLedger.Rows[z]["CL_CQTY"].ToString())) + "' where CL_CH_NO='" + stockLedger.Rows[z]["CL_CH_NO"].ToString() + "' AND CL_DATE='" + Convert.ToDateTime(stockLedger.Rows[z]["CL_DATE"].ToString()).ToString("dd/MMM/yyyy") + "' AND CL_P_CODE='" + ddlCustomer.SelectedValue + "' AND CL_I_CODE='" + stockLedger.Rows[z]["CL_I_CODE"].ToString() + "' AND CL_DOC_TYPE='IWIFP'");
                                }
                                CommonClasses.Execute("DELETE FROM CHALLAN_STOCK_LEDGER where CL_DOC_ID='" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "' and CL_DOC_TYPE='OutJWINM' AND CL_I_CODE='" + dtq.Rows[i]["IND_I_CODE"] + "'  ");
                            }
                        }
                        result = CommonClasses.Execute1("DELETE FROM INVOICE_DETAIL WHERE IND_INM_CODE='" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "'");
                        result = CommonClasses.Execute1("DELETE FROM STOCK_LEDGER WHERE STL_DOC_NO='" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "' and STL_DOC_TYPE='OutJWINM'");
                        //Tray Current Balance Update
                        CommonClasses.Execute("UPDATE ITEM_MASTER SET I_CURRENT_BAL=I_CURRENT_BAL+" + TrayQty + " where I_CODE='" + ddlTray.SelectedValue + "'");

                    }
                    result = CommonClasses.Execute1("DELETE FROM INVOICE_DETAIL WHERE IND_INM_CODE='" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "'");

                    if (result)
                    {
                        // Tray Stock Entry
                        CommonClasses.Execute1("INSERT INTO STOCK_LEDGER (STL_I_CODE,STL_DOC_NO,STL_DOC_NUMBER,STL_DOC_TYPE,STL_DOC_DATE,STL_DOC_QTY) VALUES ('" + ddlTray.SelectedValue + "','" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "','" + txtInvoiceNo.Text + "','OutJWINM','" + Convert.ToDateTime(txtDate.Text).ToString("dd/MMM/yyyy") + "','-" + txtTrayQty + "')");
                        CommonClasses.Execute1("UPDATE ITEM_MASTER SET I_CURRENT_BAL=I_CURRENT_BAL-" + txtTrayQty.Text + ",I_ISSUE_DATE='" + Convert.ToDateTime(txtDate.Text).ToString("dd/MMM/yyyy") + "' WHERE I_CODE='" + ddlTray.SelectedValue + "'");

                        for (int i = 0; i < ((DataTable)ViewState["dt2"]).Rows.Count; i++)
                        {
                            result = CommonClasses.Execute1("INSERT INTO INVOICE_DETAIL (IND_INM_CODE,IND_I_CODE,IND_CPOM_CODE,IND_INQTY,IND_RATE,IND_AMT,IND_SUBHEADING,IND_BACHNO,IND_NO_PACK,IND_PAK_QTY,IND_EX_AMT,IND_E_CESS_AMT,IND_SH_CESS_AMT,IND_REFUNDABLE_QTY,IND_PACK_DESC,IND_AMORTRATE,IND_AMORTAMT,IND_HSN_CODE) values ('" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["IND_I_CODE"] + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["PO_CODE"] + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["INV_QTY"] + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["RATE"] + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["AMT"] + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["IND_SUBHEADING"] + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["IND_BACHNO"] + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["IND_NO_PACK"] + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["IND_PAK_QTY"] + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["IND_EX_AMT"] + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["IND_E_CESS_AMT"] + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["IND_SH_CESS_AMT"] + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["INV_QTY"] + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["IND_PACK_DESC"] + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["AmortRate"] + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["AmortAmount"] + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["IND_E_TARIFF_NO"] + "')");
                            //  CommonClasses.Execute("update CUSTPO_DETAIL set CPOD_DISPACH = CPOD_DISPACH + " + ((DataTable)ViewState["dt2"]).Rows[i]["INV_QTY"] + " where CPOD_CPOM_CODE='" + ((DataTable)ViewState["dt2"]).Rows[i]["PO_CODE"] + "' and CPOD_I_CODE='" + ((DataTable)ViewState["dt2"]).Rows[i]["IND_I_CODE"] + "'");

                            if (result == true)
                            {
                                // result = CommonClasses.Execute1("UPDATE CUSTPO_DETAIL SET CPOD_DISPACH = CPOD_DISPACH + " + ((DataTable)ViewState["dt2"]).Rows[i]["INV_QTY"] + " WHERE CPOD_CPOM_CODE='" + ((DataTable)ViewState["dt2"]).Rows[i]["PO_CODE"] + "' and CPOD_I_CODE='" + ((DataTable)ViewState["dt2"]).Rows[i]["IND_I_CODE"] + "'");
                            }
                            //Entry In Stock Ledger
                            if (!chkIsSuppliement.Checked)
                            {
                                if (result == true)
                                {
                                    result = CommonClasses.Execute1("INSERT INTO STOCK_LEDGER (STL_I_CODE,STL_DOC_NO,STL_DOC_NUMBER,STL_DOC_TYPE,STL_DOC_DATE,STL_DOC_QTY) VALUES ('" + ((DataTable)ViewState["dt2"]).Rows[i]["IND_I_CODE"] + "','" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "','" + txtInvoiceNo.Text + "','OutJWINM','" + Convert.ToDateTime(txtDate.Text).ToString("dd/MMM/yyyy") + "','-" + ((DataTable)ViewState["dt2"]).Rows[i]["INV_QTY"] + "')");

                                }
                                //Removing Stock
                                if (result == true)
                                {
                                    result = CommonClasses.Execute1("UPDATE ITEM_MASTER SET I_CURRENT_BAL=I_CURRENT_BAL-" + ((DataTable)ViewState["dt2"]).Rows[i]["INV_QTY"] + ",I_ISSUE_DATE='" + Convert.ToDateTime(txtDate.Text).ToString("dd/MMM/yyyy") + "' WHERE I_CODE='" + ((DataTable)ViewState["dt2"]).Rows[i]["IND_I_CODE"] + "'");
                                }

                                #region Challan Entry
                                DataTable rowmateril = new DataTable();
                                DataTable dtchallan = new DataTable();
                                if (ddlInvoiceType.SelectedValue == "0")
                                {
                                    dtchallan = CommonClasses.Execute("  SELECT ISNULL(SUM(CL_CQTY-CL_CON_QTY),0) as PENDING_QTY,CL_CH_NO,CL_DATE,CL_CODE,BD_VQTY+  BD_SCRAPQTY  AS BD_QTY,CL_I_CODE FROM CUSTPO_DETAIL,BOM_MASTER,BOM_DETAIL,CHALLAN_STOCK_LEDGER where CPOD_CPOM_CODE='" + ((DataTable)ViewState["dt2"]).Rows[i]["PO_CODE"] + "' AND BM_I_CODE='" + ((DataTable)ViewState["dt2"]).Rows[i]["IND_I_CODE"] + "' AND CPOD_I_CODE=BM_I_CODE AND BM_CODE=BD_BM_CODE AND BD_I_CODE=CL_I_CODE AND CL_P_CODE='" + ddlCustomer.SelectedValue + "'  AND CL_DOC_TYPE='IWIFP'  GROUP BY CL_CH_NO,CL_DATE,CL_CODE ,BD_VQTY, BD_SCRAPQTY ,CL_I_CODE  HAVING  SUM(CL_CQTY-CL_CON_QTY)>0 ");
                                    rowmateril = CommonClasses.Execute(" SELECT BD_SCRAPQTY+BD_VQTY AS BD_QTY,BD_I_CODE FROM BOM_MASTER,BOM_DETAIL WHERE BM_CODE=BD_BM_CODE AND BM_I_CODE='" + ((DataTable)ViewState["dt2"]).Rows[i]["IND_I_CODE"] + "' AND BOM_MASTER.ES_DELETE=0");


                                    //IWD_REV_QTY = Math.Round(IWD_REV_QTY * Convert.ToDouble(rowmateril.Rows[0]["BD_QTY"].ToString()));
                                    //IWD_I_CODE = Convert.ToInt32(rowmateril.Rows[0]["CL_I_CODE"].ToString());
                                }
                                else
                                {
                                    dtchallan = CommonClasses.Execute("SELECT  SUM(CL_CQTY-CL_CON_QTY) as PENDING_QTY,CL_CH_NO,CL_DATE,CL_CODE FROM CHALLAN_STOCK_LEDGER  where CL_P_CODE='" + ddlCustomer.SelectedValue + "' AND CL_I_CODE='" + ((DataTable)ViewState["dt2"]).Rows[i]["IND_I_CODE"] + "'  AND CL_DOC_TYPE='IWIFP'   GROUP BY CL_CH_NO,CL_DATE,CL_CODE   HAVING  SUM(CL_CQTY-CL_CON_QTY)>0 ");

                                }
                                double IWD_REV_QTY = Convert.ToDouble(((DataTable)ViewState["dt2"]).Rows[i]["INV_QTY"].ToString());
                                int IWD_I_CODE = Convert.ToInt32(((DataTable)ViewState["dt2"]).Rows[i]["IND_I_CODE"].ToString());
                                if (dtchallan.Rows.Count > 0)
                                {
                                    if (ddlInvoiceType.SelectedValue == "0")
                                    {
                                        double qty = Convert.ToDouble(((DataTable)ViewState["dt2"]).Rows[i]["INV_QTY"].ToString());

                                        for (int s = 0; s < rowmateril.Rows.Count; s++)
                                        {
                                            IWD_REV_QTY = Math.Round(IWD_REV_QTY * Convert.ToDouble(rowmateril.Rows[s]["BD_QTY"].ToString()));
                                            IWD_I_CODE = Convert.ToInt32(rowmateril.Rows[s]["BD_I_CODE"].ToString());
                                            double Bal_Qty = IWD_REV_QTY;
                                            for (int z = 0; z < dtchallan.Rows.Count; z++)
                                            {
                                                if (Bal_Qty > 0 && dtchallan.Rows[z]["CL_I_CODE"].ToString() == IWD_I_CODE.ToString())
                                                {
                                                    if (Convert.ToDouble(dtchallan.Rows[z]["PENDING_QTY"].ToString()) >= Bal_Qty)
                                                    {
                                                        CommonClasses.Execute("INSERT INTO CHALLAN_STOCK_LEDGER (CL_CH_NO,CL_DATE,CL_EX_NO,CL_P_CODE,CL_ASSY_CODE,CL_I_CODE,CL_PROCESS_CODE,CL_CQTY,CL_CON_QTY,CL_QTY_TEMP,CL_DOC_ID,CL_DOC_NO,CL_DOC_DATE,CL_DOC_TYPE,CL_REWORK_FLAG)VALUES('" + dtchallan.Rows[z]["CL_CH_NO"].ToString() + "','" + Convert.ToDateTime(dtchallan.Rows[z]["CL_DATE"].ToString()).ToString("dd/MMM/yyyy") + "','" + dtchallan.Rows[z]["CL_CH_NO"].ToString() + "','" + ddlCustomer.SelectedValue + "','" + IWD_I_CODE + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["IND_I_CODE"] + "',' 0 ','" + -(Bal_Qty) + "',0,0,'" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "','" + txtInvoiceNo.Text + "','" + Convert.ToDateTime(txtDate.Text).ToString("dd/MMM/yyyy") + "','OutJWINM',0)");
                                                        // CommonClasses.Execute(" UPDATE CHALLAN_STOCK_LEDGER SET CL_CON_QTY =CL_CON_QTY+'" + Bal_Qty + "' where CL_CH_NO='" + dtchallan.Rows[z]["CL_CH_NO"].ToString() + "' AND CL_DATE='" + Convert.ToDateTime(dtchallan.Rows[z]["CL_DATE"].ToString()).ToString("dd/MMM/yyyy") + "' AND CL_P_CODE='" + IWM_P_CODE + "' AND CL_I_CODE='" + IWD_I_CODE + "' AND CL_DOC_TYPE='OutSUBINM'");
                                                        CommonClasses.Execute(" UPDATE CHALLAN_STOCK_LEDGER SET CL_CON_QTY =CL_CON_QTY+'" + Bal_Qty + "' where  CL_CODE='" + dtchallan.Rows[z]["CL_CODE"].ToString() + "'");
                                                        Bal_Qty = 0;
                                                        break;
                                                    }
                                                    else
                                                    {
                                                        Bal_Qty = IWD_REV_QTY - Convert.ToDouble(dtchallan.Rows[z]["PENDING_QTY"].ToString());
                                                        CommonClasses.Execute("INSERT INTO CHALLAN_STOCK_LEDGER (CL_CH_NO,CL_DATE,CL_EX_NO,CL_P_CODE,CL_ASSY_CODE,CL_I_CODE,CL_PROCESS_CODE,CL_CQTY,CL_CON_QTY,CL_QTY_TEMP,CL_DOC_ID,CL_DOC_NO,CL_DOC_DATE,CL_DOC_TYPE,CL_REWORK_FLAG)VALUES('" + dtchallan.Rows[z]["CL_CH_NO"].ToString() + "','" + Convert.ToDateTime(dtchallan.Rows[z]["CL_DATE"].ToString()).ToString("dd/MMM/yyyy") + "','" + dtchallan.Rows[z]["CL_CH_NO"].ToString() + "','" + ddlCustomer.SelectedValue + "','" + IWD_I_CODE + "','" + ((DataTable)ViewState["dt2"]).Rows[i]["IND_I_CODE"] + "',' 0 ','" + -(Convert.ToDouble(dtchallan.Rows[z]["PENDING_QTY"].ToString())) + "',0,0,'" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "','" + txtInvoiceNo.Text + "','" + Convert.ToDateTime(txtDate.Text).ToString("dd/MMM/yyyy") + "','OutJWINM',0)");
                                                        //CommonClasses.Execute(" UPDATE CHALLAN_STOCK_LEDGER SET CL_CON_QTY =CL_CON_QTY+'" +  Convert.ToDouble(dtchallan.Rows[z]["PENDING_QTY"].ToString()) + "' where CL_CH_NO='" + dtchallan.Rows[z]["CL_CH_NO"].ToString() + "' AND CL_DATE='" + Convert.ToDateTime(dtchallan.Rows[z]["CL_DATE"].ToString()).ToString("dd/MMM/yyyy") + "' AND CL_P_CODE='" + IWM_P_CODE + "' AND CL_I_CODE='" + IWD_I_CODE + "' AND CL_DOC_TYPE='OutSUBINM'");
                                                        CommonClasses.Execute(" UPDATE CHALLAN_STOCK_LEDGER SET CL_CON_QTY =CL_CON_QTY+'" + Convert.ToDouble(dtchallan.Rows[z]["PENDING_QTY"].ToString()) + "' where  CL_CODE='" + dtchallan.Rows[z]["CL_CODE"].ToString() + "'");
                                                        IWD_REV_QTY = IWD_REV_QTY - Convert.ToDouble(dtchallan.Rows[z]["PENDING_QTY"].ToString());
                                                    }
                                                }
                                            }
                                            IWD_REV_QTY = qty;
                                        }
                                    }
                                    else
                                    {

                                        double Bal_Qty = IWD_REV_QTY;
                                        for (int z = 0; z < dtchallan.Rows.Count; z++)
                                        {
                                            if (Bal_Qty > 0)
                                            {
                                                if (Convert.ToDouble(dtchallan.Rows[z]["PENDING_QTY"].ToString()) >= Bal_Qty)
                                                {
                                                    CommonClasses.Execute("INSERT INTO CHALLAN_STOCK_LEDGER (CL_CH_NO,CL_DATE,CL_EX_NO,CL_P_CODE,CL_ASSY_CODE,CL_I_CODE,CL_PROCESS_CODE,CL_CQTY,CL_CON_QTY,CL_QTY_TEMP,CL_DOC_ID,CL_DOC_NO,CL_DOC_DATE,CL_DOC_TYPE,CL_REWORK_FLAG)VALUES('" + dtchallan.Rows[z]["CL_CH_NO"].ToString() + "','" + Convert.ToDateTime(dtchallan.Rows[z]["CL_DATE"].ToString()).ToString("dd/MMM/yyyy") + "','" + dtchallan.Rows[z]["CL_CH_NO"].ToString() + "','" + ddlCustomer.SelectedValue + "','" + IWD_I_CODE + "','" + IWD_I_CODE + "',' 0 ','" + -(Bal_Qty) + "',0,0,'" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "','" + txtInvoiceNo.Text + "','" + Convert.ToDateTime(txtDate.Text).ToString("dd/MMM/yyyy") + "','OutJWINM',0)");
                                                    // CommonClasses.Execute(" UPDATE CHALLAN_STOCK_LEDGER SET CL_CON_QTY =CL_CON_QTY+'" + Bal_Qty + "' where CL_CH_NO='" + dtchallan.Rows[z]["CL_CH_NO"].ToString() + "' AND CL_DATE='" + Convert.ToDateTime(dtchallan.Rows[z]["CL_DATE"].ToString()).ToString("dd/MMM/yyyy") + "' AND CL_P_CODE='" + IWM_P_CODE + "' AND CL_I_CODE='" + IWD_I_CODE + "' AND CL_DOC_TYPE='OutSUBINM'");
                                                    CommonClasses.Execute(" UPDATE CHALLAN_STOCK_LEDGER SET CL_CON_QTY =CL_CON_QTY+'" + Bal_Qty + "' where  CL_CODE='" + dtchallan.Rows[z]["CL_CODE"].ToString() + "'");
                                                    Bal_Qty = 0;
                                                    break;
                                                }
                                                else
                                                {
                                                    Bal_Qty = IWD_REV_QTY - Convert.ToDouble(dtchallan.Rows[z]["PENDING_QTY"].ToString());
                                                    CommonClasses.Execute("INSERT INTO CHALLAN_STOCK_LEDGER (CL_CH_NO,CL_DATE,CL_EX_NO,CL_P_CODE,CL_ASSY_CODE,CL_I_CODE,CL_PROCESS_CODE,CL_CQTY,CL_CON_QTY,CL_QTY_TEMP,CL_DOC_ID,CL_DOC_NO,CL_DOC_DATE,CL_DOC_TYPE,CL_REWORK_FLAG)VALUES('" + dtchallan.Rows[z]["CL_CH_NO"].ToString() + "','" + Convert.ToDateTime(dtchallan.Rows[z]["CL_DATE"].ToString()).ToString("dd/MMM/yyyy") + "','" + dtchallan.Rows[z]["CL_CH_NO"].ToString() + "','" + ddlCustomer.SelectedValue + "','" + IWD_I_CODE + "','" + IWD_I_CODE + "',' 0 ','" + -(Convert.ToDouble(dtchallan.Rows[z]["PENDING_QTY"].ToString())) + "',0,0,'" + Convert.ToInt32(ViewState["mlCode"].ToString()) + "','" + txtInvoiceNo.Text + "','" + Convert.ToDateTime(txtDate.Text).ToString("dd/MMM/yyyy") + "','OutJWINM',0)");
                                                    //CommonClasses.Execute(" UPDATE CHALLAN_STOCK_LEDGER SET CL_CON_QTY =CL_CON_QTY+'" +  Convert.ToDouble(dtchallan.Rows[z]["PENDING_QTY"].ToString()) + "' where CL_CH_NO='" + dtchallan.Rows[z]["CL_CH_NO"].ToString() + "' AND CL_DATE='" + Convert.ToDateTime(dtchallan.Rows[z]["CL_DATE"].ToString()).ToString("dd/MMM/yyyy") + "' AND CL_P_CODE='" + IWM_P_CODE + "' AND CL_I_CODE='" + IWD_I_CODE + "' AND CL_DOC_TYPE='OutSUBINM'");
                                                    CommonClasses.Execute(" UPDATE CHALLAN_STOCK_LEDGER SET CL_CON_QTY =CL_CON_QTY+'" + Convert.ToDouble(dtchallan.Rows[z]["PENDING_QTY"].ToString()) + "' where  CL_CODE='" + dtchallan.Rows[z]["CL_CODE"].ToString() + "'");
                                                    IWD_REV_QTY = IWD_REV_QTY - Convert.ToDouble(dtchallan.Rows[z]["PENDING_QTY"].ToString());
                                                }
                                            }
                                        }

                                    }
                                }
                                #endregion
                            }
                        }

                        //CommonClasses.Execute("UPDATE QUOTATION_ENTRY set QE_PO_FLAG = 1 where QE_CODE=" + ddlQuatation.SelectedValue + "");

                        CommonClasses.RemoveModifyLock("INVOICE_MASTER", "MODIFY", "INM_CODE", Convert.ToInt32(ViewState["mlCode"].ToString()));
                        CommonClasses.WriteLog("Tax Invoice", "Update", "Tax Invoice", Convert.ToString(Convert.ToInt32(ViewState["mlCode"].ToString())), Convert.ToInt32(ViewState["mlCode"].ToString()), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                        ((DataTable)ViewState["dt2"]).Rows.Clear();
                        result = true;
                    }


                    Response.Redirect("~/Transactions/VIEW/ViewLabourChargeInvoice.aspx", false);
                }
                else
                {
                    ShowMessage("#Avisos", "Invalid Update", CommonClasses.MSG_Warning);
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert1();", true);

                    //   txtPONo.Focus();
                }
            }
            ViewState["RowCount"] = 0;
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Tax Invoice", "SaveRec", ex.Message);
        }
        return result;
    }
    #endregion

    #region txtDate_TextChanged
    protected void txtDate_TextChanged(object sender, EventArgs e)
    {
        txtIssuedate.Text = txtDate.Text;
        txtremovaldate.Text = txtDate.Text;
    }
    #endregion

    #region chkIsSuppliement_CheckedChanged
    protected void chkIsSuppliement_CheckedChanged(object sender, EventArgs e)
    {
        if (chkIsSuppliement.Checked == true)
        {
            txtActWght.Enabled = true;
            txtActWght.ReadOnly = false;
            txtRate.Enabled = true;
            txtRate.ReadOnly = false;
            txtBasicExcPer.Enabled = true;
            txtducexcper.Enabled = true;
            txtSHEExcPer.Enabled = true;
            ddlTaxName.Enabled = true;
            //LoadFilter1();
        }
        else
        {
            txtActWght.Enabled = false;
            txtActWght.ReadOnly = true;
            txtRate.Enabled = false;
            txtRate.ReadOnly = true;
            txtBasicExcPer.Enabled = false;
            txtducexcper.Enabled = false;
            txtSHEExcPer.Enabled = false;
            ddlTaxName.Enabled = false;
            //LoadFilter1();
        }
    }
    #endregion

    #region LoadITariff
    private void LoadITariff()
    {
        try
        {
            dt = CommonClasses.Execute("SELECT E_CODE,E_TARIFF_NO FROM EXCISE_TARIFF_MASTER WHERE ES_DELETE=0 AND E_CM_COMP_ID=" + (string)Session["CompanyId"] + " AND   E_TALLY_GST_EXCISE=1 ORDER BY E_TARIFF_NO");
            ddlTariff.DataSource = dt;
            ddlTariff.DataTextField = "E_TARIFF_NO";
            ddlTariff.DataValueField = "E_CODE";
            ddlTariff.DataBind();
            ddlTariff.Items.Insert(0, new ListItem("Select Item SAC Code", "0"));
            ddlTariff.SelectedIndex = -1;
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("LoadIUnit", "LoadIUnit", Ex.Message);
        }

    }
    #endregion
}
