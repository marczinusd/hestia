module Hestia.WebService.Giraffe

open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.DependencyInjection
open Microsoft.OpenApi.Models
open Giraffe

let webApp =
    choose
        [ route "/ping" >=> text "pong"
          route "/" >=> htmlFile "/pages/index.html" ]

type Startup() =

    member _.ConfigureServices(services: IServiceCollection) =
        services.AddGiraffe() |> ignore
        services.AddSwaggerGen(fun c -> c.SwaggerDoc("v1", OpenApiInfo(Title = "Hestia API", Version = "v1"))) |> ignore

    member _.Configure (app: IApplicationBuilder) (env: IWebHostEnvironment) =
        app.UseSwagger |> ignore
        app.UseSwaggerUI(fun c ->
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Hestia API v1")
            c.RoutePrefix = "" |> ignore)
        |> ignore

        if env.IsDevelopment() then app.UseDeveloperExceptionPage |> ignore
        app.UseGiraffe webApp

[<EntryPoint>]
let main _ =
    Host.CreateDefaultBuilder()
        .ConfigureWebHostDefaults(fun webHostBuilder -> webHostBuilder.UseStartup<Startup>() |> ignore).Build().Run()
    0
