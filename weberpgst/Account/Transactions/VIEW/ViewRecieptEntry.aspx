﻿<%@ Page Title="Reciept Entry" Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true"
    CodeFile="ViewRecieptEntry.aspx.cs" Inherits="Account_Transactions_VIEW_ViewRecieptEntry" %>

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
            $("#up").click();
        }
    </script>

    <div class="page-content-wrapper">
        <div class="page-content">
            <div class="row">
                <div class="col-md-1">
                </div>
                <div id="MSG" class="col-md-10">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <asp:Panel ID="PanelMsg" runat="server" Visible="false" Style="background-color: #feefb3;
                                height: 100%; width: 100%; margin-bottom: 10px; border: 1px solid #9f6000">
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
                                <i class="fa fa-reorder"></i>Reciept Entry
                            </div>
                            <div class="tools">
                                <a href="javascript:;" class="collapse"></a>
                                <asp:LinkButton ID="LinkButton1" CssClass="remove" TabIndex="4" runat="server" OnClick="btnCancel1_Click"></asp:LinkButton>
                            </div>
                        </div>
                        <div class="portlet-body">
                            <div class="form-horizontal">
                                <div class="form-body">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="btn-group">
                                                <label class="control-label">
                                                    Search
                                                </label>
                                            </div>
                                            <div class="btn-group">
                                                <div class="col-md-3">
                                                    <asp:TextBox ID="txtString" runat="server" CssClass="form-control input-medium" TabIndex="1"
                                                        OnTextChanged="txtString_TextChanged" onkeyup="RefreshUpdatePanel();"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="btn-group pull-right">
                                                <asp:LinkButton ID="btnAddNew" CssClass="btn green" runat="server" TabIndex="2" OnClick="btnAddNew_Click">Add New  <i class="fa fa-plus"></i></asp:LinkButton>
                                            </div>
                                            <div class="pull-right">
                                                &nbsp
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <%--<div style="overflow-x: auto;">--%>
                                        <asp:UpdatePanel ID="UpdatePanel100" runat="server">
                                            <ContentTemplate>
                                                <asp:GridView ID="dgDetailPO" runat="server" AutoGenerateColumns="False" Width="100%"
                                                    CellPadding="4" Font-Size="12px" Font-Names="Verdana" GridLines="Both" DataKeyNames="RCP_CODE"
                                                    CssClass="table table-striped table-bordered table-advance table-hover" AllowPaging="True"
                                                    OnRowEditing="dgDetailPO_RowEditing" OnRowCommand="dgDetailPO_RowCommand" OnRowDeleting="dgDetailPO_RowDeleting"
                                                    OnPageIndexChanging="dgDetailPO_PageIndexChanging" PageSize="15" TabIndex="3">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Select" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="50px">
                                                            <ItemTemplate>
                                                                <div class="clearfix">
                                                                    <div class="btn-group">
                                                                        <button type="button" TabIndex="4" class="btn blue btn-xs dropdown-toggle" data-toggle="dropdown">
                                                                            Select <i class="fa fa-angle-down"></i>
                                                                        </button>
                                                                        <ul class="dropdown-menu" role="menu">
                                                                            <li>
                                                                                <asp:LinkButton ID="lnkView" runat="server" CausesValidation="False" CommandName="View"
                                                                                    Text="View" CommandArgument='<%# Bind("RCP_CODE") %>'><i class="fa fa-search"></i> View</asp:LinkButton>
                                                                            </li>
                                                                            <li>
                                                                                <asp:LinkButton ID="lnkModify" runat="server" CausesValidation="False" CommandName="Modify"
                                                                                    Text="Modify" CommandArgument='<%# Bind("RCP_CODE") %>'><i class="fa fa-edit"></i> Modify</asp:LinkButton>
                                                                            </li>
                                                                            <li>
                                                                                <asp:LinkButton ID="lnkPrint" runat="server" CausesValidation="False" CommandName="Print"
                                                                                    Text="Print" CommandArgument='<%# Bind("RCP_CODE") %>'><i class="fa fa-print"></i> Print</asp:LinkButton>
                                                                            </li>
                                                                            <li>
                                                                                <asp:LinkButton ID="lnkPrintMult" runat="server" CausesValidation="False" CommandName="PrintMult" Visible="false"
                                                                                    Text="Print" CommandArgument='<%# Bind("RCP_CODE") %>'><i class="fa fa-print"></i> </asp:LinkButton>
                                                                            </li>
                                                                            <li>
                                                                                <asp:LinkButton ID="lnkDelete" runat="server" CausesValidation="False" CommandName="Delete"
                                                                                    Text="Delete"><i class="fa fa-trash-o"></i> Delete</asp:LinkButton>
                                                                            </li>
                                                                        </ul>
                                                                    </div>
                                                                </div>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" Width="50px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="CPOM_CODE" SortExpression="CPOM_CODE" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblRCP_CODE" CssClass="formlabel" runat="server" Text='<%# Bind("RCP_CODE") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Reciept Number" SortExpression="RCP_NO" HeaderStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblRCP_NO" runat="server" Text='<%# Eval("RCP_NO") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Reciept Date" SortExpression="RCP_DATE" HeaderStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblRCP_DATE" runat="server" Text='<%# Eval("RCP_DATE") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Inward Code" SortExpression="RCP_P_CODE" HeaderStyle-HorizontalAlign="Left"
                                                            Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblRCP_P_CODE" runat="server" Text='<%# Eval("RCP_P_CODE") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Customer Name" SortExpression="P_NAME" HeaderStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblP_NAME" runat="server" Text='<%# Eval("P_NAME") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Cheque No." SortExpression="RCP_CHEQUE_NO" HeaderStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblRCP_CHEQUE_NO" CssClass=" Control-label" runat="server" Text='<%# Eval("RCP_CHEQUE_NO") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="AMOUNT" SortExpression="AMOUNT" HeaderStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblRCP_AMOUNT" CssClass=" Control-label" runat="server" Text='<%# Eval("RCP_AMOUNT") %>'></asp:Label>
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
                                  <%--  </div>--%>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <link href="../../../assets/Avisos/Avisos.css" rel="stylesheet" type="text/css" />
    <script src="../../../assets/Avisos/Avisos.js" type="text/javascript"></script> <script
    src="../../../assets/JS/Util.js" type="text/javascript"></script> <script src="../../../assets/scripts/jquery-1.7.1.min.js"
    type="text/javascript"></script> <script src="../../../assets/plugins/jquery-1.10.2.min.js"
    type="text/javascript"></script> <script src="../../../assets/plugins/jquery-migrate-1.2.1.min.js"
    type="text/javascript"></script> <script src="../../../assets/plugins/bootstrap/js/bootstrap.min.js"
    type="text/javascript"></script> <script src="../../../assets/plugins/bootstrap-hover-dropdown/twitter-bootstrap-hover-dropdown.min.js"
    type="text/javascript"></script> <script src="../../../assets/plugins/jquery-slimscroll/jquery.slimscroll.min.js"
    type="text/javascript"></script> <script src="../../../assets/plugins/jquery.blockui.min.js"
    type="text/javascript"></script> <script src="../../../assets/plugins/jquery.cokie.min.js"
    type="text/javascript"></script> <script src="../../../assets/plugins/uniform/jquery.uniform.min.js"
    type="text/javascript"></script> <!-- END CORE PLUGINS --> <!-- BEGIN PAGE LEVEL
    PLUGINS --> <script src="../../../assets/plugins/bootstrap-daterangepicker/moment.min.js"
    type="text/javascript"></script> <script src="../../../assets/plugins/bootstrap-daterangepicker/daterangepicker.js"
    type="text/javascript"></script> <script src="../../../assets/plugins/gritter/js/jquery.gritter.js"
    type="text/javascript"></script> <!-- BEGIN PAGE LEVEL SCRIPTS --> <script src="../../../assets/scripts/app.js"
    type="text/javascript"></script> <!--END PAGE LEVEL SCRIPTS-->
</asp:Content>
