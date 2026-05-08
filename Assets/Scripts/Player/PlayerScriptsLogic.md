```mermaid
classDiagram

class PlayerParticleManager {

}
class Player {

}

class PlayerJump {

}

class PlayerBallInteractions {

}

class PlayerGroundCheck { 

}

class PlayerBallCollider {
    Comunica con ball
}








PlayerGroundCheck ..> PlayerJump : trigghera event >onLand
PlayerGroundCheck --o Player

PlayerJump --o Player
PlayerBallInteractions --o Player

Player --o PlayerJump
Player --o PlayerBallInteractions

PlayerBallInteractions --> PlayerBallCollider



note for PlayerParticleManager "Si iscrive agli eventi di praticamente tutti"


```