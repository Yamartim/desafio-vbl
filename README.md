# VBL Smart City Simulation – Interactive Urban Mobility

This is a challenge to create a frogger like in Unity game where the environment conditions are obtained through a HTTP requests.

[Play the game in your browser here.](https://yamartim.github.io/desafio-vbl/)

Unity version: 6000.4.0f1

Non built-in Unity packages used:

- DOTween
- Cinemachine

To simulate the API, the software used was Mockoon with the following openAPI import:

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