using Akka.Actor;
using Akka.NET.Router.PaymentsProcessor.ExternalSystems;
using Akka.NET.Router.PaymentsProcessor.Message;
using System;
using System.Collections.Generic;
using System.Text;

namespace Akka.NET.Router.PaymentsProcessor.Actors
{
    class PaymentWorkerActor : ReceiveActor
    {
        private readonly IPaymentGateway _paymentGateway;

        public PaymentWorkerActor(IPaymentGateway paymentGateway)
        {
            _paymentGateway = paymentGateway;
            Receive<SendPaymentMessage>(m => SendPayment(m));
        }

        private void SendPayment(SendPaymentMessage message)
        {
            Console.WriteLine($"Sending payment for {message.FirstName} {message.LastName}");
            _paymentGateway.Pay(message.AccountNumber, message.Amount);
            Console.WriteLine($"Sender: {Sender.Path}");
            Sender.Tell(new PaymentSentMessage(message.AccountNumber));
        }
    }
}
