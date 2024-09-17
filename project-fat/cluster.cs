class Cluster{
    public string Data {get; set;}
    public string? NextFile {get; set;}
    public bool Eof {get; set;}
    public Cluster(string data, string? nextfile, bool eof){
        Data = data;
        NextFile = nextfile;
        Eof = eof;
    }
}