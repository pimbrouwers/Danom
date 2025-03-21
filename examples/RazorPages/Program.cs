var bldr = WebApplication.CreateBuilder(args);

bldr.Services
    .AddRazorPages();

var wapp = bldr.Build();

wapp.MapRazorPages();

wapp.Run();
