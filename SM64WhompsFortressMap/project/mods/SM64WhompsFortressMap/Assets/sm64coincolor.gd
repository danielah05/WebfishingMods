extends Spatial

export var red_coin = false

func _ready():
	if red_coin == true:
		$AnimatedSprite3D.modulate = Color(1, 0, 0)
