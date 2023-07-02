using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SaveShop(ShopData shopData)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/shop.sf";
        //Debug.Log(path);
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, shopData);
        stream.Close();
    }

    public static ShopData LoadShop()
    {
        string path = Application.persistentDataPath + "/shop.sf";
        //Debug.Log(path);
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            ShopData shopData =  formatter.Deserialize(stream) as ShopData;
            stream.Close();

            return shopData;
        }
        else
        {
            Debug.Log("No save file at path: " + path);
            return new ShopData();
        }
    }

    public static void SaveGems(GemsSaveData gemsSaveData)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/gems.sf";
        //Debug.Log(path);
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, gemsSaveData);
        stream.Close();
    }

    public static GemsSaveData LoadGems()
    {
        string path = Application.persistentDataPath + "/gems.sf";
        //Debug.Log(path);
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            GemsSaveData gemsSaveData = formatter.Deserialize(stream) as GemsSaveData;
            stream.Close();

            return gemsSaveData;
        }
        else
        {
            Debug.Log("No save file at path: " + path);
            return new GemsSaveData(0);
        }
    }

    public static void SaveHeadPosition(HeadPositionData headPositionData)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/head.sf";
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, headPositionData);
        stream.Close();
    }

    public static HeadPositionData LoadHeadPosition()
    {
        string path = Application.persistentDataPath + "/head.sf";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            HeadPositionData mapData = formatter.Deserialize(stream) as HeadPositionData;
            stream.Close();

            return mapData;
        }
        else
        {
            Debug.LogError("No save file at path: " + path);
            return null;
        }
    }

    public static void SaveVolume(VolumeData volumeData)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/volume.sf";
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, volumeData);
        stream.Close();
    }

    public static VolumeData LoadVolume()
    {
        string path = Application.persistentDataPath + "/volume.sf";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            VolumeData volumeData = formatter.Deserialize(stream) as VolumeData;
            stream.Close();

            return volumeData;
        }
        else
        {
            Debug.LogError("No save file at path: " + path);
            return new VolumeData(0.5f, 0.5f, 0.5f);
        }
    }

    public static void SaveLevels(LevelData levelData)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/levels.sf";
        //Debug.Log(path);
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, levelData);
        stream.Close();
    }

    public static LevelData LoadLevels()
    {
        string path = Application.persistentDataPath + "/levels.sf";
        //Debug.Log(path);
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            LevelData levelData = formatter.Deserialize(stream) as LevelData;
            stream.Close();

            return levelData;
        }
        else
        {
            Debug.Log("No save file at path: " + path);
            return new LevelData();
        }
    }

}
