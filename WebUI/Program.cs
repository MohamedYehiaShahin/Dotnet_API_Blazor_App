using Application.DI;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using NetcodeHub.Packages.Components;
using NetcodeHub.Packages.Components.Toast;
using WebUI;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddApplicationService();
builder.Services.AddScoped<ToastService>();
builder.Services.AddVirtualizationService(); 

await builder.Build().RunAsync();
