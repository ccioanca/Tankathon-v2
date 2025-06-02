extends CanvasLayer



func _ready() -> void:
    # Get the battle info from the gamemanager to display
    pass

func _process(_delta: float) -> void:
    pass

func _unhandled_input(event: InputEvent) -> void:
    if event is InputEventKey:
        if event.pressed and event.keycode == KEY_ENTER:
            (get_node("AnimationPlayer") as AnimationPlayer).play("intro_enter")
    pass