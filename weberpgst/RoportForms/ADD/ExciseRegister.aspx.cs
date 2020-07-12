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

public partial class RoportForms_ADD_ExciseRegister : System.Web.UI.Page
{
    DatabaseAccessLayer DL_DBAccess = null;

    protected void Page_Load(object sender, EventArgs e)
    {

    }
    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/RoportForms/VIEW/VIewExciseRegister.aspx", false);
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
            string Type = Request.QueryString[6].ToString();
            string ReportType = Request.QueryString[7].ToString();
            //i_name = i_name.Replace("'", "''");

            GenerateReport(Title, From, To, group, way, Cond, Type, ReportType);
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
    private void GenerateReport(string Title, string From, string To, string group, string way, string Condition, string Type, string RepType)
    {
        try
        {

            DL_DBAccess = new DatabaseAccessLayer();
            string Query = "";
            // Query = "  Select DCM_CODE,DCM_DATE,DCM_NO,I_CODENO,I_NAME,P_NAME,I_UOM_NAME as I_UOM_NAME,DCM_TYPE,DCD_ORD_QTY ,SUM(DND_REC_QTY) AS DND_REC_QTY ,SUM(DND_SCRAP_QTY) AS  DND_SCRAP_QTY from DELIVERY_CHALLAN_MASTER,DELIVERY_CHALLAN_DETAIL,ITEM_MASTER,PARTY_MASTER,ITEM_UNIT_MASTER , DC_RETURN_MASTER,DC_RETURN_DETAIL where " + Condition + " DCM_TYPE='DLCT' AND  DCM_CODE=DCD_DCM_CODE and DCD_I_CODE=I_CODE and DCM_P_CODE=P_CODE and DELIVERY_CHALLAN_MASTER.ES_DELETE=0 and DCD_UM_CODE=ITEM_UNIT_MASTER.I_UOM_CODE AND DNM_CODE=DND_DNM_CODE AND DC_RETURN_MASTER.ES_DELETE=0 AND DNM_PARTY_DC_NO =DCM_CODE AND DCD_I_CODE=DND_I_CODE  GROUP BY DCM_CODE,DCM_DATE,DCM_NO,I_CODENO,I_NAME,P_NAME,I_UOM_NAME ,DCM_TYPE,DCD_ORD_QTY";
 
           // Query = "SELECT EXICSE_ENTRY.EX_IND,EXICSE_ENTRY.EX_NO, convert(varchar,EXICSE_ENTRY.EX_DATE,106) as EX_DATE,EXICSE_ENTRY.EX_DOC_TYPE,EXICSE_ENTRY.EX_DOC_NO, convert(varchar,EXICSE_ENTRY.EX_DOC_DATE,106) as EX_DOC_DATE, EXICSE_ENTRY.EX_BANK_AMT, EXICSE_ENTRY.EX_EX_DUTY, EXICSE_ENTRY.EX_EX_CESS, EXICSE_ENTRY.EX_EX_HCESS,EXICSE_ENTRY.EX_PAY_DETAIL, EXICSE_ENTRY.ES_DELETE, EXICSE_ENTRY.EX_BASIC_AMT, PARTY_MASTER.P_NAME, PARTY_MASTER.P_ECC_NO,PARTY_MASTER.P_EXC_DIV, PARTY_MASTER.P_EXC_RANGE FROM EXICSE_ENTRY LEFT OUTER JOIN PARTY_MASTER AS PARTY_MASTER ON EXICSE_ENTRY.EX_P_CODE = PARTY_MASTER.P_CODE WHERE " + Condition + " (EXICSE_ENTRY.ES_DELETE = 0) ORDER BY EXICSE_ENTRY.EX_NO";
 


            //Query = "SELECT EXICSE_ENTRY.EX_IND,EXICSE_ENTRY.EX_NO, EXICSE_ENTRY.EX_DATE  ,EXICSE_ENTRY.EX_DOC_TYPE,EXICSE_ENTRY.EX_DOC_NO,EXICSE_ENTRY.EX_DOC_DATE, EXICSE_ENTRY.EX_BANK_AMT, EXICSE_ENTRY.EX_EX_DUTY, EXICSE_ENTRY.EX_EX_CESS, EXICSE_ENTRY.EX_EX_HCESS,EXICSE_ENTRY.EX_PAY_DETAIL, EXICSE_ENTRY.ES_DELETE, EXICSE_ENTRY.EX_BASIC_AMT, PARTY_MASTER.P_NAME, PARTY_MASTER.P_ECC_NO,PARTY_MASTER.P_EXC_DIV, PARTY_MASTER.P_EXC_RANGE FROM EXICSE_ENTRY LEFT OUTER JOIN PARTY_MASTER AS PARTY_MASTER ON EXICSE_ENTRY.EX_P_CODE = PARTY_MASTER.P_CODE WHERE " + Condition + " (EXICSE_ENTRY.ES_DELETE = 0) ORDER BY EXICSE_ENTRY.EX_NO";
            Query = "SELECT EXICSE_ENTRY.EX_CODE, EXICSE_ENTRY.EX_IND, EXICSE_ENTRY.EX_NO,  EXICSE_ENTRY.EX_DATE , EXICSE_ENTRY.EX_DOC_TYPE, EXICSE_ENTRY.EX_DOC_NO, EXICSE_ENTRY.EX_DOC_DATE, EXICSE_ENTRY.EX_BANK_AMT, EXICSE_ENTRY.EX_EX_DUTY,EXICSE_ENTRY.EX_EX_CESS, EXICSE_ENTRY.EX_EX_HCESS, EXICSE_ENTRY.EX_PAY_DETAIL, EXICSE_ENTRY.ES_DELETE, EXICSE_ENTRY.EX_BASIC_AMT,PARTY_MASTER.P_NAME, P_LBT_NO AS P_ECC_NO, PARTY_MASTER.P_EXC_DIV, PARTY_MASTER.P_EXC_RANGE, ex.EXD_I_CODE,EXCISE_TARIFF_MASTER.E_TARIFF_NO, EXCISE_TARIFF_MASTER.E_COMMODITY, EXCISE_TARIFF_MASTER.E_BASIC, ITEM_MASTER.I_CODENO,ITEM_MASTER.I_NAME, INWARD_MASTER.IWM_CODE, INWARD_MASTER.IWM_DATE, INWARD_MASTER.IWM_NO FROM INWARD_MASTER INNER JOIN EXICSE_ENTRY INNER JOIN EXCISE_DETAIL AS ex ON EXICSE_ENTRY.EX_CODE = ex.EXD_EX_CODE INNER JOIN EXCISE_TARIFF_MASTER INNER JOIN ITEM_MASTER ON EXCISE_TARIFF_MASTER.E_CODE = ITEM_MASTER.I_E_CODE ON ex.EXD_I_CODE = ITEM_MASTER.I_CODE ON  INWARD_MASTER.IWM_CODE = ex.EXD_IWM_CODE LEFT OUTER JOIN PARTY_MASTER AS PARTY_MASTER ON EXICSE_ENTRY.EX_P_CODE = PARTY_MASTER.P_CODE WHERE  " + Condition + "   (EXICSE_ENTRY.ES_DELETE = 0) AND (ex.EXD_I_CODE = (SELECT TOP (1) EXD_I_CODE FROM EXCISE_DETAIL WHERE (EXD_EX_CODE = EXICSE_ENTRY.EX_CODE)))ORDER BY EXICSE_ENTRY.EX_NO ";

            //if (Type == "1")
            //{
            //    Query = Query + " HAVING ISNULL(DCD_ORD_QTY,0) -(ISNULL(SUM(DND_REC_QTY),0)+ISNULL(SUM(DND_SCRAP_QTY),0))>0 ";
            //}


            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt = CommonClasses.Execute(Query);
            if (dt.Rows.Count > 0)
            {
                if (RepType == "SHOW")
                {


                    DataTable DTcompInfo = CommonClasses.Execute("select CM_CODE,CM_ID,CM_NAME,CM_ADDRESS1,CM_GST_NO from COMPANY_MASTER where CM_CODE=" + Convert.ToInt32(Session["CompanyCode"]) + " and CM_ACTIVE_IND=1");
                    ReportDocument rptname = null;
                    rptname = new ReportDocument();
                    //if (group == "Datewise")
                    //{
                    rptname.Load(Server.MapPath("~/Reports/rptExciseRegister.rpt"));
                    rptname.FileName = Server.MapPath("~/Reports/rptExciseRegister.rpt");
                    rptname.Refresh();
                    rptname.SetDataSource(dt);

                    rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());
                    rptname.SetParameterValue("txtDate", " From " + From + " To " + To);
                    rptname.SetParameterValue("txtCompAddress", DTcompInfo.Rows[0]["CM_ADDRESS1"]);
                    rptname.SetParameterValue("txtECCNO", DTcompInfo.Rows[0]["CM_GST_NO"]);

                    CrystalReportViewer1.ReportSource = rptname;
                }
                else
                {
                    try
                    {


                        DataTable dtResult = new DataTable();
                        dtResult = dt;
                        DataTable dtExport = new DataTable();
                        if (dt.Rows.Count > 0)
                        {
                            dtExport.Columns.Add("ENTRY NO");
                            dtExport.Columns.Add("ENTRY DATE");
                            dtExport.Columns.Add("BILL NO");
                            dtExport.Columns.Add("BILL DATE");
                            dtExport.Columns.Add("SUPPLIER NAME");
                            dtExport.Columns.Add("ECC No.");
                            dtExport.Columns.Add("TARIFF CODE");
                            dtExport.Columns.Add("DISCRIPTION");
                            dtExport.Columns.Add("GIN No.");
                            dtExport.Columns.Add("ASS VALUE");
                            dtExport.Columns.Add("CENVAT");
                            dtExport.Columns.Add("E.Cess");
                            dtExport.Columns.Add("S & H.");
                            dtExport.Columns.Add("ADI");
                            dtExport.Columns.Add("CENVAT ");
                            dtExport.Columns.Add("E.Cess ");
                            dtExport.Columns.Add("S & H. ");

                            for (int i = 0; i < dtResult.Rows.Count; i++)
                            {
                                dtExport.Rows.Add(
                                                  dtResult.Rows[i]["EX_NO"].ToString(),
                                                 Convert.ToDateTime(dtResult.Rows[i]["EX_DATE"].ToString()).ToString("dd.MM.yyyy"),
                                                  dtResult.Rows[i]["EX_DOC_NO"].ToString(),
                                                   Convert.ToDateTime(dtResult.Rows[i]["EX_DOC_DATE"].ToString()).ToString("dd.MM.yyyy"),
                                                  dtResult.Rows[i]["P_NAME"].ToString(),
                                                  dtResult.Rows[i]["P_ECC_NO"].ToString(),
                                                  dtResult.Rows[i]["E_TARIFF_NO"].ToString(),
                                                  dtResult.Rows[i]["I_NAME"].ToString(),
                                                   dtResult.Rows[i]["IWM_NO"].ToString(),

                                                    dtResult.Rows[i]["EX_BASIC_AMT"].ToString(),
                                                  dtResult.Rows[i]["EX_EX_DUTY"].ToString(),
                                                  dtResult.Rows[i]["EX_EX_CESS"].ToString(),
                                                   dtResult.Rows[i]["EX_EX_HCESS"].ToString(),
                                                  ' ',
                                                  0, 0, 0
                                                 );

                            }



                        }

                        HttpContext.Current.Response.Clear();
                        HttpContext.Current.Response.ClearContent();
                        HttpContext.Current.Response.ClearHeaders();
                        HttpContext.Current.Response.Buffer = true;
                        HttpContext.Current.Response.ContentType = "application/ms-excel";
                        HttpContext.Current.Response.Write(@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">");

                        HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=ExciseRegister.xls");

                        HttpContext.Current.Response.Charset = "utf-8";
                        HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("windows-1250");
                        //sets font
                        HttpContext.Current.Response.Write("<font style='font-size:10.0pt; font-family:Calibri;'>");
                        HttpContext.Current.Response.Write("<BR><BR><BR>");
                        HttpContext.Current.Response.Write("<Table border='1' bgColor='#ffffff' " +
                        "borderColor='#000000' cellSpacing='0' cellPadding='0' " +
                        "style='font-size:10.0pt; font-family:Calibri; background:white;'> <TR>");
                        //am getting my grid's column headers
                        int columnscount = dtExport.Columns.Count;
                        for (int j = 0; j < columnscount; j++)
                        {      //write in new column
                            HttpContext.Current.Response.Write("<Td>");
                            //Get column headers  and make it as bold in excel columns
                            HttpContext.Current.Response.Write("<B>");
                            HttpContext.Current.Response.Write(dtExport.Columns[j].ColumnName.ToString());
                            HttpContext.Current.Response.Write("</B>");
                            HttpContext.Current.Response.Write("</Td>");
                        }

                        HttpContext.Current.Response.Write("</TR>");
                        for (int k = 0; k < dtExport.Rows.Count; k++)
                        {//write in new row

                            HttpContext.Current.Response.Write("<TR>");
                            for (int i = 0; i < dtExport.Columns.Count; i++)
                            {
                                if (i == dtExport.Columns.Count - 1)
                                {
                                    HttpContext.Current.Response.Write("<Td style=\"display:none;\">");
                                    HttpContext.Current.Response.Write(Convert.ToString(dtExport.Rows[k][i].ToString()));
                                    HttpContext.Current.Response.Write("</Td>");
                                }
                                else
                                {

                                    if (i == 5)
                                    {
                                        //if (dtExport.Rows[k]["Item Code"].ToString().Length > 0)
                                        //{
                                        //    HttpContext.Current.Response.Write(@"<Td style='mso-number-format:\@'>");
                                        //}
                                        //else
                                        //{
                                        HttpContext.Current.Response.Write("<Td>");
                                        // }
                                    }
                                    else
                                    {
                                        HttpContext.Current.Response.Write("<Td>");
                                    }
                                    HttpContext.Current.Response.Write(Convert.ToString(dtExport.Rows[k][i].ToString()));
                                    HttpContext.Current.Response.Write("</Td>");
                                }
                            }
                            HttpContext.Current.Response.Write("</TR>");
                        }
                        HttpContext.Current.Response.Write("</Table>");
                        HttpContext.Current.Response.Write("</font>");
                        HttpContext.Current.Response.Flush();
                         
                       HttpContext.Current.Response.End();
                    }
                    catch(Exception ex )
                    {

                    }
                   
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
