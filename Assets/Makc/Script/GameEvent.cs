using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameEvent : MonoBehaviour
{
    [Header("Perfab Setting")]
    [SerializeField] private int _id;

    [SerializeField] protected LvlSate state = LvlSate.Pause;
    [SerializeField] protected TypeGameEvent typeEvent = TypeGameEvent.MainMenu;

    public delegate void Commands();
    public abstract event Commands DeadAction;
    public abstract event Commands WinAction;
    public abstract event Commands StartAction;
    public abstract event Commands StopAction;

    private static Dictionary<int, GameEvent> _objetToScene = new Dictionary<int, GameEvent>();

    public LvlSate State => state;
    public TypeGameEvent TypeEvent => typeEvent;

    private void Awake()
    {
        if (_objetToScene.ContainsKey(_id))
        {
            typeEvent = _objetToScene[_id].TypeEvent;
            state = _objetToScene[_id].State;
            _objetToScene.Remove(_id);
        }
        _objetToScene.Add(_id, this);
    }
}
