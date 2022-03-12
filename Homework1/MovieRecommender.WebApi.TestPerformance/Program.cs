using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MovieRecommender.WebApi.TestPerformance;

var services = new ServiceCollection()
        .AddLogging(configure => configure.AddConsole())
        .AddTransient<ITestRunnerService, TestRunnerService>()
        .BuildServiceProvider();

var testRunner = services.GetRequiredService<ITestRunnerService>();
await testRunner.Run();
