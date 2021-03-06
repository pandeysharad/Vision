﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/MasterPage.master" AutoEventWireup="true" CodeFile="CreateExamSchedule.aspx.cs" Inherits="Pages_CreateExamSchedule" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link href="../css/main.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="row">
        <div class="col-sm-12">
            <div class="panel-body" style="border: thin solid black; background-color: #42b3f4;
                color: White; height: 35px; margin-top: 2px">
                <div class="form-horizontal">
                    <div class="form-group" style="text-align: center; margin-top: -12px; font-weight: bold;
                        font-size: 22px; font-family: Maiandra GD">
                       CREATE EXAM SCHEDULE 
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="row">
     <asp:UpdatePanel ID="UpdatePanel2" runat="server">
    <ContentTemplate> 
     <div class="panel-body" style="border:thin solid black; margin-left:15px;margin-right:15px;margin-top:2px">
        <div class="col-xs-12">
            <div class="form-horizontal">
               <div class="form-group">
                    <label for="message" class="col-xs-2">
                        Exam Term:<span for="message" style="color: Red">*</span></label>
                    <div class="col-xs-4">
                        <asp:DropDownList ID="ddlTerms" CssClass="form-control  input-sm" 
                            runat="server" AutoPostBack="True" 
                            onselectedindexchanged="ddlTerms_SelectedIndexChanged" >
                        </asp:DropDownList>
                    </div>
                </div>
            <div class="form-group">
                    <label for="message" class="col-xs-2">
                        Exam Title:<span for="message" style="color: Red">*</span></label>
                    <div class="col-xs-4">
                        <asp:DropDownList ID="ddlExamTitle" CssClass="form-control  input-sm" 
                            runat="server" AutoPostBack="True" 
                            onselectedindexchanged="ddlExamTitle_SelectedIndexChanged" >
                        </asp:DropDownList>
                    </div>
                </div>
              <div class="form-group">
                    <label for="message" class="col-xs-2">
                        Class:<span for="message" style="color: Red">*</span></label>
                    <div class="col-xs-4">
                        <asp:DropDownList ID="ddlClass" CssClass="form-control  input-sm" 
                            runat="server" AutoPostBack="True" 
                            onselectedindexchanged="ddlClass_SelectedIndexChanged">
                        </asp:DropDownList>
                    </div>
                </div>
                 <div class="form-group">
                    <label for="message" class="col-xs-2">
                       Subject Name:<span for="message" style="color: Red">*</span></label>
                    <div class="col-xs-4">
                        <asp:DropDownList ID="ddlSubjectName" CssClass="form-control  input-sm" 
                            runat="server" AutoPostBack="True" >
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <label for="message" class="col-xs-2">
                        Minimum Marks:<span for="message" style="color: Red">*</span></label>
                    <div class="col-xs-3">
                        <asp:TextBox ID="txtMinimumMarks" CssClass="form-control  input-sm" runat="server" ></asp:TextBox>
                    </div>
                </div>
                 <div class="form-group">
                    <label for="message" class="col-xs-2">
                        Maximum Marks:<span for="message" style="color: Red">*</span></label>
                    <div class="col-xs-3">
                        <asp:TextBox ID="txtMaximumMarks" CssClass="form-control  input-sm" runat="server" ></asp:TextBox>
                    </div>
                </div>
                 <div class="form-group">
                    <div class="col-xs-4">
                        <asp:CheckBox ID="chkAdditionalSubject" Text="Any Additional Subject" 
                            runat="server" AutoPostBack="True" 
                            oncheckedchanged="chkAdditionalSubject_CheckedChanged" />
                    </div>
                </div>
                 <div class="col-xs-12" style="margin-left: -10px;">
            <div class="form-horizontal">
                        <asp:Panel ID="pnlAdditional" runat="server" Visible="False" Width="100%" Height="130px">
                            <div class="form-group">
                                <label for="message" class="col-xs-2">
                                    Subject Name:<span for="message" style="color: Red">*</span></label>
                                <div class="col-xs-3">
                                    <asp:DropDownList ID="ddlAdditionalSubjectName" CssClass="form-control  input-sm"
                                        runat="server" AutoPostBack="True">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <label for="message" class="col-xs-2">
                                    Minimum Marks:<span for="message" style="color: Red">*</span></label>
                                <div class="col-xs-3">
                                    <asp:TextBox ID="txtadnMinimumMarks" CssClass="form-control  input-sm" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label for="message" class="col-xs-2">
                                    Maximum Marks:<span for="message" style="color: Red">*</span></label>
                                <div class="col-xs-3">
                                    <asp:TextBox ID="txtadnMaximumMarks" CssClass="form-control  input-sm" runat="server"></asp:TextBox>
                                </div>
                            </div>
                        </asp:Panel>
                        </div>
                        </div>
                <div class="form-group">
                    <div class="col-xs-2">
                    </div>
                    <div class="col-xs-2">
                        <asp:LinkButton ID="btnSave" CssClass="btn btn-primary" runat="server" 
                            Width="100%" onclick="btnSave_Click">
                                <span aria-hidden="true" class="glyphicon glyphicon-floppy-disk"> Create</span>
                        </asp:LinkButton>
                    </div>
                    <div class="col-xs-2">
                        <asp:LinkButton ID="btnRefresh" CssClass="btn btn-primary" runat="server" 
                            Width="100%" onclick="btnRefresh_Click">
                                <span aria-hidden="true" class="glyphicon glyphicon-refresh"> Refresh</span>
                        </asp:LinkButton>
                    </div>
                </div>
                  <div class="form-group"> 
                        <div class="row">
                          <div class="col-sm-12">
                             <div class="col-xs-8">
                              <asp:GridView ID="GridSchedule" runat="server" HeaderStyle-BackColor="#66ccff" 
                                     AutoGenerateColumns="false" Width="100%"
                                HeaderStyle-Font-Size="Small" HeaderStyle-Font-Names="Maiandra GD"  
                                     HeaderStyle-Font-Bold="true"  RowStyle-Font-Size="Small"    RowStyle-Font-Names="Maiandra GD"
                                RowStyle-Font-Bold="true" AutoGenerateSelectButton="true" DataKeyNames="ExamScheduleId"
                                     AllowPaging="True" 
                                     onselectedindexchanging="GridSchedule_SelectedIndexChanging">
                                    <Columns> 
                                        <asp:BoundField DataField="ExamTerm" 
                                            HeaderText="TERM" >
                                        <ItemStyle Width="10%"/>
                                        </asp:BoundField>
                                         <asp:BoundField DataField="ExamTitleName"
                                            HeaderText="EXAM TITLE" >
                                        <ItemStyle Width="20%"/>
                                        </asp:BoundField>
                                         <asp:BoundField DataField="CourseName" 
                                            HeaderText="CLASS NAME">
                                        <ItemStyle Width="14%"/>
                                        </asp:BoundField>
                                         <asp:BoundField DataField="Subjects" 
                                            HeaderText="SUBJECT NAME">
                                        <ItemStyle Width="20%" />
                                        </asp:BoundField>
                                         <asp:BoundField DataField="Minimum" 
                                            HeaderText="MINIMUM MARKS">
                                        <ItemStyle Width="18%"/>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Maximum"
                                         HeaderText="MAXIMUM MARKS" >
                                        <ItemStyle Width="18%"/>
                                        </asp:BoundField>
                                    </Columns>
                                    <HeaderStyle BackColor="#66CCFF" Font-Bold="True" Font-Names="Maiandra GD" 
                                        Font-Size="Small"/>
                                    <PagerStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    <RowStyle Font-Bold="True" Font-Names="Maiandra GD" Font-Size="Small" />
                             </asp:GridView>
                             </div>
                             <div class="col-xs-2"></div>
                             </div>
                             </div>
                             </div>
            </div>
        </div>
        </div>
        </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
