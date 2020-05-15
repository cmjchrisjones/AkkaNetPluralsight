using Akka.Actor;
using Akka.DI.Core;
using Akka.DI.Ninject;
using Ninject;
using ReactiveStock.ActorModel.Actors;
using ReactiveStock.ExternalServices;
using System;

namespace ReactiveStock.ActorModel
{
    static class ActorSystemReferance
    {
        public static ActorSystem ActorSystem { get; private set; }

        static ActorSystemReferance()
        {
            CreateActorSystem();
        }

        private static void CreateActorSystem()
        {
            ActorSystem = ActorSystem.Create("ReactiveStockActorSystem");
            var container = new StandardKernel();
            container.Bind<IStockPriceServiceGateway>().To<RandomStockPriceServiceGateway>();
            container.Bind<StockPriceLookupActor>().ToSelf();

            IDependencyResolver resolver = new NinjectDependencyResolver(container, ActorSystem);
        }
    }
}
