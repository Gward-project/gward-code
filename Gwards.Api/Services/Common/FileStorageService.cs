namespace Gwards.Api.Services.Common;

public class FileStorageService
{
    private readonly string _rootPath;

    public FileStorageService(IWebHostEnvironment env)
    {
        _rootPath = env.WebRootPath;
    }

    public Stream Open(string fileName)
    {
        var path = GetPath(fileName);
        
        return File.OpenRead(path);
    }
    
    public void Save(Stream stream, string fileName)
    {
        var path = GetPath(fileName);
        
        using var fileStream = new FileStream(path, FileMode.Create);
        stream.CopyTo(fileStream);
    }
    
    public void Save(byte[] bytes, string fileName)
    {
        var path = GetPath(fileName);
        
        using var fileStream = new FileStream(path, FileMode.Create);
        fileStream.Write(bytes);
    }

    public bool IsExist(string fileName)
    {
        var path = GetPath(fileName);
        return File.Exists(path);
    }
    
    public void Delete(string fileName)
    {
        var path = GetPath(fileName);
        if (!File.Exists(path))
        {
            return;
        }

        File.Delete(path);
    }

    private string GetPath(string fileName)
    {
        fileName = fileName.Replace("/static/", "");
        return Path.Combine(_rootPath, fileName);
    }
}