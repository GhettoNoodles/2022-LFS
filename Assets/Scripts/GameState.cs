using System;
using Newtonsoft.Json;
using UnityEngine;

[Serializable]
public class GameState
{
    [JsonProperty("active_cp")] public int activeCP;
    [JsonProperty("ring_transforms")] public bool[] ringStates;
    [JsonProperty("player_position")] public Vector3 playerPos;
    [JsonProperty("player_velocity")] public Vector3 playerVelocity;
    [JsonProperty("player_rings")] public int playerRings;
    [JsonProperty("player_hp")] public int playerHP;
}