using Akka.Actor;
using Akka.PoC.Remote.DomainModels.Models.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Akka.PoC.Remote.DomainModels.Models.Actors
{
    public class DirectorClusterActor : ReceiveActor
    {
        private readonly Guid _guid;
        protected Akka.Cluster.Cluster Cluster = Akka.Cluster.Cluster.Get(Context.System);
        public DirectorClusterActor()
        {
            
            _guid = Guid.NewGuid();



            //Cluster.Subscribe(Self, new[] { typeof(EpisodeRequest) });
            var makingTimeRandomizer = new Random(DateTime.Now.Millisecond);
            ;

            this.Receive<EpisodeRequest>(episodeRq =>
            {
                Console.WriteLine();
                Console.WriteLine("{2}[{0}]: Please make {1} \"Bold and Beautiful\" episode.", episodeRq.SenderName, episodeRq.Episode, Thread.CurrentThread.ManagedThreadId);
                Console.WriteLine("[{0}]: On duty!!! Making...", _guid);
                int time = makingTimeRandomizer.Next(1, 10);
                Thread.Sleep(time * 1000);

                Console.WriteLine("[{0}]: Episode {1} done.", _guid, episodeRq.Episode);

                Sender.Tell(new EpisodeDoneResponse(episodeRq.Episode, _guid.ToString()));
            });
        }

        //protected override void OnReceive(object message)
        //{
        //    var episodeRq = message as EpisodeRequest;

        //    if (episodeRq == null)
        //        return;

        //    var makingTimeRandomizer = new Random(DateTime.Now.Millisecond);
        //    int time = makingTimeRandomizer.Next(1, 10);
        //    Console.WriteLine();
        //    Console.WriteLine("{2}[{0}]: Please make {1} \"Bold and Beautiful\" episode.", episodeRq.SenderName, episodeRq.Episode, Thread.CurrentThread.ManagedThreadId);
        //    Console.WriteLine("[{0}]: On duty!!! Making...", _guid);

        //    Thread.Sleep(time * 1000);

        //    Console.WriteLine("[{0}]: Episode {1} done.", _guid, episodeRq.Episode);
        //}
    }
}
