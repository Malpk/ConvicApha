using UnityEngine;
using System.Collections;

public class PlayerMovement 
{
    private readonly Player player;
    private readonly Rigidbody2D rigidBody;

    public PlayerMovement(Player player,Rigidbody2D rigidBody)
    {
        this.player = player;
        this.rigidBody = rigidBody;
    }

    public void Move(Vector2 move)
    {
        rigidBody.velocity = move;
    }
    public void Rotate(Vector2 direction ,float speedRotation)
    {
            Quaternion rotation = Quaternion.Lerp(player.Rotation, Quaternion.LookRotation(Vector3.forward, direction), speedRotation * Time.deltaTime);
            rigidBody.MoveRotation(rotation);
    }
}
