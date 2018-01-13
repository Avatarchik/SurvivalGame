using UnityEngine;
using System.Collections.Generic;

public class TerrainChunk : MonoBehaviour
{

    public List<GameObject> treeList;
    public List<GameObject> rockList;

    public bool hasTreesGen;

	const float colliderGenerationDistanceThreshold = 5;
	public event System.Action<TerrainChunk, bool> onVisibilityChanged;
	public Vector2 coord;

	GameObject meshObject;
	Vector2 sampleCentre;
	Bounds bounds;

	MeshRenderer meshRenderer;
	MeshFilter meshFilter;
	MeshCollider meshCollider;

	LODInfo[] detailLevels;
	LODMesh[] lodMeshes;
	int colliderLODIndex;

	HeightMap heightMap;
	bool heightMapReceived;
	int previousLODIndex = -1;
	bool hasSetCollider;
	float maxViewDst;

	HeightMapSettings heightMapSettings;
	MeshSettings meshSettings;
	Transform viewer;

    NetworkView view;

    TerrainGenerator gen;

    void Start()
    {
        view = meshObject.AddComponent<NetworkView>();
        gen = GameObject.FindObjectOfType<TerrainGenerator>();

        //view.RPC("TreesGenerated", RPCMode.AllBuffered, hasTreesGen);
    }

	public TerrainChunk(Vector2 coord, HeightMapSettings heightMapSettings, MeshSettings meshSettings, LODInfo[] detailLevels, int colliderLODIndex, Transform parent, Transform viewer, Material material) {

        this.coord = coord;
		this.detailLevels = detailLevels;
		this.colliderLODIndex = colliderLODIndex;
		this.heightMapSettings = heightMapSettings;
		this.meshSettings = meshSettings;
		this.viewer = viewer;

        treeList = GameObject.FindObjectOfType<TerrainGenerator>().treeObjects;
        rockList = GameObject.FindObjectOfType<TerrainGenerator>().rockObjects;

        sampleCentre = coord * meshSettings.meshWorldSize / meshSettings.meshScale;
		Vector2 position = coord * meshSettings.meshWorldSize ;
		bounds = new Bounds(position,Vector2.one * meshSettings.meshWorldSize );


		meshObject = new GameObject("Terrain Chunk");
		meshRenderer = meshObject.AddComponent<MeshRenderer>();
		meshFilter = meshObject.AddComponent<MeshFilter>();
		meshCollider = meshObject.AddComponent<MeshCollider>();
		meshRenderer.material = material;

		meshObject.transform.position = new Vector3(position.x,0,position.y);
		meshObject.transform.parent = parent;
        meshObject.gameObject.tag = "Terrain";
        meshObject.gameObject.layer = LayerMask.NameToLayer("Terrain");
		SetVisible(false);

		lodMeshes = new LODMesh[detailLevels.Length];
		for (int i = 0; i < detailLevels.Length; i++) {
			lodMeshes[i] = new LODMesh(detailLevels[i].lod);
			lodMeshes[i].updateCallback += UpdateTerrainChunk;
			if (i == colliderLODIndex) {
				lodMeshes[i].updateCallback += UpdateCollisionMesh;
			}
		}

        maxViewDst = detailLevels [detailLevels.Length - 1].visibleDstThreshold;
    }

	public void Load() {
		ThreadedDataRequester.RequestData(() => HeightMapGenerator.GenerateHeightMap (meshSettings.numVertsPerLine, meshSettings.numVertsPerLine, heightMapSettings, sampleCentre), OnHeightMapReceived);
	}



	void OnHeightMapReceived(object heightMapObject) {
		this.heightMap = (HeightMap)heightMapObject;
		heightMapReceived = true;

		UpdateTerrainChunk ();
	}

	Vector2 viewerPosition {
		get
        {
			return new Vector2 (viewer.position.x, viewer.position.z);
		}
	}

    public void UpdateTerrainChunk() {


		if (heightMapReceived)
        {
			float viewerDstFromNearestEdge = Mathf.Sqrt (bounds.SqrDistance (viewerPosition));

			bool wasVisible = IsVisible ();
			bool visible = viewerDstFromNearestEdge <= maxViewDst;

			if (visible) {
				int lodIndex = 0;

				for (int i = 0; i < detailLevels.Length - 1; i++) {
					if (viewerDstFromNearestEdge > detailLevels [i].visibleDstThreshold) {
						lodIndex = i + 1;
					} else {
						break;
					}
				}

				if (lodIndex != previousLODIndex) {
					LODMesh lodMesh = lodMeshes [lodIndex];
					if (lodMesh.hasMesh) {
						previousLODIndex = lodIndex;
						meshFilter.mesh = lodMesh.mesh;
					} else if (!lodMesh.hasRequestedMesh) {
						lodMesh.RequestMesh (heightMap, meshSettings);
					}
				}

            }

			if (wasVisible != visible) {
				
				SetVisible (visible);
				if (onVisibilityChanged != null) {
					onVisibilityChanged (this, visible);
				}
			}
		}
	}

