
# Agendamento de Churrascos

Este é um exemplo de projeto sobre como implementar uma API para agendamento e gerenciamento de churrascos.

## Tecnologias

- `.NET 6`
- `MS SQL Server`
- `EF core 7`
- `Docker`


## Preparando o ambiente

```bash
# Create certificate  pfx - Necessário para rodar API no protocolo HTTPS no Docker
dotnet dev-certs https --trust -ep $env:USERPROFILE\.aspnet\https\schedulebarbecueapp.pfx -p localhost

# Run database - Download da imagem e criação do container do SQL Server
docker compose up -d mssql

# Run migrations - Criação do banco de dados da API dentro do container do SQL Server
docker compose up migrations

# Run tests - Excução dos testes unitários e integrados
docker compose up run-tests

# Run service - Inicia a API usando o dotnet watch
docker compose up -d api
```
Caso tenha dúvidas sobre a geração do certificado pfx, consulte essa documentação: https://learn.microsoft.com/en-us/aspnet/core/security/docker-https?view=aspnetcore-6.0

### URLs
Após efetuados os passos acima a API estará disponível no seeguinte endereço abaixo
- Api: https://localhost:5000/swagger/index.html


## Visão Geral

Esta solução contém uma API com rotinas para efetuar o CRUD para agendamentos de churrascos, a ideia é que consiga: 
 - Cadastrar um usuário.
 - Se autenticar.
 - Cadastrar um churrasco.
 - Listar os churrascos cadastrados.
 - Cadastrar um participante.
 - Listar os participantes cadastrados.
 
Os passos acima são pertinentes a alimentação básica do sistema para que consiga efetuar as operações que envolvem a contribuição monetária dos participantes nos churrascos cadastrados.

Depois de alimentado com os cadastrados básicos, a ideia é que consiga:
 - Cadastrar uma contribuição.
 - Excluir uma contribuição.
 - Listar todos os participantes de acordo com o id do churrasco.
 - Listar o resumo de um determinado churrasco de acordo com o id do churrasco contendo as informações (data do churrasco, nome, quantidade de participantes e valor total de contribuição).
 - Listar o resumo de todos os churrascos contendo as informações (data do churrasco, nome, quantidade de participantes e valor total de contribuição).

> **Note:** Por questão de não prolongar o tempo de entrega do teste, foram implementados apenas testes unitários e integrados pertinentes no que tange a churrasco, participante e contribuição. 