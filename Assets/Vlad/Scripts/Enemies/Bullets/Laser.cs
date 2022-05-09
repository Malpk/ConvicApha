using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : DeviceWithNegativeEffect
{
    [SerializeField]
    private ContactFilter2D _contactFilter2D;
    private void Update()
    {
        List<RaycastHit2D> hit = new List<RaycastHit2D>();
        if (Physics2D.Raycast(transform.position, transform.up, _contactFilter2D, hit, 50f) > 0)
        {
            transform.localScale = new Vector3(1f, hit[0].distance, 1f);
            EffectsHandler effectsHandler = hit[0].rigidbody?.GetComponent<EffectsHandler>();
            if (effectsHandler)
            {
                ActivateDevice(effectsHandler);
            }
        }
    }


}
