using Microsoft.EntityFrameworkCore;
using PolicyPro360.Models;
using PolicyPro360.Services;
using PolicyPro360.Hubs;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    Args = args,
    EnvironmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"
});


builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

// Add services to the container.


builder.Services.AddAuthorization();
builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<myContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddSession();
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("Smtp"));
builder.Services.AddSingleton<IEmailService, EmailService>();
builder.Services.AddHttpClient();
builder.Services.AddSingleton<OpenAiService>();
// Program.cs or Startup.cs
builder.Services.AddHttpClient<PolicyPro360.Services.IOpenAiService, PolicyPro360.Services.OpenAiService>();
builder.Services.AddScoped<PolicyPro360.Services.IQuizService, PolicyPro360.Services.QuizService>();
builder.Services.AddAntiforgery();
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient(); // <-- registers IHttpClientFactory
builder.Services.AddScoped<IOpenAiService, OpenAiService>();
builder.Services.AddScoped<IQuizService, QuizService>();
builder.Services.AddSignalR();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();

app.UseAuthorization();
app.MapRazorPages();
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=UserHome}/{action=Index}/{id?}");
app.MapHub<PolicyPro360.Hubs.ChatHub>("/hubs/chat");

app.Run();
