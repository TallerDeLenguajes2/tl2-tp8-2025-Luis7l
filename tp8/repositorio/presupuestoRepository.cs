using Microsoft.Data.Sqlite;
using Models;

namespace Repository;


public class PresupuestoRepository
{
    private readonly string connectionString = "Data Source=Db/Tienda_final.db";

    public bool Insertar(Presupuesto obj)
    {
        using var connection = new SqliteConnection(connectionString);

        connection.Open();

        var sqlQuery = @"INSERT INTO Presupuestos (NombreDestinatario, FechaCreacion) 
                             VALUES ($nombreDestinatario, $fechaCreacion)";
        var command = new SqliteCommand(sqlQuery, connection);

        command.Parameters.AddWithValue("$nombreDestinatario", obj.NombreDestinatario);
        command.Parameters.AddWithValue("$fechaCreacion", obj.FechaCreacion);

        return command.ExecuteNonQuery() == 1;


    }

    public List<Presupuesto> Listar()
    {
        // el JOIN de sqlQuery me devuelve el mismo producto con distintos detalles en distintas lineas
        // por eso utilizo el diccionario para reconocer cuando ya se ha traído un producto
        var presupuestos = new Dictionary<int, Presupuesto>();

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            var sqlQuery = @"SELECT p.idPresupuesto, p.NombreDestinatario, p.FechaCreacion, pr.idProducto, pr.Descripcion, pr.Precio, pd.Cantidad 
                             FROM Presupuestos p
                             LEFT JOIN PresupuestosDetalle pd USING (idPresupuesto)
                             LEFT JOIN Productos pr USING (idProducto)";

            using (var sqlCmd = new SqliteCommand(sqlQuery, connection))
            using (var sqlReader = sqlCmd.ExecuteReader())
            {
                while (sqlReader.Read())
                {
                    int idPresupuesto = sqlReader.GetInt32(0);

                    if (!presupuestos.TryGetValue(idPresupuesto, out var presupuesto))
                        presupuesto = generarPresupuesto(sqlReader);

                    if (!sqlReader.IsDBNull(3))
                    {
                        var producto = new Producto(sqlReader.GetInt32(3), sqlReader.GetString(4), sqlReader.GetInt32(5));
                        var detalle = new PresupuestoDetalle(sqlReader.GetInt32(6), producto);

                        presupuesto.Detalle.Add(detalle);
                    }

                    presupuestos.TryAdd(idPresupuesto, presupuesto);
                }
            }

            connection.Close();
        }

        return new List<Presupuesto>(presupuestos.Values);
    }

        public bool Eliminar(int id)
        {
            using var connection = new SqliteConnection(connectionString);
            connection.Open();

            string queryString = "DELETE FROM Presupuestos WHERE idPresupuesto = @id";
            using var command = new SqliteCommand(queryString, connection);

            command.Parameters.AddWithValue("@id", id);

            return command.ExecuteNonQuery() == 1;
        }
    
