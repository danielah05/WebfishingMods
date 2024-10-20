extends Minigame

const end_goal = 95.0

var main_progress = 0.0
var bad_progress = 0.0
var yank_spots = []
var reel_speed = 0.2
var bad_speed = 0.1

var vibrate_amt = 0.0
var main_offset = 0.0

var active = false
var offset_active = false
var at_yank = false
var yank_hp = 3

var thrash_cd = 0.0
var thrash_max = 180.0
var thrash_min = 90.0
var thrash_timer = 0.0
var thrash_fail = false
var killed = false
var kill_torque = 0.0
var over = false

var recent_reel = 0
var recent_yank = 0
var rod_type = ""
var reel_mult = 1.0

var challenge_star_timer = 0
var challenge_star_worth = 0
var challenge_stars_active = false

onready var reel_button = $main / CenterContainer / texmain / reel_button
onready var reel = $main / reel
onready var reel_pivot = $main / CenterContainer / texmain / pivot
onready var reel_point = $main / CenterContainer / texmain / pivot / TextureRect / reelpoint
onready var center = $main / CenterContainer
onready var scroll = $main / CenterContainer / texmain / scroll
onready var scroll_bad = $main / CenterContainer / texmain / scroll_bad
onready var main = $main
onready var anim = $AnimationPlayer
onready var cntdn = $main / countdown / TextureRect
onready var yank_warning = $main / yank_warning
onready var reel_warning = $main / reel_warning

onready var shatter_particle = preload("res://Scenes/Minigames/Fishing3/shatter_particle.tscn")
onready var shatter_particle_solo = preload("res://Scenes/Minigames/Fishing3/shatter_particle_solo.tscn")

signal _player_click

func _ready():
	$bars / ColorRect.rect_size = Vector2(1920, 1080)
	
	
	
	
	
	rod_type = params["rod_type"]
	
	randomize()
	main_offset = - 1000.0
	main.rect_position.x = - 1000.0
	reel.visible = false
	reel.set_as_toplevel(true)
	
	center.modulate = Color("#747474")
	cntdn.visible = false
	
	yield (get_tree().create_timer(0.3), "timeout")
	reel.visible = true
	main.visible = true
	offset_active = true
	anim.play("start")
	
	
	var diff = difficulty
	print("DIFFICULTY: ", diff, " ______________________________________")
	
	var yank_spots = min(floor(diff / 2.2) + 1, 5)
	yank_spots = clamp(ceil(rand_range(1, yank_spots)), 1, 6)
	var total_hp = ceil((diff * diff * 0.35) * 2)
	
	var last_spot = 0
	for i in yank_spots:
		last_spot += 10 + randi() % int(60 / yank_spots)
		last_spot = clamp(last_spot, 5, 75)
		var health = ceil(rand_range(0.5, 1.2) * (total_hp / yank_spots))
		_create_yank_spot(last_spot, health)
	
	var rod_speed_mult = 1.0 + params["speed"]
	if rod_type == "quick":
		params["damage"] = ceil(params["damage"] * 1.15)
		rod_speed_mult = 1.25
	
	thrash_max = 280.0 - (14.0 * diff)
	thrash_min = 100.0 - (3.6 * diff)
	if diff < 2.0:
		thrash_min = 99999.0
	reel_speed = max((0.25 - ((diff) * 0.001)), 0.09) * rod_speed_mult * reel_mult
	bad_speed = max(0.15 - ((diff) * 0.001), 0.1)
	
	thrash_cd = 999
	thrash_timer = - 1
	bad_progress = - 20.0
	
	var stars = params["quality"]
	for i in stars:
		var t = TextureRect.new()
		t.texture = preload("res://Assets/Textures/UI/Fishing3Minigame/star.png")
		$bars / Control / stars.add_child(t)
	
	
	var idata = Globals.item_data[params["fish"]]["file"]
	var t = 1.0 + (0.25 * idata.tier)
	var w = idata.loot_weight
	var value = pow(5 * t, 2.5 - w)
	value = value * PlayerData.QUALITY_DATA[params["quality"]]["worth"]
	
	if w < 0.4: value *= 1.1
	if w < 0.15: value *= 1.25
	if w < 0.05: value *= 1.5
	challenge_star_worth = ceil(value * 0.04)
	challenge_star_timer = randi() % 120 + 60
	
	yield (get_tree().create_timer(0.5), "timeout")
	cntdn.visible = true
	
	challenge_stars_active = rod_type == "challenge"
	
	if rod_type == "patient":
		cntdn.texture = preload("res://Assets/Textures/UI/Fishing3Minigame/cntdn6.png")
		yield (self, "_player_click")
	
	cntdn.texture = preload("res://Assets/Textures/UI/Fishing3Minigame/cntdn1.png")
	yield (get_tree().create_timer(0.4), "timeout")
	cntdn.texture = preload("res://Assets/Textures/UI/Fishing3Minigame/cntdn2.png")
	yield (get_tree().create_timer(0.4), "timeout")
	cntdn.texture = preload("res://Assets/Textures/UI/Fishing3Minigame/cntdn3.png")
	yield (get_tree().create_timer(0.4), "timeout")
	cntdn.visible = false
	
	center.modulate = Color("#ffffff")
	active = true

