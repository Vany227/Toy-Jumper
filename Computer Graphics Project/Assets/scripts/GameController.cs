using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Unity.VisualScripting;
using System.Linq;

public class GameController : MonoBehaviour
{
    private Vector3 Startposition;
    private Vector3Int startingPOS = new Vector3Int(0, 0, 0);
    private Grid grid;
    private Mode mode;
    public Transform[,] game_matrix = new Transform[3, 3];
    public enum Type { TWOXONE, TWOXTWO, THREEXTHREE };
    public enum Mode { TWOD, THREED};
    public List<Transform> screens = new List<Transform>();
    public Type m_type;
    public Transform Camera;
    public Transform Player;
    // Start is called before the first frame update
    void Start()
    {
        mode = Mode.TWOD;
        grid = this.GetComponent<Grid>();
        
        

        
        
        switch (m_type){
            case Type.TWOXTWO:
                for (int i = 0; i < screens.Count; i++)
                {
                    Transform currentScreen = screens[i];
                    Startposition = grid.CellToWorld(new Vector3Int(currentScreen.GetComponent<ScreenController>().GridX, currentScreen.GetComponent<ScreenController>().GridY, -30));
                    Startposition.Set(Startposition.x, Startposition.y, currentScreen.position.z);
                    currentScreen.position = Startposition;
                    game_matrix[currentScreen.GetComponent<ScreenController>().GridX, currentScreen.GetComponent<ScreenController>().GridY] = currentScreen;

                    if (currentScreen.GetComponent<ScreenController>().startingScreen)
                    {
                        Startposition.Set(Startposition.x + 16, Startposition.y + 16, -30);
                        Camera.position = Startposition;
                        Player.GetComponent<Character_Controller>().currentScreen = currentScreen;
                    }
                }
                break;
        }


        ScreenUpdate();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public class c_info
    {
        public List<int> rightSideYs = new List<int>();
        public List<int> leftSideYs = new List<int>();
        public List<int> upXs = new List<int>();
        public List<int> downXs = new List<int>();
        public Transform screen;
    }

    void ScreenUpdate()
    {
        List<c_info> connectionInformation = new List<c_info>();
        for (int i = 0; i < screens.Count; i++)
        {
            
            Transform currentScreen = screens[i];
            Tilemap[] connectors = currentScreen.GetChild(0).GetChild(0).GetChild(0).GetComponentsInChildren<Tilemap>();
            c_info currentInfo = new c_info();
            currentInfo.screen = currentScreen;


            for (int j = 0; j < connectors.Length; j++)
            {
                List<TileBase> connectorTiles = new List<TileBase>();
                Tilemap currentMap = connectors[j];
                BoundsInt bounds = currentMap.cellBounds;
                TileBase[] allTiles = currentMap.GetTilesBlock(bounds);
                
                for (int x = 0; x < bounds.size.x; x++)
                {
                    for (int y = 0; y < bounds.size.y; y++)
                    {
                        TileBase tile = allTiles[x + y * bounds.size.x];
                        if (tile != null)
                        {
                            switch (currentMap.name)
                            {
                                case "Right Side":
                                    //Debug.Log("Right Side");
                                    if (currentScreen.GetComponent<ScreenController>().GridX + 1 < 3 && game_matrix[currentScreen.GetComponent<ScreenController>().GridX + 1, currentScreen.GetComponent<ScreenController>().GridY] != null)
                                    {
                                        //Transform connectingScreen = game_matrix[currentScreen.GetComponent<ScreenController>().GridX + 1, currentScreen.GetComponent<ScreenController>().GridY];
                                        currentInfo.rightSideYs.Add(y);
                                    }
                                    break;
                                case "Left Side":
                                    //Debug.Log("Left Side");
                                    if (currentScreen.GetComponent<ScreenController>().GridX - 1 > -1 && game_matrix[currentScreen.GetComponent<ScreenController>().GridX - 1, currentScreen.GetComponent<ScreenController>().GridY] != null)
                                    {
                                        currentInfo.leftSideYs.Add(y);
                                    }
                                    break;
                                case "Top Side":
                                    if (currentScreen.GetComponent<ScreenController>().GridY + 1 < 3 && game_matrix[currentScreen.GetComponent<ScreenController>().GridX, currentScreen.GetComponent<ScreenController>().GridY + 1] != null)
                                    {
                                        currentInfo.upXs.Add(x);
                                    }
                                    break;
                                case "Bottom Side":
                                    if (currentScreen.GetComponent<ScreenController>().GridY - 1 > -1 && game_matrix[currentScreen.GetComponent<ScreenController>().GridX, currentScreen.GetComponent<ScreenController>().GridY - 1] != null)
                                    {
                                        currentInfo.downXs.Add(x);
                                    }
                                    break;
                            }
                        }
                    }
                } 
            }
            connectionInformation.Add(currentInfo);
        }

        for(int i = 0; i < connectionInformation.Count; i++)
        {
            if(connectionInformation[i].rightSideYs.Count > 0)
            {
                Transform otherScreen = game_matrix[connectionInformation[i].screen.GetComponent<ScreenController>().GridX + 1, connectionInformation[i].screen.GetComponent<ScreenController>().GridY];
                for(int temp = 0; temp < connectionInformation.Count; temp++)
                {
                    if(connectionInformation[temp].screen == otherScreen)
                    {
                        if (connectionInformation[i].rightSideYs.SequenceEqual(connectionInformation[temp].leftSideYs)){
                            connectionInformation[i].screen.GetChild(0).GetChild(0).GetChild(0).GetComponentsInChildren<TilemapRenderer>()[0].enabled = false;
                        }
                    }
                }
            }
            if (connectionInformation[i].leftSideYs.Count > 0)
            {
                Transform otherScreen = game_matrix[connectionInformation[i].screen.GetComponent<ScreenController>().GridX - 1, connectionInformation[i].screen.GetComponent<ScreenController>().GridY];
                for (int temp = 0; temp < connectionInformation.Count; temp++)
                {
                    if (connectionInformation[temp].screen == otherScreen)
                    {
                        if (connectionInformation[i].leftSideYs.SequenceEqual(connectionInformation[temp].rightSideYs))
                        {
                            connectionInformation[i].screen.GetChild(0).GetChild(0).GetChild(0).GetComponentsInChildren<TilemapRenderer>()[1].enabled = false;
                        }
                    }
                }
            }
            if (connectionInformation[i].upXs.Count > 0)
            {
                Transform otherScreen = game_matrix[connectionInformation[i].screen.GetComponent<ScreenController>().GridX, connectionInformation[i].screen.GetComponent<ScreenController>().GridY + 1];
                for (int temp = 0; temp < connectionInformation.Count; temp++)
                {
                    if (connectionInformation[temp].screen == otherScreen)
                    {
                        if (connectionInformation[i].upXs.SequenceEqual(connectionInformation[temp].downXs))
                        {
                            connectionInformation[i].screen.GetChild(0).GetChild(0).GetChild(0).GetComponentsInChildren<TilemapRenderer>()[2].enabled = false;
                        }
                    }
                }
            }
            if (connectionInformation[i].downXs.Count > 0)
            {
                Transform otherScreen = game_matrix[connectionInformation[i].screen.GetComponent<ScreenController>().GridX, connectionInformation[i].screen.GetComponent<ScreenController>().GridY - 1];
                for (int temp = 0; temp < connectionInformation.Count; temp++)
                {
                    if (connectionInformation[temp].screen == otherScreen)
                    {
                        if (connectionInformation[i].downXs.SequenceEqual(connectionInformation[temp].upXs))
                        {
                            connectionInformation[i].screen.GetChild(0).GetChild(0).GetChild(0).GetComponentsInChildren<TilemapRenderer>()[3].enabled = false;
                        }
                    }
                }
            }
        }
    }
}
