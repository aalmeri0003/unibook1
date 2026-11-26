<%@ Page Language="C#" AutoEventWireup="true" CodeFile="books.aspx.cs" Inherits="BooksPage" %>
<!DOCTYPE html>
<html>

<head>
    <meta charset="UTF-8" />
    <title>UniBook - Manage Books</title>
    <link rel="stylesheet" type="text/css" href="style.css" />
    <script src="script.js"></script>
</head>

<body>
    <header>
    <img src="logo.jpg" alt="UniBook logo" width="120" height="120" usemap="#logo" />
        <map name="logo">
            <area shape="rect" coords="0,0,120,120" href="about.aspx" alt="Go to About Us" />
        </map>
        <h1>UniBook</h1>
    </header>

    <nav>
        <a href="index.aspx">Home</a>
        <a href="login.aspx">Login</a>
        <a href="signup.aspx">Sign Up</a>
        <a href="about.aspx">About Us</a>
        <a href="books.aspx">Manage Books</a>
        <asp:Literal ID="litUser" runat="server" /> <!-- welcome + logout when logged in (session) -->
    </nav>
    <main>
        <h2>Manage Books</h2>
        <form id="booksForm" runat="server">
            <section>
                <h3>Search Books</h3>
                <p>
                    <label>
                        <b>Book name or course code:</b>
                        <asp:TextBox ID="txtSearch" runat="server" size="40" MaxLength="100" />
                    </label>
                    <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" />
                    <asp:Button ID="btnShowAll" runat="server" Text="Show All" OnClick="btnShowAll_Click" />
                </p>
                <asp:GridView ID="gvBooks" runat="server" AutoGenerateColumns="false" EmptyDataText="No books found">
                    <Columns>
                        <asp:BoundField DataField="CourseCode" HeaderText="Course Code" />
                        <asp:BoundField DataField="BookName" HeaderText="Book Name" />
                        <asp:BoundField DataField="Faculty" HeaderText="Faculty" />
                        <asp:BoundField DataField="Version" HeaderText="Version" />
                        <asp:BoundField DataField="NewPrice" HeaderText="New Book Price (SR)" />
                        <asp:BoundField DataField="UsedPrice" HeaderText="Used Book Price (SR)" />
                    </Columns>
                </asp:GridView>
            </section>

            <section>
                <h3>Add / Update / Delete a Book</h3>
                <p>To update or delete a book, type its course code and press <b>Load</b> first.</p>
                <p>
                    <label>
                        <b>Course Code:</b>
                        <asp:TextBox ID="txtCode" runat="server" size="12" MaxLength="10" />
                    </label>
                    <asp:Button ID="btnLoad" runat="server" Text="Load" OnClick="btnLoad_Click" />
                </p>
                <p>
                    <label>
                        <b>Book Name:</b>
                        <asp:TextBox ID="txtName" runat="server" size="60" MaxLength="200" />
                    </label>
                </p>
                <p>
                    <label>
                        <b>Faculty:</b>
                        <asp:DropDownList ID="ddlFaculty" runat="server">
                            <asp:ListItem Value="computing">Faculty of Computing and Information Technology</asp:ListItem>
                            <asp:ListItem Value="engineering">Faculty of Engineering</asp:ListItem>
                            <asp:ListItem Value="business">Faculty of Business</asp:ListItem>
                        </asp:DropDownList>
                    </label>
                </p>
                <p>
                    <label>
                        <b>Version:</b>
                        <asp:TextBox ID="txtVersion" runat="server" size="5" MaxLength="5" />
                    </label>
                </p>
                <p>
                    <label>
                        <b>New Book Price (SR):</b>
                        <asp:TextBox ID="txtNewPrice" runat="server" size="6" MaxLength="5" />
                    </label>
                </p>
                <p>
                    <label>
                        <b>Used Book Price (SR):</b>
                        <asp:TextBox ID="txtUsedPrice" runat="server" size="6" MaxLength="5" />
                    </label>
                </p>
                <p>
                    <asp:Button ID="btnAdd" runat="server" Text="Add" OnClick="btnAdd_Click" />
                    <asp:Button ID="btnUpdate" runat="server" Text="Update" OnClick="btnUpdate_Click" />
                    <asp:Button ID="btnDelete" runat="server" Text="Delete" OnClick="btnDelete_Click" OnClientClick="return confirm('Delete this book?');" />
                </p>
                <p><asp:Label ID="lblMsg" runat="server" /></p>
            </section>
        </form>
    </main>

    <footer>

       <div style="font-size: 20px; text-align: right">
            
            <h3 class="title">About Us</h3>
            <a href="about.aspx"><img src="icoabo.png" alt="About icon" width="90" height="90" class="below"></a>
            <img src="handab.png" alt="Pointing hand" width="45" height="45" class="above">
            
            
        </div>
        <b class="z">Customer Service</b>
        <address>
            <a href="mailto:aalmeri0003@stu.kau.edu.sa">aalmeri0003@stu.kau.edu.sa</a><br />
            <a href="mailto:rbarnawi0015@stu.kau.edu.sa">rbarnawi0015@stu.kau.edu.sa</a>
        </address>
        <h6>&copy; 2026 UniBook - King Abdulaziz University. All rights reserved </h6>

    </footer>
</body>

</html>