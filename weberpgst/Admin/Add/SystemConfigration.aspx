﻿<%@ Page Title="System Configration" Language="C#" MasterPageFile="~/main.master"
    AutoEventWireup="true" CodeFile="SystemConfigration.aspx.cs" Inherits="Admin_Add_SystemConfigration" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="Server">
    <style type="text/css">
        .ajax__calendar_container
        {
            z-index: 1000;
        }
    </style>

    <script type="text/javascript">
        function oknumber(sender, e) {
            $find('ModalPopupPrintSelection').hide();
            __doPostBack('Button5', e);
        }
        function oncancel(sender, e) {
            $find('ModalPopupPrintSelection').hide();
            __doPostBack('Button6', e);
        }
        
    </script>

    <!-- BEGIN CONTENT -->
    <div class="page-content-wrapper">
        <div class="page-content">
            <!-- BEGIN PAGE CONTENT-->
            <div class="row">
                <div class="col-md-12">
                    <div id="Avisos">
                    </div>
                    <div class="row">
                        <div class="col-md-1">
                        </div>
                        <div class="col-md-12">
                            <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                <ContentTemplate>
                                    <asp:Panel ID="PanelMsg" runat="server" Visible="false" Style="background-color: #feefb3;
                                        height: 50px; width: 100%; border: 1px solid #9f6000">
                                        <div style="vertical-align: middle; margin-top: 10px;">
                                            <asp:Label ID="lblmsg" runat="server" Style="color: #9f6000; font-size: medium; font-weight: bold;
                                                margin-top: 50px; margin-left: 10px;"></asp:Label>
                                        </div>
                                    </asp:Panel>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                    <br>
                    <div class="portlet box green">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>System Configration
                            </div>
                            <div class="tools">
                                <a href="javascript:;" class="collapse"></a>
                                <asp:LinkButton ID="btnClose" CssClass="remove" runat="server" OnClick="btnCancel_Click"> </asp:LinkButton>
                            </div>
                        </div>
                        <!-- Page Body -->
                        <div class="portlet-body form">
                            <div class="form-horizontal">
                                <div class="form-body">
                                    <h4 class="form-section">
                                        Tally Name Selection</h4>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-4 control-label">
                                                    <span class="required">*</span> Discount Account</label>
                                                <div class="col-md-4">
                                                    <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                                        <ContentTemplate>
                                                            <asp:DropDownList runat="server" TabIndex="1" ID="ddlTallyDisc" AutoPostBack="True"
                                                                CssClass="select2_category form-control" MsgObrigatorio="Discount Account">
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <!--/row-->
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-4 control-label">
                                                    <span class="required">*</span> Packing Account</label>
                                                <div class="col-md-4">
                                                    <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                                        <ContentTemplate>
                                                            <asp:DropDownList runat="server" TabIndex="2" ID="ddlTallyPackAmt" AutoPostBack="True"
                                                                CssClass="select2_category form-control" MsgObrigatorio="Packing Account">
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <!--/row-->
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-4 control-label">
                                                    <span class="required">*</span> Other Charges Account</label>
                                                <div class="col-md-4">
                                                    <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                                                        <ContentTemplate>
                                                            <asp:DropDownList runat="server" TabIndex="3" ID="ddlTallyOther" AutoPostBack="True"
                                                                CssClass="select2_category form-control" MsgObrigatorio="Other Charges Account">
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <!--/row-->
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-4 control-label">
                                                    <span class="required">*</span> Freight Account</label>
                                                <div class="col-md-4">
                                                    <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                                                        <ContentTemplate>
                                                            <asp:DropDownList runat="server" TabIndex="4" ID="ddlTallyFreight" AutoPostBack="True"
                                                                CssClass="select2_category form-control" MsgObrigatorio="Freight Account">
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <!--/row-->
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-4 control-label">
                                                    <span class="required">*</span> Insurance Account</label>
                                                <div class="col-md-4">
                                                    <asp:UpdatePanel ID="UpdatePanel11" runat="server">
                                                        <ContentTemplate>
                                                            <asp:DropDownList runat="server" TabIndex="5" ID="ddlTallyInsurance" AutoPostBack="True"
                                                                CssClass="select2_category form-control" MsgObrigatorio="Insurance Account">
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <!--/row-->
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-4 control-label">
                                                    <span class="required">*</span> Transport Account</label>
                                                <div class="col-md-4">
                                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                        <ContentTemplate>
                                                            <asp:DropDownList runat="server" TabIndex="5" ID="ddlTallyTrans" AutoPostBack="True"
                                                                CssClass="select2_category form-control" MsgObrigatorio="Transport Account">
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <!--/row-->
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-4 control-label">
                                                    <span class="required">*</span> Octri Account</label>
                                                <div class="col-md-4">
                                                    <asp:UpdatePanel ID="UpdatePanel12" runat="server">
                                                        <ContentTemplate>
                                                            <asp:DropDownList runat="server" TabIndex="6" ID="ddlTallyOctri" AutoPostBack="True"
                                                                CssClass="select2_category form-control" MsgObrigatorio="Octri Account">
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <!--/row-->
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-4 control-label">
                                                    <span class="required">*</span>TCS Amount</label>
                                                <div class="col-md-4">
                                                    <asp:UpdatePanel ID="UpdatePanel13" runat="server">
                                                        <ContentTemplate>
                                                            <asp:DropDownList runat="server" TabIndex="7" ID="ddlTallyTcs" AutoPostBack="True"
                                                                CssClass="select2_category form-control" MsgObrigatorio="TCS Amount">
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <!--/row-->
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="col-md-4 control-label">
                                                    <span class="required">*</span> Advance Duty Account</label>
                                                <div class="col-md-4">
                                                    <asp:UpdatePanel ID="UpdatePanel14" runat="server">
                                                        <ContentTemplate>
                                                            <asp:DropDownList runat="server" TabIndex="8" ID="ddlTallyAdvDuty" AutoPostBack="True"
                                                                CssClass="select2_category form-control" MsgObrigatorio="Advance Duty Account">
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <!--/row-->
                                    <asp:UpdatePanel runat="server" ID="UpdatePanel28">
                                        <ContentTemplate>
                                            <asp:LinkButton ID="CheckCondition" runat="server" BackColor="" CssClass="formlabel"></asp:LinkButton>
                                            <cc1:ModalPopupExtender runat="server" ID="ModalPopupPrintSelection" BackgroundCssClass="modalBackground"
                                                OnOkScript="oknumber()" OnCancelScript="oncancel()" DynamicServicePath="" Enabled="True"
                                                PopupControlID="popUpPanel5" TargetControlID="CheckCondition">
                                            </cc1:ModalPopupExtender>
                                            <asp:Panel ID="popUpPanel5" runat="server" Style="display: none;">
                                                <div class="portlet box blue">
                                                    <div class="portlet-title">
                                                        <div class="captionPopup">
                                                            Alert
                                                        </div>
                                                    </div>
                                                    <div class="portlet-body form">
                                                        <div class="form-horizontal">
                                                            <div class="form-body">
                                                                <div class="row">
                                                                    <label class="col-md-12 control-label">
                                                                        Do you want to cancel record ?
                                                                    </label>
                                                                </div>
                                                                <div class="row">
                                                                    <div class="col-md-offset-3 col-md-9">
                                                                        <asp:LinkButton ID="Button5" CssClass="btn blue" TabIndex="26" runat="server" Visible="true"
                                                                            OnClick="btnOk_Click">  Yes </asp:LinkButton>
                                                                        <asp:LinkButton ID="Button6" CssClass="btn default" TabIndex="28" runat="server"
                                                                            OnClick="btnCancel1_Click"> No</asp:LinkButton>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </asp:Panel>
                                            <div class="form-actions fluid">
                                                <div class="col-md-offset-4 col-md-9">
                                                    <asp:LinkButton ID="btnSubmit" CssClass="btn green" TabIndex="28" runat="server"
                                                        OnClientClick="return VerificaCamposObrigatorios();" OnClick="btnSubmit_Click"><i class="fa fa-check-square"> </i>  Save </asp:LinkButton>
                                                    <asp:LinkButton ID="btnCancel" CssClass="btn default" TabIndex="29" runat="server"
                                                        OnClick="btnCancel_Click"><i class="fa fa-refresh"> </i> Cancel</asp:LinkButton>
                                                </div>
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                    <!--/row-->
                                </div>
                            </div>
                        </div>
                        <!-- End  Page Body -->
                    </div>
                </div>
            </div>
        </div>
        <!-- END PAGE CONTENT-->
    </div>
    <!-- END CONTENT -->
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

    <script type="text/javascript">
        function VerificaCamposObrigatorios() {
            try {
                if (VerificaValorCombo('#<%=ddlTallyPackAmt.ClientID%>', '#Avisos') == false) {
                    return false;
                }
                else if (VerificaValorCombo('#<%=ddlTallyOther.ClientID%>', '#Avisos') == false) {
                    return false;
                }
                else if (VerificaValorCombo('#<%=ddlTallyFreight.ClientID%>', '#Avisos') == false) {
                    return false;
                }
                else if (VerificaValorCombo('#<%=ddlTallyInsurance.ClientID%>', '#Avisos') == false) {
                    return false;
                }
                else if (VerificaValorCombo('#<%=ddlTallyOctri.ClientID%>', '#Avisos') == false) {
                    return false;
                }
                else if (VerificaValorCombo('#<%=ddlTallyTcs.ClientID%>', '#Avisos') == false) {
                    return false;
                }
                else if (VerificaValorCombo('#<%=ddlTallyAdvDuty.ClientID%>', '#Avisos') == false) {
                    return false;
                }
                else if (VerificaValorCombo('#<%=ddlTallyTrans.ClientID%>', '#Avisos') == false) {
                    return false;
                }
                else {
                    return true;
                }
            }
            catch (err) {
                alert('Erro in Required Fields: ' + err.description);
                return false;
            }
        }
    </script>

    <!-- END JAVASCRIPTS -->
</asp:Content>
