using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Cluster;
using Akka.Actor;
using Akka.Event;
using Akka.Configuration.Hocon;
using System.Configuration;
using Akka.Configuration;
using Akka.PoC.Remote.DomainModels.Models.Actors;

namespace Akka.PoC.Cluster.SeedNode
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] ports = { "2551", "2552" };

            var section = ConfigurationManager.GetSection("akka") as AkkaConfigurationSection;


            var seedAddresses = section.AkkaConfig.GetStringList("akka.cluster.seed-nodes");

            foreach (var addr in seedAddresses)
            {
                var seedNodeAddr = new Uri(addr);

                //Override the configuration of the port
                var config = ConfigurationFactory.ParseString("akka.remote.helios.tcp.port=" + seedNodeAddr.Port)
                    .WithFallback(section.AkkaConfig);

                //create an Akka system
                var system = ActorSystem.Create("ClusterPoC", config);

                //create an actor that handles cluster domain events
                system.ActorOf(Props.Create(typeof(SimpleClusterListener)), "clusterListener");
            }

            Console.WriteLine("Cluster set.");
            Console.ReadKey();
        }
    }
}
