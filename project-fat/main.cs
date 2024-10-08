﻿using System.Text.Json;

static void create_file(string namefile, string data){
    string actual_directory = Directory.GetCurrentDirectory();
    string filePath = $"{actual_directory}/Tables.json";

    bool fileExist = false;

    if (!Directory.Exists($"{actual_directory}\\Clusters")){
        Directory.CreateDirectory($"{actual_directory}\\Clusters");
    }
    
    FatTable NewTable = new(namefile, $"{actual_directory}\\Clusters\\{namefile}_init.json", data.Length);

    try{
        string jsonFromFile = File.ReadAllText(filePath);

        List<FatTable> deserializedTables = JsonSerializer.Deserialize<List<FatTable>>(jsonFromFile)!;

        foreach(FatTable table in deserializedTables){
            if (table.FileName == namefile){
                fileExist = true;
                break;
            }
        }

        if (!fileExist){
            deserializedTables.Add(NewTable);
            
            string jsonString = JsonSerializer.Serialize(deserializedTables);

            File.WriteAllText(filePath, jsonString);
        }
        

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

static void show_files(){
    
    int i = 0;

    string actual_directory = Directory.GetCurrentDirectory();
    string filePath = $"{actual_directory}/Tables.json";

    try{    
        string jsonFromFile = File.ReadAllText(filePath);
        List<FatTable> deserializedTables = JsonSerializer.Deserialize<List<FatTable>>(jsonFromFile)!;

        foreach(FatTable table in deserializedTables){
            if (!table.ReciclynBin){
                i ++;
                Console.WriteLine($"{i}. {table.Info()}");
            }
        }
    }
        
    catch (Exception ex)
    {   
        Console.WriteLine(ex.Message);
        Console.WriteLine("Vaya, parece que se encuentra vacío el terreno.");
    }
    
}

static FatTable? show_file(int fileNumber){
    int i = 0;

    string actual_directory = Directory.GetCurrentDirectory();
    string filePath = $"{actual_directory}/Tables.json";

    try{    
        string jsonFromFile = File.ReadAllText(filePath);
        List<FatTable> deserializedTables = JsonSerializer.Deserialize<List<FatTable>>(jsonFromFile)!;

        foreach(FatTable table in deserializedTables){
            if (!table.ReciclynBin){
                i ++;
                if (i == fileNumber){
                    Console.WriteLine(table.Info());
                    Console.WriteLine(transversal(table.Directory, ""));
                    return table;
                }
            }
        }
    }
        
    catch
    {   
        Console.WriteLine("Vaya, parece que ese archivo no lo encuentro.");
    }

    return null;

}

static string transversal(string? directory, string text){
    
    if (directory == null){
        return "";
    }

    string jsonFromFile = File.ReadAllText(directory);
    Cluster cluster = JsonSerializer.Deserialize<Cluster>(jsonFromFile)!;

    if (cluster.Eof){
        return text += cluster.Data;
    }
    else{
        return text += cluster.Data + transversal(cluster.NextFile, text);
    }

}

static void modificate_file(int fileNumber, string data){
    int i = 0;

    string actual_directory = Directory.GetCurrentDirectory();
    string filePath = $"{actual_directory}/Tables.json";

    string jsonFromFile = File.ReadAllText(filePath);
    List<FatTable> deserializedTables = JsonSerializer.Deserialize<List<FatTable>>(jsonFromFile)!;

    foreach(FatTable table in deserializedTables){
        if (!table.ReciclynBin){
            i ++;
            if (i == fileNumber){
                Console.WriteLine(table.Info());
                Console.WriteLine(transversal(table.Directory, ""));
                string? currentDirectory = table.Directory;
                while (true){
                    
                    if (currentDirectory == null){
                        break;
                    }

                    jsonFromFile = File.ReadAllText(currentDirectory);
                    Cluster current = JsonSerializer.Deserialize<Cluster>(jsonFromFile)!;
                    File.Delete(currentDirectory);

                    if (current.Eof == true){
                        break;
                    }
                    else
                    {
                        currentDirectory = current.NextFile;
                    }
                }

                create_file(table.FileName, data);
                table.ModificatedDate = DateTime.Now.ToString();
                table.Chartotal = data.Length;

                string jsonString = JsonSerializer.Serialize(deserializedTables);

                File.WriteAllText(filePath, jsonString);

            }
        }
    }
}

static void delete_(int fileNumber){
    int i = 0;

    string actual_directory = Directory.GetCurrentDirectory();
    string filePath = $"{actual_directory}/Tables.json";

    string jsonFromFile = File.ReadAllText(filePath);
    List<FatTable> deserializedTables = JsonSerializer.Deserialize<List<FatTable>>(jsonFromFile)!;

    foreach(FatTable table in deserializedTables){
            if (!table.ReciclynBin){
                i ++;
                if (i == fileNumber){
                    table.ReciclynBin = true;
                    table.EliminatedDate = DateTime.Now.ToString();

                    string jsonString = JsonSerializer.Serialize(deserializedTables);
                    File.WriteAllText(filePath, jsonString);

                }
            }
        }
}


static void backup_(int fileNumber){
    int i = 0;

    string actual_directory = Directory.GetCurrentDirectory();
    string filePath = $"{actual_directory}/Tables.json";

    string jsonFromFile = File.ReadAllText(filePath);
    List<FatTable> deserializedTables = JsonSerializer.Deserialize<List<FatTable>>(jsonFromFile)!;

    foreach(FatTable table in deserializedTables){
            if (table.ReciclynBin){
                i ++;
                Console.WriteLine($"{i}. {table.Info()}");

                if (i == fileNumber){
                    table.ReciclynBin = false;
                    table.EliminatedDate = DateTime.Now.ToString();

                    string jsonString = JsonSerializer.Serialize(deserializedTables);
                    File.WriteAllText(filePath, jsonString);

                }
            }
        }
} 

static void show_deleted_files(){
    
    int i = 0;

    string actual_directory = Directory.GetCurrentDirectory();
    string filePath = $"{actual_directory}/Tables.json";

    try{    
        string jsonFromFile = File.ReadAllText(filePath);
        List<FatTable> deserializedTables = JsonSerializer.Deserialize<List<FatTable>>(jsonFromFile)!;

        foreach(FatTable table in deserializedTables){
            if (table.ReciclynBin){
                i ++;
                Console.WriteLine($"{i}. {table.Info()}");
            }
        }
    }
        
    catch (Exception ex)
    {   
        Console.WriteLine(ex.Message);
        Console.WriteLine("Vaya, parece que se encuentra vacío el terreno.");
    }
    
}

static FatTable? show_deleted_file(int fileNumber){
    int i = 0;

    string actual_directory = Directory.GetCurrentDirectory();
    string filePath = $"{actual_directory}/Tables.json";

    try{    
        string jsonFromFile = File.ReadAllText(filePath);
        List<FatTable> deserializedTables = JsonSerializer.Deserialize<List<FatTable>>(jsonFromFile)!;

        foreach(FatTable table in deserializedTables){
            if (table.ReciclynBin){
                i ++;
                if (i == fileNumber){
                    Console.WriteLine(table.Info());
                    Console.WriteLine(transversal(table.Directory, ""));
                    return table;
                }
            }
        }
    }
        
    catch
    {   
        Console.WriteLine("Vaya, parece que ese archivo no lo encuentro.");
    }

    return null;

}

static void main(){
    while (true){
        Console.WriteLine("Bienvenido a mi Sistema de Simulación de FAT");
        Console.WriteLine("1.- Crear un archivo\n2.- Listar Archivos");
        Console.WriteLine("3.- Abrir un Archivo\n4.- Modificar un Archivo");
        Console.WriteLine("5.- Eliminar un Archivo\n6.- Recuperar un Archivo");
        Console.WriteLine("7.- Salir");

        string op = Console.ReadLine()!;

        if (op == "1"){
            Console.WriteLine("---CREAR ARCHIVOS---");

            Console.WriteLine("Nombre del Archivo:");
            string namefile = Console.ReadLine()!;

            Console.WriteLine("Contenido a Guardar:");
            string data = Console.ReadLine()!;

            create_file(namefile, data);
        }

        if (op == "2"){
            Console.WriteLine("---LISTAR ARCHIVOS---");

            show_files();
        }
        
        if (op == "3"){
            Console.WriteLine("---ABRIR UN ARCHIVO---");

            show_files();

            Console.WriteLine("¿Qué archivo deseas visualizar?");
            string opfile = Console.ReadLine()!;

            show_file(Convert.ToInt32(opfile));
        }
        
        if (op == "4"){
            Console.WriteLine("---MODIFICAR UN ARCHIVO---");

            show_files();

            Console.WriteLine("¿Qué archivo deseas modificar?");
            string opfile = Console.ReadLine()!;

            Console.WriteLine("Ingresa los nuevos datos:");
            string newData = Console.ReadLine()!;
            
            if (Console.ReadKey().Key == ConsoleKey.Escape){
                Console.WriteLine("¿Estas seguro de realizar la modificación? (s/n)");
                string op2 = Console.ReadLine()!;

                if (op2 == "s"){
                    Console.WriteLine("Modificando...");
                    modificate_file(Convert.ToInt32(opfile), newData);
                }
                else{
                    Console.WriteLine("Vaya, vuelve a intentarlo más tarde.");
                }

            }

        }

        if (op == "5"){
            Console.WriteLine("---ELIMINAR UN ARCHIVO---");
            show_files();

            Console.WriteLine("¿Qué archivo deseas eliminar?");
            string opfile = Console.ReadLine()!;
            
            show_file(Convert.ToInt32(opfile));

            Console.WriteLine("¿Estas seguro de realizar la eliminación? (s/n)");
            
            string op2 = Console.ReadLine()!;


            if (op2 == "s"){
                Console.WriteLine("Eliminando...");
                delete_(Convert.ToInt32(opfile));
            }
            else{
                Console.WriteLine("Eliminación Cancelada.");
            }

        }
            

        if (op == "6"){
            Console.WriteLine("---RECUPERAR UN ARCHIVO---");

            show_deleted_files();

            Console.WriteLine("¿Qué archivo deseas recuperar?");
            string opfile = Console.ReadLine()!;

            show_deleted_file(Convert.ToInt32(opfile));

            Console.WriteLine("¿Estas seguro de realizar la recuperación? (s/n)");
            
            string op2 = Console.ReadLine()!;


            if (op2 == "s"){
                Console.WriteLine("Recuperando...");
                backup_(Convert.ToInt32(opfile));
            }
            else{
                Console.WriteLine("Recuperación Cancelada.");
            }

        }
        

        if (op == "7"){
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