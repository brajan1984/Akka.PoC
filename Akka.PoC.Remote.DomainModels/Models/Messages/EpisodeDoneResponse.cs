using Akka.PoC.Remote.DomainModels.Models.Messages.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akka.PoC.Remote.DomainModels.Models.Messages
{
    public class EpisodeDoneResponse : EpisodeMessage
    {
        public EpisodeDoneResponse(int episode, string name)
            : base(episode, name)
        {
        }
    }
}
