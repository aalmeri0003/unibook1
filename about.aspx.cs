using System;

// About page: only shows who is logged in
public partial class AboutPage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        litUser.Text = Db.UserBar(Session);
    }
}
