using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System;
using System.IO;
using System.Linq;

// Comprobaciones sobre las escenas reales en Play Mode. No se incluyen en el ejecutable.
public static class PruebasEntrega
{
 static int paso;static double siguiente,limite;static float altura;static Vector3 plataforma;static int checks;
 public static void Ejecutar(){EntregaMario.Preparar();paso=0;checks=0;limite=EditorApplication.timeSinceStartup+100;siguiente=EditorApplication.timeSinceStartup+1;EditorSettings.enterPlayModeOptionsEnabled=true;EditorSettings.enterPlayModeOptions=EnterPlayModeOptions.DisableDomainReload;EditorApplication.update+=Avanzar;EditorApplication.EnterPlaymode();}
 static void Check(bool value,string name){if(!value)throw new Exception(name);checks++;Debug.Log("MARIO_CHECK_OK "+name);}
 static void Captura(string nombre){var c=Camera.main;var rt=new RenderTexture(1280,720,24);c.targetTexture=rt;c.Render();RenderTexture.active=rt;var t=new Texture2D(1280,720,TextureFormat.RGB24,false);t.ReadPixels(new Rect(0,0,1280,720),0,0);t.Apply();Directory.CreateDirectory("../Evidencias");File.WriteAllBytes("../Evidencias/"+nombre+".png",t.EncodeToPNG());c.targetTexture=null;RenderTexture.active=null;UnityEngine.Object.DestroyImmediate(rt);UnityEngine.Object.DestroyImmediate(t);}
 static void Avanzar(){
  try{
   if(EditorApplication.timeSinceStartup>limite)throw new Exception("Tiempo agotado");if(!EditorApplication.isPlaying||EditorApplication.timeSinceStartup<siguiente)return;
   var g=GameManager.Instance;
   switch(paso){
    case 0:Check(g!=null&&g.lives==3,"Menu y tres vidas iniciales");Check(EditorBuildSettings.scenes.Length==7,"Siete escenas en el build");g.NewGame();break;
    case 1:Check(SceneManager.GetActiveScene().name=="1-1"&&GameObject.FindWithTag("Player")!=null,"Nivel original conservado");g.ResetLevel();break;
    case 2:Check(g.lives==2&&SceneManager.GetActiveScene().name=="1-1","Muerte descuenta una vida y reinicia nivel");g.ResetLevel();break;
    case 3:Check(g.lives==1,"Segunda muerte conserva la partida");g.ResetLevel();break;
    case 4:Check(g.lives==0&&SceneManager.GetActiveScene().name=="GameOver","Game Over real al perder tres vidas");g.Practice(1);break;
    case 5:{var n=ReplicaNivel.Actual;Check(n!=null&&n.tipo==0,"Subterraneo cargado");Check(UnityEngine.Object.FindObjectsByType<BoxCollider2D>(FindObjectsSortMode.None).Length>800,"Geometria subterranea a escala de tiles");Check(n.jugador.position.y>1.5f,"Inicio subterraneo sobre suelo solido");Check(n.mapa!=null&&n.mapa.width==3072,"Mapa de referencia completo");Captura("Subterraneo");var m=n.jugador.GetComponent<PlayerMovement>();m.enabled=false;n.jugador.position=new Vector3(165.1f,4);break;}
    case 6:{Check(SceneManager.GetActiveScene().name=="2-2","Salida del subterraneo conduce al acuatico");var n=ReplicaNivel.Actual;Check(n.tipo==1&&n.pezAnimacion.Length==2&&n.calamarAnimacion.Length==2,"Enemigos acuaticos animados");n.jugador.position=new Vector3(6,6);n.jugador.GetComponent<Rigidbody2D>().position=new Vector2(6,6);n.jugador.GetComponent<PlayerMovement>().Swim();altura=n.jugador.position.y;break;}
    case 7:{var n=ReplicaNivel.Actual;Check(n.jugador.position.y>altura,"Impulso de natacion eleva al jugador");Captura("Acuatico");n.jugador.GetComponent<PlayerMovement>().enabled=false;n.jugador.position=new Vector3(187.1f,5);break;}
    case 8:{Check(SceneManager.GetActiveScene().name=="Castillo","Castillo despues del nivel acuatico");var n=ReplicaNivel.Actual;Check(n.longitud==152&&n.bowser.GetComponent<SpriteRenderer>().sprite!=null,"Castillo original y sprite de Bowser");Check(n.puente.Count==15,"Puente de quince piezas");Check(UnityEngine.Object.FindObjectsByType<Plataforma>(FindObjectsSortMode.None).Length==5,"Cinco plataformas moviles del castillo");var p=UnityEngine.Object.FindFirstObjectByType<Plataforma>();plataforma=p.transform.position;Camera.main.transform.position=new Vector3(132,6.5f,-10);Captura("Castillo");break;}
    case 9:{var n=ReplicaNivel.Actual;Check(Vector3.Distance(UnityEngine.Object.FindFirstObjectByType<Plataforma>().transform.position,plataforma)>.01f,"Plataformas se animan durante la partida");var p=n.jugador;p.GetComponent<PlayerMovement>().enabled=false;p.GetComponent<Collider2D>().enabled=true;var rb=p.GetComponent<Rigidbody2D>();rb.bodyType=RigidbodyType2D.Dynamic;rb.gravityScale=0;rb.position=new Vector2(142,4.5f);Physics2D.SyncTransforms();break;}
    case 10:Check(SceneManager.GetActiveScene().name=="Victoria","Hacha destruye puente y conduce a victoria");Check(g.lives>0,"Victoria no consume vidas");Debug.Log("MARIO_PRUEBAS_OK "+checks+" Unity="+Application.unityVersion);EditorApplication.update-=Avanzar;EditorApplication.Exit(0);return;
   }
   paso++;siguiente=EditorApplication.timeSinceStartup+(paso==10?3:paso==7?.35:1.1);
  }catch(Exception e){Debug.LogException(e);Debug.LogError("MARIO_PRUEBAS_FALLO paso="+paso);EditorApplication.update-=Avanzar;EditorApplication.Exit(1);}
 }
}
