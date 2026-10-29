# Super Mario PGM611 — Cristopher Lazcano

Entrega académica en Unity 6000.3.11f1 basada en el proyecto 2D original de Jesusbautistah. Este fork conserva su historial y autores.

**[Descargar el ejecutable Windows](https://github.com/Crisshubb/super-mario-pgm611/releases/tag/v1.0.0)**. Extraer todo el ZIP y ejecutar `Mario-PGM611.exe`; no hace falta Unity para jugar.

## Mejoras agregadas

- Subterráneo 1-2 basado en el mapa original.
- Acuático 2-2, natación y enemigos animados.
- Escena Game Over después de agotar las tres vidas.
- Castillo propio después del agua, con Bowser, puente, trampas y final.
- Sonidos, acompañamiento, cámara lateral, menú, práctica, pausa, HUD y pantalla completa.

Detalles y límites de las réplicas: [MEJORAS.md](MEJORAS.md). Comprobaciones realizadas: [PRUEBAS.md](PRUEBAS.md). Procedencia del proyecto y los recursos: [CREDITOS.md](CREDITOS.md).

## Controles y ejecución

A/D o flechas: mover. Espacio: saltar/nadar. S: entrar en tuberías del nivel inicial. Esc: pausa. F11: pantalla completa.

Ruta de campaña: **1-1 > 1-2 > 2-2 > Castillo > Victoria**. En el castillo, alcanza la estrella detrás de Bowser para activar el interruptor del puente.

Para editar, clonar y abrir la raíz de este repositorio en Unity 6000.3.11f1. Abrir `Assets/Scenes/Menu.unity`. **Entrega > Preparar niveles** regenera las escenas añadidas; **Entrega > Compilar Windows** genera el ejecutable en `../Entrega/Windows`. `Herramientas/empaquetar.py` crea el ZIP y su SHA-256.

## Referencia original

> Super Mario Bros is a platformer game developed and published by Nintendo for the NES game console. Players control Mario as they traverse the Mushroom Kingdom to rescue Princess Peach from Bowser. They traverse side-scrolling stages while avoiding hazards such as enemies and pits with the aid of power-ups such as the Super Mushroom, Fire Flower, and Starman.

- **Topics**: Physics, Level Design, Game Feel
- **Version**: Unity 2021.3 (LTS)
- [**Download**](https://github.com/zigurous/unity-super-mario-tutorial/archive/refs/heads/main.zip)
- [**Video Playlist**](https://youtube.com/playlist?list=PLqlFiJjSZ2x1mrMpSQgYdRm8PyWRTg6He)
