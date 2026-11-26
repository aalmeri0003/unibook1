using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;

// Manage Books page (logged in users only):
// search, show, add, update and delete books in the database
public partial class BooksPage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // session: only logged in users can open this page
        if (Session["user"] == null)
        {
            Response.Redirect("login.aspx");
            return;
        }

        litUser.Text = Db.UserBar(Session);

        if (!IsPostBack)
        {
            ShowBooks("");
        }
    }

    // RETRIEVE / SEARCH: fill the table with the books (all books if search is empty)
    private void ShowBooks(string search)
    {
        using (SqlConnection con = new SqlConnection(Db.ConnStr))
        {
            SqlCommand cmd = new SqlCommand(
                "SELECT CourseCode, BookName, Faculty, Version, NewPrice, UsedPrice FROM Books " +
                "WHERE BookName LIKE @text OR CourseCode LIKE @text ORDER BY Faculty, CourseCode", con);
            cmd.Parameters.AddWithValue("@text", "%" + search + "%");
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable table = new DataTable();
            adapter.Fill(table);
            gvBooks.DataSource = table;
            gvBooks.DataBind();
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        ShowBooks(txtSearch.Text.Trim());
    }

    protected void btnShowAll_Click(object sender, EventArgs e)
    {
        txtSearch.Text = "";
        ShowBooks("");
    }

    // RETRIEVE one book: put its data in the boxes so it can be updated
    protected void btnLoad_Click(object sender, EventArgs e)
    {
        string code = txtCode.Text.Trim();
        if (code == "")
        {
            ShowMsg("Type a course code first", Color.Red);
            return;
        }

        using (SqlConnection con = new SqlConnection(Db.ConnStr))
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM Books WHERE CourseCode = @code", con);
            cmd.Parameters.AddWithValue("@code", code);
            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                txtName.Text = reader["BookName"].ToString();
                ddlFaculty.SelectedValue = reader["Faculty"].ToString();
                txtVersion.Text = reader["Version"].ToString();
                txtNewPrice.Text = reader["NewPrice"].ToString();
                txtUsedPrice.Text = reader["UsedPrice"].ToString();
                ShowMsg("Book loaded, you can update or delete it now", Color.Green);
            }
            else
            {
                ShowMsg("No book with course code " + code, Color.Red);
            }
            reader.Close();
        }
    }

    // INSERT a new book
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        int newPrice, usedPrice;
        if (!CheckFields(out newPrice, out usedPrice))
        {
            return;
        }

        using (SqlConnection con = new SqlConnection(Db.ConnStr))
        {
            con.Open();

            SqlCommand check = new SqlCommand("SELECT COUNT(*) FROM Books WHERE CourseCode = @code", con);
            check.Parameters.AddWithValue("@code", txtCode.Text.Trim());
            if ((int)check.ExecuteScalar() > 0)
            {
                ShowMsg("A book with this course code already exists, use Update instead", Color.Red);
                return;
            }

            SqlCommand cmd = new SqlCommand(
                "INSERT INTO Books (CourseCode, BookName, Faculty, Version, NewPrice, UsedPrice) " +
                "VALUES (@code, @name, @fac, @ver, @new, @used)", con);
            AddBookParameters(cmd, newPrice, usedPrice);
            cmd.ExecuteNonQuery();
        }

        ShowMsg("Book added", Color.Green);
        ShowBooks("");
    }

    // UPDATE the book with this course code
    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        int newPrice, usedPrice;
        if (!CheckFields(out newPrice, out usedPrice))
        {
            return;
        }

        int rows;
        using (SqlConnection con = new SqlConnection(Db.ConnStr))
        {
            SqlCommand cmd = new SqlCommand(
                "UPDATE Books SET BookName = @name, Faculty = @fac, Version = @ver, NewPrice = @new, UsedPrice = @used " +
                "WHERE CourseCode = @code", con);
            AddBookParameters(cmd, newPrice, usedPrice);
            con.Open();
            rows = cmd.ExecuteNonQuery();
        }

        if (rows == 0)
        {
            ShowMsg("No book with this course code", Color.Red);
        }
        else
        {
            ShowMsg("Book updated", Color.Green);
            ShowBooks("");
        }
    }

    // DELETE the book with this course code
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        string code = txtCode.Text.Trim();
        if (code == "")
        {
            ShowMsg("Type the course code of the book you want to delete", Color.Red);
            return;
        }

        int rows;
        using (SqlConnection con = new SqlConnection(Db.ConnStr))
        {
            SqlCommand cmd = new SqlCommand("DELETE FROM Books WHERE CourseCode = @code", con);
            cmd.Parameters.AddWithValue("@code", code);
            con.Open();
            rows = cmd.ExecuteNonQuery();
        }

        if (rows == 0)
        {
            ShowMsg("No book with this course code", Color.Red);
        }
        else
        {
            ShowMsg("Book deleted", Color.Green);
            ClearFields();
            ShowBooks("");
        }
    }

    // checks the boxes are filled and the prices are numbers
    private bool CheckFields(out int newPrice, out int usedPrice)
    {
        usedPrice = 0;
        if (txtCode.Text.Trim() == "" || txtName.Text.Trim() == "" || txtVersion.Text.Trim() == "")
        {
            newPrice = 0;
            ShowMsg("Please fill the course code, book name and version", Color.Red);
            return false;
        }
        if (!int.TryParse(txtNewPrice.Text, out newPrice) || !int.TryParse(txtUsedPrice.Text, out usedPrice) || newPrice < 0 || usedPrice < 0)
        {
            ShowMsg("Prices must be numbers", Color.Red);
            return false;
        }
        return true;
    }

    // same parameters for insert and update
    private void AddBookParameters(SqlCommand cmd, int newPrice, int usedPrice)
    {
        cmd.Parameters.AddWithValue("@code", txtCode.Text.Trim());
        cmd.Parameters.AddWithValue("@name", txtName.Text.Trim());
        cmd.Parameters.AddWithValue("@fac", ddlFaculty.SelectedValue);
        cmd.Parameters.AddWithValue("@ver", txtVersion.Text.Trim());
        cmd.Parameters.AddWithValue("@new", newPrice);
        cmd.Parameters.AddWithValue("@used", usedPrice);
    }

    private void ClearFields()
    {
        txtCode.Text = "";
        txtName.Text = "";
        txtVersion.Text = "";
        txtNewPrice.Text = "";
        txtUsedPrice.Text = "";
    }

    private void ShowMsg(string text, Color color)
    {
        lblMsg.Text = text;
        lblMsg.ForeColor = color;
    }
}
