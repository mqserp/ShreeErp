﻿<%@ Page Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="GST_HSNSAC_Master.aspx.cs"
    Inherits="Masters_ADD_GST_HSNSAC_Master" Title="GST HSN/SAC Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="Server">
    <style type="text/css">
        .ajax__calendar_container
        {
            z-index: 1000;
        }
        .modalBackground
        {
            background-color: #8B8B8B;
            filter: alpha(opacity=70);
            opacity: 0.7;
        }
    </style>

    <script type="text/javascript">

        function oknumber1(sender, e) {
            $find('ModalPopupPrintSelection').hide();
            __doPostBack('Button5', e);
        }
        function oncancel(sender, e) {
            $find('ModalPopupPrintSelection').hide();
            __doPostBack('Button6', e);
        }
    </script>

    <script type="text/javascript">
        function Showalert() {
            $('#Avisos').fadeIn(6000)
            $('#Avisos').fadeOut(6000)
            $("#up").click();
        }
    </script>

    <script type="text/javascript">
        function Showalert1() {
            $('#MSG').fadeIn(6000)
            $('#MSG').fadeOut(6000)
            $("#up").click();
        }
    </script>

    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function(evt, args) {
            jQuery("#<%=ddlTallyBasic.ClientID %>").select2();
            jQuery("#<%=ddlTallySpecial.ClientID %>").select2();
            jQuery("#<%=ddlTallyEdu.ClientID %>").select2();
            jQuery("#<%=ddlTallySHEdu.ClientID %>").select2();
        });
    </script>

    <div class="page-content-wrapper">
        <div class="page-content">
            <!-- BEGIN PAGE CONTENT-->
            <div class="row">
                <div class="col-md-12">
                    <div id="Avisos">
                    </div>
                </div>
            </div>
            <div class="row">
                <div id="MSG" class="col-md-12">
                    <asp:UpdatePanel ID="UpdatePanel8" runat="server">
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
            <br />
            <div class="row">
                <div class="col-md-12">
                    <div class="portlet box green">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>GST HSN/SAC Master
                            </div>
                            <div class="tools">
                                <a href="javascript:;" class="collapse"></a>
                                <%--<a href="javascript:;" class="remove"></a>--%>
                                <asp:LinkButton ID="LinkDtn" CssClass="remove" OnClick="btnCancel_Click" runat="server"></asp:LinkButton>
                            </div>
                        </div>
                        <div class="portlet-body form">
                            <div class="form-horizontal">
                                <div class="form-body">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="col-md-12">
                                                <div class="form-group">
                                                    <div class="col-md-5">
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:UpdatePanel runat="server" ID="UpdatePanel9">
                                                            <ContentTemplate>
                                                                <asp:RadioButtonList ID="rbtType" runat="server" AutoPostBack="True" TabIndex="1"
                                                                    RepeatDirection="Horizontal" CssClass="checker">
                                                                    <asp:ListItem Value="0" Selected="True">&nbsp;HSN&nbsp;&nbsp;&nbsp;</asp:ListItem>
                                                                    <asp:ListItem Value="1">&nbsp;SAC</asp:ListItem>
                                                                </asp:RadioButtonList>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="col-md-12">
                                                <div class="form-group">
                                                    <label class="col-md-2 control-label text-right">
                                                        <span class="required">*</span> HSN Code / SAC Code No.
                                                    </label>
                                                    <div class="col-md-3">
                                                        <asp:TextBox CssClass="form-control " ID="txtTariffNo" placeholder="HSN Code / SAC Code No."
                                                            TabIndex="1" runat="server" MaxLength="50" MsgObrigatorio="HSN Code / SAC Code No."></asp:TextBox>
                                                    </div>
                                                    <label class="col-md-2 control-label text-right">
                                                        <span class="required">*</span> Commodity
                                                    </label>
                                                    <div class="col-md-3">
                                                        <asp:TextBox CssClass="form-control " ID="txtCommodity" placeholder="Commodity" TabIndex="2"
                                                            runat="server" MaxLength="50" MsgObrigatorio="Commodity"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="col-md-12">
                                                <div class="form-group">
                                                    <label class="col-md-2 control-label text-right">
                                                        <span class="required">*</span>Central Tax @ X %
                                                    </label>
                                                    <div class="col-md-3">
                                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox CssClass="form-control  text-right" ID="txtBasicExciseDuty" placeholder="0.00"
                                                                    TabIndex="3" runat="server" MsgObrigatorio="Central Tax %" AutoPostBack="true"
                                                                    OnTextChanged="txtBasicExciseDuty_TextChangesd">
                                                                </asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" TargetControlID="txtBasicExciseDuty"
                                                                    ValidChars="0123456789." runat="server" />
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                    <label class="col-md-2 control-label label-sm">
                                                        <span class="required">* </span>Tally Name For Central Tax
                                                    </label>
                                                    <div class="col-md-4">
                                                        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                                            <ContentTemplate>
                                                                <asp:DropDownList ID="ddlTallyBasic" CssClass="select2" AutoPostBack="true" runat="server"
                                                                    Width="80%" OnSelectedIndexChanged="ddlTallyBasic_SelectedIndexChanged" MsgObrigatorio="Tally Name For Central Tax"
                                                                    TabIndex="4">
                                                                </asp:DropDownList>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="col-md-12">
                                                <div class="form-group">
                                                    <label class="col-md-2 control-label text-right">
                                                        <span class="required">*</span> Cess @ X%
                                                    </label>
                                                    <div class="col-md-3">
                                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox CssClass="form-control  text-right " ID="txtSpecialExciseDuty" placeholder="0.00"
                                                                    TabIndex="5" runat="server" MsgObrigatorio="Cess %" OnTextChanged="txtSpecialExciseDuty_TextChangesd"
                                                                    AutoPostBack="true"></asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" TargetControlID="txtSpecialExciseDuty"
                                                                    ValidChars="0123456789." runat="server" />
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                    <label class="col-md-2 control-label label-sm">
                                                        <span class="required">* </span>Tally Name For Cess
                                                    </label>
                                                    <div class="col-md-4">
                                                        <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                                            <ContentTemplate>
                                                                <asp:DropDownList ID="ddlTallySpecial" CssClass="select2" AutoPostBack="true" runat="server"
                                                                    Width="80%" OnSelectedIndexChanged="ddlTallySpecial_SelectedIndexChanged" MsgObrigatorio="Tally Name For Cess"
                                                                    TabIndex="6">
                                                                </asp:DropDownList>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="col-md-12">
                                                <div class="form-group">
                                                    <label class="col-md-2 control-label text-right">
                                                        <span class="required">*</span> State/Union Territory Tax @ X %
                                                    </label>
                                                    <div class="col-md-3">
                                                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox CssClass="form-control  text-right" ID="txtEducationalCess" placeholder="0.00"
                                                                    TabIndex="7" runat="server" MsgObrigatorio="State/Union Territory Tax @ X %"
                                                                    OnTextChanged="txtEducationalCess_TextChangesd" AutoPostBack="true"></asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" TargetControlID="txtEducationalCess"
                                                                    ValidChars="0123456789." runat="server" />
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                    <label class="col-md-2 control-label label-sm">
                                                        <span class="required">* </span>Tally Name For State/Union Territory Tax
                                                    </label>
                                                    <div class="col-md-4">
                                                        <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                                            <ContentTemplate>
                                                                <asp:DropDownList ID="ddlTallyEdu" CssClass="select2" AutoPostBack="true" runat="server"
                                                                    Width="80%" OnSelectedIndexChanged="ddlTallyEdu_SelectedIndexChanged" MsgObrigatorio="Tally Name For State/Union Territory Tax"
                                                                    TabIndex="8">
                                                                </asp:DropDownList>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="col-md-12">
                                                <div class="form-group">
                                                    <label class="col-md-2 control-label text-right">
                                                        <span class="required">*</span> Integrated Tax @ X %
                                                    </label>
                                                    <div class="col-md-3">
                                                        <asp:UpdatePanel ID="UpdatePanelff4" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox CssClass="form-control  text-right " ID="txtSHEdu" placeholder="0.00"
                                                                    TabIndex="9" runat="server" MsgObrigatorio="Integrated Tax %" OnTextChanged="txtSHEdu_TextChangesd"
                                                                    AutoPostBack="true"></asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" TargetControlID="txtSHEdu"
                                                                    ValidChars="0123456789." runat="server" />
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                    <label class="col-md-2 control-label label-sm">
                                                        <span class="required">* </span>Tally Name For Integrated Tax @ X %
                                                    </label>
                                                    <div class="col-md-4">
                                                        <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                                            <ContentTemplate>
                                                                <asp:DropDownList ID="ddlTallySHEdu" CssClass="select2" AutoPostBack="true" runat="server"
                                                                    Width="80%" OnSelectedIndexChanged="ddlTallySHEdu_SelectedIndexChanged" MsgObrigatorio="Tally Name For Integrated Tax"
                                                                    TabIndex="10">
                                                                </asp:DropDownList>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="form-actions fluid">
                                <div class="col-md-offset-5 col-md-9">
                                    <asp:LinkButton ID="btnSubmit" CssClass="btn green" TabIndex="11" runat="server"
                                        OnClick="btnSubmit_Click" OnClientClick="return VerificaCamposObrigatorios();"><i class="fa fa-check-square"> </i>  Save </asp:LinkButton>
                                    <asp:LinkButton ID="btnCancel" CssClass="btn default" TabIndex="12" runat="server"
                                        OnClick="btnCancel_Click"><i class="fa fa-refresh" ></i> Cancel</asp:LinkButton>
                                </div>
                            </div>
                        </div>
                    </div>
                    <asp:UpdatePanel runat="server" ID="UpdatePanel32">
                        <ContentTemplate>
                            <asp:LinkButton ID="CheckCondition" runat="server" BackColor="" CssClass="formlabel"></asp:LinkButton>
                            <cc1:ModalPopupExtender runat="server" ID="ModalPopupPrintSelection" BackgroundCssClass="modalBackground"
                                OnOkScript="oknumber1()" OnCancelScript="oncancel()" DynamicServicePath="" Enabled="True"
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
                                                        Do you Want to Cancel Record?
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
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
        <!-- END PAGE CONTENT-->
    </div>
    <!-- BEGIN JAVASCRIPTS(Load javascripts at bottom, this will reduce page load time) -->
    <!-- BEGIN CORE PLUGINS -->
    <!--[if lt IE 9]>
