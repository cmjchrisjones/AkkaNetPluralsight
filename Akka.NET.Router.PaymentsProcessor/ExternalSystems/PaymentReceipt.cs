using System;
using System.Collections.Generic;
using System.Text;

namespace Akka.NET.Router.PaymentsProcessor.ExternalSystems
{
    class PaymentReceipt
    {
        public int AccountNumber { get; set; }

        public string PaymentConfirmationReceipt { get; set; }
    }
}
