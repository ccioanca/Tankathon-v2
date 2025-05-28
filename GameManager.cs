using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Tankathon.API;
using Tankathon.EvilTank;

namespace Tankathon.API.Internal;
public partial class GameManager : Node2D
{
	[ExportGroup("TL Tank")]
	[Export]
	Resource tlTankRes;

	[ExportGroup("TR Tank")]
	[Export]
	Resource trTankRes;

	[ExportGroup("BL Tank")]
	[Export]
	Resource blTankRes;

	[ExportGroup("BR Tank")]
	[Export]
	Resource brTankRes;

	TheTank tlTank = null;
	TheTank trTank = null;
	TheTank blTank = null;
	TheTank brTank = null;

	//list of combatants
	private List<TeamModel> _tankTypes;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		//initialize the current fight here
		_tankTypes = new List<TeamModel>
		{
			new TeamModel(typeof(DumTank), tlTankRes)
		};



		tlTank = GetNode<TheTank>("TopLeftTank");
		trTank = GetNode<TheTank>("TopRightTank");
		blTank = GetNode<TheTank>("BottomLeftTank");
		brTank = GetNode<TheTank>("BottomRightTank");


		tlTank.thisTank = Activator.CreateInstance(_tankTypes[0].tankType) as ITank;
		trTank.thisTank = Activator.CreateInstance(_tankTypes[0].tankType) as ITank;
		blTank.thisTank = Activator.CreateInstance(_tankTypes[0].tankType) as ITank;
		brTank.thisTank = Activator.CreateInstance(_tankTypes[0].tankType) as ITank;

		tlTank.Init(_tankTypes[0].tankInfo.Get("shootSound").As<AudioStream>(), _tankTypes[0].tankInfo.Get("deathSound").As<AudioStream>());
		trTank.Init();
		blTank.Init();
		brTank.Init();
	}
}
