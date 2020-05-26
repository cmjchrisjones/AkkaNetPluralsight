using System;
using System.Collections.Generic;
using System.Text;

namespace Akka.NET.Router.PaymentsProcessor.Message
{
    class SendPaymentMessage
    {
        public string FirstName { get; private set; }

        public string LastName { get; private set; }

        public int AccountNumber { get; private set; }

        public decimal Amount { get; private set; }

        public SendPaymentMessage(string firstName, string lastLame, decimal amount, int accountNumber)
        {
            FirstName = firstName;
            LastName = lastLame;
            AccountNumber = accountNumber;
            Amount = amount;
        }

    }
}
