using NovaStore.Services;
var builder=WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();builder.Services.AddDistributedMemoryCache();builder.Services.AddSession(o=>{o.IdleTimeout=TimeSpan.FromDays(7);o.Cookie.HttpOnly=true;o.Cookie.IsEssential=true;});builder.Services.AddSingleton<StoreService>();
var app=builder.Build();if(!app.Environment.IsDevelopment())app.UseExceptionHandler("/Home/Error");app.UseStaticFiles();app.UseRouting();app.UseSession();app.MapControllerRoute(name:"default",pattern:"{controller=Home}/{action=Index}/{id?}");app.Run();