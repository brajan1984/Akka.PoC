using Akka.Actor;
using Akka.PoC.Remote.DomainModels.Models.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akka.PoC.Remote.DomainModels.Models.Actors
{
    public class ProducerActor : ReceiveActor
    {
        private List<IActorRef> _remoteActors;
        private int _episodeCounter;
        private ICancelable _episodeTask;
        private Guid _guid;
        private int _current = 0;

        public ProducerActor(List<IActorRef> actors)
        {
            _guid = Guid.NewGuid();
            _remoteActors = actors;

            this.Receive<EpisodeDoneResponse>(response =>
            {
                Console.WriteLine("Episode {0} done by {1}", response.Episode, response.SenderName);
            });

            this.Receive<EpisodeOrder>(order =>
            {

                _current++;
                if (_current > _remoteActors.Count - 1)
                {
                    _current = 0;
                }

                _remoteActors[_current].Tell(new EpisodeRequest(_episodeCounter++, _guid.ToString()));
            });
        }

        public void IncreaseProductivity(IActorRef actor)
        {
            _remoteActors.Add(actor);
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
