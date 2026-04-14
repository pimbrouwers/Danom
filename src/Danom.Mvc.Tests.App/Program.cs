var bldr = WebApplication.CreateBuilder(args);

bldr.Services
    .AddControllersWithViews()
    .AddRazorOptions(o => {
        // Enable the use of the Features folder
        o.ViewLocationFormats.Add("/Features/Shared/{0}.cshtml");
        o.ViewLocationFormats.Add("/Features/{1}/{0}.cshtml");
    }); ;

var wapp = bldr.Build();

wapp.MapDefaultControllerRoute();

wapp.Run();

public partial class Program { }
