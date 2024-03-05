using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public interface IHurtful
    {
        void Hurt(IDamageable damageable);
    }
}