func _physics_process(delta):
	
	
	var reeling = reel_button.pressed and active
	if active:
		if Input.is_action_pressed("primary_action") and not reeling:
			_on_Button_pressed()
			yield(get_tree().create_timer(0.5), "timeout") # time out so this isnt that overpowered
		if Input.is_action_pressed("primary_action"):
			reeling = true
	
	if Input.is_action_just_pressed("primary_action"): emit_signal("_player_click")
	
	var reel_sound = false
	
	
	if reeling and main_progress < end_goal and active and thrash_timer <= 0:
		if not at_yank: reel_sound = true
		
		if yank_spots.size() > 0 and main_progress > yank_spots[0]:
			main_progress += 0.0
			at_yank = true
			
			var ys = $main / CenterContainer / texmain / yankzones.get_child(0)
			yank_warning.rect_global_position = ys.rect_global_position + Vector2(96, - 76)
			
		else :
			_yank( - 1)
			var vib_amt = 0.9 * (main_progress / end_goal)
			if vibrate_amt < vib_amt: vibrate_amt = vib_amt
			main_progress += reel_speed
			recent_reel = 100
			
			if main_progress >= end_goal:
				_yank( - 10)
				_reached_end(true)
	
	
	if active:
		bad_progress += bad_speed
		var dist = (1.0 - min(1.0, abs(main_progress - bad_progress) / 10.0))
		_yank(dist)
		if bad_progress > main_progress and not killed:
			_kill()
			_reached_end(false)
	
	$sfx / reeling.volume_db = lerp($sfx / reeling.volume_db, linear2db(0.14 if reel_sound else 0.01), 0.1)
	if not $sfx / reeling.playing and $sfx / reeling.volume_db > - 38: $sfx / reeling.playing = true
	if $sfx / reeling.playing and $sfx / reeling.volume_db <= - 38: $sfx / reeling.playing = false
	
	
	if active:
		
		
		
		if thrash_cd <= 0 and main_progress < (end_goal - 5):
			thrash_timer = 60
			thrash_fail = true
			thrash_cd = randi() % int(thrash_max) + thrash_min
			scroll.texture_progress = preload("res://Assets/Textures/UI/Fishing3Minigame/bg3.png")
		
		thrash_timer -= 1
		if thrash_timer > 0:
			recent_yank = 0
			_yank(3.5)
			vibrate_amt = 8.0
		if thrash_timer <= 0 and thrash_fail:
			thrash_fail = false
			var tween = get_tree().create_tween()
			tween.tween_property(self, "main_progress", main_progress - 15.0, 0.3)
			_thrash_end()
	else :
		thrash_fail = false
		thrash_timer = - 1
	
	
	if challenge_stars_active and active:
		challenge_star_timer -= 1
		if challenge_star_timer <= 0:
			challenge_star_timer = randi() % 180 + 60
			
			var star = preload("res://Scenes/Minigames/Fishing3/challenge_star.tscn").instance()
			star.value = challenge_star_worth
			star.rect_global_position = $main / challenge_points.get_children()[randi() % $main / challenge_points.get_child_count()].rect_global_position - Vector2(64, 64)
			
			
			
			add_child(star)
			star.connect("_expired", self, "_star_expire")
	
	main_progress = clamp(main_progress, 0.0, end_goal)
	
	
	if reeling: reel_pivot.rect_rotation += 12
	reel.rect_global_position = reel_point.rect_global_position - Vector2(36, 38)
	vibrate_amt = lerp(vibrate_amt, 0.0, 0.1)
	scroll.value = main_progress
	scroll_bad.value = bad_progress
	if at_yank: $main / CenterContainer / texmain / yank_button.modulate.a = 0.7 + sin(OS.get_ticks_msec() * 0.008) * 0.3
	else : $main / CenterContainer / texmain / yank_button.modulate.a = 1.0
	
	center.rect_pivot_offset.x = center.rect_size.x / 2.0
	if killed:
		center.rect_pivot_offset.y = (center.rect_size.y / 2.0)
		center.rect_global_position = $flop_body.global_position + (Vector2.ONE * sin(OS.get_ticks_msec() * 0.04) * vibrate_amt)
		center.rect_rotation += kill_torque
	else :
		center.rect_pivot_offset.y = (center.rect_size.y / 2.0) + (center.rect_size.y * 0.25)
		center.rect_position.x = lerp(center.rect_position.x, sin(OS.get_ticks_msec() * 0.04) * vibrate_amt, 0.9)
		center.rect_rotation = lerp(center.rect_rotation, 0.0, 0.2)
	
	main.rect_position.x = lerp(main.rect_position.x, main_offset, 0.08)
	if offset_active: main_offset = 0
	
	
	recent_reel -= 1
	recent_yank -= 1
	reel_warning.visible = active and thrash_timer <= 0 and not at_yank and recent_reel <= 0
	yank_warning.visible = active and (thrash_timer > 0 or at_yank) and recent_yank <= 0
	reel_warning.modulate.a = 0.6 + (sin(OS.get_ticks_msec() * 0.01) * 0.4)
	yank_warning.modulate.a = 0.6 + (sin(OS.get_ticks_msec() * 0.01) * 0.4)

