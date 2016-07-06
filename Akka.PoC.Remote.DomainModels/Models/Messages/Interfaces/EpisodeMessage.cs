using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akka.PoC.Remote.DomainModels.Models.Messages.Interfaces
{
    /// <summary>
    /// Immutable message
    /// </summary>
    public abstract class EpisodeMessage
    {
        public EpisodeMessage(int episode, string senderName)
        {
            _episode = episode;
            _senderName = senderName;
        }

        private int _episode;

        public int Episode
        {
            get { return _episode; }
            private set { _episode = value; }
        }

        private string _senderName;

        public string SenderName
        {
            get { return _senderName; }
            private set { _senderName = value; }
        }


    }
}
