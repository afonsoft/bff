ARG VERSION=8.0

FROM mcr.microsoft.com/dotnet/sdk:$VERSION AS build-env

# Set the working directory
WORKDIR /app

# Copy source code
COPY . .

# Build and publish the application and dependencies, self contained with linux runtime
RUN dotnet restore ./src/Eaf.Template.Bff.Host/Eaf.Template.Bff.Host.csproj --ignore-failed-sources --configfile ./Nuget.config --verbosity minimal
RUN dotnet publish /p:PublishTrimmed=false -c Release -o ./output ./src/Eaf.Template.Bff.Host/Eaf.Template.Bff.Host.csproj

# Use Debian as base image
FROM mcr.microsoft.com/dotnet/aspnet:$VERSION AS runtime-env

# Create a directory for the application
RUN mkdir /app

# Copy the published output from build environment to runtime environment
COPY --from=build-env /app/output /app

# Declare the environment variables required to the runtime and application
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
ENV ASPNETCORE_URLS=http://+:4000
ENV DOTNET_URLS=http://+:4000

# Set the working directory
WORKDIR /app

# Install ICU and set timezone and set the timezone to America/Sao_Paulo
RUN apt update && \
    apt install -yq tzdata libc6-dev libgdiplus zlib1g-dev icu-devtools && \
    ln -fs /usr/share/zoneinfo/America/Sao_Paulo /etc/localtime && \
    dpkg-reconfigure -f noninteractive tzdata && \
	chown -R app /app && \
	apt clean && \
	rm -rf /var/lib/apt/lists/*

USER app

ENV TZ=America/Sao_Paulo

# Define the required exposed ports
EXPOSE 5000
EXPOSE 443

# Start process when the container start running.
ENTRYPOINT ["dotnet","./Eaf.Template.Bff.Host.dll"]