/*public bool Eliminar(int id)
{
    using var connection = new SqliteConnection(connectionString);
    connection.Open();
    
    // Iniciar una transacción
    using var transaction = connection.BeginTransaction();

    try
    {
        // 1. Eliminar los detalles (hijos) PRIMERO
        var sqlQueryDetalle = @"DELETE FROM PresupuestosDetalle WHERE idPresupuesto=$id";
        using var sqlCmdDetalle = new SqliteCommand(sqlQueryDetalle, connection, transaction);
        sqlCmdDetalle.Parameters.AddWithValue("$id", id); // <-- Parámetro correcto
        sqlCmdDetalle.ExecuteNonQuery();

        // 2. Eliminar el presupuesto (padre) DESPUÉS
        var sqlQuery = @"DELETE FROM Presupuestos WHERE idPresupuesto=$id";
        using var sqlCmd = new SqliteCommand(sqlQuery, connection, transaction);
        sqlCmd.Parameters.AddWithValue("$id", id); // <-- Parámetro correcto
        int rowsAffected = sqlCmd.ExecuteNonQuery();

        // 3. Confirmar cambios
        transaction.Commit();
        
        return rowsAffected == 1;
    }
    catch (Exception)
    {
        // 4. Revertir si algo falla
        transaction.Rollback();
        return false;
    }
}*/
    public bool Modificar(int id, Presupuesto obj)
    {
        using var connection = new SqliteConnection(connectionString);

        connection.Open();

        var sqlQuery = @"UPDATE Presupuestos 
                             SET NombreDestinatario=$nombreDestinatario, FechaCreacion=$fechaCreacion 
                             WHERE idPresupuesto=$id";

        using var command = new SqliteCommand(sqlQuery, connection);

        command.Parameters.AddWithValue("$nombreDestinatario", obj.NombreDestinatario);
        command.Parameters.AddWithValue("$fechaCreacion", obj.FechaCreacion);
        command.Parameters.AddWithValue("$id", obj.IdPresupuesto);
        return command.ExecuteNonQuery() == 1;


    }

    public Presupuesto Obtener(int id)
    {
        Presupuesto presupuestoBuscado = new Presupuesto();

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            var sqlQuery = @"SELECT p.idPresupuesto, p.NombreDestinatario, p.FechaCreacion, pr.idProducto, pr.Descripcion, pr.Precio, pd.Cantidad
                             FROM Presupuestos p 
                             LEFT JOIN PresupuestosDetalle pd USING (idPresupuesto)
                             LEFT JOIN Productos pr USING (idProducto)
                             WHERE idPresupuesto=$id";

            using (var sqlCmd = new SqliteCommand(sqlQuery, connection))
            {
                sqlCmd.Parameters.AddWithValue("$id", id);
                using (var sqlReader = sqlCmd.ExecuteReader())
                {
                    while (sqlReader.Read())
                    {
                        if (presupuestoBuscado.IdPresupuesto == -1)
                            presupuestoBuscado = generarPresupuesto(sqlReader);

                        if (!sqlReader.IsDBNull(3))
                        {
                            var producto = new Producto(sqlReader.GetInt32(3), sqlReader.GetString(4), sqlReader.GetInt32(5));
                            var detalle = new PresupuestoDetalle(sqlReader.GetInt32(6), producto);
                            presupuestoBuscado.Detalle.Add(detalle);
                        }
                    }
                }
            }

            connection.Close();
        }

        return presupuestoBuscado;
    }

    public bool InsertarDetalle(int idPresupuesto, int idProducto, int cantidad)
    {
        using var connection = new SqliteConnection(connectionString);

        connection.Open();

        var query = @"INSERT INTO PresupuestosDetalle (idPresupuesto, idProducto, Cantidad)
                             VALUES ($idPresupuesto, $idProducto, $cantidad)";
        using var command = new SqliteCommand(query, connection);

        command.Parameters.AddWithValue("$idPresupuesto", idPresupuesto);
        command.Parameters.AddWithValue("$idProducto", idProducto);
        command.Parameters.AddWithValue("$cantidad", cantidad);

        return command.ExecuteNonQuery() == 1;



    }

    /// <summary>
    /// Genera un objeto <see cref="Presupuesto"/> partir de los datos 
    /// de un SqliteDataReader generado por alguna consulta
    /// </summary>
    /// <param name="reader">Lector de alguna consulta</param>
    /// <returns>Nueva instancia de <see cref="Presupuesto"/> con los datos
    /// traídos del reader o una instancia por defecto si es que ocurre algun error</returns>
    private Presupuesto generarPresupuesto(SqliteDataReader reader)
    {
        try
        {
            return new Presupuesto(reader.GetInt32(0),
                                   reader.GetString(1),
                                   reader.GetString(2));
        }
        catch (Exception)
        {
            return new Presupuesto();
        }
    }
}