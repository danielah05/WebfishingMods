extends Area

onready var coin = get_parent()
onready var coinsfx = get_parent().get_node("AudioStreamPlayer3D")

var hastriggered = false

var state = 0

var respawntimer = 0

func _process(delta):
	if hastriggered == true:
		match state:
			0:
				coinsfx.play()
				coin.visible = false
				state = 1
			1:
				respawntimer += 1
				if respawntimer > 600: # 600
					state = 2
			2:
				coin.visible = true
				respawntimer = 0
				state = 0
				hastriggered = false
	

func _on_Area_body_entered(body):
	if body.is_in_group("player"):
		hastriggered = true
	else:
		return 
