using Godot;
using Godot.Collections;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Tankathon.API;
using Tankathon.EvilTank;
using Tankathon.Scripts;

namespace Tankathon.API.Internal;

public partial class GameManager : Node2D
{
	//TODO: Actually name all these resources based on the team names
	[ExportGroup("Battle Info")]
	[Export]
	BattleInfo battleInfo;

	// [ExportGroup("R2 Fight")]
	// [Export]
	// Resource trTankRes;

	// [ExportGroup("R3 Fight")]
	// [Export]
	// Resource blTankRes;

	// [ExportGroup("R4 Fight")]
	// [Export]
	// Resource brTankRes;

	TheTank tlTank = null;
	TheTank trTank = null;
	TheTank blTank = null;
	TheTank brTank = null;

	//list of combatants
	private List<TeamData> _tankTypes;

	//Game state
	[Export]
	public bool GAMESTART = false;

	AudioStreamPlayer musicPlayer;



	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		musicPlayer = GetNode<AudioStreamPlayer>("%MusicPlayer");

		//initialize the fights here
		//Test Fight
		_tankTypes = new List<TeamData>
		{
			battleInfo.Get("teams").As<Godot.Collections.Array>()[0].As<TeamData>(),
			battleInfo.Get("teams").As<Godot.Collections.Array>()[1].As<TeamData>(),
			battleInfo.Get("teams").As<Godot.Collections.Array>()[2].As<TeamData>(),
			battleInfo.Get("teams").As<Godot.Collections.Array>()[3].As<TeamData>()
		};

		tlTank = GetNode<TheTank>("TopLeftTank");
		brTank = GetNode<TheTank>("BottomRightTank");
		trTank = GetNodeOrNull<TheTank>("TopRightTank");
		blTank = GetNodeOrNull<TheTank>("BottomLeftTank");


		tlTank.thisTank = Activator.CreateInstance(Type.GetType(_tankTypes[0].tankType)) as ITank;
		brTank.thisTank = Activator.CreateInstance(Type.GetType(_tankTypes[1].tankType)) as ITank;
		if (trTank != null)
			trTank.thisTank = Activator.CreateInstance(Type.GetType(_tankTypes[2].tankType)) as ITank;
		if (blTank != null)
			blTank.thisTank = Activator.CreateInstance(Type.GetType(_tankTypes[3].tankType)) as ITank;

		tlTank.Init(_tankTypes[0]);
		brTank.Init(_tankTypes[1]);
		if (trTank != null)
			trTank.Init(_tankTypes[2]);
		if (blTank != null)
			blTank.Init(_tankTypes[3]);
	}

	public void StartGame()
	{
		GAMESTART = true;
		musicPlayer.Play();
	}

    public override void _UnhandledInput(InputEvent @event)
    {
		if (@event is InputEventKey eventKey)
			if (eventKey.Pressed && eventKey.Keycode == Key.Ctrl) //TODO: maybe use a different key? 
				Engine.TimeScale = 3f;
			else
				Engine.TimeScale = 1f;

			base._UnhandledInput(@event);
    }
	
}
