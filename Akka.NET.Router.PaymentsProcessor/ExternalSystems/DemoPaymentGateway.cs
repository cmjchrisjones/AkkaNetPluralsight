using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Akka.NET.Router.PaymentsProcessor.ExternalSystems
{
    class DemoPaymentGateway : IPaymentGateway
    {
        public void Pay(int accountNumber, decimal amount)
        {
            Thread.Sleep(200);
        }
    }
}
