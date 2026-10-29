# Mejoras de la entrega

Cristopher Iori Lazcano Gutierrez — PGM611

1. **Nivel subterráneo 1-2:** reproducción visual de la cuadrícula del mapa NES, geometría de colisiones a escala de 16 píxeles, obstáculos, plataformas móviles, monedas y enemigos. La salida conduce al acuático.
2. **Nivel acuático 2-2:** disposición visual del mapa original, colisiones de suelo y pilares, natación con impulsos, gravedad reducida, límite superior del agua, Cheep Cheep y Blooper con dos fotogramas, monedas y salida hacia el castillo.
3. **Game Over:** se conservan tres vidas entre escenas; cada muerte descuenta una. Al agotarse se carga una escena independiente con reinicio y salida. No se reinicia la campaña automáticamente.
4. **Castillo de Bowser original:** escenario propio de 152 bloques, cinco plataformas móviles, lava, trampas giratorias, Bowser con desplazamiento y disparos, puente de quince piezas y estrella que activa su retirada. Termina en la escena Victoria.

## Mejoras comunes

- Menú inicial y acceso directo de práctica a los tres niveles añadidos.
- Conservación del 1-1 del proyecto base y progresión entre los cuatro niveles.
- Movimiento, salto, animación del personaje, enemigos y efectos sonoros.
- Acompañamiento y efectos sintetizados por código: salto, natación, monedas, muerte y final.
- Cámara lateral con límites según la relación de aspecto, evitando márgenes fuera del nivel.
- HUD de vidas y monedas, pausa y pantalla completa con F11.
- Herramienta de editor para preparar las escenas y compilar Windows.
- Pruebas automáticas en Play Mode y empaquetado con verificación CRC y SHA-256.

## Alcance de las réplicas

Los niveles clásicos utilizan los mapas documentados por Rick N. Bruns como referencia visual y para su geometría. Los enemigos, las monedas, el audio y algunos comportamientos están adaptados al controlador del proyecto base; no se afirma una emulación exacta del juego NES. El castillo es una propuesta propia, no una réplica de un castillo existente. Procedencias en CREDITOS.md.
