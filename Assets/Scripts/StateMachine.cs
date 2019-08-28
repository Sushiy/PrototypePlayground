using UnityEngine;

public class StateMachine<T>
{
    public class State
    {
        internal T p; //parent

        public virtual void Enter() { }
        public virtual void Update() { }
        public virtual void Exit() { }
        
        public State(T r)
        {
            p = r;
        }

        public void Init(T r)
        {
            p = r;
        }
    }

    public State state;

    public StateMachine()
    {
        SetState(null);
    }

    public StateMachine(State initial)
    {
        SetState(initial);
    }

    public void SetState(State state)
    {
        if (this.state == state)
        {
            Debug.LogWarning( "State is already entered! " + state + " was set again.");
            return;
        }
        
        if(this.state != null)
        {
            this.state.Exit();
        }
        
        if(state != null)
        {
            state.Enter();
        }            
        this.state = state;
    }

    public void Update()
    {
        if(state != null)
        {
            state.Update();
        }            
    }
}