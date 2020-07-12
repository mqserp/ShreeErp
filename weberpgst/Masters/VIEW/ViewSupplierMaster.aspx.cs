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

public partial class Masters_VIEW_ViewSupplierMaster : System.Web.UI.Page
{
    #region Variable Declaration
    static bool CheckModifyLog = false;
    static string right = "";
    static string fieldName;
    DataTable dtFilter = new DataTable();

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
                    DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='11'");
                    right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();
                    LoadSupplier();                   
                    //LoadFilter();
                }
                txtString.Focus();
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Supplier Master", "Page_Load", Ex.Message);
        }

    }
    #endregion

    #region btnAddNew_Click
    protected void btnAddNew_Click(object sender, EventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(3, 1)), this, "For Add"))
            {
                string type = "INSERT";
                Response.Redirect("~/Masters/ADD/SupplierMaster.aspx?c_name=" + type, false);
            }

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Supplier Master", "btnAddNew_Click", Ex.Message);
        }
    }
    #endregion


    #region LoadSupplier
    private void LoadSupplier()
    {
        try
        {
            DataTable dt = new DataTable();

            dt = CommonClasses.Execute("SELECT P_CODE,P_PARTY_CODE,P_NAME,P_CONTACT,P_PHONE FROM PARTY_MASTER WHERE P_CM_COMP_ID = '" + Convert.ToInt32(Session["CompanyId"]) + "' and P_TYPE=2 and ES_DELETE=0 ORDER BY P_NAME");
            if (dt.Rows.Count == 0)
            {
                dgCustomerMaster.Enabled = false;
                dtFilter.Clear();

                if (dtFilter.Columns.Count == 0)
                {
                    dtFilter.Columns.Add(new System.Data.DataColumn("P_CODE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("P_PARTY_CODE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("P_NAME", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("P_CONTACT", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("P_PHONE", typeof(String)));

                    dtFilter.Rows.Add(dtFilter.NewRow());
                    dgCustomerMaster.DataSource = dtFilter;
                    dgCustomerMaster.DataBind();
                }
            }
            else
            {
                dgCustomerMaster.Enabled = true;
                dgCustomerMaster.DataSource = dt;
                dgCustomerMaster.DataBind();
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Supplier Master", "LoadCustomer", Ex.Message);
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

        catch (Exception Ex)
        {
            CommonClasses.SendError("Supplier Master", "txtString_TextChanged", Ex.Message);
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

            DataTable dtfilter = new DataTable();

            if (txtString.Text != "")
                dtfilter = CommonClasses.Execute("SELECT P_CODE,P_PARTY_CODE,P_NAME,P_CONTACT,P_PHONE FROM PARTY_MASTER WHERE P_CM_COMP_ID = '" + Convert.ToInt32(Session["CompanyId"]) + "' and ES_DELETE='0' and P_TYPE=2 and (P_PARTY_CODE like upper('%" + str + "%') OR P_NAME like upper('%" + str + "%') OR P_CONTACT like upper('%" + str + "%') OR P_PHONE like upper('%" + str + "%')) order by P_NAME");
            else
                dtfilter = CommonClasses.Execute("SELECT P_CODE,P_PARTY_CODE,P_NAME,P_CONTACT,P_PHONE FROM PARTY_MASTER WHERE P_CM_COMP_ID = '" + Convert.ToInt32(Session["CompanyId"]) + "' and P_TYPE=2 and ES_DELETE='0' order by P_NAME");

            if (dtfilter.Rows.Count > 0)
            {
                dgCustomerMaster.Enabled = true;
                dgCustomerMaster.DataSource = dtfilter;
                dgCustomerMaster.DataBind();
            }
            else
            {
                dtFilter.Clear();

                if (dtFilter.Columns.Count == 0)
                {
                    dgCustomerMaster.Enabled = false;
                    dtFilter.Columns.Add(new System.Data.DataColumn("P_CODE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("P_PARTY_CODE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("P_NAME", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("P_CONTACT", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("P_PHONE", typeof(String)));
                    
                    dtFilter.Rows.Add(dtFilter.NewRow());
                    dgCustomerMaster.DataSource = dtFilter;
                    dgCustomerMaster.DataBind();
                }

            }

        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Supplier Master", "LoadStatus", ex.Message);
        }
    }

    #endregion

    #region Grid events
    protected void dgCustomerMaster_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(4, 1)), this, "For Delete"))
            {
                if (!ModifyLog(((Label)(dgCustomerMaster.Rows[e.RowIndex].FindControl("lblP_CODE"))).Text))
                {

                    string p_code = ((Label)(dgCustomerMaster.Rows[e.RowIndex].FindControl("lblP_CODE"))).Text;
                    string p_name = ((Label)(dgCustomerMaster.Rows[e.RowIndex].FindControl("lblP_NAME"))).Text;
                    string p_party_code = ((Label)(dgCustomerMaster.Rows[e.RowIndex].FindControl("lblP_PARTY_CODE"))).Text;

                    if (CommonClasses.CheckUsedInTran("SUPP_PO_MASTER", "SPOM_P_CODE", "AND ES_DELETE=0", p_code))
                    {
                        //ShowMessage("#Avisos", "You can't delete this record it has used in Components", CommonClasses.MSG_Warning);
                        PanelMsg.Visible = true;
                        lblmsg.Text = "You can't delete this record it has used in Purchase Order";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
        
                    }
                    else if (CommonClasses.CheckUsedInTran("INWARD_MASTER", "IWM_P_CODE", "AND ES_DELETE=0", p_code))
                    {
                        //ShowMessage("#Avisos", "You can't delete this record it has used in Components", CommonClasses.MSG_Warning);
                        PanelMsg.Visible = true;
                        lblmsg.Text = "You can't delete this record it has used in Inward";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                    }

                    else
                    {
                        bool flag = CommonClasses.Execute1("UPDATE PARTY_MASTER SET ES_DELETE = 1 WHERE P_CODE='" + Convert.ToInt32(p_code) + "' and P_TYPE=2 ");
                        if (flag == true)
                        {
                            CommonClasses.WriteLog("Supplier Master", "Delete", "Supplier Master", p_name, Convert.ToInt32(p_code), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                            //ShowMessage("#Avisos", CommonClasses.strRegDelSucesso, CommonClasses.MSG_Erro);
                            PanelMsg.Visible = true;
                            lblmsg.Text = "Record Deleted Successfully";
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
        

                        }
                    }
                    //PanelMsg.Visible = false;
                    //}
                    LoadSupplier();
                }
                else
                {
                    //ShowMessage("#Avisos", "Record Used By Another Person", CommonClasses.MSG_Erro);
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Record Used By Another Person";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
        
                    return;
                }

            }

            else
            {
               // ShowMessage("#Avisos", "You Have No Rights To Delete", CommonClasses.MSG_Erro);
                PanelMsg.Visible = true;
                lblmsg.Text = "You Have No Rights To Delete";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
        
                return;
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Supplier Master", "dgCustomerMaster_RowDeleting", Ex.Message);
        }
    }
    protected void dgCustomerMaster_RowEditing(object sender, GridViewEditEventArgs e)
    {

        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(2, 1)), this, "For Modify"))
            {
                string p_code = ((Label)(dgCustomerMaster.Rows[e.NewEditIndex].FindControl("lblP_CODE"))).Text;
                string type = "MODIFY";
                Response.Redirect("~/Masters/ADD/CustomerMaster.aspx?c_name=" + type + "&p_code=" + p_code, false);
            }
            else
            {
               // ShowMessage("#Avisos", "You Have No Rights To Modify", CommonClasses.MSG_Erro);
                PanelMsg.Visible = true;
                lblmsg.Text = "You Have No Rights To Modify";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
        
                return;
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Supplier Master", "dgCustomerMaster_RowEditing", Ex.Message);
        }
    }
    protected void dgCustomerMaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            dgCustomerMaster.PageIndex = e.NewPageIndex;
            LoadSupplier();
        }
        catch (Exception)
        {
        }

    }


    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/Masters/ADD/PurchaseDefault.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Supplier Master", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion

    protected void dgCustomerMaster_RowCommand(object sender, GridViewCommandEventArgs e)
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
                        string p_code = e.CommandArgument.ToString();
                        Response.Redirect("~/Masters/ADD/SupplierMaster.aspx?c_name=" + type + "&i_uom_code=" + p_code, false);
                    }
                    else
                    {
                        //ShowMessage("#Avisos", "Record Used By Another Person", CommonClasses.MSG_Erro);
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Record Used By Another Person";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
        
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
                        string p_code = e.CommandArgument.ToString();
                        Response.Redirect("~/Masters/ADD/SupplierMaster.aspx?c_name=" + type + "&p_code=" + p_code, false);
                    }
                    else
                    {
                        //ShowMessage("#Avisos", "Record Used By Another Person", CommonClasses.MSG_Erro);
                        PanelMsg.Visible = true;
                        lblmsg.Text = "Record Used By Another Person";
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
        
                        return;
                    }

                }
                else
                {
                   // ShowMessage("#Avisos", "You Have No Rights To Modify", CommonClasses.MSG_Erro);
                    PanelMsg.Visible = true;
                    lblmsg.Text = "You Have No Rights To Modify";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
        
                    return;
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Supplier Master", "GridView1_RowCommand", Ex.Message);
        }
    }
    #endregion

    #region ModifyLog
    bool ModifyLog(string PrimaryKey)
    {
        try
        {
            CheckModifyLog = false;
            DataTable dt = CommonClasses.Execute("select MODIFY from PARTY_MASTER where P_CODE=" + PrimaryKey + " AND P_TYPE=2  ");
            if (dt.Rows.Count > 0)
            {
                if (Convert.ToBoolean(dt.Rows[0][0].ToString()) == true)
                {
                    //ShowMessage("#Avisos", "Record used by another user", CommonClasses.MSG_Warning);
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Record Used By Another Person";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
        
                    return true;
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Supplier Master", "ModifyLog", Ex.Message);
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
            CommonClasses.SendError("Supplier Master", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion


}



