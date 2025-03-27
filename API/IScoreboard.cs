using System;
using System.Collections.Generic;

namespace Tankathon.API
{
    public interface IScoreboard
    {
        public int timeLeft { get; }
    }
}