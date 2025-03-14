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


public partial class Admin_View_ViewTransportMaster : System.Web.UI.Page
{
    #region Variables
    TransportMaster_BL BL_TransportMaster = null;
    static bool CheckModifyLog = false;
    static string right = "";
    static string fieldName;
    DataTable dtFilter = new DataTable();
    #endregion

    #region Evenets

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
                    DataTable dtRights = CommonClasses.Execute("select UR_RIGHTS from USER_RIGHT where UR_IS_DELETE=0 AND UR_UM_CODE='" + Convert.ToInt32(Session["UserCode"]) + "' and UR_SM_CODE='132'");
                    right = dtRights.Rows.Count == 0 ? "0000000" : dtRights.Rows[0][0].ToString();
                    LoadTransport();
                    dgTransportMaster.Enabled = true;
                    if (dgTransportMaster.Rows.Count == 0)
                    {
                        dtFilter.Clear();
                        if (dtFilter.Columns.Count == 0)
                        {
                            dtFilter.Columns.Add(new System.Data.DataColumn("T_CODE", typeof(String)));
                            dtFilter.Columns.Add(new System.Data.DataColumn("T_NAME", typeof(String)));

                            dtFilter.Rows.Add(dtFilter.NewRow());
                            dgTransportMaster.DataSource = dtFilter;
                            dgTransportMaster.DataBind();
                            dgTransportMaster.Enabled = false;
                        }
                    }

                    //}
                    //else
                    //{
                    //    Response.Write("<script> alert('You Have No Rights To View.');window.location='../Default.aspx'; </script>");

