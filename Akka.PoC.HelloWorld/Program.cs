using Akka.Actor;
using Akka.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akka.PoC.HelloWorld
{
    public class Greet
    {
        public Greet(string who, string message)
        {
            Who = who;
            Message = message;
        }
        public string Who { get; private set; }

        public string Message { get; private set; }
    }

    public class GreetingActor : ReceiveActor
    {
        public GreetingActor()
        {
            Receive<Greet>(greet =>
            {
                Console.WriteLine("{0}: {1}", greet.Who, greet.Message);
                Sender.Tell(new Greet("Receiver", "Oh! Hello dear " + greet.Who));
            });

            
        }
    }

    class Program
    {
        static void Main(string[] args)
        {

            // Create a new actor system (a container for your actors)
            var system = ActorSystem.Create("MySystem");

            // Create your actor and get a reference to it.
            // This will be an "ActorRef", which is not a
            // reference to the actual actor instance
            // but rather a client or proxy to it.
            var greeter = system.ActorOf<GreetingActor>("greeter");

            // Send a message to the actor
            var answer =  Task.Run(async () => 
            {
                var result = await greeter.Ask<Greet>(new Greet("Sender", "Hello"));

                return result;
            });

            Console.WriteLine("{0}: {1}", answer.Result.Who, answer.Result.Message);

            // This prevents the app from exiting
            // before the async work is done
            Console.ReadLine();
        }
    }
}
