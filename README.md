# VBL Smart City Simulation – Interactive Urban Mobility

This is the project done for a challenge to create a frogger like in Unity game where the environment conditions are obtained through HTTP requests.

## How to Play

[Play the game in your browser here](https://yamartim.github.io/desafio-vbl/) or download the linux binary in the releases section and run it.

In order to play the game, the "VBL Traffic & Weather API" is required to be running.

To simulate the API, the recommended software is Mockoon with the following openAPI import:

```yaml
openapi: 3.0.0
info:
  title: VBL Traffic & Weather API
  version: 1.0.0
paths:
  /v1/traffic/status:
    get:
      summary: Retorna o estado atual e as predições de tráfego/clima
      responses:
        '200':
          description: OK
          content:
            application/json:
              schema:
                $ref: '#/components/schemas/TrafficResponse'
components:
  schemas:
    TrafficResponse:
      type: object
      properties:
        current_status:
          $ref: '#/components/schemas/Status'
        predicted_status:
          type: array
          items:
            type: object
            properties:
              estimated_time:
                type: integer
                description: Milissegundos para o evento acontecer
              predictions:
                $ref: '#/components/schemas/Status'
    Status:
      type: object
      properties:
        vehicleDensity:
          type: number
          format: float
          minimum: 0.1
          maximum: 1.0
        averageSpeed:
          type: number
          format: float
          description: Velocidade em km/h (0 a 100)
        weather:
          type: string
          enum: [sunny, clouded, foggy, light rain, heavy rain]
```

It is recommended to run the api at the address `http://localhost:3002/` as this is the default configured in the game. 

You can also set a different address and check if the API is reachable and working as the game expects in the main menu.

## How to edit and build the game in Unity

1. Install Unity version 6000.4.0f1

2. Through the Unity editor package manager, download the following packages
    - **Cinemachine** through the Unity registry
    - **DOTween** through the Unity asset store, it's necessary to add the package into your account first through its [page in the asset store](https://assetstore.unity.com/packages/tools/animation/dotween-hotween-v2-27676)
