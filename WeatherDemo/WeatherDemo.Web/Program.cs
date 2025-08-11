
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSingleton<WeatherDemo.Library.IRandomProvider, WeatherDemo.Library.RandomProvider>();
builder.Services.AddTransient<WeatherDemo.Library.WeatherService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();


app.MapGet("/", context => {
    context.Response.Redirect("/Weather");
    return System.Threading.Tasks.Task.CompletedTask;
});
app.UseStaticFiles();
app.MapRazorPages();

app.Run();
