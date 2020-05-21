# Helper.Swagger

Is a .net core template to implement swagger ui , with configuration options.

**Implementation in Startup****

```c#
        services.ConfigureSwagger(new SwaggerOptions
        {
            Title ="API",
            Version = "1",
            Description = "API used for.",
        });
```
```c#
            app.ConfigureSwagger(new SwaggerUISettings
            {
                EndPointName = "My API V1",
                EndPointUrl = "/swagger/v1/swagger.json",
                StylePath = $"/swagger/ui/custom.css"
            });
```