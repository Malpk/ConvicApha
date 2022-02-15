using System.Collections;
using UnityEngine;
using PlayerComponent;
using Underworld;

public class TTile : TernBase
{
    [Header("Game Setting")]
    [SerializeField] private TileSettimg _fire;
    [Header("Component Setting")]
    [SerializeField] private Animator _animator;

    private TernState _state = TernState.Warning;

    public override TernState state => _state;

    private void Start()
    {
        StartCoroutine(Work());
    }
    protected override IEnumerator Work()
    {
        while (true)
        {
            _animator.SetInteger("State", 1);
            soundSource.Play();
            var fire = InstatiateFire(_fire.tile);
            _state = TernState.Fire;
            yield return new WaitUntil(() => (fire == null));
            soundSource.Stop();
            _state = TernState.Warning;
            _animator.SetInteger("State", 0);
            yield return new WaitForSeconds(_fire.timeActive);
        }
    }

    protected override void Damage(Player player)
    {
        if (_state == TernState.Fire)
            player.Incineration();
    }

    protected override void Intializate()
    {
    }
}


