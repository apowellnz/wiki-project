using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.EntityFrameworkCore;
using AjpWiki.Infrastructure.Data;
using AjpWiki.Web;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// add to your host Program.cs or Startup
builder.Services.AddDbContext<WikiDbContext>(options =>
    options.UseInMemoryDatabase("WikiInMemory"));

await builder.Build().RunAsync();
