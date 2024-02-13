# README - API Consumer e Producer

Este é um conjunto de APIs para consumir e produzir dados de clientes. A API Producer envia os dados dos clientes para uma fila do Azure Service Bus e armazena no azure cosmos db,  a API Consumer processa esses dados consumindo a fila do service bus.

## Pré-requisitos

Certifique-se de ter os seguintes pré-requisitos instalados em seu ambiente de desenvolvimento:

- .NET SDK (versão 8)
- Azure CLI (opcional, se você estiver usando o Azure)

## Rodando a API Producer

Para rodar a API Producer, siga estas etapas:

1. Navegue até o diretório `APIProducer` em seu terminal.
2. Execute o seguinte comando para iniciar a aplicação:

dotnet watch

3. Certifique-se de que a API está ouvindo na porta especificada (por padrão, `localhost:5199`).

4. Para registrar um dado no banco e enviar para a fila, envie uma requisicao post para : /api/customers.

5. Utilize um corpo JSON com o formato DTO (customer)

{
name,
email,
phoneNumber
}

## Rodando a API Consumer

Para rodar a API Consumer, siga estas etapas:

1. Navegue até o diretório `APIConsumer` em seu terminal.
2. Execute o seguinte comando para iniciar a aplicação:
   
dotnet watch

4. Certifique-se de que a API está ouvindo na porta especificada (por padrão, `localhost:5190`).

