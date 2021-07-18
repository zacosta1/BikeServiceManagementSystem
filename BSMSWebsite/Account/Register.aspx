<%@ Page Title="Register &middot; Zen Bikez" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Register.aspx.cs" Inherits="Account_Register" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <div class="text-center form-signin my-auto">
        <h1 class="mb-5">Create Account</h1>
        <p class="text-danger">
            <asp:Literal runat="server" ID="ErrorMessage" />
        </p>

        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="UserName" CssClass="sr-only">Username</asp:Label>
            <asp:TextBox runat="server" ID="UserName" CssClass="form-control" placeholder="Username" />
            <small class="form-text">
                <asp:RequiredFieldValidator runat="server" ControlToValidate="UserName"
                    CssClass="text-danger" ErrorMessage="A user name is required." 
                    Display="Dynamic" SetFocusOnError="true" />
            </small>
        </div>
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="Password" CssClass="sr-only">Password</asp:Label>
            <asp:TextBox runat="server" ID="Password" TextMode="Password" CssClass="form-control" placeholder="Password" />
            <small class="form-text">
                <asp:RequiredFieldValidator runat="server" ControlToValidate="Password"
                    CssClass="text-danger" ErrorMessage="A password is required."
                    Display="Dynamic" SetFocusOnError="true" />
            </small>
        </div>
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="ConfirmPassword" CssClass="sr-only">Confirm password</asp:Label>
            <asp:TextBox runat="server" ID="ConfirmPassword" TextMode="Password" CssClass="form-control" placeholder="Confirm Password" ToolTip="Re-enter password to confirm." />
            <small class="form-text">
                <asp:RequiredFieldValidator runat="server" ControlToValidate="ConfirmPassword"
                    CssClass="text-danger" Display="Dynamic" ErrorMessage="A password confirmation is required." SetFocusOnError="true" />
                <asp:CompareValidator runat="server" ControlToCompare="Password" ControlToValidate="ConfirmPassword"
                    CssClass="text-danger" Display="Dynamic" ErrorMessage="The password and confirmation password do not match." SetFocusOnError="true"  />
            </small>
        </div>
        <asp:Button runat="server" OnClick="CreateUser_Click" Text="Register" CssClass="btn btn-lg btn-success btn-block" />
    </div>
    
    <style>
        main {
            margin-top: auto;
            margin-bottom: auto;
        }
        .footer {
            margin: 0;
        }
    </style>
</asp:Content>

