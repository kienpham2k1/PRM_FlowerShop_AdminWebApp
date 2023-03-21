using BusinessObject;
using BusinessObject.IService;
using DataAccess.Models;
using Firebase.Storage;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddScoped<FlowerShopContext, FlowerShopContext>(); 
builder.Services.AddScoped<IFireBaseStorage, FireBaseStorage>(); 
builder.Services.AddScoped<IUploadFileService, LocalFileUploadService>();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(cfg => { 
    cfg.Cookie.Name = "xuanthulab"; 
    cfg.IdleTimeout = new TimeSpan(0, 60, 0); 
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();
app.UseSession();

app.Run();
