using Akka.Actor;
using Akka.PoC.Remote.DomainModels.Models.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akka.PoC.Remote.DomainModels.Models.Actors
{
    public class ProducerClusterActor : ReceiveActor
    {
        //private List<IActorRef> _remoteActors;
        private int _episodeCounter;
        private ICancelable _episodeTask;
        private Guid _guid;

        public ProducerClusterActor()
        {
            _guid = Guid.NewGuid();

            this.Receive<EpisodeDoneResponse>(response =>
            {
                Console.WriteLine("Episode {0} done by {1}", response.Episode, response.SenderName);
            });

            this.Receive<EpisodeOrder>(order =>
            {
                var cluster = Cluster.Cluster.Get(Context.System);

                var sel = cluster.System.ActorSelection("user/director");

                sel.Tell(new EpisodeRequest(_episodeCounter++, _guid.ToString()));

                Console.WriteLine("Episode {0} request sent", _episodeCounter);
            });
        }

        protected override void PreStart()
        {
            _episodeTask = Context.System.Scheduler.ScheduleTellRepeatedlyCancelable(TimeSpan.FromSeconds(1),
                TimeSpan.FromSeconds(1), Context.Self, new EpisodeOrder(), ActorRefs.NoSender);
        }

        protected override void PostStop()
        {
            _episodeTask.Cancel();
        }
    }
}
