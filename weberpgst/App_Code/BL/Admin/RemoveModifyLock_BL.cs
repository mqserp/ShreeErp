﻿using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.SqlClient;

public class RemoveModifyLock_BL
{
    #region "Data Members"
    DatabaseAccessLayer DL_DBAccess = null;
    DataSet ds = null;
    SqlParameter[] para = null;
    #endregion

    #region "Variables"
    int _LoginCode;
    string _TableCode;
    string _TableName;
    string _TableMod;
    #endregion

    #region "Properties"

    public string TableCode
    {
        get { return _TableCode; }
        set { _TableCode = value; }
    } 
    public string TableName
    {
        get { return _TableName; }
        set { _TableName = value; }
    }

    public string TableMod
    {
        get { return _TableMod; }
        set { _TableMod = value; }
    }

    public int LoginCode
    {
        get { return _LoginCode; }
        set { _LoginCode = value; }
    }

    public string Msg = "";
    #endregion

    #region Save
    public bool Save(GridView xgrid)
    {
        bool result = false;

        try
        {
            DL_DBAccess = new DatabaseAccessLayer();
            for (int i = 0; i < xgrid.Rows.Count; i++)
            {
                string str = "";
                if (((CheckBox)(xgrid.Rows[i].FindControl("chkRemoveDg"))).Checked == true)
                {
                    //str = str + "1";
                    //string Status = str;
                    int Code = Convert.ToInt32(((Label)(xgrid.Rows[i].FindControl("TabCode"))).Text);
                    // CommonClasses.Execute("Update " + TableName + " Set " + TableMod + " = 0 Where " + TableCode + "= " + Code + " ");

                    SqlParameter[] par = new SqlParameter[4];
                    par[0] = new SqlParameter("@Code", Code);
                    par[1] = new SqlParameter("@TableCode", TableCode);
                    par[2] = new SqlParameter("@TableName", TableName);
                    par[3] = new SqlParameter("@TableMod", TableMod);
                    result = DL_DBAccess.Insertion_Updation_Delete("SP_UpdateRemoveModifyLock", par);
                    if (result == true)
                    {
                        Msg = "Data Updated Successfully";
                    }
                    else
                    {
                        Msg = "Data Not Updated";
                    }
                }
            }
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Remove Modify Lock Class", "Save", ex.Message);
        }
        return result;
    }
    #endregion Save 

    #region SaveLogin
    public bool SaveLogin()
    {
        bool result = false;

        try
        {
            DL_DBAccess = new DatabaseAccessLayer();
            

                    SqlParameter[] par = new SqlParameter[1];
                    par[0] = new SqlParameter("@LoginCode", LoginCode);
                    result = DL_DBAccess.Insertion_Updation_Delete("SP_CM_UpdateRemoveLoginFlag", par);
                    if (result == true)
                    {
                        Msg = "Data Updated Successfully";
                    }
                
        }
        catch (Exception ex)
        {
            CommonClasses.SendError("Remove Modify Lock Class", "SaveLogin", ex.Message);
        }
        return result;
    }
    #endregion Save 




	
}
