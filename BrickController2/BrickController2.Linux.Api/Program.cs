using BrickController2.Linux.Api.Api;
using BrickController2.Linux.Api.PlatformServices.BluetoothLE;
using BrickController2.Linux.Api.Services;
using BrickController2.PlatformServices.BluetoothLE;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSystemd();

builder.WebHost.UseUrls(builder.Configuration.GetValue("Urls", "http://127.0.0.1:5080"));

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
});

builder.Services.AddSingleton<IBluetoothLEService, BlueZBluetoothLEService>();
builder.Services.AddSingleton<BluetoothDeviceSessionRegistry>();

var app = builder.Build();

app.MapGet("/", () => Results.Ok(new
{
    Service = "BrickController2 Linux API",
    Status = "/api/bluetooth/status"
}));
app.MapBluetoothApi();

app.Run();
