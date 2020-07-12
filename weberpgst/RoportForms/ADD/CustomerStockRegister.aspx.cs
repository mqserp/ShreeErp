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


public partial class RoportForms_ADD_CustomerStockRegister : System.Web.UI.Page
{
    DatabaseAccessLayer DL_DBAccess = null;

    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    #endregion Page_Load

    #region Page_Init
    protected void Page_Init(object sender, EventArgs e)
    {
        try
        {
            string Title = Request.QueryString[0];
            //bool ChkAll = Convert.ToBoolean(Request.QueryString[1].ToString());
            //bool ChkComp = Convert.ToBoolean(Request.QueryString[2].ToString());
            string Condition = Request.QueryString[1].ToString();
             string From = Request.QueryString[2].ToString();
            string To = Request.QueryString[3].ToString();
            string type = Request.QueryString[4].ToString();
            string ttype = Request.QueryString[5].ToString();
            string WithVal = Request.QueryString[6].ToString();
            string Party = Request.QueryString[7].ToString();
            // string rpttype = Request.QueryString[6].ToString();
            if (From == "" && To == "")
            {
                From = Session["CompanyOpeningDate"].ToString();
                To = Session["CompanyClosingDate"].ToString();
            }

            //string i_name = Request.QueryString[5].ToString();
            //bool ChkCatAll = Convert.ToBoolean(Request.QueryString[7].ToString()); 
            //string category = Request.QueryString[8].ToString();
            //string detail = Request.QueryString[6].ToString();
            if (type == "O2ODetail")
            {
                if (Condition != "")
                {
                    Condition = "WHERE " + Condition;
                    //string sub = Condition.Substring(0, Condition.Length-3);
                    //Condition = sub;
                }
                GenerateReport(Title, "~/Reports/rptStockLedger.rpt", Condition, From, To, type, ttype, ttype);
            }
            //if (type == "O2OSummary")
            //{
            //    GenerateReport(Title, "~/Reports/rptStockLedger.rpt", Condition, From, To, type, ttype);
            //}
            if (type == "BOMDetail")
            {
                GenerateReport(Title, Condition, From, To, ttype, "Detail", Party);
            }
            //if (type == "BOMSummary")
            //{
            //    GenerateReport(Title, "~/Reports/rptStockLedger.rpt", Condition, From, To, type, ttype);      
            //}

            if (type == "Summary")
            {
                // GenerateReport(Title, Condition, From, To, ttype, type);
                GenerateReport1(Title, Condition, From, To, type, "0", WithVal);
            }
        }
        catch (Exception)
        {

        }
    }
    #endregion

