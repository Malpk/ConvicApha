using UnityEngine;

namespace Underworld.Editor
{
    #region BaseSetting
    public class BaseNodeSetting : NodeSetting
    {
        [SerializeField] private BaseSetting setting;
        public BaseNodeSetting(Vector2 position)
            : base(ModeTypeNew.BaseMode, position)
        {
            this.setting = new BaseSetting();
        }
        public override object Setting => setting;
    }
    #endregion
    #region IslandSetting
    [System.Serializable]
    public class IslandNodeSetting : NodeSetting
    {
        [SerializeField] private IslandSetting setting;
        public IslandNodeSetting(Vector2 position)
            : base(ModeTypeNew.IslandMode, position)
        {
            this.setting = new IslandSetting();
        }

        public override object Setting => Setting;
    }
    #endregion
    [System.Serializable]
    public abstract class NodeSetting : ScriptableObject
    {
        public Vector2 Position;
        public int NextNode;

        public readonly ModeTypeNew type;
        public NodeSetting(ModeTypeNew type, Vector2 position)
        {
            this.type = type;
            Position = position;
        }
        public abstract object Setting { get; }

    }

}