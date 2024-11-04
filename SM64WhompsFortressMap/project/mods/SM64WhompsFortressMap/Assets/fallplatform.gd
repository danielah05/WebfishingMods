extends Area

onready var platform = get_parent()
onready var platformsfx = get_parent().get_node("AudioStreamPlayer3D")

var hastriggered = false

var grav = 0.0

var rot = 0.0

var waittimer = 0
var falltimer = 0

var state = 0

var origin_y = 0.0

func _ready():
	origin_y = platform.translation.y

func _process(delta):
	if hastriggered == true:
		match state:
			0:
				waittimer += 1
				if waittimer > 5:
					state = 1
					platformsfx.play()
			1:
				falltimer += 1
				grav -= 0.3
				rot += 0.1
				platform.translation.y += grav * delta
				platform.rotate_x(rot * delta)
				platform.rotate_y(rot * delta)
				platform.rotate_z(rot * delta)
				if falltimer > 600: # 600
					state = 2
			2:
				waittimer = 0
				falltimer = 0
				grav = 0.0
				rot = 0.0
				platform.translation.y = origin_y
				platform.set_rotation(Vector3(0.0, 0.0, 0.0))
				hastriggered = false
				state = 0
				

func _on_Area_body_entered(body):
	if body.is_in_group("player"):
		hastriggered = true
	else:
		return 
