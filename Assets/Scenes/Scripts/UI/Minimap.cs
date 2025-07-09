using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Minimap : MonoBehaviour
{
    public RenderTexture minimapTexture;
    public RawImage minimapDisplay;
    public int minimapSize = 256;
    public float zoom = 10f;
    public GameObject player;
    public GameObject playerMarker;
    public List<GameObject> enemies;
    public GameObject enemyMarkerPrefab;
    private List<GameObject> enemyMarkers;
    public float minX = -50f, maxX = 50f;
    public float minZ = -50f, maxZ = 50f;
    private Camera minimapCamera;

    void Start()
    {
        // Periksa apakah semua referensi telah disambungkan
        if (player == null || playerMarker == null || enemyMarkerPrefab == null)
        {
            Debug.LogError("Minimap: Pastikan semua referensi telah disambungkan di Inspector.");
            return;
        }

        // Buat render texture jika belum ada
        if (minimapTexture == null)
        {
            minimapTexture = new RenderTexture(minimapSize, minimapSize, 16);
        }

        // Buat kamera untuk minimap
        minimapCamera = new GameObject("Minimap Camera").AddComponent<Camera>();
        minimapCamera.transform.parent = transform;
        minimapCamera.orthographic = true;
        minimapCamera.orthographicSize = zoom;
        minimapCamera.targetTexture = minimapTexture;

        // Sambungkan render texture ke UI RawImage
        if (minimapDisplay != null)
        {
            minimapDisplay.texture = minimapTexture;
        }

        // Buat marker untuk setiap enemy
        enemyMarkers = new List<GameObject>();
        foreach (GameObject enemy in enemies)
        {
            if (enemyMarkerPrefab != null)
            {
                GameObject marker = Instantiate(enemyMarkerPrefab, minimapDisplay.transform);
                marker.SetActive(true);
                enemyMarkers.Add(marker);
            }
        }
    }

    void Update()
    {
        if (player == null || playerMarker == null || minimapCamera == null) return;

        // Update zoom menggunakan scroll mouse
        zoom -= Input.GetAxis("Mouse ScrollWheel") * 2;
        zoom = Mathf.Clamp(zoom, 5f, 20f);
        minimapCamera.orthographicSize = zoom;

        // Update posisi marker player
        if (playerMarker != null)
        {
            Vector3 markerPos = WorldToMinimap(player.transform.position);
            playerMarker.transform.localPosition = markerPos;
        }

        // Update posisi marker enemy
        for (int i = enemyMarkers.Count - 1; i >= 0; i--)
        {
            if (enemies[i] == null)
            {
                Destroy(enemyMarkers[i]);
                enemyMarkers.RemoveAt(i);
                enemies.RemoveAt(i);
            }
            else
            {
                Vector3 markerPos = WorldToMinimap(enemies[i].transform.position);
                enemyMarkers[i].transform.localPosition = markerPos;
            }
        }
    }

    void LateUpdate()
    {
        if (player == null || minimapCamera == null) return;

        // Update posisi kamera minimap mengikuti player
        Vector3 newCameraPos = player.transform.position;
        newCameraPos.y += 10f; // Pastikan kamera berada di atas
        newCameraPos.x = Mathf.Clamp(newCameraPos.x, minX, maxX);
        newCameraPos.z = Mathf.Clamp(newCameraPos.z, minZ, maxZ);

        minimapCamera.transform.position = newCameraPos;
        minimapCamera.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
    }

    private Vector3 WorldToMinimap(Vector3 worldPos)
    {
        // Konversi posisi dunia ke posisi minimap
        float xNormalized = Mathf.InverseLerp(minX, maxX, worldPos.x);
        float zNormalized = Mathf.InverseLerp(minZ, maxZ, worldPos.z);

        float xMinimap = (xNormalized - 0.5f) * minimapSize;
        float yMinimap = (zNormalized - 0.5f) * minimapSize;

        return new Vector3(xMinimap, yMinimap, 0f);
    }
}
