using UnityEngine;
using System;

namespace Game
{
    public interface IMoveInput
    {
        event Action<Vector2> OnMoveInput;
    }
}