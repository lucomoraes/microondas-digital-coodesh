# 🔥 Micro-ondas Digital Web

Aplicação web completa de um micro-ondas digital com interface JavaScript e backend .NET, desenvolvida seguindo os princípios de Clean Architecture e SOLID.

## 📋 Sobre o Projeto

Sistema web que simula um micro-ondas digital completo com funcionalidades de aquecimento manual, programas pré-definidos, programas customizados e gerenciamento de usuários. O projeto implementa autenticação JWT, criptografia de senhas com SHA-256, tratamento global de exceções e persistência em SQL Server.

## 🚀 Tecnologias Utilizadas

### Backend
- **.NET 10** - Framework principal
- **C# 14.0** - Linguagem de programação
- **ASP.NET Core Web API** - API RESTful
- **Entity Framework Core** - ORM para acesso a dados
- **SQL Server** - Banco de dados
- **JWT Bearer Authentication** - Autenticação e autorização
- **FluentValidation** - Validação de dados
- **SHA-256** - Criptografia de senhas

### Testes
- **xUnit** - Framework de testes unitários
- **Moq** - Biblioteca de mocking para isolamento de dependências
- **FluentAssertions** - Asserções expressivas e legíveis
- **Coverlet** - Cobertura de código

### Frontend
- **HTML5/CSS3** - Estrutura e estilização
- **JavaScript (Vanilla)** - Lógica da interface
- **Fetch API** - Comunicação com backend

### Arquitetura e Padrões
- **Clean Architecture** - Separação em camadas (Domain, Application, Infrastructure, API)
- **CQRS Pattern** - Separação de Commands e Queries
- **Repository Pattern** - Abstração de acesso a dados
- **Unit of Work Pattern** - Gerenciamento de transações
- **Dependency Injection** - Inversão de controle
- **Middleware Pattern** - Tratamento global de exceções

## 📁 Estrutura do Projeto

```
Web.Microondas/
├── Web.Microondas.Domain/          # Entidades e regras de negócio
│   ├── Entities/
│   │   ├── Microwave.cs
│   │   ├── HeatingProgram.cs
│   │   └── Users.cs
│   ├── Interfaces/
│   │   ├── Repository/
│   │   └── UnitOfWork/
│   ├── Enums/
│   └── DomainException/
├── Web.Microondas.Application/     # Casos de uso e lógica de aplicação
│   ├── Services/
│   │   ├── Implementations/
│   │   └── Interfaces/
│   ├── UseCases/
│   │   ├── Auth/
│   │   ├── User/
│   │   ├── HeatingProgram/
│   │   └── Microwave/
│   ├── DTOs/
│   ├── Validators/
│   ├── Helpers/
│   └── Exceptions/
├── Web.Microondas.Infrastructure/  # Acesso a dados e persistência
│   ├── DatabaseContext/
│   ├── Repositories/
│   ├── Migrations/
│   ├── Mappings/
│   └── UnitOfWork/
├── Web.Microondas.API/             # Camada de apresentação
│   ├── Controllers/
│   ├── Middleware/
│   └── wwwroot/                    # Frontend
│       ├── js/
│       ├── css/
│       └── index.html
└── Web.Microondas.Test/            # Testes unitários
    ├── Domain/                     # Testes de entidades
    ├── Application/
    │   ├── Services/               # Testes de serviços
    │   ├── Handlers/               # Testes de handlers
    │   ├── Validators/             # Testes de validadores
    │   ├── Helpers/                # Testes de helpers
    │   └── Exceptions/             # Testes de exceções
    └── Infrastructure/
        └── Repositories/           # Testes de repositórios
```

## ⚙️ Pré-requisitos

