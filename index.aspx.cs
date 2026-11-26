using System;
using System.Data.SqlClient;
using System.Text;
using System.Web;

// Home page: shows the last visit (cookie) and searches the books in the database
public partial class IndexPage : System.Web.UI.Page
{
    protected string searchText = ""; // what the user searched for (to keep it in the box)

    protected void Page_Load(object sender, EventArgs e)
    {
        litUser.Text = Db.UserBar(Session);

        if (!IsPostBack)
        {
            // cookie: show the last visit, then save this visit for next time
            if (Request.Cookies["LastVisit"] != null)
            {
                string last = HttpUtility.UrlDecode(Request.Cookies["LastVisit"].Value);
                litLastVisit.Text = "Welcome back! Your last visit was on " + HttpUtility.HtmlEncode(last);
            }
            HttpCookie visit = new HttpCookie("LastVisit", HttpUtility.UrlEncode(DateTime.Now.ToString("dd/MM/yyyy hh:mm tt")));
            visit.Expires = DateTime.Now.AddDays(30);
            Response.Cookies.Add(visit);
            return;
        }

        // search form was submitted
        searchText = (Request.Form["Serbook"] ?? "").Trim();
        if (searchText == "")
        {
            return;
        }
        SearchBooks(searchText);
    }

    // finds books by name or course code and builds a results table
    private void SearchBooks(string text)
    {
        StringBuilder html = new StringBuilder();
        int count = 0;

        using (SqlConnection con = new SqlConnection(Db.ConnStr))
        {
            SqlCommand cmd = new SqlCommand(
                "SELECT CourseCode, BookName, Version, NewPrice, UsedPrice FROM Books " +
                "WHERE BookName LIKE @text OR CourseCode LIKE @text ORDER BY CourseCode", con);
            cmd.Parameters.AddWithValue("@text", "%" + text + "%");
            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            html.Append("<table><caption><b>Search results for: " + HttpUtility.HtmlEncode(text) + "</b></caption>");
            html.Append("<tr><th>Course Code</th><th>Book Name</th><th>Version</th><th>New Book Price</th><th>Used Book Price</th></tr>");
            while (reader.Read())
            {
                html.Append("<tr>");
                html.Append("<td>" + HttpUtility.HtmlEncode(reader["CourseCode"].ToString()) + "</td>");
                html.Append("<td>" + HttpUtility.HtmlEncode(reader["BookName"].ToString()) + "</td>");
                html.Append("<td>" + HttpUtility.HtmlEncode(reader["Version"].ToString()) + "</td>");
                html.Append("<td>" + reader["NewPrice"] + " SR</td>");
                html.Append("<td>" + reader["UsedPrice"] + " SR</td>");
                html.Append("</tr>");
                count++;
            }
            html.Append("</table>");
            reader.Close();
        }

        if (count == 0)
        {
            litResults.Text = "<p style=\"color:red\">No books found for: " + HttpUtility.HtmlEncode(text) + "</p>";
        }
        else
        {
            litResults.Text = html.ToString();
        }
    }
}
