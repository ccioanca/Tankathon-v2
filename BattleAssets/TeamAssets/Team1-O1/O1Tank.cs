using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using Tankathon.API;
using GD = Godot.GD;
using Side = Tankathon.API.Side;

namespace Tankathon.MyTank;
public class O1Tank : ITank {
	const double delta = 1.0 / 60.0;
	private double stateTime = 0;
	private double noMovingTime = 0;
	private TankState state = TankState.Searching;

	enum TankState {
		Searching,
		Following,
		Turning,
		Dodging
	}

	public void Setup(ITankSetup setup) {
		setup.name = "o(1)";
		setup.primaryColor = "#FF6600";
		setup.secondaryColor = "#CC4400";
	}


	private Rotation currentRoataionDirection = Rotation.CW;
	private float distanceTank = 200;
	private float distanceObstacle = 140;
	private float speedTank = 3.33333f;
	private float speedBullet = 4.33333f;
	private float speedRotating = 1.9f;

	private Side[] sides = [Side.Middle, Side.Left, Side.Right];


	private static double ToDegrees(double radians) {
		return radians / Math.PI * 180.0;
	}

	private static double ToRadians(double degrees) {
		return degrees * Math.PI / 180.0;
	}

	private Queue<(float x, float y, double d)> targetPoints = new(10);

	private void AddTargetPoint(Entity e) {
		if (targetPoints.Count >= 10) {
			targetPoints.Dequeue();
		}
		targetPoints.Enqueue((e.globalPosition.X, e.globalPosition.Y, ToDegrees(e.rotation))); // Add the new item
	}

	private float myLastX;
	private float myLastY;

	private double bulletLastX;
	private double bulletLastY;
	private double bulletLastRotation;
	private double bulletLastTime;

	public void Do(IActions actions, IScoreboard scoreboard) {

		stateTime += delta;
		noMovingTime += delta;
		
		var myX = actions.stats.xPos;
		var myY = actions.stats.yPos;
		var myRotation = actions.stats.rotation;

		var scan = actions.Scan();

		DetectBullet(actions, scan);

		switch (state) {
			case TankState.Searching:
				DoSearch(actions, scan);
				break;

			case TankState.Following:
				DoFollow(actions, scan);
				break;

			case TankState.Turning:
				DoTurn(actions, scan);
				break;

			case TankState.Dodging:
				DoDodge(actions, scan);
				break;
		}

		var moved = Math.Sqrt(Math.Pow(myX - myLastX, 2) + Math.Pow(myY - myLastY, 2));
		myLastX = myX;
		myLastY = myY;
		if (moved > 1) { 		
			noMovingTime = 0;
		}


		actions.Aim(currentRoataionDirection);

		if (!(state == TankState.Searching && stateTime > 4)) {

			actions.MoveForward();

		}

	}

	private void DoSearch(IActions actions, Dictionary<Side, Entity> scan) {

		if (scan[Side.Left].eType == EntityType.Tank || scan[Side.Middle].eType == EntityType.Tank || scan[Side.Right].eType == EntityType.Tank) {

			//GD.Print($"Left: {(scan[Side.Left].eType == EntityType.Tank ? global::Tankathon.MyTank.MyTank.ToDegrees(scan[Side.Left].rotation) : "")}, Middle: {(scan[Side.Middle].eType == EntityType.Tank ? global::Tankathon.MyTank.MyTank.ToDegrees(scan[Side.Middle].rotation) : "")}, Right: {(scan[Side.Right].eType == EntityType.Tank ? global::Tankathon.MyTank.MyTank.ToDegrees(scan[Side.Right].rotation) : "")}");

			if (scan[Side.Left].eType == EntityType.Tank && scan[Side.Middle].eType == EntityType.Tank && scan[Side.Right].eType == EntityType.Tank) {
				AddTargetPoint(scan[Side.Middle]);
			}
			else if (scan[Side.Left].eType == EntityType.Tank && scan[Side.Middle].eType == EntityType.Tank) {
				AddTargetPoint(scan[Side.Middle]);
			}
			else if (scan[Side.Middle].eType == EntityType.Tank && scan[Side.Right].eType == EntityType.Tank) {
				AddTargetPoint(scan[Side.Middle]);
			}
			else if (scan[Side.Left].eType == EntityType.Tank) {
				AddTargetPoint(scan[Side.Left]);
			}
			else if (scan[Side.Right].eType == EntityType.Tank) {
				AddTargetPoint(scan[Side.Right]);
			}
			else if (scan[Side.Middle].eType == EntityType.Tank) {
				AddTargetPoint(scan[Side.Middle]);
			}

			if (scan[Side.Middle].eType == EntityType.Tank) {
				//PredictAndFire(actions);	
				state = TankState.Following;

			}

		}
		else {

			if (currentRoataionDirection == Rotation.None) {
				currentRoataionDirection = scan[Side.Left].distanceTo > scan[Side.Right].distanceTo ? Rotation.CCW : Rotation.CW;
				stateTime = 0;
			}
			else {
				if (stateTime > 8) {
					currentRoataionDirection = currentRoataionDirection == Rotation.CW ? Rotation.CCW : Rotation.CW;
					stateTime = 0;
				}
			}

		}

	}


