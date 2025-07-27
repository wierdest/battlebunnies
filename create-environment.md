# 📜 Guia do Script `create-environment.sh`

Este script em Bash automatiza a configuração do ambiente de desenvolvimento do **BattleBunnies** usando o `podman`. Ele garante que todos os serviços (PostgreSQL, RabbitMQ, Redis, API .NET e EmailConfirmationMS) funcionem dentro de um mesmo Pod, compartilhando rede via `localhost`. As migrações do banco de dados também são executadas automaticamente.

---

## 📄 Novidade: uso de `env.yaml`

Agora, todas as variáveis sensíveis e de configuração foram movidas para um arquivo externo chamado `env.yaml`, facilitando a publicação do script no GitHub sem expor credenciais. O script parseia esse YAML diretamente em Bash.

Exemplo de `env.yaml`:

```yaml
postgres_user: seu_usuario_db
postgres_password: seu_secret_db
postgres_db: seu_db
smtp_host: algum.host.smtp
smtp_port: 587
smtp_username: algum_username@host
smtp_password: sua senha aqui
smtp_from: algum_username@host
redis_connection_string: localhost:6379
confirmation_base_url: http://localhost:5276/api/users/confirm
confirmation_code_secret_key: sua frase secreta aqui
```

---

## 🛠 O que o Script Faz

1. **🚪 Libera a Porta 5432**
   Finaliza qualquer processo ocupando a porta padrão do PostgreSQL.

2. **🗑️ Remove Pod Existente**
   Se já existir um pod chamado `battlebunnies-pod`, ele é removido.

3. **🚀 Cria Novo Pod**
   Mapeia as portas:

   * 80 para a API
   * 5432 para PostgreSQL
   * 15672 para a UI do RabbitMQ

4. **🐘 Sobe o PostgreSQL**
   Utiliza as variáveis do YAML para configurar usuário, senha e banco.

5. **📫 Sobe o RabbitMQ**
   Utiliza volume persistente para manter configurações.

6. **🔁 Sobe o Redis**
   Usado pelo serviço de confirmação de e-mail.

7. **⏱️ Espera 10 segundos**
   Tempo para os serviços estarem prontos.

8. **📂 Cria Container de Migração**
   Instala o `dotnet-ef` e executa as migrações.

9. **📡 Aplica Migrações do EF Core**
   Utiliza `BattleBunnies.Infrastructure` e `BattleBunnies.Api`.

10. **🧨 Recompila a API**
    Cria nova imagem e executa container da API com configurações do YAML.

11. **📬 Recompila EmailConfirmationMS**
    Lê todas as variáveis de SMTP do `env.yaml`.

12. **🤝 Garante dependências**
    API e EmailMS se asseguram que o outro container esteja rodando.

13. **✅ Mensagem Final**
    Informa que o ambiente está pronto.

---

## 🧭 Como usar

```bash
./create-environment.sh --all     # Sobe tudo
./create-environment.sh --api     # Sobe apenas a API e garante que EmailMS esteja rodando
./create-environment.sh --email   # Sobe apenas o EmailConfirmationMS e garante que a API esteja rodando
```

---
