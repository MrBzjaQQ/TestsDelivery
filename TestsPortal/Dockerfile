#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["TestsPortal/TestsPortal.csproj", "TestsPortal/"]
COPY ["TestsDelivery.Infrastructure/TestsDelivery.Infrastructure.csproj", "TestsDelivery.Infrastructure/"]
COPY ["TestsPortal.BL/TestsPortal.BL.csproj", "TestsPortal.BL/"]
COPY ["TestsDelivery.BL.Shared/TestsDelivery.BL.Shared.csproj", "TestsDelivery.BL.Shared/"]
COPY ["TestsDelivery.UserModels/TestsDelivery.UserModels.csproj", "TestsDelivery.UserModels/"]
COPY ["TestsPortal.DAL.Shared/TestsDelivery.DAL.Shared.csproj", "TestsPortal.DAL.Shared/"]
COPY ["TestsPortal.DAL/TestsPortal.DAL.csproj", "TestsPortal.DAL/"]
COPY ["TestsPortal.Domain/TestsPortal.Domain.csproj", "TestsPortal.Domain/"]
RUN dotnet restore "TestsPortal/TestsPortal.csproj"
COPY . .
WORKDIR "/src/TestsPortal"
RUN dotnet build "TestsPortal.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TestsPortal.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TestsPortal.dll"]