using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hero.Abilities
{
    public abstract class Ability : MonoBehaviour
    {
        public abstract IEnumerator Cast();
    }
}
