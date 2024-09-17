class FatTable{
    public string FileName {get; set;}
    public string Directory {get; set;}
    public bool ReciclynBin {get; set;}
    public int Chartotal {get; set;}
    public string CreatedDate {get; set;}
    public string ModificatedDate {get; set;}
    public string EliminatedDate {get; set;}

    public FatTable(string file, string directory, int total){
        FileName = file;
        Directory = directory;
        ReciclynBin = false;
        Chartotal = total;
        CreatedDate = DateTime.Now.ToString();
        ModificatedDate = "";
        EliminatedDate = "";
    }

}