    #region GenerateReport Deatils
    private void GenerateReport(string Title, string Condition, string From, string To, string ttype, string type, string Party)
    {
        DL_DBAccess = new DatabaseAccessLayer();
        string Query = "";
        DataTable dt = new DataTable();
        DataTable dtRec = new DataTable();

        dt = CommonClasses.Execute("DELETE FROM CHALLAN_STOCK_LEDGER_REPORT");
        dt.Clear();
        //dt = CommonClasses.Execute(" INSERT INTO CHALLAN_STOCK_LEDGER_REPORT(CL_CH_NO,CL_DATE,CL_P_CODE,P_NAME,CL_I_CODE,I_CODENO,I_NAME,UOM_NAME,CL_DOC_NO,CL_DOC_ID,CL_DOC_DATE,CL_CQTY,CL_DOC_TYPE,IWD_I_CODE,FINI_I_CODENO,FINI_I_NAME,FINI_UOM_NAME,IWD_SQTY,OP_BAL,CL_BAL,CL_BD_V_QTY,CL_CON_QTY,IWM_CHALLAN_NO,IWM_INWARD_TYPE)SELECT cast(CL_CH_NO as int) as CL_CH_NO ,CL_DATE,CL_P_CODE,P_NAME,CL_I_CODE,I.I_CODENO,I.I_NAME,U.I_UOM_NAME, CL_DOC_NO,CL_DOC_ID,CL_DOC_DATE,CL_CQTY,CL_DOC_TYPE,CL_ASSY_CODE as IWD_I_CODE,ITEM_MASTER.I_CODENO as FINI_I_CODENO,ITEM_MASTER.I_NAME as FINI_I_NAME,ASS_U.I_UOM_NAME, (SELECT IWD_SQTY FROM INWARD_DETAIL WHERE IWD_I_CODE = CHL.CL_ASSY_CODE AND IWD_IWM_CODE = CHL.CL_DOC_ID AND CHL.CL_DOC_TYPE='IWIAP') as IWD_SQTY,(SELECT isnull(SUM(CL_CQTY),0) FROM CHALLAN_STOCK_LEDGER COP WHERE COP.CL_I_CODE = CHL.CL_I_CODE AND COP.CL_P_CODE = CHL.CL_P_CODE AND CHL.CL_CH_NO = COP.CL_CH_NO AND COP.CL_DATE <= '" + Convert.ToDateTime(From) + "') AS OP_BAL,(SELECT isnull(SUM(CL_CQTY),0) FROM CHALLAN_STOCK_LEDGER COP WHERE COP.CL_I_CODE = CHL.CL_I_CODE AND COP.CL_P_CODE = CHL.CL_P_CODE AND CHL.CL_CH_NO = COP.CL_CH_NO) AS CL_BAL,(SELECT (IWD_SQTY * BD_VQTY ) AS BD_VQTY FROM INWARD_DETAIL,BOM_MASTER,BOM_DETAIL WHERE BM_CODE = BD_BM_CODE AND  BD_I_CODE = CHL.CL_I_CODE  AND BM_I_CODE = IWD_I_CODE AND IWD_I_CODE=CHL.CL_ASSY_CODE AND  IWD_IWM_CODE = CHL.CL_DOC_ID AND CHL.CL_DOC_TYPE='IWIAP' AND BOM_MASTER.ES_DELETE ='0') as BD_VQTY,CL_CON_QTY  , ISNULL((SELECT ISNULL(IWM_CHALLAN_NO,0) AS IWM_CHALLAN_NO  FROM INWARD_MASTER where   IWM_CODE=CL_DOC_ID  AND IWM_NO=CL_DOC_NO ),0) AS IWM_CHALLAN_NO,  ISNULL((SELECT ISNULL(IWM_INWARD_TYPE,0) FROM INWARD_MASTER where   IWM_CODE=CL_DOC_ID  AND IWM_NO=CL_DOC_NO ),0) AS IWM_INWARD_TYPE FROM CHALLAN_STOCK_LEDGER CHL LEFT OUTER JOIN  ITEM_MASTER  on ITEM_MASTER.I_CODE = CHL.CL_ASSY_CODE Left outer JOIN ITEM_UNIT_MASTER ASS_U on ASS_U.I_UOM_CODE = ITEM_MASTER.I_UOM_CODE,ITEM_MASTER I, ITEM_UNIT_MASTER U, PARTY_MASTER Where   CHL.CL_P_CODE = P_CODE And i.i_code = CHL.CL_I_CODE And I.I_UOM_CODE = U.I_UOM_CODE AND CL_DOC_DATE >= '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' AND CL_DOC_DATE <='" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "'    AND    P_TYPE=2   AND CL_DOC_TYPE IN ('IWIAP','OutSUBINM')  ORDER BY CL_P_CODE,CL_I_CODE,CL_DOC_DATE,CL_CH_NO");
        //dt = CommonClasses.Execute("INSERT INTO CHALLAN_STOCK_LEDGER_REPORT(CL_CH_NO,CL_DATE,CL_P_CODE,P_NAME,CL_I_CODE,I_CODENO,I_NAME,UOM_NAME,CL_DOC_NO,CL_DOC_ID,CL_DOC_DATE,CL_CQTY,CL_DOC_TYPE,IWD_I_CODE,FINI_I_CODENO,FINI_I_NAME,FINI_UOM_NAME,IWD_SQTY,OP_BAL,CL_BAL,CL_BD_V_QTY,CL_CON_QTY,IWM_CHALLAN_NO,IWM_INWARD_TYPE)SELECT cast(CL_CH_NO as int) as CL_CH_NO ,CL_DATE,CL_P_CODE,P_NAME,CL_I_CODE,I.I_CODENO,I.I_NAME,U.I_UOM_NAME, CL_DOC_NO,CL_DOC_ID,CL_DOC_DATE,CL_CQTY,CL_DOC_TYPE,CL_ASSY_CODE as IWD_I_CODE,ITEM_MASTER.I_CODENO as FINI_I_CODENO,ITEM_MASTER.I_NAME as FINI_I_NAME,ASS_U.I_UOM_NAME, (SELECT IWD_SQTY FROM INWARD_DETAIL WHERE IWD_I_CODE = CHL.CL_ASSY_CODE AND IWD_IWM_CODE = CHL.CL_DOC_ID AND CHL.CL_DOC_TYPE='IWIFP') as IWD_SQTY,(SELECT isnull(SUM(CL_CQTY),0) FROM CHALLAN_STOCK_LEDGER COP WHERE COP.CL_I_CODE = CHL.CL_I_CODE AND COP.CL_P_CODE = CHL.CL_P_CODE AND COP.CL_DOC_DATE < '" + Convert.ToDateTime(From) + "') AS OP_BAL,(SELECT isnull(SUM(CL_CQTY),0) FROM CHALLAN_STOCK_LEDGER COP WHERE COP.CL_I_CODE = CHL.CL_I_CODE AND COP.CL_P_CODE = CHL.CL_P_CODE AND COP.CL_DOC_DATE <='" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "' ) AS CL_BAL,(SELECT (IWD_SQTY * BD_VQTY ) AS BD_VQTY FROM INWARD_DETAIL,BOM_MASTER,BOM_DETAIL WHERE BM_CODE = BD_BM_CODE AND  BD_I_CODE = CHL.CL_I_CODE AND BM_I_CODE = IWD_I_CODE AND IWD_I_CODE=CHL.CL_ASSY_CODE AND IWD_IWM_CODE = CHL.CL_DOC_ID AND CHL.CL_DOC_TYPE='IWIFP' AND BOM_MASTER.ES_DELETE ='0') as BD_VQTY,CL_CON_QTY,ISNULL((SELECT ISNULL(IWM_CHALLAN_NO,0) AS IWM_CHALLAN_NO  FROM INWARD_MASTER where IWM_CODE=CL_DOC_ID AND IWM_NO=CL_DOC_NO ),0) AS IWM_CHALLAN_NO,ISNULL((SELECT ISNULL(IWM_INWARD_TYPE,0) FROM INWARD_MASTER where IWM_CODE=CL_DOC_ID AND IWM_NO=CL_DOC_NO ),0) AS IWM_INWARD_TYPE FROM CHALLAN_STOCK_LEDGER CHL LEFT OUTER JOIN ITEM_MASTER on ITEM_MASTER.I_CODE = CHL.CL_ASSY_CODE Left outer JOIN ITEM_UNIT_MASTER ASS_U on ASS_U.I_UOM_CODE = ITEM_MASTER.I_UOM_CODE,ITEM_MASTER I, ITEM_UNIT_MASTER U, PARTY_MASTER Where CHL.CL_P_CODE = P_CODE And i.i_code = CHL.CL_I_CODE And I.I_UOM_CODE = U.I_UOM_CODE AND CL_DOC_DATE >= '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' AND CL_DOC_DATE <='" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "' AND P_TYPE=1 AND CL_DOC_TYPE IN ('OutJWINM','IWIFP') ORDER BY CL_P_CODE,CL_I_CODE,CL_DOC_DATE,CL_CH_NO");
        //dt = CommonClasses.Execute("INSERT INTO CHALLAN_STOCK_LEDGER_REPORT(CL_CH_NO,CL_DATE,CL_P_CODE,P_NAME,CL_I_CODE,I_CODENO,I_NAME,UOM_NAME,CL_DOC_NO,CL_DOC_ID,CL_DOC_DATE,CL_CQTY,CL_DOC_TYPE,IWD_I_CODE,FINI_I_CODENO,FINI_I_NAME,FINI_UOM_NAME,IWD_SQTY,OP_BAL,CL_BAL,CL_BD_V_QTY,CL_CON_QTY,IWM_CHALLAN_NO,IWM_INWARD_TYPE)SELECT cast(CL_CH_NO as int) as CL_CH_NO ,CL_DATE,CL_P_CODE,P_NAME,CL_I_CODE,I.I_CODENO,I.I_NAME,U.I_UOM_NAME, CL_DOC_NO,CL_DOC_ID,CL_DOC_DATE,CL_CQTY,CL_DOC_TYPE,CL_ASSY_CODE as IWD_I_CODE,ITEM_MASTER.I_CODENO as FINI_I_CODENO,ITEM_MASTER.I_NAME as FINI_I_NAME,ASS_U.I_UOM_NAME,    (SELECT IND_INQTY FROM INVOICE_DETAIL WHERE IND_I_CODE = CHL.CL_I_CODE AND IND_INM_CODE = CHL.CL_DOC_ID AND CHL.CL_DOC_TYPE='OutJWINM') as IWD_SQTY,(SELECT isnull(SUM(CL_CQTY),0) FROM CHALLAN_STOCK_LEDGER COP WHERE COP.CL_I_CODE = CHL.CL_I_CODE AND COP.CL_P_CODE = CHL.CL_P_CODE AND COP.CL_DOC_DATE < '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "') AS OP_BAL,(SELECT isnull(SUM(CL_CQTY),0) FROM CHALLAN_STOCK_LEDGER COP WHERE COP.CL_I_CODE = CHL.CL_ASSY_CODE AND COP.CL_P_CODE = CHL.CL_P_CODE AND COP.CL_DOC_DATE <='" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "' ) AS CL_BAL,(SELECT (IWD_SQTY * BD_VQTY ) AS BD_VQTY FROM INWARD_DETAIL,BOM_MASTER,BOM_DETAIL WHERE BM_CODE = BD_BM_CODE AND  BD_I_CODE = CHL.CL_I_CODE AND BM_I_CODE = IWD_I_CODE AND IWD_I_CODE=CHL.CL_ASSY_CODE AND IWD_IWM_CODE = CHL.CL_DOC_ID AND CHL.CL_DOC_TYPE='IWIFP' AND BOM_MASTER.ES_DELETE ='0') as BD_VQTY,CL_CON_QTY,ISNULL((SELECT ISNULL(IWM_CHALLAN_NO,0) AS IWM_CHALLAN_NO  FROM INWARD_MASTER where IWM_CODE=CL_DOC_ID AND IWM_NO=CL_DOC_NO ),0) AS IWM_CHALLAN_NO,ISNULL((SELECT ISNULL(IWM_INWARD_TYPE,0) FROM INWARD_MASTER where IWM_CODE=CL_DOC_ID AND IWM_NO=CL_DOC_NO ),0) AS IWM_INWARD_TYPE FROM CHALLAN_STOCK_LEDGER CHL LEFT OUTER JOIN ITEM_MASTER on ITEM_MASTER.I_CODE = CHL.CL_ASSY_CODE Left outer JOIN ITEM_UNIT_MASTER ASS_U on ASS_U.I_UOM_CODE = ITEM_MASTER.I_UOM_CODE,ITEM_MASTER I, ITEM_UNIT_MASTER U, PARTY_MASTER Where CHL.CL_P_CODE = P_CODE And i.i_code = CHL.CL_I_CODE And I.I_UOM_CODE = U.I_UOM_CODE AND CL_DOC_DATE >= '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' AND CL_DOC_DATE <='" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "' AND P_TYPE=1 AND CL_DOC_TYPE IN ('OutJWINM','IWIFP')   ORDER BY CL_P_CODE,CL_I_CODE,CL_DOC_DATE,CL_CH_NO");
        dt = CommonClasses.Execute("INSERT INTO CHALLAN_STOCK_LEDGER_REPORT(CL_CH_NO,CL_DATE,CL_P_CODE,P_NAME,CL_I_CODE,I_CODENO,I_NAME,UOM_NAME,CL_DOC_NO,CL_DOC_ID,CL_DOC_DATE,CL_CQTY,CL_DOC_TYPE,IWD_I_CODE,FINI_I_CODENO,FINI_I_NAME,FINI_UOM_NAME,IWD_SQTY,OP_BAL,CL_BAL,CL_BD_V_QTY,CL_CON_QTY,IWM_CHALLAN_NO,IWM_INWARD_TYPE) SELECT  CAST(CHL.CL_CH_NO AS int) AS CL_CH_NO, CHL.CL_DATE, CHL.CL_P_CODE, PARTY_MASTER.P_NAME, CHL.CL_I_CODE, RAW.I_CODENO, RAW.I_NAME,  RAWUNIT.I_UOM_NAME, CHL.CL_DOC_NO, CHL.CL_DOC_ID, CHL.CL_DOC_DATE, CHL.CL_CQTY, CHL.CL_DOC_TYPE, CHL.CL_ASSY_CODE AS IWD_I_CODE,  ITEM_MASTER.I_CODENO AS FINI_I_CODENO, ITEM_MASTER.I_NAME AS FINI_I_NAME, ASS_U.I_UOM_NAME AS FINI_UOM_NAME, (SELECT IND_INQTY FROM INVOICE_DETAIL WHERE IND_I_CODE = CHL.CL_I_CODE AND IND_INM_CODE = CHL.CL_DOC_ID AND CHL.CL_DOC_TYPE='OutJWINM') as IWD_SQTY, (SELECT isnull(SUM(CL_CQTY),0) FROM CHALLAN_STOCK_LEDGER COP WHERE COP.CL_ASSY_CODE = CHL.CL_ASSY_CODE AND COP.CL_P_CODE = CHL.CL_P_CODE AND COP.CL_DOC_DATE < '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "') AS OP_BAL,(SELECT isnull(SUM(CL_CQTY),0) FROM CHALLAN_STOCK_LEDGER COP WHERE COP.CL_ASSY_CODE = CHL.CL_ASSY_CODE AND COP.CL_P_CODE = CHL.CL_P_CODE AND COP.CL_DOC_DATE <= '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "' ) AS CL_BAL, (SELECT (IND_INQTY * BD_VQTY ) AS BD_VQTY FROM INVOICE_DETAIL,BOM_MASTER,BOM_DETAIL WHERE BM_CODE = BD_BM_CODE AND  BD_I_CODE = CHL.CL_ASSY_CODE AND BM_I_CODE = IND_I_CODE AND IND_I_CODE=CHL.CL_I_CODE AND IND_INM_CODE = CHL.CL_DOC_ID AND CHL.CL_DOC_TYPE='OutJWINM' AND BOM_MASTER.ES_DELETE ='0') as BD_VQTY,CL_CON_QTY ,0 AS IWM_CHALLAN_NO,0 AS IWM_INWARD_TYPE   FROM ITEM_UNIT_MASTER AS RAWUNIT INNER JOIN ITEM_MASTER AS RAW ON RAWUNIT.I_UOM_CODE = RAW.I_UOM_CODE INNER JOIN CHALLAN_STOCK_LEDGER AS CHL LEFT OUTER JOIN ITEM_MASTER ON ITEM_MASTER.I_CODE = CHL.CL_I_CODE LEFT OUTER JOIN ITEM_UNIT_MASTER AS ASS_U ON ASS_U.I_UOM_CODE = ITEM_MASTER.I_UOM_CODE INNER JOIN PARTY_MASTER ON CHL.CL_P_CODE = PARTY_MASTER.P_CODE ON RAW.I_CODE = CHL.CL_ASSY_CODE WHERE     (CHL.CL_DOC_DATE >= '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "') AND (CHL.CL_DOC_DATE <=  '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "' ) AND (PARTY_MASTER.P_TYPE = 1) AND (CHL.CL_DOC_TYPE IN ('OutJWINM', 'IWIFP'))  ORDER BY CHL.CL_P_CODE, CHL.CL_I_CODE, CHL.CL_DOC_DATE, CL_CH_NO");
        dt.Clear();

        //SqlParameter[] par = new SqlParameter[2];
        //par[0] = new SqlParameter("@fromdate", Convert.ToDateTime(From));
        //par[1] = new SqlParameter("@todate", Convert.ToDateTime(To));
        ////par[2] = new SqlParameter("@IWM_CM_CODE", IWM_CM_CODE);
        //DL_DBAccess.Insertion_Updation_Delete("prGetOPCLBalance", par);
      
        double srpRcvQty = 0;
        dt.Clear();
        dt = CommonClasses.Execute("SELECT CL_CH_NO,CL_DATE,CL_P_CODE,CHALLAN_STOCK_LEDGER_REPORT.P_NAME,CL_I_CODE,CHALLAN_STOCK_LEDGER_REPORT.I_CODENO,CHALLAN_STOCK_LEDGER_REPORT.I_NAME,CL_DOC_NO,CL_DOC_DATE,CL_CQTY ,CL_DOC_TYPE,'' AS FINI_I_CODENO ,'' AS FINI_I_NAME ,0 AS IWD_SQTY , OP_BAL , CL_BAL,' ' AS IWM_CHALLAN_NO, 0 AS IWM_INWARD_TYPE FROM CHALLAN_STOCK_LEDGER_REPORT,PARTY_MASTER,ITEM_MASTER where " + Condition + " P_CODE=CL_P_CODE AND CL_I_CODE=I_CODE AND PARTY_MASTER.ES_DELETE=0 AND CL_DOC_TYPE='IWIFP' UNION SELECT 0 AS CL_CH_NO,CL_DOC_DATE AS CL_DATE,CL_P_CODE,PARTY_MASTER.P_NAME,0 AS CL_I_CODE,CHALLAN_STOCK_LEDGER_REPORT.I_CODENO,CHALLAN_STOCK_LEDGER_REPORT.I_NAME,CL_DOC_NO,CL_DOC_DATE,SUM(CL_CQTY) AS CL_CQTY,CL_DOC_TYPE,FINI_I_CODENO,FINI_I_NAME ,ISNULL(IWD_SQTY,0) AS IWD_SQTY,OP_BAL,CL_BAL,IWM_CHALLAN_NO,IWM_INWARD_TYPE FROM CHALLAN_STOCK_LEDGER_REPORT,PARTY_MASTER,ITEM_MASTER where " + Condition + " P_CODE=CL_P_CODE AND CL_I_CODE=I_CODE AND PARTY_MASTER.ES_DELETE=0 AND CL_DOC_TYPE='OutJWINM' GROUP BY CL_DOC_NO,CL_DOC_DATE,CL_DOC_TYPE,FINI_I_CODENO,FINI_I_NAME,OP_BAL,CL_BAL,IWM_CHALLAN_NO,IWM_INWARD_TYPE,IWD_SQTY,CHALLAN_STOCK_LEDGER_REPORT.I_CODENO,CHALLAN_STOCK_LEDGER_REPORT.I_NAME,CL_P_CODE,PARTY_MASTER.P_NAME ORDER BY CL_DOC_DATE,CL_DOC_NO");

        if (dt.Rows.Count > 0)
        {
            ReportDocument rptname = null;
            rptname = new ReportDocument();

            rptname.Load(Server.MapPath("~/Reports/rptCustStockBOMDetailNew.rpt")); //rptVendorStockBOMDetails1  rptCustomerStockBOMDetails1
            rptname.FileName = Server.MapPath("~/Reports/rptCustStockBOMDetailNew.rpt"); //rptVendorStockBOMDetails1  rptCustomerStockBOMDetails1
            rptname.Refresh();

            rptname.SetDataSource(dt);
            rptname.SetParameterValue("Comp", Session["CompanyName"].ToString());
            rptname.SetParameterValue("Title", "Customer Stock Detail Report");
            rptname.SetParameterValue("type", ttype);
            rptname.SetParameterValue("Date", " From " + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + " To " + Convert.ToDateTime(To).ToString("dd/MMM/yyyy"));
            CrystalReportViewer1.ReportSource = rptname;
        }
        else
        {
            PanelMsg.Visible = true;
            lblmsg.Text = "Record Not Found";
            return;
        }
    }
    #endregion

