using Microsoft.EntityFrameworkCore;
using TuneScore.Data;
using TuneScore.Helpers;
using TuneScore.Repositories;
using TuneScore.Repositories.Interfaces;
using TuneScore.Services;
using TuneScore.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<HelperPathProvider>();
builder.Services.AddScoped<IRepositoryAlbums, RepositoryAlbums>();
builder.Services.AddScoped<IAlbumImageService, AlbumImageService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserSaltRepository, UserSaltRepository>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<PasswordService>();



// string connectionString = builder.Configuration.GetConnectionString("TuneScoreDBHome");
string connectionStringMac = builder.Configuration.GetConnectionString("TuneScoreDBMac");
Console.WriteLine("Connection string being used:");
Console.WriteLine(connectionStringMac);
builder.Services.AddDbContext<TuneScoreContext>(options =>
    options.UseSqlServer(connectionStringMac));
Console.WriteLine("Connection string being used:");
Console.WriteLine(connectionStringMac);
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

app.UseSession();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