	public void UpdateCollisionMesh() {

        if (!hasSetCollider)
        {
			float sqrDstFromViewerToEdge = bounds.SqrDistance (viewerPosition);

			if (sqrDstFromViewerToEdge < detailLevels [colliderLODIndex].sqrVisibleDstThreshold) {
				if (!lodMeshes [colliderLODIndex].hasRequestedMesh)
                {
					lodMeshes [colliderLODIndex].RequestMesh (heightMap, meshSettings);
				}
			}

			if (lodMeshes [colliderLODIndex].hasMesh)
            {
				meshCollider.sharedMesh = lodMeshes [colliderLODIndex].mesh;
				hasSetCollider = true;

                if (hasTreesGen == false)
                {

                    int treeNumber = Random.Range(20, 30);
                    for (int i = 0; i < treeNumber; i++)
                    {
                        CreateTree();
                        hasTreesGen = true;
                    }

                    int rockNumberOne = Random.Range(10, 20);
                    for (int j = 0; j < rockNumberOne; j++)
                    {
                        CreateRockLayer(0, 5);
                    }

                    int rockNumberTwo = Random.Range(1, 5);
                    for (int j = 0; j < rockNumberTwo; j++)
                    {
                        CreateRockLayer(5, 20);
                    }

                    int rockNumberThree = Random.Range(20, 30);
                    for (int j = 0; j < rockNumberThree; j++)
                    {
                        CreateRockLayer(25, 100);
                    }
                }
            }
		}
	}

    void CreateTree()
    {
        float treePosX = coord.x * 100 + Random.Range(-50, 50);
        float treePosZ = coord.y * 100 + Random.Range(-50, 50);

        RaycastHit hit;
        Ray ray = new Ray(new Vector3(treePosX, 100, treePosZ), Vector3.down);

        int treeType = Random.Range(0, treeList.Count);

        if(meshCollider.Raycast(ray, out hit, 2.0f * 100))
        {
        }

        if (hit.point.y > 5 && hit.point.y < 25)
        {
            Vector3 treePos = new Vector3(treePosX, hit.point.y + 7.5f, treePosZ);
            Quaternion treeRot = new Quaternion(267f, 0, Random.Range(0, 360), 0);
            GameObject curTree = Instantiate(treeList[treeType], treePos, treeRot) as GameObject;
            curTree.transform.eulerAngles = new Vector3(treeRot.x, treeRot.y, treeRot.z);
            curTree.transform.parent = meshObject.transform;
        }
    }

    void CreateRockLayer(float minHeight, float maxHeight)
    {
        float rockPosX = coord.x * 100 + Random.Range(-50, 50);
        float rockPosZ = coord.y * 100 + Random.Range(-50, 50);

        RaycastHit hit;
        Ray ray = new Ray(new Vector3(rockPosX, 100, rockPosZ), Vector3.down);

        int rockType = Random.Range(0, rockList.Count);

        if (meshCollider.Raycast(ray, out hit, 2.0f * 100))
        {
        }

        if (hit.point.y > minHeight && hit.point.y < maxHeight)
        {
            Vector3 rockPos = new Vector3(rockPosX, hit.point.y, rockPosZ);
            Quaternion rockRot = new Quaternion(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360), 0);
            GameObject curRock = Instantiate(rockList[rockType], rockPos, rockRot) as GameObject;
            curRock.transform.eulerAngles = new Vector3(rockRot.x, rockRot.y, rockRot.z);
            curRock.transform.parent = meshObject.transform;
        }
    }
 
    public void SetVisible(bool visible) {
		meshObject.SetActive (visible);
	}

	public bool IsVisible() {
		return meshObject.activeSelf;
	}

}

class LODMesh {

	public Mesh mesh;
	public bool hasRequestedMesh;
	public bool hasMesh;
	int lod;
	public event System.Action updateCallback;

	public LODMesh(int lod) {
		this.lod = lod;
	}

	void OnMeshDataReceived(object meshDataObject) {
		mesh = ((MeshData)meshDataObject).CreateMesh ();
		hasMesh = true;

		updateCallback ();
	}

	public void RequestMesh(HeightMap heightMap, MeshSettings meshSettings) {
		hasRequestedMesh = true;
		ThreadedDataRequester.RequestData (() => MeshGenerator.GenerateTerrainMesh (heightMap.values, meshSettings, lod), OnMeshDataReceived);
	}

}
