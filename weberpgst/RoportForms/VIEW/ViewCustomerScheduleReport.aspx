﻿<%@ Page Title="Customer Schedule Report" Language="C#" MasterPageFile="~/main.master"
    AutoEventWireup="true" CodeFile="ViewCustomerScheduleReport.aspx.cs" Inherits="RoportForms_VIEW_ViewCustomerScheduleReport" %>

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

        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function(evt, args) {
            jQuery("#<%=ddlItemCode.ClientID %>").select2();
            jQuery("#<%=ddlCustomer.ClientID %>").select2();
        });
    </script>

    <script type="text/javascript">
        function onCalendarShown() {
            var cal = $find("calendar1");
            //Setting the default mode to month
            cal._switchMode("months", true);
            //Iterate every month Item and attach click event to it
            if (cal._monthsBody) {
                for (var i = 0; i < cal._monthsBody.rows.length; i++) {
                    var row = cal._monthsBody.rows[i];
                    for (var j = 0; j < row.cells.length; j++) {
                        Sys.UI.DomEvent.addHandler(row.cells[j].firstChild, "click", call);
                    }
                }
            }
        }
    </script>

    <script type="text/javascript">
        function onCalendarHidden() {
            var cal = $find("calendar1");
            //Iterate every month Item and remove click event from it
            if (cal._monthsBody) {
                for (var i = 0; i < cal._monthsBody.rows.length; i++) {
                    var row = cal._monthsBody.rows[i];
                    for (var j = 0; j < row.cells.length; j++) {
                        Sys.UI.DomEvent.removeHandler(row.cells[j].firstChild, "click", call);
                    }
                }
            }
        }
    </script>

    <script type="text/javascript">
        function call(eventElement) {
            var target = eventElement.target;
            switch (target.mode) {
                case "month":
                    var cal = $find("calendar1");
                    cal._visibleDate = target.date;
                    cal.set_selectedDate(target.date);
                    cal._switchMonth(target.date);
                    cal._blur.post(true);
                    cal.raiseDateSelectionChanged();
                    break;
            }
        }
    </script>

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

    <div class="page-content-wrapper">
        <div class="page-content">
            <!-- BEGIN PAGE CONTENT-->
            <div class="row">
                <div class="col-md-1">
                </div>
                <div class="col-md-10">
                    <div id="Avisos">
                    </div>
                </div>
            </div>
            <div class="row">
                <%--<div class="col-md-1">
                </div>--%>
                <div class="col-md-12">
                    <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                        <ContentTemplate>
                            <asp:Panel ID="PanelMsg" runat="server" Visible="false" Style="background-color: #feefb3;
                                height: 50px; width: 100%; margin-bottom: 10px; border: 1px solid #9f6000">
                                <div style="vertical-align: middle; margin-top: 10px;">
                                    <asp:Label ID="lblmsg" runat="server" Style="color: #9f6000; font-size: medium; font-weight: bold;
                                        margin-top: 50px; margin-left: 10px;"></asp:Label>
                                </div>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="row">
                <div class="col-md-1">
                </div>
                <div class="col-md-10">
                    <div class="portlet box green">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>Customer Schedule Report
                            </div>
                            <div class="tools">
                                <a href="javascript:;" class="collapse"></a>
                                <asp:LinkButton ID="CncLnk" CssClass="remove" OnClick="btnCancel_Click" runat="server"></asp:LinkButton>
                            </div>
                        </div>
                        <div class="portlet-body form">
                            <div class="form-horizontal">
                                <div class="form-body">
                                    <div class="row">
                                        <div class="form-group">
                                            <div class="col-md-1">
                                            </div>
                                            <label class="col-md-2 control-label text-right">
                                                Month
                                            </label>
                                            <div class="col-md-2">
                                                <asp:UpdatePanel runat="server" ID="UpdatePanel5">
                                                    <ContentTemplate>
                                                        <div class="input-group">
                                                            <asp:TextBox ID="txtMonth" runat="server" placeholder="MMM yyyy" CssClass="form-control"
                                                                TabIndex="1" ValidationGroup="Save" AutoPostBack="true" OnTextChanged="txtMonth_TextChanged"></asp:TextBox>
                                                            <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                            <cc1:CalendarExtender ID="txtMonth_CalenderExtender" BehaviorID="calendar1" runat="server"
                                                                Enabled="True" TargetControlID="txtMonth" OnClientHidden="onCalendarHidden" OnClientShown="onCalendarShown"
                                                                PopupButtonID="txtMonth" Format="MMM yyyy">
                                                            </cc1:CalendarExtender>
                                                        </div>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="form-group">
                                                    <label class="col-md-3 control-label text-right ">
                                                        Customer Name</label>
                                                    <div class="col-md-6">
                                                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                            <ContentTemplate>
                                                                <asp:DropDownList ID="ddlCustomer" CssClass="select2" Width="100%" runat="server"
                                                                    TabIndex="2" MsgObrigatorio="Customer Name" OnSelectedIndexChanged="ddlCustomer_SelectedIndexChanged"
                                                                    AutoPostBack="True" Visible="True">
                                                                </asp:DropDownList>
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="chkAllCust" EventName="CheckedChanged" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                    <div class="col-md-2">
                                                        <asp:UpdatePanel runat="server" ID="UpdatePanel4">
                                                            <ContentTemplate>
                                                                <asp:CheckBox ID="chkAllCust" runat="server" CssClass="checker" Text="&nbspAll" AutoPostBack="True"
                                                                    TabIndex="3" OnCheckedChanged="chkAllCust_CheckedChanged" />
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="form-group">
                                                <label class="col-md-3 control-label text-right">
                                                    Item Code
                                                </label>
                                                <div class="col-md-6">
                                                    <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                                                        <ContentTemplate>
                                                            <asp:DropDownList ID="ddlItemCode" runat="server" placeholder="Select Item Code"
                                                                AutoPostBack="True" CssClass="select2" Width="100%" TabIndex="3" OnSelectedIndexChanged="ddlItemCode_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="chkAllComp" EventName="CheckedChanged" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <div class="col-md-2">
                                                    <asp:UpdatePanel runat="server" ID="UpdatePanel2">
                                                        <ContentTemplate>
                                                            <asp:CheckBox ID="chkAllComp" runat="server" CssClass="checker" Text="&nbspAll" AutoPostBack="True"
                                                                TabIndex="3" OnCheckedChanged="chkAllComp_CheckedChanged" />
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="form-group">
                                                <div class="col-md-3">
                                                </div>
                                                 
                                                <div class="col-md-9">
                                                    <asp:UpdatePanel runat="server" ID="UpdatePanel7">
                                                        <ContentTemplate>
                                                            <asp:RadioButtonList ID="rbtType" runat="server" AutoPostBack="True" TabIndex="1"
                                                                RepeatDirection="Horizontal" CssClass="checker" CellPadding="10">
                                                                <asp:ListItem Value="0" Selected="True">&nbsp;Schedule/Sale (M)</asp:ListItem>
                                                                <asp:ListItem Value="1">&nbsp;Schedule/Sale (D)</asp:ListItem>
                                                                <asp:ListItem Value="2">&nbsp;Sch/Sale W/O Val</asp:ListItem>
                                                            </asp:RadioButtonList>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="form-actions fluid">
                                    <div class="col-md-offset-4 col-md-9">
                                        <asp:LinkButton ID="btnSubmit" CssClass="btn green" TabIndex="3" runat="server" OnClick="btnShow_Click"
                                            OnClientClick="return VerificaCamposObrigatorios();"><i class="fa fa-check-square"> </i>  Show </asp:LinkButton>
                                        <asp:LinkButton ID="btnCancel" CssClass="btn default" TabIndex="4" runat="server"
                                            OnClick="btnCancel_Click"><i class="fa fa-refresh"></i> Cancel</asp:LinkButton>
                                    </div>
                                </div>
                            </div>
                        </div>
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
                                                            Do you Want to Show Report ?
                                                        </label>
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
        </div>
        <!-- END PAGE CONTENT-->
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

    <script src="../../assets/plugins/select2/select2.js" type="text/javascript"></script>

    <!-- END CORE PLUGINS -->
    <!-- BEGIN PAGE LEVEL PLUGINS -->

    <script src="../../assets/plugins/bootstrap-daterangepicker/moment.min.js" type="text/javascript"></script>

    <script src="../../assets/plugins/bootstrap-daterangepicker/daterangepicker.js" type="text/javascript"></script>

    <script src="../../assets/plugins/gritter/js/jquery.gritter.js" type="text/javascript"></script>

    <!-- END PAGE LEVEL PLUGINS -->
    <!-- BEGIN PAGE LEVEL SCRIPTS -->

    <script src="../../assets/scripts/app.js" type="text/javascript"></script>

    <!-- END PAGE LEVEL SCRIPTS -->
    k
</asp:Content>
