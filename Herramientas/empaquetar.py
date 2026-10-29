"""Empaqueta la compilación Windows completa, sin cachés de Unity."""
from pathlib import Path
import hashlib
import zipfile
import re

project = Path(__file__).resolve().parents[1]
delivery = project.parent / 'Entrega'
settings = (project / 'ProjectSettings/ProjectSettings.asset').read_text(encoding='utf-8')
version = re.search(r'^\s*bundleVersion:\s*(\S+)', settings, re.M).group(1)
windows = delivery / ('Windows-' + version)
if version == '1.0.0' and not windows.exists():
    windows = delivery / 'Windows'
assert (windows / 'Mario-PGM611.exe').is_file(), 'Compilar primero desde Entrega > Compilar Windows'
assert (windows / 'UnityPlayer.dll').is_file()
assert (windows / 'Mario-PGM611_Data').is_dir()
archive = delivery / ('Mario-PGM611-Windows-v' + version + '.zip')
guide = '''MARIO PGM611 - CRISTOPHER IORI LAZCANO GUTIERREZ

Extraer TODO el ZIP y ejecutar Mario-PGM611.exe.
Conservar UnityPlayer.dll, Mario-PGM611_Data y las otras carpetas junto al EXE.
No hace falta instalar Unity para jugar.

CONTROLES: A/D o flechas para moverse; Espacio para saltar o nadar.
Esc pausa; F11 cambia pantalla completa. En las tuberías del nivel inicial, S.

CAMPAÑA: 1-1 original > subterráneo 1-2 > acuático 2-2 > castillo propio.
Tres vidas. Al agotarlas aparece Game Over. El menú permite practicar cada mejora.
En el castillo, evita las llamas y alcanza la estrella al otro lado de Bowser
para activar el interruptor, retirar el puente y terminar la partida.

Repositorio: https://github.com/Crisshubb/super-mario-pgm611
Ver README.md, MEJORAS.md y CREDITOS.md para detalles y procedencia de recursos.
'''
(delivery / 'LEEME-PARA-JUGAR.txt').write_text(guide, encoding='utf-8')
with zipfile.ZipFile(archive, 'w', zipfile.ZIP_DEFLATED, compresslevel=6) as package:
    for file in sorted(windows.rglob('*')):
        if file.is_file():
            package.write(file, file.relative_to(windows).as_posix())
    package.writestr('LEEME-PARA-JUGAR.txt', guide)
    for name in ('MEJORAS.md', 'PRUEBAS.md', 'CREDITOS.md'):
        package.write(project / name, name)
with zipfile.ZipFile(archive) as package:
    assert package.testzip() is None, 'ZIP dañado'
digest = hashlib.sha256(archive.read_bytes()).hexdigest()
(delivery / 'SHA256SUMS.txt').write_text(digest + '  ' + archive.name + '\n', encoding='utf-8')
print(archive)
print('SHA256', digest)
