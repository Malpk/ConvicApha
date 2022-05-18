using UnityEngine;
using UnityEditor;
using System.Collections;

namespace Underworld.Editors
{
    #region BaseSetting
    [System.Serializable]
    public class BaseNodeSetting : NodeSetting
    {
        public BaseSetting setting;

        private void Awake()
        {
            this.setting = new BaseSetting();
        }
        public override IModeSetting Setting => setting;

        public override string SettingSeirilize => JsonUtility.ToJson(setting);

        public override void Constructor(Vector2 position)
        {
            Constructor(ModeTypeNew.BaseMode, position);
        }

        //public void OnBeforeSerialize()
        //{
        //    settingSeirilize = SettingSeirilize;
        //}

        //public void OnAfterDeserialize()
        //{
        //    setting = JsonUtility.FromJson<BaseSetting>(settingSeirilize);
        //}
    }
    #endregion
    #region IslandSetting
    [System.Serializable]
    public class IslandNodeSetting : NodeSetting
    {
        public IslandSetting setting;
        public override void Constructor(Vector2 position)
        {
            Constructor(ModeTypeNew.IslandMode, position);
            this.setting = new IslandSetting();
        }
        public override string SettingSeirilize => JsonUtility.ToJson(setting);
        public override IModeSetting Setting => setting;
    }
    #endregion
    #region BoxMode
    [System.Serializable]
    public class BoxNodeSetting : NodeSetting
    {
        public BoxSetting setting;
        public override void Constructor(Vector2 position)
        {
            Constructor(ModeTypeNew.BoxMode, position);
            this.setting = new BoxSetting();
        }
        public override string SettingSeirilize => JsonUtility.ToJson(setting);
        public override IModeSetting Setting => setting;
    }
    #endregion
    #region ViseMode
    [System.Serializable]
    public class ViseNodeSetting : NodeSetting
    {
        public ViseSetting setting;
        public override void Constructor(Vector2 position)
        {
            Constructor(ModeTypeNew.ViseMode, position);
            this.setting = new ViseSetting();
        }
        public override string SettingSeirilize => JsonUtility.ToJson(setting);
        public override IModeSetting Setting => setting;
    }
    #endregion
    #region CoruselMode
    [System.Serializable]
    public class CoruselNodeSetting : NodeSetting
    {
        public CoruselSetting setting;
        public override void Constructor(Vector2 position)
        {
            Constructor(ModeTypeNew.CoruselMode, position);
            this.setting = new CoruselSetting();
        }
        public override IModeSetting Setting => setting;
        public override string SettingSeirilize => JsonUtility.ToJson(setting);
    }
    #endregion
    #region TridenMode
    [System.Serializable]
    public class TridenNodeSetting : NodeSetting
    {
        public TridetSetting setting;
        public override void Constructor(Vector2 position)
        {
            Constructor(ModeTypeNew.TrindetMode, position);
            this.setting = new TridetSetting();
        }
        public override string SettingSeirilize => JsonUtility.ToJson(setting);
        public override IModeSetting Setting => setting;
    }
    #endregion
    #region PaternCreater
    [System.Serializable]
    public class PaternCreaterNodeSetting : NodeSetting
    {
        public PaternCraterSetting setting;
        public override void Constructor(Vector2 position)
        {
            Constructor(ModeTypeNew.PaternCreater, position);
            setting = new PaternCraterSetting();
        }
        public override IModeSetting Setting => setting;
        public override string SettingSeirilize => JsonUtility.ToJson(setting);
    }
    #endregion
    #region RayMode
    [System.Serializable]
    public class RayNodeSetting : NodeSetting
    {
        [SerializeField] public RaySetting setting;
        public override void Constructor(Vector2 position)
        {
            Constructor(ModeTypeNew.RayMode, position);
            setting = new RaySetting();
        }
        public override string SettingSeirilize => JsonUtility.ToJson(setting);
        public override IModeSetting Setting => setting;
    }
    #endregion

    [System.Serializable]
    public abstract class NodeSetting : ScriptableObject
    {
        public int NextNode;
        public Vector2 Position;

        [SerializeField] protected string settingSeirilize;

        public abstract void Constructor(Vector2 position);
        public void Constructor(ModeTypeNew type, Vector2 position)
        {
            TypeMode = type;
            Position = position;
        }
        private void OnDisable()
        {
        }
        private void OnDestroy()
        {
        }
        public ModeTypeNew TypeMode { get; private set; }
        public abstract string SettingSeirilize { get; }
        public abstract IModeSetting Setting {get;}
    }
    [System.Serializable]
    public  class NodeSettingData
    {
        [SerializeField] private int _nextNode;
        [SerializeField] private Vector2 _position;
        [SerializeField] private ModeTypeNew _type;
        [SerializeField] private string _settingSeirilize;

        public NodeSettingData(int nextNode, Vector2 position, ModeTypeNew type, string setting)
        {
            _nextNode = nextNode;
            _position = position;
            _type = type;
            _settingSeirilize = setting;
        }
        public int NextNode => _nextNode;
        public Vector2 Position => _position;
        public ModeTypeNew Type => _type;
        public string Setting => _settingSeirilize;
    }
}