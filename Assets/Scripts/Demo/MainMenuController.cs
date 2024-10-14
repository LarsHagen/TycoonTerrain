using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Demo
{
    public class MainUIController : MonoBehaviour
    {
        [SerializeField] private TerrainManipulation terrainManipulation;

        public VisualElement root { get; private set; }
        public Button RaiseTerrainButton { get; private set; }
        public Button LowerTerrainButton { get; private set; }
        public Button PaintButton { get; private set; }

        public IntegerField IntegerFieldSurfaceX { get; private set; }
        public IntegerField IntegerFieldSurfaceY { get; private set; }

        public IntegerField IntegerFieldSidesX { get; private set; }
        public IntegerField IntegerFieldSidesY { get; private set; }

        private void Awake()
        {
            root = GetComponent<UIDocument>().rootVisualElement;
            RaiseTerrainButton = root.Q<Button>("ButtonRaise");
            LowerTerrainButton = root.Q<Button>("ButtonLower");
            PaintButton = root.Q<Button>("Paint");

            IntegerFieldSurfaceX = root.Q<IntegerField>("SurfaceMaterialX");
            IntegerFieldSurfaceY = root.Q<IntegerField>("SurfaceMaterialY");
            IntegerFieldSidesX = root.Q<IntegerField>("SidesMaterialX");
            IntegerFieldSidesY = root.Q<IntegerField>("SidesMaterialY");

            RaiseTerrainButton.clicked += ClickedRaiseTerrainButton;
            LowerTerrainButton.clicked += ClickedLowerTerrainButton;
            PaintButton.clicked += ClickedPaint;
        }

        private void ClickedLowerTerrainButton()
        {
            PaintButton.RemoveFromClassList("ButtonSelected");
            RaiseTerrainButton.RemoveFromClassList("ButtonSelected");
            LowerTerrainButton.AddToClassList("ButtonSelected");
            terrainManipulation.tool = TerrainManipulation.Tool.LowerTerrain;
        }

        private void ClickedRaiseTerrainButton()
        {
            PaintButton.RemoveFromClassList("ButtonSelected");
            LowerTerrainButton.RemoveFromClassList("ButtonSelected");
            RaiseTerrainButton.AddToClassList("ButtonSelected");
            terrainManipulation.tool = TerrainManipulation.Tool.RaiseTerrain;
        }

        private void ClickedPaint()
        {
            RaiseTerrainButton.RemoveFromClassList("ButtonSelected");
            LowerTerrainButton.RemoveFromClassList("ButtonSelected");
            PaintButton.AddToClassList("ButtonSelected");
            terrainManipulation.tool = TerrainManipulation.Tool.Paint;
        }
    }
}