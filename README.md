# 📈 Stock Quote Alert

## Objetivo

Desenvolver uma aplicação de console em **C#/.NET 10** capaz de monitorar a cotação de um ativo da bolsa de valores utilizando a API BRAPI e enviar alertas por e-mail quando o preço atingir os limites de compra ou venda definidos pelo usuário.

---

## Recursos

- Monitoramento periódico da cotação de um ativo.
- Consulta de preços por meio da API BRAPI.
- Envio de alertas por e-mail quando os limites configurados são atingidos.
- Configuração da aplicação por meio do arquivo `appsettings.json`.
- Validação dos argumentos informados pelo usuário.
- Tratamento de erros durante a comunicação com a API e com o servidor SMTP.

---

## Tecnologias utilizadas

- C# / .NET 10
- API REST (BRAPI)
- SMTP
- Injeção de Dependência

---

## Estrutura do projeto

```text
StockQuoteAlert
│
├── Email
│   ├── IEmailSender.cs
│   └── EmailSender.cs
│
├── Integration
│   ├── IStockPriceService.cs
│   └── StockPriceService.cs
│
├── Models
│   ├── ApiResponse.cs
│   └── StockResult.cs
│
├── Settings
│   ├── AppSettings.cs
│   └── EmailSettings.cs
│
├── Validation
│   ├── IValidator.cs
│   └── Validator.cs
│
├── Program.cs
├── appsettings.example.json
├── .gitignore
└── README.md
```

---

## Como executar

Clone o repositório:

```bash
git clone https://github.com/eamonaco/stock-quote-alert
```

Acesse a pasta do projeto:

```bash
cd stock-quote-alert
```

Crie um arquivo `appsettings.json` a partir do arquivo `appsettings.example.json` e informe suas credenciais de e-mail.

## Exemplo de execução
Execute a aplicação:

```bash
dotnet run PETR4 40.00 35.00
```

---

## Argumentos

| Argumento | Descrição |
|-----------|-----------|
| PETR4 | Código do ativo a ser monitorado. |
| 40.00 | Preço de referência para venda. |
| 35.00 | Preço de referência para compra. |

---

## Configuração

Exemplo de `appsettings.json`:

```json
{
  "EmailSettings": {
    "EmailTo": "destinatario@email.com",
    "SmtpHost": "smtp.gmail.com",
    "SmtpPort": 587,
    "EnableSsl": true,
    "Username": "usuario@gmail.com",
    "Password": "senha_de_aplicativo",
    "From": "usuario@gmail.com"
  }
}
```

> **Importante:** para contas Gmail é necessário utilizar uma **Senha de Aplicativo**, e não a senha da conta.

---

## Funcionamento

A aplicação consulta a API da BRAPI a cada 10 segundos.

Quando:

- o preço atual é superior ao preço de venda informado, um e-mail é enviado recomendando a venda do ativo;
- o preço atual é inferior ao preço de compra informado, um e-mail é enviado recomendando a compra do ativo.

Para evitar notificações repetidas para uma mesma cotação, a aplicação não envia múltiplos e-mails consecutivos.

---

## Tratamento de erros

A aplicação realiza validações para:

- quantidade de argumentos informados;
- formato dos preços de compra e venda;
- falhas de comunicação com a API;
- falhas durante o envio de e-mails.

Quando uma dessas situações é identificada, uma mensagem descritiva é exibida ao usuário e a execução da aplicação é encerrada de forma controlada.

---

## Uso de Inteligência Artificial

Durante o desenvolvimento deste projeto, ferramentas de Inteligência Artificial foram utilizadas como apoio para pesquisa e aprendizado de tecnologias específicas da plataforma .NET.

A IA foi utilizada principalmente para:

- esclarecer conceitos da linguagem e do ecossistema .NET;
- consultar exemplos de utilização de bibliotecas, como SMTP para envio de e-mails;
- compreender a implementação de Injeção de Dependência;
- revisar a documentação do projeto.

As decisões de implementação, integração entre os componentes, adaptações e validação do funcionamento da aplicação foram realizadas pelo autor.