<%@ Page Title="Log In &middot; Zen Bikez" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Account_Login" Async="true" %>

<%@ Register Src="~/Account/OpenAuthProviders.ascx" TagPrefix="uc" TagName="OpenAuthProviders" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <div class="text-center form-signin my-auto">
        <h1 class="mt-5 display-4">Zen Bikez</h1>
        <h2 class="h3 mb-3 font-weight-normal">Log In</h2>
        <asp:PlaceHolder runat="server" ID="ErrorMessage" Visible="false">
            <p class="text-danger">
                <asp:Literal runat="server" ID="FailureText" />
            </p>
        </asp:PlaceHolder>
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="UserName" CssClass="sr-only">User name</asp:Label>
            <asp:TextBox runat="server" ID="UserName" CssClass="form-control" placeholder="User name" />
            <small class="form-text">
                <asp:RequiredFieldValidator runat="server" ControlToValidate="UserName" CssClass="text-danger" ErrorMessage="User name required."
                    Display="Dynamic" SetFocusOnError ="true"/>
            </small>
        </div>

        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="Password" CssClass="sr-only">Password</asp:Label>
            <asp:TextBox runat="server" ID="Password" TextMode="Password" CssClass="form-control" placeholder="Password" />
            <small class="form-text text-danger">
                <asp:RequiredFieldValidator runat="server" ControlToValidate="Password" CssClass="text-danger" ErrorMessage="Password required."
                    Display="Dynamic" SetFocusOnError ="true"/>
            </small>
        </div>

        <div class="custom-checkbox mb-3">
            <asp:Label runat="server" AssociatedControlID="RememberMe">
            <asp:CheckBox runat="server" ID="RememberMe"/> Remember me </asp:Label>
        </div>

        <asp:Button runat="server" OnClick="LogIn" Text="Log in" CssClass="btn btn-lg btn-primary btn-block mb-1" />
        <p class="mb-0">
            <asp:HyperLink runat="server" ID="RegisterHyperLink" ViewStateMode="Disabled">Register</asp:HyperLink>
            if you don't have an account.
        </p>
       

            <%--<div class="col-md-4">
                <section id="socialLoginForm">
                    <uc:openauthproviders runat="server" id="OpenAuthLogin" />
                </section>
            </div>--%>
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

