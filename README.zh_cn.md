[![构建状态](https://ci.appveyor.com/api/projects/status/TODO?svg=true)](https://ci.appveyor.com/project/oldrev/sandwych-smartconfig)
[![NuGet](https://img.shields.io/nuget/v/Sandwych.SmartConfig.svg)](https://www.nuget.org/packages/Sandwych.SmartConfig)

# Sandwych.SmartConfig

一个从零开始实现的微信 AirKiss 和乐鑫 ESPTouch 的 WiFi SmartConfig 配网功能库。

[English](README.md) | 简体中文


## 特性

* 使用纯 C# 和 .NET BCL 的 `UDPClient` 类实现，不依赖设备厂家或其他的第三方包，可同时用于 Xamarin 手机 App 或桌面；
* 目前支持的协议：AirKiss、ESPTouch；
* 配网速度快，兼容性好；
* 设计清晰可扩展，可自行增加其他协议，也可参考本项目编写其他语言的 AirKiss 和 ESPTouch 实现；
* IoC 容器友好；
* 纯异步设计

一般来说，如果你想用 .NET 的 Xamarin 开发涉及 WiFi 物联网设备的手机 App，那 Sandwych.SmartConfig 是你必须会用到的。

## 快速上手


### 前置需求

* Microsoft Visual Studio 2019（免费社区版即可）
* DocFX 用于生成 API 文档（可选）

### 支持平台

* .NET Standard 2.0+

### 安装

把 Sandwych.SmartConfig 通过 **[NuGet](https://www.nuget.org/packages/Sandwych.SmartConfig)** 安装到你的项目中即可使用。

## 例子

### 简单调用本库进行配网

以 ESPTouch 协议为例：

```csharp

var provider = new EspSmartConfigProvider();
var ctx = provider.CreateContext();

// 设备通过 UDP 广播回报 IP 以后引发此事件，注意同一个设备 IP 只会触发一次
ctx.DeviceDiscoveredEvent += (s, e) => {
	// 这里如果涉及更新 GUI 的话需要同步到主线程
	Console.WriteLine("Found device: IP={0}    MAC={1}", e.Device.IPAddress, e.Device.MacAddress);
};

// 设置配网参数
var scArgs = new SmartConfigArguments()
{
	Ssid = "YourWiFiSSID",
	Bssid = PhysicalAddress.Parse("10-10-10-10-10-10"),
	Password = "YourWiFiPassword"
	LocalAddress = IPAddress.Parse("192.168.1.10")
};

// 调用 SmartConfigJob 进行实际的配网
using (var job = new SmartConfigJob(TimeSpan.FromSeconds(20))) // 设置最长配网时间 20秒
{
	await job.ExecuteAsync(ctx, scArgs);
}

```

### 使用例子程序

本项目包括了一个通用的 Xamarin.Android 配网程序作为例子，可自行编译运行，也可直接下载编译好的 `.APK` 安装测试：

APK 下载地址： TODO

## 支持本项目

假如本项目对你有用，可以考虑请我喝杯啤酒:

微信打赏二维码：

![微信](https://github.com/oldrev/sandwych-smartconfig/blob/master/assets/wechat_qrcode.png)


* BTW 如果你需要在本项目贡献者处留名，打赏时请备注。

当然，也非常欢迎你测试提交 bug、贡献代码、帮其他用户解决问题、编写文档，这些都是金钱不能衡量的。

## 贡献者

* **李维** - *初始开发及现维护者* - [oldrev](https://github.com/oldrev)

## 授权协议

版权所有 &copy; Sandwych.SmartConfig 贡献者。

本项目使用 MIT 协议授权，详情见： [LICENSE.md](LICENSE.md)。

## 致谢

* 乐鑫 EsptouchForAndroid: https://github.com/EspressifApp/EsptouchForAndroid
