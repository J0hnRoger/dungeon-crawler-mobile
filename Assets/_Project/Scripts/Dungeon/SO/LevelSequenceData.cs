using System.Collections.Generic;
using UnityEngine;

namespace DungeonCrawler._Project.Scripts.Dungeon.SO
{
    /// <summary>
    /// Liste des niveaux disponibles, par ordre 
    /// </summary>
    [CreateAssetMenu(fileName = "New Level Sequence", menuName = "ScriptableObjects/LevelSequence")]
    public class LevelSequenceData : ScriptableObject
    {
        public List<GameObject> Levels;
    }
}