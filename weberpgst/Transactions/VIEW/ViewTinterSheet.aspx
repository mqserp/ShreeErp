﻿<%@ Page Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="ViewTinterSheet.aspx.cs" Inherits="Transactions_VIEW_ViewTinterSheet" Title="Tinter Sheet" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="Server">

    <script type="text/javascript">
        function RefreshUpdatePanel() {
            __doPostBack('<%= txtString.ClientID %>', '');
        };
    </script>
 <script type="text/javascript">
     function Showalert() {
         $('#MSG').fadeIn(6000)
         $('#MSG').fadeOut(6000)
     }
    </script>

    <div class="page-content-wrapper">
        <div class="page-content">
            <%--<div id="Avisos">
            </div>--%>
            <div class="row">
                <div id="MSG" class="col-md-12">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
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
                <div class="col-md-1">
                </div>
                <div class="col-md-12">
                    <div class="portlet box green">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>Tinter Sheet</div>
                            <div class="tools">
                                <a href="javascript:;" class="collapse"></a><a href="../../Masters/ADD/RNDDefault.aspx"
                                    class="remove"></a>
                            </div>
                        </div>
                        <div class="portlet-body">
                            <div class="form-horizontal">
                                <div class="form-body">
                                    <div class="btn-group">
                                        <label class="control-label">
                                            Search
                                        </label>
                                    </div>
                                    <div class="btn-group">
                                        <asp:TextBox ID="txtString" runat="server" CssClass="form-control input-medium" TabIndex="3"
                                            OnTextChanged="txtString_TextChanged" onkeyup="RefreshUpdatePanel();"></asp:TextBox>
                                    </div>
                                    <div class="btn-group pull-right">
                                        <asp:LinkButton ID="btnAddNew" CssClass="btn green" runat="server" OnClick="btnAddNew_Click">Add New  <i class="fa fa-plus"></i></asp:LinkButton>
                                    </div>
                                    <div class="pull-right">
                                        &nbsp
                                    </div>
                                </div>
                                <br />
                               
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="form-body">
                                            <%--<div class="row" style="overflow: auto;">--%>
                                            <asp:UpdatePanel ID="UpdatePanel100" runat="server">
                                                <ContentTemplate>
                                                    <asp:GridView ID="dgBatch" runat="server" Width="100%" AutoGenerateColumns="False"
                                                        CellPadding="4" Font-Size="12px" ShowFooter="false" Font-Names="Verdana" GridLines="Both"
                                                        CssClass="table table-striped table-bordered table-advance table-hover" DataKeyNames="TSM_CODE"
                                                        OnRowDeleting="dgBatch_RowDeleting" OnRowEditing="dgBatch_RowEditing" OnPageIndexChanging="dgBatch_PageIndexChanging"
                                                        OnRowCommand="dgBatch_RowCommand" OnRowUpdating="dgBatch_RowUpdating" AllowPaging="true"
                                                        PageSize="15">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Select" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="50px">
                                                                <ItemTemplate>
                                                                    <div class="clearfix">
                                                                        <div class="btn-group">
                                                                            <button type="button" class="btn blue btn-xs dropdown-toggle" data-toggle="dropdown">
                                                                                Select <i class="fa fa-angle-down"></i>
                                                                            </button>
                                                                            <ul class="dropdown-menu" role="menu">
                                                                                <li>
                                                                                    <asp:LinkButton ID="lnkView" runat="server" CausesValidation="False" CommandName="View"
                                                                                        Text="View" CommandArgument='<%# Bind("TSM_CODE") %>'><i class="fa fa-search"></i> View</asp:LinkButton>
                                                                                </li>
                                                                                <li>
                                                                                    <asp:LinkButton ID="lnkModify" runat="server" CausesValidation="False" CommandName="Modify"
                                                                                        Text="Modify" CommandArgument='<%# Bind("TSM_CODE") %>'><i class="fa fa-edit"></i> Modify</asp:LinkButton>
                                                                                </li>
                                                                                <li>
                                                                                    <asp:LinkButton ID="lnkDelete" runat="server" CausesValidation="False" OnClientClick="return confirm('Are you sure to Delete?');"
                                                                                        CommandName="Delete" Text="Delete"><i class="fa fa-trash-o"></i> Delete</asp:LinkButton>
                                                                                </li>
                                                                                <li>
                                                                                    <asp:LinkButton ID="lnkPrint" runat="server" CausesValidation="False" CommandName="Print"
                                                                                        Text="Print" CommandArgument='<%# Bind("TSM_CODE") %>'><i class="fa fa-print"></i> Print</asp:LinkButton></li>
                                                                            </ul>
                                                                        </div>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="TSM_CODE" SortExpression="TSM_CODE" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblTSM_CODE" runat="server" Text='<%# Bind("TSM_CODE") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Batch No" SortExpression="BT_NO" HeaderStyle-HorizontalAlign="Left">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblBT_NO" CssClass="" runat="server" Text='<%# Eval("BT_NO") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Date" SortExpression="TSM_DATE" HeaderStyle-HorizontalAlign="Left">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblTSM_DATE" CssClass="" runat="server" Text='<%# Eval("TSM_DATE") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Customer Name" SortExpression="P_NAME" HeaderStyle-HorizontalAlign="Left">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblP_NAME" CssClass="" runat="server" Text='<%# Eval("P_NAME") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <AlternatingRowStyle CssClass="alt" />
                                                        <PagerStyle CssClass="pgr" />
                                                    </asp:GridView>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="txtString" EventName="TextChanged" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                            <%-- </div>--%>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-md-offset-10">
        </div>
    </div>
   <%-- </div>--%>
    <link href="../../assets/Avisos/Avisos.css" rel="stylesheet" type="text/css" />

    <script src="../../assets/Avisos/Avisos.js" type="text/javascript"></script>

    <script src="../../assets/JS/Util.js" type="text/javascript"></script>

    <script src="../../assets/scripts/jquery-1.7.1.min.js" type="text/javascript"></script>

    <script src="../../assets/plugins/jquery-1.10.2.min.js" type="text/javascript"></script>

    <script src="../../assets/plugins/jquery-migrate-1.2.1.min.js" type="text/javascript"></script>

    <!-- IMPORTANT! Load jquery-ui-1.10.3.custom.min.js before bootstrap.min.js to fix bootstrap tooltip conflict with jquery ui tooltip -->

    <script src="../../../assets/plugins/jquery-ui/jquery-ui-1.10.3.custom.min.js" type="text/javascript"></script>

    <script src="../../assets/plugins/bootstrap/js/bootstrap.min.js" type="text/javascript"></script>

    <script src="../../assets/plugins/bootstrap-hover-dropdown/twitter-bootstrap-hover-dropdown.min.js"
        type="text/javascript"></script>

    <script src="../../assets/plugins/jquery-slimscroll/jquery.slimscroll.min.js" type="text/javascript"></script>

    <script src="../../assets/plugins/jquery.blockui.min.js" type="text/javascript"></script>

    <script src="../../assets/plugins/jquery.cokie.min.js" type="text/javascript"></script>

    <script src="../../assets/plugins/uniform/jquery.uniform.min.js" type="text/javascript"></script>

    <!-- END CORE PLUGINS -->
    <!-- BEGIN PAGE LEVEL PLUGINS -->

    <script src="../../../assets/plugins/flot/jquery.flot.js" type="text/javascript"></script>

    <script src="../../../assets/plugins/flot/jquery.flot.resize.js" type="text/javascript"></script>

    <script src="../../../assets/plugins/jquery.pulsate.min.js" type="text/javascript"></script>

    <script src="../../assets/plugins/bootstrap-daterangepicker/moment.min.js" type="text/javascript"></script>

    <script src="../../assets/plugins/bootstrap-daterangepicker/daterangepicker.js" type="text/javascript"></script>

    <script src="../../assets/plugins/gritter/js/jquery.gritter.js" type="text/javascript"></script>

    <!-- IMPORTANT! fullcalendar depends on jquery-ui-1.10.3.custom.min.js for drag & drop support -->

    <script src="../../../assets/plugins/fullcalendar/fullcalendar/fullcalendar.min.js"
        type="text/javascript"></script>

    <script src="../../../assets/plugins/jquery-easy-pie-chart/jquery.easy-pie-chart.js"
        type="text/javascript"></script>

    <script src="../../../assets/plugins/jquery.sparkline.min.js" type="text/javascript"></script>

    <!-- END PAGE LEVEL PLUGINS -->
    <!-- BEGIN PAGE LEVEL SCRIPTS -->

    <script src="../../assets/scripts/app.js" type="text/javascript"></script>

    <script src="../../../assets/scripts/index.js" type="text/javascript"></script>

    <script src="../../../assets/scripts/tasks.js" type="text/javascript"></script>

    <script src="../../../assets/plugins/bootstrap-sessiontimeout/jquery.sessionTimeout.min.js"
        type="text/javascript"></script>

    <!-- END PAGE LEVEL SCRIPTS -->

    <script>
        jQuery(document).ready(function () {
            App.init();

            // initialize session timeout settings
            $.sessionTimeout({
                title: 'Session Timeout Notification',
                message: 'Your session is about to expire.',
                keepAliveUrl: 'demo/timeout-keep-alive.php',
                redirUrl: '../Lock.aspx',
                logoutUrl: '../Default.aspx',
                //        warnAfter: 5000, //warn after 5 seconds
                //redirAfter: 10000, //redirect after 10 secons
            });
        });
    </script>

    <!-- END JAVASCRIPTS -->
</asp:Content>
