using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    /**
     * <summary>
     * Interface for objects that can take damage.
     * </summary>
     */
    public interface IDamageable
    {
        void TakeDamage();
    }
}
