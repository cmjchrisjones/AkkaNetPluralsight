using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Akka.Actor;
using Akka.DI.Core;
using Akka.NET.Router.PaymentsProcessor.Message;
using Akka.Routing;

namespace Akka.NET.Router.PaymentsProcessor.Actors
{
    class JobCoordinatorActor : ReceiveActor
    {
        private readonly IActorRef PaymentWorker;
        private int NumberOfRemainingPayments;

        public JobCoordinatorActor()
        {
            // OLD WAY
            //PaymentWorker = Context.ActorOf(Context.DI().Props<PaymentWorkerActor>(), "PaymentsWorker");

            // NEW WAY - using group actors
            PaymentWorker = Context.ActorOf(Props.Empty.WithRouter(new RoundRobinGroup(
                "/user/PaymentWorker1",
                "/user/PaymentWorker2",
                "/user/PaymentWorker3"
                )));

            Receive<ProcessFileMessage>(m =>
            {
                StartNewJob(m.FileName);
            });

            Receive<PaymentSentMessage>(m =>
            {
                NumberOfRemainingPayments--;

                Console.WriteLine($"Requests remaining: {NumberOfRemainingPayments}");

                var jobIsComplete = NumberOfRemainingPayments == 0;

                if (jobIsComplete)
                {
                    Context.System.Terminate();
                }
            });
        }

        private void StartNewJob(string fileName)
        {
            List<SendPaymentMessage> requests = ParseCsvFile(fileName);
            NumberOfRemainingPayments = requests.Count;
            foreach(var spm in requests)
            {
                PaymentWorker.Tell(spm);
            }
        }

        private List<SendPaymentMessage> ParseCsvFile(string fileName)
        {
            var messagesToSend = new List<SendPaymentMessage>();
            var fileLines = File.ReadAllLines(fileName);
            foreach(var line in fileLines)
            {
                var values = line.Split(',');

                var message = new SendPaymentMessage(
                    values[0],
                    values[1],
                    decimal.Parse(values[2]),
                    int.Parse(values[3]));

                messagesToSend.Add(message);
            }

            return messagesToSend; 
        }
    }
}
