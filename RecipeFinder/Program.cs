using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(o =>
{
    o.DefaultScheme = "Application";
    o.DefaultSignInScheme = "External";
    
})
            .AddCookie("Application")
            .AddCookie("External")
            .AddGoogle(o =>
            {
                o.ClientId = "209254984697-brd11gguh1l2l434n0mqa31p3jtua124.apps.googleusercontent.com";
                o.ClientSecret = "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";
                o.Events.OnRedirectToAuthorizationEndpoint = context =>
                {
                    context.Response.Redirect(context.RedirectUri + "&prompt=consent");
                    return Task.CompletedTask;
                };
            });


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

app.UseAuthentication();


app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
