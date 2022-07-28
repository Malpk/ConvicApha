using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainMode.Effects;

namespace MainMode.Items
{
    public class Balonchik : Artifact
    {
        [SerializeField] private float _timeActive;
        [SerializeField] private AirJet perfab;

        public override void Use()
        {
            var jet = Instantiate(perfab, user.transform.position, user.transform.rotation).GetComponent<AirJet>();
            user.AddEffects(jet, _timeActive);
            jet.Enter(user.GetComponent<Rigidbody2D>());
        }
    }
}