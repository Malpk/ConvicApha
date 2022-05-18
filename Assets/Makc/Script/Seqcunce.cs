using System.IO;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Underworld.Editors;
using UnityEditor;

namespace Underworld
{
    [System.Serializable]
    public class Seqcunce : ISerializationCallbackReceiver
    {
        public List<NodeSetting> Elements;
        public List<string> SaveData = new List<string>();

        [SerializeField] private List<NodeSettingData> _dataSetting;

        public Seqcunce(bool createFirstElement = true)
        {
            Elements = new List<NodeSetting>();
            if (createFirstElement)
            {
                var setting = new BaseNodeSetting();
                _dataSetting = new List<NodeSettingData>();
                Elements.Add(setting);
                setting.Constructor(Vector2.zero);
            }
        }
        public void Load()
        {
            Elements.Clear();
            foreach (var data in _dataSetting)
            {
                var node = DefineMode(data);
                node.NextNode = data.NextNode;
                node.Constructor(data.Type, data.Position);
                Elements.Add(node);
            }
        }
        private NodeSetting DefineMode(NodeSettingData data)
        {
            switch (data.Type)
            {
                case ModeTypeNew.BaseMode:
                    var baseMode = new BaseNodeSetting();
                    baseMode.setting = JsonUtility.FromJson<BaseSetting>(data.Setting);
                    return baseMode;
                case ModeTypeNew.IslandMode:
                    var islandMode = new IslandNodeSetting();
                    islandMode.setting = JsonUtility.FromJson<IslandSetting>(data.Setting);
                    return islandMode;
                case ModeTypeNew.BoxMode:
                    var boxMode = new BoxNodeSetting();
                    boxMode.setting = JsonUtility.FromJson<BoxSetting>(data.Setting);
                    return boxMode;
                case ModeTypeNew.CoruselMode:
                    var coruselMode = new CoruselNodeSetting();
                    coruselMode.setting = JsonUtility.FromJson<CoruselSetting>(data.Setting);
                    return coruselMode;
                case ModeTypeNew.PaternCreater:
                    var paternCreater = new PaternCreaterNodeSetting();
                    paternCreater.setting = JsonUtility.FromJson<PaternCraterSetting>(data.Setting);
                    return paternCreater;
                case ModeTypeNew.TrindetMode:
                    var trindetMode = new TridenNodeSetting();
                    trindetMode.setting = JsonUtility.FromJson<TridetSetting>(data.Setting);
                    return trindetMode;
                case ModeTypeNew.RayMode:
                    var rayMode = new RayNodeSetting();
                    rayMode.setting = JsonUtility.FromJson<RaySetting>(data.Setting);
                    return rayMode; ;
                case ModeTypeNew.ViseMode:
                    var viseMode = new ViseNodeSetting();
                    viseMode.setting = JsonUtility.FromJson<ViseSetting>(data.Setting);
                    return viseMode;
                default:
                    return null;
            }
        }
        public void OnAfterDeserialize()
        {
            Debug.Log("Desirilize");
            _dataSetting.Clear();
            foreach (var element in SaveData)
            {
                _dataSetting.Add(JsonUtility.FromJson<NodeSettingData>(element));
            }
        }

        public void OnBeforeSerialize()
        {
            SaveData.Clear();
            foreach (var element in Elements)
            {
                var data = new NodeSettingData(element.NextNode, element.Position, element.TypeMode, element.SettingSeirilize);
                SaveData.Add(JsonUtility.ToJson(data));
            }
        }
    }
}