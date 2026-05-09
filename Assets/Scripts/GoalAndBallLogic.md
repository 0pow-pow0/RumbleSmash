# Goal & Ball Logic And Events

``` mermaid
classDiagram

namespace BallLogic {
    
    class Ball {
        >onBallScore
    }
}

namespace GoalLogic { 
    
    class Goal { 
        >!onShieldDamage
        >!onShieldDestroy
    }

    class GoalCollider { 

    }

    class GoalShieldCollider {

    }

    class GoalAnimationManager {

    }
}

MatchManager ..> Goal : Gestisce un po' del behaviour in roundstart 

GoalAnimationManager --> Goal

GoalCollider --> Ball : !onBallScore

GoalShieldCollider --> Goal

GoalShieldCollider --> GoalCollider

note for Goal "E' il centro della logica"

note for Goal "Un po' del behaviour si <br> trova in roundManager e MatchManager"

note for GoalCollider "Funge solo da contenitore dei componenti importanti,<br> chiama eventi, ma non possiede variabili significative nel gameplay"

```