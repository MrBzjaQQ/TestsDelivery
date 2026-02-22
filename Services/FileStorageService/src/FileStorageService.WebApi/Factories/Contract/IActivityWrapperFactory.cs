using FileStorageService.WebApi.Wrappers.Contract;
using Microsoft.AspNetCore.Mvc;

namespace FileStorageService.WebApi.Factories.Contract;

public interface IActivityWrapperFactory
{
    IActivityWrapper GetActivity();
}
