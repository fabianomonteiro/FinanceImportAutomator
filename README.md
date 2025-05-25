# FinanceImportAutomator

Este projeto demonstra a evolução de uma aplicação simples de importação de transações financeiras através de diferentes abordagens arquiteturais. O objetivo principal é ilustrar como um mesmo problema pode ser resolvido com diferentes designs de código, destacando as vantagens e desvantagens de cada um, e como conceitos como Inversão de Controle, princípios SOLID, Clean Code e testabilidade são aplicados em arquiteturas mais robustas.

A aplicação base consiste em ler um arquivo CSV contendo transações financeiras, categorizar essas transações e, em seguida, importá-las para um banco de dados SQL Server.

## Evolução Arquitetural

O projeto está organizado em diferentes pastas, cada uma representando uma etapa da evolução arquitetural:

### 1. `01-Procedural`
Nesta primeira versão, a aplicação é implementada utilizando um estilo de programação procedural. Todo o código reside principalmente no code-behind do formulário de interface do usuário (Windows Forms).

- **Características:**
    - Lógica de negócio, acesso a dados e interface do usuário acoplados.
    - Baixa coesão e alto acoplamento.
    - Dificuldade de manutenção e testes unitários.
    - Rápido desenvolvimento inicial para funcionalidades simples.

### 2. `02-BOLOVO`
A segunda versão introduz uma arquitetura em camadas, separando a aplicação em:
    - **BO (Business Objects):** Camada de lógica de negócio.
    - **LO (Logic Objects / Data Access Layer):** Camada de acesso a dados.
    - **VO (Value Objects):** Objetos para transferência de dados.
    - **UI (User Interface):** Camada de apresentação (Windows Forms).

Apesar da tentativa de separação, esta abordagem ainda pode levar a problemas comuns como o "BOLOVO" (Business Object Logic Over Value Object), onde a lógica de negócio pode vazar para os VOs ou a camada de acesso a dados pode conter regras de negócio.

- **Características:**
    - Tentativa de separação de responsabilidades.
    - Melhor organização do código em comparação com o procedural.
    - Ainda pode apresentar acoplamento entre camadas se não implementada cuidadosamente.
    - Testabilidade ainda pode ser um desafio.

### 3. `03-DDD` (Domain-Driven Design)
Esta versão refatora a aplicação seguindo os princípios do Domain-Driven Design. O foco é no domínio do negócio, com a criação de um modelo rico e expressivo.

- **Camadas Típicas:**
    - **Application:** Orquestra os casos de uso, utilizando os serviços de domínio.
    - **Domain:** Contém as entidades, agregados, value objects, serviços de domínio e interfaces de repositório. É o coração da aplicação.
    - **Infrastructure:** Implementa os detalhes técnicos, como acesso a banco de dados (repositórios), envio de emails, etc.
    - **CrossCutting:** Funcionalidades transversais como logging, injeção de dependência.

- **Características:**
    - Foco no modelo de domínio e na linguagem ubíqua.
    - Alta coesão e baixo acoplamento.
    - Maior testabilidade do domínio.
    - Aplicação de princípios SOLID.
    - Inversão de Dependência (DIP) através de interfaces de repositório no domínio, implementadas na infraestrutura.

### 4. `04-Clean Architecture`
A Clean Architecture, proposta por Robert C. Martin (Uncle Bob), leva os conceitos de separação de preocupações e inversão de dependência a um nível mais rigoroso. As dependências sempre apontam para dentro, em direção ao núcleo do domínio.

- **Círculos Concêntricos:**
    - **Entities:** Regras de negócio mais genéricas e de alto nível.
    - **Use Cases (Interactors):** Lógica de negócio específica da aplicação.
    - **Interface Adapters:** Conversores de dados entre os Use Cases e as camadas mais externas (e.g., Presenters, Controllers, Gateways).
    - **Frameworks & Drivers:** Camada mais externa, contendo UI, banco de dados, frameworks web, etc.

- **Características:**
    - Independência de frameworks, UI, banco de dados.
    - Alta testabilidade do domínio e dos casos de uso.
    - Aplicação rigorosa da Regra de Dependência (dependências apontam para dentro).
    - Código limpo e bem organizado.

