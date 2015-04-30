# Yandex.Money API SDK Generic .NET application sample

## Requirements

The sample application requires:
* .NET 4.5 or later
* [LINQpad](http://www.linqpad.net/)
* [NuGet package manager](https://www.nuget.org/)

## Getting started

The simplest way to try the sample application is using popular lightweight utility [LINQpad](http://www.linqpad.net/).

To run sample you have to do the following steps:
1. Install  [LINQpad](http://www.linqpad.net/)
2. Download [a command-line NuGet utility](https://nuget.org/nuget.exe) to project directory, for example "C:\sample"
3. Install .NET Yandex.Money API SDK by command "nuget install Yandex.Money.Api.Sdk" in project directory, for example "C:\sample"
4. Open api.sdk.sample.linq file by LINQpad
5. Choose "Language - C# Program"
6. To be able to use the sample you should register your application and get your unique *client id*. To register an application please follow the steps described on [this page](http://tech.yandex.com/money/doc/dg/tasks/register-client.xml) (also available in [Russian](http://tech.yandex.ru/money/doc/dg/tasks/register-client.xml)).
7. Write down ClientId and RedirectUri values into the script
8. Run the sample and enjoy!

#### Keep in mind
1. Sample uses WebBrowser control which uses Microsoft Internet Explorer settings.
2. To use sample, the "TLS" protocol support must be enabled in MSIE settings (enabled by default).
3. You you see warnings, check "Browser / Disable script debugging (Internet Explorer)" option in browser settings.
