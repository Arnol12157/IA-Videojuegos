using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class MNode
{
	public int x; //posicion x en la matriz
	public int y; //posicion y en la matriz
	public int player; //id del jugador
}
	
public class MovesAndScores
{
	public int score; //puntaje
	public MNode point; //

	public MovesAndScores(int score, MNode point)
	{
		this.score = score;
		this.point = point;
	}
}
	
public class script : MonoBehaviour
{
	public GameObject xprefab;
	public GameObject oprefab;
	MNode[,] grid = new MNode[3,3];
	public List<MovesAndScores> rootsChildrenScores;
	public GameObject ui;

	void Start ()
	{

	}

	// Update is called once per frame
	void Update () {
		if (!isGameOver()) // el juego acaba si alguien gano o si el ablero esta lleno
		{
			//input por teclado
			if (Input.GetKeyDown ("q") && isValidMove (0,0))
			{
				Instantiate (xprefab, new Vector3 (0, 1, 0), Quaternion.identity);
				MNode node = new MNode ();
				node.x = 0;
				node.y = 0;
				node.player = 1; //1 = id del jugador
				grid [node.x, node.y] = node;
				callMinimax (0, 1);

				MNode best = returnBestMove ();
				if(isValidMove(best.x, best.y))
				{
					Instantiate (oprefab, new Vector3 (best.x, 1, best.y), Quaternion.identity);
					best.player = 2;
					grid[best.x,best.y] = best;
				}

			}
			if (Input.GetKeyDown ("w") && isValidMove (0,1))
			{
				Instantiate (xprefab, new Vector3 (0, 1, 1), Quaternion.identity);
				MNode node = new MNode ();
				node.x = 0;
				node.y = 1;
				node.player = 1;
				grid [node.x, node.y] = node;
				callMinimax (0, 1);
			
				MNode best = returnBestMove ();
				if(isValidMove(best.x, best.y))
				{
					Instantiate (oprefab, new Vector3 (best.x, 1, best.y), Quaternion.identity);
					best.player = 2;
					grid[best.x,best.y] = best;
				}
			}
			if (Input.GetKeyDown ("e") && isValidMove (0,2))
			{
				Instantiate (xprefab, new Vector3 (0, 1, 2), Quaternion.identity);
				MNode node = new MNode ();
				node.x = 0;
				node.y = 2;
				node.player = 1;
				grid [node.x, node.y] = node;
				callMinimax (0, 1);
			
				MNode best = returnBestMove ();
				if(isValidMove(best.x, best.y))
				{
					Instantiate (oprefab, new Vector3 (best.x, 1, best.y), Quaternion.identity);
					best.player = 2;
					grid[best.x,best.y] = best;
				}
			}
			if (Input.GetKeyDown ("a") && isValidMove (1,0))
			{
				Instantiate (xprefab, new Vector3 (1, 1, 0), Quaternion.identity);
				MNode node = new MNode ();
				node.x = 1;
				node.y = 0;			
				node.player = 1;
				grid [node.x, node.y] = node;
				callMinimax (0, 1);

				MNode best = returnBestMove ();
				if(isValidMove(best.x, best.y))
				{
					Instantiate (oprefab, new Vector3 (best.x, 1, best.y), Quaternion.identity);
					best.player = 2;
					grid[best.x,best.y] = best;
				}
			}
			if (Input.GetKeyDown ("s") && isValidMove (1,1))
			{
				Instantiate (xprefab, new Vector3 (1, 1, 1), Quaternion.identity);
				MNode node = new MNode ();
				node.x = 1;
				node.y = 1;
				node.player = 1;
				grid [node.x, node.y] = node;
				callMinimax (0, 1);

				MNode best = returnBestMove ();
				if(isValidMove(best.x, best.y))
				{
					Instantiate (oprefab, new Vector3 (best.x, 1, best.y), Quaternion.identity);
					best.player = 2;
					grid[best.x,best.y] = best;
				}
			}
			if (Input.GetKeyDown ("d") && isValidMove (1,2))
			{
				Instantiate (xprefab, new Vector3 (1, 1, 2), Quaternion.identity);
				MNode node = new MNode ();
				node.x = 1;
				node.y = 2;
				node.player = 1;
				grid [node.x, node.y] = node;
				callMinimax (0, 1);

				MNode best = returnBestMove ();
				if(isValidMove(best.x, best.y))
				{
					Instantiate (oprefab, new Vector3 (best.x, 1, best.y), Quaternion.identity);
					best.player = 2;
					grid[best.x,best.y] = best;
				}
			}
			if (Input.GetKeyDown ("z") && isValidMove (2,0))
			{
				Instantiate (xprefab, new Vector3 (2, 1, 0), Quaternion.identity);
				MNode node = new MNode ();
				node.x = 2;
				node.y = 0;
				node.player = 1;
				grid [node.x, node.y] = node;
				callMinimax (0, 1);

				MNode best = returnBestMove ();
				if(isValidMove(best.x, best.y))
				{
					Instantiate (oprefab, new Vector3 (best.x, 1, best.y), Quaternion.identity);
					best.player = 2;
					grid[best.x,best.y] = best;
				}
			}
			if (Input.GetKeyDown ("x") && isValidMove (2,1))
			{
				Instantiate (xprefab, new Vector3 (2, 1, 1), Quaternion.identity);
				MNode node = new MNode ();
				node.x = 2;
				node.y = 1;
				node.player = 1;
				grid [node.x, node.y] = node;
				callMinimax (0, 1);

				MNode best = returnBestMove ();
				if(isValidMove(best.x, best.y))
				{
					Instantiate (oprefab, new Vector3 (best.x, 1, best.y), Quaternion.identity);
					best.player = 2;
					grid[best.x,best.y] = best;
				}
			}
			if (Input.GetKeyDown ("c") && isValidMove (2,2))
			{
				Instantiate (xprefab, new Vector3 (2, 1, 2), Quaternion.identity);
				MNode node = new MNode ();
				node.x = 2;
				node.y = 2;
				node.player = 1;
				grid [node.x, node.y] = node;
				callMinimax (0, 1);

				MNode best = returnBestMove ();
				if(isValidMove(best.x, best.y))
				{
					Instantiate (oprefab, new Vector3 (best.x, 1, best.y), Quaternion.identity);
					best.player = 2;
					grid[best.x,best.y] = best;
				}
			}
		}
	}

