# 🧪 BattleBunnies — Projeto de Aprendizado

**BattleBunnies** é um projeto de aprendizado criado para explorar os seguintes conceitos e tecnologias:

- 🧱 **Clean Architecture (Arquitetura Limpa)** – Separação de responsabilidades em camadas: Domínio, Aplicação e Infraestrutura.
- 🧩 **Vertical Slice Architecture (VSA)** – Organização dos casos de uso como fatias independentes com seus próprios comandos, handlers e endpoints.
- 🔁 **CQRS (Segregação de Comando e Consulta)** – Separação entre leitura e escrita para maior escalabilidade e clareza.
- 🧭 **Padrão Mediator** – Uso do MediatR para desacoplar a lógica de manipulação de requisições do fluxo da aplicação.
- 🐘 **PostgreSQL** – Banco de dados relacional usado para persistência com EF Core.
- 🐰 **RabbitMQ** – Integração de um message broker para processamentos em segundo plano (como confirmação de e-mail).
- 🐳 **Containers com Podman** – Utilização de `podman` e containers Linux no lugar do Docker.
- 🧵 **.NET 9** – Prática de desenvolvimento moderno com C# usando ASP.NET Core e EF Core.
- 🧰 **Scripting e Automação** – Automatização do setup do ambiente de desenvolvimento com Bash e ferramentas de container.

---

## 🐰 Resumo do Projeto BattleBunnies

### 🧱 Arquitetura

Clean Architecture com os seguintes projetos:

- **BattleBunnies.Api** – API Web, CQRS com MediatR
- **BattleBunnies.Application** – Casos de uso, interfaces e handlers do MediatR
- **BattleBunnies.Domain** – Entidades e interfaces como `IUserRepository`
- **BattleBunnies.Infrastructure** – EF Core, RabbitMQ, Redis e SMTP
- **BattleBunnies.Contracts** – Definições de mensagens entre serviços
- **BattleBunnies.EmailConfirmationMS** – Serviço em segundo plano com SMTP e Redis

---

### 🧩 Funcionalidades

**Registro de Usuário:**

- Envia `UserRegisteredMessage` para o RabbitMQ após o registro

**Serviço de Confirmação de E-mail (EmailConfirmationMS):**

- Consome `UserRegisteredMessage`
- Envia e-mail via SMTP do Gmail
- Armazena código de confirmação no Redis
- [Fazendo] Validação de código de confirmação via endpoint na API

---

### 📦 Stack Tecnológico

- .NET 9 (SDK e ASP.NET)
- PostgreSQL (migrações EF Core executadas em container)
- RabbitMQ (comunicação desacoplada)
- Redis (armazenamento dos códigos de confirmação)
- SMTP via Gmail (usado pelo `SMTPEmailSender`)

---

### 🐳 Containerização

- Uso do Podman para todos os serviços
- Containers executados em um pod compartilhado chamado `battlebunnies-pod`
- Script customizado `create-environment.sh` que constrói e executa:
  - PostgreSQL
  - RabbitMQ
  - API
  - EmailConfirmationMS
  - Redis
  - Container one-off para `dotnet ef database update`
- Configurações passadas por variáveis de ambiente (sem uso de `appsettings.json`)

---

### ✅ Conquistas

- Fluxo completo de mensagens via RabbitMQ funcionando
- E-mail de confirmação enviado com sucesso via SMTP do Gmail
- Redis integrado para armazenamento de códigos
- Script de ambiente containerizado funcionando e idempotente