### 5. `05-Hexagonal` (Ports & Adapters)
A Arquitetura Hexagonal, ou Ports and Adapters, também visa isolar o núcleo da aplicação (domínio e lógica de aplicação) das preocupações externas.

- **Conceitos Chave:**
    - **Core (Hexágono):** Contém a lógica de negócio pura, sem dependências de tecnologias externas.
    - **Ports:** Interfaces definidas pelo Core que ditam como o Core interage com o mundo exterior (e.g., `ITransactionRepositoryPort`, `IFileReaderPort`). Existem dois tipos de ports:
        - **Driving/Primary Ports:** APIs do Core, chamadas por adaptadores primários (e.g., UI, testes).
        - **Driven/Secondary Ports:** Interfaces que o Core usa para interagir com ferramentas externas (e.g., banco de dados, serviços de terceiros), implementadas por adaptadores secundários.
    - **Adapters:** Implementações concretas dos Ports. Conectam o Core com o mundo exterior.
        - **Primary/Driving Adapters:** Adaptadores que "dirigem" a aplicação (e.g., UI, testes automatizados, controllers de API).
        - **Secondary/Driven Adapters:** Adaptadores que são "dirigidos" pela aplicação (e.g., implementações de acesso a banco de dados, adaptadores de serviços de mensageria).

- **Características:**
    - Isola o domínio de preocupações externas.
    - Alta testabilidade, permitindo substituir adaptadores por mocks/stubs.
    - Flexibilidade para trocar tecnologias externas (e.g., mudar de SQL Server para PostgreSQL) sem impactar o Core.
    - Clara separação entre "o quê" a aplicação faz (Core) e "como" ela faz (Adapters).
    - Forte aplicação da Inversão de Controle e do Princípio da Inversão de Dependência (DIP).

## Conceitos Abordados na Evolução

Ao longo das diferentes arquiteturas, os seguintes conceitos são progressivamente introduzidos e refinados:

- **SOLID Principles:**
    - **S**ingle Responsibility Principle (SRP)
    - **O**pen/Closed Principle (OCP)
    - **L**iskov Substitution Principle (LSP)
    - **I**nterface Segregation Principle (ISP)
    - **D**ependency Inversion Principle (DIP)
- **Clean Code:** Práticas para escrever código legível, compreensível e fácil de manter.
- **Testabilidade:** A capacidade de testar unidades de código de forma isolada e eficaz. Arquiteturas mais desacopladas facilitam a criação de testes unitários e de integração.
- **Inversão de Controle (IoC):** Um princípio onde o controle sobre o fluxo de execução de partes de um programa é invertido. Frequentemente implementado através de Injeção de Dependência.
- **Injeção de Dependência (DI):** Um padrão de design onde as dependências de um objeto são fornecidas (injetadas) por uma entidade externa, em vez de serem criadas pelo próprio objeto.

## Como Executar

1.  Clone o repositório.
2.  Abra a solução `FinanceImportAutomator.sln` no Visual Studio.
3.  Configure o banco de dados SQL Server:
    - Execute o script SQL encontrado em `src/Database/` para criar a tabela `Transactions`.
    - Atualize a string de conexão no arquivo `App.config` de cada projeto que você deseja executar.
4.  Escolha o projeto desejado como projeto de inicialização (e.g., `HexagonalFinanceImportAutomator`) e execute.
5.  Um arquivo CSV de exemplo (`Transactions.csv`) está disponível na pasta `data/` na raiz do projeto.

## Referências

- [BOLOVO - Você conhece esse modelo de arquitetura de software?](https://medium.com/codigorefinado/bolovo-voc%C3%AA-conhece-esse-modelo-de-arquitetura-de-software-1590c778f394)
- [Clean Architecture - FabianoMonteiro](https://github.com/fabianomonteiro/CleanArchitecture)
- [Clean Architecture Manga - Ivan Paulovich](https://github.com/ivanpaulovich/clean-architecture-manga)
- [Aspect Oriented Programming - FabianoMonteiro](https://github.com/fabianomonteiro/AspectOrientedProgramming)
- [FluentInteract - FabianoMonteiro](https://github.com/fabianomonteiro/FluentInteract)

Este projeto serve como um guia prático para entender a transição e os benefícios de arquiteturas de software mais modernas e robustas.
