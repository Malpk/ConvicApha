using UnityEngine;

public class JetImpulseSet : MonoBehaviour
{
    [Min(1)]
    [SerializeField] private float _force = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Rigidbody2D body))
        {
            body.AddForce((Vector2)collision.transform.up * (-_force), ForceMode2D.Impulse);
        }
    }
}
