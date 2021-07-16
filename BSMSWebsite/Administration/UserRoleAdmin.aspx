<%@ Page Title="Security &middot; Zen Bikez" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="UserRoleAdmin.aspx.cs" Inherits="Administration_UserRoleAdmin" MaintainScrollPositionOnPostback="true" %>

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
                <div class="card text-center">
                    <div class="card-header">
                        <!--Nav tabs-->
                        <ul class="nav nav-tabs nav-justified card-header-tabs" role="tablist">
                            <li class="nav-item" role="presentation">
                                <a class="nav-link active text-decoration-none" id="user-tab" data-toggle="tab" href="#user" role="tab" aria-controls="user" aria-selected="true">Manage Users</a>
                            </li>
                            <li class="nav-item" role="presentation">
                                <a class="nav-link text-decoration-none" id="role-tab" data-toggle="tab" href="#role" role="tab" aria-controls="role" aria-selected="false">Manage Roles</a>
                            </li>
                        </ul>
                    </div>

                    <!--Tab panes one for each tab-->
                    <div class="card-body tab-content"> 
                        <!--Manage Users tab pane-->
                        <div class="tab-pane fade show active" id="user" role="tabpanel" aria-labelledby="user-tab">
                            <h5 class="card-title">Manage Users</h5>
                            <p class="card-text text-muted">Add or remove users</p>
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
                                        <span>No users have been set up.</span>
                                    </EmptyDataTemplate>
                                    <LayoutTemplate>
                                        <div class="table-responsive">
                                            <table runat="server" id="itemPlaceholderContainer" class="table table-hover table-striped table-borderless mb-0">
                                                <caption>List of users</caption>
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
                                                    CssClass="btn btn-link btn-block text-decoration-none text-danger" ToolTip="Remove User">
                                                    <i class="bi bi-person-dash-fill h6"></i>
                                                </asp:LinkButton>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                    <InsertItemTemplate>
                                        <tr class="table-active bg-transparent text-center">
                                            <td class="align-top">
                                                 <asp:TextBox ID="UserNameTextBox" runat="server"
                                                     text='<%# BindItem.UserName %>' 
                                                     placeholder="User Name"
                                                     CssClass="form-control" />
                                                <small id="newUserNameHelp" class="form-text">
                                                    <asp:RequiredFieldValidator ErrorMessage="User Name is required." ControlToValidate="UserNameTextBox" runat="server"
                                                        CssClass="text-danger" ValidationGroup="InsertUserGroup" Display="Dynamic" SetFocusOnError ="true" />
                                                </small>
                                            </td>
                                            <td class="align-top">
                                            </td>
                                            <td class="align-top">
                                                <asp:TextBox ID="EmployeeIDTextBox" runat="server"
                                                    text='<%# BindItem.EmployeeId %>' TextMode="Number" 
                                                    placeholder="Employee ID"
                                                    CssClass="form-control"/>
                                                <small id="newUserEmployeeIdHelp" class="form-text">
                                                    <asp:RequiredFieldValidator ErrorMessage="Employee ID is required." ControlToValidate="EmployeeIDTextBox" runat="server"
                                                        CssClass="text-danger" ValidationGroup="InsertUserGroup" Display="Dynamic" SetFocusOnError ="true" />
                                                </small>
                                            </td>
                                            <td class="align-top text-left">
                                                <asp:CheckBoxList ID="RoleMemberships" runat="server"
                                                    DataSourceID="RoleNameODS" CssClass="bg-transparent custom-control custom-checkbox" RepeatDirection="Vertical" RepeatLayout="Flow"></asp:CheckBoxList>
                                            </td>
                                            <td class="align-top">
                                                <asp:LinkButton ID="InsertUser" runat="server" CommandName="Insert" Text="Add User" ToolTip="Add User"
                                                    CssClass="btn btn-link text-decoration-none text-success" CausesValidation="true" ValidationGroup="InsertUserGroup">
                                                    <i class="bi bi-person-plus-fill h6"></i>
                                                </asp:LinkButton>
                                                <asp:LinkButton ID="CancelButton" runat="server" CommandName="Cancel" Text="Clear" ToolTip="Clear Insert Row" 
                                                    CssClass="btn btn-link text-decoration-none text-secondary">
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

                        <!--Manage Roles tab pane-->
                        <div class="tab-pane fade" id="role" role="tabpanel" aria-labelledby="role-tab">
                            <h5 class="card-title">Manage Roles</h5>
                            <p class="card-text text-muted">Add or remove roles</p>
                            <asp:UpdatePanel ID="UpdatePanelrole" runat="server">
                                <ContentTemplate>
                                   <asp:ListView ID="RoleListView" runat="server"
                                       DataSourceID="RoleODS"
                                       InsertItemPosition="LastItem"
                                       DataKeyNames="RoleID"
                                       ItemType="BSMSData.Entities.Security.RoleProfile"
                                       OnItemDeleted="RefreshAll"
                                       OnItemInserted="RefreshAll">
                                       <EditItemTemplate>
                                           <span>No Security roles have been set up.</span>
                                       </EditItemTemplate>
                                       <LayoutTemplate>
                                           <div class="table-responsive">
                                               <table runat="server" id="itemPlaceholderContainer" class="table table-hover table-striped table-borderless mb-0">
                                                   <caption>List of roles</caption>
                                                   <thead>
                                                       <tr runat="server" class="thead-dark text-center align-middle">
                                                           <th runat="server" scope="col">Role</th>
                                                           <th runat="server" scope="col">Member(s)</th>
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
                                           <tr class="text-center align-middle">
                                               <td class="align-middle">
                                                   <%# Item.RoleName %></td>
                                               <td class="align-middle">
                                                   <asp:Repeater ID="RoleUserRepeater" runat="server"
                                                       DataSource="<%# Item.UserNames %>"
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
                                                   <asp:LinkButton ID="DeleteButton" runat="server" CommandName="Delete" Text="Remove"
                                                       CssClass="btn btn-link btn-block text-decoration-none text-danger" ToolTip="Remove Role">
                                                       <i class="bi bi-trash-fill h6"></i>
                                                   </asp:LinkButton>
                                               </td>
                                           </tr>
                                       </ItemTemplate>
                                       <InsertItemTemplate>
                                           <tr class="table-active bg-transparent text-center align-middle">
                                               <td class="align-top">
                                                   <asp:TextBox ID="RoleNameTextBox" runat="server"
                                                       Text="<%# BindItem.RoleName %>" 
                                                       placeholder="Role Name" CssClass="form-control"/>
                                                   <small id="newRoleNameHelp" class="form-text">
                                                    <asp:RequiredFieldValidator ErrorMessage="Role Name is required." ControlToValidate="RoleNameTextBox" runat="server"
                                                        CssClass="text-danger" ValidationGroup="InsertRoleGroup" Display="Dynamic" SetFocusOnError ="true" />
                                                </small>
                                               </td>
                                               <td></td>
                                               <td class="align-top">
                                                   <asp:LinkButton ID="InsertButton" runat="server" CommandName="Insert" Text="Add Role" ToolTip="Add Role"
                                                       CssClass="btn btn-link text-decoration-none text-success" CausesValidation="true" ValidationGroup="InsertRoleGroup">
                                                       <i class="bi bi-plus-circle-fill h6"></i>
                                                   </asp:LinkButton>
                                                   <asp:LinkButton ID="CancelButton" runat="server" CommandName="Cancel" Text="Clear" ToolTip="Clear Insert Row" 
                                                       CssClass="btn btn-link text-decoration-none text-secondary">
                                                       <i class="bi bi-backspace-fill h6"></i>
                                                   </asp:LinkButton>
                                               </td>
                                           </tr>
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
    </div>
    
    <uc1:MessageUserControl runat="server" ID="MessageUserControl" />

</asp:Content>

