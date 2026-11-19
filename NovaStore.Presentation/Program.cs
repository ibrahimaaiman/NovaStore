using NovaStore.BusinessLogic.Services;
using NovaStore.DataAccess.Repositories;
using Microsoft.AspNetCore.DataProtection;
var builder=WebApplication.CreateBuilder(args);
builder.Services.AddDataProtection().PersistKeysToFileSystem(new DirectoryInfo(Path.Combine(builder.Environment.ContentRootPath,"App_Data","keys")));
builder.Services.AddControllersWithViews();builder.Services.AddDistributedMemoryCache();builder.Services.AddSession(o=>{o.IdleTimeout=TimeSpan.FromDays(7);o.Cookie.HttpOnly=true;o.Cookie.IsEssential=true;});builder.Services.AddSingleton<JsonStoreRepository>();builder.Services.AddSingleton<IStoreRepository>(sp=>sp.GetRequiredService<JsonStoreRepository>());builder.Services.AddSingleton<IStoreService,StoreService>();
var app=builder.Build();if(!app.Environment.IsDevelopment())app.UseExceptionHandler("/Home/Error");app.UseStaticFiles();app.UseRouting();app.UseSession();app.MapControllerRoute(name:"default",pattern:"{controller=Home}/{action=Index}/{id?}");app.Run();
