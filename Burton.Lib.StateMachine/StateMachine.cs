namespace Burton.Lib.StateMachine
{
    public class ExampleState : IState<int>
    {
        public void OnEnter(int Entity)
        {
            throw new System.NotImplementedException();
        }

        public void OnExecute(int Entity)
        {
            throw new System.NotImplementedException();
        }

        public void OnExit(int Entity)
        {
            throw new System.NotImplementedException();
        }
    }

    public interface IState<TEntity>
    {
        void OnEnter(TEntity Entity);
        void OnExecute(TEntity Entity);
        void OnExit(TEntity Entity);
    }

    public class StateMachine<TEntity>
    {
        private TEntity Owner;
        public IState<TEntity> CurrentState;
        public IState<TEntity> PreviousState;
        public IState<TEntity> GlobalState;

        public StateMachine(TEntity Owner)
        {
            this.Owner = Owner;
            CurrentState = null;
            PreviousState = null;
            GlobalState = null;
        }

        public void Update()
        {
            if (GlobalState != null)
            {
                GlobalState.OnExecute(Owner);
            }

            if (CurrentState != null)
            {
                CurrentState.OnExecute(Owner);
            }  
        }

        public void ChangeState(IState<TEntity> NewState)
        {
            if (NewState == null)
                return;

            PreviousState = CurrentState;

            if (CurrentState != null)
            {
                CurrentState.OnExit(Owner);
            }

            CurrentState = NewState;
            CurrentState.OnEnter(Owner);
        }

        public void RevertToPreviousState()
        {
            ChangeState(PreviousState);
        }
    }
}
