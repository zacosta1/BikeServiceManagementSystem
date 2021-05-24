<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MessageUserControl.ascx.cs" Inherits="User_Controls_MessageUserControl" %>

<div aria-live="polite" aria-atomic="true" class="position-fixed p-3 d-flex justify-content-center align-items-center" style="z-index: 5; right: 0; left:0; top: 5%;">
    <asp:Panel ID="MessagePanel" runat="server" role="alert" CssClass="toast" aria-atomic="true" data-delay="30000">
        <div class="toast-header">
            <asp:Label ID="MessageTitleIcon" runat="server"/>
            <strong class="mr-auto"><asp:Label ID="MessageTitle" runat="server" /></strong>
            <button type="button" class="ml-2 b-1 close" data-dismiss="toast" aria-label="Close">
                <span aria-hidden="true">&times;</span>
            </button>
        </div>
        <div class="toast-body">
            <p class="mb-0">
                <asp:Label ID="MessageLabel" runat="server" />
            </p>
            <asp:Repeater ID="MessageDetailsRepeater" runat="server" EnableViewState="false">
                <headertemplate>
                    <hr />
                    <ul>
                </headertemplate>
                <footertemplate>
                    </ul>
                </footertemplate>
                <ItemTemplate>
                    <li><%# Eval("Error") %></li>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </asp:Panel>
</div>
<%-- temporarily fixes close button --%>
<script hidden>
    $('.toast').toast('show');
</script>