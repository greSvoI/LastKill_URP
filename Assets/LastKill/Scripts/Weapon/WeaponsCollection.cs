using System.Collections.Generic;
using UnityEngine;

namespace LastKill
{
    [CreateAssetMenu(fileName = "Weapons Collection", menuName = "Weapons Collection")]
    public class WeaponsCollection : ScriptableObject
    {
        List<WeaponsData> weapons = new List<WeaponsData>();
    }
}
