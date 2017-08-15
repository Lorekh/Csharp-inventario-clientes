using System.Windows;
using LibreriaClases;
using System.Globalization;
using System.Data.SQLite;
using System;
using System.Data;
using System.Collections.Generic;

namespace NostraWPF
{
    /// <summary>
    /// Interaction logic for recibo.xaml
    /// </summary>
    public partial class recibo : Window
    {
        DB miDB = new DB();
        static string path = ( Environment.GetFolderPath( Environment.SpecialFolder.MyDocuments ) + @"\Nostra_Inv\" );
        static SQLiteConnection conexionDB;
        static SQLiteCommand cmd;
        static SQLiteDataReader reader;

        public recibo() {
            InitializeComponent();
            DateTime fechaHoy = DateTime.Now;
            string fecha = fechaHoy.ToString("d");

            labelFecha.Content = fecha ;
        }

        private void buttonCancelar_Click(object sender, RoutedEventArgs e) {
            Hide();
        }

        private void buttonAgregar_Click(object sender, RoutedEventArgs e) {
            TextInfo ProperCase = new CultureInfo( "en-US", false ).TextInfo;
            char[] delimiter = { ' ' };

            string cliente = cbBoxClientes.SelectedValue.ToString();

            string[] separar = cliente.Split(delimiter);

            string nombre = separar[ 0 ];
            string apellido = separar[ 1 ];

            DateTime fechaHoy = DateTime.Now;
            string fecha = fechaHoy.ToString( "d" );

            string marca = textBoxMarca.Text;
            marca = ProperCase.ToTitleCase( marca.ToLower() );
            string modelo = textBoxModelo.Text;
            modelo = ProperCase.ToTitleCase( modelo.ToLower() );
            string serial = textBoxSerial.Text;
            serial = ProperCase.ToTitleCase( serial.ToLower() );

            string descripcion = textBoxDescripcion.Text;

            if (cliente != " ") {
                miDB.agregarRecibo( nombre, apellido, fecha, marca, modelo, serial, descripcion );
                //MessageBox.Show("Recibo agregado para: " + marca + " " + modelo + " " + serial + "\n" + "cliente: " + cliente);
                textBoxMarca.Text = string.Empty;
                textBoxModelo.Text = string.Empty;
                textBoxSerial.Text = string.Empty;
                textBoxDescripcion.Text = string.Empty;
            } else {
                MessageBox.Show("No se pudo agregar el recibo");
            }
            

            
        }

        private void ComboBox_Load(object sender, RoutedEventArgs e) {
            conexionDB = new SQLiteConnection( "Data Source=" + path + "\\DBinv.db" );
            conexionDB.Open();
            string sql = "Select ID, Nombre, Apellido from Clientes";
            string idcliente;

            try {
                cmd = new SQLiteCommand(sql, conexionDB);
                reader = cmd.ExecuteReader();
                List<String> lstNombres = new List<string>();

                while( reader.Read() ) {

                    idcliente = ( reader[ 0 ].ToString() );

                    lstNombres.Add(reader[ 1 ].ToString() + " " + reader[ 2 ].ToString());

                    cbBoxClientes.ItemsSource = lstNombres;
                };
            } catch( Exception ex ) {
                MessageBox.Show("No se puedo cargar los nombres." + ex);
            }

        }

    }
}
