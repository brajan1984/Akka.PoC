using Akka.Actor;
using Akka.Configuration;
using Akka.PoC.Remote.DomainModels.Models.Actors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akka.PoC.Remote
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var system = ActorSystem.Create("Deployer", ConfigurationFactory.ParseString(@"
            akka {  
                actor{
                    provider = ""Akka.Remote.RemoteActorRefProvider, Akka.Remote""
                    deployment {
                        /RemoteDirector {
                            remote = ""akka.tcp://DeployTarget@localhost:8090""
                        }
                    }
                }
                remote {
                    helios.tcp {
                        port = 0
                        hostname = localhost
                    }
                }
            }")))
            {
                var remoteAddress = Address.Parse("akka.tcp://DeployTarget@localhost:8090");
                //deploy remotely via config
                var remoteDirector1 = system.ActorOf(Props.Create(() => new DirectorActor()), "RemoteDirector");

                //deploy remotely via code
                var remoteDirector2 =
                    system.ActorOf(
                        Props.Create(() => new DirectorActor())
                            .WithDeploy(Deploy.None.WithScope(new RemoteScope(remoteAddress))), "RemoteDirectorByCode");
                
                var producerRef = system.ActorOf(Props.Create(() => new ProducerActor(new List<IActorRef>() { remoteDirector1, remoteDirector2 })));

                

                Console.ReadKey();
            }
        }
    }
}