<script src="assets/plugins/respond.min.js"></script>
<script src="assets/plugins/excanvas.min.js"></script>
<![endif]-->
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

    <!-- IMPORTANT! fullcalendar depends on jquery-ui-1.10.3.custom.min.js for drag & drop support -->
    <!-- BEGIN PAGE LEVEL SCRIPTS -->

    <script src="../../assets/plugins/select2/select2.js" type="text/javascript"></script>

    <script src="../../assets/scripts/app.js" type="text/javascript"></script>

    <!-- END PAGE LEVEL SCRIPTS -->

    <script type="text/javascript">
        function VerificaCamposObrigatorios() {
            try {
                if (VerificaObrigatorio('#<%=txtTariffNo.ClientID%>', '#Avisos') == false) {
                    $("#Avisos").fadeOut(6000);
                    $("#up").click();
                    return false;
                }
                else if (VerificaObrigatorio('#<%=txtCommodity.ClientID%>', '#Avisos') == false) {
                    $("#Avisos").fadeOut(6000);
                    $("#up").click();
                    return false;
                }
                else if (VerificaObrigatorio('#<%=txtBasicExciseDuty.ClientID%>', '#Avisos') == false) {
                    $("#Avisos").fadeOut(6000);
                    $("#up").click();
                    return false;
                }
                else if (VerificaValorCombo('#<%=ddlTallyBasic.ClientID%>', '#Avisos') == false) {
                    $("#Avisos").fadeOut(6000);
                    $("#up").click();
                    return false;
                }
                else if (VerificaObrigatorio('#<%=txtSpecialExciseDuty.ClientID%>', '#Avisos') == false) {
                    $("#Avisos").fadeOut(6000);
                    $("#up").click();
                    return false;
                }
                else if (VerificaValorCombo('#<%=ddlTallySpecial.ClientID%>', '#Avisos') == false) {
                    $("#Avisos").fadeOut(6000);
                    $("#up").click();
                    return false;
                }
                else if (VerificaObrigatorio('#<%=txtEducationalCess.ClientID%>', '#Avisos') == false) {
                    $("#Avisos").fadeOut(6000);
                    $("#up").click();
                    return false;
                }
                else if (VerificaValorCombo('#<%=ddlTallyEdu.ClientID%>', '#Avisos') == false) {
                    $("#Avisos").fadeOut(6000);
                    $("#up").click();
                    return false;
                }
                else if (VerificaObrigatorio('#<%=txtSHEdu.ClientID%>', '#Avisos') == false) {
                    $("#Avisos").fadeOut(6000);
                    $("#up").click();
                    return false;
                }
                else if (VerificaValorCombo('#<%=ddlTallySHEdu.ClientID%>', '#Avisos') == false) {
                    $("#Avisos").fadeOut(6000);
                    $("#up").click();
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

</asp:Content>
