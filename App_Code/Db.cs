using System;
using System.Configuration;
using System.Web;
using System.Web.SessionState;

// Shared helpers used by all the pages
public static class Db
{
    // connection string to the UniBook database (written in Web.config)
    public static string ConnStr
    {
        get { return ConfigurationManager.ConnectionStrings["UniBookDB"].ConnectionString; }
    }

    // text for the menu: "Welcome, name" + Logout link if the user is logged in (session)
    public static string UserBar(HttpSessionState session)
    {
        if (session["user"] == null)
        {
            return "";
        }
        string name = HttpUtility.HtmlEncode(session["name"].ToString());
        return "<span class=\"welcome\">Welcome, " + name + "</span> <a href=\"logout.aspx\">Logout</a>";
    }
}
