class FatTable{
    public string FileName {get; set;}
    public string Directory {get; set;}
    public bool ReciclynBin {get; set;}
    public int Chartotal {get; set;}
    public string CreatedDate {get; set;}
    public string ModificatedDate {get; set;}
    public string EliminatedDate {get; set;}

    public FatTable(string filename, string directory, int chartotal){
        FileName = filename;
        Directory = directory;
        ReciclynBin = false;
        Chartotal = chartotal;
        CreatedDate = DateTime.Now.ToString();
        ModificatedDate = "";
        EliminatedDate = "";
    }

    public string Info(){
        string temp = ModificatedDate;

        if (ModificatedDate == ""){
            temp = "N/A";
        }
        
        string temp2 = EliminatedDate;

        if (EliminatedDate == ""){
            temp2 = "N/A";
        }

        return $"Nombre del Archivo: {FileName} - No. Caracteres: {Chartotal} - Fecha Creación: {CreatedDate} - Fecha Modificación: {temp} - Fecha Eliminación: {temp2}";
    }

}