                    //}

                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Transport Master ", "Page_Load", Ex.Message);
        }
    }
    #endregion

    #region btnClose_Click
    protected void btnClose_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/Admin/Default.aspx", false);
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Transport Master - View", "btnClose_Click", ex.Message);
        }
    }
    #endregion btnClose_Click

    #region dgTransportMaster_RowDeleting
    protected void dgTransportMaster_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(4, 1)), this, "For Delete"))
            {
                if (!ModifyLog(((Label)(dgTransportMaster.Rows[e.RowIndex].FindControl("lblT_CODE"))).Text))
                {
                    BL_TransportMaster = new TransportMaster_BL();
                    string T_CODE = ((Label)(dgTransportMaster.Rows[e.RowIndex].FindControl("lblT_CODE"))).Text;
                    string T_NAME = ((Label)(dgTransportMaster.Rows[e.RowIndex].FindControl("lblT_NAME"))).Text;
                    BL_TransportMaster.T_CODE = Convert.ToInt32(T_CODE);
                    //if (CommonClasses.CheckUsedInTran("STATE_MASTER", "SM_T_CODE", "AND ES_DELETE=0", T_CODE))
                    //{
                    //    //ShowMessage("#Avisos", "You cant delete this record it has used in State Master", CommonClasses.MSG_Warning);
                    //    PanelMsg.Visible = true;
                    //    lblmsg.Text = "You cant delete this record it has used in State Master";
                    //    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                    //}
                    //else
                    //{
                        bool flag = BL_TransportMaster.Delete();
                        if (flag == true)
                        {
                            CommonClasses.WriteLog("Transport Master", "Delete", "Transport Master", T_NAME, Convert.ToInt32(T_CODE), Convert.ToInt32(Session["CompanyId"]), Convert.ToInt32(Session["CompanyCode"]), (Session["Username"].ToString()), Convert.ToInt32(Session["UserCode"]));
                            //ShowMessage("#Avisos", CommonClasses.strRegDelSucesso, CommonClasses.MSG_Erro);
                            PanelMsg.Visible = true;
                            lblmsg.Text = "Record Deleted Successfully";

                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);


                        }
                    //}
                }
                LoadTransport();
            }



            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "You Have No Rights To Delete";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                //ShowMessage("#Avisos", "You Have No Rights To Delete", CommonClasses.MSG_Erro);
                return;
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Transport Master", "dgTransportMaster_RowDeleting", Ex.Message);
        }
    }
    #endregion

    #region dgTransportMaster_RowEditing
    protected void dgTransportMaster_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            if (CommonClasses.ValidRights(int.Parse(right.Substring(2, 1)), this, "For Modify"))
            {
                string T_CODE = ((Label)(dgTransportMaster.Rows[e.NewEditIndex].FindControl("lblT_CODE"))).Text;
                string type = "MODIFY";
                Response.Redirect("~/Admin/Add/TransportMaster.aspx?c_name=" + type + "&T_CODE=" + T_CODE, false);
            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "You Have No Rights To Modify";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                //ShowMessage("#Avisos", "You Have No Rights To Modify", CommonClasses.MSG_Erro);
                return;
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Transport Master", "dgTransportMaster_RowEditing", Ex.Message);
        }
    }
    #endregion

    #region dgTransportMaster_PageIndexChanging
    protected void dgTransportMaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            dgTransportMaster.PageIndex = e.NewPageIndex;
            LoadTransport();
        }
        catch (Exception)
        {
        }
    }
    #endregion

    #region dgTransportMaster_RowUpdating
    protected void dgTransportMaster_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {

    }
    #endregion

    #region dgTransportMaster_RowCommand
    protected void dgTransportMaster_RowCommand(object sender, GridViewCommandEventArgs e)
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
                        string T_CODE = e.CommandArgument.ToString();
                        Response.Redirect("~/Admin/Add/TransportMaster.aspx?c_name=" + type + "&T_CODE=" + T_CODE, false);
                    }
                    else
                    {
                        ShowMessage("#Avisos", "Record Used By Another Person", CommonClasses.MSG_Erro);
                        return;
                    }
                }



                else
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "You Have No Rights To View";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                    //ShowMessage("#Avisos", "You Have No Rights To View", CommonClasses.MSG_Erro);
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
                        string T_CODE = e.CommandArgument.ToString();
                        Response.Redirect("~/Admin/Add/TransportMaster.aspx?c_name=" + type + "&T_CODE=" + T_CODE, false);
                    }
                    else
                    {
                        ShowMessage("#Avisos", "Record Used By Another Person", CommonClasses.MSG_Erro);
                        return;
                    }
                }



                else
                {
                    PanelMsg.Visible = true;
                    lblmsg.Text = "You Have No Rights To Modify";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                    //ShowMessage("#Avisos", "You Have No Rights To Modify", CommonClasses.MSG_Erro);
                    return;
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Transport Master", "dgTransportMaster_RowCommand", Ex.Message);
        }
    }
    #endregion

    #region Search Events
    protected void txtString_TextChanged(object sender, EventArgs e)
    {
        try
        {
            LoadStatus(txtString);
        }

        catch (Exception ex)
        {
            CommonClasses.SendError("Transport Master", "btnSearch_Click", ex.Message);
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
                Response.Redirect("~/Admin/Add/TransportMaster.aspx?c_name=" + type, false);
            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "You Have No Rights To Add";
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Transport Master", "btnAddNew_Click", Ex.Message);
        }
    }
    #endregion


    #endregion

    #region Methods

    #region LoadTransport
    private void LoadTransport()
    {
        try
        {
            BL_TransportMaster = new TransportMaster_BL();
            BL_TransportMaster.T_CM_COMP_ID = Convert.ToInt32(Session["CompanyId"]);
            BL_TransportMaster.FillGrid(dgTransportMaster);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Transport Master", "LoadTransport", Ex.Message);
        }
    }
    #endregion


    #region ModifyLog
    bool ModifyLog(string PrimaryKey)
    {
        try
        {
            CheckModifyLog = false;
            DataTable dt = CommonClasses.Execute("select MODIFY from TRANSAPORT_MASTER where T_CODE=" + PrimaryKey + "  ");
            if (dt.Rows.Count > 0)
            {
                if (Convert.ToBoolean(dt.Rows[0][0].ToString()) == true)
                {
                    // ShowMessage("#Avisos", "Record used by another user", CommonClasses.MSG_Warning);
                    PanelMsg.Visible = true;
                    lblmsg.Text = "Record used by another user";
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                    return true;
                }
            }
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Transport Master", "ModifyLog", Ex.Message);
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
            CommonClasses.SendError("Transport Master", "ShowMessage", Ex.Message);
            return false;
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
                dtfilter = CommonClasses.Execute("SELECT T_CODE,T_NAME FROM TRANSAPORT_MASTER WHERE T_CM_COMP_ID='" + Session["CompanyId"] + "' and ES_DELETE='0' and (T_NAME like upper('%" + str + "%')) order by T_NAME ");
            else
                dtfilter = CommonClasses.Execute("SELECT T_CODE,T_NAME FROM TRANSAPORT_MASTER WHERE T_CM_COMP_ID='" + Session["CompanyId"] + "' and ES_DELETE='0' order by T_NAME");

            if (dtfilter.Rows.Count > 0)
            {
                dgTransportMaster.Enabled = true;
                dgTransportMaster.DataSource = dtfilter;
                dgTransportMaster.DataBind();
            }
            else
            {
                dtFilter.Clear();
                if (dtFilter.Columns.Count == 0)
                {
                    dtFilter.Columns.Add(new System.Data.DataColumn("T_CODE", typeof(String)));
                    dtFilter.Columns.Add(new System.Data.DataColumn("T_NAME", typeof(String)));

                    dtFilter.Rows.Add(dtFilter.NewRow());
                    dgTransportMaster.DataSource = dtFilter;
                    dgTransportMaster.DataBind();
                    dgTransportMaster.Enabled = false;
                }
            }

        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Transport Master", "LoadStatus", ex.Message);
        }
    }

    #endregion
    #endregion
}