	public void callMinimax(int depth, int turn)
	{
		rootsChildrenScores = new List<MovesAndScores>(); // inicializamos la variable
		minimax(depth, turn);
	}

	public MNode returnBestMove()
	{
		int MAX = -100000;
		int best = -1;

		for (int i = 0; i < rootsChildrenScores.Count; i++)
		{ 
			if (MAX < rootsChildrenScores[i].score && isValidMove(rootsChildrenScores[i].point.x, rootsChildrenScores[i].point.y))
			{
				MAX = rootsChildrenScores[i].score;
				best = i;
			}
		}
		if(best > -1)
		{
			return rootsChildrenScores[best].point;
		}
		MNode blank = new MNode();
		blank.x = 0;
		blank.y = 0;
		return blank;
	}

	public bool hasOWon()
	{
		if (grid [0, 0] != null && grid [1, 1] != null && grid [2, 2] != null)
		{
			if (grid [0, 0].player == grid [1, 1].player && grid [0, 0].player == grid [2, 2].player && grid [0, 0].player == 1)
			{
				return true;
			}
		}
		if (grid [0, 2] != null && grid [1, 1] != null && grid [2, 0] != null)
		{
			if(grid [0, 2].player == grid [1, 1].player && grid [0, 2].player == grid [2, 0].player && grid [0, 2].player == 1)
			{
				return true;
			}
		}

		for (int i = 0; i < 3; ++i)
		{
			if(grid[i,0] != null && grid[i,1] != null && grid[i,2] != null)
			{
				if(grid[i,0].player == grid[i,1].player && grid[i,0].player == grid[i,2].player && grid[i,0].player == 1)
				{
					return true;
				}
			}
			if(grid[0,i] != null && grid[1,i] != null && grid[2,i] != null)
			{
				if(grid[0,i].player == grid[1,i].player && grid[0,i].player == grid[2,i].player && grid[0,i].player == 1)
				{
					return true;
				}
			}
		}
		return false;
	}

