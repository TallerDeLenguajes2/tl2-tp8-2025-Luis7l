using Microsoft.Data.Sqlite;
using Models;
namespace Repository;

public class ProductoRepository
{
    private string ConnectionString = "DataSource=db/Tienda_final.db";

    public bool Insertar(Producto producto)
    {
        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();
        string querry = "INSERT INTO Productos (Descripcion,Precio) VALUES (@descripcion,@precio)";
        var command = new SqliteCommand(querry, connection);
        command.Parameters.AddWithValue("@descripcion", producto.Descripcion);
        command.Parameters.AddWithValue("@precio", producto.Precio);

        //comando que ejecuta
       return command.ExecuteNonQuery() == 1;

    }
    public bool Modificar(int id, Producto producto)
    {
        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();
        string querry = "UPDATE Productos SET Descripcion=@descrip,Precio=@precio WHERE IdProducto=@id ";
        var command = new SqliteCommand(querry, connection);
        command.Parameters.AddWithValue("@descrip", producto.Descripcion);
        command.Parameters.AddWithValue("@precio", producto.Precio);
        command.Parameters.AddWithValue("@id", producto.IdProducto);
        
        return command.ExecuteNonQuery() == 1;

    }
    public List<Producto> Listar()
    {
        List<Producto> productos = new List<Producto>();
        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();
        string querry = "SELECT idProducto,Descripcion,Precio FROM Productos";
        var command = new SqliteCommand(querry, connection);
        using var sqlread = command.ExecuteReader();
        while (sqlread.Read())
        {
            productos.Add(Auxiliar(sqlread));
        }
       
        return productos;
    }
    private Producto Auxiliar(SqliteDataReader reader)
    {
        try
        {
            return new Producto(reader.GetInt32(0),
                                reader.GetString(1),
                                reader.GetInt32(2));
        }
        catch (Exception ex)
        {
            System.Console.WriteLine($"Error en la generación de producto: {ex.Message}");
            return new Producto();
        }
    }
    public Producto Obtener(int id)
    {
        Producto producto1=null;
        using var Connection = new SqliteConnection(ConnectionString);
        Connection.Open();
        var querry = "SELECT * FROM Productos WHERE idProducto=@id";
        var command = new SqliteCommand(querry, Connection);
        command.Parameters.AddWithValue("@id", id);
        using var reader = command.ExecuteReader();
        if (reader.Read())
        {
            producto1 =Auxiliar(reader);
            
        }

        return producto1;
        
    } 

    public bool Eliminar (int id)
    {
        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();
        var querry = "DELETE FROM Productos WHERE idProducto=@id";
        var command = new SqliteCommand(querry, connection);
        command.Parameters.AddWithValue("@id", id);
 
        return command.ExecuteNonQuery() == 1;
    }

}