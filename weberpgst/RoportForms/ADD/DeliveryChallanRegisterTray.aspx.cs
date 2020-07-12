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
using CrystalDecisions.CrystalReports.Engine;

public partial class RoportForms_ADD_DeliveryChallanRegisterTray : System.Web.UI.Page
{
    DatabaseAccessLayer DL_DBAccess = null;

    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    #endregion Page_Load

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/RoportForms/VIEW/DeliveryChallanRegisterTray.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Delivery challan Register", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion

    #region Page_Init
    protected void Page_Init(object sender, EventArgs e)
    {
        try
        {
            string Title = Request.QueryString[0];
            //bool ChkAllDate = Convert.ToBoolean(Request.QueryString[1].ToString());
            //bool chkAllCust = Convert.ToBoolean(Request.QueryString[2].ToString());
            //bool ChkAllitem = Convert.ToBoolean(Request.QueryString[3].ToString());
            //bool ChkAllUser = Convert.ToBoolean(Request.QueryString[4].ToString());

            string From = Request.QueryString[1].ToString();
            string To = Request.QueryString[2].ToString();
            //string p_name = Request.QueryString[6].ToString();
            //string i_name = Request.QueryString[7].ToString();
            string group = Request.QueryString[3].ToString();
            string way = Request.QueryString[4].ToString();
            string Cond = Request.QueryString[5].ToString();
            //i_name = i_name.Replace("'", "''");

            GenerateReport(Title, From, To, group, way, Cond);
            #region Detail
            /*
            if (group == "Datewise")
            {

                if (ChkAllDate == true && chkAllCust == true && ChkAllitem == true)
                {
                    GenerateReport(Title, "All", "All", "All", From, To, p_name, i_name, group, way);
                }
                if (ChkAllDate != true && chkAllCust != true && ChkAllitem != true)
                {
                    GenerateReport(Title, "ONE", "ONE", "ONE", From, To, p_name, i_name, group, way);

                }
                if (ChkAllDate != true && chkAllCust == true && ChkAllitem == true)
                {
                    GenerateReport(Title, "ONE", "All", "All", From, To, p_name, i_name, group, way);

                }
                if (ChkAllDate == true && chkAllCust != true && ChkAllitem == true)
                {
                    GenerateReport(Title, "All", "ONE", "All", From, To, p_name, i_name, group, way);

                }
                if (ChkAllDate == true && chkAllCust == true && ChkAllitem != true)
                {
                    GenerateReport(Title, "All", "All", "ONE", From, To, p_name, i_name, group, way);

                }
                if (ChkAllDate != true && chkAllCust != true && ChkAllitem == true)
                {
                    GenerateReport(Title, "ONE", "ONE", "All", From, To, p_name, i_name, group, way);
                }
                if (ChkAllDate != true && chkAllCust == true && ChkAllitem != true)
                {
                    GenerateReport(Title, "ONE", "All", "ONE", From, To, p_name, i_name, group, way);
                }
                if (ChkAllDate == true && chkAllCust != true && ChkAllitem != true)
                {
                    GenerateReport(Title, "All", "ONE", "ONE", From, To, p_name, i_name, group, way);
                }
            }
            if (group == "SuppWise")
            {

                if (ChkAllDate == true && chkAllCust == true && ChkAllitem == true)
                {
                    GenerateReport(Title, "All", "All", "All", From, To, p_name, i_name, group, way);
                }
                if (ChkAllDate != true && chkAllCust != true && ChkAllitem != true)
                {
                    GenerateReport(Title, "ONE", "ONE", "ONE", From, To, p_name, i_name, group, way);
                }
                if (ChkAllDate != true && chkAllCust == true && ChkAllitem == true)
                {
                    GenerateReport(Title, "ONE", "All", "All", From, To, p_name, i_name, group, way);
                }
                if (ChkAllDate == true && chkAllCust != true && ChkAllitem == true)
                {
                    GenerateReport(Title, "All", "ONE", "All", From, To, p_name, i_name, group, way);
                }
                if (ChkAllDate == true && chkAllCust == true && ChkAllitem != true)
                {
                    GenerateReport(Title, "All", "All", "ONE", From, To, p_name, i_name, group, way);
                }
                if (ChkAllDate != true && chkAllCust != true && ChkAllitem == true)
                {
                    GenerateReport(Title, "ONE", "ONE", "All", From, To, p_name, i_name, group, way);
                }
                if (ChkAllDate != true && chkAllCust == true && ChkAllitem != true)
                {
                    GenerateReport(Title, "ONE", "All", "ONE", From, To, p_name, i_name, group, way);
                }
                if (ChkAllDate == true && chkAllCust != true && ChkAllitem != true)
                {
                    GenerateReport(Title, "All", "ONE", "ONE", From, To, p_name, i_name, group, way);
                }

            }
            */

            #endregion




        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer Rejection Register", "btnCancel_Click", Ex.Message);
        }
    }

