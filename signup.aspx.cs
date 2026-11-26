using System;
using System.Data.SqlClient;
using System.Web;

// Sign up page: receives the form data and saves the new user in the database
public partial class SignupPage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        litUser.Text = Db.UserBar(Session);

        // first time opening the page, nothing to process yet
        if (!IsPostBack)
        {
            return;
        }

        // read what the user typed in the form
        string fname = (Request.Form["fname"] ?? "").Trim();
        string lname = (Request.Form["lname"] ?? "").Trim();
        string username = (Request.Form["Username"] ?? "").Trim();
        string email = (Request.Form["email"] ?? "").Trim();
        string password = Request.Form["psw"] ?? "";
        string cpassword = Request.Form["cpsw"] ?? "";
        string faculty = Request.Form["fac"]; // null if no faculty picked

        // check again on the server (in case JavaScript is turned off)
        if (fname == "" || lname == "" || username == "" || email == "" || password == "" || faculty == null)
        {
            ShowResult("Please fill all the fields and choose your faculty", "red");
            return;
        }
        if (password != cpassword)
        {
            ShowResult("Password does not match", "red");
            return;
        }

        using (SqlConnection con = new SqlConnection(Db.ConnStr))
        {
            con.Open();

            // username must be new
            SqlCommand check = new SqlCommand("SELECT COUNT(*) FROM Users WHERE Username = @user", con);
            check.Parameters.AddWithValue("@user", username);
            int found = (int)check.ExecuteScalar();
            if (found > 0)
            {
                ShowResult("This username is already taken, try another one", "red");
                return;
            }

            // insert the new user
            SqlCommand insert = new SqlCommand(
                "INSERT INTO Users (FirstName, LastName, Username, Email, Password, Faculty) " +
                "VALUES (@fname, @lname, @user, @email, @pass, @fac)", con);
            insert.Parameters.AddWithValue("@fname", fname);
            insert.Parameters.AddWithValue("@lname", lname);
            insert.Parameters.AddWithValue("@user", username);
            insert.Parameters.AddWithValue("@email", email);
            insert.Parameters.AddWithValue("@pass", password);
            insert.Parameters.AddWithValue("@fac", faculty);
            insert.ExecuteNonQuery();
        }

        ShowResult("Account created successfully, welcome " + HttpUtility.HtmlEncode(fname) +
                   "! You can <a href=\"login.aspx\">login</a> now", "green");
    }

    // shows a colored message under the form
    private void ShowResult(string text, string color)
    {
        litResult.Text = "<span style=\"color:" + color + "\">" + text + "</span>";
    }
}
