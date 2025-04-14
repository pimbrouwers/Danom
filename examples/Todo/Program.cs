using Danom.Examples.Todo;
using Danom.Examples.Todo.Infrastructure;
using Leger;

var bldr = WebApplication.CreateBuilder(args);

bldr.Services
    .AddScoped<IDbConnectionFactory>(_ =>
        bldr.Configuration.GetConnectionString("default") is string connectionString ?
            new TodoConnectionFactory(connectionString) :
            throw new ArgumentNullException("Connection string is missing"))
    .AddScoped<TodoStore>()
    .AddControllersWithViews()
    .AddRazorOptions(o =>
    {
        // Enable the use of the Features folder
        o.ViewLocationFormats.Add("/Features/Shared/{0}.cshtml");
        o.ViewLocationFormats.Add("/Features/{1}/{0}.cshtml");
    });

var wapp = bldr.Build();

wapp.MapDefaultControllerRoute();

wapp.Run();
