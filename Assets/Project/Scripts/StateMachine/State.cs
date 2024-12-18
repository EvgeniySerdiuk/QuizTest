using System;
using System.Threading;
using System.Threading.Tasks;
using Project.Scripts.EventBuss;

namespace Project.Scripts.StateMachine
{
    public struct ExitState<T> 
    {
    }

    public struct StartState<T>
    {
    }
    
    public abstract class State : IState
    {
        public abstract Type NextState { get; }

        public bool IsCompleted { get; private set; }
        public bool IsExited { get; private set; }

        protected readonly IEventBuss EventBuss;

        protected State(IEventBuss eventBuss)
        {
            EventBuss = eventBuss;
        }

        protected void Complete()
        {
            IsCompleted = true;
        }

        public void PostExit()
        {
            IsExited = true;
        }
        
        public abstract Task Enter(CancellationToken cancellationToken);
        public abstract Task Exit(CancellationToken cancellationToken);
    }
}