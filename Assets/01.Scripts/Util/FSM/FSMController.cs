using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Util.FSM.State
{
    public enum FSMState
    {
        None,
        Idle,
        Ready,
        Playing,
        Pause,
        Result,
    }

    public class FSMController : MonoBehaviour
    {
        [SerializeField] private FSMState state;
        public FSMState State => state;

        private BaseState currentState;

        private Dictionary<FSMState, BaseState> stateDictionary = new Dictionary<FSMState, BaseState>();

        private void Awake()
        {
            Init();
        }

        // Update is called once per frame
        void Update()
        {
            currentState?.Execute();
        }

        private void Init()
        {
            if (stateDictionary == null)
            {
                stateDictionary = new Dictionary<FSMState, BaseState>();
            }

            stateDictionary.Clear();

            stateDictionary.Add(FSMState.Idle, new IdleState(this));
            stateDictionary.Add(FSMState.Ready, new ReadyState(this));
            stateDictionary.Add(FSMState.Playing, new PlayingState(this));
            stateDictionary.Add(FSMState.Pause, new PauseState(this));
            stateDictionary.Add(FSMState.Result, new ResultState(this));

            this.state = FSMState.None;
            this.currentState = null;
        }

        public void ChangeState(FSMState newState)
        {
            if (state == newState)
            {
                return;
            }

            if (!stateDictionary.ContainsKey(newState))
            {
                return;
            }

            currentState?.Exit();
            currentState = stateDictionary[newState];
            currentState?.Enter();
            state = newState;
        }
    }
}