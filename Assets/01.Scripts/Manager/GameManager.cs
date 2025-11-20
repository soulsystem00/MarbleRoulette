using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Util.FSM.State;

[System.Serializable]
public struct MapInfo
{
    public MapType mapType;
    public GameObject prefab;
}

public class GameManager : MonoSingleton<GameManager>
{
    const float X_Spacing = 0.75f;
    const float Y_Spacing = 1f;

    [SerializeField] private WinType winType = WinType.FirstToWin;
    [SerializeField] private FSMController fsmController;
    [SerializeField] private CameraController cameraController;
    [SerializeField] private GameObject spawnPoint;
    [SerializeField] private GameObject mapSpawnPoint;
    [SerializeField] private List<MapInfo> mapList;

    private List<Player> players = new List<Player>();
    private List<string> goalPlayers = new List<string>();
    private List<KeyValuePair<string, int>> playerList = new List<KeyValuePair<string, int>>();
    private MapType currentMapType = MapType.Map1;
    private GameObject currentMap = null;

    private string winnerPlayerName = string.Empty;
    public string WinnerPlayerName => winnerPlayerName;

    protected override void Init()
    {
        if (players == null)
        {
            players = new List<Player>();
        }

        if (goalPlayers == null)
        {
            goalPlayers = new List<string>();
        }

        if (fsmController == null)
        {
            fsmController = GetComponent<FSMController>();
        }

        if (playerList == null)
        {
            playerList = new List<KeyValuePair<string, int>>();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        fsmController.ChangeState(FSMState.Idle);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnIdle()
    {
        ClearAllPlayers();
        UIManager.Instance.CloseUI<UIRanking>();

        cameraController.IsFollowing = false;

        players.Clear();
        goalPlayers.Clear();
        winnerPlayerName = string.Empty;
        MapSetting(MapType.Map1);
    }

    public void SpawnPlayers(string data)
    {
        if (string.IsNullOrEmpty(data))
        {
            ClearAllPlayers();

            return;
        }

        List<KeyValuePair<string, int>> curPlayerList = new List<KeyValuePair<string, int>>();
        string[] playerRawData = data.Split(',').Select(x => x.Trim()).ToArray();

        foreach (var item in playerRawData)
        {
            string[] playerData = item.Split('*').Select(x => x.Trim()).ToArray();

            if (playerData.Length < 2)
            {
                return;
            }
            else
            {
                string playerName = playerData[0].Trim();
                if (int.TryParse(playerData[1], out var playerCount))
                {
                    var index = curPlayerList.FindIndex(x => x.Key == playerName);

                    if (index == -1)
                    {
                        KeyValuePair<string, int> player = new KeyValuePair<string, int>(playerName, playerCount);

                        Debug.Log($"Player Add / Name : {player.Key}, Value : {player.Value}");
                        curPlayerList.Add(player);
                    }
                    else
                    {
                        var temp = curPlayerList[index];
                        temp = new KeyValuePair<string, int>(temp.Key, temp.Value + playerCount);
                        curPlayerList[index] = temp;
                    }
                }
                else
                {
                    return;
                }
            }
        }

        if (PlayerListComparer.AreEqual(playerList, curPlayerList))
        {
            return;
        }

        ClearAllPlayers();

        int playerIndex = 0;

        foreach (var item in curPlayerList)
        {
            for (int i = 0; i < item.Value; i++)
            {
                var spawnPosition = GetPlayerPosition(playerIndex);

                var player = ObjectPoolManager.Instance.Instantiate<Player>(PoolType.Player, spawnPosition, Quaternion.identity, this.transform);
                player.SetPlayerName($"{item.Key}#{i + 1}");
                player.SetPlayerColor(UniqueColorGenerator.GetUniqueColor());
                players.Add(player);
                playerIndex++;
            }
        }

        playerList = curPlayerList;
    }

    public void ShufflePlayers()
    {
        players.Shuffle();

        for (int i = 0; i < players.Count; i++)
        {
            players[i].transform.position = GetPlayerPosition(i);
        }
    }

    public void MapSetting(MapType mapType)
    {
        if (currentMapType == mapType && currentMap != null)
        {
            return;
        }

        if (currentMap != null)
        {
            Destroy(currentMap);
        }
        var prefab = mapList.Where(x => x.mapType == mapType).Select(x => x.prefab).FirstOrDefault();

        if (prefab == null)
        {
            Debug.LogError($"Map Prefab is Null : {mapType}");
            return;
        }

        currentMapType = mapType;
        currentMap = Instantiate(prefab, mapSpawnPoint.transform.position, Quaternion.identity, mapSpawnPoint.transform);
        currentMap.name = mapType.ToString();
    }

    public void SetWinType(WinType type)
    {
        winType = type;
        cameraController.WinType = type;
    }

    public void Ready()
    {
        fsmController.ChangeState(FSMState.Ready);
    }

    private Vector3 GetPlayerPosition(int playerIndex, int rowCount = 5)
    {
        // rowCount: 한 줄에 몇 개 배치할지
        int row = playerIndex / rowCount;
        int col = playerIndex % rowCount;

        float x = spawnPoint.transform.position.x + col * X_Spacing;
        float y = spawnPoint.transform.position.y + row * Y_Spacing;

        return new Vector3(x, y);
    }

    public void Play()
    {
        UIManager.Instance.OpenUI<UIRanking>();

        foreach (var player in players)
        {
            player.EnableControl();
        }

        cameraController.IsFollowing = true;
    }

    public void Goal(Player player)
    {
        players.Remove(player);
        goalPlayers.Add(player.GetPlayerName());

        ObjectPoolManager.Instance.Release(PoolType.Player, player.gameObject);

        if (winType == WinType.FirstToWin)
        {
            // 첫 번째 골을 기록한 플레이어가 승리
            if (goalPlayers.Count == 1)
            {
                Debug.Log($"Winner! : {goalPlayers[0]}");
                winnerPlayerName = goalPlayers[0];
                fsmController.ChangeState(FSMState.Result);
            }
        }
        else
        {
            // 마지막 남은 플레이어가 승리
            if (players.Count == 1)
            {
                Debug.Log($"Winner! : {players[0].GetPlayerName()}");
                winnerPlayerName = players[0].GetPlayerName();
                fsmController.ChangeState(FSMState.Result);
            }
        }

        if (UIManager.Instance.IsOpenUI<UIRanking>() == true)
        {
            var targetUI = UIManager.Instance.GetUI<UIRanking>();

            targetUI.UpdateRankingText(goalPlayers);
        }
    }

    public Player GetTargetPlayer(WinType winType)
    {
        Player targetPlayer = null;

        if (winType == WinType.FirstToWin)
        {
            targetPlayer = players.OrderBy(x => x.transform.position.y).FirstOrDefault();
        }
        else
        {
            targetPlayer = players.OrderByDescending(x => x.transform.position.y).FirstOrDefault();
        }

        return targetPlayer;
    }

    private void ClearAllPlayers()
    {
        for (int i = 0; i < players.Count; i++)
        {
            ObjectPoolManager.Instance.Release(PoolType.Player, players[i].gameObject);
        }

        players.Clear();
    }
}