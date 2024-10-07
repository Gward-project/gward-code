using System.Text;
using Gwards.Api.Models;
using Gwards.Api.Models.Dto.Nft;
using Gwards.Api.Services.Common;
using Gwards.DAL.Entities;
using Gwards.DAL.Entities.Quests;
using Newtonsoft.Json;
using SkiaSharp;

namespace Gwards.Api.Services.Ton;

public class NftMetadataGenerator
{
    private readonly TelegramBotConfig _config;
    private readonly string _assetsPath;
    private readonly string _fontsPath;
    
    private readonly FileStorageService _fileStorage;

    public NftMetadataGenerator(
        TelegramBotConfig config,
        IWebHostEnvironment hostEnvironment,
        FileStorageService fileStorage
    )
    {
        _config = config;
        _assetsPath = hostEnvironment.WebRootPath;
        _fontsPath = Path.Join(_assetsPath + "/Fonts");
        _fileStorage = fileStorage;
    }

    public async Task GenerateQuestMetadata(uint nftIndex, QuestBaseEntity quest)
    {
        var metadataPath = Path.Join(quest.NftMetadataBasePath, "metadata.json");
        await using var metadataStream = _fileStorage.Open(metadataPath);
        
        var serializer = new JsonSerializer { NullValueHandling = NullValueHandling.Ignore };
        using var streamReader = new StreamReader(metadataStream);
        await using var jsonTextReader = new JsonTextReader(streamReader);
        
        var collectionMetadata = serializer.Deserialize<NftMetadataDto>(jsonTextReader);
        var nftItemMetadata = new NftMetadataDto
        {
            Name = collectionMetadata.Name,
            Description = collectionMetadata.Description,
            Image = collectionMetadata.Image
        };
        
        var serializedNftItemMetadata = JsonConvert.SerializeObject(nftItemMetadata);
        var nftMetadataBytes = Encoding.UTF8.GetBytes(serializedNftItemMetadata);
        using var nftMetadataStream = new MemoryStream(nftMetadataBytes);
        var nftMetadataPath = Path.Join(quest.NftMetadataBasePath, $"{nftIndex}.json");
        
        _fileStorage.Save(nftMetadataStream, nftMetadataPath);
    }

    public async Task GeneratePassportMetadata(uint nftIndex, string baseMetadataPath, UserEntity user)
    {
        var metadataPath = Path.Join(baseMetadataPath, $"{nftIndex}.json");
        var imagePath = Path.Join(baseMetadataPath, $"{nftIndex}.png");

        if (_fileStorage.IsExist(metadataPath))
        {
            _fileStorage.Delete(metadataPath);
        }
        
        if (_fileStorage.IsExist(imagePath))
        {
            _fileStorage.Delete(imagePath);
        }
        
        var nftItemMetadata = new NftMetadataDto
        {
            Name = $"{user.Nickname}'s Gward Passport",
            Description = $"{user.Nickname}'s gward passport with user's score",
            Image = $"{_config.WebApp}/api{imagePath}",
            Attributes = [
                new NftMetadataAttribute { TraitType = "score", Value = user.Score.ToString("D") }
            ]
        };
        var serializedNftItemMetadata = JsonConvert.SerializeObject(nftItemMetadata);
        var nftMetadataBytes = Encoding.UTF8.GetBytes(serializedNftItemMetadata);
        var nftMetadataStream = new MemoryStream(nftMetadataBytes);
        await using var imageStream = GeneratePassportImage(user);
        
        _fileStorage.Save(imageStream, imagePath);
        _fileStorage.Save(nftMetadataStream, metadataPath);
    }

    private Stream GeneratePassportImage(UserEntity user)
    {
        const int width = 1370;
        const int height = 1000;
        
        var info = new SKImageInfo(width, height);
        using var surface = SKSurface.Create(info);
        
        DrawPassportBackgroundImage(surface);
        DrawPassportNickname(user.Nickname, surface);
        DrawPassportScore(user.Score, surface);
        
        using var image = surface.Snapshot();
        using var data = image.Encode(SKEncodedImageFormat.Png, 100);
        
        return data.AsStream();
    }

    private void DrawPassportBackgroundImage(SKSurface surface)
    {
        var width = surface.Snapshot().Width;
        var height = surface.Snapshot().Height;
        
        var passportBackgroundImagePath = Path.Combine(_assetsPath, "NftImages", "PassportBackground.png");
        var passportBackgroundImageSize = new SKImageInfo(width, height);
        
        using var passportBackgroundImage = SKImage.FromEncodedData(passportBackgroundImagePath);
        using var tempPassportBitmap = SKBitmap.FromImage(passportBackgroundImage);
        using var passportBitmap = tempPassportBitmap.Resize(passportBackgroundImageSize, SKFilterQuality.None);
        
        surface.Canvas.DrawBitmap(passportBitmap, 0, 0);
        surface.Canvas.Save();
    }

    private void DrawPassportNickname(string nickName, SKSurface surface)
    {
        const int x = 935;
        const int y = 235;
        
        using var textPaint = new SKPaint();
        
        textPaint.TextSize = 70;
        textPaint.IsAntialias = true;
        textPaint.Color = new SKColor(255, 255, 255);
        textPaint.TextAlign = SKTextAlign.Center;

        var interFontPath = Path.Join(_fontsPath, "InterExtraLight.ttf");
        if (_fileStorage.IsExist(interFontPath))
        {
            textPaint.Typeface = SKTypeface.FromFile(interFontPath);
        }
        
        surface.Canvas.DrawText($"@{nickName}", x, y, textPaint);
        surface.Canvas.Save();
    }
    
    private void DrawPassportScore(int score, SKSurface surface)
    {
        const int x = 935;
        const int y = 520;
        
        using var textPaint = new SKPaint();
        
        textPaint.TextSize = 130;
        textPaint.IsAntialias = true;
        textPaint.Color = new SKColor(255, 255, 255);
        textPaint.TextAlign = SKTextAlign.Center;
        
        var interFontPath = Path.Join(_fontsPath, "InterBold.ttf");
        if (_fileStorage.IsExist(interFontPath))
        {
            textPaint.Typeface = SKTypeface.FromFile(interFontPath);
        }
        
        surface.Canvas.DrawText($"{score}", x - 120 - score.ToString().Length * 15, y, textPaint);

        textPaint.Color = new SKColor(144, 163, 197);
        
        surface.Canvas.DrawText(" / 100", x + score.ToString().Length * 40, y, textPaint);
        surface.Canvas.Save();
    }
}