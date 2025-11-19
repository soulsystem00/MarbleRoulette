namespace Util.FSM.State
{
    internal class IdleState : BaseState
    {
        private FSMController fSMController;

        public IdleState(FSMController fSMController)
        {
            this.fSMController = fSMController;
        }

        public override void Enter()
        {
            GameManager.Instance.OnIdle();

            UIManager.Instance.OpenUI<UISetting>();
        }

        public override void Execute()
        {
        }

        public override void Exit()
        {
            UIManager.Instance.CloseUI<UISetting>();
        }
    }
}
