using UnityEngine;
using UnityEngine.Serialization;

namespace DungeonCrawler._Project.Scripts.Skills.Components
{
    /// <summary>
    /// Composant à positionner sur un collider de l'enemy pour déterminer la zone touchée
    /// </summary>
    public class HitZone : MonoBehaviour
    {
        /// <summary>
        /// Pourcentage de dégat en plus ou en moins si on touche cette zone
        /// </summary>
        [SerializeField] public float DamageCoefficient;

        [SerializeField] public string BodyPart;
    }
}