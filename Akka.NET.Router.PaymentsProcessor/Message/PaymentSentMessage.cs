using System;
using System.Collections.Generic;
using System.Text;

namespace Akka.NET.Router.PaymentsProcessor.Message
{
    class PaymentSentMessage
    {
        public int AccountNumner { get; private set; }

        public string PaymentReceiptConfirmation { get; private set; }

        public PaymentSentMessage(int accountNumner, string paymentReceiptConfirmation)
        {
            AccountNumner = accountNumner;
            PaymentReceiptConfirmation = paymentReceiptConfirmation;
        }
    }
}
