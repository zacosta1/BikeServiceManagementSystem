<%@ Page Title="Manage Account &middot; Zen Bikez" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Manage.aspx.cs" Inherits="Account_Manage" %>

<%@ Register Src="~/Account/OpenAuthProviders.ascx" TagPrefix="uc" TagName="OpenAuthProviders" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="jumbotron jumbotron-fluid">
        <div class="container text-center">
            <h1 class="display-4 mt-5">Manage Account</h1>
            <p class="lead">Change your password</p>
        </div>
    </div>
    <div class="text-center form-signin my-auto">
        <asp:PlaceHolder runat="server" ID="successMessage" Visible="false" ViewStateMode="Disabled">
            <p class="text-success"><%: SuccessMessage %></p>
        </asp:PlaceHolder>

        <section id="passwordForm">
            <asp:PlaceHolder runat="server" ID="changePasswordHolder" Visible="false">
                <p>You're logged in as <strong><%: User.Identity.GetUserName() %></strong>.</p>
                
                <asp:ValidationSummary runat="server" ShowModelStateErrors="true" CssClass="text-danger" />
                <div class="form-group">
                    <asp:Label runat="server" ID="CurrentPasswordLabel" AssociatedControlID="CurrentPassword" CssClass="sr-only">Current password</asp:Label>
                    <asp:TextBox runat="server" ID="CurrentPassword" TextMode="Password" CssClass="form-control" placeholder="Current Password" ToolTip="Enter your current password."/>
                    <small class="form-text">
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="CurrentPassword" CssClass="text-danger" ErrorMessage="The current password is required."
                            Display="Dynamic" SetFocusOnError="true" ValidationGroup="ChangePassword" />
                    </small>
                </div>
                <div class="form-group">
                    <asp:Label runat="server" ID="NewPasswordLabel" AssociatedControlID="NewPassword" CssClass="sr-only">New password</asp:Label>
                    <asp:TextBox runat="server" ID="NewPassword" TextMode="Password" CssClass="form-control" placeholder="New Password" ToolTip="Enter your desired new password."/>
                    <small class="form-text">
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="NewPassword" CssClass="text-danger" ErrorMessage="A new password is required."
                            Display="Dynamic" SetFocusOnError="true" ValidationGroup="ChangePassword" />
                    </small>
                </div>
                <div class="form-group">
                    <asp:Label runat="server" ID="ConfirmNewPasswordLabel" AssociatedControlID="ConfirmNewPassword" CssClass="sr-only">Confirm new password</asp:Label>
                    <asp:TextBox runat="server" ID="ConfirmNewPassword" TextMode="Password" CssClass="form-control" placeholder="Confirm New Password"
                        ToolTip="Re-enter your desired new password to confirm."/>
                    <small class="form-text">
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="ConfirmNewPassword" CssClass="text-danger" Display="Dynamic"
                            ErrorMessage="New password confirmation is required." SetFocusOnError="true" ValidationGroup="ChangePassword" />
                        <asp:CompareValidator runat="server" ControlToCompare="NewPassword" ControlToValidate="ConfirmNewPassword" CssClass="text-danger" Display="Dynamic" SetFocusOnError="true"
                            ErrorMessage="The new password and confirmation password do not match."
                            ValidationGroup="ChangePassword" />
                    </small>
                </div>
                <asp:Button runat="server" Text="Save" OnClick="ChangePassword_Click" CssClass="btn btn-lg btn-block btn-success" ValidationGroup="ChangePassword"
                    ToolTip="Save password change"/>
            </asp:PlaceHolder>
        </section>
    </div>
    
    <style>
        main {
            margin-bottom: auto;
        }
        .footer {
            margin: 0;
        }
    </style>
</asp:Content>
