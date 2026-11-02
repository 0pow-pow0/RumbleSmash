# Event Flow
Descrive l'ordine di chiamata degli eventi.
Tutto parte
``` mermaid
classDiagram


StartFirstTimeFlowFUNCTION ..> OnPreMatchAssignDevices

OnPreMatchAssignDevices ..>  OnPreMatchShowRivals

OnPreMatchShowRivals ..> OnMatchBegin

OnMatchBegin ..> OnRoundBegin


```