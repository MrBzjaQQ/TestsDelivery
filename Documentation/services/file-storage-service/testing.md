# File Storage Service - Testing

## Test Structure

```
FileStorageService.Tests/
├── Unit/
│   ├── Services/
│   │   ├── FileUploadServiceTests.cs
│   │   └── ImageProcessorTests.cs
│   └── Controllers/
│       └── FilesControllerTests.cs
└── Integration/
    ├── TestInfrastructure/
    │   ├── DatabaseFixture.cs
    │   └── DbTestsBase.cs
    └── Controllers/
        └── FilesControllerTests.cs
```

## Unit Tests

### FileUploadServiceTests

```csharp
public class FileUploadServiceTests
{
    private readonly Mock<IFileRepository> _mockRepository;
    private readonly Mock<IImageProcessor> _mockImageProcessor;
    private readonly FileUploadService _service;

    [Fact]
    public async Task UploadFile_Should_ReturnSuccess_When_Valid()
    {
        // Arrange
        var file = CreateTestFile("test.jpg", 100000, "image/jpeg");
        var service = CreateService();

        // Act
        var result = await service.UploadFileAsync(file, Guid.NewGuid(), CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Data.Id.Should().NotBeEmpty();
    }

    [Fact]
    public async Task UploadFile_Should_ThrowInvalidFileException_When_File_Too_Large()
    {
        // Arrange
        var file = CreateTestFile("large.jpg", 15000000, "image/jpeg");
        var service = CreateService();

        // Act & Assert
        await Assert.ThrowsAsync<InvalidFileException>(() =>
            service.UploadFileAsync(file, Guid.NewGuid(), CancellationToken.None));
    }

    private IFormFile CreateTestFile(string fileName, int size, string contentType)
    {
        var memoryStream = new MemoryStream(new byte[size]);
        return new FormFile(memoryStream, 0, size, "file", fileName)
        {
            Headers = new HeaderDictionary(),
            ContentType = contentType
        };
    }
}
```

### ImageProcessorTests

```csharp
public class ImageProcessorTests
{
    [Fact]
    public async Task ProcessAsync_Should_Resize_Large_Image()
    {
        // Arrange
        var processor = new ImageProcessor();
        var largeImage = GenerateTestImage(2500, 1500); // Larger than max 1920x1080

        // Act
        var result = await processor.ProcessAsync(largeImage);

        // Assert
        using var image = Image.Load(result);
        image.Width.Should().Be(1920);
        image.Height.Should().Be(1080);
    }

    [Fact]
    public async Task GenerateThumbnail_Should_Create_200x200_Thumbnail()
    {
        // Arrange
        var processor = new ImageProcessor();
        var imageBytes = GenerateTestImage(1920, 1080);

        // Act
        var thumbnail = await processor.GenerateThumbnailAsync(imageBytes);

        // Assert
        using var thumb = Image.Load(thumbnail);
        thumb.Width.Should().Be(200);
        thumb.Height.Should().Be(200);
    }
}
```

## Integration Tests

```csharp
[Collection("Database")]
public class FilesControllerTests : DbTestsBase
{
    private readonly FilesController _controller;

    [Fact]
    public async Task UploadFile_Should_Upload_To_Database()
    {
        // Arrange
        var file = CreateTestFile("test.jpg", 100000, "image/jpeg");
        _controller = new FilesController(new FileUploadService(new FileRepository(DbContext)));

        // Act
        var result = await _controller.Upload(file, new UploadRequest { OwnerId = Guid.NewGuid() }, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Data.Id.Should().NotBeEmpty();
    }
}
```

## Running Tests

```bash
dotnet test
dotnet test --filter "FullyQualifiedName~Unit"
dotnet test --filter "FullyQualifiedName~Integration"
```