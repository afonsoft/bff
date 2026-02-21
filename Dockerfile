ARG VERSION=10.0

# Use Alpine Linux for smaller image size
FROM mcr.microsoft.com/dotnet/sdk:$VERSION-alpine AS build-env

# Set the working directory
WORKDIR /app

# Copy source code
COPY . .

# Build and publish the application and dependencies, self contained with linux runtime
RUN dotnet restore ./src/Eaf.Template.Bff.Host/Eaf.Template.Bff.Host.csproj --ignore-failed-sources --verbosity minimal
RUN dotnet publish /p:PublishTrimmed=false -c Release -o ./output ./src/Eaf.Template.Bff.Host/Eaf.Template.Bff.Host.csproj

# Use Alpine as base image for runtime
FROM mcr.microsoft.com/dotnet/aspnet:$VERSION-alpine AS runtime-env

# Create a directory for the application
RUN mkdir /app

# Copy the published output from build environment to runtime environment
COPY --from=build-env /app/output /app

# Declare the environment variables required to the runtime and application
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
ENV ASPNETCORE_URLS=http://+:4000
ENV DOTNET_URLS=http://+:4000
ENV DOTNET_PROCESSOR_COUNT=2

# Set the working directory
WORKDIR /app

# Install required packages for Alpine (much smaller than Debian)
RUN apk add --no-cache \
    tzdata \
    icu-libs \
    libssl3 \
    libstdc++ \
    libgcc \
    ca-certificates \
    zlib \
    krb5-libs \
    && \
    ln -fs /usr/share/zoneinfo/America/Sao_Paulo /etc/localtime && \
    apk del tzdata && \
    chown -R app /app && \
    rm -rf /var/cache/apk/*

USER app

ENV TZ=America/Sao_Paulo

# Define the required exposed ports
EXPOSE 5000
EXPOSE 443

# Start process when the container start running.
ENTRYPOINT ["dotnet","./Eaf.Template.Bff.Host.dll"]