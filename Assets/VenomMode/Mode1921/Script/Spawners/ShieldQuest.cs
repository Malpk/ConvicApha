using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

namespace MainMode.Mode1921
{
    public class ShieldQuest : GeneralSpawner
    {
        [Header("Spawner Setting")]
        [SerializeField] private float _distanceBothItems;
        [SerializeField] private string[] _toolLoadKey;
        [SerializeField] private ChangeTest _changeTest;

        private bool _isReady;
        private string _shieldLoadKey = "Shield1921";

        private Shield _shieldAsset;
        private ToolRepairs[] _toolsAssets;

        private Shield[] _shields;
        private ToolRepairs[] _tools;

        public System.Action ConpliteQuestAction;

        public override bool IsRedy => _isReady;

        private async void Awake()
        {
            var shieldLoadTask = LoadAssetAsync<Shield>(_shieldLoadKey);
            var toolsLoadTask = LoadAssetsAsync<ToolRepairs>(_toolLoadKey);
            await Task.WhenAll(shieldLoadTask, toolsLoadTask);
            _shieldAsset = shieldLoadTask.Result;
            _toolsAssets = toolsLoadTask.Result.ToArray();
            _isReady = true;
        }

        public void Intializate(ChangeTest chelangeTest)
        {
            _changeTest = chelangeTest;
        }

        private void OnEnable()
        {
            PlayAction += SpawnItem;
        }

        private void OnDisable()
        {
            PlayAction -= SpawnItem;
        }
        public override void Replay()
        {
            SetPosition();
        }

        private void SpawnItem()
        {
            _tools = new ToolRepairs[_toolsAssets.Length];
            _shields = new Shield[_toolsAssets.Length];
            for (int i = 0; i < _toolsAssets.Length; i++)
            {
                _shields[i] = CreateItem<Shield>(_shieldAsset.gameObject);
                _tools[i] = CreateItem<ToolRepairs>(_toolsAssets[i].gameObject);
                BindShield(_shields[i]);
            }
            SetPosition();
        }

        private T CreateItem<T>(GameObject asset) where T:SmartItem
        {
            var item = Instantiate(asset).GetComponent<T>();
            item.ShowItem();
            item.transform.parent = transform;
            return item;
        }
        private void SetPosition()
        {
            Debug.Log(_tools);
            SetDistribution(_tools, _distanceBothItems,
                SetDistribution(_shields, _distanceBothItems));
        }

        private List<Point> SetDistribution<T>(T[] items, float distance, List<Point> fromItems = null) where T : SmartItem
        {
            int index = 0;
            if (fromItems == null)
                fromItems = new List<Point>();
            while (distance > 0 && index < items.Length)
            {
                var point = GetPoint(fromItems, distance);
                if (point != null)
                {
                    point.SetItem(items[index]);
                    fromItems.Add(point);
                    index++;
                }
                else
                {
                    distance--;
                }
            }
            return fromItems;
        }

        private Point GetPoint(List<Point> fromItems, float distance)
        {
            var list = new List<Point>();
            mapGrid.GetFreePoints(out List<Point> points);
            foreach (var point in points)
            {
                if (CheakPoint(point, fromItems, distance))
                    list.Add(point);
            }
            if (list.Count > 0)
                return list[Random.Range(0,list.Count)];
            return null;
        }
        private bool CheakPoint(Point point, List<Point> fromItems,float distance)
        {
            foreach (var item in fromItems)
            {
                if (Vector2.Distance(point.Position, item.Position) < distance)
                    return false;
            }
            return true;
        }
        private Shield BindShield(Shield shield)
        {
            shield.Intializate(_changeTest);
            shield.RepairShieldAction += UpdateQuest;
            return shield;
        }

        private void UpdateQuest()
        {
            foreach (var shield in _shields)
            {
                if (!shield.IsRepair)
                    return;
            }
            if (ConpliteQuestAction != null)
                ConpliteQuestAction();
        }
    }
}