    #region GenerateReport
    private void GenerateReport(string Title, string File, string Condition, string From, string To, string type, string ttypr, string acd)
    {
        DL_DBAccess = new DatabaseAccessLayer();
        string Query = "";
        DataTable dt = new DataTable();
        DataTable dtRec = new DataTable();

        dt = CommonClasses.Execute("DELETE FROM TEMP_CHAL_STOCK_LEDGER");
        dt.Clear();
        dt = CommonClasses.Execute("INSERT INTO TEMP_CHAL_STOCK_LEDGER (CL_NO,CL_CODE,CL_DATE,CL_P_CODE,CL_I_CODE,CL_QTY,CL_CH_NO,CL_CH_DATE,CL_DOC_TYPE)SELECT INM_NO,INM_CODE,INM_DATE,INM_P_CODE,IND_I_CODE,IND_INQTY,'','','OutSUBINM' FROM INVOICE_MASTER,INVOICE_DETAIL WHERE INM_TYPE ='OutSUBINM' AND INM_CODE = IND_INM_CODE AND INVOICE_MASTER.ES_DELETE = 0  ");
        dt.Clear();
        dt = CommonClasses.Execute("INSERT INTO TEMP_CHAL_STOCK_LEDGER (CL_NO,CL_CODE,CL_DATE,CL_P_CODE,CL_I_CODE,CL_QTY,CL_CH_NO,CL_CH_DATE,CL_DOC_TYPE) SELECT IWM_NO,IWM_CODE,IWM_DATE,IWM_P_CODE,IWD_I_CODE,-(IWD_REV_QTY),IWM_CHALLAN_NO,IWM_CHAL_DATE,'IWIAP' FROM INWARD_MASTER,INWARD_DETAIL WHERE IWM_CODE = IWD_IWM_CODE AND IWM_TYPE='OUTCUSTINV'AND INWARD_MASTER.ES_DELETE=0 AND IWM_INWARD_TYPE=1");
        dt.Clear();
        dt = CommonClasses.Execute("update CHALLAN_STOCK_LEDGER set CL_UM_CODE = null where CL_UM_CODE IS NOT NULL");
        dt.Clear();
        dt = CommonClasses.Execute("Delete from TEMP_OPCHAL_STOCK_LEDGER");
        dt.Clear();
        dt = CommonClasses.Execute("INSERT INTO TEMP_OPCHAL_STOCK_LEDGER (CL_NO,CL_CODE,CL_DATE,CL_P_CODE,CL_I_CODE,CL_QTY,CL_CH_NO,CL_CH_DATE,CL_DOC_TYPE,CL_OP_BAL) (SELECT CL_NO,CL_CODE,CL_DATE,CL_P_CODE,CL_I_CODE,CL_QTY,CL_CH_NO,CL_CH_DATE,CL_DOC_TYPE,(SELECT ISNULL(SUM(CL_QTY),0) FROM TEMP_CHAL_STOCK_LEDGER WHERE CL_I_CODE = I.CL_I_CODE AND CL_P_CODE = I.CL_P_CODE AND CL_DATE <= '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "')AS OPENING FROM TEMP_CHAL_STOCK_LEDGER AS I WHERE CL_DOC_TYPE IN ('OutSubINM','IWIAP'))");
        dt.Clear();
        dt = CommonClasses.Execute("update TEMP_OPCHAL_STOCK_LEDGER set cl_op_bal = 0 where cl_op_bal is null");
        dt.Clear();
        dt = CommonClasses.Execute("UPDATE TEMP_OPCHAL_STOCK_LEDGER SET CL_SCRP = BD_SQTY FROM BOM_MASTER,BOM_DETAIL WHERE BM_CODE = BD_BM_CODE AND CL_I_CODE = BM_I_CODE");


        double srpRcvQty = 0;
        dt.Clear();
        dt = CommonClasses.Execute("select distinct CL_I_CODE,CL_P_CODE from TEMP_OPCHAL_STOCK_LEDGER");
        if (dt.Rows.Count > 0)
        {
            if (type == "O2ODetail")
            {
                dtRec = CommonClasses.Execute("select ISNULL(sum(SIWD_QTY),0)as SIWD_QTY FROM SCRAP_INWARD_MASTER,SCRAP_INWARD_DETAIL WHERE SIWM_CODE = SIWD_SIWM_CODE AND SIWM_P_CODE = '" + dt.Rows[0]["CL_P_CODE"].ToString() + "' and SCRAP_INWARD_MASTER.ES_DELETE = 0");
                if (dt.Rows.Count > 0)
                {
                    //srpRcvQty = dtRec.Rows[0]["SIWD_QTY"].ToString();
                }
            }

            dt = CommonClasses.Execute("SELECT distinct TEMP_OPCHAL_STOCK_LEDGER.CL_NO,convert(varchar,TEMP_OPCHAL_STOCK_LEDGER.CL_DATE,106) as CL_DATE, TEMP_OPCHAL_STOCK_LEDGER.CL_P_CODE, TEMP_OPCHAL_STOCK_LEDGER.CL_I_CODE,I_CODENO, TEMP_OPCHAL_STOCK_LEDGER.CL_QTY, TEMP_OPCHAL_STOCK_LEDGER.CL_CH_NO,convert(varchar,TEMP_OPCHAL_STOCK_LEDGER.CL_CH_DATE,106) as CL_CH_DATE, TEMP_OPCHAL_STOCK_LEDGER.CL_DOC_TYPE, TEMP_OPCHAL_STOCK_LEDGER.CL_OP_BAL, 0 as CL_SCRP, 0 as CL_SCRP_RECVD, ITEM_MASTER.I_NAME, PARTY_MASTER.P_NAME FROM TEMP_OPCHAL_STOCK_LEDGER AS TEMP_OPCHAL_STOCK_LEDGER INNER JOIN PARTY_MASTER AS PARTY_MASTER ON TEMP_OPCHAL_STOCK_LEDGER.CL_P_CODE = PARTY_MASTER.P_CODE INNER JOIN ITEM_MASTER AS ITEM_MASTER ON TEMP_OPCHAL_STOCK_LEDGER.CL_I_CODE = ITEM_MASTER.I_CODE " + Condition + "   ITEM_MASTER.ES_DELETE=0 group by  TEMP_OPCHAL_STOCK_LEDGER.CL_P_CODE, TEMP_OPCHAL_STOCK_LEDGER.CL_I_CODE, TEMP_OPCHAL_STOCK_LEDGER.CL_DATE,TEMP_OPCHAL_STOCK_LEDGER.CL_NO,CL_QTY,CL_CH_NO,CL_CH_DATE,CL_DOC_TYPE,CL_OP_BAL,I_NAME,P_NAME,I_CODENO");
            // where " + Condition + "
            if (dt.Rows.Count > 0)
            {
                ReportDocument rptname = null;
                rptname = new ReportDocument();

                rptname.Load(Server.MapPath("~/Reports/rptVendorStockDetail.rpt"));
                rptname.FileName = Server.MapPath("~/Reports/rptVendorStockDetail.rpt");
                rptname.Refresh();

                rptname.SetDataSource(dt);
                rptname.SetParameterValue("Comp", Session["CompanyName"].ToString());
                rptname.SetParameterValue("Title", "Customer Stock Report From " + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + " To " + Convert.ToDateTime(To).ToString("dd/MMM/yyyy"));
                //rptname.SetParamererValue("",);
                //rptname.SetParamererValue("",Session["ShowBal"].ToString());
                CrystalReportViewer1.ReportSource = rptname;

            }
            else
            {
                PanelMsg.Visible = true;
                lblmsg.Text = "Record Not Found";
                return;
            }
        }
    }

    #endregion

    #region GenerateReport
    private void GenerateReport1(string Title, string Condition, string From, string To, string type, string ttypr, string WithVal)
    {
        DL_DBAccess = new DatabaseAccessLayer();
        string Query = "";
        DataTable dt = new DataTable();
        DataTable dtRec = new DataTable();
        if (ttypr == "0") //oo
        {
            // dt = CommonClasses.Execute("SELECT DISTINCT CASE WHEN INM_INVOICE_TYPE = '0' THEN 'AS PER BOM' ELSE 'One-One' END AS INM_TYPE,ITEM_MASTER.I_CODE,ITEM_MASTER.I_CODENO, ITEM_MASTER.I_NAME,PARTY_MASTER.P_NAME,PARTY_MASTER.P_CODE, ITEM_MASTER.I_UOM_CODE, ITEM_UNIT_MASTER.I_UOM_NAME,(SELECT ISNULL(SUM(CL_CQTY), 0) AS Expr1 FROM CHALLAN_STOCK_LEDGER WHERE (CL_P_CODE = PARTY_MASTER.P_CODE) AND (CL_I_CODE = ITEM_MASTER.I_CODE) AND (CL_DOC_TYPE IN ('OutJWINM', 'IWIFP')) AND (CL_DOC_DATE < '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "')) AS OP_QTY,(SELECT ISNULL(SUM(CL_CQTY), 0) AS Expr1 FROM CHALLAN_STOCK_LEDGER AS CHALLAN_STOCK_LEDGER_2 WHERE (CL_P_CODE = PARTY_MASTER.P_CODE) AND (CL_I_CODE = ITEM_MASTER.I_CODE) AND (CL_DOC_TYPE = 'OutJWINM') AND (CL_DOC_DATE BETWEEN  '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "')) AS OUT_QTY,(SELECT ISNULL(ABS(SUM(CL_CQTY)), 0) AS Expr1 FROM CHALLAN_STOCK_LEDGER AS CHALLAN_STOCK_LEDGER_1  WHERE (CL_P_CODE = PARTY_MASTER.P_CODE) AND (CL_I_CODE = ITEM_MASTER.I_CODE) AND (CL_DOC_TYPE = 'IWIFP') AND (CL_DOC_DATE BETWEEN '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "')) AS IN_QTY,I_INV_RATE,I_UWEIGHT FROM INVOICE_MASTER INNER JOIN INVOICE_DETAIL ON INVOICE_MASTER.INM_CODE = INVOICE_DETAIL.IND_INM_CODE INNER JOIN ITEM_MASTER ON INVOICE_DETAIL.IND_I_CODE = ITEM_MASTER.I_CODE INNER JOIN  PARTY_MASTER ON INVOICE_MASTER.INM_P_CODE = PARTY_MASTER.P_CODE INNER JOIN  ITEM_UNIT_MASTER ON ITEM_MASTER.I_UOM_CODE = ITEM_UNIT_MASTER.I_UOM_CODE WHERE " + Condition + " (INVOICE_MASTER.INM_TYPE = 'OutJWINM') AND (INVOICE_MASTER.ES_DELETE = 0) ORDER BY PARTY_MASTER.P_NAME");
            dt = CommonClasses.Execute("SELECT DISTINCT CASE WHEN IWM_INWARD_TYPE = '0' THEN 'AS PER BOM' ELSE 'One-One' END AS INM_TYPE,ITEM_MASTER.I_CODE,ITEM_MASTER.I_CODENO, ITEM_MASTER.I_NAME,PARTY_MASTER.P_NAME,PARTY_MASTER.P_CODE, ITEM_MASTER.I_UOM_CODE, ITEM_UNIT_MASTER.I_UOM_NAME, (SELECT ISNULL(SUM(CL_CQTY), 0) AS Expr1 FROM CHALLAN_STOCK_LEDGER WHERE " + Condition + " (CL_P_CODE = PARTY_MASTER.P_CODE) AND (CL_ASSY_CODE = ITEM_MASTER.I_CODE) AND (CL_DOC_TYPE IN ('OutJWINM', 'IWIFP')) AND (CL_DOC_DATE <= '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "')) AS OP_QTY,(SELECT ISNULL(ABS(SUM(CL_CQTY)), 0)  AS Expr1 FROM CHALLAN_STOCK_LEDGER AS CHALLAN_STOCK_LEDGER_2 WHERE (CL_P_CODE = PARTY_MASTER.P_CODE) AND (CL_ASSY_CODE = ITEM_MASTER.I_CODE) AND (CL_DOC_TYPE = 'OutJWINM') AND (CL_DOC_DATE BETWEEN  '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "')) AS OUT_QTY,(SELECT ISNULL(ABS(SUM(CL_CQTY)), 0) AS Expr1 FROM CHALLAN_STOCK_LEDGER AS CHALLAN_STOCK_LEDGER_1  WHERE (CL_P_CODE = PARTY_MASTER.P_CODE) AND (CL_I_CODE = ITEM_MASTER.I_CODE) AND (CL_DOC_TYPE = 'IWIFP') AND (CL_DOC_DATE BETWEEN '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "')) AS IN_QTY,I_INV_RATE,I_UWEIGHT FROM INWARD_MASTER INNER JOIN INWARD_DETAIL ON INWARD_MASTER.IWM_CODE = INWARD_DETAIL.IWD_IWM_CODE INNER JOIN ITEM_MASTER ON INWARD_DETAIL.IWD_I_CODE = ITEM_MASTER.I_CODE INNER JOIN  PARTY_MASTER ON INWARD_MASTER.IWM_P_CODE = PARTY_MASTER.P_CODE INNER JOIN  ITEM_UNIT_MASTER ON ITEM_MASTER.I_UOM_CODE = ITEM_UNIT_MASTER.I_UOM_CODE WHERE " + Condition + " (INWARD_MASTER.IWM_TYPE = 'IWIFP') AND (INWARD_MASTER.ES_DELETE = 0) ORDER BY PARTY_MASTER.P_NAME");
        }
        else
        {
            dt = CommonClasses.Execute("SELECT DISTINCT CASE when INM_INVOICE_TYPE='0' then 'AS PER BOM' ELSE 'One-One' END AS  INM_TYPE ,I_CODE,I_CODENO,I_NAME,P_NAME,P_CODE,SPOM_PONO,SPOM_CODE, ITEM_MASTER.I_UOM_CODE,I_UOM_NAME,(SELECT  ISNULL(SUM((CL_CQTY)),0) FROM CHALLAN_STOCK_LEDGER WHERE " + Condition + "  CL_P_CODE=P_CODE AND CL_I_CODE=I_CODE AND CL_DOC_TYPE IN ('OutJWINM','IWIFP') AND CL_DOC_DATE <'" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' ) AS OP_QTY,(SELECT  ISNULL(SUM(CL_CQTY),0) FROM CHALLAN_STOCK_LEDGER where CL_P_CODE=P_CODE AND CL_I_CODE=I_CODE AND CL_DOC_TYPE='OutJWINM' AND CL_DOC_DATE BETWEEN  '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "') AS OUT_QTY,(SELECT ISNULL(abs(SUM(CL_CQTY)),0) FROM CHALLAN_STOCK_LEDGER where CL_P_CODE=P_CODE AND CL_I_CODE=I_CODE AND CL_DOC_TYPE='IWIFP' AND CL_DOC_DATE BETWEEN '" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + "' AND '" + Convert.ToDateTime(To).ToString("dd/MMM/yyyy") + "') AS IN_QTY FROM INVOICE_MASTER,INVOICE_DETAIL ,ITEM_MASTER,PARTY_MASTER,SUPP_PO_MASTER,SUPP_PO_DETAILS,ITEM_UNIT_MASTER ,BOM_MASTER,BOM_DETAIL where " + Condition + " INM_TYPE='OutJWINM' AND INM_INVOICE_TYPE=0 AND INM_CODE=IND_INM_CODE AND INVOICE_MASTER.ES_DELETE=0  AND BD_I_CODE=IND_I_CODE AND P_CODE=INM_P_CODE AND SPOD_SPOM_CODE=SPOM_CODE AND IND_CPOM_CODE=SPOM_CODE AND SUPP_PO_MASTER.SPOM_POTYPE=1 AND SPOD_I_CODE=BM_I_CODE AND ITEM_UNIT_MASTER.I_UOM_CODE=ITEM_MASTER.I_UOM_CODE AND BM_CODE=BD_BM_CODE AND BD_I_CODE=I_CODE");
        }
        if (dt.Rows.Count > 0)
        {
            ReportDocument rptname = null;
            rptname = new ReportDocument();

            if (WithVal == "WithAmt")
            {
                rptname.Load(Server.MapPath("~/Reports/rptCustomerStockSummaryWithVal.rpt"));  //rptVendorStockSummaryWithVal
                rptname.FileName = Server.MapPath("~/Reports/rptCustomerStockSummaryWithVal.rpt");  //rptVendorStockSummaryWithVal
                rptname.Refresh();

                rptname.SetDataSource(dt);
                rptname.SetParameterValue("Comp", Session["CompanyName"].ToString());
                rptname.SetParameterValue("Title", "Customer Stock Report Summary(With Value) From" + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + " To " + Convert.ToDateTime(To).ToString("dd/MMM/yyyy"));
                rptname.SetParameterValue("txtWithVal", "WithVal");
            }
            else
            {
                rptname.Load(Server.MapPath("~/Reports/rptCustomerStockSummary.rpt"));  //rptVendorStockSummary
                rptname.FileName = Server.MapPath("~/Reports/rptCustomerStockSummary.rpt"); //rptVendorStockSummary
                rptname.Refresh();

                rptname.SetDataSource(dt);
                rptname.SetParameterValue("txtWithVal", "");
                rptname.SetParameterValue("Comp", Session["CompanyName"].ToString());
                rptname.SetParameterValue("Title", "Customer Stock Report Summary From " + Convert.ToDateTime(From).ToString("dd/MMM/yyyy") + " To " + Convert.ToDateTime(To).ToString("dd/MMM/yyyy"));
            }
            //rptname.SetParamererValue("",);
            //rptname.SetParamererValue("",Session["ShowBal"].ToString());
            CrystalReportViewer1.ReportSource = rptname;
        }
        else
        {
            PanelMsg.Visible = true;
            lblmsg.Text = "Record Not Found";
            return;
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
            CommonClasses.SendError("Stock Ledger Register", "ShowMessage", Ex.Message);
            return false;
        }
    }
    #endregion

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/RoportForms/VIEW/CustomerStockRegister.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Stock Ledger Register", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion btnCancel_Click
}