	public bool hasXWon()
	{
		if (grid [0, 0] != null && grid [1, 1] != null && grid [2, 2] != null)
		{
			if (grid [0, 0].player == grid [1, 1].player && grid [0, 0].player == grid [2, 2].player && grid [0, 0].player == 2)
			{
				return true;
			}
		}
		if (grid [0, 2] != null && grid [1, 1] != null && grid [2, 0] != null)
		{
			if(grid [0, 2].player == grid [1, 1].player && grid [0, 2].player == grid [2, 0].player && grid [0, 2].player == 2)
			{
				return true;
			}
		}
		
		for (int i = 0; i < 3; ++i)
		{
			if(grid[i,0] != null && grid[i,1] != null && grid[i,2] != null)
			{
				if(grid[i,0].player == grid[i,1].player && grid[i,0].player == grid[i,2].player && grid[i,0].player == 2)
				{
					return true;
				}
			}
			if(grid[0,i] != null && grid[1,i] != null && grid[2,i] != null)
			{
				if(grid[0,i].player == grid[1,i].player && grid[0,i].player == grid[2,i].player && grid[0,i].player == 2)
				{
					return true;
				}
			}
		}
		return false;
	}
		
	public bool isGameOver()
	{
		Text text = ui.GetComponent<Text>();
		if (getMoves ().Capacity == 0)
		{
			text.text = "Draw!";
			return true;
		}
		if (hasOWon ())
		{
			text.text = "Win!";
			return true;
		}
		if (hasXWon ())
		{
			text.text = "Lost!";
			return true;
		}
		return false;
	}


	List<MNode> getMoves()
	{
		// verificamos las casillas vacias
		List<MNode> result = new List<MNode>();
		for(int row = 0; row < 3; row++)
		{
			for(int col = 0; col < 3; col++)
			{
				if(grid[row,col] == null)
				{
					MNode newNode = new MNode();
					newNode.x = row;
					newNode.y = col;
					result.Add(newNode);
				}
			}
		}
		return result;
	}
		
	public bool isValidMove(int x, int y)
	{
		// verificamos si la casilla [x,y] esta vacia
		if (grid [x, y] == null)
		{
			return true;
		}
		return false;
	}
		
	public int returnMin(List<int> list)
	{
		// identificamos al menor de la lista
		int min = 100000;
		int index = -1;
		for (int i = 0; i < list.Count; ++i)
		{
			if (list[i] < min)
			{
				min = list[i];
				index = i;
			}
		}
		return list[index];
	}
		
	public int returnMax(List<int> list)
	{
		// identificamos al mayor de la lista
		int max = -100000;
		int index = -1;
		for (int i = 0; i < list.Count; ++i)
		{
			if (list[i] > max)
			{
				max = list[i];
				index = i;
			}
		}
		return list[index];
	}

	public int minimax(int depth, int turn)
	{
		if (hasXWon()) return +1; //gana jugador
		if (hasOWon()) return -1; //gana computadora
		
		List<MNode> pointsAvailable = getMoves();
		if (pointsAvailable.Capacity == 0) return 0; 
		
		List<int> scores = new List<int>(); 

		for (int i = 0; i < pointsAvailable.Count; i++)
		{
			MNode point = pointsAvailable[i];
			//arbol para el jugador
			if (turn == 1)
			{ 
				MNode x = new MNode();
				x.x = point.x;
				x.y = point.y;
				x.player = 2;
				grid[point.x,point.y] = x;

				int currentScore = minimax(depth + 1, 2);
				scores.Add(currentScore);
				
				if (depth == 0)
				{
					MovesAndScores m = new MovesAndScores(currentScore, point);
					rootsChildrenScores.Add(m);
				}
			} 
			//arbol para la maquina
			else if (turn == 2)
			{
				MNode o = new MNode();
				o.x = point.x;
				o.y = point.y;
				o.player = 1;
				grid[point.x,point.y] = o;
				int currentScore = minimax(depth+1,1);
				scores.Add(currentScore);
			}
			grid[point.x, point.y] = null; //reiniciamos la casilla
		}
		return turn == 1 ? returnMax(scores) : returnMin(scores);// if turn=1->returnMax(scores) else returnMin(scores)
	}
}