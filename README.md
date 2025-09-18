# MotorCycle Rent
## Sistema de CRUD para Moto, Entregador e Aluguel

# Como rodar
## Manualmente
### Database
- Necessário ajustar ConnectionString em AppSettings.json.
- Executar update da base através de Migrations.

### RabbitMq
- Ajustar fila (url, user e pass) no appsettings da api e do consumer.
- (Caso a aplicação não consiga se conectar à fila, o fluxo nao sera comprometido, apenas nao teremos notificação no projeto worker)
- Deixei o docker-compose para ser executado e rodar apenas o rabbitmq: executar `docker-compose up rabbitmq` no diretório raiz.

### Start
- Ajustar inicialização do projeto para rodar Api + Consumer.
- Swagger deverá exposto em: https://localhost:5001/swagger/index.html

## Via Docker
- Acessar diretório raiz do projeto e executar: `docker-compose build` > `docker-compose up`
- Validar se todas as imagens subiram com sucesso (database, api, consumer e rabbitmq)
- Swagger deverá exposto em: http://localhost:5000/swagger/index.html

# Esse projeto contem:
- SwaggerUI
- EntityFramework
- Postgres
- UnitTest
- MediatR
- .Net Dependency Injection

# Detalhes
- Fiquei confuso sobre o swagger de exemplo receber Id (string) nas requisicoes, daí adotei uma abordagem com esse identificador + um IdInterno incremental na base;
- O Consumer apenas notifica no próprio projeto e insere numa tabela a parte as notificações para as motos na regra definida.