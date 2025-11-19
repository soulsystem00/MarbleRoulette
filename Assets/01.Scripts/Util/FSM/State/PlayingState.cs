namespace Util.FSM.State
{
    internal class PlayingState : BaseState
    {
        private FSMController fSMController;

        public PlayingState(FSMController fSMController)
        {
            this.fSMController = fSMController;
        }

        public override void Enter()
        {
            GameManager.Instance.Play();
        }

        public override void Execute()
        {
        }

        public override void Exit()
        {
        }
    }
}
