using Akka.PoC.Remote.DomainModels.Models.Actors;
using Akka.PoC.Remote.DomainModels.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akka.PoC.Cluster.ProducerNode
{
    class Program
    {
        static void Main(string[] args)
        {
            var deployer = new DeployActorInCluster();

            deployer.Deploy<ProducerClusterActor>("ClusterPoC", "producer");

            Console.ReadKey();
        }
    }
}