	private void DoFollow(IActions actions, Dictionary<Side, Entity> scan) {

		if (scan[Side.Left].eType == EntityType.Tank || scan[Side.Middle].eType == EntityType.Tank || scan[Side.Right].eType == EntityType.Tank) {

			if (scan[Side.Middle].eType == EntityType.Tank) {
				AddTargetPoint(scan[Side.Middle]);
				actions.Fire();
				//PredictAndFire(actions);
				currentRoataionDirection = Rotation.None;

				if (scan[Side.Middle].distanceTo < distanceTank || noMovingTime > 1) {
					state = TankState.Turning;
					stateTime = 0;
				}
			}
			else {
				if (scan[Side.Left].eType == EntityType.Tank) {
					AddTargetPoint(scan[Side.Left]);
					currentRoataionDirection = Rotation.CW;
				}
				else if (scan[Side.Right].eType == EntityType.Tank) {
					AddTargetPoint(scan[Side.Right]);
					currentRoataionDirection = Rotation.CW;
				}
			}
		}
		else {
			state = TankState.Searching;
			stateTime = 0;


		}

	}

	private void DoTurn(IActions actions, Dictionary<Side, Entity> scan) {

		foreach (var side in sides) {
			var ent = scan[side];
			if (ent.eType == EntityType.Tank) {
				AddTargetPoint(ent);
				currentRoataionDirection = ent.rotation > 0 ? Rotation.CCW : Rotation.CW;

				if (side == Side.Middle) {
					actions.Fire();
					//PredictAndFire(actions);
				}
			}
		}

		if (stateTime > 2) {
			state = TankState.Searching;
			stateTime = 0;
		}

	}

	private void DoDodge(IActions actions, Dictionary<Side, Entity> scan) {

		bulletLastTime += delta;

		if (sides.Any(s => scan[s].eType == EntityType.Bullet)) {
			if (scan[Side.Left].eType == EntityType.Bullet) {
				currentRoataionDirection = Rotation.CW;
			}
			else if (scan[Side.Right].eType == EntityType.Bullet) {
				currentRoataionDirection = Rotation.CCW;
			}
			else if (scan[Side.Middle].eType == EntityType.Bullet) {
				currentRoataionDirection = Rotation.CW;
			}
		}

		double xB = bulletLastX + speedBullet * Math.Sin(bulletLastRotation) * bulletLastTime;
		double yB = bulletLastY - speedBullet * Math.Cos(bulletLastRotation) * bulletLastTime;
		var collisionInSec = DetectCollision(xB, yB, speedBullet, bulletLastRotation, 5, actions.stats.xPos, actions.stats.yPos, speedTank, ToRadians(speedRotating), ToRadians(actions.stats.rotation), 12, currentRoataionDirection, 500);
		if (collisionInSec == -1) {
			state = TankState.Searching;
			stateTime = 0;
		}
	}

	private void DetectBullet(IActions actions, Dictionary<Side, Entity> scan) {
		foreach (var side in sides) {
			var bullet = scan[side];
			if (bullet.eType == EntityType.Bullet) {

				var collisionInSec = DetectCollision(
					bullet.globalPosition.X, bullet.globalPosition.Y, speedBullet, bullet.rotation, 5,
					actions.stats.xPos, actions.stats.yPos, speedTank, ToRadians(speedRotating), ToRadians(actions.stats.rotation), 12, currentRoataionDirection, 500);

				if (collisionInSec > 0) {
					state = TankState.Dodging;
					stateTime = 0;
					bulletLastX = bullet.globalPosition.X;
					bulletLastY = bullet.globalPosition.Y;
					bulletLastRotation = bullet.rotation;
					bulletLastTime = 0;

					return;
				}
				
			}
		}
	}

