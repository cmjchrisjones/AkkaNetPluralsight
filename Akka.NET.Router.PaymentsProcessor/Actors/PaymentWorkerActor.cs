using Akka.Actor;
using Akka.NET.Router.PaymentsProcessor.ExternalSystems;
using Akka.NET.Router.PaymentsProcessor.Message;
using System;
using System.Collections.Generic;
using System.Text;

namespace Akka.NET.Router.PaymentsProcessor.Actors
{
    class PaymentWorkerActor : ReceiveActor, IWithUnboundedStash
    {
        private readonly IPaymentGateway _paymentGateway;

        public IStash Stash { get; set; }

        private ICancelable _unstashSchedule;

        public PaymentWorkerActor(IPaymentGateway paymentGateway)
        {
            _paymentGateway = paymentGateway;
            Receive<SendPaymentMessage>(m => SendPayment(m));
            Receive<ProcessStashedPaymentsMessage>(m => HandleUnstash());

        }

        private void SendPayment(SendPaymentMessage message)
        {
            if (message.Amount > 100 && PeakTimeDemoSimulator.IsPeakHours)
            {
                Console.WriteLine($"Stashing message for {message.FirstName} {message.LastName}");
                Stash.Stash();
            }
            else
            {
                Console.WriteLine($"Sending payment for {message.FirstName} {message.LastName}");
                _paymentGateway.Pay(message.AccountNumber, message.Amount);
                Console.WriteLine($"Sender: {Sender.Path}");
                Sender.Tell(new PaymentSentMessage(message.AccountNumber));
            }
        }

        private void HandleUnstash()
        {
            Console.WriteLine("Received Handle process message");
            if(!PeakTimeDemoSimulator.IsPeakHours)
            {
                Console.WriteLine("Not in peak hours so unstashing");
                Stash.UnstashAll();
            }
        }

        // Create a schedule 
        protected override void PreStart()
        {
            _unstashSchedule = Context.System.Scheduler.ScheduleTellRepeatedlyCancelable(
                TimeSpan.FromSeconds(1),
                TimeSpan.FromSeconds(1),
                Self,
                new ProcessStashedPaymentsMessage(),
                Self);
        }

        protected override void PostStop()
        {
            _unstashSchedule.Cancel();
        }
    }
}
