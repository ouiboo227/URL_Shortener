using Microsoft.EntityFrameworkCore;
using shortenerURL_vibing.Data;
using shortenerURL_vibing.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add DbContext
builder.Services.AddDbContext<UrlDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add URL Service
builder.Services.AddScoped<IUrlService, UrlService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Add route for URL redirection
app.MapControllerRoute(
    name: "urlRedirect",
    pattern: "{shortCode}",
    defaults: new { controller = "Url", action = "RedirectToUrl" });

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<UrlDbContext>();
    db.Database.Migrate();
}


app.Run();