	private bool PredictAndFire(IActions actions) {

		// Ensure we have enough data points to predict movement
		if (targetPoints.Count < 2) {
			return false; // Not enough data to predict
		}

		if (actions.FireCooldown() != 0) {
			return false;
		}

		// Get the last two positions of the entity
		var lastPoint = targetPoints.ElementAt(targetPoints.Count - 1);
		var secondLastPoint = targetPoints.ElementAt(targetPoints.Count - 2);

		// Calculate the direction vector of the entity's movement
		var entityDirectionX = lastPoint.x - secondLastPoint.x;
		var entityDirectionY = lastPoint.y - secondLastPoint.y;

		// Normalize the direction vector
		var magnitude = Math.Sqrt(entityDirectionX * entityDirectionX + entityDirectionY * entityDirectionY);
		if (magnitude == 0) {
			actions.Fire();
			return true; // Entity is stationary
		}
		entityDirectionX /= (float)magnitude;
		entityDirectionY /= (float)magnitude;

		// Predict the next position of the entity
		var predictedX = lastPoint.x + entityDirectionX * lastPoint.d;
		var predictedY = lastPoint.y + entityDirectionY * lastPoint.d;

		// Calculate the angle to the predicted position
		var myX = actions.stats.xPos;
		var myY = actions.stats.yPos;
		var angleToTarget = Math.Atan2(predictedY - myY, predictedX - myX);
		var angleToTargetDegrees = ToDegrees((float)angleToTarget);

		// Adjust the tank's aim to the predicted position
		var myRotation = actions.stats.rotation;
		var rotationDifference = angleToTargetDegrees - myRotation;

		currentRoataionDirection = rotationDifference >= 0 ? Rotation.CW : Rotation.CCW;

		actions.Aim(currentRoataionDirection);

		actions.Fire();
		return true; // Successfully fired
	}


	private int DetectCollision(double x1, double y1, double v1, double r1, double radius1, double x2, double y2, double v2, double w2, double r2, double radius2, Rotation direction, int maxFrames) {

		if (direction == Rotation.None) {

			for (int t = 0; t <= maxFrames; t++) {
				double xB = x1 + v1 * Math.Sin(r1) * t;
				double yB = y1 - v1 * Math.Cos(r1) * t;
				double xT = x2 + v2 * Math.Sin(r2) * t;
				double yT = y2 - v2 * Math.Cos(r2) * t;
				if (xB <= 0 || yB <= 0 || xT <= 0 || yT <= 0) return -1;
				double dx = xB - xT;
				double dy = yB - yT;
				double distSq = dx * dx + dy * dy;
				double radSum = radius1 + radius2;
				if (distSq <= radSum * radSum) {
					//return (t, (xB + xT) / 2, (yB + yT) / 2);
					return t;
				}
			}
		}
		else {

			double cr = v2 / w2;
			double cx = direction == Rotation.CW ? x2 + cr * Math.Cos(r2) : x2 - cr * Math.Cos(r2);
			double cy = direction == Rotation.CW ? y2 + cr * Math.Sin(r2) : y2 - cr * Math.Sin(r2);

			for (int t = 0; t <= maxFrames; t++) {
				double xB = x1 + v1 * Math.Sin(r1) * t;
				double yB = y1 - v1 * Math.Cos(r1) * t;
				double xT = cx + cr * Math.Cos(r2 + w2 * t * (direction == Rotation.CW ? 1 : -1));
				double yT = cy + cr * Math.Sin(r2 + w2 * t * (direction == Rotation.CW ? 1 : -1));
				if (xB <= 0 || yB <= 0 || xT <= 0 || yT <= 0) return -1;
				double dx = xB - xT;
				double dy = yB - yT;
				double distSq = dx * dx + dy * dy;
				double radSum = radius1 + radius2;
				if (distSq <= radSum * radSum) {
					//return (t, (xB + xT) / 2, (yB + yT) / 2);
					return t;
				}
			}
		}

		return -1;


	}

}
