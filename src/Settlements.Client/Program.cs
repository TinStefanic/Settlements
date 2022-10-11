using FluentValidation;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Settlements.Client;
using Settlements.Client.Services;
using Settlements.Shared.Validators;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient<HostClient>();
builder.Services.AddValidatorsFromAssemblyContaining<SettlementDTOValidator>();

await builder.Build().RunAsync();
