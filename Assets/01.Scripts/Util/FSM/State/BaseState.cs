using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Util.FSM.State
{
    public abstract class BaseState
    {
        public abstract void Enter();
        public abstract void Execute();
        public abstract void Exit();
    }
}