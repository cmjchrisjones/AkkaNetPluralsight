using System;
using System.Collections.Generic;
using System.Text;

namespace Akka.NET.Router.PaymentsProcessor.Actors
{
    public class PeakTimeDemoSimulator
    {

        private static DateTime DemoStartTime;
        private static int StayPeakTimeForSeconds;

        public static bool IsPeakHours
        {
            get
            {
                // Simulate being in peak time for a number of seconds after starting teh demo
                var elapsedTimeSinceStartingDemo = DateTime.Now.Subtract(DemoStartTime).TotalSeconds;

                return elapsedTimeSinceStartingDemo < StayPeakTimeForSeconds;
            }

        }

        public static void StartDemo(int stayPeakTimeForSeconds)
        {
            DemoStartTime = DateTime.Now;
            StayPeakTimeForSeconds = stayPeakTimeForSeconds;
        }
    }
}
