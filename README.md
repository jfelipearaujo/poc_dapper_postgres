# PoC: Dapper + Postgres

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

When executed, the application creates a container running postegres and when it finishes, it destroys the container.

This example includes:

- Insert one entity
- Insert many entities
- Bulk insert
- Get count
- Get one entity
- Get all entities
- Update
- Delete

Example of bulk insert runtimes:

| Num of items | Total time |
| ------------ | ---------- |
| 1000 | 00:00:00.0745338 |
| 2000 | 00:00:00.0092279 |
| 3000 | 00:00:00.0155983 |
| 4000 | 00:00:00.0155983 |
| 5000 | 00:00:00.0603322 |
| 6000 | 00:00:00.0186088 |
| 7000 | 00:00:00.0177428 |
| 8000 | 00:00:00.0585876 |
| 9000 | 00:00:00.0237423 |
| 10000 | 00:00:00.0381435 |
