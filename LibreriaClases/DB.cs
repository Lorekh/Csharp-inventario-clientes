using System;
using System.IO;
using System.Data.SQLite;

namespace LibreriaClases {
	public class DB {
		static SQLiteConnection conexionDB;
		static SQLiteCommand cmd;
		static SQLiteDataReader reader;

		static string path = ( Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Nostra_Inv\" );
		static string nombreDB = "DBinv.db";

		public static void CrearDB() {
			if( !System.IO.File.Exists(path + nombreDB) ) {
				SQLiteConnection.CreateFile(path + nombreDB);


				conexionDB = new SQLiteConnection("Data Source=" + path + "\\DBinv.db");
				conexionDB.Open();

				//Crea tabla de inventario
				string CrearTablaInventario = "CREATE TABLE IF NOT EXISTS Inventario " + "(ID INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, Articulo TEXT NOT NULL, Cantidad INTEGER NOT NULL)";
				string CrearTablaClientes = "CREATE TABLE IF NOT EXISTS Clientes " + "(ID INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, Nombre TEXT NOT NULL, Apellido TEXT NOT NULL, Tlfcasa INTEGER, Tlfcelular INTEGER, Correo TEXT)";
                string CrearTablaRecibos = "CREATE TABLE IF NOT EXISTS Recibos " + "(ID INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, nombreCliente TEXT, apellidoCliente TEXT, FechaRecibo DATE NOT NULL, Marca TEXT, Modelo TEXT, Serial TEXT, Descripcion TEXT, FechaEntrega DATE, DescripcionReparacion TEXT, Precio INTEGER, Garantia TEXT, Estado BOOLEAN)";

				cmd = new SQLiteCommand(CrearTablaInventario, conexionDB);
				cmd.ExecuteNonQuery();

				cmd = new SQLiteCommand(CrearTablaClientes, conexionDB);
				cmd.ExecuteNonQuery();

                cmd = new SQLiteCommand( CrearTablaRecibos, conexionDB );
                cmd.ExecuteNonQuery();

                CerrarDB();
			}
		} // Fin crear DB

        public void AbrirDB() {
            conexionDB = new SQLiteConnection( "Data Source=" + path + "\\DBinv.db" );
            conexionDB.Open();
        }

        public static void CerrarDB() {
            conexionDB = new SQLiteConnection( "Data Source=" + path + "\\DBinv.db" );
            conexionDB.Close();
        }

#region Inventario
        public void AgregarInventario(string articulo, int cantidad) {
            conexionDB = new SQLiteConnection( "Data Source=" + path + "\\DBinv.db" );
            conexionDB.Open();

            string insertarSQL = "Insert into Inventario (Articulo, Cantidad) VALUES (@articulo, @cantidad);";
			cmd = new SQLiteCommand(insertarSQL, conexionDB);
            cmd.Parameters.AddWithValue("@articulo", articulo);
            cmd.Parameters.AddWithValue("@cantidad", cantidad);
			cmd.ExecuteNonQuery();

			conexionDB.Close();
		}

		public bool verificarArticulo(string articulo) {
			string verificarSQL = "select Articulo from Inventario where Articulo = @articulo;";

			conexionDB = new SQLiteConnection("Data Source=" + path + "\\DBinv.db");
			conexionDB.Open();

			using( conexionDB ) {
				cmd = new SQLiteCommand(verificarSQL, conexionDB);
                cmd.Parameters.AddWithValue("@articulo", articulo);
                reader = cmd.ExecuteReader();

				if( reader.HasRows ) {
					reader.Close();
					return true;
				} else {
					return false;
				}
			}
		} // Fin Verificar Articulo

		public void actualizarArticulo(string articulo, int cantidad) {
			string actualizarSql = "Update Inventario set Articulo = @articulo and Cantidad = @cantidad;";

            conexionDB = new SQLiteConnection( "Data Source=" + path + "\\DBinv.db" );
            conexionDB.Open();

            cmd = new SQLiteCommand(actualizarSql, conexionDB);
            cmd.Parameters.AddWithValue("@articulo", articulo);
            cmd.Parameters.AddWithValue("@cantidad", cantidad);
			cmd.ExecuteNonQuery();
		}

		public void eliminarArticulo(string articulo) {
            string eliminarSql = "Delete from Inventario where Articulo = @articulo;";

            conexionDB = new SQLiteConnection( "Data Source=" + path + "\\DBinv.db" );
            conexionDB.Open();

            cmd = new SQLiteCommand(eliminarSql, conexionDB);
            cmd.Parameters.AddWithValue("@articulo", articulo);
            cmd.ExecuteNonQuery();
		}

        #endregion


#region Clientes
        public void AgregarCliente(string nombre, string apellido, string tlfcasa, string tlfcelular, string correo) {
			string agregarSql = "Insert into Clientes (Nombre, Apellido, Tlfcasa, Tlfcelular, Correo) VALUES (@nombre, @apellido, @tlfcasa, @tlfcelular, @correo);";

            conexionDB = new SQLiteConnection( "Data Source=" + path + "\\DBinv.db" );
            conexionDB.Open();

            cmd = new SQLiteCommand(agregarSql, conexionDB);
            cmd.Parameters.AddWithValue("@nombre", nombre);
            cmd.Parameters.AddWithValue("@apellido", apellido);
            cmd.Parameters.AddWithValue("@tlfcasa", tlfcasa);
            cmd.Parameters.AddWithValue("@tlfcelular", tlfcelular);
            cmd.Parameters.AddWithValue("@correo", correo);
			cmd.ExecuteNonQuery();
		}

		public bool verificarCliente(string nombre, string apellido) {
			string verificarSql = "select Nombre, Apellido from Clientes where Nombre = @nombre AND Apellido = @apellido;";

            conexionDB = new SQLiteConnection( "Data Source=" + path + "\\DBinv.db" );
            conexionDB.Open();

            using( conexionDB ) {
				cmd = new SQLiteCommand(verificarSql, conexionDB);
                cmd.Parameters.AddWithValue("@nombre", nombre);
                cmd.Parameters.AddWithValue("@apellido", apellido);
				reader = cmd.ExecuteReader();

				if( reader.HasRows ) {
					reader.Close();
					return true;
				} else {
					reader.Close();
					return false;
				}
			}
		} // Fin verificarCliente

		//public void actualizarCliente() { }

		public void eliminarCliente(string nombre, string apellido) {
			string eliminarSql = "Delete from Clientes where Nombre = @nombre AND Apellido = @apellido;";

            conexionDB = new SQLiteConnection( "Data Source=" + path + "\\DBinv.db" );
            conexionDB.Open();

            cmd = new SQLiteCommand(eliminarSql, conexionDB);
            cmd.Parameters.AddWithValue("@nombre", nombre);
            cmd.Parameters.AddWithValue("@apellido", apellido);
            cmd.ExecuteNonQuery();
		}

        public int TotalClientes() {
            int Total = 0;

            return Total;
        }
        #endregion

#region Recibo
        public void agregarRecibo(string nombreCliente, string apellidoCliente, string fecha, string marca , string modelo, string serial, string descripcion ) {
            conexionDB = new SQLiteConnection( "Data Source=" + path + "\\DBinv.db" );
            conexionDB.Open();

            string insertarSQL = @"Insert into Recibos (nombreCliente, apellidoCliente, FechaRecibo, Marca, Modelo, Serial, Descripcion) VALUES (@nombre, @apellido, @fecha, @marca, @modelo, @serial, @descripcion);";
                        
            cmd = new SQLiteCommand( insertarSQL, conexionDB );

            cmd.Parameters.AddWithValue("@nombre", nombreCliente);
            cmd.Parameters.AddWithValue("@apellido", apellidoCliente);
            cmd.Parameters.AddWithValue("@fecha", fecha);
            cmd.Parameters.AddWithValue("@marca", marca);
            cmd.Parameters.AddWithValue("@modelo", modelo);
            cmd.Parameters.AddWithValue("@serial", serial);
            cmd.Parameters.AddWithValue("@descripcion", descripcion);

            cmd.ExecuteNonQuery();

            conexionDB.Close();
        }
#endregion

    }
}
