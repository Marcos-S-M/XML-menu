using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using System;
using System.IO;
using System.Xml.Serialization;
using static UnityEngine.LightProbeProxyVolume;
using System.Linq;
using TMPro;

public class ScpMenuCotrole : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public GameObject menuInicial,menuOptions, rawImage;
    public AudioSource audioSelecao;
    private Animator animatorRawImage;
    public string novaScena;

    //Variavel para salvar configurações
    public TMP_Dropdown resolucaoControler, qualidadeControler;
    public TMP_InputField textFPSControler;
    public Toggle limitarFPSControler, modoJanelaControler, autoSaveControler;
    public Slider geralVolumeControler, musicaVolumeControler, efeitosVolumeControler;
    

    void Start()
    {
        CarregarSave();
        animatorRawImage = rawImage.GetComponent<Animator>();
        rawImage.SetActive(false);
        menuInicial.SetActive(false);
        menuOptions.SetActive(false);
    }
    
    void Update()
    {
        
        if (!videoPlayer.isPlaying && Input.anyKeyDown) 
        {
            rawImage.SetActive(true);
            menuInicial.SetActive(true);
            audioSelecao.Play();
            videoPlayer.Play();
            animatorRawImage.SetTrigger("trasicao");
        }
        if (Input.GetKey(KeyCode.T))
        {
            CarregarSave();
        }
    }
    public void Options()
    {
        CarregarSave();
        menuInicial.SetActive(false);
        menuOptions.SetActive(true);
    }

    public void RetornarMenu()
    {
        menuInicial.SetActive(true);
        menuOptions.SetActive(false);
    }

    public void RetornarMenuESalvar()
    {
        SaveConfig();
        CarregarSave();
        RetornarMenu();
    }

    private void SaveConfig()
    {
        
        
        var resolutionModelo = new Resolucao();

        switch (resolucaoControler.value)
        {
            case 0:
                resolutionModelo.width = 3840;
                resolutionModelo.height = 2160;
                break;
            case 1:
                resolutionModelo.width = 1920;
                resolutionModelo.height = 1080;
                break;
            case 2:
                resolutionModelo.width = 1024;
                resolutionModelo.height = 768;
                break;
            default:
                Debug.LogWarning("Resolução desconhecida. Valor padrão será usado.");
                break;
        }

        var config = new ScpConfig()
        {
            //Auto save
            autoSave = autoSaveControler.isOn,
            //Resolução
            modoJanela = modoJanelaControler.isOn,
            resolucao = resolutionModelo,
            //volume
            geralVolume = geralVolumeControler.value,
            efeitosVolume = efeitosVolumeControler.value,
            musicaVolume = musicaVolumeControler.value,
            //Fps
            limiteFPS = new LimiteFPS()
            {
                fps = int.Parse(textFPSControler.text),
                limitar = limitarFPSControler.isOn
            },
            //Qualidade
            qualidade = (Qualidade)qualidadeControler.value
        };

        //-------------------Salvar--------------------- 
       
        //ScpConfig save = new ScpConfig();

        //caminho do arquivo
        string filePath = Path.Combine(Application.streamingAssetsPath, "Save.xml");
        
        //Se o diretori (caminho) não existe ele deve ser criado
        if (!Directory.Exists(Application.streamingAssetsPath))
        {
            Directory.CreateDirectory(Application.streamingAssetsPath);
        }
        //Se o arquivo não existe ele cria
        if (!File.Exists(filePath))
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ScpConfig));
            StreamWriter streamWriter = new StreamWriter(filePath);
            serializer.Serialize(streamWriter.BaseStream, config);
            streamWriter.Close();
        }
        //Se o arquivo existe ele cria
        else
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ScpConfig));
            StreamReader reader = new StreamReader(filePath);

            reader.Close();

            StreamWriter writer = new StreamWriter(filePath);
            xmlSerializer.Serialize(writer.BaseStream, config);
            writer.Close();
        }
    }

    public void CarregarSave()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "Save.xml");

        if (File.Exists(filePath))
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ScpConfig));
            StreamReader reader = new StreamReader(filePath);

            // carrega o save
            ScpConfig config = (ScpConfig)xmlSerializer.Deserialize(reader.BaseStream);
            reader.Close();

       
            //Auto save
            autoSaveControler.isOn = config.autoSave;
            //Resolução
            modoJanelaControler.isOn = config.modoJanela;
            var option = resolucaoControler.options.FirstOrDefault(x => x.text == $"{config.resolucao.width} x {config.resolucao.height}");
            
            if (option != null)
            {
                resolucaoControler.value = resolucaoControler.options.IndexOf(option);
            }
            else
            {
                Debug.LogWarning("Resolução salva não encontrada no dropdown.");
                
            }
            //fps
            textFPSControler.text = config.limiteFPS.fps.ToString();
            limitarFPSControler.isOn = config.limiteFPS.limitar;
            //Audio
            geralVolumeControler.value = config.geralVolume;
            musicaVolumeControler.value = config.musicaVolume;
            efeitosVolumeControler.value = config.efeitosVolume;
            //Qualidade
            qualidadeControler.value = (int)config.qualidade;




        }
        else
        {
            Debug.LogWarning("Arquivo de save não encontrado!");
        }
    }
    

    public void NewGame()
    {
        CarregarSave();
        SceneManager.LoadScene(novaScena);
    }


}
