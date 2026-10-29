using UnityEngine;
using UnityEngine.SceneManagement;

// Conserva vidas y monedas entre escenas; una pérdida solo puede descontar una vida.
public class GameManager : MonoBehaviour
{
 public static GameManager Instance {get;private set;}
 public int world {get;private set;}=1;
 public int stage {get;private set;}=1;
 public int lives {get;private set;}=3;
 public int coins {get;private set;}
 public static readonly string[] Ruta={"1-1","1-2","2-2","Castillo"};
 bool pendiente;
 void Awake(){if(Instance!=null){Destroy(gameObject);return;}Instance=this;DontDestroyOnLoad(gameObject);SceneManager.sceneLoaded+=Cargada;}
 void Start(){Application.targetFrameRate=60;}
 void Cargada(Scene s,LoadSceneMode mode){pendiente=false;Time.timeScale=1;}
 void OnDestroy(){if(Instance==this){SceneManager.sceneLoaded-=Cargada;Instance=null;}}
 public void NewGame(){lives=3;coins=0;LoadLevel(1,1);}
 public void Practice(int i){lives=3;coins=0;SceneManager.LoadScene(Ruta[i]);}
 public void GameOver(){SceneManager.LoadScene("GameOver");}
 public void LoadLevel(int w,int s){world=w;stage=s;SceneManager.LoadScene($"{w}-{s}");}
 public void NextLevel(){int i=System.Array.IndexOf(Ruta,SceneManager.GetActiveScene().name);SceneManager.LoadScene(i>=0&&i<3?Ruta[i+1]:"Victoria");}
 public void ResetLevel(float delay){if(pendiente)return;pendiente=true;Invoke(nameof(ResetLevel),delay);}
 public void ResetLevel(){CancelInvoke();lives--;if(lives<=0)GameOver();else SceneManager.LoadScene(SceneManager.GetActiveScene().name);}
 public void AddCoin(){coins++;MejorasAudio.Play(1000,.09f);if(coins>=100){coins-=100;AddLife();}}
 public void AddLife(){lives++;MejorasAudio.Play(1200,.2f);}
}
