Khaale.RD.MSSQLSerializationDemo
================================

Research of performance and space consumption of different serialization methods used tyo store complex data in MS Sql database:
- xml serialization with nvarchar(max) column
- xml serialization with xml column
- json serialization with nvarchar(max) column
- binary serialization with binary(max) column
- protobuf serialization with binary(max) column

Please set your personal connection string in hibernate.cfg.xml->connection.connection_string before running the tests.
