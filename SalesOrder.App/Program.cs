using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Radzen;
using SalesOrder.App;
using SalesOrder.App.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped<DialogService>();
builder.Services.AddScoped<TooltipService>();

var baseAddress = new Uri(builder.Configuration.GetValue<string>("ApiBaseAddress"));
builder.Services.AddHttpClient<IOrderService, OrderService>(client => client.BaseAddress = baseAddress);
builder.Services.AddHttpClient<IWindowService, WindowService>(client => client.BaseAddress = baseAddress);
builder.Services.AddHttpClient<ISubElementService, SubElementService>(client => client.BaseAddress = baseAddress);

await builder.Build().RunAsync();