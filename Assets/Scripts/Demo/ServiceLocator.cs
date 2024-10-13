using TerrainSystem;
using TerrainSystem.View;
using UnityEngine;

namespace Demo
{
    public class ServiceLocator : MonoBehaviour
    {
        public static ServiceLocator Instance;

        public TerrainConfig terrainConfig;
        public ITerrainView terrainView;
        public ITerrainController terrainController;

        private void Awake()
        {
            Instance = this;
            terrainConfig = new();
            terrainView = FindFirstObjectByType<TerrainView>();
            terrainController = new TerrainController(terrainConfig, terrainView);
        }
    }
}
