extends Actor

func _ready():
	PlayerData._send_notification("a meteor has landed!") # send notification whenever a meteor spawns
	$AnimationPlayer.play("main")
	$Area.id = actor_id

func _physics_process(delta):
	var s = 1.1 + (sin(OS.get_ticks_msec() * 0.004) * 0.2)
	$meatyore / Sprite3D2.scale = Vector3(s, s, s)
