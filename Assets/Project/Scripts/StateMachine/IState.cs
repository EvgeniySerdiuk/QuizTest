using System;
using System.Threading;
using System.Threading.Tasks;

namespace Project.Scripts.StateMachine
{
    public interface IState
    {
        public Type NextState { get; }
        public bool IsCompleted { get; }
        public bool IsExited { get; }

        public Task Enter(CancellationToken cancellationToken);
        public Task Exit(CancellationToken cancellationToken);
        public void PostExit();
    }
}