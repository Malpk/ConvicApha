using UnityEngine;
using System.Collections;

public class PlayerMovement 
{
    private readonly Character character;
    private readonly Rigidbody2D rigidBody;

    public PlayerMovement(Character character,Rigidbody2D rigidBody)
    {
        this.character = character;
        this.rigidBody = rigidBody;
    }

    public void Move(Vector2 move)
    {
        if (move != Vector2.zero)
            rigidBody.AddForce(move, ForceMode2D.Force);
    }
    public void Rotate(Vector2 direction ,float speedRotation)
    {
            Quaternion rotation = Quaternion.Lerp(character.Rotation, Quaternion.LookRotation(Vector3.forward, direction), speedRotation * Time.deltaTime);
            rigidBody.MoveRotation(rotation);
    }
}
