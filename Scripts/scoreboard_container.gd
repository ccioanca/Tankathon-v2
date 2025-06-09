extends PanelContainer

func _process(delta: float) -> void:
	if Input.is_action_pressed("hide_ui"):
		set("visible", false);
	if Input.is_action_just_released("hide_ui"):
		set("visible", true);
