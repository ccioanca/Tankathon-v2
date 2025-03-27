using System;
using Tankathon.API;
using Tankathon.API.Internal;

namespace Tankathon.API;

public interface ITank
{
	void Setup(ITankSetup setup);
	void Do(IActions actions, IScoreboard scoreboard);
}
