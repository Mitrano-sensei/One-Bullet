using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    /**
     * <summary>
     * Interface for objects that can hurt other objects.
     * </summary>
     */
    public interface IHurtful
    {
        void Hurt(IDamageable damageable);
    }
}
