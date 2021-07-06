<%@ Page Title="Security &middot; Zen Bikez" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="UserRoleAdmin.aspx.cs" Inherits="Administration_UserRoleAdmin" %>

<%@ Register Src="~/User_Controls/MessageUserControl.ascx" TagPrefix="uc1" TagName="MessageUserControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <div class="jumbotron jumbotron-fluid">
        <div class="container text-center">
            <h1 class="display-4 mt-5">Security</h1>
            <p class="lead">Manage users and roles</p>
        </div>
    </div>

    <div class="container">
        <div class="row">
            <div class="col-md-12">
              <!--script for tab to tab movement -->
                <%--<script>
                    function nextButton(anchorRef) {
                        $('a[href="' + anchorRef + '"]').tab('show');
                    }
                </script>--%>
                <!--Nav tabs-->
                <ul class="nav nav-tabs nav-fill nav-justified" role="tablist">
                    <li class="nav-item" role="presentation">
                        <a class="nav-link active text-decoration-none" id="user-tab" data-toggle="tab" href="#user" role="tab" aria-controls="user" aria-selected="true">User</a>
                    </li>
                    <li class="nav-item" role="presentation">
                        <a class="nav-link text-decoration-none" id="role-tab" data-toggle="tab" href="#role" role="tab" aria-controls="role" aria-selected="false">Role</a>
                    </li>
                </ul>

                <!--Tab panes one for each tab-->
                <div class="tab-content"> 
                    <div class="tab-pane fade show active" id="user" role="tabpanel" aria-labelledby="user-tab">
                        <asp:UpdatePanel ID="UpdatePanelUser" runat="server">
                            <ContentTemplate>
                                <asp:ListView ID="UserListView" runat="server" 
                                    DataSourceID="UserListViewODS"
                                    InsertItemPosition="LastItem"
                                    ItemType="BSMSData.Entities.Security.UserProfile"
                                    DataKeyNames="UserId"
                                    OnItemInserting="UserListView_ItemInserting"
                                    OnItemDeleted="RefreshAll"
                                    OnItemInserted="RefreshAll">
                                <EmptyDataTemplate>
                                    <span>No Security users have been set up.</span>
                                </EmptyDataTemplate>
                                <LayoutTemplate>
                                    <%--<div class="row bginfo">
                                        <div class="col-sm-2 h4">Action</div>
                                        <div class="col-sm-2 h4">User Names</div>
                                        <div class="col-sm-3 h4">Profile</div>
                                        <div class="col-sm-2 "></div>
                                        <div class="col-sm-3 h4">Roles</div>
                                    </div>
                                    <div runat="server" id="itemPlaceHolder">
                                    </div>--%>
                                    <div class="table-responsive">
                                        <table runat="server" id="itemPlaceholderContainer" class="table table-hover table-striped table-borderless mb-0">
                                            <caption>List of current services</caption>
                                            <thead>
                                                <tr runat="server" class="thead-dark text-center align-middle">
                                                    <th runat="server" scope="col">User Name</th>
                                                    <th runat="server" scope="col">Email</th>
                                                    <th runat="server" scope="col">Full Name</th>
                                                    <th runat="server" scope="col">Role(s)</th>
                                                    <th runat="server" scope="col"></th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                <tr runat="server" id="itemPlaceholder"></tr>
                                            </tbody>
                                        </table>
                                    </div>
                                </LayoutTemplate>
                                <ItemTemplate>
                                    <%--<div class="row">
                                        <div class="col-sm-2">
                                            <asp:LinkButton ID="RemoveUser" runat="server" 
                                                CommandName="Delete">Remove</asp:LinkButton>
                                        </div>
                                        <div class="col-sm-2">
                                            <%# Item.UserName %>
                                        </div>
                                        <div class="col-sm-5">
                                            <%# Item.Email %>&nbsp;&nbsp;
                                            <%# Item.FirstName + " " + Item.LastName %>
                                        </div>
                                        <div class="col-sm-3">
                                            <asp:Repeater ID="RoleUserReapter" runat="server"
                                                DataSource="<%# Item.RoleMemberships%>"
                                                ItemType="System.String">
                                                <ItemTemplate>
                                                        <%# Item %>
                                                </ItemTemplate>
                                                <SeparatorTemplate>, </SeparatorTemplate>
                                            </asp:Repeater>
                                        </div>
                                    </div>--%>
                                    <tr class="text-center align-middle">
                                        <td class="align-middle">
                                            <%# Item.UserName %></td>
                                        <td class="align-middle">
                                            <%# Item.Email %>
                                        </td>
                                        <td class="align-middle">
                                            <%# Item.FirstName + " " + Item.LastName %>
                                        </td>
                                        <td class="align-middle">
                                                <asp:Repeater ID="RoleUserRepeater" runat="server"
                                                    DataSource="<%# Item.RoleMemberships%>"
                                                    ItemType="System.String">
                                                    <ItemTemplate>
                                                            <%# Item %>
                                                    </ItemTemplate>
                                                    <SeparatorTemplate>
                                                        <br />
                                                    </SeparatorTemplate>
                                                </asp:Repeater>
                                        </td>
                                        <td class="align-middle">
                                            <asp:LinkButton ID="RemoveUser" runat="server" CommandName="Delete" Text="Remove"
                                                CssClass="btn btn-link btn-block btn-sm text-decoration-none text-danger" ToolTip="Remove User">
                                                <i class="bi bi-person-dash-fill h6"></i>
                                            </asp:LinkButton>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <InsertItemTemplate>
                                    <%--<div class="row">
                                        <div class="col-sm-2">
                                            <asp:LinkButton ID="InsertUser" runat="server" 
                                            CommandName="Insert">Insert</asp:LinkButton>
                                            <asp:LinkButton ID="CancelButton" runat="server" 
                                            CommandName="Cancel">Cancel</asp:LinkButton>
                                        </div>
                                        <div class="col-sm-2">
                                            <asp:TextBox ID="UserName" runat="server"
                                                text='<%# BindItem.UserName %>' 
                                                placeholder="User Name"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-2">
                                            <asp:TextBox ID="UserEmail" runat="server"
                                                text='<%# BindItem.Email %>' TextMode="Email" 
                                                placeholder="User Email"></asp:TextBox>
                                        </div>
                                    
                                        <div class="col-sm-3">
                                            <asp:TextBox ID="EmployeeID" runat="server"
                                                text='<%# BindItem.EmployeeId %>' TextMode="Number" 
                                                placeholder="Employee ID" ></asp:TextBox>
                                        </div>
                                        <div class="col-sm-3">
                                            <asp:CheckBoxList ID="RoleMemberships" runat="server"
                                                DataSourceID="RoleNameODS"></asp:CheckBoxList>
                                        </div>
                                    </div>--%>
                                    <tr class="table-active bg-transparent text-center align-middle">
                                        <td class="align-middle">
                                             <asp:TextBox ID="UserName" runat="server"
                                                 text='<%# BindItem.UserName %>' 
                                                 placeholder="User Name"
                                                 CssClass="form-control" ></asp:TextBox>
                                        </td>
                                        <td class="align-middle">
                                            <asp:TextBox ID="UserEmail" runat="server"
                                                text='<%# BindItem.Email %>' TextMode="Email" 
                                                placeholder="User Email"
                                                CssClass="form-control" ></asp:TextBox>
                                        </td>
                                        <td class="align-middle">
                                            <asp:TextBox ID="EmployeeID" runat="server"
                                                text='<%# BindItem.EmployeeId %>' TextMode="Number" 
                                                placeholder="Employee ID"
                                                CssClass="form-control" ></asp:TextBox>
                                        </td>
                                        <td class="align-middle text-left">
                                            <asp:CheckBoxList ID="RoleMemberships" runat="server"
                                                DataSourceID="RoleNameODS" CssClass="bg-transparent custom-control custom-checkbox" RepeatDirection="Vertical" RepeatLayout="Flow"></asp:CheckBoxList>
                                        </td>
                                        <td class="align-middle">
                                            <asp:LinkButton ID="InsertUser" runat="server" CommandName="Insert" Text="Add User" ToolTip="Add User"
                                                CssClass="btn btn-link btn-sm text-decoration-none text-success">
                                                <i class="bi bi-person-plus-fill h6"></i>
                                            </asp:LinkButton>
                                            <asp:LinkButton ID="CancelButton" runat="server" CommandName="Cancel" Text="Clear" ToolTip="Clear Insert Row" 
                                                CssClass="btn btn-link btn-sm text-decoration-none text-secondary">
                                                <i class="bi bi-backspace-fill h6"></i>
                                            </asp:LinkButton>
                                        </td>
                                    </tr>
                                </InsertItemTemplate>
                            </asp:ListView>
                            <asp:ObjectDataSource ID="UserListViewODS" runat="server" 
                                DataObjectTypeName="BSMSData.Entities.Security.UserProfile" 
                                DeleteMethod="RemoveUser" 
                                InsertMethod="AddUser" 
                                SelectMethod="ListAllUsers"
                                OldValuesParameterFormatString="original_{0}"  
                                TypeName="BSMSSystem.BLL.Security.UserManager"
                                 OnDeleted="CheckForException"
                                 OnInserted="CheckForException"
                                 OnSelected="CheckForException">
                            </asp:ObjectDataSource>
                                <asp:ObjectDataSource ID="RoleNameODS" runat="server" 
                                SelectMethod="ListAllRoleNames"
                                OldValuesParameterFormatString="original_{0}"  
                                TypeName="BSMSSystem.BLL.Security.RoleManager">
                            </asp:ObjectDataSource>
                            </ContentTemplate>
                        </asp:UpdatePanel>   
                    </div>

                    <div class="tab-pane fade" id="role" role="tabpanel" aria-labelledby="role-tab">
                        <asp:UpdatePanel ID="UpdatePanelrole" runat="server">
                            <ContentTemplate>
                               <asp:ListView ID="RoleListView" runat="server"
                                   DataSourceID="RoleODS"
                                   InsertItemPosition="LastItem"
                                   DataKeyNames="RoleID"
                                   ItemType="BSMSData.Entities.Security.RoleProfile"
                                   OnItemDeleted="RefreshAll"
                                   OnItemInserted="RefreshAll"
                                   >
                                   <EditItemTemplate>
                                       <span>No Security roles have been set up.</span>
                                   </EditItemTemplate>
                                   <LayoutTemplate>
                                       <div class="row bginfo">
                                           <div class="col-sm-3 h4">Action</div>
                                           <div class="col-sm-3 h4">Role</div>
                                           <div class="col-sm-6 h4">Member</div>
                                       </div>
                                       <div runat="server" id="itemPlaceholder"></div>
                                   </LayoutTemplate>
                                   <ItemTemplate>
                                       <div class="row">
                                           <div class="col-sm-3">
                                               <asp:LinkButton ID="DeleteButton" runat="server"
                                                   text="Delete" CommandName="Delete"></asp:LinkButton>
                                           </div>
                                           <div class="col-sm-3">
                                               <%# Item.RoleName %>
                                           </div>
                                           <div class="col-sm-6">
                                               <asp:Repeater ID="RoleUserRepeater" runat="server"
                                                   DataSource="<%# Item.UserNames %>"
                                                   ItemType="System.String">
                                                   <ItemTemplate>
                                                       <%# Item %>
                                                   </ItemTemplate>
                                               </asp:Repeater>
                                           </div>
                                       </div>
                                   </ItemTemplate>
                                   <InsertItemTemplate>
                                       <div class="row">
                                           <div class="col-sm-3">
                                               <asp:LinkButton ID="InsertButton" runat="server"
                                                   Text="Insert" CommandName="Insert"></asp:LinkButton>
                                                <asp:LinkButton ID="CancelButton" runat="server"
                                                   Text="Cancel" CommandName="Cancel"></asp:LinkButton>
                                           </div>
                                           <div class="col-sm-3">
                                               <asp:TextBox ID="RoleNameTextBox" runat="server"
                                                   Text="<%# BindItem.RoleName %>" 
                                                   placeholder="Role Name"></asp:TextBox>
                                           </div>
                                       </div>
                                   </InsertItemTemplate>
                               </asp:ListView>
                                <asp:ObjectDataSource ID="RoleODS" runat="server"
                                    OldValuesParameterFormatString="original_{0}"
                                    SelectMethod="ListAllRoles"
                                    DeleteMethod="DeleteRole" 
                                    InsertMethod="AddRole"
                                    TypeName="BSMSSystem.BLL.Security.RoleManager" 
                                    DataObjectTypeName="BSMSData.Entities.Security.RoleProfile"
                                    OnDeleted="CheckForException"
                                    OnInserted="CheckForException"
                                    OnSelected="CheckForException">
                                </asp:ObjectDataSource>
                            </ContentTemplate>
                        </asp:UpdatePanel>   
                    </div>
               </div>
            </div>
        </div>
    </div>
    
    <uc1:MessageUserControl runat="server" ID="MessageUserControl" />

</asp:Content>

