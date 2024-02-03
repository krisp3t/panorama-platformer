using Godot;

public partial class Level : Node3D
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        ResetPlayer();
    }

    private void ResetPlayer()
    {
        var player = GetNode<Player>("Player");
        var spawnPoint = GetNode<Marker3D>("LevelModel/PlayerSpawn");
        player.Velocity = Vector3.Zero;
        player.Position = spawnPoint.Position;
        player.Died += OnPlayerDied;
    }

    private void OnPlayerDied()
    {
        ResetPlayer();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }
}