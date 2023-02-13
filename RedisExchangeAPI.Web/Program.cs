using RedisExchangeAPI.Web.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<RedisService>();
// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();
var redisService = new RedisService(builder.Configuration);
redisService.Connect();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

