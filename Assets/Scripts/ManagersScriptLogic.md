# Relazioni fra Managers
```mermaid
classDiagram


%% ---------------- Managers
namespace Managers {
    class GameManager { 
    <<Singleton>>
    }

    class InputManager {
        <<Singleton>>
    }

    class MatchManager {
        <<Singleton>>
        >MatchBegin
        >MatchRestart
        >MatchEnd
        >onPlayer1Score
        >onPlayer2Score
    }

    class RoundManager {
        <<Singleton>>
        >onRoundStartCountdown
        >onRoundStartBeing
        >onRoundUpdate
        >onRoundEnd
    }
}


MatchManager --> RoundManager : gestisce
MatchManager ..> GameManager : utilizza



RoundManager ..> GameManager : utilizza
RoundManager --> InputManager : utilizza

MatchManager ..> InputManager : utilizza

InputManager ..> GameManager

note for GameManager "Possiede reference di quasi tutto"

note for RoundManager "Sarebbe meglio che facesse <br/> parte di MatchManager per composizione"




```