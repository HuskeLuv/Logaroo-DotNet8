# Desafio Logaroo Backend

### Passo a passo

Clone o Repositório

```sh
git clone https://github.com/HuskeLuv/Logaroo-DotNet8.git
```

Suba os containers do projeto

```sh
docker compose up -d
```

Para executar os testes é necessário acessar o bash do container, e rodar o comando de testes no diretório específico

```sh
$ docker compose exec dotnet bash
$ dotnet test src/test
```

Acessar Documentação
[http://localhost:5000/swagger/index.html](http://localhost:5000/swagger/index.html)
