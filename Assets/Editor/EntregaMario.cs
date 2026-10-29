using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using UnityEditor.Build.Reporting;
using System.IO;
public static class EntregaMario
{
 static Sprite S(string n){return AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/"+n+".png");}
 static GameObject P(string n){return AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/"+n+".prefab");}
 static Sprite Extra(string n){return AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Mejoras/"+n+".png");}
 [MenuItem("Entrega/Preparar niveles")]
 public static void Preparar(){
  foreach(string p in AssetDatabase.FindAssets("t:Texture2D",new[]{"Assets/Mejoras"})){string path=AssetDatabase.GUIDToAssetPath(p);var ti=AssetImporter.GetAtPath(path) as TextureImporter;if(ti==null)continue;ti.textureType=TextureImporterType.Sprite;ti.spritePixelsPerUnit=16;ti.filterMode=FilterMode.Point;ti.mipmapEnabled=false;ti.textureCompression=TextureImporterCompression.Uncompressed;ti.isReadable=true;ti.maxTextureSize=4096;ti.SaveAndReimport();}
  string[] scenes={"Menu","1-1","1-2","2-2","Castillo","GameOver","Victoria"};
  foreach(string n in scenes){if(n=="1-1")continue;var sc=EditorSceneManager.NewScene(NewSceneSetup.EmptyScene,NewSceneMode.Single);var cam=new GameObject("Camera");cam.tag="MainCamera";cam.AddComponent<Camera>().backgroundColor=new Color(.035f,.045f,.09f);cam.AddComponent<AudioListener>();
   if(n=="1-2"||n=="2-2"||n=="Castillo"){var g=new GameObject("Nivel");var r=g.AddComponent<ReplicaNivel>();r.tipo=n=="1-2"?0:n=="2-2"?1:2;r.mario=P("Mario");r.goomba=P("Goomba");r.koopa=P("Koopa");r.moneda=P("Coin");r.ladrillo=S("UndergroundBrick");r.duro=S("UndergroundBlock");r.tubo=S("PipeTop");r.jefe=Extra("Bowser");r.pez=Extra("Cheep-0");r.calamar=Extra("Blooper-0");r.fuego=S("MagicMushroom");if(r.tipo<2){r.mapa=AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Mejoras/Mapa-"+n+".png");r.geometria=AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/Mejoras/Celdas-"+n+".json");}}
   EditorSceneManager.SaveScene(sc,"Assets/Scenes/"+n+".unity");}
  EditorBuildSettings.scenes=System.Array.ConvertAll(scenes,n=>new EditorBuildSettingsScene("Assets/Scenes/"+n+".unity",true));
  PlayerSettings.companyName="Cristopher Lazcano";PlayerSettings.productName="Mario PGM611";PlayerSettings.bundleVersion="1.0.0";PlayerSettings.defaultScreenWidth=1280;PlayerSettings.defaultScreenHeight=720;PlayerSettings.fullScreenMode=FullScreenMode.FullScreenWindow;
  var settings=new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/ProjectSettings.asset")[0]);settings.FindProperty("activeInputHandler").intValue=0;settings.ApplyModifiedPropertiesWithoutUndo();AssetDatabase.SaveAssets();EditorSceneManager.OpenScene("Assets/Scenes/Menu.unity");Debug.Log("MARIO_ESCENAS_OK");
 }
 [MenuItem("Entrega/Compilar Windows")]
 public static void Compilar(){Preparar();Directory.CreateDirectory("../Entrega/Windows");var report=BuildPipeline.BuildPlayer(new BuildPlayerOptions{scenes=System.Array.ConvertAll(EditorBuildSettings.scenes,s=>s.path),locationPathName="../Entrega/Windows/Mario-PGM611.exe",target=BuildTarget.StandaloneWindows64,options=BuildOptions.None});if(report.summary.result!=BuildResult.Succeeded)throw new System.Exception("Build: "+report.summary.result);Debug.Log("MARIO_BUILD_OK");}
}
