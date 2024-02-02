using DyersCargoTransit_Interface.Controllers;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();



//added
builder.Services.AddDistributedMemoryCache();

//added
builder.Services.AddSession();
builder.Services.AddMvc().AddSessionStateTempDataProvider();



//login controller
builder.Services.AddHttpClient("LoginAPI", client =>
{
    client.BaseAddress = new Uri("https://localhost:7005/api/AuthAPI");
});




var app = builder.Build();








// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


//add
app.UseSession();


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
