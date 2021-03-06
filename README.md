# Baffi

[![Build status](https://ci.appveyor.com/api/projects/status/a078txulxuq5pc5l?svg=true)](https://ci.appveyor.com/project/viktornilsson91/baffi)

[![NuGet](https://img.shields.io/nuget/v/baffi.svg)](https://www.nuget.org/packages/baffi/)

![baffi-logo](baffi-logo.png)

# Install

The latest versions of the packages are available on NuGet. To install, run the following command.
```
PM> Install-Package Baffi
```

# Features

Helps you parse simple HTML templates with data binding and parse them to JSON objects.

For example you can edit the JSON in a simple JSON-editor and then save it in multiple languages.

When you have both the HTML template and the JSON object you can parse them together and get the translated HTML.

# Configuration

#### Custom tags
You can create custom tags that implements `ICustomTag` interface,
the `Process()` method runs when call `Parser.Compile()`.
```csharp
public class PriceTag : ICustomTag
{
    public string Process(string[] parameters)
    {
        var val1 = parameters.Length > 0 ? int.Parse(parameters[0]) : 0;
        var val2 = parameters.Length > 1 ? int.Parse(parameters[1]) : 0;

        return $"{val1 * val2}:-";
    }
}
```

To use this in the template, just use the name of the class.
```html
<span>{{PriceTag}}</span>
```

The value in the json object can be passed as parameters to the `Process()` method.

To pass multiple values we split the text value on the char `&`.

For example article numbers to fetch the price from the database.
```json
{
    "PriceTag": "123456&1234"
}
```

#### Initialize Baffi.Parser with custom tags.
If you want to use custom tags you need to configure them before you run `Parser.Compile()`.
For example in your `Global.asax` or `Startup.cs`.
```csharp
Parser.Initialize(cfg =>
{
    cfg.AddTag<PriceTag>();
});
```

# Usage

### We supports data-binding and for loops like this.
```html
<h1>{{Header}}</h1>
```
```html
// Can be nested as you want
{{for Products}}
<div class="product">
    <span>Name: {{Name}}</span>
    <span>Price: {{Price}}</span>
    <span>Size: {{Size}}</span>
</div>
{{end Products}}
```

#### First create the HTML template.
* [Example HTML template file](Baffi.Tests/Content/template.html)

#### Then take the template and extract the JSON object.
```csharp
// Get the template from your source
var text = System.IO.File.ReadAllText("template.html");

// Here we get the JSON object
var obj = Parser.Extract(text);
```

#### When you have the JSON object you can modify it as you want, for example like this.

* [Example JSON file](Baffi.Tests/Content/data.json)

#### Finally take the template and the object and parse them together.
```csharp
// Get the data from some source
var template = System.IO.File.ReadAllText("template.html");
var json = System.IO.File.ReadAllText("data.json");

// Convert to dynamic
dynamic obj = JsonConvert.DeserializeObject(json);

// Get the translated HTML
string html = Parser.Compile(template, obj);
```
