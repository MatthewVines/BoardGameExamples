using System.Collections.Generic;

namespace BoardGameExamples.Core
{
    public interface IGameState
    {
        bool IsTerminal();
        IEnumerable<IGameState> GetChildStates(bool isMaxPlayerTurn);
    }
}