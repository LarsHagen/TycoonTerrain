using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Demo
{
    public class MainUIController : MonoBehaviour
    {
        [SerializeField] private TerrainManipulation terrainManipulation;

        public VisualElement root;
        public Button RaiseTerrainButton;
        public Button LowerTerrainButton;

        private void Awake()
        {
            root = GetComponent<UIDocument>().rootVisualElement;
            RaiseTerrainButton = root.Q<Button>("ButtonRaise");
            LowerTerrainButton = root.Q<Button>("ButtonLower");

            RaiseTerrainButton.clicked += ClickedRaiseTerrainButton;
            LowerTerrainButton.clicked += ClickedLowerTerrainButton;
        }

        private void ClickedLowerTerrainButton()
        {
            terrainManipulation.tool = TerrainManipulation.Tool.LowerTerrain;
        }

        private void ClickedRaiseTerrainButton()
        {
            terrainManipulation.tool = TerrainManipulation.Tool.RaiseTerrain;
        }
    }
}