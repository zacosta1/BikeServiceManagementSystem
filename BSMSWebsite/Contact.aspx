<%@ Page Title="Contact" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Contact.aspx.cs" Inherits="Contact" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="jumbotron jumbotron-fluid">
        <div class="container text-center">
            <h1 class="display-4 mt-5">Contact</h1>
            <p class="lead">We'd love to hear from you! Here's how you can reach us...</p>
        </div>
    </div>
    <div class="container">
        <div class="row">
            <div class="col-md-9 mb-5">
                <h2>Contact Us</h2>
                <div class="form-group">
                    <asp:Label runat="server" ID="NameLabel" AssociatedControlID="NameTextBox">Name</asp:Label>
                    <asp:TextBox runat="server" ID="NameTextBox" CssClass="form-control" ToolTip="Enter your name."/>
                    <small class="form-text">
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="NameTextBox" CssClass="text-danger" ErrorMessage="Name required." Display="Dynamic" SetFocusOnError ="true"
                            ValidationGroup="Contact"/>
                    </small>
                </div>

                <div class="form-group">
                    <asp:Label runat="server" ID="EmailLabel" AssociatedControlID="EmailTextBox">Email</asp:Label>
                    <asp:TextBox runat="server" ID="EmailTextBox" CssClass="form-control" ToolTip="Enter your email." AutoCompleteType="Email"/>
                    <small class="form-text">
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="EmailTextBox" CssClass="text-danger" ErrorMessage="Email required." Display="Dynamic" SetFocusOnError ="true"
                            ValidationGroup="Contact"/>
                    </small>
                </div>

                <div class="form-group">
                    <asp:Label runat="server" ID="SubjectLabel" AssociatedControlID="SubjectTextBox">Subject</asp:Label>
                    <asp:TextBox runat="server" ID="SubjectTextBox" CssClass="form-control" ToolTip="Enter the subject."/>
                </div>

                <div class="form-group">
                    <asp:Label runat="server" ID="MessageLabel" AssociatedControlID="messageTextArea">Message</asp:Label>
                    <textarea runat="server" id="messageTextArea" class="form-control" rows="3" style="min-height:5rem;"></textarea>
                    <small class="form-text">
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="messageTextArea" CssClass="text-danger" ErrorMessage="Message required." Display="Dynamic"
                            SetFocusOnError ="true" ValidationGroup="Contact"/>
                    </small>
                </div>

                <asp:Button Text="Send Message" runat="server" CssClass="btn btn-primary btn-block" ID="SendMessageButton" OnClick="SendMessageButton_Click" CausesValidation="true"
                    ValidationGroup="Contact"/>
            </div>
            <div class="col-md-3">
                <h3>Location</h3>
                <address>
                    Zen Bikez<br />
                    1234 Jasper Ave<br />
                    Edmonton, AB, T6Z 3N1
                </address>
                <hr />
                <h3>Phone</h3>
                +1 (234)-567-8910
                <hr />
                <h3>Email</h3>
                <address>
                    <strong>Support:</strong>   <a href="mailto:Support@zenbikez.ca">Support@zenbikez.ca</a><br />
                    <strong>Marketing:</strong> <a href="mailto:Marketing@zenbikez.ca">Marketing@zenbikez.ca</a>
                </address>
            </div>
        </div>
    </div>
</asp:Content>
