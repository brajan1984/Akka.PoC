using Akka.Actor;
using Akka.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akka.PoC.Remote.Deployee
{
    class Program
    {
        
        static void Main(string[] args)
        {
        

            using (var system = ActorSystem.Create("DeployTarget", ConfigurationFactory.ParseString(@"
            akka {  
                actor.provider = ""Akka.Remote.RemoteActorRefProvider, Akka.Remote""
                remote {
                    helios.tcp {
                        port = 8090
                        hostname = localhost
                    }
                }
            }")))
            {
                Console.ReadKey();
            }
        }
    }
}
