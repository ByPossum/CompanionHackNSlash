using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct WorldStates
{
    public WorldAttribute<Vector3> WA_pointInFrustum;


}

public class WorldAttribute<T>
{
    (bool isActive, T attributeType) tup_state;
    public WorldAttribute(bool _isActive, T _attributeType)
    {
        tup_state = (_isActive, _attributeType);
    }
}