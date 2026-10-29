using System;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEditor;

public static class PruebasCastillo
{
 static int fase,indice,checks;static double esperar,limite;static Plataforma[] plataformas;static PlayerMovement movimiento;static Rigidbody2D cuerpo;static Transform mario;static float inicioX,inicioY;static bool caminar;static FieldInfo velocidad;
 public static void Ejecutar(){EntregaMario.Preparar();fase=0;indice=0;checks=0;limite=EditorApplication.timeSinceStartup+100;esperar=EditorApplication.timeSinceStartup+1;EditorSettings.enterPlayModeOptionsEnabled=true;EditorSettings.enterPlayModeOptions=EnterPlayModeOptions.DisableDomainReload;velocidad=typeof(PlayerMovement).GetField("velocity",BindingFlags.Instance|BindingFlags.NonPublic);EditorApplication.update+=Tick;EditorApplication.EnterPlaymode();}
 static void Check(bool value,string text){if(!value)throw new Exception(text);checks++;Debug.Log("CASTILLO_CHECK_OK "+text);}
 static void Posicionar(Vector2 p,Vector2 v){caminar=false;cuerpo.position=p;cuerpo.linearVelocity=Vector2.zero;mario.position=p;velocidad.SetValue(movimiento,v);Physics2D.SyncTransforms();Camera.main.transform.position=new Vector3(p.x,6.5f,-10);inicioX=p.x;inicioY=p.y;}
 static void Tick(){try{
  if(EditorApplication.timeSinceStartup>limite)throw new Exception("Tiempo agotado");if(!EditorApplication.isPlaying)return;
  if(caminar&&movimiento!=null){var v=(Vector2)velocidad.GetValue(movimiento);v.x=5;velocidad.SetValue(movimiento,v);}
  if(EditorApplication.timeSinceStartup<esperar)return;
  switch(fase){
   case 0:Check(Resources.Load<Font>("Fuentes/Silkscreen")!=null,"Fuente pixel incluida en Resources");Check(UnityEngine.Object.FindFirstObjectByType<InterfazEntrega>().FuenteMenu!=null,"El menu carga la fuente pixel");GameManager.Instance.Practice(3);fase++;esperar=EditorApplication.timeSinceStartup+1;return;
   case 1:
    foreach(var p in UnityEngine.Object.FindObjectsByType<PeligroNivel>(FindObjectsSortMode.None))p.gameObject.SetActive(false);
    plataformas=UnityEngine.Object.FindObjectsByType<Plataforma>(FindObjectsSortMode.None).OrderBy(p=>p.origen.x).ToArray();Check(plataformas.Length==5,"Cinco plataformas del castillo");
    mario=ReplicaNivel.Actual.jugador;movimiento=mario.GetComponent<PlayerMovement>();cuerpo=mario.GetComponent<Rigidbody2D>();
    Check(mario.GetComponent<Collider2D>().sharedMaterial.friction==0,"Mario sin friccion adherente en castillo");fase=2;goto case 2;
   case 2:{var p=plataformas[indice];Check(p.fisicaCastillo&&p.GetComponent<Rigidbody2D>().bodyType==RigidbodyType2D.Kinematic,"Plataforma cinematica "+indice);Posicionar(p.GetComponent<Rigidbody2D>().position+Vector2.up*.76f,Vector2.zero);fase=3;esperar=EditorApplication.timeSinceStartup+.45;return;}
   case 3:Check(mario.parent==null,"Mario independiente al aterrizar "+indice);Check(movimiento.enabled&&!cuerpo.isKinematic,"Control activo sobre plataforma "+indice);Check(Mathf.Abs(cuerpo.position.y-(plataformas[indice].GetComponent<Rigidbody2D>().position.y+.7f))<.7f,"Aterrizaje sin atravesar plataforma "+indice);inicioX=cuerpo.position.x;caminar=true;fase=4;esperar=EditorApplication.timeSinceStartup+.34;return;
   case 4:Check(cuerpo.position.x-inicioX>1,"Mario camina sobre plataforma "+indice);Check(mario.parent==null,"Caminar no vincula al personaje "+indice);Posicionar(plataformas[indice].GetComponent<Rigidbody2D>().position+Vector2.up*.76f,new Vector2(0,movimiento.jumpForce));fase=5;esperar=EditorApplication.timeSinceStartup+.16;return;
   case 5:Check(cuerpo.position.y>inicioY+.7f,"Mario puede saltar desde plataforma "+indice);Check(mario.parent==null,"Salto libre de plataforma "+indice);Posicionar(plataformas[indice].GetComponent<Rigidbody2D>().position-Vector2.up*.85f,new Vector2(0,8));fase=6;esperar=EditorApplication.timeSinceStartup+.22;return;
   case 6:Check(mario.parent==null&&movimiento.enabled&&!cuerpo.isKinematic,"Golpe inferior no bloquea a Mario "+indice);Posicionar(plataformas[indice].GetComponent<Rigidbody2D>().position+Vector2.left*2.03f,new Vector2(5,0));caminar=true;fase=7;esperar=EditorApplication.timeSinceStartup+.23;return;
   case 7:Check(mario.parent==null,"Contacto lateral no vincula a Mario "+indice);Check(cuerpo.position.y<inicioY-.15f||cuerpo.position.x>inicioX+.65f,"Mario cae o avanza al rozar borde "+indice);caminar=false;indice++;fase=indice<plataformas.Length?2:8;esperar=EditorApplication.timeSinceStartup+.02;return;
   case 8:Check(GameManager.Instance.lives==3,"Pruebas de plataformas sin perdida de vidas");GameManager.Instance.Practice(1);fase=9;esperar=EditorApplication.timeSinceStartup+1;return;
   case 9:Check(UnityEngine.Object.FindObjectsByType<Plataforma>(FindObjectsSortMode.None).All(p=>!p.fisicaCastillo),"Plataformas subterraneas conservan comportamiento");Check(ReplicaNivel.Actual.jugador.GetComponent<Collider2D>().sharedMaterial!=Plataforma.MaterialSinFriccion,"Material especial limitado al castillo");Debug.Log("CASTILLO_PRUEBAS_OK "+checks);EditorApplication.update-=Tick;EditorApplication.Exit(0);return;
  }
 }catch(Exception e){Debug.LogException(e);Debug.LogError("CASTILLO_PRUEBAS_FALLO fase="+fase+" plataforma="+indice);EditorApplication.update-=Tick;EditorApplication.Exit(1);}}
}
