FROM mcr.microsoft.com/dotnet/framework/aspnet:4.8-windowsservercore-ltsc2019
ARG source
WORKDIR /inetpub/wwwroot
EXPOSE 44300
COPY ${source:-obj/Docker/publish} .
