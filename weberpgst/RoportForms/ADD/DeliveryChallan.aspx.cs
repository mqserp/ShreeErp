﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using System.Data;

public partial class RoportForms_ADD_DeliveryChallan : System.Web.UI.Page
{
    DatabaseAccessLayer DL_DBAccess = null;
    string cpom_code = "";
    string print_type = "";

    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    #endregion Page_Load

    #region Page_Init
    protected void Page_Init(object sender, EventArgs e)
    {
        cpom_code = Request.QueryString[0];
        print_type = Request.QueryString[1];
        GenerateReport(cpom_code, print_type);
    }
    #endregion Page_Init

    #region btnCancel_Click
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/Transactions/VIEW/ViewCustomerPO.aspx", false);
        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Customer PO Report", "btnCancel_Click", Ex.Message);
        }
    }
    #endregion

    #region GenerateReport
    private void GenerateReport(string code, string p_type)
    {
        DL_DBAccess = new DatabaseAccessLayer();
        //DataTable dtfinal = CommonClasses.Execute("select EM_CODE ,EM_NAME ,BM_NAME,CONVERT(VARCHAR(11),EM_JOINDATE,113) as EM_JOINDATE,CONVERT(VARCHAR(11),FS_LEAVE_DATE,113) as FS_LEAVE_DATE,CONVERT(VARCHAR(11),FS_RESIGN_DATE,113) as FS_RESIGN_DATE,DS_NAME,FS_LAST_SAL,FS_BONUS_AMT,FS_LEAVE_AMT,FS_LTA_AMT,FS_ADV_AMT,FS_LOAN_AMT,FS_NOTICE_AMT,FS_FINAL_AMT,FS_PAYABLE_LEAVES as LeaveDay,FS_NOTICE_DAYS as NoticeDay from HR_FINAL_SETTLEMENT,HR_EMPLOYEE_MASTER,HR_DESINGNATION_MASTER,CM_BRANCH_MASTER where EM_CODE=FS_EM_CODE and BM_CODE=EM_BM_CODE and EM_DM_CODE=DS_CODE and FS_CODE=" + fsCode + " and FS_DELETE_FLAG=0 and FS_CM_COMP_ID=" + Session["CompanyId"].ToString() + "");
        try
        {
            DataTable dtfinal = new DataTable();
            DataTable dtType = new DataTable();
            dtType = CommonClasses.Execute(" SELECT DCM_TYPE FROM DELIVERY_CHALLAN_MASTER WHERE DCM_CODE='" + code + "'  ");
            if (dtType.Rows[0]["DCM_TYPE"].ToString() == "DLC")
            {
                //dtfinal = CommonClasses.Execute("SELECT DCM_NO,DCM_DATE,DCM_VEH_NO,case when DCM_IS_RETURNABLE =1 then 'Returnable' else 'NotReturnable' end as DCM_IS_RETURNABLE,DCM_ORDER_NO,DCD_ORD_QTY,I_NAME,P_NAME,P_ADD1,I_CODENO,DCD_REMARK FROM DELIVERY_CHALLAN_MASTER,DELIVERY_CHALLAN_DETAIL,PARTY_MASTER,ITEM_MASTER WHERE DCM_CODE=DCD_DCM_CODE AND P_CODE=DCM_P_CODE AND I_CODE=DCD_I_CODE AND DCM_CODE='" + code + "'");
                dtfinal = CommonClasses.Execute("SELECT I_CODE , I_INV_RATE ,ITEM_UNIT_MASTER.I_UOM_NAME , I_E_CODE , E_TARIFF_NO , DCM_NO ,DCM_DATE ,DCM_VEH_NO ,case when DCM_IS_RETURNABLE =1 then 'Returnable' else 'NotReturnable' end as DCM_IS_RETURNABLE,DCM_ORDER_NO, DCD_ORD_QTY, I_NAME, P_NAME, P_ADD1, I_CODENO, DCD_REMARK  FROM  DELIVERY_CHALLAN_MASTER , DELIVERY_CHALLAN_DETAIL, PARTY_MASTER, ITEM_MASTER ,ITEM_UNIT_MASTER , EXCISE_TARIFF_MASTER WHERE DCM_CODE = DCD_DCM_CODE AND P_CODE = DCM_P_CODE AND I_CODE = DCD_I_CODE AND DELIVERY_CHALLAN_DETAIL.DCD_UM_CODE = ITEM_UNIT_MASTER.I_UOM_CODE AND I_E_CODE = E_CODE AND ITEM_MASTER.ES_DELETE = 0 AND EXCISE_TARIFF_MASTER.ES_DELETE = 0 AND DCM_CODE='" + code + "' ");
            }
            else
            {
                dtfinal = CommonClasses.Execute(" SELECT DCM_NO,DCM_DATE,DCM_VEH_NO,case when DCM_IS_RETURNABLE =1 then 'Returnable' else 'NotReturnable' end as DCM_IS_RETURNABLE,DCM_ORDER_NO,SUM(DCD_ORD_QTY) AS DCD_ORD_QTY,'PLASTIC TRAY' AS I_NAME,P_NAME,P_ADD1, 'PLASTIC TRAY' AS I_CODENO,DCD_REMARK FROM DELIVERY_CHALLAN_MASTER,DELIVERY_CHALLAN_DETAIL,PARTY_MASTER,ITEM_MASTER WHERE DCM_CODE=DCD_DCM_CODE AND P_CODE=DCM_P_CODE AND I_CODE=DCD_I_CODE AND DCM_CODE='" + code + "' GROUP BY  DCM_NO,DCM_DATE,DCM_VEH_NO,  DCM_IS_RETURNABLE,DCM_ORDER_NO, P_NAME,P_ADD1 ,DCD_REMARK");
            }
            //  dtfinal = CommonClasses.Execute("SELECT DCM_NO,DCM_DATE,DCM_VEH_NO,case when DCM_IS_RETURNABLE =1 then 'Returnable' else 'NotReturnable' end as DCM_IS_RETURNABLE,DCM_ORDER_NO,DCD_ORD_QTY,I_NAME,P_NAME,P_ADD1,I_CODENO,DCD_REMARK FROM DELIVERY_CHALLAN_MASTER,DELIVERY_CHALLAN_DETAIL,PARTY_MASTER,ITEM_MASTER WHERE DCM_CODE=DCD_DCM_CODE AND P_CODE=DCM_P_CODE AND I_CODE=DCD_I_CODE AND DCM_CODE='" + code + "'");
            DataTable dtComp = CommonClasses.Execute("SELECT * FROM COMPANY_MASTER WHERE CM_ID='" + Session["CompanyId"] + "' AND ISNULL(CM_DELETE_FLAG,0) = '0' ");
            if (dtfinal.Rows.Count > 0)
            {
                if (p_type == "saleorder")
                {
                    ReportDocument rptname = null;
                    rptname = new ReportDocument();

                    rptname.Load(Server.MapPath("~/Reports/DelChallanHeimatec.rpt"));  //DelChallan  //DelChallanHeimatec 
                    rptname.FileName = Server.MapPath("~/Reports/DelChallanHeimatec.rpt"); //DelChallanHeimatecGST
                    rptname.Refresh();

                    rptname.SetDataSource(dtfinal);
                    rptname.SetParameterValue("txtCompName", Session["CompanyName"].ToString());
                    rptname.SetParameterValue("txtCompAdd", Session["CompanyAdd1"].ToString());
                    rptname.SetParameterValue("txtCompPhone", Session["CompanyPhone"].ToString());
                    rptname.SetParameterValue("txtCompfax", Session["Companyfax"].ToString());

                    //rptname.SetParameterValue("txtCompName", dtComp.Rows[0]["CM_NAME"].ToString());
                    //rptname.SetParameterValue("txtCompAdd", dtComp.Rows[0]["CM_ADDRESS1"].ToString());
                    //rptname.SetParameterValue("txtCompGSTIN", dtComp.Rows[0]["CM_GST_NO"].ToString());
                    //rptname.SetParameterValue("txtCompCIN", dtComp.Rows[0]["CM_CIN_NO"].ToString());
                    //rptname.SetParameterValue("txtCompTelPhone", dtComp.Rows[0]["CM_PHONENO1"].ToString());
                    //rptname.SetParameterValue("txtCompMobile", dtComp.Rows[0]["CM_PHONENO2"].ToString());
                    //rptname.SetParameterValue("txtCompEmail", dtComp.Rows[0]["CM_EMAILID"].ToString());
                    //rptname.SetParameterValue("txtCompWebsite", dtComp.Rows[0]["CM_WEBSITE"].ToString());
                    //string IsoNo = DL_DBAccess.GetColumn("select ISO_NO from ISONO_MASTER,CUSTPO_MASTER where CPOM_CODE='" + code + "' and ISO_SCREEN_NO=81 and ISO_WEF_DATE<=CPOM_DATE");
                    //if (IsoNo == "")
                    //{
                    rptname.SetParameterValue("txtIsoNo", " ");
                    //}
                    //else
                    //{
                    //    rptname.SetParameterValue("txtIsoNo", IsoNo.ToString());
                    //}
                    CrystalReportViewer1.ReportSource = rptname;
                }
            }

        }
        catch (Exception Ex)
        {
            CommonClasses.SendError("Delivery Challan Report", "GenerateReport", Ex.Message);

        }
    }
    #endregion
}
