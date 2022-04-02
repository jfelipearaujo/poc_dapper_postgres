# poc_dapper_postgres

This repository is an example of a reusable code using Dapper and Postgres

Installed packages:

 - Dapper (2.0.123)
 - Dapper.Contrib (2.0.78)
 - Dapper.FluentMap (2.0.0)
 - Dapper.FluentMap.Dommel (2.0.0)
 - DotNet.Testcontainers (1.5.0)
 - Microsoft.Extensions.DependencyInjection (3.1.23)
 - Npgsql (6.0.3)
 - PostgreSQLCopyHelper (2.8.0)
 - SqlKata (2.3.7)
 - SqlKata.Execution (2.3.7)

When executed, the application creates a container with postegres running and when it finishes, it destroys the container.

This example includes:

- Insert one entity
- Insert many entities
- Bulk insert
- Get count
- Get one entity
- Get all entities
- Update
- Delete
