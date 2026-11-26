<%@ Application Language="C#" %>

<script runat="server">
    // runs once when the site starts: make sure the database exists
    void Application_Start(object sender, EventArgs e)
    {
        DbSetup.CreateIfMissing(Server.MapPath("~/App_Data"));
    }
</script>
