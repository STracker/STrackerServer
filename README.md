### Resumo do projecto

O STrackerServer é o servidor e o front web client do sistema STracker. Tem como
objectivo responder a pedidos de aplicações cliente, gerir a informação e
disponibilizar um front web client para interação com os utilizadores via Web.

É disponibilizada uma API pública passível de ser utilizada por qualquer
aplicação cliente utilizando o protocolo HTTP.



### Esquema das directorias

-   STrackerServer:

    -   STrackerServer                                 - Projecto ASP.NET MVC 4.

    -   STrackerServer.BusinessLayer            - Camada de negócios.

    -   STrackerServer.DataAccessLayer         - Camada de acesso aos dados.

    -   STrackerServer.Repository.MongoDB   - Repositórios de ligação à base de
        dados MongoDB.



### URIs da API pública

Em construção...



### Frameworks utilizadas:

1.  Nunit para realização de testes unitários (<http://nunit.net>).

2.  Ninject para injector de dependências (<http://www.ninject.org>).

As frameworks foram instaladas a partir do Nuget no Visual Studio 2012.
