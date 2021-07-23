<%@ Page Title="Zen Bikez &middot; Making bike rides feel like you're floating!" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="jumbotron jumbotron-fluid">
        <div class="container text-center">
            <h1 class="display-1 mt-5">Zen Bikez</h1>
            <p class="lead">Making bike rides feel like you're floating!</p>
        </div>
    </div>

    <div class="container-fluid">
        <div class="row row-cols-1 row-cols-md-2 mb-4">
            <div class="col">
                <div class="card">
                    <div class="card-body">
                        <img src="https://via.placeholder.com/1040x320/7f7f7f/7f7f7f" class="card-img-top" alt="...">
                        <h2 class="card-title">In-Store Bike Servicing</h2>
                        <h3 class="card-subtitle mb-2 text-muted h5">We can make your bikes go zen!</h3>
                        <p>Book now and bring your bike for servicing.</p>

                        <asp:HyperLink NavigateUrl="#" runat="server" CssClass="btn btn-lg btn-primary" Text="Book Now"/>
                    </div>
                </div>
            </div>

            <div class="col">
                <div class="card h-100">
                    <div class="card-body">
                        <img src="https://via.placeholder.com/1040x320/7f7f7f/7f7f7f"" class="card-img-top" alt="...">
                        <h2 class="card-title">Shop Online</h2>
                        <h3 class="card-subtitle mb-2 text-muted h5">Want to DIY but lack the parts?</h3>
                        <p>Visit our online shop for extra parts from our inventory!</p>
                        
                        <asp:HyperLink NavigateUrl="#" runat="server" CssClass="btn btn-lg btn-primary mt-auto" Text="Shop Now"/>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