    #endregion

    #region GenerateReport
    private void GenerateReport(string Title, string From, string To, string group, string way, string Condition)
    {
        try
        {
            DL_DBAccess = new DatabaseAccessLayer();
            string Query = "";

            Query = "Select DCM_CODE,DCM_DATE,DCM_NO,I_CODENO,I_NAME,P_NAME,I_UOM_NAME AS I_UOM_NAME,DCM_TYPE,DCD_ORD_QTY from DELIVERY_CHALLAN_MASTER,DELIVERY_CHALLAN_DETAIL,ITEM_MASTER,PARTY_MASTER,ITEM_UNIT_MASTER where " + Condition + " DCM_TYPE='DLCT' AND  DCM_CODE=DCD_DCM_CODE and DCD_I_CODE=I_CODE and DCM_P_CODE=P_CODE and DELIVERY_CHALLAN_MASTER.ES_DELETE=0 and DCD_UM_CODE=ITEM_UNIT_MASTER.I_UOM_CODE";

            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt = CommonClasses.Execute(Query);
            if (dt.Rows.Count > 0)
            {
                ReportDocument rptname = null;
                rptname = new ReportDocument();
                DateTime dtFrom = Convert.ToDateTime(From);
                DateTime dtTo = Convert.ToDateTime(To);
                if (group == "Datewise")
                {
                    rptname.Load(Server.MapPath("~/Reports/rptDeliverychalanRegDateTray.rpt")); //rptDeliverychallanRegisterDateWiseTray
                    rptname.FileName = Server.MapPath("~/Reports/rptDeliverychalanRegDateTray.rpt"); //rptDeliverychalanRegDateTray.rpt
                    rptname.Refresh();
                    rptname.SetDataSource(dt);
                    
                    rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());
                    rptname.SetParameterValue("txtDate", " From " + dtFrom.ToString("dd/MMM/yyyy") + " To " + dtTo.ToString("dd/MMM/yyyy"));
                    CrystalReportViewer1.ReportSource = rptname;

                }
                if (group == "SuppWise")
                {
                    rptname.Load(Server.MapPath("~/Reports/rptDeliverychallanRegisterSuppWiseTray.rpt"));
                    rptname.FileName = Server.MapPath("~/Reports/rptDeliverychallanRegisterSuppWiseTray.rpt");
                    //rptname.Load(Server.MapPath("~/Reports/rptQtnRegDatewise.rpt"));
                    //rptname.FileName = Server.MapPath("~/Reports/rptQtnRegDatewise.rpt");

                    rptname.Refresh();
                    rptname.SetDataSource(dt);


                    rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());
                    rptname.SetParameterValue("txtDate", " From " + dtFrom.ToString("dd/MMM/yyyy") + " To " + dtTo.ToString("dd/MMM/yyyy"));

                    CrystalReportViewer1.ReportSource = rptname;
                }

            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Record Not Found";
                return;
            }
        }
        catch (Exception Ex)
        {
            lblmsg.Visible = true;
            lblmsg.Text = Ex.Message;
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
            CommonClasses.SendError("Customer Rejection Register", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion
}
