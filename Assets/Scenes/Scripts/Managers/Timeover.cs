using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Timeover : MonoBehaviour
{
    public float waktuLevel1 = 300f; // Waktu untuk level 1 (5 menit)
    public float waktuLevel2 = 480f; // Waktu untuk level 2 (8 menit)
    public int levelSaatIni = 1; // Level saat ini
    public Text textWaktu; // Referensi UI untuk menampilkan waktu
    public Text textGameOver; // Referensi UI untuk pesan Game Over
    public GameObject efekWaktuHabis; // Efek visual ketika waktu habis
    public AudioClip suaraWaktuHabis; // Suara ketika waktu habis
    public AudioSource audioSource; // Komponen Audio Source
    private float waktuSisa; // Waktu yang tersisa
    private bool paused = false; // Status paused game

    void Start()
    {
        MulaiWaktu();
        textGameOver.enabled = false;
        efekWaktuHabis.SetActive(false);
    }

    void Update()
    {
        // Periksa input untuk pause/resume
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (paused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }

        // Update timer jika tidak dalam keadaan pause
        if (!paused)
        {
            waktuSisa -= Time.deltaTime;

            // Pastikan waktu tidak negatif
            if (waktuSisa < 0)
            {
                waktuSisa = 0;
            }

            // Update teks waktu dengan format MM:SS
            int menit = Mathf.FloorToInt(waktuSisa / 60);
            int detik = Mathf.FloorToInt(waktuSisa % 60);
            textWaktu.text = $"Waktu: {menit:D2}:{detik:D2}";

            // Jika waktu habis, jalankan logika TimeOver
            if (waktuSisa <= 0)
            {
                TimeOver();
            }
        }
    }

    void MulaiWaktu()
    {
        // Set waktu sesuai level
        switch (levelSaatIni)
        {
            case 1:
                waktuSisa = waktuLevel1;
                break;
            case 2:
                waktuSisa = waktuLevel2;
                break;
            default:
                waktuSisa = waktuLevel1; // Default ke level 1 jika tidak ada level lain
                break;
        }

        // Reset status game over dan efek visual
        textGameOver.enabled = false;
        efekWaktuHabis.SetActive(false);
    }

    void TimeOver()
    {
        // Efek visual dan suara ketika waktu habis
        efekWaktuHabis.SetActive(true);
        if (audioSource && suaraWaktuHabis)
        {
            audioSource.PlayOneShot(suaraWaktuHabis);
        }
        textGameOver.enabled = true;
        textGameOver.text = "Game Over!";
    }

    void Pause()
    {
        paused = true;
        Time.timeScale = 0; // Hentikan waktu
    }

    void Resume()
    {
        paused = false;
        Time.timeScale = 1; // Lanjutkan waktu
    }

    public void NaikLevel()
    {
        levelSaatIni++; // Pindah ke level berikutnya
        MulaiWaktu(); // Reset waktu
    }
}
