using System.Text.Json;

static void create_file(string namefile, string data){
    string actual_directory = Directory.GetCurrentDirectory();
    string filePath = $"{actual_directory}/Tables.json";

    if (!Directory.Exists($"{actual_directory}\\Clusters")){
        Directory.CreateDirectory($"{actual_directory}\\Clusters");
    }
    
    FatTable NewTable = new(namefile, $"{actual_directory}\\Clusters\\{namefile}_init.json", data.Length);

    try{
        string jsonFromFile = File.ReadAllText(filePath);

        List<FatTable> deserializedTables = JsonSerializer.Deserialize<List<FatTable>>(jsonFromFile)!;
        
        deserializedTables.Add(NewTable);
            
        string jsonString = JsonSerializer.Serialize(deserializedTables);

        File.WriteAllText(filePath, jsonString);

    }
    catch
    {   
        List<FatTable> Tables = [NewTable];

        string jsonString2 = JsonSerializer.Serialize(Tables);

        File.WriteAllText(filePath, jsonString2);
    }
    
    string temp = "";
    int i = 0;
    int createdfiles = 0;

    string nextFilePath = $"{actual_directory}\\Clusters";

    foreach (char letter in data)
    {        
        i ++;
        temp += letter;
        
        if (i <= 20){
            filePath = $"{actual_directory}\\Clusters\\{namefile}_init.json";
        }
        else{
            filePath = $"{actual_directory}\\Clusters\\{namefile}_{createdfiles}.json";
        }

        if (i == data.Length){
            Cluster newCluster = new(temp, null, true);
            string jsonString = JsonSerializer.Serialize(newCluster);

            File.WriteAllText(filePath, jsonString);
            createdfiles ++;
        }
        else if (i % 20 == 0){
            Cluster newCluster = new(temp, nextFilePath + $"\\{namefile}_{i / 20}.json", false);
            string jsonString = JsonSerializer.Serialize(newCluster);

            File.WriteAllText(filePath, jsonString);
            
            temp = "";
            createdfiles ++;


        }
    }
}

static void main(){
    while (true){
        Console.WriteLine("Bienvenido a mi Sistema de Simulación de FAT");
        Console.WriteLine("1.- Crear un archivo\n2.- Buscar una palabra y mostrar su definición");
        Console.WriteLine("3.- Listar palabras y sus definiciones en orden alfabético\n4.- Eliminar una palabra");
        Console.WriteLine("5.- Restaurar una palabra eliminada\n6.- Salir");

        string op = Console.ReadLine()!;

        if (op == "1"){
            Console.WriteLine("Nombre del Archivo:");
            string namefile = Console.ReadLine()!;

            Console.WriteLine("Contenido a Guardar:");
            string data = Console.ReadLine()!;

            create_file(namefile, data);
        }

        if (op == "6"){
            Console.WriteLine("Cerrando...");
            break;
        }
        else{
            Console.ReadKey();
            Console.Clear();
            continue;
        }
    }
}

main();