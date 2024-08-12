class_name TerrainGeneration
extends Node

var mesh: MeshInstance3D
var size_depth : int = 500
var size_width : int = 500
var mesh_resolution : int = 2
var max_height : float = 90
var use_falloff : bool = true

@export var noise : FastNoiseLite 
@export var elevation_curve : Curve 
@export var water_level : float = .2

var falloff_image : Image

@onready var rng : RandomNumberGenerator = RandomNumberGenerator.new()
var spawnable_objects : Array[SpawnableObject]
@onready var water : MeshInstance3D = get_node("Water")

@onready var nav_region : NavigationRegion3D = get_node("NavigationRegion3D")

# Called when the node enters the scene tree for the first time.
func _ready() -> void:
    for i in get_children():
        if i is SpawnableObject:
            spawnable_objects.append(i)

    var falloff_texture = preload("res://Assets/Procedural Generation/Textures/TerrainFalloff.png")
    falloff_image = falloff_texture.get_image()

    noise.seed = randi()
    rng.seed = noise.seed

    generate_mesh()

func generate_mesh():
    var plane_mesh = PlaneMesh.new()
    plane_mesh.size = Vector2(size_width, size_depth)
    plane_mesh.subdivide_depth = size_depth * mesh_resolution
    plane_mesh.subdivide_width = size_width * mesh_resolution
    plane_mesh.material = preload("res://Assets/Procedural Generation/Materials/TerrainMaterial.tres")

    var surface = SurfaceTool.new()
    surface.create_from(plane_mesh, 0)

    var data = MeshDataTool.new()
    var array_plane = surface.commit()
    data.create_from_surface(array_plane, 0)

    for i in range(data.get_vertex_count()):
        var vertex = data.get_vertex(i)
        vertex.y = get_noise_y(vertex.x, vertex.z)
        data.set_vertex(i, vertex)

    array_plane.clear_surfaces()
    data.commit_to_surface(array_plane)

    surface.begin(Mesh.PRIMITIVE_TRIANGLES)
    surface.create_from(array_plane, 0)
    surface.generate_normals()

    mesh = MeshInstance3D.new()
    mesh.mesh = surface.commit()
    mesh.create_trimesh_collision()
    mesh.cast_shadow = GeometryInstance3D.SHADOW_CASTING_SETTING_OFF
    mesh.add_to_group("NavSource")
    add_child(mesh)

    water.position.y = water_level * max_height

    for i in spawnable_objects:
        spawn_objects(i)

    nav_region.bake_navigation_mesh()
    await nav_region.bake_finished

func get_noise_y(x, z) -> float:
    var val = noise.get_noise_2d(x, z)
    var remapped_value = ((val + 1) / 2)
    var adjusted_value = elevation_curve.sample(remapped_value)

    if use_falloff:
        var x_pc = (x + (size_width / 2)) / size_width
        var x_px = int(x_pc * falloff_image.get_width())

        var y_pc = (x + (size_depth / 2)) / size_depth
        var y_px = int(y_pc * falloff_image.get_height())

        return adjusted_value * max_height * falloff_image.get_pixel(x_px, y_px).r

    return adjusted_value *  max_height

func get_random_position() -> Vector3:
    var x = rng.randf_range(-size_width / 2, size_width / 2)
    var z = rng.randf_range(-size_depth / 2, size_depth / 2)
    var y = get_noise_y(x, z)

    return Vector3(x, y , z)

func spawn_objects (spawnable: SpawnableObject):
    for i in range(spawnable.spawn_count):

        var pos = get_random_position()
        while pos.y < water_level * max_height:
            pos = get_random_position()

        var obj = spawnable.scenes_to_spawn[rng.randi() % spawnable.scenes_to_spawn.size()].instantiate()
        obj.add_to_group("NavSource")
        obj.position = pos
        obj.scale = Vector3.ONE * rng.randf_range(spawnable.min_scale, spawnable.min_scale)
        obj.rotation_degrees.y = rng.randf_range(0, 360)

        add_child(obj)