﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Akka.NET.Router.PaymentsProcessor.ExternalSystems
{
    interface IPaymentGateway
    {
        Task<PaymentReceipt> Pay(int accountNumber, decimal amount);
    }
}
