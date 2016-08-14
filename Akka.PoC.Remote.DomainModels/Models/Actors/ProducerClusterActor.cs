using Akka.Actor;
using Akka.Cluster.Routing;
using Akka.PoC.Remote.DomainModels.Models.Messages;
using Akka.Routing;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Cluster;
using Akka.PoC.Remote.DomainModels.Services;

namespace Akka.PoC.Remote.DomainModels.Models.Actors
{
   
    public class ProducerClusterActor : ReceiveActor
    {
        //private List<IActorRef> _remoteActors;
        private int _episodeCounter;
        private ICancelable _episodeTask;
        private Guid _guid;
        private IActorRef _director;

        public ProducerClusterActor()
        {
            _guid = Guid.NewGuid();

            this.Receive<EpisodeDoneResponse>(response =>
            {
                Console.WriteLine("Episode {0} done by {1}", response.Episode, response.SenderName);
            });

            this.Receive<EpisodeOrder>(order =>
            {
                var members = Cluster.Cluster.Get(Context.System).State.Members;

                //var director = Context.System.ActorSelection("director");

                

                var directors = members.Where(m => m.Roles.Contains("director") && m.Status == MemberStatus.Up);


                var list = directors.Select(d => d.UniqueAddress.Address.ToString()).ToList();

                //var director = Context.System.ActorOf(Props.Empty.WithRouter(new RandomGroup(list)));

                _director.Tell(new EpisodeRequest(_episodeCounter++, _guid.ToString()));

                Console.WriteLine("Episode {0} request sent", _episodeCounter);
            });
        }

        protected override void PreStart()
        {
            //var deployer = new DeployActorInCluster();
            //_director = Context.System.ActorOf(Props.Create(() => new DirectorClusterActor()), "director");

            _director = Context.System.ActorOf(Props.Empty.WithRouter(FromConfig.Instance), "director");

            _episodeTask = Context.System.Scheduler.ScheduleTellRepeatedlyCancelable(TimeSpan.FromSeconds(1),
                TimeSpan.FromSeconds(1), Context.Self, new EpisodeOrder(), ActorRefs.NoSender);
        }

        protected override void PostStop()
        {
            _episodeTask.Cancel();
        }
    }
}
