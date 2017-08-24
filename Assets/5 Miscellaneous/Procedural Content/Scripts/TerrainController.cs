using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainController : MonoBehaviour {

	public Texture2D grassTexture;
	public Texture2D dirtTexture;
	public Texture2D rockTexture;
	public Texture2D cliffTexture;
	SplatPrototype[] splats;
	public float perlinScaleLimitX;
	public float perlinScaleLimitY;
	public int perlinPhasses;

	public float[] sharpSteps;
	public float sharpThreshold;

	void Awake()
	{
		createTerrain ();
		generateHeightMapPro ();
	}

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void createTerrain()
	{
		TerrainData terrainData = new TerrainData ();
		terrainData.heightmapResolution = 513;
		terrainData.size = new Vector3 (1000, 100, 1000);
		Texture2D[] textures = { grassTexture, dirtTexture, rockTexture, cliffTexture };
		splats = new SplatPrototype[textures.Length];
		for(int i=0; i<textures.Length; i++)
		{
			splats [i] = new SplatPrototype ();
			splats [i].texture = textures [i];
			splats [i].tileOffset = new Vector2 (1, 1);
			splats [i].tileSize = new Vector2 (15, 15);
			splats [i].texture.Apply (true);
		}
		terrainData.splatPrototypes = splats;
		Terrain.CreateTerrainGameObject (terrainData);
	}

	void generateHeightMapSimple()
	{
		var terrainData = Terrain.activeTerrain.terrainData;
		//obtenemos el tamano del heightmap
		var hw=terrainData.heightmapWidth;
		var hh = terrainData.heightmapHeight;
		//creamos un array2d del tamano del heightmap
		var heights=new float[hw,hh];
		//agregamos aleatoriedad, valores de origen para mover el perlin noise, perlin noise no es deterministico
		var ox = Random.value * 10;
		var oy = Random.value * 10;
		//tambien se puede agregar aleatoriedad con el escalar
		var perlinScale=Random.Range(perlinScaleLimitX,perlinScaleLimitY);

		for(int x=0; x<hw; x++)
		{
			for(int y=0; y<hh; y++)
			{
				//valores de entrada para el algoritmo
				var px = ox + (float)x / hw * perlinScale;
				var py = oy + (float)y / hh * perlinScale;
				//almacenamos el resultado de perlin noise
				heights[x,y]=Mathf.PerlinNoise(px,py);
			}
		}
		//asignamos al terreno
		terrainData.SetHeights(0,0,heights);
	}

	void generateHeightMapPro()
	{
		var terrainData = Terrain.activeTerrain.terrainData;
		//obtenemos el tamano del heightmap
		var hw=terrainData.heightmapWidth;
		var hh = terrainData.heightmapHeight;
		//creamos un array2d del tamano del heightmap
		var heights=new float[hw,hh];

		for(int i=0; i<perlinPhasses; i++)
		{
			perlinGenerator (heights, perlinPhasses);
		}
		//llamamos a la funcion para dividir las alturas del terreno
		Sharpen (heights);
		//asignamos al terreno
		terrainData.SetHeights(0,0,heights);
	}

	void perlinGenerator(float[,] heights, float perlinPhasses)
	{
		var hw = heights.GetLength (0);
		var hh = heights.GetLength (1);

		var ox = Random.value * 10;
		var oy = Random.value * 10;
		//tambien se puede agregar aleatoriedad con el escalar
		var perlinScale=Random.Range(perlinScaleLimitX,perlinScaleLimitY);

		for(int x=0; x<hw; x++)
		{
			for(int y=0; y<hh; y++)
			{
				//valores de entrada para el algoritmo
				var px = ox + (float)x / hw * perlinScale;
				var py = oy + (float)y / hh * perlinScale;
				//almacenamos el resultado de perlin noise
				//divido entre perlinPasses para normalizar los valores entre 0 y 1
				heights[x,y]+=Mathf.PerlinNoise(px,py)/perlinPhasses;
			}
		}
	}

	void Sharpen(float[,] heights)
	{
		var hw = heights.GetLength (0);
		var hh = heights.GetLength (1);

		for(int x=0; x<hw; x++)
		{
			for(int y=0; y<hh; y++)
			{
				var val=heights[x,y];
				// el objetivo es que los valores de altura se ajusten a los valores
				// establecidos por sharpSteps, por lo que necesitamos fijarnos a qué
				// valor del array corresponde nuestro punto x,y actual y ajustarlo
				foreach(var step in sharpSteps)
				{
					if(val>step-sharpThreshold && val<step+sharpThreshold)
					{
						val=step;
					}
				}
				//si la altura es mayor al ultimo de los steps, lo ajustamos a ese
				if(val>sharpSteps[sharpSteps.Length-1])
				{
					val=sharpSteps[sharpSteps.Length-1];
				}
				//asignamos el valor al array
				heights[x,y]=val;
			}
		}
	}
}
