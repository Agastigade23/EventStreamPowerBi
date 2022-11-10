using EventStreamPowerBi;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
        services.AddSingleton<IEvents, Events>();
    })
    .Build();

await host.StartAsync();

await host.WaitForShutdownAsync();
