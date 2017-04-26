# Autofac.SmartInterceptors
## 1. Description
Autofac Smart Interceptor leverages Autofac Dependency Injection to inject interceptors to registered interfaces. For example the LogInterceptor will output all method calls to the specified output (Console, Textfile, etc)
## 2. Interceptors
* **LogInterceptor**:
This interceptor takes a TextWriter in the contructor (Console.Out, File, etc) and ouputs all the method calls. 
Log Example: 
`[10:20:11] [INFO] MyClass.MyMethod(param1: "value", param2: "value)`
* **SmartInterceptor**
This interceptor takes a `beforeMethodCallAction` and `afterMethodCallAction` so you can have your own implementation before and after a method call.
