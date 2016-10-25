# EasyAD
Umbraco backoffice authentication based on Active Directory groups.

With this plugin you'll be able to manage your backoffice users based on Active Directory groups.
The plugin adds a new Tree called 'AD Groups' to the 'Users' application, from there you can Add and Remove AD groups.

[![Build status](https://ci.appveyor.com/api/projects/status/qypeq2sbbfxtgtv9?svg=true)](https://ci.appveyor.com/project/ThumNet/easyad)

To use the plugin follow these steps:
- Add it using Nuget (not yet published)
- Update your web.config and replace the `owin:appStartup` appSetting value with:
```
<add key="owin:appStartup" value="ThumNet.EasyAD.Startup.UmbracoEasyADOwinStartup" />
```
