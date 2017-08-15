using System;
using System.Windows;
using LibreriaClases;
using System.Data.SQLite;
using System.Data;
using System.Globalization;

namespace NostraWPF
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow: Window {
		VAgregarInventario agregarInv = new VAgregarInventario();
		VAgregarCliente agregarCli = new VAgregarCliente();
        recibo agregarRecibo = new recibo();
        DB midb = new DB();

        static string path = ( Environment.GetFolderPath( Environment.SpecialFolder.MyDocuments ) + @"\Nostra_Inv\" );

        public MainWindow() {
			directorio.CrearDirectorio();
			DB.CrearDB();

			InitializeComponent();
            ArticulosDataGrid();
            RecibosDataGrid();
            ClientesDataGrid();

        }

#region Articulos
        public void ArticulosDataGrid() {
            SQLiteConnection conexionDB = new SQLiteConnection( "Data Source=" + path + "\\DBinv.db" );
            conexionDB.Open();
			string consulta = "Select Articulo, Cantidad from Inventario";

			SQLiteCommand cmd = new SQLiteCommand( consulta, conexionDB );
			cmd.ExecuteNonQuery();

			SQLiteDataAdapter dataAdp2 = new SQLiteDataAdapter( cmd );
			DataTable dt2 = new DataTable("Inventario");
			dataAdp2.Fill(dt2);
            dataGrid_Articulos.ItemsSource = dt2.DefaultView;
            dataAdp2.Update( dt2 );

            conexionDB.Close();
		}

        private void buttonAgregarArticulo_Click( object sender, RoutedEventArgs e ) {
            agregarInv.ShowDialog();
            ArticulosDataGrid();
        }

        private void button_ActualizarArticulo_Click( object sender, RoutedEventArgs e ) {
            string articulo = ( (DataRowView)dataGrid_Articulos.SelectedItem ).Row[ "Articulo" ].ToString();


        }

        private void button_EliminarArticulo_Click( object sender, RoutedEventArgs e ) {
            SQLiteConnection conexionDB = new SQLiteConnection( "Data Source=" + path + "\\DBinv.db" );
            conexionDB.Open();

            string str = ( (DataRowView)dataGrid_Articulos.SelectedItem ).Row[ "Articulo" ].ToString();

            if( MessageBox.Show( str + "\nSeguro que desea eliminar este articulo?", "Advertencia", MessageBoxButton.YesNo ) == MessageBoxResult.Yes ) {
                midb.eliminarArticulo(str);
            }

            ArticulosDataGrid();
        }

        private void textBoxBuscarArticulo_KeyUp( object sender, System.Windows.Input.KeyEventArgs e )
        {
            TextInfo ProperCase = new CultureInfo("en-US", false).TextInfo;
            using( SQLiteConnection conn = new SQLiteConnection("Data Source = " + path + "\\DBinv.db") ) {
                conn.Open();
                string query = "Select Articulo, Cantidad from Inventario where Articulo like ('%" + textBoxBuscarArticulo.Text + "%')";

                SQLiteCommand cmd = new SQLiteCommand(query, conn);
                cmd.ExecuteNonQuery();

                SQLiteDataAdapter adap = new SQLiteDataAdapter(cmd);
                DataTable dt = new DataTable();
                adap.Fill(dt);

                dataGrid_Articulos.ItemsSource = dt.DefaultView;

                conn.Close();
            }
        }
        #endregion


        #region Clientes
        public void ClientesDataGrid() {
            SQLiteConnection conexionDB = new SQLiteConnection( "Data Source=" + path + "\\DBinv.db" );
            conexionDB.Open();

            string consulta = "Select Nombre, Apellido from Clientes";

            SQLiteCommand cmd = new SQLiteCommand( consulta, conexionDB );
            cmd.ExecuteNonQuery();

            SQLiteDataAdapter dataAdp = new SQLiteDataAdapter( cmd );
            DataTable dt = new DataTable( "Clientes" );
            dataAdp.Fill( dt );
            dataGrid_Clientes.ItemsSource = dt.DefaultView;
            dataAdp.Update( dt );

            conexionDB.Close();
        }

		private void buttonAgregarCliente_Click(object sender, RoutedEventArgs e) {
            agregarCli.ShowDialog();
            ClientesDataGrid();
		}

        private void button_ActualizarCliente_Click( object sender, RoutedEventArgs e ) {
            string nombre = ( (DataRowView)dataGrid_Clientes.SelectedItem ).Row[ "Nombre" ].ToString();
            string apellido = ( (DataRowView)dataGrid_Clientes.SelectedItem ).Row[ "Apellido" ].ToString();
            
            
        }

        private void button_EliminarCliente_Click( object sender, RoutedEventArgs e ) {
            SQLiteConnection conexionDB = new SQLiteConnection( "Data Source=" + path + "\\DBinv.db" );
            conexionDB.Open();

            string nombre = ( (DataRowView)dataGrid_Clientes.SelectedItem ).Row[ "Nombre" ].ToString();
            string apellido = ( (DataRowView)dataGrid_Clientes.SelectedItem ).Row[ "Apellido" ].ToString();

            if( MessageBox.Show( nombre + " " + apellido + "\nSeguro que desea eliminar este Cliente?", "Advertencia", MessageBoxButton.YesNo ) == MessageBoxResult.Yes ) {
                midb.eliminarCliente( nombre, apellido );
            }

            ClientesDataGrid();
        }

        private void textBoxBuscarCliente_KeyUp( object sender, System.Windows.Input.KeyEventArgs e )
        {
            TextInfo ProperCase = new CultureInfo("en-US", false).TextInfo;
            using( SQLiteConnection conn = new SQLiteConnection("Data Source = " + path + "\\DBinv.db") ) {
                conn.Open();
                string query = "Select Nombre, Apellido, tlfCasa, tlfCelular from Clientes where Nombre like ('%" + textBoxBuscarCliente.Text + "%') OR Apellido like ('%" + textBoxBuscarCliente.Text + "%')";

                SQLiteCommand cmd = new SQLiteCommand(query, conn);
                cmd.ExecuteNonQuery();

                SQLiteDataAdapter adap = new SQLiteDataAdapter(cmd);
                DataTable dt = new DataTable();
                adap.Fill(dt);

                dataGrid_Clientes.ItemsSource = dt.DefaultView;

                conn.Close();
            }
        }

        #endregion

        #region Recibo

        private void buttonAgregarRecibo_Click( object sender, RoutedEventArgs e )
        {
            agregarRecibo.ShowDialog();
        }

        private void RecibosDataGrid() {
            SQLiteConnection conexionDB = new SQLiteConnection("Data Source=" + path + "\\DBinv.db");
            conexionDB.Open();

            string consulta = "Select Marca, Modelo, nombreCliente, apellidoCliente from Recibos";

            SQLiteCommand cmd = new SQLiteCommand(consulta, conexionDB);
            cmd.ExecuteNonQuery();

            SQLiteDataAdapter dataAdp = new SQLiteDataAdapter(cmd);
            DataTable dt = new DataTable("Recibos");
            dataAdp.Fill(dt);
            dataGridRecibos.ItemsSource = dt.DefaultView;
            dataAdp.Update(dt);

            conexionDB.Close();
        }



        #endregion

       
    }
}
