using Akka.Actor;
using Akka.Configuration;
using Akka.Configuration.Hocon;
using Akka.PoC.Remote.DomainModels.Models.Actors;
using Akka.PoC.Remote.DomainModels.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akka.PoC.Cluster.NonSeedNode
{
    class Program
    {
        private const int ANY_PORT = 0;
        static void Main(string[] args)
        {
            var deployer = new DeployActorInCluster();

            //deployer.Deploy<SimpleClusterListener>("ClusterPoC", "clusterListener");
            deployer.DeployActor<ProducerClusterActor>("webcrawler", "producer");

            Console.ReadKey();
        }
    }
}
