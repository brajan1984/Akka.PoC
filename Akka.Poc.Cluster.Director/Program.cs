using Akka.PoC.Remote.DomainModels.Models.Actors;
using Akka.PoC.Remote.DomainModels.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akka.Poc.Cluster.Director
{
    class Program
    {
        static void Main(string[] args)
        {
            var deployer = new DeployActorInCluster();

            deployer.Deploy<DirectorClusterActor>("ClusterPoC", "director");

            Console.ReadKey();
        }
    }
}
