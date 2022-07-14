using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainMode.LoadScene;

namespace MainMode.Mode1921
{
    public class LoaderMode1921 : MainLoader
    {
        [SerializeField] private Mode1921 _perfab;

        public override void Load(PlayerType choose)
        {
            base.Load(choose);
            var mode = Instantiate(_perfab.gameObject, Vector3.zero, Quaternion.identity).GetComponent<Mode1921>();
            mode.GetComponent<ItemSpwaner>().Run(player.transform);
            var test = holder.GetComponentInChildren<ChangeTest>();
            if (test)
                mode.Intializate(test);
        }
    }
}