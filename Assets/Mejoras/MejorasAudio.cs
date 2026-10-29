using UnityEngine;
using UnityEngine.SceneManagement;
// Efectos y acompañamiento sintéticos originales; no se copian grabaciones comerciales.
public class MejorasAudio:MonoBehaviour
{
 static AudioSource efectos;AudioSource musica;
 void Awake(){efectos=gameObject.AddComponent<AudioSource>();musica=gameObject.AddComponent<AudioSource>();musica.loop=true;musica.volume=.12f;}
 void Start(){string n=SceneManager.GetActiveScene().name;int seed=n=="2-2"?5:n=="Castillo"?2:3;int rate=22050;float[] d=new float[rate*8];int[] notes={0,4,7,12,7,4,2,7,5,2,9,5,4,0,7,2};for(int i=0;i<d.Length;i++){float t=i/(float)rate;float f=130.81f*Mathf.Pow(2,(notes[(int)(t*2)%16]+seed)/12f);d[i]=Mathf.Sin(t*f*6.28318f)*(.25f-.2f*(t*2%1));}var c=AudioClip.Create("Tema original "+n,d.Length,1,rate,false);c.SetData(d,0);musica.clip=c;musica.Play();}
 public static void Play(float freq,float duration){if(efectos==null)return;int rate=22050;float[] d=new float[(int)(rate*duration)];for(int i=0;i<d.Length;i++)d[i]=Mathf.Sin(6.28318f*freq*i/rate)*.3f*(1-i/(float)d.Length);var c=AudioClip.Create("Efecto",d.Length,1,rate,false);c.SetData(d,0);efectos.PlayOneShot(c);Destroy(c,duration+1);}
 void OnDestroy(){if(musica!=null&&musica.clip!=null)Destroy(musica.clip);}
}
