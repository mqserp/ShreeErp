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
using System.Xml.Linq;


public partial class RoportForms_VIEW_ViewSalesSummary : System.Web.UI.Page
{
    static string right = "";

    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='25'");
            right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();

            LoadCombos();
            ddlCustomerName.Enabled = false;
            //ddlUser.Enabled = false;
            ddlFinishedComponent.Enabled = false;
            txtFromDate.Enabled = false;
            txtToDate.Enabled = false;
            chkDateAll.Checked = true;
            chkAllCustomer.Checked = true;
            chkAllItem.Checked = true;
            //chkAllUser.Checked = true;

            txtFromDate.Text = DateTime.Today.ToString("dd MMM yyyy");
            txtToDate.Text = DateTime.Today.ToString("dd MMM yyyy");
        }

    }
    #endregion

    private void LoadItems()
    {
        try
        {
            string str = "";
            if (chkDateAll.Checked != true)
            {
                if (txtFromDate.Text != "" && txtToDate.Text != "")
                {
                    str = str + "INM_DATE between '" + txtFromDate.Text + "' and '" + txtToDate.Text + "' and ";
                }
            }
            DataTable dtItemDet = new DataTable();
            dtItemDet = CommonClasses.Execute("SELECT distinct ITEM_MASTER.I_CODENO +'-'+ ITEM_MASTER.I_NAME as I_NAME,ITEM_MASTER.I_CODE FROM INVOICE_MASTER INNER JOIN INVOICE_DETAIL ON INVOICE_MASTER.INM_CODE = INVOICE_DETAIL.IND_INM_CODE INNER JOIN ITEM_MASTER ON INVOICE_DETAIL.IND_I_CODE = ITEM_MASTER.I_CODE WHERE " + str + " (ITEM_MASTER.I_CM_COMP_ID = " + (string)Session["CompanyId"] + ") AND (ITEM_MASTER.ES_DELETE = 0) and (INVOICE_MASTER.ES_DELETE=0) order by I_NAME");

            ddlFinishedComponent.DataSource = dtItemDet;
            ddlFinishedComponent.DataTextField = "I_NAME";
            ddlFinishedComponent.DataValueField = "I_CODE";
            ddlFinishedComponent.DataBind();
            ddlFinishedComponent.Items.Insert(0, new ListItem("Select Item Name", "0"));
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Order", "LoatItems", Ex.Message);
        }
    }

    #region LoadCombos
    private void LoadCombos()
    {
        try
        {
            string str = "";
            if (chkDateAll.Checked != true)
            {
                if (txtFromDate.Text != "" && txtToDate.Text != "")
                {
                    str = str + "INM_DATE between '" + txtFromDate.Text + "' and '" + txtToDate.Text + "' and ";
                }
            }
            if (chkAllItem.Checked != true)
            {
                if (ddlFinishedComponent.SelectedIndex != 0)
                {
                    str = str + " I_CODE=" + ddlFinishedComponent.SelectedValue + " and ";
                }
            }
            DataTable custdet = new DataTable();
            custdet = CommonClasses.Execute("SELECT distinct PARTY_MASTER.P_CODE,PARTY_MASTER.P_NAME FROM ITEM_MASTER INNER JOIN INVOICE_DETAIL ON ITEM_MASTER.I_CODE = INVOICE_DETAIL.IND_I_CODE INNER JOIN INVOICE_MASTER ON INVOICE_DETAIL.IND_INM_CODE = INVOICE_MASTER.INM_CODE INNER JOIN PARTY_MASTER ON INVOICE_MASTER.INM_P_CODE = PARTY_MASTER.P_CODE where " + str + " ITEM_MASTER.ES_DELETE=0 and INVOICE_MASTER.ES_DELETE=0 and P_CM_COMP_ID=" + Session["CompanyId"].ToString() + " order by P_NAME");
            ddlCustomerName.DataSource = custdet;
            ddlCustomerName.DataTextField = "P_NAME";
            ddlCustomerName.DataValueField = "P_CODE";
            ddlCustomerName.DataBind();
            ddlCustomerName.Items.Insert(0, new ListItem("Select Customer", "0"));

            DataTable dtUserDet = new DataTable();

            dtUserDet = CommonClasses.Execute("select LG_DOC_NO,LG_U_NAME,LG_U_CODE from LOG_MASTER where LG_DOC_NO='Customer Order Master' and LG_CM_COMP_ID=" + (string)Session["CompanyId"] + "  group by LG_DOC_NO,LG_U_NAME,LG_U_CODE ");

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Order", "LoadCustomer", Ex.Message);
        }

    }
    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/Masters/ADD/SalesDefault.aspx", false);
        }
        catch (Exception)
        {

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
            CommonClasses.SendError("Customer Order", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region chkAllCustomer_CheckedChanged
    protected void chkAllCustomer_CheckedChanged(object sender, EventArgs e)
    {
        if (chkAllCustomer.Checked == true)
        {
            ddlCustomerName.SelectedIndex = 0;
            ddlCustomerName.Enabled = false;
        }
        else
        {
            ddlCustomerName.SelectedIndex = 0;
            ddlCustomerName.Enabled = true;
            ddlCustomerName.Focus();
        }
    }
    #endregion

    #region chkAllCustomer_CheckedChanged
    protected void chkType_CheckedChanged(object sender, EventArgs e)
    {
        if (chkType.Checked == true)
        {
            ddltype.SelectedIndex = 0;
            ddltype.Enabled = false;
        }
        else
        {
            ddltype.SelectedIndex = 0;
            ddltype.Enabled = true;
        }
    }
    #endregion

    #region chkDateAll_CheckedChanged
    protected void chkDateAll_CheckedChanged(object sender, EventArgs e)
    {
        if (chkDateAll.Checked == true)
        {
            LoadItems();
            LoadCombos();
            txtFromDate.Enabled = false;
            txtToDate.Enabled = false;
        }
        else
        {
            LoadItems();
            LoadCombos();
            txtFromDate.Enabled = true;
            txtToDate.Enabled = true;
            txtFromDate.Attributes.Add("readonly", "readonly");
            txtToDate.Attributes.Add("readonly", "readonly");
        }
    }
    #endregion

    #region chkAllItem_CheckedChanged
    protected void chkAllItem_CheckedChanged(object sender, EventArgs e)
    {
        if (chkAllItem.Checked == true)
        {
            LoadItems();
            LoadCombos();
            ddlFinishedComponent.SelectedIndex = 0;
            ddlFinishedComponent.Enabled = false;
        }
        else
        {
            LoadItems();
            //LoadCombos();
            ddlFinishedComponent.SelectedIndex = 0;
            ddlFinishedComponent.Enabled = true;
            ddlFinishedComponent.Focus();
        }
    }
    #endregion

    #region btnShow_Click
    protected void btnShow_Click(object sender, EventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(5, 1)), this, "For Print"))
            {
                if (chkDateAll.Checked == false)
                {
                    if (txtFromDate.Text == "" || txtToDate.Text == "")
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "The Field 'Date' is required ";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        return;
                    }
                }

                if (chkAllItem.Checked == false)
                {
                    if (ddlFinishedComponent.SelectedIndex == 0)
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Select Item ";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        ddlFinishedComponent.Focus();
                        return;
                    }
                }
                if (chkAllCustomer.Checked == false)
                {
                    if (ddlCustomerName.SelectedIndex == 0)
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Select Customer ";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        ddlCustomerName.Focus();
                        return;
                    }
                }

                string From = "";
                string To = "";
                if (txtFromDate.Text.Trim() != "" && txtToDate.Text.Trim() != "")
                {
                    From = Convert.ToDateTime(txtFromDate.Text).ToString("dd/MMM/yyyy");
                    To = Convert.ToDateTime(txtToDate.Text).ToString("dd/MMM/yyyy");
                }
                else
                {
                    From = Convert.ToDateTime(Session["OpeningDate"].ToString()).ToString("dd/MMM/yyyy");
                    To = Convert.ToDateTime(Session["ClosingDate"].ToString()).ToString("dd/MMM/yyyy");
                }

                string str1 = "";
                string str = "";

                #region Detail

                if (rbtType.SelectedIndex == 0)
                {
                    str1 = "Detail";
                    if (rbtGroup.SelectedIndex == 0)
                    {
                        str = "Datewise";

                    }
                    if (rbtGroup.SelectedIndex == 1)
                    {
                        str = "ItemWise";


                    }
                    if (rbtGroup.SelectedIndex == 2)
                    {
                        str = "CustWise";

                    }
                    if (rbtGroup.SelectedIndex == 3)
                    {
                        str = "CustItemWise";

                    }
                }
                #endregion

                #region Summary
                if (rbtType.SelectedIndex == 1)
                {
                    str1 = "Summary";

                    if (rbtGroup.SelectedIndex == 0)
                    {
                        str = "Datewise";


                    }
                    if (rbtGroup.SelectedIndex == 1)
                    {
                        str = "ItemWise";


                    }
                    if (rbtGroup.SelectedIndex == 2)
                    {
                        str = "CustWise";
                    }
                    if (rbtGroup.SelectedIndex == 3)
                    {
                        str = "CustItemWise";

                    }
                }

                #endregion

                string strCondition = "";
                if (chkDateAll.Checked == true)
                {
                    strCondition = " AND INM_DATE BETWEEN '" + Convert.ToDateTime(Session["OpeningDate"].ToString()).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(Session["ClosingDate"].ToString()).ToString("dd/MMM/yyyy") + "'   ";
                }
                else
                {
                    strCondition = " AND INM_DATE BETWEEN '" + Convert.ToDateTime(txtFromDate.Text).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(txtToDate.Text).ToString("dd/MMM/yyyy") + "'   ";

                }
                if (chkAllItem.Checked != true)
                {
                    if (ddlFinishedComponent.SelectedIndex > 0)
                    {
                        strCondition = strCondition + " AND I_CODE='" + ddlFinishedComponent.SelectedValue + "'";
                    }
                }
                if (chkAllCustomer.Checked != true)
                {
                    strCondition = strCondition + " AND INM_P_CODE= '" + ddlCustomerName.SelectedValue + "'";
                }
                if (chkAllCustomer.Checked != true)
                {
                    strCondition = strCondition + " AND INM_P_CODE= '" + ddlCustomerName.SelectedValue + "'";
                }
                //if (ddltype.SelectedValue == "0")
                //{
                //    strCondition = strCondition + " AND INM_TYPE= 'TAXINV'";
                //}
                //else if (ddltype.SelectedValue == "1")
                //{
                //    strCondition = strCondition + " AND INM_TYPE= 'TAXINV' AND INM_IS_SUPPLIMENT=1 ";
                //}
                //else if (ddltype.SelectedValue == "2")
                //{
                //    strCondition = strCondition + "   AND  INM_TYPE= 'TAXINV'  AND INM_IS_SUPPLIMENT=0 ";
                //}
                Response.Redirect("~/RoportForms/ADD/SalesSummary.aspx?Title=" + Title + "&FromDate=" + From + "&ToDate=" + To + "&datewise=" + str + "&detail=" + str1 + "&Cond=" + strCondition + "", false);

            }
        }

        catch (Exception Ex)
        {
            CommonClasses.SendError("Tax Invoice Register", "btnShow_Click", Ex.Message);
        }
    }
    #endregion

    protected void txtFromDate_TextChanged(object sender, EventArgs e)
    {
        LoadCombos();
        if (txtFromDate.Text != "" || txtToDate.Text != "")
        {

            DateTime fdate = Convert.ToDateTime(txtFromDate.Text);
            DateTime todate = Convert.ToDateTime(txtToDate.Text);
            if (fdate > todate)
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "From Date should be less than equal to To Date";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtFromDate.Text = DateTime.Today.ToString("dd MMM yyyy");
                txtFromDate.Focus();
                return;
            }
        }
    }

    protected void txtToDate_TextChanged(object sender, EventArgs e)
    {
        LoadCombos();
        if (txtFromDate.Text != "" || txtToDate.Text != "")
        {

            DateTime fdate = Convert.ToDateTime(txtFromDate.Text);
            DateTime todate = Convert.ToDateTime(txtToDate.Text);
            if (todate < fdate)
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "To Date should be greater than equal to From Date";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                txtToDate.Text = DateTime.Today.ToString("dd MMM yyyy");
                txtToDate.Focus();
                return;
            }
        }
    }

    protected void ddlFinishedComponent_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadCombos();
    }

}
