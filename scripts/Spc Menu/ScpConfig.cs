
public class ScpConfig 
{
    public Resolucao resolucao {  get; set; }
    public LimiteFPS limiteFPS {  get; set; }
    public Qualidade qualidade { get; set; }
    public bool modoJanela { get; set; }
    public bool autoSave {  get; set; }
    public float geralVolume { get; set; }
    public float musicaVolume { get; set; }
    public float efeitosVolume { get; set; }
}

public enum Qualidade
{
    Alta,
    Media,
    Baixa,

}

public class Resolucao 
{
    public int width { get; set; }
    public int height { get; set; }

}
public class LimiteFPS
{
    public bool limitar { get; set; }
    public int fps { get; set; }

}