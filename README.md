# ProjetoFinalAPI
## Projeto Final do Módulo Programação Web III - Ada

Olá, professor, tudo bem?

Infelizmente eu não consegui fazer funcionar corretamente o Docker Compose, então estou enviando, junto com o projeto, uma cópia do banco de dados (playersdb.bak), pois fiz o meu projeto com Entity Framework e um banco de dados local no Microsoft SQL Server.

Obigada,
Amanda.

## Instruções para configurar previamente o projeto:

Eu criei um usuário para que fosse possível acessar o banco de dados, visto que eu estava acessando-o pelo Windows Authentication.

Para inserir este usuário, é necessário acessar o arquivo "appsettings.json" e alterar o "USER_FAKE" para "bruno" e o "PASSWORD_FAKE" para "professor".

Ademais, no mesmo arquivo, também é necessário inserir o token do JWT, para isso, altere o "TOKEN_FAKE" para "C5s9Dgudx42REb22k7mWD5Hp3yECus2b".

## Instruções para realizar a autenticação:

Depois de executar o projeto, é necessário realizar a autenticação, para isso é necessário:

1. Realizar o cadastro na opção "/Login/Register" com um usuário e senha de sua escolha;
2. Efetuar o acesso na opção "/Login" com o usuário e senha que foi cadastrado anteriormente;
3. Copiar o token que foi gerado como resposta no "Response body";
4. Clicar em "Authorize", inserir no campo "Value" a palavra bearer, um espaço e o token copiado (Exemplo: bearer token_copiado) e clicar em "Authorize".

Feito isso, o seu acesso aos métodos da API estarão liberados.