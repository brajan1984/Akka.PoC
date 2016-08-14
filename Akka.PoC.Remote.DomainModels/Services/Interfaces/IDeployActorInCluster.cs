using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akka.PoC.Remote.DomainModels.Services.Interfaces
{
    public interface IDeployActorInCluster
    {
        IActorRef DeployActor<T>(string clusterName, string role)
            where T : IInternalActor;
    }
}
