class_name pointsPanel
extends MarginContainer

@export
var tank_name := ""
@export
var tank_health = 0
@export
var tank_points := 0

var tank_name_panel
var tank_health_panel
var tank_points_panel
var tank_col_primary
var tank_col_secondary

func _ready():
	tank_name_panel = get_node("HBox/TankName") as Label;
	tank_name_panel.text = tank_name
	
	tank_health_panel = get_node("HBox/TankHealth") as Label;
	change_health(tank_health)
	
	tank_points_panel = get_node("HBox/CenterContainer/TankPoints") as Label;
	change_points(tank_points)
	
	tank_col_primary = get_node("HBox/CenterContainer/ColorPanelPrimary") as ColorRect;
	tank_col_secondary = get_node("HBox/CenterContainer/ColorPanelSecondary") as ColorRect;
	pass

func change_health(health):
	tank_health = health
	tank_health_panel.text = "%s/10" % tank_health
	if tank_health == 0:
		# gray out panel
		get_node("HBox/TankName").set("modulate", Color("#fff", 0.3))
		get_node("HBox/TankHealth").set("modulate", Color("#fff", 0.3))
		get_node("HBox/CenterContainer/TankPoints").set("modulate", Color("#fff", 0.3))
		pass
	
func change_points(points):
	tank_points = points
	tank_points_panel.text = "%s" % tank_points
	
func change_panel_color(primaryCol, secondaryCol):
	tank_col_primary.color = Color(primaryCol);
	tank_col_secondary.color = Color(secondaryCol);
	pass
	
