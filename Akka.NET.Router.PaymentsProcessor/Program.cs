using Akka.Actor;
using Akka.Configuration;
using Akka.DI.AutoFac;
using Akka.DI.Core;
using Akka.NET.Router.PaymentsProcessor.Actors;
using Akka.NET.Router.PaymentsProcessor.ExternalSystems;
using Akka.NET.Router.PaymentsProcessor.Message;
using Autofac;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Akka.NET.Router.PaymentsProcessor
{
    class Program
    {
        // Field to hold the actor system
        private static ActorSystem ActorSystem;

        static void Main(string[] args)
        {
            // Create the Actor System
            CreateActorSystem();
            Console.WriteLine("Hello World!");

            // Top Level Actor
            IActorRef jobCoordinator = ActorSystem.ActorOf<JobCoordinatorActor>("JobCoordinator");
            PeakTimeDemoSimulator.StartDemo(stayPeakTimeForSeconds: 6);

            #region Group Router
            // Create 3 instances of payment worker actors - using DI
            //ActorSystem.ActorOf(ActorSystem.DI().Props<PaymentWorkerActor>(), "PaymentWorker1");
            //ActorSystem.ActorOf(ActorSystem.DI().Props<PaymentWorkerActor>(), "PaymentWorker2");
            //ActorSystem.ActorOf(ActorSystem.DI().Props<PaymentWorkerActor>(), "PaymentWorker3");
            #endregion


            var jobTime = Stopwatch.StartNew();
            jobCoordinator.Tell(new ProcessFileMessage("payments.csv"));


            ActorSystem.WhenTerminated.Wait();
            jobTime.Stop();

            Console.WriteLine($"Job completed in {jobTime.ElapsedMilliseconds} milliseconds");
            Console.ReadLine();
        }

        private static void CreateActorSystem()
        {
            // Start building up the DI (using Autofac)
            var builder = new ContainerBuilder();
            builder.RegisterType<DemoPaymentGateway>().As<IPaymentGateway>();
            builder.RegisterType<PaymentWorkerActor>();
            var container = builder.Build();

            // Create the actor and assign to our field
            ActorSystem = ActorSystem.Create("PaymentProcessing", GetConfigFromHoconFile());

            var resolver = new AutoFacDependencyResolver(container, ActorSystem);
        }

        private static Config GetConfigFromHoconFile()
        {
            var hoconConfigFile = XElement.Parse(File.ReadAllText(Directory.GetCurrentDirectory() + "\\akka-hocon.config"));
            var config = ConfigurationFactory.ParseString(hoconConfigFile.Descendants("hocon").Single().Value);
            return config;
        }
    }
}
