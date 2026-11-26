using System;
using System.Data.SqlClient;
using System.Web;

// Login page: checks the user in the database, saves him in the session,
// and remembers the username in a cookie if "Remember me" is checked
public partial class LoginPage : System.Web.UI.Page
{
    protected string savedUser = "";       // username to show in the box
    protected string rememberChecked = ""; // "checked" if the remember me box should be ticked

    protected void Page_Load(object sender, EventArgs e)
    {
        litUser.Text = Db.UserBar(Session);

        // first time opening the page: read the cookie (if he asked us to remember him)
        if (!IsPostBack)
        {
            if (Request.Cookies["UniBookUser"] != null)
            {
                savedUser = Request.Cookies["UniBookUser"].Value;
                rememberChecked = "checked";
            }
            return;
        }

        // form was submitted: read the values
        string username = (Request.Form["Username"] ?? "").Trim();
        string password = Request.Form["psword"] ?? "";
        bool remember = Request.Form["remember"] != null;
        savedUser = username;

        if (username == "" || password == "")
        {
            ShowResult("Please type your username and password", "red");
            return;
        }

        // look for the user in the database
        string firstName = null;
        using (SqlConnection con = new SqlConnection(Db.ConnStr))
        {
            SqlCommand cmd = new SqlCommand("SELECT FirstName FROM Users WHERE Username = @user AND Password = @pass", con);
            cmd.Parameters.AddWithValue("@user", username);
            cmd.Parameters.AddWithValue("@pass", password);
            con.Open();
            object result = cmd.ExecuteScalar();
            if (result != null)
            {
                firstName = result.ToString();
            }
        }

        if (firstName == null)
        {
            ShowResult("Wrong username or password", "red");
            return;
        }

        // session: keep the user logged in while he moves between pages
        Session["user"] = username;
        Session["name"] = firstName;

        // cookie: remember the username for 7 days, or delete it
        if (remember)
        {
            HttpCookie cookie = new HttpCookie("UniBookUser", username);
            cookie.Expires = DateTime.Now.AddDays(7);
            Response.Cookies.Add(cookie);
        }
        else if (Request.Cookies["UniBookUser"] != null)
        {
            HttpCookie old = new HttpCookie("UniBookUser");
            old.Expires = DateTime.Now.AddDays(-1); // date in the past = browser deletes it
            Response.Cookies.Add(old);
        }

        Response.Redirect("index.aspx");
    }

    // shows a colored message under the form
    private void ShowResult(string text, string color)
    {
        litResult.Text = "<span style=\"color:" + color + "\">" + text + "</span>";
    }
}