Antes de começar, você precisa ter instalado em sua máquina:

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0) ou superior
- [SQL Server](https://www.microsoft.com/sql-server/sql-server-downloads) (Express ou superior)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) ou [VS Code](https://code.visualstudio.com/)
- [Git](https://git-scm.com/)

### 📦 Pacotes de Teste (Incluídos no Projeto)
- **xUnit** - Framework de testes
- **Moq** - Biblioteca de mocking
- **FluentAssertions** - Asserções fluentes para testes mais legíveis

## 🔧 Instalação e Configuração

### 1️⃣ Clone o Repositório

```bash
git clone <url-do-repositorio>
cd Web.Microondas
```

### 2️⃣ Configuração do Banco de Dados

#### 2.1 Connection String (Descriptografada)

A connection string está criptografada em Base64 no `appsettings.json`. A **connection string descriptografada** é:

```
Server=localhost\SQLEXPRESS01;Database=MicroondasDb;Trusted_Connection=True;TrustServerCertificate=True;
```

> ⚠️ **IMPORTANTE**: Ajuste o nome da instância do SQL Server (`localhost\SQLEXPRESS01`) conforme sua configuração local.

#### 2.2 Se precisar alterar a Connection String:

1. Abra o arquivo `Web.Microondas.API/appsettings.json`
2. Modifique a connection string descriptografada conforme seu ambiente
3. Converta para Base64 usando PowerShell:

```powershell
$connString = "Server=SEU_SERVIDOR;Database=MicroondasDb;Trusted_Connection=True;TrustServerCertificate=True;"
$bytes = [System.Text.Encoding]::UTF8.GetBytes($connString)
[Convert]::ToBase64String($bytes)
```

4. Substitua o valor em `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "VALOR_BASE64_AQUI"
  }
}
```

### 3️⃣ Aplicar Migrations e Criar o Banco de Dados

Abra o terminal na pasta raiz do projeto e execute:

```bash
# Navegar até a pasta da API
cd Web.Microondas.API

# Restaurar pacotes NuGet
dotnet restore

# Criar e aplicar migrations
dotnet ef database update --project ../Web.Microondas.Infrastructure --startup-project .
```

> ✅ Isso criará automaticamente o banco de dados `MicroondasDb` com todas as tabelas necessárias.

### 4️⃣ Verificar se o Banco foi Criado

Conecte-se ao SQL Server e verifique:

```sql
USE MicroondasDb;
GO

-- Verificar tabelas criadas
SELECT TABLE_NAME 
FROM INFORMATION_SCHEMA.TABLES 
WHERE TABLE_TYPE = 'BASE TABLE';

-- Deve exibir: Users, HeatingPrograms, __EFMigrationsHistory
```

## 🚀 Como Executar o Projeto

### Executar via Visual Studio

1. Abra a solution `Web.Microondas.sln`
2. Defina `Web.Microondas.API` como projeto de inicialização
3. Pressione `F5` ou clique em "▶ Run"
4. O navegador abrirá automaticamente em `https://localhost:7219`

### Executar via Command Line

```bash
# Na pasta Web.Microondas.API
dotnet run --launch-profile https

# Ou para ambiente de desenvolvimento com hot reload
dotnet watch run
```

A aplicação estará disponível em:
- **HTTPS**: `https://localhost:7219`
- **Swagger**: `https://localhost:7219/swagger`

## 🧪 Testes Unitários

O projeto inclui uma suite completa de testes unitários para a camada de negócio, implementada com **xUnit**, **Moq** e **FluentAssertions**.

### 📊 Abrangência dos Testes

#### ✅ **Domain Layer (Entidades e Lógica de Negócio)**
- **MicrowaveTests** (18 testes)
  - Testes de Quick Start e início manual
  - Validação de tempo e potência
  - Comportamento de pausa/cancelamento/resume
  - Adição de tempo durante aquecimento
  - Formatação de display e tempo
  - Validação de programas pré-definidos
  - Limite de tempo máximo (120s)

- **HeatingProgramTests** (5 testes)
  - Criação de programas preset e custom
  - Validação de propriedades
  - Diferenciação entre tipos de programas

#### ✅ **Application Layer (Serviços e Lógica de Aplicação)**

**Validators (16 testes)**
- `MicrowaveManualStartValidatorTests` - Validação de tempo e potência
- `CreateHeatingProgramRequestValidatorTests` - Validação de criação de programas
  - Campos obrigatórios
  - Validação de caractere reservado ('.')
  - Validação de caractere duplicado

**Services (10 testes)**
- `MicrowaveServiceTests` - Operações do micro-ondas
  - Estados do micro-ondas (Idle, Running, Paused, Completed)
  - Integração com programas de aquecimento
  - Tratamento de exceções de negócio

**Handlers (5 testes)**
- `CreateHeatingProgramHandlerTests` - Criação de programas com transações
- `GetAllHeatingProgramsHandlerTests` - Listagem de programas
- `DeleteHeatingProgramHandlerTests` - Exclusão com rollback em caso de erro

**Helpers (5 testes)**
- `Sha256HelperTests` - Criptografia de senhas
  - Consistência de hash
  - Validação do hash esperado (SHA-256)
  - Comprimento do hash (64 caracteres)

**Exceptions (3 testes)**
- `BusinessRuleExceptionTests` - Exception customizada de negócio

#### ✅ **Infrastructure Layer (Repositórios)**
- `HeatingProgramRepositoryTests` - Comportamento de deleção
  - Proteção de programas preset
  - Caracteres únicos

### 📈 Estatísticas dos Testes
- **Total de Testes**: 62+
- **Cobertura**: Camada de negócio (Domain e Application)
- **Frameworks**: xUnit 2.9.3, Moq 4.20.72, FluentAssertions 7.0.0

### 🚀 Como Executar os Testes

#### Via Visual Studio
1. Abra o **Test Explorer** (`Ctrl + E, T`)
2. Clique em "Run All Tests" ou `Ctrl + R, A`
3. Veja os resultados em tempo real

#### Via Command Line
```bash
# Executar todos os testes
dotnet test

# Executar com detalhes
dotnet test --verbosity detailed

# Executar com coverage (requer pacote coverlet)
dotnet test /p:CollectCoverage=true /p:CoverageReportsDirectory=./coverage

# Executar testes específicos
dotnet test --filter "FullyQualifiedName~MicrowaveTests"
dotnet test --filter "FullyQualifiedName~ValidatorTests"
```

#### Executar Testes por Categoria
```bash
# Testes de Domain
dotnet test --filter "FullyQualifiedName~Web.Microondas.Test.Domain"

# Testes de Application
dotnet test --filter "FullyQualifiedName~Web.Microondas.Test.Application"

# Testes de Infrastructure
dotnet test --filter "FullyQualifiedName~Web.Microondas.Test.Infrastructure"
```

### 📝 Exemplo de Saída de Testes
```
Passed!  - Failed:     0, Passed:    62, Skipped:     0, Total:    62, Duration: 1.2s
```

### 🎯 Cenários Testados

**Micro-ondas (Domain)**
- ✅ Início rápido com parâmetros padrão (30s, potência 10)
- ✅ Início manual com tempo e potência customizados
- ✅ Conversão de tempo (segundos → minutos:segundos)
- ✅ Display de aquecimento com caracteres baseados na potência
- ✅ Pausa e retomada de aquecimento
- ✅ Cancelamento em diferentes estados
- ✅ Adição de tempo durante aquecimento (máximo 120s)
- ✅ Bloqueio de adição de tempo em programas preset
- ✅ Conclusão de aquecimento com mensagem

**Validadores**
- ✅ Validação de tempo entre 1-120 segundos
- ✅ Validação de potência entre 1-10
- ✅ Validação de campos obrigatórios
- ✅ Validação de caractere único e não reservado
- ✅ Mensagens de erro descritivas

**Serviços e Handlers**
- ✅ Criação e exclusão de programas customizados
- ✅ Proteção contra exclusão de programas preset
- ✅ Transações com rollback em caso de erro
- ✅ Tratamento de exceções de negócio
- ✅ Integração com repositórios via mocking

**Segurança**
- ✅ Criptografia SHA-256 consistente
- ✅ Hash de 64 caracteres hexadecimais
- ✅ Hash determinístico (mesmo input = mesmo hash)

## 🧪 Como Testar a Aplicação

## Como foi feito um carga inical do banco de dados foi criado o usuário user:adm senha:admin.

### 2️⃣ Login na Aplicação

1. Acesse `https://localhost:7219`
2. Faça login com:
   - **Usuário**: `adm`
   - **Senha**: `admin`

### 3️⃣ Testar Funcionalidades

#### 🔥 Micro-ondas (Aba 1)
1. **Quick Start**: Clique em "Início Rápido" (30s, potência 10)
2. **Aquecimento Manual**:
   - Use o teclado digital para inserir tempo (ex: 0145 = 1:45)
   - Ajuste a potência clicando no botão
   - Clique em "Iniciar"
3. **Programas Pré-definidos**:
   - Selecione um programa (Pipoca, Leite, Carnes, Frango, Feijão)
   - Clique em "Iniciar Programa"
4. **Pausar/Cancelar**: Teste a pausa e cancelamento

#### ⚙️ Programas Customizados (Aba 2)
1. Clique em "Criar Novo Programa"
2. Preencha os campos:
   - Nome, Alimento, Tempo, Potência, Caractere
3. Salve e veja na lista (fonte em itálico)
4. Teste excluir um programa customizado

#### 👥 Gerenciar Usuários (Aba 3)
1. Registre um novo usuário
2. Edite um usuário existente
3. Exclua um usuário
4. Veja a lista atualizar automaticamente

## 📡 Endpoints da API

### Autenticação
- `POST /api/auth/login` - Login (público)
- `GET /api/auth/check` - Verificar autenticação (autenticado)

### Usuários
- `GET /api/user` - Listar todos os usuários
- `GET /api/user/{id}` - Obter usuário por ID
- `POST /api/user` - Criar novo usuário
- `PUT /api/user/{id}` - Atualizar usuário
- `DELETE /api/user/{id}` - Excluir usuário

### Micro-ondas
- `GET /api/microwave/state` - Obter estado atual
- `POST /api/microwave/quickstart` - Início rápido (30s)
- `POST /api/microwave/manualstart?seconds=X&power=Y` - Iniciar manual
- `POST /api/microwave/programstart/{programId}` - Iniciar programa
- `POST /api/microwave/tick` - Processar tick (1 segundo)
- `POST /api/microwave/pause-cancel` - Pausar/Cancelar
- `POST /api/microwave/resume` - Retomar
- `POST /api/microwave/reset` - Resetar

### Programas de Aquecimento
- `GET /api/heatingprograms` - Listar todos os programas
- `GET /api/heatingprograms/{id}` - Obter programa por ID
- `POST /api/heatingprograms` - Criar programa customizado
- `DELETE /api/heatingprograms/{id}` - Excluir programa customizado

> 🔒 **Todos os endpoints** (exceto `/api/auth/login`) **requerem autenticação JWT** via header `Authorization: Bearer {token}`

## 🔐 Segurança Implementada

- ✅ **Autenticação JWT** com Bearer Token
- ✅ **Senhas criptografadas** com SHA-256 (256 bits)
- ✅ **Connection String criptografada** em Base64
- ✅ **Validação de dados** com FluentValidation
- ✅ **Proteção de rotas** com `[Authorize]`
- ✅ **Tratamento global de exceções** com middleware customizado
- ✅ **Logging de erros** em arquivo de texto (`logs/log.txt`)

## 📝 Logs e Exception Handling

Todas as exceções não tratadas são:
1. Capturadas pelo `ExceptionHandlingMiddleware`
2. Logadas em `Web.Microondas.API/logs/log.txt` com:
   - Timestamp
   - Tipo da exceção
   - Mensagem
   - Inner Exception
   - Stack Trace
3. Retornadas ao cliente em formato JSON padronizado

Exemplo de log:
```
============================================
TIMESTAMP: 2024-01-15 14:30:45
EXCEPTION: Web.Microondas.Application.Exceptions.BusinessRuleException
MESSAGE: O tempo deve ser entre 1 e 120 segundos
INNER EXCEPTION: None
STACK TRACE:
   at Web.Microondas.Application.Services...
============================================
```

## ✅ Níveis de Requisitos Atendidos

### ✅ Nível 1 - Básico (100%)
- Interface com tempo e potência
- Teclado digital
- Validações de tempo (1-120s) e potência (1-10)
- Quick start (30s)
- Acréscimo de 30s durante aquecimento
- Animação de aquecimento
- Pausa/Cancelamento

### ✅ Nível 2 - Programas Pré-definidos (100%)
- 5 programas (Pipoca, Leite, Carnes, Frango, Feijão)
- Caracteres únicos
- Não editáveis/excluíveis
- Auto-preenchimento

### ✅ Nível 3 - Programas Customizados (100%)
- CRUD completo
- Validação de caractere único
- Fonte em itálico
- Persistência em SQL Server

### ✅ Nível 4 - Web API e Avançado (100%)
- Web API com todos os métodos de negócio
- Autenticação JWT Bearer Token
- Status de autenticação na UI
- Senha criptografada (SHA-256)
- Connection string criptografada
- Exception customizada (`BusinessRuleException`)
- Middleware de tratamento de exceções
- Logging de erros em arquivo

### ✅ Requisitos Desejáveis e Diferenciais (100%)
- ✅ Princípios SOLID aplicados
- ✅ Design Patterns (Repository, UoW, CQRS, Middleware, Factory)
- ✅ Clean Architecture com separação clara de camadas
- ✅ Código documentado e legível
- ✅ Proteção de acesso a dados e métodos
- ✅ **Testes Unitários completos (62+ testes)** ⭐
  - Testes de entidades do domínio
  - Testes de validadores com FluentValidation
  - Testes de serviços com mocking (Moq)
  - Testes de handlers e repositórios
  - Cobertura da camada de negócio

## 🐛 Troubleshooting

### Erro: "Cannot connect to SQL Server"
- Verifique se o SQL Server está rodando
- Confirme o nome da instância em `appsettings.json`
- Execute `services.msc` e inicie o serviço SQL Server

### Erro: "Unauthorized" após login
- Limpe o cache do navegador
- Verifique se o token está sendo salvo (F12 > Application > Local Storage)
- Refaça o login

### Banco de dados não foi criado
```bash
# Deletar e recriar
dotnet ef database drop --project ../Web.Microondas.Infrastructure --startup-project .
dotnet ef database update --project ../Web.Microondas.Infrastructure --startup-project .
```

### Erro 401 em todas as requisições
- Verifique se fez login corretamente
- Confirme se o `localStorage` tem o token salvo
- Teste primeiro via Swagger para isolar o problema

## 🎯 Recursos Extras Implementados

- 🎵 **Som de "beep"** ao concluir aquecimento
- 📱 **Interface responsiva** para dispositivos móveis
- 🎨 **UI moderna** com animações suaves
- 🔔 **Notificações toast** para feedback do usuário
- ⚡ **Hot reload** durante desenvolvimento
- 📊 **Swagger UI** para documentação da API
- 🔍 **CORS configurado** para desenvolvimento local

## 🗄️ Programas Pré-definidos

O sistema inclui 5 programas de aquecimento pré-configurados:

1. **Pipoca** - 3 min, Potência 7, Caractere: `!`
2. **Leite** - 5 min, Potência 5, Caractere: `#`
3. **Carnes de boi** - 14 min, Potência 4, Caractere: `%`
4. **Frango** - 8 min, Potência 7, Caractere: `$`
5. **Feijão** - 8 min, Potência 9, Caractere: `&`

## 👨‍💻 Autor

Desenvolvido como desafio técnico seguindo os princípios de Clean Architecture, SOLID e boas práticas de desenvolvimento.

---

> This is a challenge by [Coodesh](https://coodesh.com/)
- Exception handling global
- Logging de exceções em arquivo

## 🗄️ Banco de Dados

### Connection String (Descriptografada)
```
Server=localhost\\SQLEXPRESS01;Database=MicroondasDb;Trusted_Connection=True;TrustServerCertificate=True
```

### Programas Pré-definidos
1. **Pipoca** - 3min, Potência 7, Caractere: !
2. **Leite** - 5min, Potência 5, Caractere: #
3. **Carnes de boi** - 14min, Potência 4, Caractere: %
4. **Frango** - 8min, Potência 7, Caractere: $
5. **Feijão** - 8min, Potência 9, Caractere: &

## 📝 Logs
Todas as exceções são logadas em: `Web.Microondas.API/logs/log.txt`

## 🧪 Testes
Para testar exceções, use os endpoints de teste:
- `GET /api/test/test-exception` - Testar exception genérica
- `GET /api/test/test-business-exception` - Testar BusinessRuleException

## 🛠️ Tecnologias Utilizadas
- .NET 10
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server Express
- JWT Authentication
- FluentValidation
- HTML5 / CSS3 / JavaScript ES6+
- Fetch API

## 📂 Estrutura de Pastas
```
C:\dev\Web.Microondas\           ← Solution folder
├── Web.Microondas.API/          # API Controllers, Middleware
├── Web.Microondas.Application/  # Services, DTOs, Use Cases
├── Web.Microondas.Domain/        # Entities, Aggregates
├── Web.Microondas.Infrastructure/# Repository, Database Context
└── Web.Microondas.Web/          # Frontend (HTML/CSS/JS)
    ├── Program.cs
    ├── Web.Microondas.Frontend.csproj
    ├── Properties/
    │   └── launchSettings.json
    └── wwwroot/
        ├── index.html
        ├── css/
        │   └── style.css
        └── js/
            ├── api.js
            ├── auth.js
            ├── microwave.js
            ├── programs.js
            └── app.js
```

## 👨‍💻 Desenvolvimento
Projeto desenvolvido seguindo princípios SOLID, DDD, Clean Architecture e boas práticas de programação orientada a objetos.
