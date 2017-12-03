namespace Burton.Lib.StateMachine
{
    public class TestEntity
    {
        public StateMachine<TestEntity> StateMachine;

        public TestEntity()
        {
            StateMachine = new StateMachine<TestEntity>(this);

            // Newing up states might not be the best approach, but doesn't really matter at this point,
            // since they don't store any local state.
            StateMachine.ChangeState(new TestState());
        }

        public void Update()
        {
            StateMachine.Update();
        }
    }

    public class TestState : State<TestEntity>
    {
        public override void OnEnter(TestEntity Entity)
        {
            base.OnEnter(Entity);
        }

        public override void OnExecute(TestEntity Entity)
        {
            base.OnExecute(Entity);
        }

        public override void OnExit(TestEntity Entity)
        {
            base.OnExit(Entity);
        }
    }

    public class StateMachine<TEntity>
    {
        private TEntity Owner;
        public State<TEntity> CurrentState;
        public State<TEntity> PreviousState;
        public State<TEntity> GlobalState;

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

        public void ChangeState(State<TEntity> NewState)
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

    public class State<TEntity>
    {
        public virtual void OnEnter(TEntity Entity)
        {
        }

        public virtual void OnExecute(TEntity Entity)
        {
        }

        public virtual void OnExit(TEntity Entity)
        {
        }
    }
}
