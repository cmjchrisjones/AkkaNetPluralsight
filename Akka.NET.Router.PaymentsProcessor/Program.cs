using Akka.Actor;
using Akka.DI.AutoFac;
using Akka.NET.Router.PaymentsProcessor.Actors;
using Akka.NET.Router.PaymentsProcessor.ExternalSystems;
using Akka.NET.Router.PaymentsProcessor.Message;
using Autofac;
using System;
using System.Diagnostics;

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
            ActorSystem = ActorSystem.Create("PaymentProcessing");

            var resolver = new AutoFacDependencyResolver(container, ActorSystem);
        }
    }
}
