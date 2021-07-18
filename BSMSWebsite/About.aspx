<%@ Page Title="About &middot; Zen Bikez" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="About.aspx.cs" Inherits="About" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="jumbotron jumbotron-fluid">
        <div class="container text-center">
            <h1 class="display-4 mt-5">About</h1>
            <p class="lead">Zen Bikez is an ASP.NET Web Forms application developed to showcase my ASP.NET and C# development skills.</p>
        </div>
    </div>
    
    <div class="container">
        <div class="row mb-4">
            <div class="col-12">
                <h2>Project Scope</h2>
                <p>
                    The main scope of this project is the Servicing subsystem, which was my individual deliverable for my Intermediate Application Development class final group project.
                    In the said project, the Servicing subsystem would have worked in tandem with three other subsystems, each developed by other team members, to work as a full business solution.
                    These other subsystems would've been:
                </p>
                <ul>
                    <li>Purchasing - supports inventory item purchases</li>
                    <li>Receiving and Returns - supports reception of products from suppliers</li>
                    <li>Sales - supports sales for online customers</li>
                </ul>
            </div>
        </div>
        <div class="row mb-4">
            <div class="col-12">
                <h2>Security Implementation</h2>
                <p>
                    The original group project application setup file already included security components.
                    Each team member only needed to adjust the supplied security components to support the required and appropriate security roles.
                </p>
            </div>
            <div class="col-3">
                <div class="card h-100">
                    <div class="card-body">
                        <h5 class="card-title">Application Administrators</h5>
                        <h6 class="card-subtitle mb-2 text-muted">User(s) than can fully manage the application:</h6>
                        <ul>
                            <li>Webmaster</li>
                        </ul>
                    </div>
                </div>
            </div>
            <div class="col-3">
                <div class="card h-100">
                    <div class="card-body">
                        <h5 class="card-title">Mechanics</h5>
                        <h6 class="card-subtitle mb-2 text-muted">User(s) that cannot access the Sales and Purchasing subsystems and Security management:</h6>
                        <ul>
                            <li>Nole Body</li>
                            <li>Willie Work</li>
                            <li>Ken Fixit</li>
                            <li>Brad Brake</li>
                            <li>Brian Brown</li>
                            <li>Bob Brooks</li>
                        </ul>
                    </div>
                </div>
            </div>
            <div class="col-3">
                <div class="card h-100">
                    <div class="card-body">
                        <h5 class="card-title">Clerks</h5>
                        <h6 class="card-subtitle mb-2 text-muted">User(s) that cannot access Servicing and Sales subsystems and Security management:</h6>
                        <ul>
                            <li>Freda Flash</li>
                            <li>Sadie Star</li>
                        </ul>
                    </div>
                </div>
            </div>
            <div class="col-3">
                <div class="card h-100">
                    <div class="card-body">
                        <h5 class="card-title">Sales</h5>
                        <h6 class="card-subtitle mb-2 text-muted">User(s) that cannot access Servicing and Purchasing subsystems and Security management:</h6>
                        <ul>
                            <li>Max Smart</li>
                            <li>Shelia Seller</li>
                        </ul>
                    </div>
                </div>
            </div>
        </div>

        <div class="row mb-4">
            <div class="col-12">
                <h2>User Name</h2>
                <p class="text-muted">The default username for each user, <strong>except the Webmaster</strong>, is their first name's first letter, followed by their lastname.
                    (i.e. jdoe for John Doe). The Webmaster can login as webmaster.</p>
            </div>
        </div>
        <div class="row">
            <div class="col-12">
                <h2>Password</h2>
                <p class="text-muted">The default password for the users listed above is <strong>Pa$$word1</strong></p>
            </div>
        </div>
    </div>

</asp:Content>
