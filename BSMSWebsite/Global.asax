<%@ Application Language="C#" %>
<%@ Import Namespace="BSMSWebsite" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="System.Web.Routing" %>
<%@ Import Namespace="BSMSSystem.BLL.Security" %>

<script runat="server">

    void Application_Start(object sender, EventArgs e)
    {
        RouteConfig.RegisterRoutes(RouteTable.Routes);
        BundleConfig.RegisterBundles(BundleTable.Bundles);

        //Create the startup default roles 
        //Create the webmaster user
        //Create the employee user accounts
        //when the application starts up

        var RoleManager = new RoleManager();
        RoleManager.AddDefaultRoles();

        var UserManager = new UserManager();
        UserManager.AddWebMaster();
        UserManager.AddEmployees();
    }

</script>
