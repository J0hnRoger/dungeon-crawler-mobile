using System;
using TMPro;
using UnityEngine;

namespace DungeonCrawler._Project.Scripts.Grid
{
    /// <summary>
    /// Script de gestion de l'UI de l'Exploration
    /// </summary>
    public class GridUIView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _levelNameTxt;

        public void SetLevelName(string levelName)
        {
            _levelNameTxt.text = levelName;
        }
    }
}