# Feedback - Avaliação Geral

## Front End
### Navegação
  * Pontos positivos:
    - Possui views e rotas definidas no projeto XpertStore.Mvc
    - Implementação com Razor Pages/Views

### Design
    - Será avaliado na entrega final

### Funcionalidade
  * Pontos positivos:
    - Implementação do CRUD para categorias e produtos
    - Interface web implementada com Razor Pages/Views
    - Uso de HTML/CSS para estilização básica
## Back End
### Arquitetura
  * Pontos positivos:
    - Estrutura em camadas bem definida na pasta src:
      * XpertStore.Mvc
      * XpertStore.Api
      * XpertStore.Application
      * XpertStore.Data
      * XpertStore.Entity
    - Separação clara de responsabilidades

  * Pontos negativos:
    - Arquitetura mais complexa que o necessário com 6 camadas
    - Recomendação: Deixar o arsenal técnico para desafios que exigem complexidade
    - Mesmo com tamanha complexidade está injetando o contexto na controller e manipulando queries (não seria um problema se fosse uma arquitetura simples, mas não está fazendo sentido assim)

### Funcionalidade
  * Pontos positivos:
    - Suporte a múltiplos bancos de dados (SQL Server / SQLite)
    - Implementação de autenticação com Identity
    - API RESTful documentada com Swagger
    - Configuração de Seed de dados implementada

### Modelagem
  * Pontos positivos:
    - Uso do Entity Framework Core
    - Modelos de dados na camada XpertStore.Entity

  * Pontos negativos:
    - Modelagem mais complexa que o necessário com várias camadas

## Projeto
### Organização
  * Pontos positivos:
    - Estrutura organizada com pasta src na raiz
    - Arquivo solution (XpertStore.sln) na raiz
    - Separação clara de projetos por responsabilidade
    - Presença de .dockerignore indicando suporte a containerização

### Documentação
  * Pontos positivos:
    - README.md completo e bem estruturado com:
      * Apresentação do projeto
      * Tecnologias utilizadas
      * Estrutura do projeto
      * Instruções de execução
      * Pré-requisitos
    - Documentação da API via Swagger
    - Instruções detalhadas de configuração e execução

  * Pontos negativos:
    - Arquivo FEEDBACK.md não encontrado

### Instalação
  * Pontos positivos:
    - Suporte a múltiplos bancos (SQL Server / SQLite)
    - Configuração de Seed de dados implementada
    - Instruções claras de instalação no README
