FROM mcr.microsoft.com/dotnet/sdk:8.0 AS dev

WORKDIR /app

# Exponha a porta para o servidor de desenvolvimento
EXPOSE 5000
EXPOSE 5001

# Instalando a ferramenta dotnet-ef globalmente
RUN dotnet tool install --global dotnet-ef

# Definindo o PATH para a ferramenta dotnet-ef
ENV PATH="$PATH:/root/.dotnet/tools"

# Comando padrão para iniciar o shell
CMD ["dotnet", "watch", "run", "--urls", "http://0.0.0.0:5000"]