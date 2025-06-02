extends CanvasLayer

var battleInfo: Resource

var tlTeam;
var trTeam;
var blTeam;
var brTeam;

func _ready() -> void:
	# Get the battle info from the gamemanager to display
	battleInfo = get_tree().root.get_node("GameScene").battleInfo

	tlTeam = get_node("%TLContainer")
	trTeam = get_node("%TRContainer")
	blTeam = get_node("%BLContainer")
	brTeam = get_node("%BRContainer")


	tlTeam.get_node("Logo").texture = battleInfo.teams[0].logo
	trTeam.get_node("Logo").texture = battleInfo.teams[1].logo
	blTeam.get_node("Logo").texture = battleInfo.teams[2].logo
	brTeam.get_node("Logo").texture = battleInfo.teams[3].logo

	tlTeam.get_node("Label").text = battleInfo.teams[0].teamName
	trTeam.get_node("Label").text = battleInfo.teams[1].teamName
	blTeam.get_node("Label").text = battleInfo.teams[2].teamName
	brTeam.get_node("Label").text = battleInfo.teams[3].teamName

	get_node("%BattleName").text = battleInfo.battleName

	pass

func _process(_delta: float) -> void:
	pass

func _unhandled_input(event: InputEvent) -> void:
	if event is InputEventKey:
		if event.pressed and event.keycode == KEY_ENTER:
			(get_node("AnimationPlayer") as AnimationPlayer).play("intro_enter")
	pass
