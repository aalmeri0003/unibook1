using System;
using System.Data.SqlClient;
using System.IO;

// Creates the UniBook database the first time the site runs
// (App_Data/UniBook.mdf with the Users and Books tables + the books from the home page)
public static class DbSetup
{
    public static void CreateIfMissing(string dataFolder)
    {
        string mdf = Path.Combine(dataFolder, "UniBook.mdf");
        string ldf = Path.Combine(dataFolder, "UniBook_log.ldf");

        // database already there, nothing to do
        if (File.Exists(mdf))
        {
            return;
        }

        Directory.CreateDirectory(dataFolder);
        if (File.Exists(ldf))
        {
            File.Delete(ldf); // old log file without its database
        }

        using (SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;Integrated Security=True"))
        {
            con.Open();

            // remove an old copy with the same name (if any), then create the database files in App_Data
            Run(con, "IF DB_ID('UniBookSetup') IS NOT NULL DROP DATABASE UniBookSetup");
            Run(con, "CREATE DATABASE UniBookSetup ON (NAME = UniBook, FILENAME = '" + mdf.Replace("'", "''") + "') " +
                     "LOG ON (NAME = UniBook_log, FILENAME = '" + ldf.Replace("'", "''") + "')");

            Run(con, "USE UniBookSetup");

            // users who sign up
            Run(con, "CREATE TABLE Users (" +
                     " UserID INT IDENTITY(1,1) PRIMARY KEY," +
                     " FirstName NVARCHAR(25) NOT NULL," +
                     " LastName NVARCHAR(25) NOT NULL," +
                     " Username NVARCHAR(25) NOT NULL UNIQUE," +
                     " Email NVARCHAR(50) NOT NULL," +
                     " Password NVARCHAR(25) NOT NULL," +
                     " Faculty NVARCHAR(20) NOT NULL)");

            // books we sell
            Run(con, "CREATE TABLE Books (" +
                     " BookID INT IDENTITY(1,1) PRIMARY KEY," +
                     " CourseCode NVARCHAR(10) NOT NULL UNIQUE," +
                     " BookName NVARCHAR(200) NOT NULL," +
                     " Faculty NVARCHAR(20) NOT NULL," +
                     " Version NVARCHAR(5) NOT NULL," +
                     " NewPrice INT NOT NULL," +
                     " UsedPrice INT NOT NULL)");

            // same books that are on the home page
            Run(con, "INSERT INTO Books (CourseCode, BookName, Faculty, Version, NewPrice, UsedPrice) VALUES " +
                     "(N'CPIS-358', N'Internet & World Wide Web, By Harvey M. Deitel, Paul J. Deitel, Abbey Deitel', N'computing', N'v5', 160, 95)," +
                     "(N'CPIS-363', N'Hands-On Machine Learning with Scikit-Learn and TensorFlow', N'computing', N'v1', 200, 105)," +
                     "(N'CPIS-342', N'Data Mining: Concepts and Techniques, Jiawei Han, Micheline Kamber and Jian Pei', N'computing', N'v3', 160, 75)," +
                     "(N'MENG-201', N'Engineering Mechanics: Statics, R. C. Hibbeler', N'engineering', N'v15', 210, 125)," +
                     "(N'MENG-230', N'Thermodynamics: An Engineering Approach, Cengel, Boles & Kanoglu', N'engineering', N'v10', 220, 165)," +
                     "(N'MATH-202', N'Calculus: Early Transcendentals, James Stewart', N'engineering', N'v9', 170, 110)," +
                     "(N'ECON-101', N'Principles of Economics, N. Gregory Mankiw', N'business', N'v10', 140, 90)," +
                     "(N'ACCT-201', N'Financial Accounting, Weygandt, Kimmel & Kieso', N'business', N'v12', 130, 65)," +
                     "(N'MRKT-201', N'Principles of Marketing', N'business', N'v19', 120, 65)");

            // detach it so the site can open the .mdf file from App_Data
            Run(con, "USE master");
            Run(con, "EXEC sp_detach_db 'UniBookSetup'");
        }
    }

    private static void Run(SqlConnection con, string sql)
    {
        SqlCommand cmd = new SqlCommand(sql, con);
        cmd.ExecuteNonQuery();
    }
}
