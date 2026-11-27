using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using UniBook.Data;
using UniBook.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString)
           .ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning)));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// user accounts (login / register). No email confirmation needed so users can log in right after sign up
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

// session: keep user data on the server for 5 minutes of no activity
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(300); // 5 mins
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SameSite = SameSiteMode.Strict;
});

var app = builder.Build();

// create / update the database when the site starts (same as running update-database)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();

    // first run only: put the 9 books from the home page into the Book table
    // so the Books page and the search are not empty
    if (!db.Book.Any())
    {
        db.Book.Add(new Book { courseCode = "CPIS-358", title = "Internet & World Wide Web, By Harvey M. Deitel, Paul J. Deitel, Abbey Deitel", faculty = "Computing", version = "v5", newPrice = 160, usedPrice = 95 });
        db.Book.Add(new Book { courseCode = "CPIS-363", title = "Hands-On Machine Learning with Scikit-Learn and TensorFlow", faculty = "Computing", version = "v1", newPrice = 200, usedPrice = 105 });
        db.Book.Add(new Book { courseCode = "CPIS-342", title = "Data Mining: Concepts and Techniques, Jiawei Han, Micheline Kamber and Jian Pei", faculty = "Computing", version = "v3", newPrice = 160, usedPrice = 75 });
        db.Book.Add(new Book { courseCode = "MENG-201", title = "Engineering Mechanics: Statics, R. C. Hibbeler", faculty = "Engineering", version = "v15", newPrice = 210, usedPrice = 125 });
        db.Book.Add(new Book { courseCode = "MENG-230", title = "Thermodynamics: An Engineering Approach, Cengel, Boles & Kanoglu", faculty = "Engineering", version = "v10", newPrice = 220, usedPrice = 165 });
        db.Book.Add(new Book { courseCode = "MATH-202", title = "Calculus: Early Transcendentals, James Stewart", faculty = "Engineering", version = "v9", newPrice = 170, usedPrice = 110 });
        db.Book.Add(new Book { courseCode = "ECON-101", title = "Principles of Economics, N. Gregory Mankiw", faculty = "Business", version = "v10", newPrice = 140, usedPrice = 90 });
        db.Book.Add(new Book { courseCode = "ACCT-201", title = "Financial Accounting, Weygandt, Kimmel & Kieso", faculty = "Business", version = "v12", newPrice = 130, usedPrice = 65 });
        db.Book.Add(new Book { courseCode = "MRKT-201", title = "Principles of Marketing", faculty = "Business", version = "v19", newPrice = 120, usedPrice = 65 });
        db.SaveChanges();
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// turn on the session
app.UseSession();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapRazorPages()
   .WithStaticAssets();

app.Run();
