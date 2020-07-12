﻿<%@ Page Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="PurchaseDefault.aspx.cs"
    Inherits="Masters_PurchaseDefault" Title="Purchase" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="Server">
    <style type="text/css">
        .ajax__calendar_container
        {
            z-index: 1000;
        }
        .ModalPopupBG
        {
            background-color: #666699;
            filter: alpha(opacity=20);
            opacity: 0.2;
        }
    </style>

    <script type="text/javascript">
        function oknumber(sender, e) {
            $find('ModalPopupMsg').hide();
            __doPostBack('Button5', e);
        }
        
    </script>

    <asp:UpdatePanel runat="server" ID="UpdatePanel2">
        <ContentTemplate>
            <asp:LinkButton ID="CheckCondition" runat="server" BackColor="" CssClass="formlabel"></asp:LinkButton>
            <cc1:ModalPopupExtender runat="server" ID="ModalPopupMsg" BackgroundCssClass="ModalPopupBG"
                OnOkScript="oknumber()" CancelControlID="Button7" DynamicServicePath="" Enabled="True"
                PopupControlID="popUpPanel5" TargetControlID="CheckCondition">
            </cc1:ModalPopupExtender>
            <asp:Panel ID="popUpPanel5" runat="server" Style="display: none;">
                <div class="portlet box blue">
                    <div class="portlet-title">
                        <div style="font-size: medium;" class="captionPopup">
                            Warning
                        </div>
                    </div>
                    <div class="portlet-body form">
                        <div class="form-horizontal">
                            <div class="form-body">
                                <div class="row">
                                    <label style="font-size: medium;" class="col-md-12 control-label">
                                        You Have No Right To View
                                    </label>
                                </div>
                                <div class="row">
                                    <div class="col-md-9">
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-9">
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-offset-2 col-md-10">
                                        <asp:LinkButton ID="Button5" CssClass="btn blue" TabIndex="26" runat="server" Visible="true"
                                            OnClick="btnOk_Click">  OK </asp:LinkButton>
                                        <asp:LinkButton ID="Button7" CssClass="btn default" TabIndex="28" runat="server"> Cancel</asp:LinkButton>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="page-content-wrapper">
        <div class="page-content">
            <!-- BEGIN PAGE CONTENT-->
            <div class="row">
                <div class="col-md-12">
                    <div class="row">
                        <div class="col-md-4 sidebar-content ">
                            <ul class="ver-inline-menu tabbable margin-bottom-25">
                                <li class="active"><a href="#" data-toggle="tab"><i class="fa fa-briefcase"></i>Masters</a>
                                    <span class="after"></span></li>
                                <li id="POTypeMaster" runat="server"><a href="#" onserverclick="btnPoTypeMaster_click"
                                    runat="server"><i class="fa fa-tasks"></i>PO Type Master</a> </li>
                                <li id="SuppTypeMaster" runat="server"><a href="#" onserverclick="btnSupplierTypeMaster_click"
                                    runat="server"><i class="fa fa-tasks"></i>Supplier Type Master</a> </li>
                                <li id="SuppMaster" runat="server"><a href="#" onserverclick="btnSupplierMaster_click"
                                    runat="server"><i class="fa fa-tasks"></i>Supplier Master</a> </li>
                                <li><a href="#" onserverclick="btnItemMaster_click" runat="server"><i class="fa fa-tasks">
                                </i>Item Master</a> </li>
                                <li><a id="A12" href="#" onserverclick="btnProcess_click" runat="server"><i class="fa fa-tasks">
                                </i>Process Master</a> </li>
                                <li><a id="A14" href="#" onserverclick="btnProCode_click" runat="server"><i class="fa fa-tasks">
                                </i>Project Code Master</a> </li>
                            </ul>
                        </div>
                        <div class="col-md-4 sidebar-content ">
                            <ul class="ver-inline-menu tabbable margin-bottom-25">
                                <li class="active"><a href="#" data-toggle="tab"><i class="fa fa-briefcase"></i>Transactions</a>
                                    <span class="after"></span></li>
                                <li id="PurOrd" runat="server"><a href="#" onserverclick="btnSupplierPO_click" runat="server">
                                    <i class="fa fa-tasks"></i>Purchase Order</a> </li>
                                <li id="Li2" runat="server"><a href="#" onserverclick="btnServicePO_click" runat="server">
                                    <i class="fa fa-tasks"></i>Service Purchase Order</a> </li>
                                <li id="Li1" runat="server"><a id="A13" href="#" onserverclick="btnSubContractorPO_click"
                                    runat="server"><i class="fa fa-tasks"></i>Sub Contractor Purchase Order</a>
                                </li>
                                <li id="Inwrd" runat="server"><a id="A2" href="#" onserverclick="btnMaterialInward_click"
                                    visible="false" runat="server"><i class="fa fa-tasks"></i>Material Inward</a>
                                </li>
                                <li id="Inspection" runat="server"><a id="A4" href="#" onserverclick="btnMaterialInspection_click"
                                    visible="false" runat="server"><i class="fa fa-tasks"></i>Material Inspection</a></li>
                                <li id="BillPass" runat="server"><a id="A3" href="#" onserverclick="btnBillPassing_click"
                                    runat="server"><i class="fa fa-tasks"></i>Bill Passing</a> </li>
                                    <li id="CustomerSchedule" runat="server"><a id="A15" href="#" onserverclick="btnCustomerSchedule_click"
                                    runat="server"><i class="fa fa-tasks"></i>Customer Schedule</a> </li>
                                <%--   <li><a id="A1" href="#" onserverclick="btnIssueToProduction_click" runat="server"><i
                                    class="fa fa-tasks"></i>Issue To Production</a> </li>--%>
                                <li id="PurReq" runat="server" visible="false"><a id="A5" href="#" onserverclick="btnPurReq_click"
                                    runat="server"><i class="fa fa-tasks"></i>Purchase Requisition</a> </li>
                                <li id="PurRej" runat="server"><a id="A6" href="#" onserverclick="btnPurRej_click"
                                    runat="server"><i class="fa fa-tasks"></i>Purchase Rejection</a> </li>
                                    
                                <li id="AMC" runat="server" visible="false"><a id="A7" href="#" onserverclick="btnAMC_click"
                                    runat="server"><i class="fa fa-tasks"></i>Annual Maintenance Contract</a> </li>
                                <li id="WRK" runat="server" visible="false"><a id="A8" href="#" onserverclick="btnWO_click"
                                    runat="server"><i class="fa fa-tasks"></i>Work Order</a> </li>
                                <li id="POTransfer" runat="server" visible="false"><a id="A11" href="#" onserverclick="POTransfer_click"
                                    runat="server"><i class="fa fa-tasks"></i>PO Transfer</a> </li>
                            </ul>
                        </div>
                        <div class="col-md-4 sidebar-content ">
                            <ul class="ver-inline-menu tabbable margin-bottom-25">
                                <li class="active"><a href="#" data-toggle="tab"><i class="fa fa-briefcase"></i>Reports</a>
                                    <span class="after"></span></li>
                                <li id="POReg" runat="server"><a href="#" onserverclick="btnSupplierPoRegister_click"
                                    runat="server"><i class="fa fa-tasks"></i>Purchase Order Register</a> </li>
                                <li id="Li3" runat="server"><a href="#" onserverclick="btnServicePoRegister_click"
                                    runat="server"><i class="fa fa-tasks"></i>Service Purchase Order Register</a></li>
                                <li id="PoAmendReg" runat="server"><a id="A9" href="#" onserverclick="btnSupplierPoAmendRegister_click"
                                    runat="server"><i class="fa fa-tasks"></i>Purchase Order Amendment Register</a></li>
                                <li id="BillPassReg" runat="server"><a href="#" onserverclick="btnBillPassingRegister_click"
                                    runat="server"><i class="fa fa-tasks"></i>Bill Passing Register</a> </li>
                                <li id="PurReqReg" runat="server"><a id="A01" href="#" onserverclick="btnPurchaseRequisitionRegister_Click"
                                    runat="server"><i class="fa fa-tasks"></i>Purchase Requisition Register</a></li>
                                <li id="PurRejReg" runat="server"><a id="A10" href="#" onserverclick="btnPurchaseRejectionRegister_Click"
                                    runat="server"><i class="fa fa-tasks"></i>Purchase Rejection Register</a> </li>
                                    <li id="CstScheduleReport" runat="server"><a id="A16" href="#" onserverclick="btnScustSchedule_click"
                                    runat="server"><i class="fa fa-tasks"></i>Customer Schedule Register</a> </li>
                            </ul>
                        </div>
                    </div>
                </div>
            </div>
            <!-- END PAGE CONTENT-->
        </div>
    </div>
    <!-- BEGIN JAVASCRIPTS(Load javascripts at bottom, this will reduce page load time) -->
    <!-- BEGIN CORE PLUGINS -->
    <link href="../../assets/Avisos/Avisos.css" rel="stylesheet" type="text/css" />

    <script src="../../assets/Avisos/Avisos.js" type="text/javascript"></script>

    <script src="../../assets/JS/Util.js" type="text/javascript"></script>

    <script src="../../assets/scripts/jquery-1.7.1.min.js" type="text/javascript"></script>

    <script src="../../assets/plugins/jquery-1.10.2.min.js" type="text/javascript"></script>

    <script src="../../assets/plugins/jquery-migrate-1.2.1.min.js" type="text/javascript"></script>

    <script src="../../assets/plugins/bootstrap/js/bootstrap.min.js" type="text/javascript"></script>

    <script src="../../assets/plugins/bootstrap-hover-dropdown/twitter-bootstrap-hover-dropdown.min.js"
        type="text/javascript"></script>

    <script src="../../assets/plugins/jquery-slimscroll/jquery.slimscroll.min.js" type="text/javascript"></script>

    <script src="../../assets/plugins/jquery.blockui.min.js" type="text/javascript"></script>

    <script src="../../assets/plugins/jquery.cokie.min.js" type="text/javascript"></script>

    <script src="../../assets/plugins/uniform/jquery.uniform.min.js" type="text/javascript"></script>

    <!-- END CORE PLUGINS -->
    <!-- BEGIN PAGE LEVEL PLUGINS -->

    <script src="../../assets/plugins/bootstrap-daterangepicker/moment.min.js" type="text/javascript"></script>

    <script src="../../assets/plugins/bootstrap-daterangepicker/daterangepicker.js" type="text/javascript"></script>

    <script src="../../assets/plugins/gritter/js/jquery.gritter.js" type="text/javascript"></script>

    <!-- END PAGE LEVEL PLUGINS -->
    <!-- BEGIN PAGE LEVEL SCRIPTS -->

    <script src="../../assets/scripts/app.js" type="text/javascript"></script>

    <!-- END PAGE LEVEL SCRIPTS -->
</asp:Content>
