# Autofac Smart Interceptors
Autofac Smart Interceptor leverages Autofac Dependency Injection to inject interceptors to registered interfaces. For example the LogInterceptor will output all method calls to the specified output (Console, Textfile, etc)
## Table of Contents
### [Installation](#installation)
### [Interceptors](#interceptors)
### [Usage](#usage)
## Installation
https://www.nuget.org/packages/Autofac.SmartInterceptors
## Interceptors
* **LogInterceptor**:
This interceptor takes a TextWriter in the contructor (Console.Out, File, etc) and ouputs all the method calls. 
Log Example: 
`[10:20:11] [INFO] MyClass.MyMethod(param1: "value", param2: "value)`
* **SmartInterceptor**
This interceptor takes a `beforeMethodCallAction` and `afterMethodCallAction` so you can have your own implementation before and after a method call.
## Usage
Attach the log interceptor to Autofac. In this case we will register the `LogInterceptor` with output to `Console.Out` so all the methods in the class `TestClass` will be logged to `Console.Out`.
```csharp
//create autofac container
var builder = new ContainerBuilder();

//register the class you want to intercept method calls
builder.RegisterType<TestClass>().As<ITestClass>().Intercept();

//attach interceptors
builder.AttachInterceptorsToRegistrations(new LogInterceptor(Console.Out));

//build container
var container = builder.Build();
```
Check the Autofac.SmartInterceptor.Tests for more examples on how to use it.
