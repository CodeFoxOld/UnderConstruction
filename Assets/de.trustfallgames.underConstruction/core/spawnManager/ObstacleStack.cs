using de.trustfallGames.underConstruction.core.tilemap;
using UnityEngine;

public class ObstacleStack : MonoBehaviour {
    [SerializeField] private ObstacleType obstacleType;
    [SerializeField] private Material[] materials;
    [SerializeField] private Mesh mesh;

    public ObstacleType ObstacleType { get => obstacleType; set => obstacleType = value; }
    public Material[] Materials => materials;
    public Mesh Mesh => mesh;
}