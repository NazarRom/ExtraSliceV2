using ExtraSliceV2.Data;
using ExtraSliceV2.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddDistributedMemoryCache();
builder.Services.AddMemoryCache();
builder.Services.AddResponseCaching();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
});

string connectionString = builder.Configuration.GetConnectionString("SqlExtra");
//el repository
builder.Services.AddTransient<RepositoryUsers>();
//el context
builder.Services.AddDbContext<UsersContext>
    (options => options.UseSqlServer(connectionString));

builder.Services.AddControllersWithViews();
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
app.UseResponseCaching();
app.UseSession();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Carta}/{action=Index}/{id?}");

app.Run();
