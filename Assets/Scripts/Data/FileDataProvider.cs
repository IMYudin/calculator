using System;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public class FileDataProvider<T> : IDataProvider<T> where T : class
{
    private const string FileName = "savedata.json";
    
    private readonly string _saveFilePath;
    private readonly JsonSerializer _serializer;
    
    public T Data { get; private set; }

    public FileDataProvider()
    {
        _serializer = JsonSerializer.CreateDefault(new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto
        });
        _saveFilePath = Application.persistentDataPath + "/" + FileName;
    }

    private void Create()
    {
        Data = Activator.CreateInstance<T>();
    }
    
    public void Load()
    {
        bool loaded = false;

        if (File.Exists(_saveFilePath))
        {
            using var stream = File.OpenRead(_saveFilePath);
            using var textReader = new StreamReader(stream);
            using var jsonReader = new JsonTextReader(textReader);
            
            Data = _serializer.Deserialize<T>(jsonReader);
            loaded = true;
        }

        if (!loaded) Create();
    }

    public void Save()
    {
        using var stream = File.Create(_saveFilePath);
        using var textWriter = new StreamWriter(stream);
        using var jsonWriter = new JsonTextWriter(textWriter);
        
        _serializer.Serialize(jsonWriter, Data);
    }
}