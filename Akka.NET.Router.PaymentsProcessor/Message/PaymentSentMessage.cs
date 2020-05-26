using System;
using System.Collections.Generic;
using System.Text;

namespace Akka.NET.Router.PaymentsProcessor.Message
{
    class PaymentSentMessage
    {
        public int AccountNumner { get; private set; }

        public PaymentSentMessage(int accountNumner)
        {
            AccountNumner = accountNumner;
        }
    }
}