func _create_yank_spot(pos, health):
	var tr = preload("res://Scenes/Minigames/Fishing3/yank_bar.tscn").instance()
	$main / CenterContainer / texmain / yankzones.add_child(tr)
	
	var val = (pos / 100.0)
	
	tr.rect_position.x = 34 - (30 * val)
	tr.rect_size.x = 28.0 + (64 * val)
	tr.rect_size.y = 4 + clamp(ceil(health * 0.7), 8, 26)
	tr.rect_position.y = 550.0 * val
	
	tr.max_health = float(health)
	tr.health = tr.max_health
	
	yank_spots.append(pos)
	
	var hp = preload("res://Scenes/Minigames/Fishing3/yank_hp.tscn").instance()
	hp.pairing = tr
	$main / CenterContainer / texmain.add_child(hp)

func _yank(amt):
	if killed: return 
	center.rect_rotation += amt

func _on_Button_pressed():
	if not at_yank: return 
	
	var ys = $main / CenterContainer / texmain / yankzones.get_child(0)
	ys.health -= params["damage"]
	
	if ys.health <= 0:
		$sfx / break.play()
		$sfx / click.pitch_scale = 3.0
		$sfx / click.play()
		
		yank_spots.remove(0)
		at_yank = false
		
		var particle = shatter_particle.instance()
		add_child(particle)
		particle.global_position = ys.rect_global_position + ys.rect_size / 2.0
		particle.emitting = true
		ys.queue_free()
	
	else :
		var hp_amt = float(ys.health) / float(ys.max_health)
		$sfx / click.pitch_scale = 0.3 + ((1.0 - hp_amt) * 2.0)
		$sfx / click.play()
		
		var particle = shatter_particle_solo.instance()
		add_child(particle)
		particle.global_position = ys.rect_global_position + ys.rect_size / 2.0
		particle.emitting = true
	
	recent_yank = 45
	var m = clamp(1.0 - (ys.health / ys.max_health), 0.05, 1.5)
	_yank( - 6 * m)
	vibrate_amt += 2.0

func _thrash_end():
	thrash_fail = false
	thrash_timer = 0
	scroll.texture_progress = preload("res://Assets/Textures/UI/Fishing3Minigame/bg1.png")

func _kill():
	$flop_body.global_position = center.rect_global_position
	$flop_body.mode = RigidBody2D.MODE_RIGID
	$flop_body.apply_central_impulse(Vector2(rand_range( - 40, 40), rand_range( - 600, - 400)))
	vibrate_amt = 8.0
	kill_torque = rand_range( - 3.4, - 2.6)
	killed = true

func _reached_end(win):
	if over: return 
	over = true
	
	if win:
		$stars.global_position = $main / CenterContainer / texmain / starpoint.rect_global_position
		$stars.emitting = true
		$sfx / win.play()
	else :
		$sfx / fail.play()
		$sfx / fail2.play()
		
	
	anim.play_backwards("start")
	yield (get_tree().create_timer(0.5), "timeout")
	if win:
		offset_active = false
		main_offset = - 1000.0
	
	offset_active = false
	active = false
	if win: yield (get_tree().create_timer(0.5), "timeout")
	else : yield (get_tree().create_timer(1.3), "timeout")
	_end(win)


func _star_expire():
	if not active: return 
	_yank( - 15)
	var tween = get_tree().create_tween()
	tween.tween_property(self, "bad_progress", bad_progress + 20.0, 0.3)
