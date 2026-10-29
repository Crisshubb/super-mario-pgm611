using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class ReplicaNivel:MonoBehaviour
{
 public static ReplicaNivel Actual;public int tipo;public float longitud=192;
 public Texture2D mapa;public TextAsset geometria;public GameObject mario,goomba,koopa,moneda;public Sprite ladrillo,duro,tubo,jefe,pez,calamar,fuego;public Sprite[] pezAnimacion,calamarAnimacion;
 public Transform jugador;public bool terminado;public List<GameObject> puente=new List<GameObject>();public GameObject bowser;
 [System.Serializable]public class Celda{public int x,y;}
 [System.Serializable]public class Datos{public Celda[] cells;}
 void Awake(){Actual=this;}
 void OnDestroy(){if(Actual==this)Actual=null;}
 void Start(){
  Camera.main.orthographic=true;Camera.main.orthographicSize=7.5f;Camera.main.backgroundColor=tipo==2?new Color(.12f,.02f,.03f):tipo==1?new Color32(32,56,236,255):Color.black;Camera.main.transform.position=new Vector3(Camera.main.orthographicSize*Camera.main.aspect-.5f,6.5f,-10);
  if(tipo<2){
   var s=Sprite.Create(mapa,new Rect(0,tipo==0?240:0,3072,208),new Vector2(.5f,.5f),16);var g=new GameObject("Replica visual del mapa");g.AddComponent<SpriteRenderer>().sprite=s;g.GetComponent<SpriteRenderer>().sortingOrder=-100;g.transform.position=new Vector3(95.5f,6,0);
   var d=JsonUtility.FromJson<Datos>(geometria.text);foreach(var c in d.cells)Solido(c.x,c.y,1,1,null);
   if(tipo==0){foreach(float x in new[]{81f,122.5f,141f,156f}){var p=Solido(x,5,3, .4f,duro);p.AddComponent<Plataforma>().origen=p.transform.position;}
    foreach(float x in new[]{18f,24f,40f,57f,65f,94f,103f,116f})Instantiate(goomba,new Vector3(x,3,0),Quaternion.identity);
    foreach(float x in new[]{33f,70f,110f})Instantiate(koopa,new Vector3(x,3,0),Quaternion.identity);
   }else{
    foreach(float x in new[]{20f,35f,58f,82f,99f,118f,144f,166f}){var p=Visual("Cheep Cheep",pez,new Vector2(x,5+(x%4)),Vector2.one);var a=p.AddComponent<PeligroNivel>();a.modo=1;a.animacion=pezAnimacion;}
    foreach(float x in new[]{45f,75f,109f,151f}){var p=Visual("Blooper",calamar,new Vector2(x,7),Vector2.one);var a=p.AddComponent<PeligroNivel>();a.modo=2;a.animacion=calamarAnimacion;}
   }
   foreach(float x in new[]{12f,28f,46f,63f,91f,104f,135f,155f})for(int i=0;i<3;i++)Instantiate(moneda,new Vector3(x+i, tipo==0?4:6,0),Quaternion.identity);
  }else Castillo();
  jugador=Instantiate(mario,new Vector3(3,3,0),Quaternion.identity).transform;
  Camera.main.gameObject.AddComponent<SideScrolling>();
  var muerte=new GameObject("Caida");muerte.transform.position=new Vector3(longitud/2,-5);var c2=muerte.AddComponent<BoxCollider2D>();c2.size=new Vector2(longitud+30,3);c2.isTrigger=true;muerte.AddComponent<DeathBarrier>();
 }
 public GameObject Visual(string n,Sprite s,Vector2 p,Vector2 size){var g=new GameObject(n);g.transform.position=p;var r=g.AddComponent<SpriteRenderer>();r.sprite=s;if(s!=null)g.transform.localScale=new Vector3(size.x/s.bounds.size.x,size.y/s.bounds.size.y,1);return g;}
 public GameObject Solido(float x,float y,float w,float h,Sprite s){var g=new GameObject("Bloque");g.transform.position=new Vector3(x,y,0);var c=g.AddComponent<BoxCollider2D>();c.size=new Vector2(w,h);if(s!=null){var r=g.AddComponent<SpriteRenderer>();r.sprite=s;r.drawMode=SpriteDrawMode.Tiled;r.size=new Vector2(w,h);}return g;}
 void Castillo(){longitud=152;
  for(int x=0;x<152;x++){bool hueco=(x>=20&&x<25)||(x>=41&&x<47)||(x>=64&&x<69)||(x>=86&&x<92)||(x>=110&&x<116);if(!hueco)Solido(x,1,1,2,ladrillo);else{var lava=Visual("Lava",fuego,new Vector2(x,-.4f),new Vector2(1,1));lava.AddComponent<PeligroNivel>().modo=0;}}
  for(int x=0;x<152;x+=3)Visual("Boveda",ladrillo,new Vector2(x,13),new Vector2(3,2));
  foreach(float x in new[]{14f,34f,56f,77f,100f,119f}){Solido(x,3,3,1,duro);var centro=Visual("Barra de fuego",fuego,new Vector2(x,5),new Vector2(.5f,.5f));centro.AddComponent<PeligroNivel>().modo=3;}
  foreach(float x in new[]{23f,44f,67f,89f,113f}){var p=Solido(x,3,3,.4f,duro);p.AddComponent<Plataforma>().origen=p.transform.position;}
  foreach(float x in new[]{10f,31f,52f,73f,96f,121f})for(int i=0;i<3;i++)Instantiate(moneda,new Vector3(x+i,5,0),Quaternion.identity);
  for(int x=126;x<=140;x++)puente.Add(Solido(x,3,1,.5f,ladrillo));
  bowser=Visual("Bowser",jefe,new Vector2(133,4.5f),new Vector2(2,2));bowser.AddComponent<PeligroNivel>().modo=4;
  var hacha=Visual("Hacha del puente",tubo,new Vector2(142,4.5f),new Vector2(.7f,1));hacha.AddComponent<FinalCastillo>();
 }
 void Update(){if(jugador==null||terminado)return;if(tipo<2&&jugador.position.x>=(tipo==0?165:187)){terminado=true;MejorasAudio.Play(880,.5f);GameManager.Instance.NextLevel();}}
}
public class Plataforma:MonoBehaviour
{
 public Vector3 origen;void Update(){transform.position=origen+Vector3.up*Mathf.Sin(Time.time*1.3f)*1.5f;}
 void OnCollisionEnter2D(Collision2D c){if(c.gameObject.CompareTag("Player"))c.transform.SetParent(transform);}
 void OnCollisionExit2D(Collision2D c){if(c.gameObject.CompareTag("Player"))c.transform.SetParent(null);}
}
public class PeligroNivel:MonoBehaviour
{
 public int modo;public Sprite[] animacion;Vector3 inicio;float siguiente;
 void Start(){inicio=transform.position;var c=gameObject.AddComponent<BoxCollider2D>();c.isTrigger=true;c.size=Vector2.one*.65f;var b=gameObject.AddComponent<Rigidbody2D>();b.bodyType=RigidbodyType2D.Kinematic;}
 void Update(){float t=Time.time;
  if(animacion!=null&&animacion.Length>0)GetComponent<SpriteRenderer>().sprite=animacion[(int)(t*5)%animacion.Length];
  if(modo==1||modo==2)transform.position=inicio+new Vector3(Mathf.Sin(t*(modo==1?.7f:1.2f))*2,Mathf.Sin(t*1.3f)*1.3f,0);
  if(modo==3){transform.position=inicio+new Vector3(Mathf.Cos(t*1.7f),Mathf.Sin(t*1.7f))*1.8f;}
  if(modo==4){transform.position=inicio+new Vector3(Mathf.Sin(t*.8f)*2,Mathf.Max(0,Mathf.Sin(t*1.5f))*1.8f,0);if(t>siguiente){siguiente=t+2.4f;var f=ReplicaNivel.Actual.Visual("Llama de Bowser",ReplicaNivel.Actual.fuego,transform.position+Vector3.left, new Vector2(1,.5f));f.AddComponent<PeligroNivel>().modo=5;}}
  if(modo==5){transform.position+=Vector3.left*5*Time.deltaTime;if(transform.position.x<100)Destroy(gameObject);}
  if(modo==0)transform.localScale=new Vector3(1,1+Mathf.Sin(t*5)*.08f,1);
 }
 void OnTriggerEnter2D(Collider2D c){if(c.CompareTag("Player"))c.GetComponent<Player>().Hit();}
}
public class FinalCastillo:MonoBehaviour
{
 bool fin;void Start(){gameObject.AddComponent<BoxCollider2D>().isTrigger=true;}
 void OnTriggerEnter2D(Collider2D c){if(fin||!c.CompareTag("Player"))return;fin=true;ReplicaNivel.Actual.terminado=true;foreach(var p in ReplicaNivel.Actual.puente)Destroy(p);ReplicaNivel.Actual.bowser.SetActive(false);MejorasAudio.Play(1100,.6f);Invoke(nameof(Ganar),1.8f);}
 void Ganar(){SceneManager.LoadScene("Victoria");}
}
