﻿<%@ Page Title="Purchase Requirment Report" Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true"
    CodeFile="ViewRequirment.aspx.cs" Inherits="RoportForms_VIEW_ViewRequirment" %>

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
           
            jQuery("#<%=ddlComponent.ClientID %>").select2(); 
        });
    </script>

    <div class="page-content-wrapper">
        <div class="page-content">
            <!-- BEGIN PAGE CONTENT-->
            <div id="Avisos">
            </div>
            <div class="row">
                <div class="col-md-12">
                    <div class="portlet box green">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>Purchase Requirment Report
                            </div>
                            <div class="tools">
                                <a href="javascript:;" class="collapse"></a>
                                <asp:LinkButton ID="LinkDtn" CssClass="remove" OnClick="btnCancel_Click" runat="server"></asp:LinkButton>
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
                                                From Date
                                            </label>
                                            <div class="col-md-2">
                                                <asp:UpdatePanel runat="server" ID="UpdatePanel5">
                                                    <ContentTemplate>
                                                        <div class="input-group">
                                                            <asp:TextBox ID="txtFromDate" runat="server" placeholder="dd MMM yyyy" CssClass="form-control"
                                                                TabIndex="6" ValidationGroup="Save"></asp:TextBox>
                                                            <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                                            <cc1:CalendarExtender ID="txtFromDate_CalenderExtender" BehaviorID="calendar1" runat="server"
                                                                Enabled="True" TargetControlID="txtFromDate" PopupButtonID="txtFromDate" Format="dd MMM yyyy">
                                                            </cc1:CalendarExtender>
                                                        </div>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </div>
                                            <label class="col-md-2 control-label text-right ">
                                              
                                            </label>
                                            <div class="col-md-2">
                                                <asp:UpdatePanel runat="server" ID="UpdatePanel4">
                                                    <ContentTemplate>
                                                        <div class="input-group">
                                                            
                                                        </div>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </div>
                                            <div class="col-md-2">
                                                 
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
                                                        <asp:DropDownList ID="ddlComponent" runat="server" placeholder="Select Item Code"
                                                            AutoPostBack="True" CssClass="select2" Width="500px" TabIndex="2" OnSelectedIndexChanged="ddlComponent_SelectedIndexChanged">
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
                              
                                
                                </div>
                            </div>
                            <div class="form-actions fluid">
                                <div class="col-md-offset-4 col-md-9">
                                    <asp:LinkButton ID="btnShow" CssClass="btn green" TabIndex="3" runat="server" OnClientClick="return VerificaCamposObrigatorios();"
                                        OnClick="btnShow_Click"><i class="fa fa-check-square"> </i>  Show </asp:LinkButton>
                                    <asp:LinkButton ID="btnCancel" CssClass="btn default" TabIndex="4" runat="server"
                                        OnClick="btnCancel_Click"><i class="fa fa-refresh"></i> Cancel</asp:LinkButton>
                                </div>
                            </div>
                        </div>
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
</asp:Content>
