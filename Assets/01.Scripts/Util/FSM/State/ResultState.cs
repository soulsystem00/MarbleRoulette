using UnityEngine;

namespace Util.FSM.State
{
    internal class ResultState : BaseState
    {
        private FSMController fSMController;
        private UIResult uiResult;

        private float resultDisplayTime = 10f;
        private float timer = 0f;
        public ResultState(FSMController fSMController)
        {
            this.fSMController = fSMController;
        }

        public override void Enter()
        {
            uiResult = UIManager.Instance.OpenUI<UIResult>();
            uiResult.SetWinner(GameManager.Instance.WinnerPlayerName);
        }

        public override void Execute()
        {
            timer += Time.deltaTime;
            if (timer >= resultDisplayTime)
            {
                timer = 0f;
                fSMController.ChangeState(FSMState.Idle);
            }
        }

        public override void Exit()
        {
            UIManager.Instance.CloseUI<UIResult>();
            uiResult = null;
        }
    }
}
