# Sandwych.SmartConfig

Sandwych.SmartConfig is a pure C# implementation of various WiFi SmartConfig protocols that build from scratch.

TD;LR: If you working on a Xamarin mobile app to deal with WiFi-capability IoT devices, you may need this library.

English | [简体中文](README.zh_cn.md)

## Getting Started

## Features

* A .NET Standard class library, works on both Xamarin and desktop.
* No third-party library referenced.
* Supported protocols: WeChat's AirKiss and Espressif's ESPTouch.
* Clean architecture, easy to learn and add your own protocol.
* IoC container friendly.

## Getting Started

### Prerequisites

* Microsoft Visual Studio 2019 
* DocFX for API documents generation (Optional)

### Supported Platforms

* .NET Standard 2.0+

### Installation


## Examples

### Usage

```csharp

var provider = new EspSmartConfigProvider();
var ctx = provider.CreateContext();

ctx.DeviceDiscoveredEvent += (s, e) => {
	Console.WriteLine("Found device: IP={0}    MAC={1}", e.Device.IPAddress, e.Device.MacAddress);
};

var scArgs = new SmartConfigArguments()
{
	Ssid = "YourWiFiSSID",
	Bssid = PhysicalAddress.Parse("10-10-10-10-10-10"),
	Password = "YourWiFiPassword",
	LocalAddress = IPAddress.Parse("192.168.1.10")
};

// Do the SmartConfig job
using (var job = new SmartConfigJob(TimeSpan.FromSeconds(20))) // Set the time out to 20 seconds
{
	await job.ExecuteAsync(ctx, scArgs);
}

```

### The Demo Android App

APK Download: WIP

## Donation

If this project is useful to you, you can buy me a beer:

[![Support via PayPal.me](https://github.com/oldrev/sandwych-smartconfig/blob/master/assets/paypal_button.svg)](https://www.paypal.me/oldrev)

## Contributiors

* **Li "oldrev" Wei** - *Init work and the main maintainer* - [oldrev](https://github.com/oldrev)

## License

Copyright &copy; Sandwych.SmartConfig Contributors.

[LICENSE.md](LICENSE.md)。

## Credits

* Espressif EsptouchForAndroid: https://github.com/EspressifApp/EsptouchForAndroid
