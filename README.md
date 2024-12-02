# BlogSimplesEF
Básico de blog onde os usuários podem visualizar, criar, editar e excluir postagens.

Rodar na memória o banco
--
Para rodar os bancos em memória basta usar options.UseInMemoryDatabase("DBMemory"))

Rodar os bancos em instância Sql Server (localdb)
------
Caso usar for usar mudar o tipo de acesso para 'options.UseSQlServer(connectionString)'

Necessário rodar no console 'Package Manager Console (PMC)' os comandos:
Update-Database -context ApplicationDbContext;
Update-Database -context BlogContext