using Akka.Actor;
using System;

namespace ReactiveStock.ActorModel.Messages
{
    class UnSubscribeFromNewStockPriceMessage
    {
        public IActorRef Subscriber { get; private set; }

        public UnSubscribeFromNewStockPriceMessage(IActorRef unsubscribingActor)
        {
            Subscriber = unsubscribingActor;
        }
    }
}
