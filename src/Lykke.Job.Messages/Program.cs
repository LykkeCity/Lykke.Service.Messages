﻿using System;
using System.IO;
using System.Runtime.Loader;
using System.Threading;
using System.Threading.Tasks;
using Lykke.Job.Messages.QueueConsumers;
using Lykke.Job.SMS;
using Lykke.JobTriggers.Triggers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Lykke.Job.Messages
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine($"SMS version {Microsoft.Extensions.PlatformAbstractions.PlatformServices.Default.Application.ApplicationVersion}");
#if DEBUG
            Console.WriteLine("Is DEBUG");
#else
            Console.WriteLine("Is RELEASE");
#endif
            var webHostCancellationTokenSource = new CancellationTokenSource();
            IWebHost webHost;
            TriggerHost triggerHost = null;
            Task webHostTask = null;
            Task triggerHostTask = null;
            var end = new ManualResetEvent(false);

            try
            {
                AssemblyLoadContext.Default.Unloading += ctx =>
                {
                    Console.WriteLine("SIGTERM recieved");

                    webHostCancellationTokenSource.Cancel();

                    end.WaitOne();
                };

                webHost = new WebHostBuilder()
                    .UseKestrel()
                    .UseUrls("http://*:5000")
                    .UseContentRoot(Directory.GetCurrentDirectory())
                    .UseStartup<Startup>()
                    .UseApplicationInsights()
                    .Build();

                webHost.Services.GetService<SmsQueueConsumer>().Start();

                triggerHost = new TriggerHost(webHost.Services);

                webHostTask = webHost.RunAsync(webHostCancellationTokenSource.Token);
                webHostTask.Wait();
                //triggerHostTask = triggerHost.Start();

                // WhenAny to handle any task termination with exception, 
                // or gracefully termination of webHostTask
                //Task.WhenAny(webHostTask, triggerHostTask).Wait();
            }
            finally
            {
                Console.WriteLine("Terminating...");

                webHostCancellationTokenSource.Cancel();
                triggerHost?.Cancel();

                webHostTask?.Wait();
                triggerHostTask?.Wait();

                end.Set();
            }
           

            webHost.Services.GetService<SmsQueueConsumer>().Start();

            webHost.Run();

            Console.WriteLine("Terminated");
        }
    }
}