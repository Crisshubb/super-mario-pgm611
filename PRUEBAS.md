# Verificación de la entrega

Fecha: 29 de octubre de 2026. Editor: Unity 6000.3.11f1, Windows.

Dos pasadas consecutivas de `PruebasEntrega.Ejecutar` aprobó 20 comprobaciones en Play Mode sobre las escenas reales:

- Menú, tres vidas iniciales y siete escenas incluidas en la compilación.
- Conservación del 1-1 y descuento de vidas al reiniciar.
- Carga de GameOver después de la tercera muerte.
- Carga del subterráneo, más de 800 colisiones, posición inicial sobre suelo y mapa completo.
- Paso del subterráneo al acuático.
- Dos fotogramas disponibles por enemigo acuático y ascenso por impulso de natación.
- Paso del acuático al castillo, sprite de Bowser, puente y plataformas móviles.
- Animación de plataformas y activación del final mediante el contacto con el interruptor.
- Escena Victoria sin descuento de vidas.

Las comprobaciones automatizan cambios de escena y posiciones para aislar cada caso; no equivalen a una partida manual completa ni prueban todos los saltos del recorrido. Se inspeccionaron capturas renderizadas del subterráneo, el acuático y el castillo. La primera compilación Windows finalizó con BuildResult.Succeeded.

El editor mostró una excepción en `UnityEditor.Search.SearchDatabase` al iniciar su indexación; no procede de los scripts del juego y las comprobaciones continuaron hasta el resultado satisfactorio. No se afirma que el registro del editor esté libre de avisos.

Para repetir: abrir el proyecto con la versión indicada y ejecutar `PruebasEntrega.Ejecutar` mediante Unity en modo batch, sin `-quit` (el propio verificador termina el proceso). Para compilar, usar **Entrega > Compilar Windows**. Para empaquetar, ejecutar `python Herramientas/empaquetar.py`.

La compilaci�n final, posterior a las correcciones de c�mara, salida y sprites, termin� con `MARIO_BUILD_OK`.
