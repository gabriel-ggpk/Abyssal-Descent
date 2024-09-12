using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridController : MonoBehaviour
{
    private float[,] noiseMap;
    private Chunk[,] chunkMap;
    private int chunkSize = 16;
    private CustomTile[] customTiles;
    private List<Vector3Int> pickPowerUpTransforms;

    [SerializeField] private TileBase caveTileBase;
    [SerializeField] private TileBase poisonTileBase;
    [SerializeField] private TileBase tundraTileBase;
    [SerializeField] private TileBase hellTileBase;
    [SerializeField] private TileBase voidTileBase;
    [SerializeField] private TileBase transitionTileBase;
    [SerializeField] private TileBase powerUpTileBase;
    [SerializeField] private TileBase bedRockTileBase;
    [SerializeField] private Player player;

    [SerializeField] public CompassLogic compass;

    // Armazena os chunks atualmente visíveis
    private HashSet<Chunk> activeChunks = new HashSet<Chunk>();

    // Start is called before the first frame update
    void Start()
    {
        // Inicializa os CustomTiles com base nos TileBases
        customTiles = new CustomTile[50];
        customTiles[0] = new CustomTile(caveTileBase, 0);
        customTiles[1] = new CustomTile(caveTileBase, 1);
        customTiles[2] = new CustomTile(tundraTileBase, 2);
        customTiles[3] = new CustomTile(poisonTileBase, 3);
        customTiles[4] = new CustomTile(hellTileBase, 4);
        customTiles[5] = new CustomTile(voidTileBase, 5);
        customTiles[6] = new CustomTile(powerUpTileBase, 99);

        customTiles[7] = new CustomTile(transitionTileBase, 11);
        customTiles[8] = new CustomTile(transitionTileBase, 21);
        customTiles[9] = new CustomTile(transitionTileBase, 31);
        customTiles[10] = new CustomTile(transitionTileBase, 41);
        customTiles[11] = new CustomTile(transitionTileBase, 51);

        pickPowerUpTransforms = new List<Vector3Int>();

        System.Random random = new System.Random();
        TerrainGenerator generator = new TerrainGenerator();
        this.noiseMap = generator.buildNoiseMap(random.Next(0, 3000));
        // Chama o método para construir os chunks
        BuildChunks();
        foreach (Vector2Int position in pickPowerUpTransforms)
        {
            Debug.Log(position);
        }

        compass.destination = pickPowerUpTransforms[0];

    }

    // Update is called once per frame
    void Update()
    {
        Vector2Int playerTransform = GetPositionByTag("Player");
        // Atualiza chunks com base na posição do jogador
        UpdateVisibleChunks(playerTransform);

        Vector2Int clickPosition = GetMouseClickPosition();
        if (clickPosition != Vector2Int.zero)
        {
            Debug.Log("quebrando");
            float distance = Vector2.Distance(playerTransform, clickPosition);
            if (distance < 2)
            {
               
                RemoveTile(clickPosition);
            }
        }
    }

    // Constrói todos os chunks e os armazena em chunkMap
    private void BuildChunks()
    {
        int chunkHeightCount = noiseMap.GetLength(1) / chunkSize;  // Número de linhas (altura)
        int chunkWidthCount = noiseMap.GetLength(0) / chunkSize;   // Número de colunas (largura)
        chunkMap = new Chunk[chunkWidthCount, chunkHeightCount];

        int idController = 0;

        for (int y = 0; y < chunkHeightCount; y++)
        {
            for (int x = 0; x < chunkWidthCount; x++)
            {
                Vector2Int topLeft = new Vector2Int(x * chunkSize, -y * chunkSize);
                Vector2Int botRight = new Vector2Int(topLeft.x + chunkSize, topLeft.y - chunkSize);
                Chunk chunk = new Chunk(topLeft, botRight, idController, transform);  // Passe o transform do Grid

                // Preenche o chunk com tiles baseados no noiseMap
                for (int tileY = topLeft.y; tileY > botRight.y; tileY--)
                {
                    for (int tileX = topLeft.x; tileX < botRight.x; tileX++)
                    {
                        int noiseValue = Mathf.FloorToInt(noiseMap[tileX, -tileY]);  // Recupera o valor do noiseMap como ID

                        if (noiseValue == 99)
                        {
                            pickPowerUpTransforms.Add(new Vector3Int(tileX, tileY, 0));
                        }
                        // Verifica se o ID do noiseMap é válido
                        if (noiseValue >= 0 && noiseValue < 100)
                        {
                            CustomTile selectedTile = GetTileById(noiseValue);
                            Vector3Int position = new Vector3Int(tileX, tileY, 0);
                            chunk.setTile(position, selectedTile.tileBase);
                        }
                    }
                }

                chunk.UpdateTilemap();  // Atualiza o Tilemap após setar os tiles
                chunkMap[x, y] = chunk;  // Armazena o chunk no mapa
                idController++;
            }
        }
    }

    // Atualiza os chunks visíveis com base na posição do jogador
    public void UpdateVisibleChunks(Vector2Int playerPosition)
    {
        // Inverter o Y para considerar o eixo invertido
        Vector2Int currentChunkCoord = new Vector2Int(playerPosition.x / chunkSize, -playerPosition.y / chunkSize);

        // Apenas manter a matriz 3x3 ao redor do jogador visível
        HashSet<Chunk> newActiveChunks = new HashSet<Chunk>();

        for (int y = -1; y <= 1; y++)
        {
            for (int x = -1; x <= 1; x++)
            {
                int chunkX = currentChunkCoord.x + x;
                int chunkY = currentChunkCoord.y + y;

                // Verificar se o chunk está dentro dos limites do mapa
                if (chunkX >= 0 && chunkX < chunkMap.GetLength(0) && chunkY >= 0 && chunkY < chunkMap.GetLength(1))
                {
                    Chunk chunk = chunkMap[chunkX, chunkY];
                    newActiveChunks.Add(chunk);

                    // Se o chunk não estava ativo antes, ativa ele
                    if (!activeChunks.Contains(chunk))
                    {
                        chunk.SetActive(true);
                    }
                }
            }
        }

        // Desativa os chunks que não estão mais na matriz 3x3
        foreach (Chunk chunk in activeChunks)
        {
            if (!newActiveChunks.Contains(chunk))
            {
                chunk.SetActive(false);
            }
        }

        // Atualiza os chunks ativos
        activeChunks = newActiveChunks;
    }





    public Vector2Int GetPositionByTag(string tag)
    {
        GameObject obj = GameObject.FindWithTag(tag);

        if (obj != null)
        {
            Vector3 worldPosition = obj.transform.position;
            return new Vector2Int(Mathf.FloorToInt(worldPosition.x), Mathf.FloorToInt(worldPosition.y));
        }
        else
        {
            return Vector2Int.zero;
        }
    }

    public Vector2Int GetMouseClickPosition()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseScreenPosition = Input.mousePosition;
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mouseScreenPosition.x, mouseScreenPosition.y, Camera.main.nearClipPlane));
            return new Vector2Int(Mathf.FloorToInt(worldPosition.x), Mathf.FloorToInt(worldPosition.y));
        }

        return Vector2Int.zero;
    }

    public void RemoveTile(Vector2Int tilePosition)
    {
        int chunkX = Mathf.FloorToInt(tilePosition.x / (float)chunkSize);
        int chunkY = Mathf.FloorToInt(-tilePosition.y / (float)chunkSize);

        int noiseValue = Mathf.FloorToInt(noiseMap[tilePosition.x, -tilePosition.y]);  // Recupera o valor do noiseMap como ID
        //função que recupera a picareta do player baseado nas transitions bounds
        if(noiseValue == 11 || noiseValue == 21 || noiseValue == 31 || noiseValue == 41)
        {
            Debug.Log("aaaaaaaa");
            if (player.power < noiseValue) return;
        }
            if (chunkX >= 0 && chunkX < chunkMap.GetLength(0) && chunkY >= 0 && chunkY < chunkMap.GetLength(1))
            {
                Chunk chunk = chunkMap[chunkX, chunkY];
                Vector3Int tileConvertPosition = (Vector3Int)tilePosition;
                chunk.RemoveTile(tileConvertPosition);
                if (pickPowerUpTransforms.Contains(tileConvertPosition))
                {
                pickPowerUpTransforms.RemoveAt(0);
                player.power += 10;
                compass.destination = pickPowerUpTransforms[0];
                foreach (Vector2Int position in pickPowerUpTransforms)
                {
                    Debug.Log(position);
                }

            }
        }
    }

    private CustomTile GetTileById(int id)
    {
        foreach (CustomTile customTile in customTiles)
        {
            if (customTile.id == id)
            {
                return customTile;
            }
        }
        return null;
    }

    // Classe Chunk corrigida
    private class Chunk
    {
        private GameObject tileMapObject;
        private Tilemap tileMap;
        private TilemapRenderer tilemapRenderer;
        private TilemapCollider2D tilemapCollider;
        private Tuple<Vector2Int, Vector2Int> pair;

        public Chunk(Vector2Int position1, Vector2Int position2, int id, Transform parent)
        {
            this.pair = new Tuple<Vector2Int, Vector2Int>(position1, position2);
            this.tileMapObject = new GameObject("chunk_" + id);
            this.tileMapObject.transform.parent = parent;
            this.tileMapObject.transform.localPosition = Vector3.zero;

            this.tileMap = tileMapObject.AddComponent<Tilemap>();
            this.tilemapRenderer = tileMapObject.AddComponent<TilemapRenderer>();
            this.tilemapCollider = tileMapObject.AddComponent<TilemapCollider2D>();
            tileMapObject.AddComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            tileMapObject.AddComponent<CompositeCollider2D>();

            this.tileMapObject.layer = LayerMask.NameToLayer("TileMap");
            tilemapCollider.usedByComposite = true;
            SetActive(false);
        }

        public void setTile(Vector3Int position, TileBase tile)
        {
            tileMap.SetTile(position, tile);
        }

        public void UpdateTilemap()
        {
            tileMap.CompressBounds();
            tileMap.RefreshAllTiles();
        }

        public void SetActive(bool isActive)
        {
            tileMapObject.SetActive(isActive);
        }

        public void RemoveTile(Vector3Int localPosition)
        {
            tileMap.SetTile(localPosition, null);
        }
    }

    private class CustomTile
    {
        public TileBase tileBase;
        public int id;

        public CustomTile(TileBase tileBase, int id)
        {
            this.tileBase = tileBase;
            this.id = id;
        }
    }
}