using System;

// Logout: end the session and go back to the home page
public partial class LogoutPage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Session.Abandon();
        Response.Redirect("index.aspx");
    }
}
