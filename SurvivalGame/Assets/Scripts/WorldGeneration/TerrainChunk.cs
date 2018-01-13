using UnityEngine;
using System.Collections.Generic;

public class TerrainChunk {

    public List<GameObject> treeList;

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
    }

	public TerrainChunk(Vector2 coord, HeightMapSettings heightMapSettings, MeshSettings meshSettings, LODInfo[] detailLevels, int colliderLODIndex, Transform parent, Transform viewer, Material material) {

        this.coord = coord;
		this.detailLevels = detailLevels;
		this.colliderLODIndex = colliderLODIndex;
		this.heightMapSettings = heightMapSettings;
		this.meshSettings = meshSettings;
		this.viewer = viewer;

        treeList = GameObject.FindObjectOfType<TerrainGenerator>().treeObjects;

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

                if (hasTreesGen == false) {
                    int treeNumber = Random.Range(5, 15);
                    Debug.Log("Trees Generated: " + treeNumber);

                    for (int i = 0; i < treeNumber; i++)
                    {
                        CreateTree();
                        hasTreesGen = true;
                        view.RPC("TreesGenerated", RPCMode.AllBuffered, hasTreesGen);
                    }
                }
            }
		}
	}

    [RPC]
    void TreesGenerated(bool receivedGen)
    {
        hasTreesGen = receivedGen;
    }

    void CreateTree()
    {
        float treePosX = coord.x * 100 + Random.Range(-50, 50);
        float treePosZ = coord.y * 100 + Random.Range(-50, 50);

        RaycastHit hit;
        Ray ray = new Ray(new Vector3(treePosX, 100, treePosZ), Vector3.down);


        if(meshCollider.Raycast(ray, out hit, 2.0f * 100))
        {
            Debug.Log("hit Point" + hit.point);
        }

        if (hit.point.y > 5 && hit.point.y < 25)
        {
            Vector3 treePos = new Vector3(treePosX, hit.point.y + 7.5f, treePosZ);
            Quaternion treeRot = new Quaternion(267f, 0, Random.Range(0, 360), 0);
            GameObject curTree = Network.Instantiate(treeList[0], treePos, treeRot, 0) as GameObject;
            curTree.transform.eulerAngles = new Vector3(treeRot.x, treeRot.y, treeRot.z);
            curTree.transform.parent = meshObject.transform;

            Debug.Log("Created Tree at " + treePos);
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
