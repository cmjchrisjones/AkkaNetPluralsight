﻿using Akka.Actor;
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

        public PaymentWorkerActor(IPaymentGateway paymentGateway)
        {
            _paymentGateway = paymentGateway;
            Receive<SendPaymentMessage>(m => SendPayment(m));

        }

        private async void SendPayment(SendPaymentMessage message)
        {
            var sender = Sender;
            var result = await _paymentGateway.Pay(message.AccountNumber, message.Amount);
            sender.Tell(new PaymentSentMessage(result.AccountNumber, result.PaymentConfirmationReceipt));
        }
    }
}
