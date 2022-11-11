#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["AdminPanel/AdminPanel.csproj", "AdminPanel/"]
COPY ["TestsDelivery.BL/TestsDelivery.BL.csproj", "TestsDelivery.BL/"]
COPY ["TestsDelivery.BL.Shared/TestsDelivery.BL.Shared.csproj", "TestsDelivery.BL.Shared/"]
COPY ["TestsDelivery.DAL/TestsDelivery.DAL.csproj", "TestsDelivery.DAL/"]
COPY ["TestsDelivery.Domain/TestsDelivery.Domain.csproj", "TestsDelivery.Domain/"]
COPY ["TestsPortal.DAL.Shared/TestsDelivery.DAL.Shared.csproj", "TestsPortal.DAL.Shared/"]
COPY ["TestsDelivery.UserModels/TestsDelivery.UserModels.csproj", "TestsDelivery.UserModels/"]
COPY ["TestsDelivery.Infrastructure/TestsDelivery.Infrastructure.csproj", "TestsDelivery.Infrastructure/"]
RUN dotnet restore "AdminPanel/AdminPanel.csproj"
COPY . .
WORKDIR "/src/AdminPanel"
RUN dotnet build "AdminPanel.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AdminPanel.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AdminPanel.dll"]