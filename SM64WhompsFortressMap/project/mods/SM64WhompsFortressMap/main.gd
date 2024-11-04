extends Node

const ID = "SM64WhompsFortressMap"
onready var Lure = get_node("/root/SulayreLure")

var applyjump = false

func _ready():
	Lure.add_map("SM64WhompsFortressMap", "sm64wf_map", "mod://Scenes/sm64wf_map.tscn", "SM64 Whomp's Fortress")

func _process(delta):
	var map: Node = get_tree().current_scene
	
	if map.name == "world":
		var player = get_tree().current_scene.get_node("Viewport/main/entities/player")
		if Lure.Mapper.selected_map == "SM64WhompsFortressMap.sm64wf_map":
			if applyjump == false:
				player.jump_height += 4.0
				applyjump = true
		else:
			if applyjump == true:
				player.jump_height -= 4.0
				applyjump = false
