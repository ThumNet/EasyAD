# EasyAD
Umbraco backoffice authentication based on Active Directory groups.

With this plugin you'll be able to manage your backoffice users based on Active Directory groups.
The plugin adds a new Tree called 'AD Groups' to the 'Users' application, from there you can Add and Remove AD groups.

![Build status](https://ci.appveyor.com/api/projects/status/github/thumnet/easyad?branch=master&svg=true)

To use the plugin follow these steps:
1. Add it using Nuget (not yet published)
2. Update your web.config and replace the `owin:appStartup` appSetting value with:
```
<add key="owin:appStartup" value="ThumNet.EasyAD.Startup.UmbracoEasyADOwinStartup" />
```
