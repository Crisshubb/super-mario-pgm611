using UnityEngine;
using UnityEngine.SceneManagement;
public class InterfazEntrega:MonoBehaviour
{
 bool pausa;GUIStyle titulo,normal,boton;
 public Font FuenteMenu {get;private set;}
 void Awake(){FuenteMenu=Resources.Load<Font>("Fuentes/Silkscreen");}public static bool Pruebas;
 [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]static void Inicializar(){SceneManager.sceneLoaded+=AlCargar;AlCargar(SceneManager.GetActiveScene(),LoadSceneMode.Single);}
 static void AlCargar(Scene s,LoadSceneMode m){if(GameManager.Instance==null)new GameObject("Partida").AddComponent<GameManager>();new GameObject("Sonido y pantalla").AddComponent<MejorasAudio>().gameObject.AddComponent<InterfazEntrega>();foreach(var flag in Object.FindObjectsByType<FlagPole>(FindObjectsSortMode.None)){flag.nextWorld=1;flag.nextStage=2;}}
 void Update(){if(Input.GetKeyDown(KeyCode.Escape)){pausa=!pausa;Time.timeScale=pausa?0:1;}if(Input.GetKeyDown(KeyCode.F11))Screen.fullScreen=!Screen.fullScreen;}
 void OnGUI(){if(Pruebas)return;GUI.matrix=Matrix4x4.TRS(Vector3.zero,Quaternion.identity,new Vector3(Screen.width/1280f,Screen.height/720f,1));if(titulo==null){titulo=new GUIStyle(GUI.skin.label){font=FuenteMenu,fontSize=40,alignment=TextAnchor.MiddleCenter,fontStyle=FontStyle.Normal};normal=new GUIStyle(GUI.skin.label){font=FuenteMenu,fontSize=18,alignment=TextAnchor.MiddleCenter,wordWrap=true};boton=new GUIStyle(GUI.skin.button){font=FuenteMenu,fontSize=17,alignment=TextAnchor.MiddleCenter,wordWrap=true};}string scene=SceneManager.GetActiveScene().name;
 if(scene=="Menu"||scene=="GameOver"||scene=="Victoria"||pausa){GUI.Box(new Rect(160,70,960,580),"");GUI.Label(new Rect(180,100,920,90),pausa?"PAUSA":scene=="GameOver"?"GAME OVER":scene=="Victoria"?"¡CASTILLO LIBERADO!":"SUPER MARIO · PGM611",titulo);GUI.Label(new Rect(190,195,900,65),scene=="GameOver"?"Has perdido todas las vidas":scene=="Victoria"?"Derrotaste a Bowser y completaste la aventura":"Subterráneo · Agua · Castillo original",normal);
 if(GUI.Button(new Rect(420,280,440,55),pausa?"CONTINUAR":"JUGAR DESDE EL PRINCIPIO",boton)){pausa=false;Time.timeScale=1;if(scene!="Menu"&&scene!="GameOver"&&scene!="Victoria")return;GameManager.Instance.NewGame();}
 if(!pausa){for(int i=0;i<3;i++)if(GUI.Button(new Rect(250+i*265,365,250,55),new[]{"Practicar subterráneo","Practicar acuático","Practicar castillo"}[i],boton))GameManager.Instance.Practice(i+1);}
 if(GUI.Button(new Rect(420,460,440,50),"SALIR",boton))Application.Quit();GUI.Label(new Rect(200,540,880,65),"A/D o flechas: mover · Espacio: saltar/nadar\nEsc: pausa · F11: pantalla completa",normal);
 }else{GUI.Box(new Rect(0,0,1280,60),"");GUI.Label(new Rect(20,8,1240,42),scene+"    VIDAS: "+GameManager.Instance.lives+"    MONEDAS: "+GameManager.Instance.coins+"    ESC: PAUSA",new GUIStyle(GUI.skin.label){fontSize=22,alignment=TextAnchor.MiddleCenter});}
 }
}
