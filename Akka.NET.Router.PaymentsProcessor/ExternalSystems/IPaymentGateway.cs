using System;
using System.Collections.Generic;
using System.Text;

namespace Akka.NET.Router.PaymentsProcessor.ExternalSystems
{
    interface IPaymentGateway
    {
        void Pay(int accountNumber, decimal amount);
    }
}
