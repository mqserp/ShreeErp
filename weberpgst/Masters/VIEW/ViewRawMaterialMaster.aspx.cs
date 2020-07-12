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
using System.Data.SqlClient;


public partial class Masters_VIEW_ViewRawMaterialMaster : System.Web.UI.Page
{
    #region Vraible   
    RawMaterial_BL BL_RawMatrial = null;
    static string right = "";
    DataTable dtFilter = new DataTable();
    string c_type = "";
    #endregion

    #region Event

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
                if (Request.QueryString[0].Equals("sale"))
                {
                    c_type = "sale";
                }
                else if (Request.QueryString[0].Equals("purchase"))
                {
                    c_type = "purchase";
                }
                if (!IsPostBack)
                {


                    DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='11'");
                    right = dtRights.Rows.Count == 0 ? "000000" : dtRights.Rows[0][0].ToString();
                    LoadItem();
                    if (dgRawMaterial.Rows.Count == 0)
                    {
                        dtFilter.Clear();
                        dgRawMaterial.Enabled = false;
                        if (dtFilter.Columns.Count == 0)
                        {
                            dtFilter.Columns.Add(new System.Data.DataColumn("I_CODE", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("I_CODENO", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("I_NAME", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("I_CURRENT_BAL", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("I_UOM_NAME", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("I_CAT_NAME", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("I_INV_CAT", typeof(String)));
                            dtFilter.Rows.Add(dtFilter.NewRow());
                            dgRawMaterial.DataSource = dtFilter;
                            dgRawMaterial.DataBind();
                        }
                    }
                    //LoadStatus();


                    //LoadFilter();
                }
                txtString.Focus();

            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Item Master Master", "Page_Load", Ex.Message);
        }
    }
    #endregion    

    #region btnAddNew_Click
    protected void btnAddNew_Click(object sender, EventArgs e)
    {
        try
        {
            //if (CommonClasses.ValidRights(int.Parse(right.Substring(3, 1)), this, "For Add"))
            // {
            string type = "INSERT";
            Response.Redirect("~/Masters/ADD/RawMaterial.aspx?c_name=" + type + "&c_type=" + c_type, false);
            // }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Item Master Master", "btnAddNew_Click", Ex.Message);
        }
    }
    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            if (c_type == "sale")
            {
                Response.Redirect("~/Masters/ADD/MasterDefault.aspx", false);
            }
            else if (c_type == "purchase")
            {
                Response.Redirect("~/Masters/ADD/MasterDefault.aspx", false);
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Item Master", "btnCancel_Click", ex.Message);
        }
    }
    #endregion

    #region txtString_TextChanged
    protected void txtString_TextChanged(object sender, EventArgs e)
    {
        try
        {
            LoadStatus(txtString);
        }

        catch (Exception ex)
        {
            CommonClasses.SendError("Item Master", "btnSearch_Click", ex.Message);
        }
    }
    #endregion

    #region dgRawMaterial_RowDeleting
    protected void dgRawMaterial_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        if (CommonClasses.ValidRights(int.Parse(right.Substring(4, 1)), this, "For Delete"))
        {
            if (!ModifyLog(((Label)(dgRawMaterial.Rows[e.RowIndex].FindControl("lblI_CODE"))).Text))
            {

                string i_code = ((Label)(dgRawMaterial.Rows[e.RowIndex].FindControl("lblI_CODE"))).Text;
                string i_name = ((Label)(dgRawMaterial.Rows[e.RowIndex].FindControl("lblI_NAME"))).Text;
                string i_codeno = ((Label)(dgRawMaterial.Rows[e.RowIndex].FindControl("lblI_CODENO"))).Text;

                if (CommonClasses.CheckUsedInTran("SUPP_PO_MASTER,SUPP_PO_DETAILS", "SPOD_I_CODE", "AND SPOD_SPOM_CODE=SPOM_CODE and SUPP_PO_MASTER.ES_DELETE=0", i_code))
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "You cant delete this record it has used in Purchase Order";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    return;
                }
                if (CommonClasses.CheckUsedInTran("CUSTPO_MASTER,CUSTPO_DETAIL", "CPOD_I_CODE", "AND CPOD_CPOM_CODE=CPOM_CODE and CUSTPO_MASTER.ES_DELETE=0", i_code))
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "You cant delete this record it has used in Sales Order";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    return;
                }
                if (CommonClasses.CheckUsedInTran("ISSUE_MASTER,ISSUE_MASTER_DETAIL ", "IMD_I_CODE", " AND ISSUE_MASTER.IM_CODE=ISSUE_MASTER_DETAIL.IM_CODE AND ISSUE_MASTER.ES_DELETE=0", i_code))
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "You cant delete this record it has used in Issue To Production";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    return;
                }
                if (CommonClasses.CheckUsedInTran("PRODUCTION_TO_STORE_MASTER,PRODUCTION_TO_STORE_DETAIL ", "PSD_I_CODE", " AND PS_CODE=PSD_PS_CODE AND PRODUCTION_TO_STORE_MASTER.ES_DELETE=0", i_code))
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "You cant delete this record it has used in Production TO store";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    return;
                }
                bool flag = CommonClasses.Execute1("UPDATE ITEM_MASTER SET ES_DELETE = 1 WHERE I_CODE='" + Convert.ToInt32(i_code) + "'");
                if (flag == true)
                {
                    CommonClasses.WriteLog("item Master", "Delete", "item Master", i_name, Convert.ToInt32(i_code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                    //ShowMessage("#Avisos", CommonClasses.strRegDelSucesso, CommonClasses.MSG_Erro);
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Record Deleted Successfully";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                }
            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Record Used By Another Person";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                return;
            }
            LoadItem();
        }
        else
        {
            //ShowMessage("#Avisos", "You Have No Rights To Delete", CommonClasses.MSG_Erro);
            PanelMsg.Visible = true;
            lblmsg.Text = "You Have No Rights To Delete";
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
            return;
        }
    }
    #endregion

    #region dgRawMaterial_PageIndexChanging
    protected void dgRawMaterial_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            dgRawMaterial.PageIndex = e.NewPageIndex;
            //LoadItem();
            LoadStatus(txtString);
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Item Master", "dgRawMaterial_PageIndexChanging", ex.Message);
        }
    }
    #endregion

    #region dgRawMaterial_RowCommand
    protected void dgRawMaterial_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.Equals("View"))
            {
                if (CommonClasses.ValidRights(int.Parse(right.Substring(1, 1)), this, "For View"))
                {
                    if (!ModifyLog(e.CommandArgument.ToString()))
                    {

                        string type = "VIEW";
                        string i_code = e.CommandArgument.ToString();
                        Response.Redirect("~/Masters/ADD/RawMaterial.aspx?c_name=" + type + "&i_code=" + i_code + "&c_type=" + c_type, false);
                    }
                    else
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Record Used By Another Person";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                        //ShowMessage("#Avisos", "Record Used By Another Person", CommonClasses.MSG_Erro);
                        return;
                    }

                }
                else
                {
                    //ShowMessage("#Avisos", "You Have No Rights To View", CommonClasses.MSG_Erro);
                    PanelMsg.Visible = true;
                    lblmsg.Text = "You Have No Rights To View";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                    return;
                }
            }
            if (e.CommandName.Equals("Modify"))
            {
                if (CommonClasses.ValidRights(int.Parse(right.Substring(2, 1)), this, "For Modify"))
                {
                    if (!ModifyLog(e.CommandArgument.ToString()))
                    {

                        string type = "MODIFY";
                        string i_code = e.CommandArgument.ToString();
                        Response.Redirect("~/Masters/ADD/RawMaterial.aspx?c_name=" + type + "&i_code=" + i_code + "&c_type=" + c_type, false);
                    }
                    else
                    {
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Record Used By Another Person";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                        //ShowMessage("#Avisos", "Record Used By Another Person", CommonClasses.MSG_Erro);
                        return;
                    }

                }
                else
                {
                    //ShowMessage("#Avisos", "You Have No Rights To Modify", CommonClasses.MSG_Erro);
                    PanelMsg.Visible = true;
                    lblmsg.Text = "You Have No Rights To Modify";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                    return;
                }
            }

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Item Master", "dgRawMaterial_RowCommand", Ex.Message);
        }
    }
    #endregion       

    #endregion

    #region User Defined Method

    #region LoadItem
    private void LoadItem()
    {
        try
        {
            string strSql = "";
            strSql = " SELECT I_CODE,I_CODENO,I_NAME,I_SIZE,cast(I_CURRENT_BAL as numeric(20,2)) as I_CURRENT_BAL,I_CAT_NAME,I_UOM_NAME,  CASE WHEN I_INV_CAT=1 then'A' WHEN I_INV_CAT=2 then'B' WHEN I_INV_CAT=3 then'C' ELSE '' END AS I_INV_CAT FROM ITEM_MASTER,ITEM_CATEGORY_MASTER,ITEM_UNIT_MASTER  WHERE ITEM_CATEGORY_MASTER.I_CAT_CODE=ITEM_MASTER.I_CAT_CODE and I_CM_COMP_ID = '1' and ITEM_MASTER.ES_DELETE='0'  AND ITEM_MASTER.I_UOM_CODE=ITEM_UNIT_MASTER.I_UOM_CODE and I_CM_COMP_ID = '" + Convert.ToInt32(Session["CompanyId"]) + "' and ITEM_MASTER.ES_DELETE='0'";
            DataTable dtfilter = new DataTable();

            if (c_type == "sale")
            {
                //strSql = strSql + " and ITEM_CATEGORY_MASTER.I_CAT_CODE=-2147483633";
                dtfilter = CommonClasses.Execute(strSql);
                if (dtfilter.Rows.Count > 0)
                {
                    dgRawMaterial.Enabled = true;
                    dgRawMaterial.DataSource = dtfilter;
                    dgRawMaterial.DataBind();
                }
            }
            //else if (c_type == "purchase")
            else
            {
                BL_RawMatrial = new RawMaterial_BL();
                BL_RawMatrial.I_CM_COMP_ID = Convert.ToInt32(Session["CompanyId"]);
                BL_RawMatrial.FillGrid(dgRawMaterial);
                dgRawMaterial.Enabled = dgRawMaterial.Rows.Count == 0 ? false : true;
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Item Master", "LoadItem", Ex.Message);
        }
    }
    #endregion

    #region LoadStatus
    private void LoadStatus(TextBox txtString)
    {
        try
        {
            string str = "";
            str = txtString.Text.Trim();
            string strSql = "";
            strSql = " SELECT I_CODE,I_CODENO,I_NAME,I_SIZE,cast(I_CURRENT_BAL as numeric(20,2)) as I_CURRENT_BAL,I_CAT_NAME,I_UOM_NAME,  CASE WHEN I_INV_CAT=1 then'A' WHEN I_INV_CAT=2 then'B' WHEN I_INV_CAT=3 then'C' ELSE '' END AS I_INV_CAT   FROM ITEM_MASTER,ITEM_CATEGORY_MASTER,ITEM_UNIT_MASTER  WHERE ITEM_CATEGORY_MASTER.I_CAT_CODE=ITEM_MASTER.I_CAT_CODE   and ITEM_MASTER.ES_DELETE='0'  AND ITEM_MASTER.I_UOM_CODE=ITEM_UNIT_MASTER.I_UOM_CODE and I_CM_COMP_ID = '" + Convert.ToInt32(Session["CompanyId"]) + "' and ITEM_MASTER.ES_DELETE='0'";
            DataTable dtfilter = new DataTable();

            if (c_type == "sale")
            {
                //strSql = strSql + " and ITEM_CATEGORY_MASTER.I_CAT_CODE=-2147483633";
            }
            else if (c_type == "purchase")
            {

            }
            if (txtString.Text != "")
                //dtfilter = CommonClasses.Execute("SELECT I_CODE,I_CODENO,I_NAME,I_SIZE,I_CURRENT_BAL FROM ITEM_MASTER WHERE I_CM_COMP_ID = '" + Convert.ToInt32(Session["CompanyId"]) + "' AND I_CAT_CODE!='-2147483647' AND I_CAT_CODE!='-2147483646' and ES_DELETE='0' and (I_CODENO like upper('%" + str + "%') OR I_NAME like upper('%" + str + "%') OR I_CURRENT_BAL like upper('%" + str + "%'))");
                dtfilter = CommonClasses.Execute(strSql + " and (I_CODENO like upper('%" + str + "%') OR I_NAME like upper('%" + str + "%') OR I_CURRENT_BAL like upper('%" + str + "%') OR I_CAT_NAME like upper('%" + str + "%') )");

            else
                //dtfilter = CommonClasses.Execute("SELECT I_CODE,I_CODENO,I_NAME,I_SIZE,I_CURRENT_BAL FROM ITEM_MASTER WHERE I_CM_COMP_ID = '" + Convert.ToInt32(Session["CompanyId"]) + "' AND I_CAT_CODE!='-2147483647' AND I_CAT_CODE!='-2147483646' and ES_DELETE='0'");
                dtfilter = CommonClasses.Execute(strSql);


            if (dtfilter.Rows.Count > 0)
            {
                dgRawMaterial.Enabled = true;
                dgRawMaterial.DataSource = dtfilter;
                dgRawMaterial.DataBind();
            }
            else
            {
                dtFilter.Clear();
                dgRawMaterial.Enabled = false;
                if (dtFilter.Columns.Count == 0)
                {
                    dtFilter.Columns.Add(new System.Data.DataColumn("I_CODE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("I_CODENO", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("I_NAME", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("I_CURRENT_BAL", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("I_UOM_NAME", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("I_CAT_NAME", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("I_INV_CAT", typeof(String)));
                    dtFilter.Rows.Add(dtFilter.NewRow());
                    dgRawMaterial.DataSource = dtFilter;
                    dgRawMaterial.DataBind();
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Item Master", "LoadStatus", ex.Message);
        }
    }

    #endregion

    #region ModifyLog
    bool ModifyLog(string PrimaryKey)
    {
        try
        {

            DataTable dt = CommonClasses.Execute("select MODIFY from ITEM_MASTER where I_CODE=" + PrimaryKey + "  ");
            if (dt.Rows.Count > 0)
            {
                if (Convert.ToBoolean(dt.Rows[0][0].ToString()) == true)
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Record Used By Another Person";

                    //ShowMessage("#Avisos", "Record used by another user", CommonClasses.MSG_Warning);
                    return true;
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Item Master", "ModifyLog", Ex.Message);
        }

        return false;
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
            CommonClasses.SendError("Item Master", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #endregion
}
