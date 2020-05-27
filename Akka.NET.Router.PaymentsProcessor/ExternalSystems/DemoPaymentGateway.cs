using Akka.NET.Router.PaymentsProcessor.Actors;
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
            if (PeakTimeDemoSimulator.IsPeakHours && amount > 100)
            {
                Console.WriteLine($"Account number {accountNumber} payment takes longer because its peak hour and the amount ({amount}) is over the 100 threshold");
                Thread.Sleep(2000);
            }
            else
            {
                Thread.Sleep(200);
            }
        }
    }
}
