var bldr = WebApplication.CreateBuilder(args);

bldr.Services.AddControllersWithViews();

var wapp = bldr.Build();

wapp.MapDefaultControllerRoute();

wapp.Run();
