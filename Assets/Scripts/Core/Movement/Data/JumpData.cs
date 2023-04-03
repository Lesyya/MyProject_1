using System;
using UnityEngine;
//using InputReader;
//using Core.Animation;
using Player;
using Core.Movement.Controller;
using Core.Movement.Data;
using Core.Enums;
//using Core.Services.Updater;
using Core.Tools;

namespace Core.Movement.Data
{
    [Serializable]
    public class JumpData
    {
        [field: SerializeField] public float JumpForce { get; private set; }
        [field: SerializeField] public int GravityScale { get; private set; }
    }
}