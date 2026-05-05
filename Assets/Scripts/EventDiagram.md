# Event Diagram
Fa capire come vengono triggherati certi eventi e chi vi si iscrive.
- ">": variabile evento
- "!": invoca 
- ">!": crea variabile e invoca nella stessa classe
- ">>": si iscrive a

``` mermaid
classDiagram



%% ---------------- Managers
namespace Managers {

    class MatchManager {
        <<Singleton>>
        >!onPreMatchShowRivals
        >!onMatchBegin
        >!onMatchRestart
        >!onMatchEnd
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

class Ball {
    >onBallScore
}



Goal ..> MatchManager : !onPlayer1Score & !onPlayer2Score

Goal ..> Ball : !onBallScore


note for GameManager "Possiede reference di quasi tutto"

note for RoundManager "Sarebbe meglio che facesse <br/> parte di MatchManager per composizione"

%% ------------- UI
namespace UI {
    class UIScoreboard {
        
    }

    class UIRoundAnimations {

    }
    
   

    class UIMatchAnimations {

    }

    class UIPreMatchAnimations {

    }

    class UIInGame {

    }
}

namespace Plr {
    class Player {
        >! onPlayerMoveStart
        >! onPlayerMoveEnd
    }

    class PlayerJump {
        >! onFirstJumpPerformed
        >! onLand
        >! onDoppioScattoStart
        >! onDoppioScattoEnd
    }

    class PlayerBallInteractions {
        >! onKickStart
        >! onKickEnd
        >! onChargeStart
        >! onCharging
        >! onChargeEnd
        >! onBallHit
    }
}


UIInGame ..> PlayerJump : >>onDoppioScattoStart



UIScoreboard ..> MatchManager : >>onPlayer1Score & >>onPlayer2Score

UIRoundAnimations ..> RoundManager : >>onRoundStartCountdown

UIMatchAnimations ..> MatchManager : >>MatchBegin & >>MatchEnd & Chiama MatchRestart()

UIPreMatchAnimations ..> MatchManager : >>onPreMatchShowRivals

```