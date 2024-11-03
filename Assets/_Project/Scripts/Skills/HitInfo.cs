using UnityEngine;

namespace DungeonCrawler._Project.Scripts.Skills
{
    public class HitInfo
    {
        /// <summary>
        /// Le point d'impact avec un enemy, en World position
        /// </summary>
        public Vector3 HitPosition;
        /// <summary>
        /// Pour l'instant, je me contente de récupérer le nom de la partie touchée 
        /// </summary>
        public string HitZone;

        // Pour l'instant, les zone touchées permettent d'appliquer des augmentation/reduction de damage  
        public float DamageCoefficient = 1;
    }
}