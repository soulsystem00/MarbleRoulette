namespace Util.FSM.State
{
    public class PauseState : BaseState
    {
        private FSMController fSMController;

        public PauseState(FSMController fSMController)
        {
            this.fSMController = fSMController;
        }

        public override void Enter()
        {
        }

        public override void Execute()
        {
        }

        public override void Exit()
        {
        }
    }
}

