
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);
    // … configuración …
    var app = builder.Build();
    // … middleware y endpoints …
    app.Run();

    // Esto debe ser público:
    public partial class Program { }

