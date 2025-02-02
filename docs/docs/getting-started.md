# Getting Started

Welcome to **TemplateMAUI**! This guide will help you get started with integrating the TemplateMAUI library into your .NET MAUI application. Follow the steps below to install the NuGet package and start using the custom templated controls provided by TemplateMAUI.

### Installation

**Step 1: Add the NuGet Package**
To add the TemplateMAUI NuGet package to your project, follow these steps:

Using **Visual Studio**:
1. Right-click on your MAUI project in the Solution Explorer.
2. Select Manage NuGet Packages.
3. Go to the Browse tab, search for TemplateMAUI, and click Install.

Using **.NET CLI**:

1. Open a terminal or command prompt.
2. Navigate to your project directory.
3. Run the following command:

```
dotnet add package TemplateMAUI
```

**Step 2: Initialize the library**

After adding the NuGet package, initialize the library in the **App** class:

```
TemplateMAUI.Init();
``` 

**Step 3: Add the Namespace**

After installing the package, you need to add the TemplateMAUI namespace to your XAML or C# files.

**XAML**

Add the following namespace declaration to the top of your XAML file:

```
xmlns:template="http://schemas.microsoft.com/dotnet/2021/maui/templatemaui"
```

**C#**

Add the following using directive to the top of your C# file:

```
using TemplateMAUI;
```


### Usage

Now that you have installed the package and added the necessary namespaces, you can start using the custom templated controls in your MAUI application.

**Example 1: Using Divider in XAML**

Here’s an example of how to use the Divider control in XAML:

```
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:template="clr-namespace:TemplateMAUI;assembly=TemplateMAUI"
             x:Class="YourNamespace.MainPage">

    <StackLayout>
        <Label Text="Section 1" />
        <template:Divider Color="Gray" Thickness="1" Orientation="Horizontal" />
        <Label Text="Section 2" />
    </StackLayout>
</ContentPage>
```

**Example 2: Using Divider in C#**

Here’s an example of how to use the Divider control in C#:

```
using TemplateMAUI;

var stackLayout = new StackLayout();

var section1Label = new Label { Text = "Section 1" };
stackLayout.Children.Add(section1Label);

var divider = new Divider
{
    Color = Colors.Gray,
    Thickness = 1,
    Orientation = DividerOrientation.Horizontal
};
stackLayout.Children.Add(divider);

var section2Label = new Label { Text = "Section 2" };
stackLayout.Children.Add(section2Label);

Content = stackLayout;
```
