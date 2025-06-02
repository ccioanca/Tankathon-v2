using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using Tankathon.API.Internal;

namespace Tankathon.API
{
	public partial class Scoreboard : Control, IScoreboard
	{
		public int timeLeft => (int)_timer.TimeLeft;


		//private members
		Timer _timer = new Timer();
		Label _timeLeft;
		//Label _blueScore;
		//Label _redScore;

		public override void _Ready()
		{
			base._Ready();

			//Add the 5 Minute Round Timer
			AddChild(_timer);
			_timer.Timeout += () => Timeout();
			_timeLeft = GetNode<Label>("TimeLeft");
		}

		public override void _Process(double delta)
		{
			base._Process(delta);
		}

		public override void _PhysicsProcess(double delta)
		{
			base._PhysicsProcess(delta);

			_timeLeft.Text = TimeSpan.FromSeconds(_timer.TimeLeft).ToString(@"mm\:ss");
		}

		private void Timeout()
		{
			Engine.TimeScale = 0;
		}
		public void RestartPressed()
		{
			GetTree().ReloadCurrentScene();
		}

		public void StartTimer()
		{
			_timer.Start(3 * 60); //start the 3 minute timer
		}
        
    }
}
