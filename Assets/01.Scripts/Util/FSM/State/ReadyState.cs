using UnityEngine;

namespace Util.FSM.State
{
    public class ReadyState : BaseState
    {
        private const int COUNTDOWN_TIME = 5;

        private FSMController fSMController;
        private UICountDown uiCountDown;

        private float timer = 0f;

        public ReadyState(FSMController fSMController)
        {
            this.fSMController = fSMController;
        }

        public override void Enter()
        {
            uiCountDown = UIManager.Instance.OpenUI<UICountDown>();
            uiCountDown.Init(COUNTDOWN_TIME);

            timer = 0f;
        }

        public override void Execute()
        {
            // Decrease countdown time and set Time on UI

            // If countdown time reaches zero, change state to Playing


            // make a code

            timer += Time.deltaTime;
            int timeLeft = COUNTDOWN_TIME - (int)timer;
            uiCountDown.SetTime(timeLeft);
            if (timer >= COUNTDOWN_TIME)
            {
                fSMController.ChangeState(FSMState.Playing);
            }
        }

        public override void Exit()
        {
            UIManager.Instance.CloseUI<UICountDown>();

            uiCountDown = null;

            timer = 0f;
        }
    }
}