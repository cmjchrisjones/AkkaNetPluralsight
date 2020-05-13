using Akka.Actor;
using System;

namespace MovieStreamingActorSystem
{
    public class Program
    {
        private static ActorSystem MovieStreamingActorSystem;

        static void Main(string[] args)
        {
            MovieStreamingActorSystem = ActorSystem.Create("MovieStreamingActorSystem");

            Console.ReadLine();

            MovieStreamingActorSystem.Terminate();
        }
    }
}
