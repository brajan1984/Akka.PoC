using Akka.Actor;
using Akka.Configuration;
using Akka.Configuration.Hocon;
using Akka.PoC.Remote.DomainModels.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Routing;

namespace Akka.PoC.Remote.DomainModels.Services
{
    public class DeployActorInCluster : IDeployActorInCluster
    {
        private const int ANY_PORT = 0;

        private ActorSystem _system;

        public IActorRef DeployActor<T>(string clusterName, string role)
            where T : IInternalActor
        {
            var section = ConfigurationManager.GetSection("akka") as AkkaConfigurationSection;
            //Override the configuration of the port
            var config =
                ConfigurationFactory.ParseString("akka.remote.helios.tcp.port=" + ANY_PORT)
                    .WithFallback(section.AkkaConfig);

            //create an Akka system
            _system = ActorSystem.Create(clusterName, config);

            //create an actor that handles cluster domain events
            return _system.ActorOf(Props.Create(typeof(T)), role);
        }

        public void Deploy(string clusterName, string role)
        {
            var section = ConfigurationManager.GetSection("akka") as AkkaConfigurationSection;
            //Override the configuration of the port
            var config =
                ConfigurationFactory.ParseString("akka.remote.helios.tcp.port=" + ANY_PORT)
                    .WithFallback(section.AkkaConfig);

            //create an Akka system
            _system = ActorSystem.Create(clusterName, config);
        }
    }
}
