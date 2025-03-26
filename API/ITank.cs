using System;
using Tankathon.API;
using Tankathon.API.Internal;

namespace Tankathon.API;

public interface ITank
{
	void Setup(ITankStats stats);
	void Do(IActions actions, IScoreboard scoreboard);
}
