using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;

public class scenetrigger : MonoBehaviour
{
    public static int currentSceneIndex ;
     public static List<int> numbers = new List<int>();

    public List<List<int>> graph = new List<List<int>>();
    public  int randomNumber;
    CoinManager coinManager = CoinManager.Instance;

    private bool randomNumberGenerated = false;
    public static scenetrigger Instance { get; private set; }

    public int PlayerHealth { get; private set; }

    public void GenerateGraphAndNumbers()
{
    
    numbers.Clear();
    currentSceneIndex = 0;
    if (!randomNumberGenerated)
    {
        GenerateRandomNumber();
        randomNumberGenerated = true;
    }
    
    int numVertices = 4;
    graph = CreateCompleteGraph(numVertices);
     numbers = BFS(graph, randomNumber);
    foreach (int num in numbers)
    {
        Debug.Log("Number: " + num);
    }
    PlayerManager playermanager = PlayerManager.Instance;
     SceneManager.LoadScene(numbers[currentSceneIndex]);
      playermanager.currentHealth = 100;
    
    
}

    void Start()
    {
        
        
       
    }
    public void SetPlayerHealth(int health)
    {
        PlayerHealth = health;
    }
    
     void GenerateRandomNumber()
    {
        randomNumber = Random.Range(0, 3);
        Debug.Log("Random Number: " + randomNumber);
    }
    
   


    void OnTriggerEnter2D(Collider2D other)
{
    PlayerManager playermanager = PlayerManager.Instance;
    if (other.CompareTag("Player"))
    {
        currentSceneIndex++;
        if (numbers.Count > 0 && currentSceneIndex < numbers.Count)
        {
            SceneManager.LoadScene(numbers[currentSceneIndex]);
            
        }
        else
        {
            

           SceneManager.LoadScene("Win");
            
           
        
        }
    }
}
    List<List<int>> CreateCompleteGraph(int numVertices)
    {
        

        for (int i = 0; i < numVertices; i++)
        {
            graph.Add(new List<int>());
        }
        for (int i = 0; i < numVertices; i++)
        {
            for (int j = i + 1; j < numVertices; j++)
            {
                graph[i].Add(j);
                graph[j].Add(i);
            }
        }

        return graph;
    }

    List<int> BFS(List<List<int>> graph, int start)
    {
        List<int> path = new List<int>();
        Queue<int> queue = new Queue<int>();
        bool[] visited = new bool[graph.Count];

        queue.Enqueue(start);
        visited[start] = true;

        while (queue.Count > 0)
        {
            int current = queue.Dequeue();
            path.Add(current);

            List<int> shuffledNeighbors = graph[current].OrderBy(x => UnityEngine.Random.value).ToList();

            foreach (int neighbor in shuffledNeighbors)
            {
                if (!visited[neighbor])
                {
                    queue.Enqueue(neighbor);
                    visited[neighbor] = true;
                }
            }
        }

        return path;
    }
}
